namespace PstAnalyzer.Forms
{
    partial class MainAnalyzerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpSource         = new System.Windows.Forms.GroupBox();
            this.lstSourceFiles    = new System.Windows.Forms.ListBox();
            this.btnAddFiles       = new System.Windows.Forms.Button();
            this.btnAddFolder      = new System.Windows.Forms.Button();
            this.btnRemoveFile     = new System.Windows.Forms.Button();
            this.btnClearList      = new System.Windows.Forms.Button();
            this.grpMerged         = new System.Windows.Forms.GroupBox();
            this.txtMergedPst      = new System.Windows.Forms.TextBox();
            this.btnBrowseMerged   = new System.Windows.Forms.Button();
            this.chkScanMerged     = new System.Windows.Forms.CheckBox();
            this.grpOptions        = new System.Windows.Forms.GroupBox();
            this.chkAutoOpen       = new System.Windows.Forms.CheckBox();
            this.chkSaveReport     = new System.Windows.Forms.CheckBox();
            this.txtSavePath       = new System.Windows.Forms.TextBox();
            this.btnBrowseSave     = new System.Windows.Forms.Button();
            this.btnAnalyze        = new System.Windows.Forms.Button();
            this.btnCancel         = new System.Windows.Forms.Button();
            this.btnAbout          = new System.Windows.Forms.Button();
            this.progressBar       = new System.Windows.Forms.ProgressBar();
            this.lblStatus         = new System.Windows.Forms.Label();
            this.txtLog            = new System.Windows.Forms.TextBox();
            this.lblLog            = new System.Windows.Forms.Label();
            this.grpSource.SuspendLayout();
            this.grpMerged.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();

            // ── grpSource ──
            this.grpSource.Controls.Add(this.lstSourceFiles);
            this.grpSource.Controls.Add(this.btnAddFiles);
            this.grpSource.Controls.Add(this.btnAddFolder);
            this.grpSource.Controls.Add(this.btnRemoveFile);
            this.grpSource.Controls.Add(this.btnClearList);
            this.grpSource.Location  = new System.Drawing.Point(12, 12);
            this.grpSource.Size      = new System.Drawing.Size(580, 190);
            this.grpSource.Text      = "Source PST Files (to analyze / merge)";
            this.grpSource.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            this.lstSourceFiles.Location = new System.Drawing.Point(10, 22);
            this.lstSourceFiles.Size     = new System.Drawing.Size(455, 155);
            this.lstSourceFiles.Font     = new System.Drawing.Font("Consolas", 8.5F);

            this.btnAddFiles.Location = new System.Drawing.Point(475, 22);
            this.btnAddFiles.Size     = new System.Drawing.Size(95, 26);
            this.btnAddFiles.Text     = "Add Files...";
            this.btnAddFiles.Click   += new System.EventHandler(this.btnAddFiles_Click);

            this.btnAddFolder.Location = new System.Drawing.Point(475, 54);
            this.btnAddFolder.Size     = new System.Drawing.Size(95, 26);
            this.btnAddFolder.Text     = "Add Folder...";
            this.btnAddFolder.Click   += new System.EventHandler(this.btnAddFolder_Click);

            this.btnRemoveFile.Location = new System.Drawing.Point(475, 86);
            this.btnRemoveFile.Size     = new System.Drawing.Size(95, 26);
            this.btnRemoveFile.Text     = "Remove";
            this.btnRemoveFile.Click   += new System.EventHandler(this.btnRemoveFile_Click);

            this.btnClearList.Location = new System.Drawing.Point(475, 118);
            this.btnClearList.Size     = new System.Drawing.Size(95, 26);
            this.btnClearList.Text     = "Clear All";
            this.btnClearList.Click   += new System.EventHandler(this.btnClearList_Click);

            // ── grpMerged ──
            this.grpMerged.Controls.Add(this.txtMergedPst);
            this.grpMerged.Controls.Add(this.btnBrowseMerged);
            this.grpMerged.Controls.Add(this.chkScanMerged);
            this.grpMerged.Location = new System.Drawing.Point(12, 212);
            this.grpMerged.Size     = new System.Drawing.Size(580, 70);
            this.grpMerged.Text     = "Merged / Destination PST (optional — for post-merge analysis)";
            this.grpMerged.Font     = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            this.chkScanMerged.Location = new System.Drawing.Point(10, 22);
            this.chkScanMerged.Size     = new System.Drawing.Size(130, 20);
            this.chkScanMerged.Text     = "Include in report";
            this.chkScanMerged.Checked  = true;
            this.chkScanMerged.Font     = new System.Drawing.Font("Segoe UI", 9F);
            this.chkScanMerged.CheckedChanged += new System.EventHandler(this.chkScanMerged_CheckedChanged);

            this.txtMergedPst.Location = new System.Drawing.Point(145, 20);
            this.txtMergedPst.Size     = new System.Drawing.Size(330, 22);
            this.txtMergedPst.Font     = new System.Drawing.Font("Consolas", 8.5F);

            this.btnBrowseMerged.Location = new System.Drawing.Point(481, 19);
            this.btnBrowseMerged.Size     = new System.Drawing.Size(88, 24);
            this.btnBrowseMerged.Text     = "Browse...";
            this.btnBrowseMerged.Click   += new System.EventHandler(this.btnBrowseMerged_Click);

