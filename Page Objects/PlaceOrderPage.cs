/*
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
            await _page.WaitForTimeoutAsync(1000);
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
}*/

using Microsoft.Playwright;
using System.Threading.Tasks;

public class PlaceOrderPage
{
    private readonly IPage _page;

    public PlaceOrderPage(IPage page)
    {
        _page = page;
    }

    // Locators
    private ILocator PlaceOrderBtn => _page.Locator(".btn-success");// Assuming this is the correct selector for the Place Order button
    private ILocator NameInput => _page.Locator(".modal-content #name");
    private ILocator CountryInput => _page.Locator(".modal-content #country");
    private ILocator CityInput => _page.Locator(".modal-content #city");
    private ILocator CardInput => _page.Locator(".modal-content #card");
    private ILocator MonthInput => _page.Locator(".modal-content #month");
    private ILocator YearInput => _page.Locator(".modal-content #year");
    private ILocator PurchaseButton => _page.Locator("button:has-text('Purchase')");
    private ILocator SuccessModal => _page.Locator(".sweet-alert.showSweetAlert.visible");
    private ILocator OKButton => _page.Locator("button.confirm");

    // Method to fill form and submit
    public async Task ClickPlaceOrderAsync()
    {
        await PlaceOrderBtn.ClickAsync();
        await _page.WaitForTimeoutAsync(2000);
    }
    
    public async Task FillOrderFormAndSubmitAsync(string name, string country, string city, string card, string month, string year)
    {
        Console.WriteLine("✔ Order form fill started...");

        // ✅ Wait for the modal to be visible
        await _page.Locator("#orderModal").WaitForAsync(new() { State = WaitForSelectorState.Visible });

        Console.WriteLine("✔ Modal is visible, filling order form...");


        // ✅ Fill the form fields with waits for visibility
        await NameInput.WaitForAsync();
        await NameInput.FillAsync(name);
        Console.WriteLine("✔ Entered name.");

        await CountryInput.WaitForAsync();
        await CountryInput.FillAsync(country);
        Console.WriteLine("✔ Entered country.");

        await CityInput.WaitForAsync();
        await CityInput.FillAsync(city);
        Console.WriteLine("✔ Entered city.");

        await CardInput.WaitForAsync();
        await CardInput.FillAsync(card);
        Console.WriteLine("✔ Entered card details.");

        await MonthInput.WaitForAsync();
        await MonthInput.FillAsync(month);
        Console.WriteLine("✔ Entered month.");

        await YearInput.WaitForAsync();
        await YearInput.FillAsync(year);
        Console.WriteLine("✔ Entered year.");

        // ✅ Click the Purchase button
        await PurchaseButton.WaitForAsync();
        await PurchaseButton.ClickAsync();
        Console.WriteLine("✔ Purchase button clicked, waiting for confirmation...");
}


    // Check if success popup is visible
    public async Task<bool> IsOrderSuccessfulAsync()
    {
        await _page.WaitForSelectorAsync(".sweet-alert.showSweetAlert.visible");
        return await SuccessModal.IsVisibleAsync();
    }

    // Confirm/Close the success modal
    public async Task ConfirmSuccessAsync()
    {
        await OKButton.ClickAsync();
    }
}
