using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OptionChain.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using System.Runtime.InteropServices;

namespace OptionChain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly OptionDbContext _optionDbContext;
        private readonly UpStoxDbContext _upStoxDbContext;
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public UsersController(ILogger<UsersController> logger,
            OptionDbContext optionDbContext,
            UpStoxDbContext upStoxDbContext)
        {
            _logger = logger;
            _optionDbContext = optionDbContext;
            _upStoxDbContext = upStoxDbContext;
        }

        [HttpPost]
        public async Task<bool> Post(Users user)
        {
            try
            {
                var usersEntry = await _optionDbContext.Users.AsNoTracking().Where(x => x.Email.ToLower() == user.Email.ToLower()).FirstOrDefaultAsync();

                if (usersEntry == null)
                {
                    await _optionDbContext.Users.AddAsync(user);
                }
                else
                {
                    usersEntry.Name = user.Name;
                    usersEntry.GivenName = user.GivenName;
                    usersEntry.FamilyName = user.FamilyName;
                    usersEntry.ProfileImgeUrl = user.ProfileImgeUrl;
                    usersEntry.LastUpdated = DateTime.Now;
                }

                await _optionDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        [HttpGet("UpdateIndex")]
        public async Task<bool> GetUpdateIndex()
        {
            var metaData = new List<MarketMetaData>();

            string filePath = Path.Combine(AppContext.BaseDirectory, "NSE.json");
            string jsonString = await System.IO.File.ReadAllTextAsync(filePath);

            var config = JsonSerializer.Deserialize<List<StockInfo>>(jsonString);

            var list = config?.Where(x => x.InstrumentType == "INDEX").ToList();

            string[] customIndexList = {
                "NSE_INDEX|Nifty Consumption"
                ,"NSE_INDEX|Nifty Realty"
                ,"NSE_INDEX|Nifty IT"
                ,"NSE_INDEX|Nifty PSE"
                ,"NSE_INDEX|Nifty 50"
                ,"NSE_INDEX|Nifty Next 50"
                ,"NSE_INDEX|Nifty Pvt Bank"
                ,"NSE_INDEX|NIFTY CONSR DURBL"
                ,"NSE_INDEX|NIFTY OIL AND GAS"
                ,"NSE_INDEX|Nifty Ind Defence"
                ,"NSE_INDEX|Nifty Pharma"
                ,"NSE_INDEX|Nifty PSU Bank"
                ,"NSE_INDEX|NIFTY SMLCAP 50"
                ,"NSE_INDEX|Nifty Midcap 50"
                ,"NSE_INDEX|Nifty Media"
                ,"NSE_INDEX|Nifty Auto"
                ,"NSE_INDEX|Nifty Bank"
                ,"NSE_INDEX|Nifty Energy"
                ,"NSE_INDEX|Nifty MidSml Hlth"
                ,"NSE_INDEX|Nifty Ind Tourism"
                ,"NSE_INDEX|Nifty Fin Service"
                ,"NSE_INDEX|Nifty Metal"
                ,"NSE_INDEX|Nifty FMCG"
                ,"NSE_INDEX|NIFTY HEALTHCARE"
                ,"NSE_INDEX|Nifty MS IT Telcm"
                ,"NSE_INDEX|NIFTY MID SELECT"
            };

            list = list?.Where(x => customIndexList.Contains(x.InstrumentKey)).ToList();

            var stockList = _upStoxDbContext.MarketMetaDatas.Where(x => x.Name.Contains("NSE_INDEX")).ToList();

            foreach (var item in list)
            {
                var stock = stockList.Where(x => x.InstrumentToken == item.InstrumentKey).FirstOrDefault();
                if (stock != null)
                {
                    stock.Name = item.InstrumentKey.Replace("|", ":");
                    stock.InstrumentToken = item.InstrumentKey;
                }
                else
                {
                    metaData.Add(new MarketMetaData
                    {
                        Name = item.InstrumentKey.Replace("|", ":"),
                        InstrumentToken = item.InstrumentKey,
                        CreatedDate = DateTime.Now.Date
                    });
                }
            }

            if (metaData.Count > 0)
                _upStoxDbContext.MarketMetaDatas.AddRange(metaData);

            _upStoxDbContext.SaveChanges();

            return true;
        }

        [HttpGet("HistoricalData")]
        public async Task<bool> GetHistoricalData(string fromDate, string toDate)
        {
            var marketMetaData = _upStoxDbContext.MarketMetaDatas.AsNoTracking().ToList();
            var stockNameWithKey = marketMetaData.ToDictionary(x => x.InstrumentToken, x => x.Id);

            foreach (var item in marketMetaData)
            {
                var instrumentKey = item.InstrumentToken;
                stockNameWithKey.TryGetValue(instrumentKey, out var stockMetaDataId);

                GetMarketUpdate(item.InstrumentToken, stockMetaDataId, DateTime.Parse(fromDate), DateTime.Parse(toDate));
            }

            return true;
        }

        private void GetMarketUpdate(string instrumentKeyToFetch, long stockId, DateTime fromDate, DateTime toDate)
        {
            string accessToken = "eyJ0eXAiOiJKV1QiLCJrZXlfaWQiOiJza192MS4wIiwiYWxnIjoiSFMyNTYifQ.eyJzdWIiOiI3MkFBM0siLCJqdGkiOiI2OGZmNDVjYTc2ZmRlMzIxNzU2YjYxMDkiLCJpc011bHRpQ2xpZW50IjpmYWxzZSwiaXNQbHVzUGxhbiI6ZmFsc2UsImlhdCI6MTc2MTU2MDAxMCwiaXNzIjoidWRhcGktZ2F0ZXdheS1zZXJ2aWNlIiwiZXhwIjoxNzYxNjAyNDAwfQ.RG64JgGO3eL583LBULHjzYEaMSlHbz0KqsnTnAFFQZY";

            HttpClient _httpClient = new HttpClient();

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // API endpoint (you can dynamically change symbols if needed), NSE_EQ|INE040A01034,NSE_EQ|INE062A01020
                string url = "https://api.upstox.com/v3/historical-candle/" + instrumentKeyToFetch + "/minutes/1/" + toDate.Date.ToString("yyyy-MM-dd") + "/" + fromDate.Date.ToString("yyyy-MM-dd");

                // Make GET request
                HttpResponseMessage response = _httpClient.GetAsync(url).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    string error = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    throw new Exception($"Upstox API failed ({response.StatusCode}): {error}");
                }
                string jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                CandleResponse apiResponse = JsonSerializer.Deserialize<CandleResponse>(jsonResponse, _jsonOptions) ?? new CandleResponse();

                AddMarketDataEFCore(apiResponse, stockId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMarketUpdate: {ex.Message}");
            }
        }

        private bool AddMarketDataEFCore(CandleResponse apiResponse, long stockMetaDataId)
        {
            var prevOhlcList = new List<OHLC>();

            if (apiResponse.Data == null && apiResponse.Status != "success")
                return false;

            foreach (var item in apiResponse.Data.Candles)
            {
                var timeStamp = DateTime.Parse(item.ElementAt(0).ToString());

                prevOhlcList.Add(new OHLC
                {
                    StockMetaDataId = stockMetaDataId,
                    Timestamp = new DateTimeOffset(timeStamp).ToUnixTimeMilliseconds(),
                    Open = Convert.ToDecimal(item.ElementAt(1).ToString()),
                    High = Convert.ToDecimal(item.ElementAt(2).ToString()),
                    Low = Convert.ToDecimal(item.ElementAt(3).ToString()),
                    Close = Convert.ToDecimal(item.ElementAt(4).ToString()),
                    Volume = long.Parse(item.ElementAt(5).ToString()),
                    CreatedDate = timeStamp.Date,
                    Time = new TimeSpan(timeStamp.Hour, timeStamp.Minute, 0)
                });
            }

            if (prevOhlcList?.Count == 0)
                return false;

            _upStoxDbContext.OHLCs.AddRange(prevOhlcList);
            _upStoxDbContext.SaveChanges();

            return true;
        }

    }

    public class StockInfo
    {
        [JsonPropertyName("segment")]
        public string Segment { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("isin")]
        public string Isin { get; set; }

        [JsonPropertyName("instrument_type")]
        public string InstrumentType { get; set; }

        [JsonPropertyName("instrument_key")]
        public string InstrumentKey { get; set; }

        [JsonPropertyName("lot_size")]
        public int LotSize { get; set; }

        [JsonPropertyName("freeze_quantity")]
        public double FreezeQuantity { get; set; }

        [JsonPropertyName("exchange_token")]
        public string ExchangeToken { get; set; }

        [JsonPropertyName("tick_size")]
        public double TickSize { get; set; }

        [JsonPropertyName("trading_symbol")]
        public string TradingSymbol { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("qty_multiplier")]
        public double QtyMultiplier { get; set; }

        [JsonPropertyName("security_type")]
        public string SecurityType { get; set; }

        [JsonPropertyName("underlying_type")]
        public string UnderlyingType { get; set; }

        [JsonPropertyName("underlying_key")]
        public string UnderlyingKey { get; set; }

        [JsonPropertyName("asset_symbol")]
        public string AssertSymbol { get; set; }

        [JsonPropertyName("asset_type")]
        public string AssetType { get; set; }
    }

    public class CandleResponse
    {
        public string Status { get; set; }
        public CandleData Data { get; set; }
    }

    public class CandleData
    {
        public List<List<object>> Candles { get; set; }
    }

    public class Candle
    {
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public long OpenInterest { get; set; }
    }

}
