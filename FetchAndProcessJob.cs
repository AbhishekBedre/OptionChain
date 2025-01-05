using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;

namespace OptionChain
{
    public class FetchAndProcessJob : IJob
    {
        private readonly ILogger<FetchAndProcessJob> _logger;
        private readonly OptionDbContext _optionDbContext;
        private readonly IConfiguration _configuration;
        private object counter = 0;
        private object stockCounter = 0;
        private double? previousCPEOIDiffValue = null; // To store the previous X value
        private double? previousCPEColDiffValue = null; // To store the previous X value

        public FetchAndProcessJob(ILogger<FetchAndProcessJob> log, OptionDbContext optionDbContext, IConfiguration configuration)
        {
            _logger = log;
            _optionDbContext = optionDbContext;
            _configuration = configuration;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Started");

            /*var optionsTask = GetNiftyOptions(context);
           
            var stockTask = GetStockData(context);
            
            await Task.WhenAll(optionsTask, stockTask);
            */

            await GetNiftyOptions(context);
            
            await GetStockData(context);

            await Task.CompletedTask;
        }

        public async Task GetStockData(IJobExecutionContext context)
        {

        STEP:

            try
            {
                (bool status, object result, StockRoot StockRoot) = await GetStockData(stockCounter, context);

                if (status == false && Convert.ToInt16(result) <= 5)
                {
                    await Task.Delay(5000);
                    stockCounter = result;

                    goto STEP;
                }

                if (Convert.ToInt32(stockCounter) <= 5)
                {
                    stockCounter = 0;
                    // Make a Db Call
                    await StoreStockDataInTable(StockRoot, context);
                }
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"Multiple tried for stock data but not succeed. counter: {stockCounter}");
                stockCounter = 0;

                Utility.LogDetails(ex.Message);
            }
        }

        public async Task GetNiftyOptions(IJobExecutionContext context)
        {
        STEP:

            try
            {
                (bool status, object result, Root? optionData) = await GetNiftyOptionData(counter, context);

                if (status == false && Convert.ToInt16(result) <= 5)
                {
                    await Task.Delay(2000);
                    counter = result;

                    goto STEP;

                }

                if (Convert.ToInt32(counter) <= 5)
                {
                    counter = 0;
                    // Make a Db Call
                    await StoreOptionDataInTable(optionData, context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Multiple tried nifty options but not succeed. counter: {counter}");
                counter = 0;

                Utility.LogDetails(ex.Message);
            }
        }


        private async Task<(bool, object, Root?)> GetNiftyOptionData(object counter, IJobExecutionContext context)
        {
            Utility.LogDetails("Send quots reqest counter:" + counter + ", Time: " + context.FireTimeUtc.ToString("hh:mm"));

            bool status = true;
            Root? optionData = null;

            _logger.LogInformation($"Exection time: {counter}");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.43.0");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");

            string url = "https://www.nseindia.com/api/option-chain-indices?symbol=NIFTY";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    optionData = JsonSerializer.Deserialize<Root>(jsonContent, options);

                    if (optionData == null || optionData.Filtered == null || optionData.Records == null)
                    {
                        _logger.LogInformation("Failed to parse JSON content.");
                        Utility.LogDetails("Failed to parse JSON content.");
                        throw new Exception("Failed to parse JSON content.");
                    }
                }
                else
                {
                    Utility.LogDetails($"HTTP Error: {response.StatusCode}");
                    _logger.LogInformation($"HTTP Error: {response.StatusCode}");
                    throw new Exception($"Http Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Utility.LogDetails($"Exception: {ex.Message}");
                _logger.LogInformation($"Exception: {ex.Message}");
                counter = Convert.ToInt16(counter) + 1;
                status = false;
            }

            return (status, counter, optionData);
        }

        private async Task<bool> StoreOptionDataInTable(Root? optionData, IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Adding data to table.");

                if (optionData != null)
                {
                    await _optionDbContext.Database.BeginTransactionAsync();

                    optionData.Records.Data?.ForEach(r =>
                    {
                        r.EntryDate = DateTime.Now.Date;
                        r.Time = DateTime.Now.TimeOfDay;
                    });

                    await _optionDbContext.AllOptionData.AddRangeAsync(optionData.Records.Data);

                    await _optionDbContext.SaveChangesAsync();

                    await _optionDbContext.CurrentExpiryOptionDaata.AddRangeAsync(new FilteredOptionData().ConvertToFilterOptionData(optionData.Filtered.Data));

                    await _optionDbContext.SaveChangesAsync();

                    // Calculate the Summary

                    var currentCPEOIValue = optionData.Filtered.CE.TotOI - optionData.Filtered.PE.TotOI;
                    var currentCPEVolValue = optionData.Filtered.CE.TotVol - optionData.Filtered.PE.TotVol;

                    long previousCEPEOIId = 0;

                    if (await _optionDbContext.Summary.AnyAsync())
                    {
                        previousCEPEOIId = await _optionDbContext.Summary.MaxAsync(m => m.Id);
                    }

                    var lastRecord = await _optionDbContext.Summary.Where(w => w.Id == previousCEPEOIId).FirstOrDefaultAsync();

                    if (lastRecord != null)
                    {
                        previousCPEOIDiffValue = lastRecord.CEPEOIDiff;
                        previousCPEColDiffValue = lastRecord.CEPEVolDiff;
                    }

                    double CEPEOIPreDiff = previousCPEOIDiffValue.HasValue ? ((currentCPEOIValue) - (previousCPEOIDiffValue.Value)) : 0;
                    double CEPEVolPreDiff = previousCPEColDiffValue.HasValue ? ((currentCPEOIValue) - (previousCPEColDiffValue.Value)) : 0;

                    Summary summary = new Summary
                    {
                        TotOICE = optionData.Filtered.CE.TotOI,
                        TotVolCE = optionData.Filtered.CE.TotVol,

                        TotOIPE = optionData.Filtered.PE.TotOI,
                        TotVolPE = optionData.Filtered.PE.TotVol,

                        CEPEOIDiff = optionData.Filtered.CE.TotOI - optionData.Filtered.PE.TotOI,
                        CEPEVolDiff = optionData.Filtered.CE.TotVol - optionData.Filtered.PE.TotVol,

                        CEPEOIPrevDiff = CEPEOIPreDiff,
                        CEPEVolPrevDiff = CEPEVolPreDiff,

                        Time = context.FireTimeUtc.ToLocalTime().TimeOfDay,
                        EntryDate = DateTime.Now.Date
                    };

                    await _optionDbContext.Summary.AddAsync(summary);

                    await _optionDbContext.SaveChangesAsync();

                    await _optionDbContext.Database.CommitTransactionAsync();
                }
            }
            catch (Exception ex)
            {
                Utility.LogDetails($"DB Function Exception: {ex.Message}");
                await _optionDbContext.Database.RollbackTransactionAsync();
                return false;
            }

            return true;
        }

