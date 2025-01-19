using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OptionChain.Migrations;
using System.Collections.Immutable;
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

        public OptionsController(ILogger<OptionsController> logger, OptionDbContext optionDbContext)
        {
            _logger = logger;
            _optionDbContext = optionDbContext;
        }

        [HttpGet("trading-view-option")]
        public async Task<AllOptionResponse> GetTradingViewOption(string currentDate)
        {
            Test ob = new Test();
            var result = await _optionDbContext.Summary.Where(x => x.EntryDate == Convert.ToDateTime(currentDate).Date).ToListAsync();

            var positiveValue = result.Select(s => new OptionsResponse
            {
                value = s.CEPEOIPrevDiff * -1,
                Time = ob.ConvertToUnixTimestamp(s.Time ?? new TimeSpan(), s.EntryDate ?? new DateTime())
            }).ToList();

            var negetiveValue = result.Select(s => new OptionsResponse
            {
                value = s.CEPEOIPrevDiff * -1,
                Time = ob.ConvertToUnixTimestamp(s.Time ?? new TimeSpan(), s.EntryDate ?? new DateTime())
            }).ToList();

            AllOptionResponse allOptionResponse = new AllOptionResponse();
            positiveValue.ToList().ForEach(s =>
            {
                if (s.value < 0)
                {
                    s.value = null;
                }
            });

            negetiveValue.ForEach(s =>
            {
                if (s.value >= 0)
                {
                    s.value = null;
                }
            });

            allOptionResponse.PositiveValue = positiveValue;
            allOptionResponse.NegetiveValue = negetiveValue;

            return allOptionResponse;
        }

        [HttpGet("Bank")]
        public async Task<AllOptionResponse> GetBank(string currentDate)
        {
            Test ob = new Test();
            var result = await _optionDbContext.BankSummary.Where(x => x.EntryDate == Convert.ToDateTime(currentDate).Date).ToListAsync();


            var positiveValue = result.Select(s => new OptionsResponse
            {
                value = s.CEPEOIPrevDiff * -1,
                Time = ob.ConvertToUnixTimestamp(s.Time ?? new TimeSpan(), s.EntryDate ?? new DateTime())
            }).ToList();

            var negetiveValue = result.Select(s => new OptionsResponse
            {
                value = s.CEPEOIPrevDiff * -1,
                Time = ob.ConvertToUnixTimestamp(s.Time ?? new TimeSpan(), s.EntryDate ?? new DateTime())
            }).ToList();

            AllOptionResponse allOptionResponse = new AllOptionResponse();
            positiveValue.ToList().ForEach(s =>
            {
                if (s.value < 0)
                {
                    s.value = null;
                }
            });

            negetiveValue.ForEach(s =>
            {
                if (s.value >= 0)
                {
                    s.value = null;
                }
            });

            allOptionResponse.PositiveValue = positiveValue;
            allOptionResponse.NegetiveValue = negetiveValue;

            return allOptionResponse;
        }

        [HttpGet("sectors")]
        public async Task<List<SectorsResponse>> GetSectorsTrend(string currentdate, int overall = 1)
        {
            List<SectorsResponse> sectorsResponses = new List<SectorsResponse>();

            if (overall == 1)
            {
                var sectorialIndex = await _optionDbContext.BroderMarkets
                    .Where(x => x.Key == "SECTORAL INDICES" && x.EntryDate == Convert.ToDateTime(currentdate))
                    .GroupBy(x => x.IndexSymbol)
                    .Select(group => new
                    {
                        IndexSymbol = group.Key,
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
                            Sector = item.IndexSymbol.Replace("NIFTY",""),
                            PChange = lastSectorialIndexUpdate.PercentChange
                        });
                    }
                }
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

            return sectorsResponses.OrderByDescending(x => x.PChange).ToList();
        }

        [HttpGet("sector-stocks")]
        public async Task<IEnumerable<Sector>> GetSectorStocks(string currentDate = "2025-01-17")
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

                foreach (var stock in stockNames)
                {
                    SectorStocksResponse sectorStocksResponse = new SectorStocksResponse();

                    var stockDetail = result.Where(x => x.Symbol == stock).OrderByDescending(x => x.Id).FirstOrDefault();

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
                        });
                    }
                }

                mySector.PChange = Math.Round(mySector.Stocks.Average(x => x.PChange) ?? 0, 2);

                if (mySector.PChange < 0)
                {
                    mySector.Stocks = mySector.Stocks.OrderBy(x => x.PChange).ToList();
                }

                if (mySector.PChange >= 0)
                {
                    mySector.Stocks = mySector.Stocks.OrderByDescending(x => x.PChange).ToList();
                }

                sectorsStocks.Add(mySector);
            }

            return sectorsStocks;
        }

        [HttpGet("nifty-chart")]
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
        }

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

            var topIndex = await _optionDbContext.BroderMarkets.Where(x => x.EntryDate == Convert.ToDateTime(currentDate) && majorIndex.Contains(x.IndexSymbol ?? "")).OrderByDescending(x=>x.Id).Take(5).ToListAsync();

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
