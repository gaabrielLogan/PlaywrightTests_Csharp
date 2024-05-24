using Microsoft.Playwright;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using PlaywrightTests.Pages;

namespace PlaywrightTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class CarFormTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPage _page;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
        }

        [SetUp]
        public async Task SetUp()
        {
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            await _page.GotoAsync("https://gaabrielogan2.github.io/app-car");
        }

        [TearDown]
        public async Task TearDown()
        {
            await _page.CloseAsync();
            await _context.CloseAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [Test]
        public async Task FillingInMandatoryFieldsAndCheckingTheSuccessMessage()
        {
            var fillingInMandatoryFields = new FillingInMandatoryFields(_page);

            await fillingInMandatoryFields.FillInMandatoryFieldsAndSubmitAsync(
                modelo: "Chevette",
                marca: "Chevrolet",
                ano: "1973",
                senha: "senha",
                pais: "Alemanha"
            );

            var successMessage = await fillingInMandatoryFields.GetSuccessMessageAsync();

            Assert.That(successMessage, Is.EqualTo("Pedido realizado com sucesso!"));
        }

        [Test]
        public async Task EnablingADisabledFieldAndFillingItOut()
        {
            var campoDesabilitado = await _page.QuerySelectorAsync("#campoDesabilitado");
            Assert.NotNull(campoDesabilitado);
            Assert.That(await campoDesabilitado.InputValueAsync(), Is.Empty);
            await campoDesabilitado.EvaluateAsync("element => element.removeAttribute('disabled')");
            await campoDesabilitado.FillAsync("50%");
            Assert.That(await campoDesabilitado.InputValueAsync(), Is.EqualTo("50%"));
        }

        [Test]
        public async Task InteractingWithADateField()
        {
            await _page.FillAsync("#hora", "23:23");
            Assert.That(await _page.InputValueAsync("#hora"), Is.EqualTo("23:23"));
        }

        [Test]
        public async Task SimulatingCtrlVCommandToPasteExtendedTextIntoTextareaField()
        {
            var textareaPage = new TextareaPage(_page);

            var longText = new string('0', 200);
            await textareaPage.FillTextareaAsync(longText);

            var textValue = await textareaPage.GetTextareaValueAsync();
            Assert.That(textValue, Is.EqualTo(longText));
        }

        [Test]
        public async Task SelectingTheFuelInDropdownMenu()
        {
            await _page.SelectOptionAsync("#combustivel", "Gasolina");
            var selectedValue = await _page.EvalOnSelectorAsync<string>("#combustivel", "e => e.value");
            Assert.That(selectedValue, Is.EqualTo("gasolina"));
        }

        [Test]
        public async Task TestingCheckboxElement()
        {
            var checkbox = await _page.QuerySelectorAsync("#direcaoHidraulica");
            Assert.NotNull(checkbox);
            Assert.That(await checkbox.IsCheckedAsync(), Is.False);
            await checkbox.CheckAsync();
            Assert.That(await checkbox.IsCheckedAsync(), Is.True);
        }

        [Test]
        public async Task TestingInputDate()
        {
            await _page.FillAsync("#data", "2024-01-01");
            var inputValue = await _page.EvalOnSelectorAsync<string>("#data", "e => e.value");
            Assert.That(inputValue, Is.EqualTo("2024-01-01"));
        }

        [Test]
        public async Task TestingInputRadio()
        {
            var radioButtons = await _page.QuerySelectorAllAsync("input[type='radio']");
            Assert.That(radioButtons.Count, Is.EqualTo(3));

            foreach (var radioButton in radioButtons)
            {
                Assert.That(await radioButton.IsCheckedAsync(), Is.False);
                await radioButton.CheckAsync();
                Assert.That(await radioButton.IsCheckedAsync(), Is.True);
            }
        }

        [Test]
        public async Task TestingInputColor()
        {
            await _page.FillAsync("#cor", "#ffa500");
            var inputValue = await _page.EvalOnSelectorAsync<string>("#cor", "e => e.value");
            Assert.That(inputValue, Is.EqualTo("#ffa500"));
        }

        [Test]
        public async Task TestingLinks()
        {
            var links = await _page.QuerySelectorAllAsync("a");

            foreach (var link in links)
            {
                await link.EvaluateAsync("el => el.removeAttribute('target')");
                await link.ClickAsync();
                await _page.WaitForSelectorAsync(".gallery-title", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            }
        }

        [Test]
        public async Task CssTest()
        {
            var button = await _page.QuerySelectorAsync("button[type='submit']");
            var buttonColor = await button.EvaluateAsync<string>("btn => getComputedStyle(btn).color");
            Assert.That(buttonColor, Is.EqualTo("rgb(255, 255, 255)"));
        }

        [Test]
        public async Task DisplayingErrorMessageAndHiddenCarInTheApp()
        {
            var errorMessageAndHiddenCarPage = new ErrorMessageAndHiddenCarPage(_page);

            Assert.That(await errorMessageAndHiddenCarPage.IsCartVisibleAsync(), Is.False);

            await errorMessageAndHiddenCarPage.ShowCartAsync();
            Assert.That(await errorMessageAndHiddenCarPage.IsCartVisibleAsync(), Is.True);

            Assert.That(await errorMessageAndHiddenCarPage.IsSuccessMessageVisibleAsync(), Is.False);

            await errorMessageAndHiddenCarPage.ShowSuccessMessageAsync();
            Assert.That(await errorMessageAndHiddenCarPage.IsSuccessMessageVisibleAsync(), Is.True);

            Assert.That(await errorMessageAndHiddenCarPage.IsErrorMessageVisibleAsync(), Is.False);

            await errorMessageAndHiddenCarPage.ShowErrorMessageAsync();
            Assert.That(await errorMessageAndHiddenCarPage.IsErrorMessageVisibleAsync(), Is.True);
        }
    }
}