using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;
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

            //var optionsTask = GetNiftyOptions(context);
            //await GetNiftyOptions(context);

            //var stockTask = GetStockData(context);
            await GetStockData(context);

            //await Task.WhenAll(optionsTask, stockTask);

            await Task.CompletedTask;
        }

        public async Task GetStockData(IJobExecutionContext context)
        {

        STEP:

            try
            {
                (bool status, object result, StockRoot StockRoot) = await GetStockData(stockCounter, context);

                if (status == false && Convert.ToInt16(result) <= 2)
                {
                    await Task.Delay(2000);
                    stockCounter = result;

                    goto STEP;
                }

                if (Convert.ToInt32(stockCounter) <= 2)
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

                if (status == false && Convert.ToInt16(result) <= 2)
                {
                    await Task.Delay(2000);
                    counter = result;

                    goto STEP;

                }

                if (Convert.ToInt32(counter) <= 2)
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
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.43.0");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");

            string url = "https://www.nseindia.com/api/equity-stockIndices?index=NIFTY 200";

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

                    stockRoot.Data?.ForEach(f =>
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
                            PerChange30d = f.PerChange30d,
                            Chart30dPath = f.Chart30dPath,
                            ChartTodayPath = f.ChartTodayPath,
                            EntryDate = DateTime.Now.Date,
                            Time = DateTime.Now.TimeOfDay
                        };

                        stockDatas.Add(stockData);

                        stockMetaDatas.Add(new StockMetaData
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
                        });
                    });

                    await _optionDbContext.StockData.AddRangeAsync(stockDatas);

                    await _optionDbContext.StockMetaData.AddRangeAsync(stockMetaDatas);

                    await _optionDbContext.SaveChangesAsync();

                    // Advances data

                    stockRoot.Advance.EntryDate = DateTime.Now.Date;
                    stockRoot.Advance.Time = context.FireTimeUtc.ToLocalTime().TimeOfDay;

                    await _optionDbContext.Advance.AddRangeAsync(stockRoot.Advance);

                    await _optionDbContext.SaveChangesAsync();

                    await _optionDbContext.Database.CommitTransactionAsync();

                    if (context.FireTimeUtc.ToString("hh:mm") == "09:15"
                        || context.FireTimeUtc.ToString("hh:mm") == "09:20"
                        || context.FireTimeUtc.ToString("hh:mm") == "09:25"
                        || context.FireTimeUtc.ToString("hh:mm") == "09:30")
                    {
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
                    }
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
