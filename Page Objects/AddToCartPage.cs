// PageObjects/AddToCartPage.cs
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightEcommerce.PageObjects
{
    public class AddToCartPage
    {
        private readonly IPage _page;

        // Locators
        private ILocator   CartLink => _page.Locator("#cartur");
        private ILocator CartTable => _page.Locator("#tbodyid");
        private ILocator addToCartButton => _page.Locator(".btn-success");
        private ILocator ProductRow(string productName) => _page.Locator($"#tbodyid tr:has-text('{productName}')");

        public AddToCartPage(IPage page)
        {
            _page = page;
        }
        public async Task ClickAddtoCartAsync()
        {
           Console.WriteLine("Place Order button clicked success.");
            await addToCartButton.ClickAsync(new LocatorClickOptions { Force = true });

        }
        public async Task NavigateToCartAsync()
        {
            await CartLink.ClickAsync();
            Console.WriteLine("NavigateToCartAsync successfully.");

            // await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        }

        public async Task<bool> IsProductInCartAsync(string productName)
        {
            Console.WriteLine("IsProductInCartAsync successfully.");
            return await ProductRow(productName).IsVisibleAsync();
        }

        
    }
}
