using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OptionChain
{
    public class FetchAndProcessJob : IJob
    {
        private readonly ILogger<FetchAndProcessJob> _logger;
        private readonly OptionDbContext _optionDbContext;
        private readonly IConfiguration _configuration;
        private object counter = 0;
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
                else
                {
                    counter = 0;
                    // Make a Db Call
                    await StoreOptionDataInTable(optionData, context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Multiple tried but not succeed. counter: {counter}");
                counter = 0;

                Utility.LogDetails(ex.Message);
            }

            await Task.CompletedTask;
        }

        public async Task<(bool, object, Root?)> GetNiftyOptionData(object counter, IJobExecutionContext context)
        {
            Utility.LogDetails("Send quots reqest counter:" + counter + ", Time: " + context.FireTimeUtc.ToString("hh:mm"));

            bool status = true;
            Root? optionData = null;

            _logger.LogInformation($"Exection time: {counter}");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.43.0");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");

            string url = "https://www.nseindia.com/api/option-chain-indices?symbol=NIFTY1";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    optionData = JsonSerializer.Deserialize<Root>(jsonContent, options);

                    if (optionData != null && optionData.Filtered != null)
                    {
                        string data = DateTime.Now.ToString() + Environment.NewLine;
                        data += $"CE - {optionData.Filtered.CE.TotOI}, PE - {optionData.Filtered.PE.TotOI}, Diff - {optionData.Filtered.CE.TotOI - optionData.Filtered.PE.TotOI}" + Environment.NewLine;
                        data += $"CE - {optionData.Filtered.CE.TotVol}, PE - {optionData.Filtered.PE.TotVol}, Diff - {optionData.Filtered.CE.TotVol - optionData.Filtered.PE.TotVol}" + Environment.NewLine;
                        data += "-------------------------------------------" + Environment.NewLine;

                        _logger.LogInformation(data);
                    }
                    else
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

        public async Task<bool> StoreOptionDataInTable(Root? optionData, IJobExecutionContext context)
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

                        Time = context.FireTimeUtc.ToLocalTime().ToString("hh:mm"),
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
    }
}
