using System;
using System.IO;
using System.Windows.Forms;

namespace PstAnalyzer.Forms
{
    /// <summary>
    /// Optional in-app viewer for the generated HTML report (uses IE WebBrowser control).
    /// Users can also just open the report in their default browser.
    /// </summary>
    public partial class ReportViewerForm : Form
    {
        private string _htmlContent;

        public ReportViewerForm(string htmlContent, string title = "PST Analysis Report")
        {
            InitializeComponent();
            _htmlContent = htmlContent;
            lblTitle.Text = title;
            this.Text = title + " — PST Analyzer";

            // Load HTML into the browser
            webBrowser.DocumentText = htmlContent;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter   = "HTML Report (*.html)|*.html";
                sfd.FileName = string.Format("PstReport_{0:yyyyMMdd_HHmmss}.html", DateTime.Now);
                sfd.Title    = "Save Report As";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(sfd.FileName, _htmlContent, System.Text.Encoding.UTF8);
                        MessageBox.Show("Report saved to:\n" + sfd.FileName, "Saved",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to save: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
