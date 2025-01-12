using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Text.Json;

namespace OptionChain
{
    // Task Never Sleeps
    public class MakeServerLive : IJob
    {
        private readonly ILogger<MakeServerLive> _logger;

        public MakeServerLive(ILogger<MakeServerLive> logger)
        {
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Execution call from {nameof(MakeServerLive)}  Started: " + context.FireTimeUtc.ToLocalTime().ToString("hh:mm:ss"));
            Utility.LogDetails($"Execution call from {nameof(MakeServerLive)} Started: " + context.FireTimeUtc.ToLocalTime().ToString("hh:mm:ss"));

            await Task.CompletedTask;
        }
    }

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
            _logger.LogInformation($"Execution call from {nameof(FetchAndProcessJob)} Started: " + context.FireTimeUtc.ToLocalTime().ToString("hh:mm:ss"));
            Utility.LogDetails($"Execution call from {nameof(FetchAndProcessJob)} Started: " + context.FireTimeUtc.ToLocalTime().ToString("hh:mm:ss"));

            await GetNiftyOptions(context);

            await GetStockData(context);

            //await Task.WhenAll(niftyTask, stockTask);

            await Task.CompletedTask;
        }

        public async Task GetStockData(IJobExecutionContext context)
        {

            STEP:

            try
            {
                (bool status, object result, StockRoot StockRoot) = await GetStockData(stockCounter, context);

                if (status == false && Convert.ToInt16(result) <= 3)
                {
                    await Task.Delay(2000);
                    stockCounter = result;

                    goto STEP;
                }

                if (Convert.ToInt32(stockCounter) <= 3)
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

                Utility.LogDetails($"{nameof(GetStockData) } Exception: {ex.Message}");
            }
        }

        public async Task GetNiftyOptions(IJobExecutionContext context)
        {
            STEP:

            try
            {
                (bool status, object result, Root? optionData) = await GetNiftyOptionData(counter, context);

                if (status == false && Convert.ToInt16(result) <= 3)
                {
                    await Task.Delay(2000);
                    counter = result;

                    goto STEP;
                }

                if (Convert.ToInt32(counter) <= 3)
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

                Utility.LogDetails($"{nameof(GetNiftyOptions) } Exception: {ex.Message}");
            }
        }

        private async Task<(bool, object, Root?)> GetNiftyOptionData(object counter, IJobExecutionContext context)
        {
            Utility.LogDetails($"{nameof(GetNiftyOptionData)} -> Send quots reqest counter:" + counter + ", Time: " + context.FireTimeUtc.ToLocalTime().ToString("hh:mm"));

            bool status = true;
            Root? optionData = null;

            _logger.LogInformation($"Exection time: {counter}");

            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.43.0");
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                //client.DefaultRequestHeaders.Add("referer", "https://www.nseindia.com/option-chain");
                //client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
                //client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                //client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                //client.DefaultRequestHeaders.Add("cookie", "_abck=B2CA542D4B41013155829F45D5CBA53E~-1~YAAQn3UsMeNwm0WUAQAAQy65RQ1mSjkar9I3HsD4qRDgNXRGY15APTnhGm4Zm1Rs/OcKGepQoTN0TtIFyk0S/Fqj+75GgvgefCbanjUSNxVVEmfnKLS8wrJF/jo5AsJpFHMNKcHfCYYTfB9aCJA4e+dxxY+DKNTekJw83qbQudp50O+4/zsTGNiDoJ534KhgxDIw0wHAy9VoRjwGdM/z5hQPcx+UL+jaLgvfUN+lyKzUeuieFcbeH8dZYqA9a3mY5tDTHxYA69BX6iSa0wBffaVXQTR3E1ZVZDAJl6T1qXr0b1VEHBCJIjO59zr1YS+zWmlj6wA1sHS/32E1m2GLVyG1967IPQKrBV4PEhPJyqx3t4KJklUjkdFk8vU4dWbFhEm/nBDwUSUnsi3lyPFK9IPSMiNhpljMwAZHsrCI7tajv3Yvq/I/Mn3+~-1~-1~-1; ak_bmsc=3D1063340016099859208A8E2424381C~000000000000000000000000000000~YAAQn3UsMeRwm0WUAQAAQy65RRqaeYfi3x/vWm0cUXqF7kAyB6iLGn+oA7r6XfSzcxKA2ksmaa2ld9j6AX4U7NodtNdg8CmlvUsUMA99Kf0r3IRF8qJH3bdqlAIVu6nwVEyiBQDm/SiSneOflUIx79/dDnMwvZ2xkUiBM1kKGIiD0245UDUNkP7uR867GGCcMgENTg5BRdndBapppX6Jq8aw/QzX36YYol8PaorjPOSV4KJ6F7aojE7K2odFrStGakppEUeFM9Cpgh6JYSfiz2lGLjtk/pd5FZtVzLNhwaeXVt+/cLVp+Ucuy0NC203hcdcyWv9ukSq5anoOaTT3uu+JE1n/DI9huZ3V; bm_sv=344BE8C60C7B1AE41C4E93D555D1214C~YAAQvIosMZ7jlUSUAQAARyrARRqyjK3VlOsF0cDME1Bcdxn7krWE29RZV7Rfxbo1vkRihe6DYRL6upiKlJpFuT2xiRDqcKULAZz7LRHCb15UXAPRFUAllmnF9+Ae5Smm2IK5Yyqh8Hg1VnnrgEOLkkyr2DUU838OBw1I/3EdTCCJB7HarMKncGS8T8TIrtDfJVn83dkZwBgpyMbO1AyAPwWMvJcEQTwUGgtti2CZ2r41L0DFXsQKgk2ZtrTxV4zmW6M=~1; bm_sz=D3E7C40E38ED349B57BAEF68D48E6A25~YAAQn3UsMeZwm0WUAQAAQy65RRpOxNl0ezDkfr4Y9qYG+350EqBHTc4Bnohh9xpRqiL6s3npH1/vlwnezfV7MfWbPNy3siCT6RMLAgkItE9rRO0kE4WsS0PTsMHb7ehZTO1P/OCam4yvNEsPIGWlt7JFix8BheqYxePRkCsj+6yY+GjF+i0+zJYzXHlX581bNE/MKuSo1MzibzoRNdEBZF50qKHk67FDkaikoKfadqLQD7s9kjzAvwmmOeIQ23U5TV7TMzO2fQnoew91JcCZ1ujXRzojtKcwbj3BIK28UfRg+KE+WMuv3PXSGWTiQoq1tekS34B3zU4TZbUEAT63nw0hpRIwTqcid52/tQ==~3552309~3160117; nseappid=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJhcGkubnNlIiwiYXVkIjoiYXBpLm5zZSIsImlhdCI6MTczNjMzNjU1MCwiZXhwIjoxNzM2MzQzNzUwfQ.1Or4NousiqpNrZCQ6pWTICHekupQkqPvq9RtpMFXt98; nsit=kC7PFCY3kcbjl5B3NHJ-bPVB");
    
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
                            Utility.LogDetails($"{nameof(GetNiftyOptionData)} -> Failed to parse JSON content.");
                            throw new Exception("Failed to parse JSON content.");
                        }
                    }
                    else
                    {                        
                        Utility.LogDetails($"{nameof(GetNiftyOptionData)} -> HTTP Error: {response.StatusCode}.");
                        _logger.LogInformation($"HTTP Error: {response.StatusCode}");
                        throw new Exception($"Http Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {                    
                    Utility.LogDetails($"{nameof(GetNiftyOptionData)} -> Exception: {ex.Message}.");
                    _logger.LogInformation($"Exception: {ex.Message}");
                    counter = Convert.ToInt16(counter) + 1;
                    status = false;
                }
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

                    if (optionData.Records != null 
                        && optionData.Filtered != null 
                        && optionData.Records.Data != null 
                        && optionData.Filtered.Data != null)
                    {
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

                    }
                    else
                    {
                        await _optionDbContext.Database.RollbackTransactionAsync();
                    }

                    await _optionDbContext.Database.CommitTransactionAsync();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DB Function Exception: {ex.Message}");                
                Utility.LogDetails($"{nameof(StoreOptionDataInTable)} -> Exception: {ex.Message}.");
                await _optionDbContext.Database.RollbackTransactionAsync();
                return false;
            }

            return true;
        }

        private async Task<(bool, object, StockRoot)> GetStockData(object counter, IJobExecutionContext context)
        {
            Utility.LogDetails($"{nameof(GetStockData)} -> Send quots reqest counter:" + counter + ", Time: " + context.FireTimeUtc.ToLocalTime().ToString("hh:mm"));

            bool status = true;
            StockRoot stockData = null;

            _logger.LogInformation($"Exection time: {counter}");

            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.43.0");
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                //client.DefaultRequestHeaders.Add("referer", "https://www.nseindia.com/option-chain");
                //client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
                //client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                //client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                //client.DefaultRequestHeaders.Add("cookie", "_abck=B2CA542D4B41013155829F45D5CBA53E~-1~YAAQn3UsMeNwm0WUAQAAQy65RQ1mSjkar9I3HsD4qRDgNXRGY15APTnhGm4Zm1Rs/OcKGepQoTN0TtIFyk0S/Fqj+75GgvgefCbanjUSNxVVEmfnKLS8wrJF/jo5AsJpFHMNKcHfCYYTfB9aCJA4e+dxxY+DKNTekJw83qbQudp50O+4/zsTGNiDoJ534KhgxDIw0wHAy9VoRjwGdM/z5hQPcx+UL+jaLgvfUN+lyKzUeuieFcbeH8dZYqA9a3mY5tDTHxYA69BX6iSa0wBffaVXQTR3E1ZVZDAJl6T1qXr0b1VEHBCJIjO59zr1YS+zWmlj6wA1sHS/32E1m2GLVyG1967IPQKrBV4PEhPJyqx3t4KJklUjkdFk8vU4dWbFhEm/nBDwUSUnsi3lyPFK9IPSMiNhpljMwAZHsrCI7tajv3Yvq/I/Mn3+~-1~-1~-1; ak_bmsc=3D1063340016099859208A8E2424381C~000000000000000000000000000000~YAAQn3UsMeRwm0WUAQAAQy65RRqaeYfi3x/vWm0cUXqF7kAyB6iLGn+oA7r6XfSzcxKA2ksmaa2ld9j6AX4U7NodtNdg8CmlvUsUMA99Kf0r3IRF8qJH3bdqlAIVu6nwVEyiBQDm/SiSneOflUIx79/dDnMwvZ2xkUiBM1kKGIiD0245UDUNkP7uR867GGCcMgENTg5BRdndBapppX6Jq8aw/QzX36YYol8PaorjPOSV4KJ6F7aojE7K2odFrStGakppEUeFM9Cpgh6JYSfiz2lGLjtk/pd5FZtVzLNhwaeXVt+/cLVp+Ucuy0NC203hcdcyWv9ukSq5anoOaTT3uu+JE1n/DI9huZ3V; bm_sv=344BE8C60C7B1AE41C4E93D555D1214C~YAAQvIosMZ7jlUSUAQAARyrARRqyjK3VlOsF0cDME1Bcdxn7krWE29RZV7Rfxbo1vkRihe6DYRL6upiKlJpFuT2xiRDqcKULAZz7LRHCb15UXAPRFUAllmnF9+Ae5Smm2IK5Yyqh8Hg1VnnrgEOLkkyr2DUU838OBw1I/3EdTCCJB7HarMKncGS8T8TIrtDfJVn83dkZwBgpyMbO1AyAPwWMvJcEQTwUGgtti2CZ2r41L0DFXsQKgk2ZtrTxV4zmW6M=~1; bm_sz=D3E7C40E38ED349B57BAEF68D48E6A25~YAAQn3UsMeZwm0WUAQAAQy65RRpOxNl0ezDkfr4Y9qYG+350EqBHTc4Bnohh9xpRqiL6s3npH1/vlwnezfV7MfWbPNy3siCT6RMLAgkItE9rRO0kE4WsS0PTsMHb7ehZTO1P/OCam4yvNEsPIGWlt7JFix8BheqYxePRkCsj+6yY+GjF+i0+zJYzXHlX581bNE/MKuSo1MzibzoRNdEBZF50qKHk67FDkaikoKfadqLQD7s9kjzAvwmmOeIQ23U5TV7TMzO2fQnoew91JcCZ1ujXRzojtKcwbj3BIK28UfRg+KE+WMuv3PXSGWTiQoq1tekS34B3zU4TZbUEAT63nw0hpRIwTqcid52/tQ==~3552309~3160117; nseappid=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJhcGkubnNlIiwiYXVkIjoiYXBpLm5zZSIsImlhdCI6MTczNjMzNjU1MCwiZXhwIjoxNzM2MzQzNzUwfQ.1Or4NousiqpNrZCQ6pWTICHekupQkqPvq9RtpMFXt98; nsit=kC7PFCY3kcbjl5B3NHJ-bPVB");

                string url = "https://www.nseindia.com/api/equity-stockIndices?index=NIFTY%2050";
    
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
                            Utility.LogDetails($"{nameof(GetStockData)} -> Failed to parse JSON content.");
                            throw new Exception("Failed to parse JSON content.");
                        }
                    }
                    else
                    {                        
                        Utility.LogDetails($"{nameof(GetStockData)} -> HTTP Error: {response.StatusCode}.");
                        _logger.LogInformation($"HTTP Error: {response.StatusCode}");
                        throw new Exception($"Http Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {                    
                    Utility.LogDetails($"{nameof(GetStockData)} -> Exception: {ex.Message}.");
                    _logger.LogInformation($"Exception: {ex.Message}");
                    counter = Convert.ToInt16(counter) + 1;
                    status = false;
                }
            }

            return (status, counter, stockData);
        }

        private async Task<bool> StoreStockDataInTable(StockRoot? stockRoot, IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Adding data to stock table.");

                if (stockRoot != null
                    && stockRoot.Data != null)
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
                Utility.LogDetails($"{nameof(StoreStockDataInTable)} -> Exception: {ex.Message}.");
                await _optionDbContext.Database.RollbackTransactionAsync();
                return false;
            }

            return true;
        }
    }
}
