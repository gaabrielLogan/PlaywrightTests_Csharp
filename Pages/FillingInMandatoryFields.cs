using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages
{
    public class FillingInMandatoryFields
    {
        private readonly IPage _page;

        public FillingInMandatoryFields(IPage page)
        {
            _page = page;
            _txtModelo = _page.Locator("#modelo");
            _txtMarca = _page.Locator("#marca");
            _txtAno = _page.Locator("#ano");
            _txtSenha = _page.Locator("#senha");
            _txtPais = _page.Locator("#pais");
            _btnLogin = _page.Locator("button[type='submit']");
        }

        private readonly ILocator _txtModelo;
        private readonly ILocator _txtMarca;
        private readonly ILocator _txtAno;
        private readonly ILocator _txtSenha;
        private readonly ILocator _txtPais;
        private readonly ILocator _btnLogin;

        public async Task FillInMandatoryFieldsAndSubmitAsync(string modelo, string marca, string ano, string senha, string pais)
        {
            await _txtModelo.FillAsync(modelo);
            await _txtMarca.FillAsync(marca);
            await _txtAno.FillAsync(ano);
            await _txtSenha.FillAsync(senha);
            await _txtPais.FillAsync(pais);
            await _btnLogin.ClickAsync();
        }

        public async Task<string> GetSuccessMessageAsync()
        {
            await _page.WaitForSelectorAsync("#mensagem-sucesso", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            return await _page.TextContentAsync("#mensagem-sucesso");
        }
    }
}
