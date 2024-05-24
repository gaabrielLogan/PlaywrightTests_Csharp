using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages
{
    public class ErrorMessageAndHiddenCarPage
    {
        private readonly IPage _page;
        private readonly ILocator _cart;
        private readonly ILocator _successMessage;
        private readonly ILocator _errorMessage;

        public ErrorMessageAndHiddenCarPage(IPage page)
        {
            _page = page;
            _cart = _page.Locator("#cart");
            _successMessage = _page.Locator("#mensagem-sucesso");
            _errorMessage = _page.Locator("#mensagem-erro");
        }

        public async Task<bool> IsCartVisibleAsync() => await _cart.IsVisibleAsync();
        public async Task ShowCartAsync() => await _page.EvalOnSelectorAsync("#cart", "cart => cart.style.display = 'block'");
        public async Task<bool> IsSuccessMessageVisibleAsync() => await _successMessage.IsVisibleAsync();
        public async Task ShowSuccessMessageAsync() => await _page.EvalOnSelectorAsync("#mensagem-sucesso", "successMessage => successMessage.style.display = 'block'");
        public async Task<bool> IsErrorMessageVisibleAsync() => await _errorMessage.IsVisibleAsync();
        public async Task ShowErrorMessageAsync() => await _page.EvalOnSelectorAsync("#mensagem-erro", "errorMessage => errorMessage.style.display = 'block'");
    }
}
