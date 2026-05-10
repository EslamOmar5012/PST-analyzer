namespace PstAnalyzer.Forms
{
    partial class ReportViewerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.webBrowser    = new System.Windows.Forms.WebBrowser();
            this.btnClose      = new System.Windows.Forms.Button();
            this.btnSaveAs     = new System.Windows.Forms.Button();
            this.lblTitle      = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.lblTitle.AutoSize  = true;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location  = new System.Drawing.Point(12, 10);
            this.lblTitle.Text      = "PST Analysis Report";

            this.webBrowser.Location = new System.Drawing.Point(0, 36);
            this.webBrowser.Size     = new System.Drawing.Size(1100, 680);
            this.webBrowser.Anchor   = System.Windows.Forms.AnchorStyles.Top |
                                       System.Windows.Forms.AnchorStyles.Bottom |
                                       System.Windows.Forms.AnchorStyles.Left |
                                       System.Windows.Forms.AnchorStyles.Right;
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;

            this.btnSaveAs.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnSaveAs.Location = new System.Drawing.Point(900, 726);
            this.btnSaveAs.Size     = new System.Drawing.Size(90, 28);
            this.btnSaveAs.Text     = "Save As...";
            this.btnSaveAs.Click   += new System.EventHandler(this.btnSaveAs_Click);

            this.btnClose.Anchor    = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.Location  = new System.Drawing.Point(1000, 726);
            this.btnClose.Size      = new System.Drawing.Size(90, 28);
            this.btnClose.Text      = "Close";
            this.btnClose.Click    += new System.EventHandler(this.btnClose_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1100, 762);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.btnClose);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Text        = "PST Analysis Report Viewer";
            this.Font        = new System.Drawing.Font("Segoe UI", 9F);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Button     btnClose;
        private System.Windows.Forms.Button     btnSaveAs;
        private System.Windows.Forms.Label      lblTitle;
    }
}
