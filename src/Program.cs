using System.IO;

namespace AcompanhamentoConsultas;

static class Program
{
    // Log de diagnóstico: prova até onde a inicialização chegou, mesmo se a
    // janela nunca aparecer (caso "tela verde" da Store). Fica em pasta gravável.
    public static string DiagDir =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "wavizo.SlSSAcompanhamentodeconsultas");

    public static string StepLog => Path.Combine(DiagDir, "startup.log");
    public static string ErrorLog => Path.Combine(DiagDir, "WebView2", "startup-error.log");

    public static void Step(string message)
    {
        try
        {
            Directory.CreateDirectory(DiagDir);
            File.AppendAllText(StepLog, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
        }
        catch { /* log nunca pode travar o app */ }
    }

    public static void Fatal(Exception ex)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ErrorLog)!);
            File.WriteAllText(ErrorLog, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex}");
        }
        catch { }
    }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Step("Main enter");
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            Step("FATAL unhandled: " + e.ExceptionObject?.GetType().FullName);
            if (e.ExceptionObject is Exception ex) Fatal(ex);
        };
        try
        {
            Step("config-initialize");
            ApplicationConfiguration.Initialize();
            Step("form-create");
            var form = new Form1();
            Step("application-run");
            Application.Run(form);
            Step("exit-ok");
        }
        catch (Exception ex)
        {
            Step("STARTUP-EX: " + ex.GetType().FullName + ": " + ex.Message);
            Fatal(ex);
            try
            {
                MessageBox.Show(
                    $"O aplicativo não conseguiu iniciar.\n\n{ex.Message}\n\nDetalhes salvos em:\n{ErrorLog}",
                    "SlSS Acompanhamento de consultas",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { }
        }
    }
}
