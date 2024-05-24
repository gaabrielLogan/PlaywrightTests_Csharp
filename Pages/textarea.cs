using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages
{
    public class TextareaPage
    {
        private readonly IPage _page;

        public TextareaPage(IPage page)
        {
            _page = page;
        }

        public async Task FillTextareaAsync(string text)
        {
            await _page.FillAsync("textarea", text);
        }

        public async Task<string> GetTextareaValueAsync()
        {
            return await _page.InputValueAsync("textarea");
        }
    }
}
