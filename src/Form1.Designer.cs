namespace AcompanhamentoConsultas;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private Microsoft.Web.WebView2.WinForms.WebView2 webView21;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
        ((System.ComponentModel.ISupportInitialize)(this.webView21)).BeginInit();
        this.SuspendLayout();
        //
        // webView21
        //
        this.webView21.AllowExternalDrop = false;
        this.webView21.CreationProperties = null;
        this.webView21.Dock = System.Windows.Forms.DockStyle.Fill;
        this.webView21.Location = new System.Drawing.Point(0, 0);
        this.webView21.Name = "webView21";
        this.webView21.Size = new System.Drawing.Size(1280, 720);
        this.webView21.TabIndex = 0;
        this.webView21.ZoomFactor = 1D;
        this.webView21.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.WebView21_CoreWebView2InitializationCompleted);
        //
        // Form1
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1280, 720);
        this.Controls.Add(this.webView21);
        this.MinimumSize = new System.Drawing.Size(800, 600);
        this.Name = "Form1";
        this.Text = "SlSS Acompanhamento de consultas";
        this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        ((System.ComponentModel.ISupportInitialize)(this.webView21)).EndInit();
        this.ResumeLayout(false);
    }

    #endregion
}
