using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                OI = s.CEPEOIPrevDiff,
                Time = s.Time?.Hours.ToString()+ ":" + s.Time?.Minutes.ToString()
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
                            join sd in _optionDbContext.StockData.Where(x=>x.EntryDate == Convert.ToDateTime(currentdate).Date && (x.PChange < -0.5 || x.PChange > 0.5))
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
                            join sd in _optionDbContext.StockData.Where(x => x.EntryDate == Convert.ToDateTime(currentdate).Date && (x.PChange < -0.5 || x.PChange > 0.5))
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
