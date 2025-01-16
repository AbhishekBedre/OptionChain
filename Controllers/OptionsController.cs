using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OptionChain.Migrations;
using System.Linq;

namespace OptionChain.Controllers
{

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

        /// <summary>
        /// Read Options Chart.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<OptionsResponse>> Get(string currentDate)
        {
            var result = await _optionDbContext.Summary.Where(x => x.EntryDate == Convert.ToDateTime(currentDate).Date).ToListAsync();

            var optionsResponse = result.Select(s => new OptionsResponse
            {
                Id = s.Id,
                OI = s.CEPEOIPrevDiff * -1,
                Time = s.Time?.Hours.ToString() + ":" + s.Time?.Minutes.ToString()
            });

            return optionsResponse;
        }

        [HttpGet("Bank")]
        public async Task<IEnumerable<OptionsResponse>> GetBank(string currentDate)
        {
            var result = await _optionDbContext.BankSummary.Where(x => x.EntryDate == Convert.ToDateTime(currentDate).Date).ToListAsync();

            var optionsResponse = result.Select(s => new OptionsResponse
            {
                Id = s.Id,
                OI = s.CEPEOIPrevDiff * -1,
                Time = s.Time?.Hours.ToString() + ":" + s.Time?.Minutes.ToString()
            });

            return optionsResponse;
        }

        [HttpGet("sectors")]
        public async Task<IEnumerable<SectorsResponse>> GetSectorsTrend(string currentdate, int overall = 1)
        {
            IEnumerable<SectorsResponse> sectorsResponses = null;

            if (overall == 1)
            {
                var query = from s in _optionDbContext.Sectors
                            join sd in _optionDbContext.StockData.Where(x => x.EntryDate == Convert.ToDateTime(currentdate).Date && (x.PChange < -0.8 || x.PChange > 0.8))
                            on s.Symbol equals sd.Symbol
                            group sd by s.MappingName into g
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
                    PChange = Math.Round(s.AvgPChange, 2)
                });
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
                    PChange = Math.Round(s.AvgPChange, 2)
                });
            }

            return sectorsResponses;
        }

        [HttpGet("sector-stocks")]
        public async Task<IEnumerable<Sector>> GetSectorStocks(string currentDate = "2025-01-10")
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
        public int Id { get; set; }
        public string? Sector { get; set; }
        public double? PChange { get; set; }
    }

    public class OptionsResponse
    {
        public long Id { get; set; }
        public double OI { get; set; }
        public string? Time { get; set; }
    }
}