        private async Task<(bool, object, StockRoot)> GetStockData(object counter, IJobExecutionContext context)
        {
            Utility.LogDetails("Send quots reqest counter:" + counter + ", Time: " + context.FireTimeUtc.ToString("hh:mm"));

            bool status = true;
            StockRoot stockData = null;

            _logger.LogInformation($"Exection time: {counter}");

            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(120);
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.43.0");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Cookie", "_abck=BC0AD507C49FDBB22F38C680380003C3~-1~YAAQFv7UF8Uu99GTAQAAbPlEMg1c2ejml1MS0fAnmGS3JkYKEEkUReDmmNvp3+TT3kyaLlHTM/h4Wk+qeIwwSGpLXJHnSmfoj2vbLrBfMD+IIfp/g7SxA4eDJ5oskq7PubIR39FiVNs/mqACWkMmqQo2FzR8/bXeBaDAYCI8QUrY7500Qae7Hy7M7w+BXvDjucJm1I5lkGlNH08oobF9gUkpSCgp0pkyx5oRTz5EeGWTCqEZy3gUbktIXN3Jyv5gq4lvbbpN8kjKdZ/mERSp6Ku0O7YE/3X2L2A+NmbvkeaT3kDkOTQDRuH2aAKe1+FudrMXmAzl0pcANBVZ0NFixUrM5DXMYFsSAknLE4Ta2RjQvxe3AtGBBqg+6sIOZwiiMh096XBHGjMmw5PsFQUiU5e3pZd4CrUcVZX/cuR+cYl1cNSXkjQBu365~-1~-1~-1; ak_bmsc=C8715B2C1DA1EF610EE202E7ACECBCF0~000000000000000000000000000000~YAAQFv7UF8Yu99GTAQAAbPlEMho+GKzEIDLyoHQoqVOq4sbpOPowS3l5BCHAcqyZf7vUm0XYi1iwTb67kR1AcPB6N5wC4WmjMO3clK6WvnqBY5HtuduPRYFaKh47QPZLn/PgHXamsYibin+iKcWjYhmCf+rTFdXi5PdusjPz5+7A3eeh1Qvml9RZ3pT6OTMhjO8QfSDf8dC1edWI1E6/lr5Npoc31cEFnTfdeVXHlOy9bmDjfYcSLeosKjl0c4qMUSuRTXmUWbolESqwoc1QZ7w2JgPZlv2YIiLQQcGSeGLTgcdnfLfOtLGu7RFo/5UJqhs3CGyJPdL7ij+vtR+rEIknD7AUq6e22aUafQ0=; bm_sv=34F43D4AEF5F064A63B9A5ACFAB1DF85~YAAQFv7UF/Jl99GTAQAA3HZMMhpLfrPAMdANRumRq60hMZ7v5WWKG9CnXbvil0DBJ5T4Vaef9od6Krly9cM/1mCmU2X21yGW+TfebMuxMV07EbYWXyaB639sAGB38q6JtE1zLScxvR1qmDAIChxfeEU9FDOQXVYXqnksCkOlP/WHPTQ6Cp0PkM+QF4fqCCgnLp31L3WN/HAwtL0wxp2nx83CJU+Bd0UOebPvLzDK1J3a7a/W5odU6JgegnL8BXjTW5BW~1; bm_sz=4F242C6A1EF8F6F2551345A8B951D5C0~YAAQFv7UF8cu99GTAQAAbPlEMho5OasKj4T1EJJDuXD5YicU3cvlIoEjpr0LH3bLPJ48g2+qQq1gDhrGqyvolYe7SeBSlYpoX4tCnXsMet/vHjVG3ffb0OohO4aVBLrc6oDPHb3/IYNQKLFjQUU2N1FFYY+BH2qDx/gOJBsY+22pjxWShV4hHz9HyslRmavlKXS8+EXbiSBYVxxMzZMN1/emt8GG80VVxgDQxRvwncLoBRSgSf5t2lyiRu35AqhyJC+dKnziMd4RbRqEatb/z8y1+LUfMEs1WzT7Ywfj0DXvCJbJZZW9qLaAn5OQiTBkynDwulf549h0c9Cbyi6H0G9z5a75YCnTifil29ra~3551282~3486007");

