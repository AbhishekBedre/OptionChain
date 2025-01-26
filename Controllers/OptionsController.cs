using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OptionChain.Migrations;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace OptionChain.Controllers
{
    public class Test
    {
        public long ConvertToUnixTimestamp(TimeSpan timeSpan, DateTime entryDate)
        {
            // Get the current DateTime
            DateTime currentDateTime = new DateTime(entryDate.Year, entryDate.Month, entryDate.Day, timeSpan.Hours, timeSpan.Minutes, 0, DateTimeKind.Local).ToUniversalTime();

            long unixTime = ((DateTimeOffset)currentDateTime).ToUniversalTime().ToUnixTimeSeconds();

            return unixTime;

        }
    }

    [ApiController]
    [Route("[controller]")]
    public class OptionsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly OptionDbContext _optionDbContext;
        private readonly IMemoryCache _memoryCache;

        public OptionsController(ILogger<OptionsController> logger, OptionDbContext optionDbContext, IMemoryCache memoryCache)
        {
            _logger = logger;
            _optionDbContext = optionDbContext;
            _memoryCache = memoryCache;
        }

        // Nifty Option Chart
        [HttpGet("trading-view-option")]
        public async Task<AllOptionResponse> GetTradingViewOption(string currentDate)
        {
            Test ob = new Test();

            // Generate time slots from 9:15 AM to 3:30 PM with a 5-minute interval
            var startTime = new TimeSpan(9, 15, 0);
            var endTime = new TimeSpan(15, 30, 0);
            var interval = TimeSpan.FromMinutes(5);

            var timeSlots = Enumerable.Range(0, (int)((endTime - startTime).TotalMinutes / interval.TotalMinutes) + 1)
                .Select(i => startTime.Add(TimeSpan.FromMinutes(i * 5)))
                .ToList();

            var result = await _optionDbContext.Summary.Where(x => x.EntryDate == Convert.ToDateTime(currentDate).Date).ToListAsync();

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


            var result = await _optionDbContext.BankSummary.Where(x => x.EntryDate == Convert.ToDateTime(currentDate).Date).ToListAsync();

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

            if (overall == 1)
            {
                var sectorialIndex = await _optionDbContext.BroderMarkets
                    .Where(x => x.Key == "SECTORAL INDICES" && x.EntryDate == Convert.ToDateTime(currentdate))
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
                var query = from s in _optionDbContext.StockMetaData
                            join sd in _optionDbContext.StockData.Where(x => x.EntryDate == Convert.ToDateTime(currentdate).Date)
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

        [HttpGet("sector-stocks")]
        public async Task<IEnumerable<Sector>> GetSectorStocks(string currentDate = "2025-01-22")
        {
            List<Sector> sectorsStocks = new List<Sector>();

            var sectorWithStockName = await _optionDbContext.Sectors
                .AsQueryable()
                .GroupBy(g => new { g.MappingName, g.Symbol })
                .Select(s => new
                {
                    Name = s.Key.MappingName,
                    Symbol = s.Key.Symbol ?? ""
                })
            .ToListAsync();

            var sectors = sectorWithStockName.GroupBy(g => g.Name).Select(s => s.Key).ToList();

            foreach (var sector in sectors)
            {
                var stockNames = sectorWithStockName.Where(x => x.Name == sector?.ToString()).Select(s => s.Symbol).ToList();

                var result = await _optionDbContext.StockData.AsQueryable()
                                .Where(x => !string.IsNullOrEmpty(x.Symbol)
                                    && x.EntryDate == Convert.ToDateTime(currentDate).Date
                                    && stockNames.Contains(x.Symbol)).ToListAsync();

                Sector mySector = new Sector
                {
                    Id = 0,
                    Name = sector,
                    Stocks = new List<SectorStocksResponse>()
                };

                var tFactorDetail = await _optionDbContext.RFactors
                            .Where(x => !string.IsNullOrEmpty(x.Symbol)
                                && x.EntryDate == Convert.ToDateTime(currentDate)
                                && stockNames.Contains(x.Symbol))
                            .GroupBy(x => x.Symbol) // Group by Symbol
                            .Select(g => g.OrderByDescending(x => x.Id).FirstOrDefault()) // Get the latest entry by Id
                            .ToListAsync();

                foreach (var stock in stockNames)
                {
                    SectorStocksResponse sectorStocksResponse = new SectorStocksResponse();

                    var stockDetail = result.Where(x => x.Symbol == stock).OrderByDescending(x => x.Id).FirstOrDefault();

                    var tFactor = tFactorDetail.Where(x => x.Symbol == stock).OrderByDescending(x => x.Id).FirstOrDefault();

                    if (stockDetail != null)
                    {
                        double tFact = tFactor?.RFactor ?? 0;

                        mySector.Stocks.Add(new SectorStocksResponse
                        {
                            Id = stockDetail.Id,
                            Symbol = stockDetail.Symbol,
                            PChange = stockDetail.PChange,
                            LastPrice = stockDetail.LastPrice,
                            Change = stockDetail.Change,
                            DayHigh = stockDetail.DayHigh,
                            DayLow = stockDetail.DayLow,
                            TFactor = Math.Round(tFact, 2)
                        });
                    }
                }

                var sectorUpdate = _memoryCache.Get<List<SectorsResponse>>("sectorUpdate");

                if (sectorUpdate == null || sectorUpdate.Where(x => x.Sector == sector).FirstOrDefault() == null)
                {
                    mySector.PChange = Math.Round(mySector.Stocks.Average(x => x.PChange) ?? 0, 2);
                }
                else
                {
                    mySector.PChange = Convert.ToDouble(sectorUpdate.Where(x => x.Sector == sector).FirstOrDefault()?.PChange);
                }

                if (mySector.PChange < 0)
                {
                    mySector.Stocks = mySector.Stocks.OrderBy(x => x.PChange).ThenByDescending(x => x.TFactor).ToList();
                }

                if (mySector.PChange >= 0)
                {
                    mySector.Stocks = mySector.Stocks.OrderByDescending(x => x.PChange).ThenByDescending(x => x.TFactor).ToList();
                }

                sectorsStocks.Add(mySector);
            }

            return sectorsStocks;
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

            var topIndex = await _optionDbContext.BroderMarkets.Where(x => x.EntryDate == Convert.ToDateTime(currentDate) && majorIndex.Contains(x.IndexSymbol ?? "")).OrderByDescending(x => x.Id).Take(5).ToListAsync();

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
        public double? Change { get; set; }
        public double? DayHigh { get; set; }
        public double? DayLow { get; set; }
        public double? TFactor { get; set; }
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
}
