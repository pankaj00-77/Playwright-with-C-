// PageObjects/PlaceOrderPage.cs
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightEcommerce.PageObjects
{
    public class PlaceOrderPage(IPage page)
    {
        private readonly IPage _page = page;

        // Cart Locators
        private ILocator CartProductRow(string productName) => _page.Locator($"#tbodyid tr:has-text('{productName}')");

        // Modal Locators
        private ILocator OrderModal => _page.Locator("#orderModal");
        private ILocator placeOrderbtn => _page.GetByText("Place Order");
        private ILocator NameInput => _page.Locator("#name");
        private ILocator CountryInput => _page.Locator("#country");
        private ILocator CityInput => _page.Locator("#city");
        private ILocator CardInput => _page.Locator("#card");
        private ILocator MonthInput => _page.Locator("#month");
        private ILocator YearInput => _page.Locator("#year");
        private ILocator PurchaseButton => _page.Locator("button:has-text('Purchase')");
        private ILocator ConfirmationMessage => _page.Locator(".sweet-alert > h2");
        private ILocator OKButton => _page.Locator("button:has-text('OK')");

        public async Task<bool> VerifyProductInCartAsync(string productName)
{
    Console.WriteLine($"Verifying product '{productName}' in cart...");
    return await CartProductRow(productName).IsVisibleAsync();
}
public async Task ClickPlaceOrderAsync()
{
    await _page.WaitForSelectorAsync("text=Place Order", new() { State = WaitForSelectorState.Visible });
    await placeOrderbtn.ClickAsync();
}

        public async Task WaitForOrderModalAsync()
        {
            await OrderModal.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 5000 });


        }
       

        public async Task FillOrderDetailsAsync(string name, string country, string city, string card, string month, string year)
        {
            await NameInput.FillAsync(name);
            await CountryInput.FillAsync(country);
            await CityInput.FillAsync(city);
            await CardInput.FillAsync(card);
            await MonthInput.FillAsync(month);
            await YearInput.FillAsync(year);
        }


        public async Task SubmitOrderAsync()
{
    await PurchaseButton.ClickAsync();
    await ConfirmationMessage.WaitForAsync(new() { Timeout = 5000 });
}

        public async Task<string> GetConfirmationAsync()
        {
            return await ConfirmationMessage.InnerTextAsync(); // e.g., "Thank you for your purchase!"
        }

        public async Task ClickOkAsync()
        {
            await OKButton.ClickAsync();
        }
    }
}
