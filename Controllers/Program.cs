using Microsoft.Playwright;
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Step 1: Use Playwright to extract cookies

        string finalCookie = "";
        string url = "https://www.nseindia.com/market-data/live-equity-market?symbol=NIFTY%20200";

        var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"
        });

        var page = await context.NewPageAsync();

        await Task.Delay(1500);

        await page.SetExtraHTTPHeadersAsync(new System.Collections.Generic.Dictionary<string, string>
        {
            { "Referer", "https://www.nseindia.com/" },
            { "Accept-Language", "en-US,en;q=0.9" },
            { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8" }
        });

        try
        {
            await page.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle                
            });

            await Task.Delay(10000);

            Console.WriteLine("Page loaded successfully!");

            var cookies = await context.CookiesAsync();
            string cookieHeader = string.Join("; ", cookies.Select(c => $"{c.Name}={c.Value}"));

            foreach (var cookie in cookieHeader.Split(";"))
            {
                if (cookie.Trim().StartsWith("_abck="))
                {
                    finalCookie += cookie.Trim() + ";";
                }

                if (cookie.Trim().StartsWith("ak_bmsc="))
                {
                    finalCookie += cookie.Trim() + ";";
                }

                if (cookie.Trim().StartsWith("bm_sv="))
                {
                    finalCookie += cookie.Trim() + ";";
                }

                if (cookie.Trim().StartsWith("bm_sz="))
                {
                    finalCookie += cookie.Trim() + ";";
                }

                if (cookie.Trim().StartsWith("nseappid="))
                {
                    finalCookie += cookie.Trim() + ";";
                }

                if (cookie.Trim().StartsWith("nsit="))
                {
                    finalCookie += cookie.Trim() + ";";
                }
            }
            finalCookie = finalCookie.Trim();

            Console.WriteLine("Cookie: " + finalCookie);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            await browser.CloseAsync();
        }

        try
        {
            string htmlContent = await FetchHtmlAsync(finalCookie);

            Console.WriteLine("Writing the result");

            Console.WriteLine("========================================================================================");

            Console.WriteLine(htmlContent);

            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }
    }

    static async Task<string> FetchHtmlAsync(string cookie)
    {
        try
        {
            string url = "https://www.nseindia.com/api/equity-stockIndices?index=NIFTY%20200";

            using (HttpClient client = new HttpClient())
            {
                // Set User-Agent header to mimic a browser
                client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.43.0");
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("cookie", cookie);

                HttpResponseMessage response = await client.GetAsync(url);
                
                if(response.IsSuccessStatusCode)
                {
                    string htmlContent = await response.Content.ReadAsStringAsync();
                    return htmlContent;
                }
                else
                {
                    Console.WriteLine("Status Code FetchHtmlAsync : " + response.StatusCode);
                }

                return "";                
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"FetchHtmlAsync : Error fetching the URL: {e.Message}");
            return "";
        }
    }
}
