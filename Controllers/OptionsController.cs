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
    }

    public class OptionsResponse
    {
        public long Id { get; set; }
        public double OI { get; set; }
        public string? Time { get; set; }
    }
}
