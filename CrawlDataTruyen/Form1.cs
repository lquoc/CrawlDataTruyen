using Common;
using Common.ErrorNovel;
using CrawlDataService;
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
            txtThreadNumber.Text = RuntimeContext.MaxThraed.ToString();
            cbChangeTextIntoVoice.Checked = RuntimeContext.IsChangeTextIntoVoice;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLinkCrawl.Text) || string.IsNullOrEmpty(txtFolderPath.Text))
            {
                MessageBox.Show("Please fill all info in Crawl Data.");
                return;
            }
            if (cbChangeTextIntoVoice.Checked == true)
            {
                if (string.IsNullOrEmpty(txtPathFolderVoice.Text)) 
                {
                    MessageBox.Show("Please fill all info in Create Voice File.");
                    return;
                }
            }

            if (int.TryParse(txtThreadNumber.Text, out var threadNUmber))
            {
                RuntimeContext.MaxThraed = threadNUmber;
            }
            RuntimeContext.PathSaveLocal = txtFolderPath.Text;

            RuntimeContext.IsChangeTextIntoVoice = cbChangeTextIntoVoice.Checked;
            RuntimeContext.PathSaveFileMp3 = txtPathFolderVoice.Text;

            var getDataFromWebService = RuntimeContext._serviceProvider.GetRequiredService<GetDataFromWebService>();
            var getChangeTextToVoice = RuntimeContext._serviceProvider.GetRequiredService<ChangeTextToVoice>();
            Task.Run(async () =>
            {
                await getDataFromWebService.StartCrawlData(1, txtFolderPath.Text, txtPathFolderVoice.Text, txtLinkCrawl.Text);
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
            if (string.IsNullOrEmpty(txtPathFolderVoice.Text)) return;

            RuntimeContext.IsChangeTextIntoVoice = cbChangeTextIntoVoice.Checked;
            RuntimeContext.PathSaveFileMp3 = txtPathFolderVoice.Text;

            if (int.TryParse(txtThreadNumber.Text, out var threadNUmber))
            {
                RuntimeContext.MaxThraed = threadNUmber;
            }

            var getDataFromWebService = RuntimeContext._serviceProvider.GetRequiredService<GetDataFromWebService>();
            var getChangeTextToVoice = RuntimeContext._serviceProvider.GetRequiredService<ChangeTextToVoice>();
            Task.Run(async () =>
            {
                await getDataFromWebService.StartReCrawData(RuntimeContext.numberBatch, errorNovel, errorChaper, txtFolderPath.Text, txtPathFolderVoice.Text);
            });
        }

        private void txtChoiceFolderVoice_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPathFolderVoice.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
