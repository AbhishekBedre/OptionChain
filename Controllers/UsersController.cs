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
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace OptionChain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UpStoxDbContext _upStoxDbContext;
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        private string accessToken = "eyJ0eXAiOiJKV1QiLCJrZXlfaWQiOiJza192MS4wIiwiYWxnIjoiSFMyNTYifQ.eyJzdWIiOiI3MkFBM0siLCJqdGkiOiI2OTNjMjVhMTI3ZWEyZDM5ODdiYmU4Y2QiLCJpc011bHRpQ2xpZW50IjpmYWxzZSwiaXNQbHVzUGxhbiI6ZmFsc2UsImlhdCI6MTc2NTU0OTQ3MywiaXNzIjoidWRhcGktZ2F0ZXdheS1zZXJ2aWNlIiwiZXhwIjoxNzY1NTc2ODAwfQ.hHJ9WstgAlAkeY9aeVQ0_Cfz-93YjFd6fpGRXq9JguY";


        public UsersController(ILogger<UsersController> logger, UpStoxDbContext upStoxDbContext)
        {
            _logger = logger;
            _upStoxDbContext = upStoxDbContext;
        }

        [HttpPost]
        public async Task<IResult> CreateUpdateUserPost(UserMaster user)
        {
            try
            {
                var usersEntry = await _upStoxDbContext.UserMasters
                    .Where(x => x.Email.ToLower() == user.Email.ToLower())
                    .FirstOrDefaultAsync();

                if (usersEntry == null)
                {
                    await _upStoxDbContext.UserMasters.AddAsync(user);
                }
                else
                {
                    usersEntry.Name = user.Name;
                    usersEntry.GivenName = user.GivenName;
                    usersEntry.FamilyName = user.FamilyName;
                    usersEntry.ProfileImgeUrl = user.ProfileImgeUrl;
                    usersEntry.LastUpdated = DateTime.Now;
                }

                await _upStoxDbContext.SaveChangesAsync();

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
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
            var marketMetaData = _upStoxDbContext.MarketMetaDatas
                .AsNoTracking()
                .ToList();

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

            // get the previous close collection off the stocks

            var stockPrecomputedData = _upStoxDbContext.PreComputedDatas
                .AsNoTracking()
                .Where(x => x.StockMetaDataId == stockMetaDataId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            var findLastDate = _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => x.StockMetaDataId == stockMetaDataId && x.Time == new TimeSpan(15, 29, 0))
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            // Previous day close stock
            var stockDetails = _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => x.StockMetaDataId == stockMetaDataId && x.CreatedDate != null
                    && findLastDate != null
                    && findLastDate.CreatedDate != null
                    && x.CreatedDate.Value.Date == findLastDate.CreatedDate.Value.Date
                    && x.Time == new TimeSpan(15, 29, 0))
                .FirstOrDefault();

            apiResponse.Data.Candles.Reverse();

            foreach (var item in apiResponse.Data.Candles)
            {
                var timeStamp = DateTime.Parse(item.ElementAt(0).ToString()); // Time

                var previousClose = stockDetails?.LastPrice ?? stockDetails?.Close ?? 0;

                var closePrice = Convert.ToDecimal(item.ElementAt(4).ToString());

                var pChange = previousClose == 0 ? 0 : (((closePrice - previousClose) * 100) / previousClose);

                var rFactor = (stockPrecomputedData != null && closePrice != 0) ? ((stockPrecomputedData.DaysHigh - stockPrecomputedData.DaysLow) / closePrice ?? 1) * 100 : 0;

                prevOhlcList.Add(new OHLC
                {
                    StockMetaDataId = stockMetaDataId,
                    Timestamp = new DateTimeOffset(timeStamp).ToUnixTimeMilliseconds(),
                    Open = Convert.ToDecimal(item.ElementAt(1).ToString()), // Open
                    High = Convert.ToDecimal(item.ElementAt(2).ToString()), // high
                    Low = Convert.ToDecimal(item.ElementAt(3).ToString()), // low
                    Close = Convert.ToDecimal(item.ElementAt(4).ToString()), // Close
                    Volume = long.Parse(item.ElementAt(5).ToString()), // volumn
                    CreatedDate = timeStamp.Date,
                    Time = new TimeSpan(timeStamp.Hour, timeStamp.Minute, 0), //open interest,
                    LastPrice = 0,
                    PChange = pChange,
                    RFactor = rFactor
                });
            }

            if (prevOhlcList?.Count == 0)
                return false;

            _upStoxDbContext.OHLCs.AddRange(prevOhlcList);
            _upStoxDbContext.SaveChanges();

            return true;
        }

        [HttpGet("TodaysData")]
        public bool GetTodaysData(string date = "2025-12-12")
        {
            var marketMetaData = _upStoxDbContext.MarketMetaDatas.AsNoTracking().ToList();
            var stockNameWithKey = marketMetaData.ToDictionary(x => x.InstrumentToken, x => x.Id);

            foreach (var item in marketMetaData)
            {
                var instrumentKey = item.InstrumentToken;
                stockNameWithKey.TryGetValue(instrumentKey, out var stockMetaDataId);

                GetIntraDayMarketUpdate(item.InstrumentToken, stockMetaDataId);
            }

            return true;
        }

        private bool GetIntraDayMarketUpdate(string instrumentKeyToFetch, long stockId)
        {
            HttpClient _httpClient = new HttpClient();

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // API endpoint (you can dynamically change symbols if needed), NSE_EQ|INE040A01034,NSE_EQ|INE062A01020
                string url = "https://api.upstox.com/v3/historical-candle/intraday/" + instrumentKeyToFetch + "/minutes/1";

                // Make GET request
                HttpResponseMessage response = _httpClient.GetAsync(url).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    string error = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    throw new Exception($"Upstox API failed ({response.StatusCode}): {error}");
                }

                string jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                CandleResponse apiResponse = JsonSerializer.Deserialize<CandleResponse>(jsonResponse, _jsonOptions) ?? new CandleResponse();

                return AddMarketDataEFCore(apiResponse, stockId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMarketUpdate: {ex.Message}");
                return false;
            }
        }

        [HttpGet("UpdatePreComputedValues")]
        public async Task UpdatePreComputedValues()
        {
            const int NO_OF_DAYS = 10;
            var preCompuerDataList = new List<PreComputedData>();

            var firstRow = await _upStoxDbContext.OHLCs
                .AsNoTracking()
                .Where(x => x.StockMetaDataId == 1)
                .Select(x => x.CreatedDate)
                .Distinct()
                .OrderByDescending(x => x)
                .Take(NO_OF_DAYS)
                .ToListAsync();

            var equityStocks = await _upStoxDbContext.MarketMetaDatas
                .AsNoTracking()
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();

            // Take the last date to filter the values
            var startDate = firstRow.Last();
            var previousDate = firstRow.ElementAt(0);

            foreach (var stock in equityStocks)
            {
                var ohlcData = await _upStoxDbContext.OHLCs
                    .AsNoTracking()
                    .Where(x => x.StockMetaDataId == stock.Id && x.CreatedDate >= startDate)
                    .ToListAsync();

                var daysHigh = ohlcData.Max(x => x.High);
                var daysLow = ohlcData.Min(x => x.Low);
                var daysAverageClose = ohlcData.Average(x => x.Close);
                var daysAverageVolume = (long)ohlcData.Average(x => x.Volume);

                var previousOHLCData = ohlcData.Where(x => x.CreatedDate == previousDate).ToList();
                var previousDayHigh = previousOHLCData.Max(x => x.High);
                var previousDayLow = previousOHLCData.Min(x => x.Low);
                var previousDayClose = previousOHLCData.OrderByDescending(x => x.Time).FirstOrDefault()?.LastPrice ?? 0;

                var precomputedValue = new PreComputedData
                {
                    CreatedDate = DateTime.Now.Date,
                    DaysHigh = daysHigh,
                    DaysLow = daysLow,
                    DaysAverageClose = daysAverageClose,
                    DaysAverageVolume = daysAverageVolume,
                    DaysAboveVWAPPercentage = 0,
                    DaysATR = 0,
                    DaysAverageBodySize = 0,
                    DaysGreenPercentage = 0,
                    DaysHighLowRangePercentage = 0,
                    DaysMedianATR = 0,
                    DaysStdDevClose = 0,
                    DaysStdDevVolume = 0,
                    DaysTrendScore = 0,
                    DaysVWAP = 0,
                    StockMetaDataId = stock.Id,
                    PreviousDayHigh = previousDayHigh,
                    PreviousDayClose = previousDayClose,
                    PreviousDayLow = previousDayLow
                };

                preCompuerDataList.Add(precomputedValue);
            }

            await _upStoxDbContext.PreComputedDatas.AddRangeAsync(preCompuerDataList);
            await _upStoxDbContext.SaveChangesAsync();
        }

        [HttpGet("FutureStocks")]
        public async Task<string> FutureStocksAsync()
        {
            try
            {
                var result = await _upStoxDbContext.MarketMetaDatas
                    .AsNoTracking()
                    .ToListAsync();

                string stocks = "";

                foreach (var item in result)
                {
                    stocks += item.Name.Split(":")[1].ToString() +", ";
                }

                return stocks;
            }
            catch (Exception)
            {
                throw;
            }
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
