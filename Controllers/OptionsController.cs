using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OptionChain.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace OptionChain.Controllers
{
    public class Test
    {
        public long ConvertToUnixTimestamp(TimeSpan timeSpan, DateTime entryDate)
        {
            var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            // Get the current DateTime
            DateTime currentDateTime = new DateTime(entryDate.Year, entryDate.Month, entryDate.Day, timeSpan.Hours, timeSpan.Minutes, 0, DateTimeKind.Unspecified);

            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(currentDateTime, istZone);

            long unixTime = new DateTimeOffset(utcDateTime).ToUnixTimeSeconds();

            return unixTime;

        }
    }

    [ApiController]
    [Route("[controller]")]
    public class OptionsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly OptionDbContext _optionDbContext;
        private readonly UpStoxDbContext _upStoxDbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan cacheDuration = TimeSpan.FromHours(10); // Set cache expiration time

        public OptionsController(ILogger<OptionsController> logger,
            OptionDbContext optionDbContext,
            UpStoxDbContext upStoxDbContext,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _optionDbContext = optionDbContext;
            _memoryCache = memoryCache;
            _upStoxDbContext = upStoxDbContext;
        }

        // Nifty Option Chart
        [HttpGet("trading-view-option")]
        public async Task<AllOptionResponse> GetTradingViewOption(string currentDate)
        {
            Test ob = new Test();

            // Generate time slots from 9:15 AM to 3:30 PM with a 5-minute interval
            var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            DateTime currDt = DateTime.Parse(currentDate);

            DateTime istDate = TimeZoneInfo.ConvertTime(
                DateTime.SpecifyKind(currDt, DateTimeKind.Unspecified),
                istZone
            );

            DateTime startTime = istDate.Date.AddHours(9).AddMinutes(15);
            DateTime endTime = istDate.Date.AddHours(15).AddMinutes(30);
            TimeSpan interval = TimeSpan.FromMinutes(1);

            var timeSlots = Enumerable
                .Range(0, (int)((endTime - startTime).TotalMinutes) + 1)
                .Select(i => startTime.AddMinutes(i))
                .ToList();

            //var responseOptionValues = await _optionDbContext.OptionValues.FromSqlRaw(@"EXEC [NiftyOptions] '" + currentDate + "'").ToListAsync();
            var responseOptionValues = await _upStoxDbContext.optionExpirySummaries
                .AsNoTracking()
                .Where(x => x.EntryDate.Value.Date == currDt.Date)
                .ToListAsync();

            var positiveValue = new List<OptionsResponse>();
            var negetiveValue = new List<OptionsResponse>();

            foreach (var item in timeSlots)
            {
                var row = responseOptionValues.FirstOrDefault(x => x.Time.Value.ToString(@"hh\:mm").Trim() == item.ToString(@"HH\:mm").Trim());
                var time = ob.ConvertToUnixTimestamp(item.TimeOfDay, istDate);
                
                positiveValue.Add(new OptionsResponse
                {
                    Time = time,
                    value = row?.CEPEOIDiff > 0 ? row.CEPEOIDiff : null
                });

                negetiveValue.Add(new OptionsResponse
                {
                    Time = time,
                    value = row?.CEPEOIDiff > 0 ? null : (row?.CEPEOIDiff ?? 0)
                });
            }

            AllOptionResponse allOptionResponse = new AllOptionResponse();

            allOptionResponse.PositiveValue = positiveValue;
            allOptionResponse.NegetiveValue = negetiveValue;

            return allOptionResponse;
        }

        [HttpGet("CEPEBank")]
        public async Task<Dictionary<string, double>> CEPEDiff(string currentDate = "2025-01-24")
        {
            var cepeDiff = await _optionDbContext.Summary.AsNoTracking().Where(x => x.EntryDate == Convert.ToDateTime(currentDate)).ToListAsync();

            Dictionary<string, double> response = new Dictionary<string, double>();

            foreach (var item in cepeDiff)
            {
                var curDate = Convert.ToDateTime(currentDate);
                var d = new DateTime(curDate.Year, curDate.Month, curDate.Day, item.Time.Value.Hours, item.Time.Value.Minutes, 0);

                response.TryAdd(d.ToString("hh:mm"), item.CEPEOIDiff);
            }

            return response;
        }

        // Bank Nifty Option Chart
        [HttpGet("Bank")]
        public async Task<AllOptionResponse> GetBank(string currentDate)
        {
            Test ob = new Test();

            // Generate time slots from 9:15 AM to 3:30 PM with a 5-minute interval
            var startTime = new TimeSpan(9, 15, 0);
            var endTime = new TimeSpan(15, 30, 0);
            var interval = TimeSpan.FromMinutes(5);

            var timeSlots = Enumerable.Range(0, (int)((endTime - startTime).TotalMinutes / interval.TotalMinutes) + 1)
                .Select(i => startTime.Add(TimeSpan.FromMinutes(i * 5)))
                .ToList();

            var result = await _optionDbContext.BankSummary.AsNoTracking().Where(x => x.EntryDate == Convert.ToDateTime(currentDate).Date).ToListAsync();

            // Map existing data to a dictionary for quick lookup
            var resultDictionary = result.ToDictionary(
            x => new TimeSpan(x.Time.Value.Hours, x.Time.Value.Minutes, 0), // Truncate milliseconds
            x => new OptionsResponse
            {
                value = x.CEPEOIPrevDiff * -1,
                Time = ob.ConvertToUnixTimestamp(x.Time ?? TimeSpan.Zero, x.EntryDate ?? DateTime.MinValue)
            });

            // Create positive and negative value lists with default values
            var positiveValue = timeSlots.Select(slot =>
            {
                if (resultDictionary.TryGetValue(slot, out var existingData) && existingData.value >= 0)
                {
                    return existingData;
                }
                return new OptionsResponse
                {
                    value = null,
                    Time = ob.ConvertToUnixTimestamp(slot, Convert.ToDateTime(currentDate))
                };
            }).ToList();

            var negetiveValue = timeSlots.Select(slot =>
            {
                if (resultDictionary.TryGetValue(slot, out var existingData) && existingData.value < 0)
                {
                    return existingData;
                }
                return new OptionsResponse
                {
                    value = null,
                    Time = ob.ConvertToUnixTimestamp(slot, Convert.ToDateTime(currentDate))
                };
            }).ToList();

            AllOptionResponse allOptionResponse = new AllOptionResponse();

            allOptionResponse.PositiveValue = positiveValue;
            allOptionResponse.NegetiveValue = negetiveValue;

            return allOptionResponse;
        }

        [HttpGet("sectors")]
        public async Task<List<SectorsResponse>> GetSectorsTrend(string currentdate = "2025-01-24", int overall = 1)
        {
            List<SectorsResponse> sectorsResponses = new List<SectorsResponse>();

            DateTime dt = DateTime.Parse(currentdate);

            if (overall == 1)
            {
                var sectorialIndex = await _optionDbContext.BroderMarkets
                    .AsNoTracking()
                    .Where(x => x.Key == "SECTORAL INDICES" && x.EntryDate.Value.Date == dt.Date)
                    .GroupBy(x => x.IndexSymbol)
                    .Select(group => new
                    {
                        IndexSymbol = group.Key.Replace("NIFTY", ""),
                        Items = group.ToList() // Retrieves all items in the group if needed
                    }).ToListAsync();

                foreach (var item in sectorialIndex)
                {
                    var lastSectorialIndexUpdate = item.Items.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (lastSectorialIndexUpdate != null)
                    {
                        sectorsResponses.Add(new SectorsResponse
                        {
                            Id = lastSectorialIndexUpdate.Id,
                            Sector = item.IndexSymbol.Trim(),
                            PChange = lastSectorialIndexUpdate.PercentChange
                        });
                    }
                }

                // Add Custom Sectors

                /*var sectorNames = sectorialIndex.Select(s => s.IndexSymbol.Trim()).ToList();

                var otherSectors = await _optionDbContext.Sectors.Select(x => x.MappingName).Distinct().ToListAsync();

                var getPending = otherSectors.Where(x => !sectorNames.Contains(x)).ToList();

                // get stockNames from sectors

                foreach (var item in getPending)
                {
                    var sectorStocks = await _optionDbContext.Sectors.Where(x => x.MappingName == item).Select(x => x.Symbol).ToListAsync();

                    var result = await _optionDbContext.StockData.Where(x => x.EntryDate == Convert.ToDateTime(currentdate) && sectorStocks.Contains(x.Symbol)).ToListAsync();

                    Sector mySector = new Sector
                    {
                        Id = 0,
                        Name = item,
                        Stocks = new List<SectorStocksResponse>(),
                    };

                    foreach (var stock in sectorStocks)
                    {
                        SectorStocksResponse sectorStocksResponse = new SectorStocksResponse();

                        var stockDetail = result.Where(x => x.Symbol == stock).OrderByDescending(x => x.Id).FirstOrDefault();

                        mySector.Stocks.Add(new SectorStocksResponse
                        {
                            PChange = stockDetail?.PChange,
                        });
                    }

                    mySector.PChange = mySector.Stocks.Average(x => x.PChange);

                    sectorsResponses.Add(new SectorsResponse
                    {
                        Id = 0,
                        Sector = item,
                        PChange = Math.Round(Convert.ToDecimal(mySector.Stocks.Average(x => x.PChange)), 2)
                    }); ;
                }*/
            }
            else
            {
                var query = from s in _optionDbContext.StockMetaData.AsNoTracking()
                            join sd in _optionDbContext.StockData.AsNoTracking().Where(x => x.EntryDate == Convert.ToDateTime(currentdate).Date)
                            on s.Symbol equals sd.Symbol
                            group sd by s.Industry into g
                            select new
                            {
                                Sector = g.Key,
                                AvgPChange = g.Average(x => x.PChange)
                            }
                            into result
                            orderby result.AvgPChange
                            select result;

                sectorsResponses = query.ToList().Select((s, index) => new SectorsResponse
                {
                    Id = index,
                    Sector = s.Sector,
                    PChange = Convert.ToDecimal(Math.Round(s.AvgPChange, 2))
                }).ToList();
            }

            _memoryCache.Remove("sectorUpdate");

            _memoryCache.Set("sectorUpdate", sectorsResponses);

            return sectorsResponses.OrderByDescending(x => x.PChange).ToList();
        }

        [HttpGet("sectors-v2")]
        public async Task<List<SectorsResponse>> GetSectorsTrendsAsync(string currentDate = "2025-11-21")
        {
            List<SectorsResponse> sectorsResponses = new List<SectorsResponse>();
            DateTime currentDt = DateTime.Parse(currentDate);

            // Get Index Id
            var indexIdsAndName = await _upStoxDbContext.MarketMetaDatas
                .AsNoTracking()
                .Where(x => x.Name.Contains("index"))
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();

            var iDsOfUpdatedSectors = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => x.CreatedDate == currentDt && indexIdsAndName.Select(x => x.Id).Contains(x.StockMetaDataId))
                .GroupBy(g => g.StockMetaDataId)
                .Select(g => g.Max(x => x.Id))
                .ToListAsync();

            var sectorsDetails = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => iDsOfUpdatedSectors.Contains(x.Id))
                .ToListAsync();

            foreach (var item in sectorsDetails)
            {
                var sector = new SectorsResponse
                {
                    Id = item.Id,
                    Sector = indexIdsAndName.FirstOrDefault(x => x.Id == item.StockMetaDataId).Name.Split(":")[1],
                    PChange = item.PChange ?? 0
                };

                sectorsResponses.Add(sector);
            }

            sectorsResponses = sectorsResponses.OrderByDescending(x => x.PChange).ToList();

            return sectorsResponses;
        }

        [HttpGet("sector-stocks")]
        public async Task<IEnumerable<Sector>> GetSectorStocks(string currentDate = "2025-01-22", bool isFNO = true)
        {
            List<Sector> sectorsStocks = new List<Sector>();

            _logger.LogInformation("START - Executing [StockWithSectors] stored procedure.");

            var responseSectorsStocks = await _optionDbContext.ResponseSectorsStocks.FromSqlRaw(@"EXEC [StockWithSectors] '" + currentDate + "'").ToListAsync();

            _logger.LogInformation("END - Executing [StockWithSectors] stored procedure.");

            var sectors = responseSectorsStocks.GroupBy(x => x.SectorName).ToList();

            var sectorUpdate = _memoryCache.Get<List<SectorsResponse>>("sectorUpdate");

            foreach (var sector in sectors)
            {
                var stockNames = sectors.Where(s => s.Key == sector?.Key?.ToString()).FirstOrDefault();

                var result = responseSectorsStocks.Where(x => x.SectorName == sector?.Key?.ToString()).ToList();

                if (isFNO)
                {
                    result = result.Where(x => x.IsFNOSec == isFNO).ToList();
                }

                Sector mySector = new Sector
                {
                    Id = 0,
                    Name = sector.Key ?? "",
                    Stocks = new List<SectorStocksResponse>()
                };

                foreach (var stock in stockNames)
                {
                    SectorStocksResponse sectorStocksResponse = new SectorStocksResponse();

                    var stockDetail = result.Where(x => x.Symbol == stock.Symbol).FirstOrDefault();

                    if (stockDetail != null)
                    {
                        mySector.Stocks.Add(new SectorStocksResponse
                        {
                            Id = stockDetail.Id,
                            Symbol = stockDetail.Symbol,
                            PChange = stockDetail.PChange,
                            LastPrice = stockDetail.LastPrice,
                            Change = stockDetail.Change,
                            DayHigh = stockDetail.DayHigh,
                            DayLow = stockDetail.DayLow,
                            TFactor = Math.Round(Convert.ToDouble(stockDetail.TFactor), 2),
                            Open = stockDetail.Open,
                            Time = stockDetail.Time,
                            IsNifty50 = stockDetail.IsNifty50,
                            IsNifty100 = stockDetail.IsNifty100,
                            IsNifty200 = stockDetail.IsNifty200
                        });
                    }
                }

                if (sectorUpdate == null || sectorUpdate.Where(x => x.Sector == sector.Key).FirstOrDefault() == null)
                {
                    mySector.PChange = Math.Round(mySector.Stocks.Average(x => x.PChange) ?? 0, 2);
                }
                else
                {
                    mySector.PChange = Convert.ToDouble(sectorUpdate.Where(x => x.Sector == sector.Key).FirstOrDefault()?.PChange);
                }

                if (mySector.PChange < 0)
                {
                    mySector.Stocks = mySector.Stocks.OrderByDescending(x => x.TFactor).ToList();
                }

                if (mySector.PChange >= 0)
                {
                    mySector.Stocks = mySector.Stocks.OrderByDescending(x => x.TFactor).ToList();
                }

                sectorsStocks.Add(mySector);
            }

            return sectorsStocks;
        }

        [HttpGet("sector-stocks-v2")]
        public async Task<IEnumerable<Sector>> GetSectorStocksSync(string currentDate = "2025-11-21", bool isFNO = true, int totalSector = 2)
        {
            List<Sector> sectorsStocks = new List<Sector>();

            var marketMetaDatasCache = _memoryCache.Get<List<MarketMetaData>>("marketMetadatasCache");
            if (marketMetaDatasCache == null)
            {
                // Get Index Id
                marketMetaDatasCache = await _upStoxDbContext.MarketMetaDatas
                    .AsNoTracking()
                    .ToListAsync();

                // Store in cache with expiration policy
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12)); // Cache expires after 12 hours, this is not going to change

                _memoryCache.Set("marketMetadatasCache", marketMetaDatasCache, cacheOptions);
            }

            var indexIdsAndName = marketMetaDatasCache
                .Where(x => x.Name.Contains("index", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            var allStockMetaDataIdsCache = await GetSectorStockMetaDatasFromCacheIfAvailableAsync();

            var allIndexUpdatedIds = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => x.CreatedDate == DateTime.Parse(currentDate) && indexIdsAndName.Select(x => x.Id).Contains(x.StockMetaDataId))
                .GroupBy(g => g.StockMetaDataId)
                .Select(x => x.Max(x => x.Id))
                .ToListAsync();

            var allIndexUpdatedEntries = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => allIndexUpdatedIds.Contains(x.Id))
                .OrderByDescending(x => x.PChange) // the most positive sector will top in this list so just get top and bottom according to the totalSector
                .ToListAsync();

            var indexFiltered = allIndexUpdatedEntries.Take(totalSector).ToList();
            var indexFilteredBottom2 = allIndexUpdatedEntries.TakeLast(totalSector).ToList();

            if (totalSector > 0)
            {
                indexFiltered.AddRange(indexFilteredBottom2);
                indexIdsAndName = indexIdsAndName.Where(x => indexFiltered.Select(y => y.StockMetaDataId).Contains(x.Id)).ToList();
            }

            foreach (var item in indexIdsAndName)
            {
                List<SectorStocksResponse> sectorStocks = new List<SectorStocksResponse>();

                var stockMetaDataIds = allStockMetaDataIdsCache
                    .Where(x => x.SectorId == item.Id)
                    .Select(x => x.StockMetaDataId)
                    .ToList();

                var indexUpdatedEntry = allIndexUpdatedEntries
                    .Where(x => x.StockMetaDataId == item.Id)
                    .FirstOrDefault();

                foreach (var stock in stockMetaDataIds)
                {
                    var stockEntry = await _upStoxDbContext.OHLCs
                        .AsNoTracking()
                        .Where(x => x.CreatedDate == DateTime.Parse(currentDate) && x.StockMetaDataId == stock)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefaultAsync();

                    var sectorStock = new SectorStocksResponse
                    {
                        Id = ((int)stockEntry.Id),
                        LastPrice = (double)(stockEntry?.LastPrice > 0 ? stockEntry?.LastPrice : stockEntry.Close),
                        PChange = (double)stockEntry.PChange,
                        TFactor = (double)stockEntry.RFactor,
                        Time = stockEntry.Time.ToString(),
                        Symbol = marketMetaDatasCache.Where(x => x.Id == stock).FirstOrDefault().Name.Split(":")[1].ToString()
                    };

                    sectorStocks.Add(sectorStock);
                }

                var sectorsStock = new Sector
                {
                    Id = (int)item.Id,
                    Name = indexIdsAndName.FirstOrDefault(x => x.Id == item.Id).Name.Split(":")[1],
                    PChange = (double)indexUpdatedEntry.PChange,
                    Stocks = sectorStocks.OrderByDescending(x => x.PChange).ToList()
                };

                sectorsStocks.Add(sectorsStock);
            }

            return sectorsStocks;
        }

        [HttpGet()]

        [HttpGet("advances")]
        public async Task<Advance> GetAdvancesDetails(string currentDate = "2025-02-12")
        {
            var result = await _optionDbContext.Advance.AsNoTracking().Where(x => x.EntryDate == Convert.ToDateTime(currentDate)).OrderByDescending(x => x.Time).FirstOrDefaultAsync();
            return result;
        }

        [HttpGet("advances-v2")]
        public async Task<Advance> GetAdvancesDetailsAsync(string currentDate = "2025-11-21")
        {
            var currentDt = DateTime.Parse(currentDate);

            var marketMetaDatasCache = _memoryCache.Get<List<MarketMetaData>>("marketMetadatasCache");
            if (marketMetaDatasCache == null)
            {
                // Get Index Id
                marketMetaDatasCache = await _upStoxDbContext.MarketMetaDatas
                    .AsNoTracking()
                    .ToListAsync();

                // Store in cache with expiration policy
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12)); // Cache expires after 12 hours, this is not going to change

                _memoryCache.Set("marketMetadatasCache", marketMetaDatasCache, cacheOptions);
            }

            var indexIdsAndName = marketMetaDatasCache
                .Where(x => x.Name.Contains("index", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            var stockIds = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => x.CreatedDate == currentDt && !indexIdsAndName.Select(x => x.Id).Contains(x.StockMetaDataId))
                .GroupBy(x => new { x.CreatedDate, x.StockMetaDataId })
                .Select(g => g.Max(z => z.Id))
                .ToListAsync();

            var stockRecords = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => stockIds.Contains(x.Id))
                .ToListAsync();


            var result = new Advance
            {
                Id = 1,
                EntryDate = currentDt,
                Time = stockRecords.FirstOrDefault().Time,
                Advances = stockRecords.Where(x => x.PChange > 0).Count().ToString(),
                Declines = stockRecords.Where(x => x.PChange < 0).Count().ToString(),
                Unchanged = stockRecords.Where(x => x.PChange == 0).Count().ToString()
            };

            return result;
        }

        /*[HttpGet("nifty-chart")]
        public async Task<IEnumerable<NiftyChart>> GetNiftyChartsAsync(string currentDate = "2025-01-16")
        {
            NiftyChart niftyChart = new NiftyChart();

            var result = await _optionDbContext.BroderMarkets.Where(x => x.EntryDate == Convert.ToDateTime(currentDate)
                         && x.IndexSymbol == "NIFTY 50").OrderBy(x => x.Id).Select(s => new NiftyChart
                         {
                             //Id = s.Id,
                             x = s.Time.HasValue ? s.Time.Value.ToString(@"hh\:mm") : "00:00",
                             y = new List<decimal> { s.Open, s.High, s.Low, s.Last }.ToArray()
                         }).ToListAsync();

            var updatedResult = FillMissingData(result);


            return updatedResult; //.OrderBy(x=>x.Id);            
        }*/

        public static List<NiftyChart> FillMissingData(List<NiftyChart> data, string startTime = "09:15", string endTime = "15:30")
        {
            // Parse start and end times
            TimeSpan start = TimeSpan.Parse(startTime);
            TimeSpan end = TimeSpan.Parse(endTime);

            // Convert input data to a dictionary for fast lookups
            var dataDict = new Dictionary<string, decimal[]>();
            foreach (var item in data)
            {
                dataDict[item.x] = item.y;
            }

            // Initialize the result list
            var result = new List<NiftyChart>();

            // Loop through each minute between start and end times
            for (var time = start; time <= end; time = time.Add(TimeSpan.FromMinutes(5)))
            {
                string timeStr = time.ToString(@"hh\:mm");
                if (dataDict.ContainsKey(timeStr))
                {
                    // If data exists for the time, add it to the result
                    result.Add(new NiftyChart { x = timeStr, y = dataDict[timeStr] });
                }
                else
                {
                    // Otherwise, add a placeholder with an empty decimal array
                    result.Add(new NiftyChart { x = timeStr, y = new decimal[0] });
                }
            }

            return result;
        }

        [HttpGet("major-index")]
        public async Task<List<MajorIndexReponse>> GetMajorIndexAsync(string currentDate)
        {
            List<string> majorIndex = new List<string> { "NIFTY 50", "NIFTY NEXT 50", "NIFTY BANK", "NIFTY FIN SERVICE", "NIFTY MID SELECT" };

            var topIndex = await _optionDbContext.BroderMarkets.AsNoTracking().Where(x => x.EntryDate == Convert.ToDateTime(currentDate) && majorIndex.Contains(x.IndexSymbol ?? "")).OrderByDescending(x => x.Id).Take(5).ToListAsync();

            List<MajorIndexReponse> response = new List<MajorIndexReponse>();

            foreach (var item in topIndex)
            {
                var index = new MajorIndexReponse
                {
                    Name = item.Index,
                    Variation = item.Variation,
                    PChange = item.PercentChange,
                    PChangeLast30Days = item.PerChange30d,
                    LastPrice = item.Last
                };
                response.Add(index);
            }

            return response;
        }

        [HttpGet("major-index-v2")]
        public async Task<List<MajorIndexReponse>> GetMajorIndexV2Async(string currentDate)
        {
            Dictionary<long, string> majorIndexName = new Dictionary<long, string> {
                { 89, "NIFTY 50" },
                { 90,  "NIFTY NEXT 50" },
                { 76,  "NIFTY BANK" },
                { 77, "NIFTY FIN SERVICE" },
                { 78, "NIFTY MID SELECT" }
            };

            var indexIds = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => x.CreatedDate == Convert.ToDateTime(currentDate) && majorIndexName.Keys.Contains(x.StockMetaDataId))
                .GroupBy(x => new { x.CreatedDate, x.StockMetaDataId })
                .Select(g => g.Max(z => z.Id))
                .ToListAsync();

            var topIndex = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => indexIds.Contains(x.Id))
                .ToListAsync();

            List<MajorIndexReponse> response = new List<MajorIndexReponse>();

            foreach (var item in topIndex)
            {
                var index = new MajorIndexReponse
                {
                    Name = majorIndexName[item.StockMetaDataId],
                    Variation = (item.PChange / 100) * (item.LastPrice > 0 ? item.LastPrice : item.Close),
                    PChange = item.PChange,
                    PChangeLast30Days = 0,
                    LastPrice = item.LastPrice > 0 ? item.LastPrice : item.Close
                };

                response.Add(index);
            }

            return response;
        }

        [HttpGet("same-open-low-high")]
        public async Task<List<SectorStocksResponse>> SameOpenLowHighStocks(string currentDate = "2025-01-31")
        {
            try
            {
                string cacheKey = $"SectorStocksOpenHighLow_{currentDate}";

                // Check if result exists in cache
                //if (!_memoryCache.TryGetValue(cacheKey, out List<SectorStocksResponse> sectorStocksResponses))
                //{
                // Fetch from database
                var sectorStocksResponses = await _optionDbContext.SameOpenLowHigh
                    .FromSqlRaw("EXEC [GetOpenLowHighStock] {0}", currentDate)
                    .ToListAsync();

                // Store in cache with expiration policy
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(cacheDuration); // Cache expires after 5 minutes

                _memoryCache.Set(cacheKey, sectorStocksResponses, cacheOptions);
                //}

                return sectorStocksResponses;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("weekly-update")]
        public async Task<List<WeeklySectorUpdate>> WeeklySectorUpdate(string currentDate = "2025-02-14", int overall = 1)
        {
            try
            {
                var weeklySectorUpdate = _memoryCache.Get<List<WeeklySectorUpdate>>("weeklySectorUpdate");

                if (weeklySectorUpdate != null)
                    return weeklySectorUpdate;

                DateTime selectedDate = Convert.ToDateTime(currentDate); // or any specific date
                int diff = (7 + (selectedDate.DayOfWeek - DayOfWeek.Monday)) % 7;
                DateTime mondayDate = selectedDate.AddDays(-diff);

                DateTime fridayDate = mondayDate.AddDays(4);

                List<WeeklySectorUpdate> weeklySectorUpdates = new List<WeeklySectorUpdate>();

                var result = await _optionDbContext.WeeklySectorUpdate
                    .FromSqlRaw("EXEC [WeeklyMarketUpdate]")
                    .ToListAsync();

                if (overall == 1)
                {
                    result = result.Where(x => x.WeekStartDate >= mondayDate.Date && x.WeekEndDate <= fridayDate.Date).ToList();
                }

                var resultGrup = result.GroupBy(x => x.Name).ToList();

                var allKeys = resultGrup.Select(x => x.Key).ToList();

                foreach (var item in allKeys)
                {
                    WeeklySectorUpdate week = new WeeklySectorUpdate();

                    var weekData = result.Where(x => x.Name.ToLower() == item.ToLower()).OrderByDescending(x => x.WeekStartDate).Take(5).ToList();

                    week.Name = item.Replace("NIFTY", "").Trim();

                    if (weekData.Count > 5)
                        week.Week4 = weekData[4].PChange;

                    if (weekData.Count > 3)
                        week.Week3 = weekData[3].PChange;

                    if (weekData.Count > 2)
                        week.Week2 = weekData[2].PChange;

                    if (weekData.Count > 1)
                        week.Week1 = weekData[1].PChange;

                    if (weekData.Count > 0)
                        week.LatestWeek = weekData[0].PChange;

                    weeklySectorUpdates.Add(week);
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(cacheDuration); // Cache expires after 5 minutes

                _memoryCache.Set("weeklySectorUpdate", weeklySectorUpdates, cacheOptions);

                return weeklySectorUpdates;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("sectors-stocks")]
        public async Task<List<SectorStocksResponse>> GetSectorsStocks(string sectorName, string currentDate = "2025-02-07")
        {
            try
            {
                DateTime selectedDate = Convert.ToDateTime(currentDate); // or any specific date
                int diff = (7 + (selectedDate.DayOfWeek - DayOfWeek.Monday)) % 7;
                DateTime mondayDate = selectedDate.AddDays(-diff);

                DateTime fridayDate = mondayDate.AddDays(4);

                var sectorStocksResponses = await _optionDbContext.WeeklyStockUpdates.FromSqlRaw("EXEC [WeeklyStockUpdate] '" + mondayDate.ToString("yyyy-MM-dd") + "', '" + fridayDate.ToString("yyyy-MM-dd") + "' ").ToListAsync();

                sectorStocksResponses.ForEach(x =>
                {
                    x.PChange = Convert.ToDouble(Math.Round(Convert.ToDecimal(x.PChange), 2));
                    x.TFactor = Convert.ToDouble(Math.Round(Convert.ToDecimal(x.TFactor), 2));
                });

                return sectorStocksResponses;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("watchlist-stocks")]
        public async Task<List<SectorStocksResponse>> GetWatchlistStocks()
        {
            try
            {
                var weeklyWatchListResponse = _memoryCache.Get<List<SectorStocksResponse>>("weekly-watch-list-update");

                if (weeklyWatchListResponse != null)
                    return weeklyWatchListResponse;

                List<SectorStocksResponse> finalResult = new();

                DateTime today = DateTime.Today;

                // Find the most recent Monday (start of the current week)
                DateTime currentWeekMonday = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);

                // Get previous week's Monday and Friday
                DateTime previousWeekMonday = currentWeekMonday.AddDays(-7);
                DateTime previousWeekFriday = previousWeekMonday.AddDays(4);

                var result = await _optionDbContext.WeeklyStockUpdates.FromSqlRaw(@"EXEC [WeeklyStockUpdate] '" + previousWeekMonday.ToString("yyyy-MM-dd") + "', '" + previousWeekFriday.ToString("yyyy-MM-dd") + "'").ToListAsync();

                result.ForEach(x =>
                {
                    x.PChange = Convert.ToDouble(Math.Round(Convert.ToDecimal(x.PChange), 2));
                    x.TFactor = Convert.ToDouble(Math.Round(Convert.ToDecimal(x.TFactor), 2));
                });

                var positiveStocks = result.Where(x => x.PChange > 8).OrderByDescending(x => x.PChange).Take(10).ToList();
                var negetiveStocks = result.Where(x => x.PChange < -8).OrderBy(x => x.PChange).Take(10).ToList();

                if (positiveStocks.Any())
                    finalResult.AddRange(positiveStocks);

                if (negetiveStocks.Any())
                    finalResult.AddRange(negetiveStocks);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(cacheDuration); // Cache expires after 5 minutes

                _memoryCache.Set("weekly-watch-list-update", finalResult, cacheOptions);

                return finalResult;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("watchlist-stocks-v2")]
        public async Task<List<SectorStocksResponse>> GetWatchListStocksAsync(string currentDate = "2025-11-24")
        {
            DateTime currentDt = DateTime.Parse(currentDate);
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(12)); // Cache expires after 12 hours, this is not going to change


            var getWatchListStocks = _memoryCache.Get<List<SectorStocksResponse>>(currentDate);
            if (getWatchListStocks != null)
                return getWatchListStocks;

            getWatchListStocks = new List<SectorStocksResponse>();

            var marketMetaDatasCache = await GetMarketMetaDataFromCacheIfAvailable();

            var trendingStocks = await _upStoxDbContext.FuturePreComputedDatas
                .AsNoTracking()
                .Where(x => x.ForDate == currentDt && x.TR1 == true)
                .Select(x => new { x.StockMetaDataId, x.CreatedDate })
                .ToListAsync();

            var previousDayCreatedDate = trendingStocks.Select(x => x.CreatedDate).FirstOrDefault();

            var stockDetailIds = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => x.CreatedDate == previousDayCreatedDate && trendingStocks.Select(x => x.StockMetaDataId).Contains(x.StockMetaDataId))
                .GroupBy(x => new { x.CreatedDate, x.StockMetaDataId })
                .Select(x => x.Max(z => z.Id))
                .ToListAsync();

            var stockDetails = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => stockDetailIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in stockDetails)
            {
                var stock = new SectorStocksResponse
                {
                    Id = item.Id,
                    LastPrice = (double)(item.LastPrice > 0 ? item.LastPrice : item.Close),
                    PChange = (double)item.PChange,
                    Symbol = marketMetaDatasCache.Where(x => x.Id == item.StockMetaDataId).First().Name.Split(":")[1].ToString(),
                    TFactor = (double)item.RFactor,
                    Time = item.Time.ToString()
                };
                getWatchListStocks.Add(stock);
            }

            getWatchListStocks = getWatchListStocks.OrderByDescending(x => x.TFactor).ToList();

            _memoryCache.Set(currentDate, getWatchListStocks, cacheOptions);

            return getWatchListStocks;

        }

        [HttpGet("intra-day-blaster")]
        public async Task<List<IntradayBlast>> GetIntraDayBlaster(string currentDate = "2025-02-28")
        {
            try
            {
                DateTime selectedDate = Convert.ToDateTime(currentDate); // or any specific date

                var intraDayBlaster = await _optionDbContext.IntradayBlasts.AsNoTracking().AsQueryable()
                    .Where(x => x.EntryDate.HasValue && x.EntryDate.Value.Date == selectedDate.Date)
                    .ToListAsync();

                return intraDayBlaster;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("breakout-stocks")]
        public async Task<List<BreakoutStock>> GetBreakoutStocksAsync(string currentDate = "2025-09-19")
        {
            try
            {
                var result = await _optionDbContext.BreakoutStocks
                    .FromSqlRaw("EXEC [GetDayHighBreakouts] '" + currentDate + "'").ToListAsync();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("breakout-stocks-v2")]
        public async Task<List<IntradayBlast>> GetBreakoutStocksV2Async(string currentDate = "2025-12-04")
        {
            try
            {
                var response = _memoryCache.Get<List<IntradayBlast>>("intradayBreakout");

                if (response == null)
                    response = new List<IntradayBlast>();

                DateTime currentDt = DateTime.Parse(currentDate);

                var result = await _upStoxDbContext.OHLCs
                    .AsNoTracking()
                    .Where(x => x.CreatedDate == currentDt && x.Time <= new TimeSpan(12, 0, 0))
                    .ToListAsync();

                var lastEntryDateRecord = await _upStoxDbContext.PreComputedDatas
                    .AsNoTracking()
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                var lastEntryDate = lastEntryDateRecord.CreatedDate;

                var preComputedDatas = await _upStoxDbContext.PreComputedDatas
                    .AsNoTracking()
                    .Where(x => x.CreatedDate == lastEntryDate)
                    .ToListAsync();

                var getCPRFroTrend = await _upStoxDbContext.FuturePreComputedDatas
                    .AsNoTracking()
                    .Where(x => x.ForDate == currentDt)
                    .ToListAsync();

                var totalStocks = result.Select(x => x.StockMetaDataId).Distinct().Where(x => x > 0).ToList();

                foreach (var stock in totalStocks)
                {
                    var stockRecords = result.Where(x => x.StockMetaDataId == stock).ToList();

                    var marketMetaData = await GetMarketMetaDataFromCacheIfAvailable();

                    var firstMax = stockRecords.MaxBy(x => x.High);
                    var secondMax = stockRecords.Where(x => x.Id != firstMax.Id).MaxBy(x => x.High);
                    var thirdMax = stockRecords.Where(x => (x.Id != firstMax.Id && x.Id != secondMax.Id)).MaxBy(x => x.High);

                    List<OHLC> orderByHigh = new List<OHLC>
                    {
                        thirdMax,
                        secondMax,
                        firstMax
                    };

                    var getLatestEntry = orderByHigh.Last();
                    var getCPRFroTrendForStock = getCPRFroTrend.Where(x => x.StockMetaDataId == stock).FirstOrDefault();

                    var preComputedDataForStock = preComputedDatas.Where(x => x.StockMetaDataId == stock).FirstOrDefault();

                    var isHigherHigh = (orderByHigh.ElementAt(0).High < orderByHigh.ElementAt(1).High)
                        && (orderByHigh.ElementAt(1).High < orderByHigh.ElementAt(2).High);


                    var isBullish = orderByHigh.ElementAt(0).High > getCPRFroTrendForStock.PivotPoint
                        && orderByHigh.ElementAt(1).High > getCPRFroTrendForStock.PivotPoint
                        && orderByHigh.ElementAt(2).High > getCPRFroTrendForStock.PivotPoint;

                    if (isBullish && isHigherHigh && preComputedDataForStock != null &&
                        (orderByHigh.ElementAt(0).Time < orderByHigh.ElementAt(1).Time) && (orderByHigh.ElementAt(1).Time < orderByHigh.ElementAt(2).Time)
                        && (orderByHigh.ElementAt(0).Volume > preComputedDataForStock.DaysAverageVolume
                        && orderByHigh.ElementAt(1).Volume > preComputedDataForStock.DaysAverageVolume
                        && orderByHigh.ElementAt(2).Volume > preComputedDataForStock.DaysAverageVolume))
                    {
                        var symbol = marketMetaData.Where(x => x.Id == stock).FirstOrDefault().Name.Split(":")[1].ToString();

                        if (response.Find(x => x.Symbol == symbol) == null)
                        {
                            response.Add(new IntradayBlast
                            {
                                Id = getLatestEntry.Id,
                                PChange = (double)getLatestEntry.PChange,
                                EntryDate = getLatestEntry.CreatedDate,
                                LastPrice = (double)(getLatestEntry.LastPrice ?? getLatestEntry.Close),
                                Time = getLatestEntry.Time,
                                Counter = 1,
                                PrevLastPrice = (double)preComputedDataForStock.PreviousDayClose,
                                Symbol = symbol
                            });
                        }
                    }
                }

                response = response.OrderByDescending(x => x.Time).ToList();

                // cache the reponse so that we get to know what was 

                var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(12)); // Cache expires after 12 hours, this is not going to change

                _memoryCache.Set("intradayBreakout", response, cacheOptions);

                var breakDownStock = await GetBreakdownStocksV2Async(currentDate);

                response.AddRange(breakDownStock);

                return response.OrderByDescending(x=>x.Time).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("breakdown-stocks")]
        public async Task<List<BreakoutStock>> GetBreakdownStocksAsync(string currentDate = "2025-09-19")
        {
            try
            {
                var result = await _optionDbContext.BreakoutStocks
                    .FromSqlRaw("EXEC [GetDayLowBreakdowns] '" + currentDate + "'").ToListAsync();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("breakdown-stocks-v2")]
        public async Task<List<IntradayBlast>> GetBreakdownStocksV2Async(string currentDate = "2025-12-04")
        {
            try
            {
                var response = _memoryCache.Get<List<IntradayBlast>>("intradayBreakdown");

                if (response == null)
                    response = new List<IntradayBlast>();

                DateTime currentDt = DateTime.Parse(currentDate);

                var result = await _upStoxDbContext.OHLCs
                    .AsNoTracking()
                    .Where(x => x.CreatedDate == currentDt && x.Time <= new TimeSpan(12, 0, 0))
                    .ToListAsync();

                var lastEntryDateRecord = await _upStoxDbContext.PreComputedDatas
                    .AsNoTracking()
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                var lastEntryDate = lastEntryDateRecord.CreatedDate;

                var preComputedDatas = await _upStoxDbContext.PreComputedDatas
                    .AsNoTracking()
                    .Where(x => x.CreatedDate == lastEntryDate)
                    .ToListAsync();

                var getCPRFroTrend = await _upStoxDbContext.FuturePreComputedDatas
                    .AsNoTracking()
                    .Where(x => x.ForDate == currentDt)
                    .ToListAsync();

                var totalStocks = result.Select(x => x.StockMetaDataId).Distinct().Where(x => x > 0).ToList();

                foreach (var stock in totalStocks)
                {
                    var stockRecords = result.Where(x => x.StockMetaDataId == stock).ToList();

                    var marketMetaData = await GetMarketMetaDataFromCacheIfAvailable();

                    var firstMin = stockRecords.MinBy(x => x.Low);
                    var secondMin = stockRecords.Where(x => x.Id != firstMin.Id).MinBy(x => x.Low);
                    var thirdMin = stockRecords.Where(x => (x.Id != firstMin.Id && x.Id != secondMin.Id)).MinBy(x => x.Low);

                    List<OHLC> orderByHigh = new List<OHLC>
                    {
                        thirdMin,
                        secondMin,
                        firstMin
                    };

                    var getLatestEntry = orderByHigh.Last();

                    var getCPRFroTrendForStock = getCPRFroTrend.Where(x => x.StockMetaDataId == stock).FirstOrDefault();
                    var preComputedDataForStock = preComputedDatas.Where(x => x.StockMetaDataId == stock).FirstOrDefault();

                    var isLowerLow = (orderByHigh.ElementAt(0).Low > orderByHigh.ElementAt(1).Low)
                        && (orderByHigh.ElementAt(1).Low > orderByHigh.ElementAt(2).Low);

                    var isBearish = orderByHigh.ElementAt(0).Low < (getCPRFroTrendForStock?.PivotPoint ?? 0)
                        && orderByHigh.ElementAt(1).Low < (getCPRFroTrendForStock?.PivotPoint ?? 0)
                        && orderByHigh.ElementAt(2).Low < (getCPRFroTrendForStock?.PivotPoint ?? 0);

                    if (isBearish && isLowerLow && preComputedDataForStock != null &&
                        (orderByHigh.ElementAt(0).Time < orderByHigh.ElementAt(1).Time) && (orderByHigh.ElementAt(1).Time < orderByHigh.ElementAt(2).Time)
                        && (orderByHigh.ElementAt(0).Volume > preComputedDataForStock.DaysAverageVolume
                        && orderByHigh.ElementAt(1).Volume > preComputedDataForStock.DaysAverageVolume
                        && orderByHigh.ElementAt(2).Volume > preComputedDataForStock.DaysAverageVolume))
                    {
                        var symbol = marketMetaData.Where(x => x.Id == stock).FirstOrDefault().Name.Split(":")[1].ToString();

                        if (response.Find(x => x.Symbol == symbol) == null)
                        {
                            response.Add(new IntradayBlast
                            {
                                Id = getLatestEntry.Id,
                                PChange = (double)getLatestEntry.PChange,
                                EntryDate = getLatestEntry.CreatedDate,
                                LastPrice = (double)(getLatestEntry.LastPrice ?? getLatestEntry.Close),
                                Time = getLatestEntry.Time,
                                Counter = 1,
                                PrevLastPrice = (double)preComputedDataForStock.PreviousDayClose,
                                Symbol = symbol
                            });
                        }
                    }
                }

                response = response.OrderByDescending(x => x.Time).ToList();

                // cache the reponse so that we get to know what was 

                var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(12)); // Cache expires after 12 hours, this is not going to change

                _memoryCache.Set("intradayBreakdown", response, cacheOptions);

                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("rfactors")]
        public async Task<List<RFactorTable>> GetRFactorTablesAsync(string stockName, string currentDate = "2025-10-03")
        {
            try
            {
                DateTime dt = DateTime.Parse(currentDate);
                var result = await _optionDbContext.RFactors
                    .AsNoTracking()
                    .Where(x => x.EntryDate.HasValue && x.EntryDate.Value.Date == dt.Date && x.Symbol.ToLower() == stockName.ToLower())
                    .OrderBy(x => x.Id)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("stock-quots")]
        public async Task<string> GetStockQuotsAsync(string currentDate = "2025-12-12")
        {
            DateTime currDt = DateTime.Parse(currentDate);

            var maxStockIds = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => x.CreatedDate == currDt.Date)
                .GroupBy(g => g.StockMetaDataId)
                .Select(x => x.Max(x => x.Id))
                .ToListAsync();

            var latestEntry = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => maxStockIds.Contains(x.Id))
                .ToListAsync();

            StringBuilder sb = new StringBuilder();

            var marketData = await GetMarketMetaDataFromCacheIfAvailable();

            marketData = marketData.Where(x => !x.Name.Contains("INDEX")).ToList();

            foreach (var item in latestEntry)
            {
                string name = marketData.FirstOrDefault(x => x.Id == item.StockMetaDataId)?.Name ?? "";

                if (item.StockMetaDataId > 0 && !string.IsNullOrEmpty(name))
                {
                    sb
                        .Append(" " + name.Split(":")[1].ToString() + " ")
                        .Append(" ")
                        .Append(" ₹" + (item.LastPrice > 0 ? item.LastPrice : item.Close).ToString())
                        .Append(" ")
                        .Append(item.PChange > 0 ? "<span class=\"text-meta-3 format\">" + item.PChange + "%</span>" : "<span class=\"text-red format\">" + item.PChange + "%</span>")
                        .Append(" ");
                }
            }

            return sb.ToString();
        }

        [HttpGet("trending-stocks-v2")]
        public async Task<List<BreakoutStock>> GetTendingStocksAsync(string currentDate="2025-12-19")
        {
            try
            {
                var marketData = await GetMarketMetaDataFromCacheIfAvailable();
                var allStockMetaDataIdsCache = await GetSectorStockMetaDatasFromCacheIfAvailableAsync();

                DateTime dtCurrent = DateTime.Parse(currentDate);

                var maxOHLCIds = await _upStoxDbContext.OHLCs
                    .AsNoTracking()
                    .Where(x => x.CreatedDate == dtCurrent.Date)
                    .GroupBy(g => g.StockMetaDataId)
                    .Select(x => x.Max(x => x.Id))
                    .ToListAsync();

                var rFactorResult = await _upStoxDbContext.OHLCs
                    .AsNoTracking()
                    .Where(x => maxOHLCIds.Contains(x.Id) && x.RFactor > 15)
                    .OrderByDescending(x=>x.RFactor)
                    .Take(20)
                    .ToListAsync();

                var result = rFactorResult.Select(x => new BreakoutStock
                {
                    Id = x.Id,
                    LastPrice = (double)(x.LastPrice ?? 0),
                    EntryDate = x.CreatedDate,
                    PChange = (double)(x.PChange ?? 0),
                    Time = x.Time,
                    RFactor = x.RFactor,
                    SectorName = allStockMetaDataIdsCache.FirstOrDefault(y=>y.StockMetaDataId == x.StockMetaDataId)?.SectorDisplayName.ToString(),
                    Symbol = marketData.FirstOrDefault(y=>y.Id == x.StockMetaDataId)?.Name.Split(":")[1].ToString(),
                }).ToList();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("break-out-down-stocks-v2")]
        public async Task<List<BreakOutDownStockResponse>> GetBreakOutDownStocksAsync(string currentDate="2025-12-24")
        {
            try
            {
                var response = new List<BreakOutDownStockResponse>();

                DateTime currDate = DateTime.Parse(currentDate);

                var result = await _upStoxDbContext.BreakOutDownStocks
                    .AsNoTracking()
                    .Where(x => x.CreatedDate == currDate.Date)
                    .ToListAsync();

                var marketMetaData = await GetMarketMetaDataFromCacheIfAvailable();

                response = result.Select(x => new BreakOutDownStockResponse
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    LastPrice = x.LastPrice,
                    PChange = x.PChange,
                    StockMetaDataId = x.StockMetaDataId,
                    Time = x.Time,
                    Trend = x.Trend,
                    Symbol = marketMetaData?.FirstOrDefault(y=>y.Id== x.StockMetaDataId)?.Name.Split(":")[1].ToString() ?? ""
                }).ToList();

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private async Task<List<MarketMetaData>> GetMarketMetaDataFromCacheIfAvailable()
        {
            // Store in cache with expiration policy
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(12)); // Cache expires after 12 hours, this is not going to change

            var marketMetaDatasCache = _memoryCache.Get<List<MarketMetaData>>("marketMetadatasCache");
            if (marketMetaDatasCache == null)
            {
                // Get Index Id
                marketMetaDatasCache = await _upStoxDbContext.MarketMetaDatas
                    .AsNoTracking()
                    .ToListAsync();

                _memoryCache.Set("marketMetadatasCache", marketMetaDatasCache, cacheOptions);
            }

            return marketMetaDatasCache;
        }

        private async Task<List<SectorStockMetaData>> GetSectorStockMetaDatasFromCacheIfAvailableAsync()
        {
            const string cacheKey = "SectorStockMetaData";

            var allStockMetaDataIdsCache = _memoryCache.Get<List<SectorStockMetaData>>(cacheKey);

            if (allStockMetaDataIdsCache == null)
            {
                // Get All stock metaData Ids
                allStockMetaDataIdsCache = await _upStoxDbContext.SectorStockMetaDatas
                    .AsNoTracking()
                    .ToListAsync();

                // Store in cache with expiration policy
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12)); // Cache expires after 12 hours, this is not going to change

                _memoryCache.Set(cacheKey, allStockMetaDataIdsCache, cacheOptions);
            }

            return allStockMetaDataIdsCache;
        }
    }

    public class WeeklySectorUpdateParse
    {
        public string Name { get; set; }
        public DateTime? WeekStartDate { get; set; }
        public DateTime? WeekEndDate { get; set; }
        public decimal? WeeklyAverage { get; set; }
        public decimal? PChange { get; set; }
    }

    public class WeeklySectorUpdate
    {
        public string Name { get; set; }
        public decimal? LatestWeek { get; set; }
        public decimal? Week1 { get; set; }
        public decimal? Week2 { get; set; }
        public decimal? Week3 { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Week4 { get; set; }
    }

    public class MajorIndexReponse
    {
        public string? Name { get; set; }
        public decimal? Variation { get; set; }
        public decimal? PChange { get; set; }
        public decimal? PChangeLast30Days { get; set; }
        public decimal? LastPrice { get; set; }
    }

    public class NiftyChart
    {
        //public long Id { get; set; }
        public string? x { get; set; }
        public decimal[]? y { get; set; }
    }

    public class Sector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? PChange { get; set; }
        public List<SectorStocksResponse> Stocks { get; set; }
    }

    public class SectorStocksResponse
    {
        public long Id { get; set; }
        public string? Symbol { get; set; }
        public double? PChange { get; set; }
        public double? LastPrice { get; set; }
        public double? Open { get; set; }
        public double? Change { get; set; }
        public double? DayHigh { get; set; }
        public double? DayLow { get; set; }
        public double? TFactor { get; set; }
        public string? Time { get; set; }
        public bool IsNifty50 { get; set; }
        public bool IsNifty100 { get; set; }
        public bool IsNifty200 { get; set; }
    }

    public class OptionValues
    {
        public string? Time { get; set; }
        public double? Value { get; set; }
    }

    public class SectorsResponse
    {
        public long Id { get; set; }
        public string? Sector { get; set; }
        public decimal PChange { get; set; }
    }

    public class OptionsResponse
    {
        public double? value { get; set; }
        public long? Time { get; set; }
    }

    public class AllOptionResponse
    {
        public IEnumerable<OptionsResponse>? PositiveValue { get; set; }
        public IEnumerable<OptionsResponse>? NegetiveValue { get; set; }
    }

    public class ResponseSectorsStocks
    {
        public long Id { get; set; }
        public string? Symbol { get; set; }
        public string? SectorName { get; set; }
        public double? PChange { get; set; }
        public double? LastPrice { get; set; }
        public double? Open { get; set; }
        public double? Change { get; set; }
        public double? DayHigh { get; set; }
        public double? DayLow { get; set; }
        public double? TFactor { get; set; }
        public string? Time { get; set; }
        public bool IsFNOSec { get; set; }
        public bool IsNifty50 { get; set; }
        public bool IsNifty100 { get; set; }
        public bool IsNifty200 { get; set; }
    }

    public class BreakOutDownStockResponse : BreakOutDownStock
    {
        public string Symbol { get; set; }
    }
}