            // ── grpOptions ──
            this.grpOptions.Controls.Add(this.chkAutoOpen);
            this.grpOptions.Controls.Add(this.chkSaveReport);
            this.grpOptions.Controls.Add(this.txtSavePath);
            this.grpOptions.Controls.Add(this.btnBrowseSave);
            this.grpOptions.Location = new System.Drawing.Point(12, 292);
            this.grpOptions.Size     = new System.Drawing.Size(580, 75);
            this.grpOptions.Text     = "Report Options";
            this.grpOptions.Font     = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            this.chkAutoOpen.Location = new System.Drawing.Point(10, 22);
            this.chkAutoOpen.Size     = new System.Drawing.Size(160, 20);
            this.chkAutoOpen.Text     = "Open report in browser";
            this.chkAutoOpen.Checked  = true;
            this.chkAutoOpen.Font     = new System.Drawing.Font("Segoe UI", 9F);

            this.chkSaveReport.Location = new System.Drawing.Point(10, 46);
            this.chkSaveReport.Size     = new System.Drawing.Size(130, 20);
            this.chkSaveReport.Text     = "Save report to:";
            this.chkSaveReport.Checked  = true;
            this.chkSaveReport.Font     = new System.Drawing.Font("Segoe UI", 9F);
            this.chkSaveReport.CheckedChanged += new System.EventHandler(this.chkSaveReport_CheckedChanged);

            this.txtSavePath.Location = new System.Drawing.Point(145, 44);
            this.txtSavePath.Size     = new System.Drawing.Size(330, 22);
            this.txtSavePath.Font     = new System.Drawing.Font("Consolas", 8.5F);

            this.btnBrowseSave.Location = new System.Drawing.Point(481, 43);
            this.btnBrowseSave.Size     = new System.Drawing.Size(88, 24);
            this.btnBrowseSave.Text     = "Browse...";
            this.btnBrowseSave.Click   += new System.EventHandler(this.btnBrowseSave_Click);

            // ── Action buttons ──
            this.btnAnalyze.BackColor = System.Drawing.Color.FromArgb(63, 185, 80);
            this.btnAnalyze.ForeColor = System.Drawing.Color.Black;
            this.btnAnalyze.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAnalyze.Location  = new System.Drawing.Point(12, 378);
            this.btnAnalyze.Size      = new System.Drawing.Size(160, 34);
            this.btnAnalyze.Text      = "&#9658; Run Analysis";
            this.btnAnalyze.Text      = "▶ Run Analysis";
            this.btnAnalyze.UseVisualStyleBackColor = false;
            this.btnAnalyze.Click    += new System.EventHandler(this.btnAnalyze_Click);

            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(215, 58, 73);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location  = new System.Drawing.Point(182, 378);
            this.btnCancel.Size      = new System.Drawing.Size(90, 34);
            this.btnCancel.Text      = "Cancel";
            this.btnCancel.Visible   = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click    += new System.EventHandler(this.btnCancel_Click);

            this.btnAbout.Location = new System.Drawing.Point(502, 378);
            this.btnAbout.Size     = new System.Drawing.Size(90, 34);
            this.btnAbout.Text     = "About";
            this.btnAbout.Click   += new System.EventHandler(this.btnAbout_Click);

            // ── Progress ──
            this.progressBar.Location = new System.Drawing.Point(12, 422);
            this.progressBar.Size     = new System.Drawing.Size(580, 18);
            this.progressBar.Style    = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.MarqueeAnimationSpeed = 0;

            this.lblStatus.Location  = new System.Drawing.Point(12, 443);
            this.lblStatus.Size      = new System.Drawing.Size(580, 18);
            this.lblStatus.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Text      = "Ready. Add source PST files and click Run Analysis.";

            // ── Log ──
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(12, 465);
            this.lblLog.Text     = "Activity Log:";
            this.lblLog.Font     = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);

            this.txtLog.BackColor  = System.Drawing.Color.Black;
            this.txtLog.ForeColor  = System.Drawing.Color.Lime;
            this.txtLog.Font       = new System.Drawing.Font("Consolas", 8.5F);
            this.txtLog.Location   = new System.Drawing.Point(12, 483);
            this.txtLog.Multiline  = true;
            this.txtLog.ReadOnly   = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size       = new System.Drawing.Size(580, 130);

            // ── MainForm ──
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize    = new System.Drawing.Size(608, 628);
            this.Controls.Add(this.grpSource);
            this.Controls.Add(this.grpMerged);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.txtLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text        = "PST Analyzer & Report Tool v1.0";
            this.Font        = new System.Drawing.Font("Segoe UI", 9F);
            this.grpSource.ResumeLayout(false);
            this.grpMerged.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ── Controls ──────────────────────────────────────────────────────────
        private System.Windows.Forms.GroupBox   grpSource;
        private System.Windows.Forms.ListBox    lstSourceFiles;
        private System.Windows.Forms.Button     btnAddFiles;
        private System.Windows.Forms.Button     btnAddFolder;
        private System.Windows.Forms.Button     btnRemoveFile;
        private System.Windows.Forms.Button     btnClearList;
        private System.Windows.Forms.GroupBox   grpMerged;
        private System.Windows.Forms.TextBox    txtMergedPst;
        private System.Windows.Forms.Button     btnBrowseMerged;
        private System.Windows.Forms.CheckBox   chkScanMerged;
        private System.Windows.Forms.GroupBox   grpOptions;
        private System.Windows.Forms.CheckBox   chkAutoOpen;
        private System.Windows.Forms.CheckBox   chkSaveReport;
        private System.Windows.Forms.TextBox    txtSavePath;
        private System.Windows.Forms.Button     btnBrowseSave;
        private System.Windows.Forms.Button     btnAnalyze;
        private System.Windows.Forms.Button     btnCancel;
        private System.Windows.Forms.Button     btnAbout;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label      lblStatus;
        private System.Windows.Forms.TextBox    txtLog;
        private System.Windows.Forms.Label      lblLog;
    }
}
