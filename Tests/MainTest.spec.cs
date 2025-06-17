// Tests/MainTest.spec.cs
using NUnit.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using PlaywrightEcommerce.Utils;
using PlaywrightEcommerce.PageObjects;


namespace PlaywrightEcommerce.Tests
{
    public class MainTest : BaseTest
    {
        private class TestData
        {
            public string? username { get; set; }
            public string? password { get; set; }
            public string? productName { get; set; }
        }

        private TestData testData;

        [OneTimeSetUp]
        public void LoadTestData()
        {
            var jsonPath = Path.Combine(AppContext.BaseDirectory, @"TestData\TestData.json");
            var json = File.ReadAllText(jsonPath);
            testData = JsonSerializer.Deserialize<TestData>(json) 
            ?? throw new Exception("Test data JSON could not be parsed.");

        }

        [Test]
        public async Task CompleteEcommerceFlow()
        {
           Console.WriteLine(typeof(Assert).FullName);




            // Initialize page objects
            var loginPage = new LoginPage(Page);
            var dashboardPage = new DashboardPage(Page);
            var cartPage = new AddToCartPage(Page);
            var placeOrderPage = new PlaceOrderPage(Page);

            // Step 1: Go to site and login
            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(testData.username, testData.password);

            var welcomeText = await dashboardPage.GetLoggedInUsernameAsync();
            //             Console.WriteLine($"[DEBUG] Expected username: {testData.username}");

            // Console.WriteLine($"[DEBUG] welcomeText from dashboard: {welcomeText}");

            // Assert.IsTrue(welcomeText.Contains(testData.username), "Login failed");




            // Step 2: Select product
            await dashboardPage.SelectProductAsync(testData!.productName!);
            await dashboardPage.AddToCartAsync();

            // Accept alert popup
            await Page.WaitForTimeoutAsync(1000); // Small wait for alert
            // await Page.RunAndWaitForPopupAsync(() => Page.EvaluateAsync("window.alert = () => true")); // Dummy alert handler
            Page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
await Page.WaitForTimeoutAsync(1000); // Wait for the alert to appear

            // await Page.WaitForTimeoutAsync(1000); // Wait for alert to be processed

            // Step 3: Go to cart
            await cartPage.NavigateToCartAsync();
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await cartPage.ClickAddtoCartAsync();
            // Assert.IsTrue(await cartPage.IsProductInCartAsync(testData.productName!), "Product not found in cart!");
            
            Console.WriteLine("Place Order button clicked successfully.");
            // Step 4: Place order
            // await Page.WaitForSelectorAsync("#orderModal", new() { State = WaitForSelectorState.Visible });
            


            Assert.IsTrue(await placeOrderPage.VerifyProductInCartAsync(testData.productName!), "Product missing in order modal!");
            await Task.Delay(6000);
             Console.WriteLine("asserted product in order modal successfully.");
            
            await placeOrderPage.ClickPlaceOrderAsync();
            // await placeOrderPage.ClickPlaceOrderAsync();
            await placeOrderPage.WaitForOrderModalAsync(); // wait until modal appears
            Console.WriteLine("Place Order button clicked fully.");

            // Fill in order details and submit
            
            await placeOrderPage.FillOrderDetailsAsync(
                name: testData.username,
                country: "India",
                city: "Delhi",
                card: "123412341234",
                month: "06",
                year: "2025"
            );

            await placeOrderPage.SubmitOrderAsync();

            var confirmation = await placeOrderPage.GetConfirmationAsync();
            Assert.That(confirmation, Is.EqualTo("Thank you for your purchase!"));


            await placeOrderPage.ClickOkAsync();
        }
    }
}
