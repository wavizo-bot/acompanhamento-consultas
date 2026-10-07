using Microsoft.Web.WebView2.Core;
using System.IO;

namespace AcompanhamentoConsultas;

public partial class Form1 : Form
{
    // Nome reservado na Store (Package/Properties/DisplayName).
    private const string AppTitle = "SlSS Acompanhamento de consultas";

    public Form1()
    {
        InitializeComponent();
        Text = AppTitle;
        InitializeWebViewAsync();
    }

    private static string UserDataFolder =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "wavizo.SlSSAcompanhamentodeconsultas",
            "WebView2");

    private async void InitializeWebViewAsync()
    {
        try
        {
            // Pasta de dados explicita e gravavel: o padrao do WebView2 falha
            // sob identidade de pacote (MSIX/Store) em varias maquinas.
            Directory.CreateDirectory(UserDataFolder);
            var env = await CoreWebView2Environment.CreateAsync(null, UserDataFolder);
            await webView21.EnsureCoreWebView2Async(env);

            CoreWebView2 core = webView21.CoreWebView2;
            if (core == null)
                throw new InvalidOperationException("CoreWebView2 nulo após inicialização.");

            string htmlPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "index.html");
            if (!File.Exists(htmlPath))
                throw new FileNotFoundException($"Arquivo inicial não encontrado: {htmlPath}");

            core.Navigate(new Uri(htmlPath).AbsoluteUri);
        }
        catch (Exception ex)
        {
            ReportStartupError(ex);
        }
    }

    private void WebView21_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        if (!e.IsSuccess)
        {
            ReportStartupError(e.InitializationException
                ?? new InvalidOperationException("Falha desconhecida ao inicializar o WebView2."));
        }
    }

    private static void ReportStartupError(Exception ex)
    {
        // Log em local gravavel para diagnostico (o usuario pode enviar o arquivo).
        try
        {
            Directory.CreateDirectory(UserDataFolder);
            File.WriteAllText(
                Path.Combine(UserDataFolder, "startup-error.log"),
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex}");
        }
        catch { /* ultimo recurso: nao travar por causa do log */ }

        MessageBox.Show(
            $"Não foi possível abrir o conteúdo do app.\n\n{ex.Message}\n\n" +
            "Verifique se o Microsoft Edge WebView2 Runtime está instalado.\n" +
            $"Detalhes salvos em: {Path.Combine(UserDataFolder, "startup-error.log")}",
            AppTitle,
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