            string url = "https://www.nseindia.com/api/equity-stockIndices?index=NIFTY%20200";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    stockData = JsonSerializer.Deserialize<StockRoot>(jsonContent, options);

                    if (stockData == null || stockData.Data == null)
                    {
                        _logger.LogInformation("Failed to parse JSON content.");
                        Utility.LogDetails("Failed to parse JSON content.");
                        throw new Exception("Failed to parse JSON content.");
                    }
                }
                else
                {
                    Utility.LogDetails($"HTTP Error: {response.StatusCode}");
                    _logger.LogInformation($"HTTP Error: {response.StatusCode}");
                    throw new Exception($"Http Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Utility.LogDetails($"Exception: {ex.Message}");
                _logger.LogInformation($"Exception: {ex.Message}");
                counter = Convert.ToInt16(counter) + 1;
                status = false;
            }

            return (status, counter, stockData);
        }

        private async Task<bool> StoreStockDataInTable(StockRoot? stockRoot, IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Adding data to stock table.");

                if (stockRoot != null)
                {
                    await _optionDbContext.Database.BeginTransactionAsync();

                    List<StockData> stockDatas = new List<StockData>();
                    List<StockMetaData> stockMetaDatas = new List<StockMetaData>();

                    foreach (var (f, index) in stockRoot.Data.Select((f, index) => (f, index)))
                    {                        
                        var stockData = new StockData
                        {
                            Priority = f.Priority,
                            Symbol = f.Symbol,
                            Identifier = f.Identifier,
                            Series = f.Series,
                            Open = f.Open,
                            DayHigh = f.DayHigh,
                            DayLow = f.DayLow,
                            LastPrice = f.LastPrice,
                            PreviousClose = f.PreviousClose,
                            Change = f.Change,
                            PChange = f.PChange,
                            TotalTradedVolume = f.TotalTradedVolume,
                            StockIndClosePrice = f.StockIndClosePrice,
                            TotalTradedValue = f.TotalTradedValue,
                            LastUpdateTime = f.LastUpdateTime,
                            YearHigh = f.YearHigh,
                            Ffmc = f.Ffmc,
                            YearLow = f.YearLow,
                            NearWKH = f.NearWKH,
                            NearWKL = f.NearWKL,
                            Date365dAgo = f.Date365dAgo,
                            Chart365dPath = f.Chart365dPath,
                            Date30dAgo = f.Date30dAgo,
                            //PerChange30d = f.PerChange30d,
                            Chart30dPath = f.Chart30dPath,
                            ChartTodayPath = f.ChartTodayPath,
                            EntryDate = DateTime.Now.Date,
                            Time = DateTime.Now.TimeOfDay
                        };

                        stockDatas.Add(stockData);

                        if (f.Meta != null)
                        {
                            var meta = new StockMetaData
                            {
                                Symbol = f.Meta.Symbol,
                                CompanyName = f.Meta.CompanyName,
                                Industry = f.Meta.Industry,
                                IsFNOSec = f.Meta.IsFNOSec,
                                IsCASec = f.Meta.IsCASec,
                                IsSLBSec = f.Meta.IsSLBSec,
                                IsDebtSec = f.Meta.IsDebtSec,
                                IsSuspended = f.Meta.IsSuspended,
                                IsETFSec = f.Meta.IsETFSec,
                                IsDelisted = f.Meta.IsDelisted,
                                Isin = f.Meta.Isin,
                                SlbIsin = f.Meta.SlbIsin,
                                ListingDate = f.Meta.ListingDate,
                                IsMunicipalBond = f.Meta.IsMunicipalBond,
                                EntryDate = DateTime.Now.Date,
                            };

                            stockMetaDatas.Add(meta);
                        }
                    }

                    await _optionDbContext.StockData.AddRangeAsync(stockDatas);

                    //await _optionDbContext.StockMetaData.AddRangeAsync(stockMetaDatas);

                    //await _optionDbContext.SaveChangesAsync();

                    // Advances data

                    stockRoot.Advance.EntryDate = DateTime.Now.Date;
                    stockRoot.Advance.Time = context.FireTimeUtc.ToLocalTime().TimeOfDay;

                    await _optionDbContext.Advance.AddRangeAsync(stockRoot.Advance);

                    await _optionDbContext.SaveChangesAsync();

                    await _optionDbContext.Database.CommitTransactionAsync();

                    /*if (context.FireTimeUtc.ToString("hh:mm") == "09:15"
                        || context.FireTimeUtc.ToString("hh:mm") == "09:20"
                        || context.FireTimeUtc.ToString("hh:mm") == "09:25"
                        || context.FireTimeUtc.ToString("hh:mm") == "09:30")
                    {*/
                        foreach (var stock in stockMetaDatas)
                        {
                            var metaData = await _optionDbContext.StockMetaData.Where(x => x.Symbol.ToLower() == stock.Symbol.ToLower()).FirstOrDefaultAsync();
                            if (metaData == null)
                            {
                                await _optionDbContext.StockMetaData.AddAsync(new StockMetaData
                                {
                                    CompanyName = stock.CompanyName,
                                    IsCASec = stock.IsCASec,
                                    Symbol = stock.Symbol,
                                    EntryDate = DateTime.Now.Date,
                                    Time = context.FireTimeUtc.ToLocalTime().TimeOfDay,
                                    ListingDate = stock.ListingDate,
                                    Industry = stock.Industry,
                                    IsDebtSec = stock.IsDebtSec,
                                    IsDelisted = stock.IsDelisted,
                                    IsETFSec = stock.IsETFSec,
                                    IsFNOSec = stock.IsFNOSec,
                                    Isin = stock.Isin,
                                    IsMunicipalBond = stock.IsMunicipalBond,
                                    IsSLBSec = stock.IsSLBSec,
                                    IsSuspended = stock.IsSuspended,
                                    SlbIsin = stock.SlbIsin,
                                });
                            }
                            else
                            {
                                metaData.CompanyName = stock.CompanyName;
                                metaData.IsCASec = stock.IsCASec;
                                metaData.Symbol = stock.Symbol;
                                metaData.EntryDate = DateTime.Now.Date;
                                metaData.Time = context.FireTimeUtc.ToLocalTime().TimeOfDay;
                                metaData.ListingDate = stock.ListingDate;
                                metaData.Industry = stock.Industry;
                                metaData.IsDebtSec = stock.IsDebtSec;
                                metaData.IsDelisted = stock.IsDelisted;
                                metaData.IsETFSec = stock.IsETFSec;
                                metaData.IsFNOSec = stock.IsFNOSec;
                                metaData.Isin = stock.Isin;
                                metaData.IsMunicipalBond = stock.IsMunicipalBond;
                                metaData.IsSLBSec = stock.IsSLBSec;
                                metaData.IsSuspended = stock.IsSuspended;
                                metaData.SlbIsin = stock.SlbIsin;
                            }

                            await _optionDbContext.SaveChangesAsync();
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                Utility.LogDetails($"DB Function Exception: {ex.Message}");
                await _optionDbContext.Database.RollbackTransactionAsync();
                return false;
            }

            return true;
        }
    }
}
