using Common;
using Common.ErrorNovel;
using CrawlDataService.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CrawlDataTruyen
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            txtLinkCrawl.Text = "/tim-kiem?status=5794f03dd7ced228f4419191&qs=1&m=8&q=&start=20&so=1&y=2024&vo=1";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLinkCrawl.Text)) return;
            if (string.IsNullOrEmpty(txtFolderPath.Text)) return;
            if (int.TryParse(txtThreadNumber.Text, out var threadNUmber))
            {
                RuntimeContext.MaxThraed = threadNUmber;
            }
            RuntimeContext.PathSaveLocal = txtFolderPath.Text;

            var getDataFromWebService = RuntimeContext._serviceProvider.GetRequiredService<GetDataFromWebService>();
            Task.Run(async () =>
            {
                await getDataFromWebService.StartCrawlData(1, txtFolderPath.Text, txtLinkCrawl.Text);
            });
        }


        private void txtThreadNumber_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtThreadNumber.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                txtThreadNumber.Text = txtThreadNumber.Text.Remove(txtThreadNumber.Text.Length - 1);
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFolderPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnReCrawl_Click(object sender, EventArgs e)
        {
            var infoLog = ReadFile.ReadFileLog(RuntimeContext.PathChapterError);
            var errorNovel = infoLog.Where(e => e.IsNovelError == true).ToList();
            var errorChaper = infoLog.Where(e => e.IsNovelError == false).ToList();

            if (string.IsNullOrEmpty(txtLinkCrawl.Text)) return;
            if (string.IsNullOrEmpty(txtFolderPath.Text)) return;
            if (int.TryParse(txtThreadNumber.Text, out var threadNUmber))
            {
                RuntimeContext.MaxThraed = threadNUmber;
            }

            var getDataFromWebService = RuntimeContext._serviceProvider.GetRequiredService<GetDataFromWebService>();
            Task.Run(async () =>
            {
                await getDataFromWebService.StartReCrawData(RuntimeContext.numberBatch, errorNovel, errorChaper, txtFolderPath.Text);
            });
        }
    }
}
