using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OptionChain.Extensions;
using OptionChain.Migrations.UpStoxDb;
using OptionChain.Models;

namespace OptionChain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IDbContextFactory<UpStoxDbContext> _upStoxDbContext;
        private readonly DateTime _currentISTDateTime;
        private readonly IMemoryCache _memoryCache;

        public NotificationController(IHubContext<NotificationHub> hubContext, 
            IDbContextFactory<UpStoxDbContext> upStoxDbContext,
            IMemoryCache memoryCache)
        {
            _hubContext = hubContext;
            _upStoxDbContext = upStoxDbContext;
            _memoryCache = memoryCache;
            _currentISTDateTime = DateTime.UtcNow.ToIst().Date;
        }

        [HttpGet("notify-intraday")]
        public async Task NotifyIntraday(NotificationType notificationType)
        {
            //send notification to clients
            await _hubContext.Clients.All.SendAsync("NewStock", notificationType);
        }

        [HttpGet("data-update")]
        public async Task NotifyDataUpdate(NotificationType notificationType)
        {
            // Execute if the current interval is of 5 min like 09:16, 09:21

            int minutes = _currentISTDateTime.TimeOfDay.Minutes;

            if ((minutes - 1) % 5 == 0)
            {
                // Scan for the breakout stocks
                await ScanForBreakOutDownStock();

                //send notification to clients
                await _hubContext.Clients.All.SendAsync("DataUpdate", notificationType);
            }
        }

        private async Task<bool> ScanForBreakOutDownStock()
        {
            var dbContext = _upStoxDbContext.CreateDbContext();
            List<Task> dbOperationTasks = new List<Task>();
            Task<List<FuturePreComputedData>>? narrowCPRStocksTask = null;
            Task<List<PreComputedData>>? preComputedRecordTask = null;

            var lastOHLCDataTask = Task.Run(async () =>
            {
                await using var db = _upStoxDbContext.CreateDbContext();

                return await db.OHLCs
                    .AsNoTracking()
                    .Where(x => x.CreatedDate == _currentISTDateTime && x.StockMetaDataId > 0)
                    .GroupBy(g => g.StockMetaDataId)
                    .Select(x => x.Max(x => x.Id))
                    .ToListAsync();
            });

            dbOperationTasks.Add(lastOHLCDataTask);

            var breakOutDownStocksTask = Task.Run(async () =>
            {
                await using var db = _upStoxDbContext.CreateDbContext();

                return await db.BreakOutDownStocks
                    .AsNoTracking()
                    .Where(x => x.CreatedDate == _currentISTDateTime)
                    .ToListAsync();
            });

            dbOperationTasks.Add(breakOutDownStocksTask);

            var preComputedRecordsAllDay = _memoryCache.Get<List<PreComputedData>>("preComputedRecordsAllDay");

            if (preComputedRecordsAllDay == null)
            {
                // Get CreatedDate of last entry
                preComputedRecordTask = Task.Run(async () =>
                {
                    await using var db = _upStoxDbContext.CreateDbContext();

                    return await db.PreComputedDatas
                        .AsNoTracking()
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                    //.FirstOrDefaultAsync();
                });

                dbOperationTasks.Add(preComputedRecordTask);
            }

            var narrowCPRStocks = _memoryCache.Get<List<FuturePreComputedData>>("narrowCPRStocks");

            if (narrowCPRStocks == null)
            {
                narrowCPRStocksTask = Task.Run(async () =>
                {
                    await using var db = _upStoxDbContext.CreateDbContext();

                    return await db.FuturePreComputedDatas
                        .AsNoTracking()
                        .Where(x => x.ForDate == _currentISTDateTime && (x.TR1 == true || x.TR2 == true))
                        .ToListAsync();
                });

                dbOperationTasks.Add(narrowCPRStocksTask);
            }

            // Wait to finish all the tasks
            await Task.WhenAll(dbOperationTasks);

            // Read the result
            var lastOHLCData = lastOHLCDataTask.Result;
            var breakOutDownStocks = breakOutDownStocksTask.Result;

            if (preComputedRecordTask != null)
            {
                preComputedRecordsAllDay = preComputedRecordTask.Result;

                // Store in cache with expiration policy
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12)); // Cache expires after 12 hours, this is not going to change

                _memoryCache.Set("preComputedRecordsAllDay", preComputedRecordsAllDay, cacheOptions);
            }

            if (narrowCPRStocksTask != null)
            {
                narrowCPRStocks = narrowCPRStocksTask.Result;

                // Store in cache with expiration policy
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12)); // Cache expires after 12 hours, this is not going to change

                _memoryCache.Set("narrowCPRStocks", narrowCPRStocks, cacheOptions);
            }

            var stockList = await dbContext.OHLCs
                .AsNoTracking()
                .Where(x => lastOHLCData.Contains(x.Id))
                .ToListAsync();

            // Read for createdDate column
            var preComputedLastEntryDate = preComputedRecordsAllDay.OrderByDescending(x => x.Id).FirstOrDefault();

            var allPrecomputedDataOfPreviousDay = preComputedRecordsAllDay
                .Where(x => x.CreatedDate == (preComputedLastEntryDate?.CreatedDate ?? _currentISTDateTime))
                .ToList();

            List<BreakOutDownStock> breakOutDownStockList = new List<BreakOutDownStock>();

            foreach (var item in stockList)
            {
                var stockPrecomputedData = allPrecomputedDataOfPreviousDay.Where(x => x.StockMetaDataId == item.StockMetaDataId).FirstOrDefault();

                if (stockPrecomputedData != null)
                {
                    var stock = CheckBreakOutDownStockAndAddToTable(breakOutDownStocks, 
                        item.StockMetaDataId,
                        Convert.ToDecimal(item.LastPrice > 0 ? item.LastPrice : item.Close),
                        stockPrecomputedData,
                        preComputedRecordsAllDay,
                        narrowCPRStocks,
                        Convert.ToDecimal(item.PChange),
                        item.Time);

                    if (stock != null)
                        breakOutDownStockList.Add(stock);
                }
            }

            if (breakOutDownStockList.Count > 0)
            {
                await dbContext.BreakOutDownStocks.AddRangeAsync(breakOutDownStockList);
                await dbContext.SaveChangesAsync();

                await NotifyIntraday(NotificationType.Intraday);

                return true;
            }

            return false;
        }

        private BreakOutDownStock? CheckBreakOutDownStockAndAddToTable(List<BreakOutDownStock> breakOutDownStocks,
            long stockMetaDataId,
            decimal lastPrice,
            PreComputedData stockPreComputedData,
            List<PreComputedData> preComputedRecordsAllDay,
            List<FuturePreComputedData> futurePreComputedDatas, 
            decimal pChange,
            TimeSpan? timeSpan)
        {
            decimal dayHigh = Convert.ToDecimal(stockPreComputedData.PreviousDayHigh);
            decimal dayLow = Convert.ToDecimal(stockPreComputedData.PreviousDayLow);

            decimal daysHigh = Convert.ToDecimal(stockPreComputedData.DaysHigh);
            decimal daysLow = Convert.ToDecimal(stockPreComputedData.DaysLow);

            if (!breakOutDownStocks.Select(x => x.StockMetaDataId).Contains(stockMetaDataId)
                && (lastPrice > dayHigh || lastPrice < dayLow)
                && (lastPrice > daysHigh || lastPrice < daysLow))
            {
                // Find same days high or low of total occurance.
                var totalPreComputedEntries = preComputedRecordsAllDay.Where(x => x.StockMetaDataId == stockMetaDataId).ToList();
                var totalSameHigDays = totalPreComputedEntries.Where(x => x.DaysHigh == dayHigh).Count();
                var totalSameLowDays = totalPreComputedEntries.Where(x => x.DaysLow == dayLow).Count();

                // Check the narrow CPR
                var narrowCPRDetail = futurePreComputedDatas.Where(x => x.StockMetaDataId == stockMetaDataId).FirstOrDefault();

                // Get Direction
                var trend = (lastPrice > dayHigh) ? 1 : (lastPrice < dayLow) ? 2 : 0;

                double strengthCounter = ((trend == 1 ? totalSameHigDays : trend == 2 ? totalSameLowDays : 0) * 0.5) + (narrowCPRDetail?.TR1 == true ? 1 :narrowCPRDetail?.TR2 == true ? 0.5: 0);

                // Prepare alert entry
                var stockAlert = new BreakOutDownStock
                {
                    LastPrice = lastPrice,
                    StockMetaDataId = stockMetaDataId,
                    Time = timeSpan,
                    Trend = trend, // 0 => No Trend, 1 => Up trend, 2 => Down Trend
                    PChange = pChange,
                    CreatedDate = _currentISTDateTime,
                    StrengthCount = strengthCounter
                };

                return stockAlert;
            }

            return null;
        }
    }
}

public enum NotificationType
{
    DataUpdate,
    Intraday,
    IntradayBlast,
    Daily,
    Weely
}
