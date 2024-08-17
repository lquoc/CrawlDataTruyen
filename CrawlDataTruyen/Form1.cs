using Common;
using Common.ErrorNovel;
using CrawlDataService;
using CrawlDataService.Service;
using Microsoft.Extensions.DependencyInjection;
using Repository.Enum;
using static Repository.Enum.ListEnum;

namespace CrawlDataTruyen
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            txtLinkCrawl.Text = "/tim-kiem?status=5794f03dd7ced228f4419191&qs=1&m=8&q=&start=20&so=1&y=2024&vo=1";
            txtThreadNumber.Text = RuntimeContext.MaxThread.ToString();
            cbChangeTextIntoVoice.Checked = RuntimeContext.IsChangeTextIntoVoice;
            StartCreateComboxBox();
        }
        void StartCreateComboxBox()
        {
            cboCrawlFromPage.Items.Add(ListEnum.EnumPage.WikiDich);
            cboCrawlFromPage.Items.Add(ListEnum.EnumPage.DTruyen);
            cboCrawlFromPage.SelectedIndex = (int)RuntimeContext.EnumPage;
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RuntimeContext.PathCrawl) || string.IsNullOrEmpty(RuntimeContext.PathSaveLocal))
            {
                MessageBox.Show("Please fill all info in Crawl Data.");
                return;
            }
            if (cbChangeTextIntoVoice.Checked == true)
            {
                if (string.IsNullOrEmpty(RuntimeContext.PathSaveFileMp3))
                {
                    MessageBox.Show("Please fill all info in Create Voice File.");
                    return;
                }
            }
            CrawlNovelSerivce crawlNovelService;
            if(RuntimeContext.EnumPage == EnumPage.WikiDich)
            {
                crawlNovelService = RuntimeContext._serviceProvider.GetRequiredService<CrawlNovelFromWiki>();
            }
            else
            {
                crawlNovelService = RuntimeContext._serviceProvider.GetRequiredService<CrawlNovelFromDTruyen>();
            }
            var getChangeTextToVoice = RuntimeContext._serviceProvider.GetRequiredService<ChangeTextToVoice>();
            Task.Run(async () =>
            {
                await crawlNovelService.StartCrawlData(1, RuntimeContext.PathSaveLocal, RuntimeContext.PathSaveFileMp3, RuntimeContext.PathCrawl);
            });
        }

        private void txtThreadNumber_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtThreadNumber.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                txtThreadNumber.Text = txtThreadNumber.Text.Remove(txtThreadNumber.Text.Length - 1);
                return;
            }
            RuntimeContext.MaxThread = int.Parse(txtThreadNumber.Text);
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
                RuntimeContext.MaxThread = threadNUmber;
            }

            var getDataFromWebService = RuntimeContext._serviceProvider.GetRequiredService<CrawlNovelFromWiki>();
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

        private void txtLinkCrawl_TextChanged(object sender, EventArgs e)
        {
            RuntimeContext.PathCrawl = txtLinkCrawl.Text;
        }

        private void txtFolderPath_TextChanged(object sender, EventArgs e)
        {
            RuntimeContext.PathSaveLocal = txtFolderPath.Text;
        }

        private void cbChangeTextIntoVoice_CheckedChanged(object sender, EventArgs e)
        {
            RuntimeContext.IsChangeTextIntoVoice = cbChangeTextIntoVoice.Checked;
        }

        private void txtPathFolderVoice_TextChanged(object sender, EventArgs e)
        {
            RuntimeContext.PathSaveFileMp3 = txtPathFolderVoice.Text;
        }

        private void cboCrawlFromPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            RuntimeContext.EnumPage = (EnumPage)cboCrawlFromPage.SelectedIndex;
        }
    }
}
