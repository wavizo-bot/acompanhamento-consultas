using Microsoft.Web.WebView2.Core;
using System.IO;

namespace AcompanhamentoConsultas;

public partial class Form1 : Form
{
    // Nome reservado na Store (Package/Properties/DisplayName).
    private const string AppTitle = "SlSS Acompanhamento de consultas";
    private bool _webViewReady;

    public Form1()
    {
        InitializeComponent();
        Text = AppTitle;
        Shown += Form1_Shown;
    }

    private static string UserDataFolder => Path.Combine(Program.DiagDir, "WebView2");

    private async void Form1_Shown(object? sender, EventArgs e)
    {
        Program.Step("form-shown, webview-init-start");
        // Watchdog: se a inicialização travar sem exceção, registra e avisa.
        var watchdog = Task.Delay(TimeSpan.FromSeconds(60)).ContinueWith(_ =>
        {
            if (!_webViewReady)
            {
                Program.Step("webview-init-TIMEOUT-60s");
                ShowWebError("O conteúdo demorou demais para carregar (60 s). Verifique o Microsoft Edge WebView2 Runtime.");
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());

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
            _webViewReady = true;
            Program.Step("webview-init-ok, navigate: " + htmlPath);
        }
        catch (Exception ex)
        {
            Program.Step("webview-init-EX: " + ex.GetType().FullName + ": " + ex.Message);
            ReportStartupError(ex);
        }
    }

    private void WebView21_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        if (!e.IsSuccess)
        {
            Program.Step("webview-init-event-FAILED");
            ReportStartupError(e.InitializationException
                ?? new InvalidOperationException("Falha desconhecida ao inicializar o WebView2."));
        }
    }

    private void ShowWebError(string message)
    {
        try
        {
            var label = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(24),
                Text = $"{AppTitle}\n\n{message}\n\nDetalhes em:\n{Program.ErrorLog}"
            };
            Controls.Add(label);
            label.BringToFront();
        }
        catch { }
    }

    private void ReportStartupError(Exception ex)
    {
        // Log em local gravavel para diagnostico (o usuario pode enviar o arquivo).
        Program.Fatal(ex);
        ShowWebError(ex.Message);
        try
        {
            MessageBox.Show(
                $"Não foi possível abrir o conteúdo do app.\n\n{ex.Message}\n\n" +
                "Verifique se o Microsoft Edge WebView2 Runtime está instalado.\n" +
                $"Detalhes salvos em: {Program.ErrorLog}",
                AppTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch { }
    }
}
