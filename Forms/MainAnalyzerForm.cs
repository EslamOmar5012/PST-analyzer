using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PstAnalyzer.Models;
using PstAnalyzer.Services;

namespace PstAnalyzer.Forms
{
    public partial class MainAnalyzerForm : Form
    {
        private CancellationTokenSource _cts;
        private MergeReport _lastReport;

        public MainAnalyzerForm()
        {
            InitializeComponent();

            // Default save path
            txtSavePath.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                string.Format("PstReport_{0:yyyyMMdd_HHmmss}.html", DateTime.Now));

            Log("PST Analyzer & Report Tool initialized.");
            Log("Add your source PST files, optionally specify the merged PST, then click Run Analysis.");
        }

        // ── File list management ───────────────────────────────────────────────

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter    = "Outlook Data Files (*.pst)|*.pst";
                ofd.Multiselect = true;
                ofd.Title     = "Select PST Files to Analyze";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var f in ofd.FileNames)
                    {
                        if (!lstSourceFiles.Items.Contains(f))
                        {
                            lstSourceFiles.Items.Add(f);
                            Log("Added: " + Path.GetFileName(f));
                        }
                    }
                }
            }
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select folder containing PST files";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    var files = Directory.GetFiles(fbd.SelectedPath, "*.pst", SearchOption.TopDirectoryOnly);
                    int added = 0;
                    foreach (var f in files)
                    {
                        if (!lstSourceFiles.Items.Contains(f))
                        { lstSourceFiles.Items.Add(f); added++; }
                    }
                    Log(string.Format("Folder scan: {0} PST file(s) added from {1}", added, fbd.SelectedPath));
                    if (files.Length == 0)
                        MessageBox.Show("No PST files found in the selected folder.", "No Files Found",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (lstSourceFiles.SelectedIndex >= 0)
            {
                Log("Removed: " + Path.GetFileName(lstSourceFiles.SelectedItem?.ToString()));
                lstSourceFiles.Items.RemoveAt(lstSourceFiles.SelectedIndex);
            }
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            lstSourceFiles.Items.Clear();
            Log("Source file list cleared.");
        }

        private void btnBrowseMerged_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Outlook Data Files (*.pst)|*.pst";
                ofd.Title  = "Select Merged/Destination PST";
                if (ofd.ShowDialog() == DialogResult.OK)
                    txtMergedPst.Text = ofd.FileName;
            }
        }

        private void chkScanMerged_CheckedChanged(object sender, EventArgs e)
        {
            txtMergedPst.Enabled    = chkScanMerged.Checked;
            btnBrowseMerged.Enabled = chkScanMerged.Checked;
        }

        private void chkSaveReport_CheckedChanged(object sender, EventArgs e)
        {
            txtSavePath.Enabled    = chkSaveReport.Checked;
            btnBrowseSave.Enabled  = chkSaveReport.Checked;
        }

        private void btnBrowseSave_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter      = "HTML Report (*.html)|*.html";
                sfd.FileName    = string.Format("PstReport_{0:yyyyMMdd_HHmmss}.html", DateTime.Now);
                sfd.Title       = "Save Analysis Report";
                if (sfd.ShowDialog() == DialogResult.OK)
                    txtSavePath.Text = sfd.FileName;
            }
        }

        // ── Core analysis ─────────────────────────────────────────────────────

        private async void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (lstSourceFiles.Items.Count == 0)
            {
                MessageBox.Show("Please add at least one source PST file.", "No Files",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mergedPath = (chkScanMerged.Checked && !string.IsNullOrWhiteSpace(txtMergedPst.Text))
                ? txtMergedPst.Text
                : null;

            if (mergedPath != null && !File.Exists(mergedPath))
            {
                if (MessageBox.Show(
                    "The merged PST file was not found:\n" + mergedPath +
                    "\n\nContinue with source analysis only?",
                    "File Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
                mergedPath = null;
            }

            // Build source list
            var sourceFiles = new string[lstSourceFiles.Items.Count];
            for (int i = 0; i < lstSourceFiles.Items.Count; i++)
                sourceFiles[i] = lstSourceFiles.Items[i].ToString();

            // ── UI lock ──
            SetUiBusy(true);
            progressBar.MarqueeAnimationSpeed = 40;
            Log("─────────────────────────────────────────────");
            Log(string.Format("Starting analysis of {0} source file(s)...", sourceFiles.Length));
            if (mergedPath != null) Log("Post-merge scan: " + Path.GetFileName(mergedPath));

            _cts = new CancellationTokenSource();

            try
            {
                var svc = new PstAnalyzerService();
                svc.Progress += (pct, msg) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        Log(msg);
                        lblStatus.Text = msg;
                    }));
                };

                _lastReport = await Task.Run(() =>
                    svc.Analyze(sourceFiles, mergedPath, _cts.Token));

                if (_cts.Token.IsCancellationRequested)
                {
                    Log("CANCELLED by user.");
                    lblStatus.Text = "Analysis cancelled.";
                    return;
                }

                Log("Analysis complete. Generating HTML report...");

                string html = HtmlReportGenerator.Generate(_lastReport);

                // Save to file
                string reportPath = null;
                if (chkSaveReport.Checked && !string.IsNullOrWhiteSpace(txtSavePath.Text))
                {
                    reportPath = txtSavePath.Text;
                    File.WriteAllText(reportPath, html, System.Text.Encoding.UTF8);
                    Log("Report saved: " + reportPath);
                }
                else
                {
                    // Always save to a temp file so we can open it
                    reportPath = Path.Combine(Path.GetTempPath(),
                        string.Format("PstReport_{0:yyyyMMdd_HHmmss}.html", DateTime.Now));
                    File.WriteAllText(reportPath, html, System.Text.Encoding.UTF8);
                }

                // Summary in log
                Log(string.Format("── Source files: {0}", _lastReport.TotalSourceFiles));
                Log(string.Format("── Total items (before): {0:N0}", _lastReport.TotalSourceItems));
                Log(string.Format("── Total emails (before): {0:N0}", _lastReport.TotalSourceEmails));
                Log(string.Format("── Cross-file duplicate groups: {0:N0}", _lastReport.CrossFileDuplicates.Count));
                Log(string.Format("── Total duplicates to remove: {0:N0}", _lastReport.TotalDuplicatesRemoved));
                Log(string.Format("── Deduplication rate: {0:F1}%", _lastReport.DeduplicationRate));
                if (_lastReport.MergedReport != null)
                    Log(string.Format("── Items after merge: {0:N0}", _lastReport.ActualAfterMerge));

                // Open in browser
                if (chkAutoOpen.Checked)
                {
                    Process.Start(new ProcessStartInfo(reportPath) { UseShellExecute = true });
                    Log("Report opened in default browser.");
                }

                lblStatus.Text = "Analysis complete! Report generated.";

                // Offer to view in built-in viewer
                if (MessageBox.Show(
                    "Analysis complete!\n\nReport generated successfully.\n\nOpen the HTML report now?",
                    "Analysis Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!chkAutoOpen.Checked) // don't double-open
                        Process.Start(new ProcessStartInfo(reportPath) { UseShellExecute = true });
                }
            }
            catch (OperationCanceledException)
            {
                Log("Analysis cancelled by user.");
                lblStatus.Text = "Cancelled.";
            }
            catch (Exception ex)
            {
                Log("FATAL ERROR: " + ex.Message);
                lblStatus.Text = "Error occurred — see log.";
                MessageBox.Show("An error occurred during analysis:\n\n" + ex.Message,
                    "Analysis Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetUiBusy(false);
                progressBar.MarqueeAnimationSpeed = 0;
                if (_cts != null) { _cts.Dispose(); _cts = null; }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_cts != null)
            {
                Log("Cancellation requested...");
                _cts.Cancel();
                btnCancel.Enabled = false;
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            string msg =
                "PST Analyzer & Report Tool v1.0\n\n" +
                "Developed by: Eslam Omar 2026\n\n" +
                "This tool provides detailed analytics on PST files\n" +
                "before and after the merge process, including:\n\n" +
                "  • Per-file item counts & type breakdown\n" +
                "  • Folder structure visualization\n" +
                "  • Cross-file duplicate detection\n" +
                "  • Within-file duplicate detection\n" +
                "  • Before vs. After merge comparison\n" +
                "  • Interactive HTML report generation\n\n" +
                "Compatible with Microsoft Outlook 2013 / 2016 / 2019 / 365\n\n" +
                "NOTE: This tool is READ-ONLY and does not modify any PST files.";

            MessageBox.Show(msg, "About PST Analyzer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void SetUiBusy(bool busy)
        {
            btnAnalyze.Enabled   = !busy;
            btnAddFiles.Enabled  = !busy;
            btnAddFolder.Enabled = !busy;
            btnRemoveFile.Enabled = !busy;
            btnClearList.Enabled  = !busy;
            btnCancel.Visible    = busy;
            btnCancel.Enabled    = busy;
            lstSourceFiles.Enabled = !busy;
        }

        private void Log(string message)
        {
            if (txtLog.InvokeRequired) { txtLog.Invoke(new Action(() => Log(message))); return; }
            string line = string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, message);
            txtLog.AppendText(line + Environment.NewLine);
        }
    }
}
