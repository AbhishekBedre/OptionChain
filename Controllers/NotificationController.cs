using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OptionChain.Extensions;
using OptionChain.Models;

namespace OptionChain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IDbContextFactory<UpStoxDbContext> _upStoxDbContext;

        public NotificationController(IHubContext<NotificationHub> hubContext, IDbContextFactory<UpStoxDbContext> upStoxDbContext)
        {
            _hubContext = hubContext;
            _upStoxDbContext = upStoxDbContext;
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
            // Scan for the breakout stocks
            await ScanForBreakOutDownStock();

            //send notification to clients
            await _hubContext.Clients.All.SendAsync("DataUpdate", notificationType);
        }

        private async Task<bool> ScanForBreakOutDownStock()
        {
            DateTime dt = DateTime.UtcNow.ToIst();

            var dbContext = _upStoxDbContext.CreateDbContext();

            var lastOHLCDataTask = Task.Run(async () =>
            {
                await using var db = _upStoxDbContext.CreateDbContext();

                return await db.OHLCs
                    .AsNoTracking()
                    .Where(x => x.CreatedDate == dt.Date && x.StockMetaDataId > 0)
                    .GroupBy(g => g.StockMetaDataId)
                    .Select(x => x.Max(x => x.Id))
                    .ToListAsync();
            });

            var preComputedRecordTask = Task.Run(async () =>
            {
                await using var db = _upStoxDbContext.CreateDbContext();

                return await db.PreComputedDatas
                    .AsNoTracking()
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();
            });

            var breakOutDownStocksTask = Task.Run(async () =>
            {
                await using var db = _upStoxDbContext.CreateDbContext();

                return await db.BreakOutDownStocks
                .AsNoTracking()
                .Where(x => x.CreatedDate == dt.Date)
                .ToListAsync();
            });

            // Wait to finish all the tasks.
            await Task.WhenAll(lastOHLCDataTask, preComputedRecordTask, breakOutDownStocksTask);

            // Read the result
            var lastOHLCData = lastOHLCDataTask.Result;
            var preComputedRecord = preComputedRecordTask.Result;
            var breakOutDownStocks = breakOutDownStocksTask.Result;

            var stockList = await dbContext.OHLCs
                .AsNoTracking()
                .Where(x => lastOHLCData.Contains(x.Id))
                .ToListAsync();

            var allPrecomputedData = await dbContext.PreComputedDatas
                .AsNoTracking()
                .Where(x => x.CreatedDate == preComputedRecord.CreatedDate)
                .ToListAsync();

            List<BreakOutDownStock> breakOutDownStockList = new List<BreakOutDownStock>();

            foreach (var item in stockList)
            {
                var stockPrecomputedData = allPrecomputedData.Where(x => x.StockMetaDataId == item.StockMetaDataId).FirstOrDefault();

                var stock = CheckBreakOutDownStockAndAddToTable(breakOutDownStocks, item.StockMetaDataId,
                    Convert.ToDecimal(item.LastPrice > 0 ? item.LastPrice : item.Close),
                    Convert.ToDecimal(stockPrecomputedData.DaysHigh),
                    Convert.ToDecimal(stockPrecomputedData.DaysLow),
                    Convert.ToDecimal(item.PChange),
                    item.Time);

                if (stock != null)
                    breakOutDownStockList.Add(stock);
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
            decimal daysHigh,
            decimal daysLow,
            decimal pChange,
            TimeSpan? timeSpan)
        {
            DateTime dt = DateTime.UtcNow.ToIst();

            if (!breakOutDownStocks.Select(x => x.StockMetaDataId).Contains(stockMetaDataId) && (lastPrice > daysHigh || lastPrice < daysLow))
            {
                var stockAlert = new BreakOutDownStock
                {
                    LastPrice = lastPrice,
                    StockMetaDataId = stockMetaDataId,
                    Time = timeSpan,
                    Trend = (lastPrice > daysHigh) ? true : false,
                    PChange = pChange,
                    CreatedDate = dt.Date
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
