/*using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightEcommerce.Utils
{
    public class BaseTest
    {
        protected IPlaywright _playwright;
        protected IBrowser _browser;
        protected IBrowserContext _context;
        protected IPage _page;

        [SetUp]
        public async Task ChromeSetup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 50 // Optional: slows down operations for better visibility


            });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        [TearDown]
        public async Task Teardown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        protected IPage Page => _page;
    }
}
*/
/*
using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightEcommerce.Utils
{
    public class BaseTest
    {
        protected IBrowser Browser;
        protected IPage _page;
        protected IBrowserContext Context;
        protected IPlaywright Playwright;


        [SetUp]
        public async Task WebkitSetup()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            Browser = await Playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            Context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                RecordVideoDir = "Reports/Videos", // for video
                RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 },
                // Screenshots are taken manually in C#
            });

            _page = await _context.NewPageAsync();

            // ✅ Start tracing (like `trace: 'on'`)
            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        [TearDown]
        public async Task TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;

            // ✅ Stop tracing and save trace file (like `trace: 'on'`)
            await Context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = $"Reports/Traces/{testName}_trace.zip"
            });

            var result = TestContext.CurrentContext.Result.Outcome.Status;
            if (result == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                // ✅ Manually take screenshot (screenshot: 'off' means only take on failure)
                await _page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = $"Reports/Screenshots/{testName}_failed.png"
                });
            }

            await Browser.CloseAsync();
            Playwright.Dispose();
        }
    }
}
*/

using Allure.Commons;
using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightEcommerce.Utils
{
    public class BaseTest
    {
        protected IBrowser Browser;
        protected IBrowserContext Context;
        protected IPage _page;
        protected IPlaywright Playwright;

        [SetUp]
        public async Task Setup()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            string browserName = TestContext.Parameters.Get("BrowserName", "chromium"); // default: chromium

            IBrowserType browserType = browserName switch
            {
                "firefox" => Playwright.Firefox,
                "webkit" => Playwright.Webkit,
                _ => Playwright.Chromium
            };

            Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true, // Set to true for headless mode
                

            });
            Context = await Browser.NewContextAsync();
            _page = await Context.NewPageAsync();
        }
    }
}

// // ✅ Make it responsive: Set viewport & device scale
// Context = await Browser.NewContextAsync(new BrowserNewContextOptions
// {
//     ViewportSize = new ViewportSize { Width = 375, Height = 812 }, // iPhone 12 size
//     IsMobile = true,
//     DeviceScaleFactor = 2,
//     UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1",
//     RecordVideoDir = "Reports/Videos"
// });
/*
            _page = await Context.NewPageAsync();

            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        [TearDown]
        public async Task TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;

            await Context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = $"C:/Users/Pankaj/OneDrive/Desktop/DemoBlaze playwright C#.net/PlaywrightEcommerce/Reports/{testName}_trace.zip"
            });

            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                await _page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = $"C:/Users/Pankaj/OneDrive/Desktop/DemoBlaze playwright C#.net/PlaywrightEcommerce/Reports/{testName}_failed.png"
                });
            }

            await Browser.CloseAsync();
            Playwright.Dispose();
        }
    }
}

/*
using Microsoft.Playwright;
using Allure.Commons;

namespace PlaywrightEcommerce.Utils
{
    [AllureNUnit] // Enables Allure
    public class BaseTest
    {
        protected IBrowser Browser;
        protected IBrowserContext Context;
        protected IPage Page;
        protected IPlaywright Playwright;

        private readonly string _reportDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
        private readonly string _traceDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Traces");
        private readonly string _screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Screenshots");

        [SetUp]
        public async Task Setup()
        {
            Directory.CreateDirectory(_traceDir);
            Directory.CreateDirectory(_screenshotDir);

            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            string browserName = TestContext.Parameters.Get("BrowserName", "chromium");
            bool headless = bool.Parse(TestContext.Parameters.Get("Headless", "true"));

            IBrowserType browserType = browserName switch
            {
                "firefox" => Playwright.Firefox,
                "webkit" => Playwright.Webkit,
                _ => Playwright.Chromium
            };

            Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            Context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                RecordVideoDir = Path.Combine(_reportDir, "Videos")
            });

            Page = await Context.NewPageAsync();

            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        [TearDown]
        public async Task TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            string tracePath = Path.Combine(_traceDir, $"{testName}_trace.zip");
            await Context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });

            // Attach trace to Allure
            AllureLifecycle.Instance.AddAttachment($"{testName} Trace", "application/zip", tracePath);

            if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                string screenshotPath = Path.Combine(_screenshotDir, $"{testName}_failed.png");
                await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });

                // Attach screenshot to Allure
                AllureLifecycle.Instance.AddAttachment($"{testName} Screenshot", "image/png", screenshotPath);
            }

            await Browser.CloseAsync();
            Playwright.Dispose();
        }
    }

        internal class AllureNUnitAttribute : Attribute
        {
        }
    }
    */
