// PageObjects/LoginPage.cs
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightEcommerce.PageObjects
{
    public class LoginPage
    {
        private readonly IPage _page;

        // Locators
        private ILocator LoginLink => _page.Locator("#login2");
        private ILocator UsernameInput => _page.Locator("#loginusername");
        private ILocator PasswordInput => _page.Locator("#loginpassword");
        private ILocator LoginButton => _page.Locator("button:has-text('Log in')");

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateAsync()
        {
            await _page.GotoAsync("https://www.demoblaze.com/");
        }

        public async Task ClickLoginLinkAsync()
        {
            await LoginLink.ClickAsync();
        }

        public async Task DeleteUsernameAsync()
        {
            await UsernameInput.FillAsync(" "); // Clear the input
        }

        public async Task EnterUsernameAsync(string username)
        {
            await UsernameInput.FillAsync(username);
        }

        public async Task DeletePasswordAsync()
        {
            await PasswordInput.FillAsync(" "); // Clear the input
        }


        public async Task EnterPasswordAsync(string password)
        {
            await PasswordInput.FillAsync(password);
        }

        public async Task ClickLoginButtonAsync()
        {
            await LoginButton.ClickAsync();
        }



        public async Task LoginAsync(string username, string password)
        {
            await ClickLoginLinkAsync();
            await _page.WaitForTimeoutAsync(500); // Small wait for modal to appear
            await DeleteUsernameAsync();
            await DeletePasswordAsync();
            await EnterUsernameAsync(username);
            await EnterPasswordAsync(password);
            
            await ClickLoginButtonAsync();
        }
    }
}
