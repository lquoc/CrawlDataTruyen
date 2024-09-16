﻿using Common;
using CrawlDataService.Common;
using Microsoft.ClearScript.JavaScript;
using Microsoft.Extensions.DependencyInjection;
using static Repository.Enum.ListEnum;

namespace CrawlDataTruyen
{
    public partial class FCrawlTruyen : Form
    {

        public FCrawlTruyen()
        {
            InitializeComponent();
            txtLinkCrawl.Text = "https://truyenqqto.com/truyen-tranh/tinh-yeu-cua-chieri-cao-toi-tan-8-met-13285";

            txtThreadNumber.Text = RuntimeContext.MaxThread.ToString();
            cbChangeTextIntoVoice.Checked = RuntimeContext.IsChangeTextIntoVoice;
            radibtnMp3.Checked = RuntimeContext.TypeFile == TypeFile.MP3;
            radioMultiNovel.Checked = RuntimeContext.TypeCrawl == TypeCrawl.MultiNovel;
            txtKeyAPIProxy.Enabled = false;
            StartCreateComboxBox();
            //InitRickTextBox();
        }
        //void InitRickTextBox()
        //{
        //    var consoleOutput = new ConsoleOutput(rtbConsole);
        //    Console.SetOut(consoleOutput);
        //    rtbConsole.ReadOnly = true;
        //}

        void StartCreateComboxBox()
        {
            var listEnum = Enum.GetValues(typeof(NovelWeb)).ToEnumerable().Select(e => e.ToString()).ToArray();
            if (listEnum != null)
            {
                cboCrawlFromPage.Items.AddRange(listEnum);
                cboCrawlFromPage.SelectedIndex = (int)RuntimeContext.EnumWeb;
            }
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

            RuntimeContext.IsStart = true;
            ChangeEnableGroupBox(false);
            var managerService = RuntimeContext._serviceProvider.GetRequiredService<ManagerService>();
            Task.Run(async () =>
            {
                await managerService.StartNovelService(ChangeEnableGroupBox);
            });
        }

        private void ChangeEnableGroupBox(bool enable)
        {
            groupCrawlDataInfo.Invoke(new MethodInvoker(delegate
            {
                groupCrawlDataInfo.Enabled = enable;
            }));
            groupChangeText.Invoke(new MethodInvoker(delegate
            {
                groupChangeText.Enabled = enable;
            }));
            btn_Start.Invoke(new MethodInvoker(delegate
            {
                btn_Start.Enabled = enable;
            }));
        }

        private void txtThreadNumber_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtThreadNumber.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                txtThreadNumber.Text = txtThreadNumber.Text.Remove(txtThreadNumber.Text.Length - 1);
                return;
            }
            if (!string.IsNullOrEmpty(txtThreadNumber.Text))
            {
                RuntimeContext.MaxThread = int.Parse(txtThreadNumber.Text);
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

        //private void btnReCrawl_Click(object sender, EventArgs e)
        //{
        //    var infoLog = ReadFile.ReadFileLog(RuntimeContext.PathChapterError);
        //    var errorNovel = infoLog.Where(e => e.IsNovelError == true).ToList();
        //    var errorChaper = infoLog.Where(e => e.IsNovelError == false).ToList();

        //    if (string.IsNullOrEmpty(txtLinkCrawl.Text)) return;
        //    if (string.IsNullOrEmpty(txtFolderPath.Text)) return;
        //    if (string.IsNullOrEmpty(txtPathFolderVoice.Text)) return;

        //    RuntimeContext.IsChangeTextIntoVoice = cbChangeTextIntoVoice.Checked;
        //    RuntimeContext.PathSaveFileMp3 = txtPathFolderVoice.Text;

        //    if (int.TryParse(txtThreadNumber.Text, out var threadNUmber))
        //    {
        //        RuntimeContext.MaxThread = threadNUmber;
        //    }

        //    var getDataFromWebService = RuntimeContext._serviceProvider.GetRequiredService<CrawlNovelFromWiki>();
        //    var getChangeTextToVoice = RuntimeContext._serviceProvider.GetRequiredService<ChangeTextToVoice>();
        //    Task.Run(async () =>
        //    {
        //        await getDataFromWebService.StartReCrawData(RuntimeContext.numberBatch, errorNovel, errorChaper, txtFolderPath.Text, txtPathFolderVoice.Text);
        //    });
        //}

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
            RuntimeContext.EnumWeb = (NovelWeb)cboCrawlFromPage.SelectedIndex;
        }

        private void radibtnMp3_CheckedChanged(object sender, EventArgs e)
        {
            RuntimeContext.TypeFile = TypeFile.MP3;
            txtThreadNumber.Enabled = true;
        }

        private void radiobtnMP4_CheckedChanged(object sender, EventArgs e)
        {
            RuntimeContext.TypeFile = TypeFile.MP4;
            txtThreadNumber.Enabled = false;
            txtThreadNumber.Text = "1";
        }

        private void radiobtnOneNovel_CheckedChanged(object sender, EventArgs e)
        {
            RuntimeContext.TypeCrawl = TypeCrawl.OneNovel;
        }

        private void NhiềuMultiNovel_CheckedChanged(object sender, EventArgs e)
        {
            RuntimeContext.TypeCrawl = TypeCrawl.MultiNovel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RuntimeContext.IsStart = false;
            groupCrawlDataInfo.Enabled = true;
            groupChangeText.Enabled = true;
            btn_Start.Enabled = true;
        }

        private void cbChangeProxy_CheckedChanged(object sender, EventArgs e)
        {
            txtKeyAPIProxy.Enabled = cbChangeProxy.Checked;
            RuntimeContext.IsChangeProxy = true;
        }

        private void txtKeyAPIProxy_TextChanged(object sender, EventArgs e)
        {
            RuntimeContext.ProxyKey = txtKeyAPIProxy.Text;
        }

    }
}
