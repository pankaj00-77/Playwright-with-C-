// PageObjects/DashboardPage.cs
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightEcommerce.PageObjects
{
    public class DashboardPage
    {
        private readonly IPage _page;

        // Locators
        private ILocator LoggedInUser => _page.Locator("#nameofuser"); // Example: "Welcome Ram Singh"
        private ILocator ProductCards => _page.Locator(".card-title");
        private ILocator AddToCartButton => _page.Locator("a:has-text('Add to cart')");
        private ILocator ProductLink(string productName) => _page.Locator($".card-title:has-text('{productName}')");

        public DashboardPage(IPage page)
        {
            _page = page;
        }

        public async Task<string> GetLoggedInUsernameAsync()
        {
            return await LoggedInUser.InnerTextAsync(); // Returns something like "Welcome Ram Singh"
        }

        public async Task SelectProductAsync(string productName)
        {
            await ProductLink(productName).ClickAsync();
        }

        public async Task AddToCartAsync()
        {
            await AddToCartButton.ClickAsync();
            Console.WriteLine("Add to cart button clicked successfully.");
        }
    }
}
