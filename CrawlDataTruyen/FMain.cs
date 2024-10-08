
namespace CrawlDataTruyen
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
            FCrawlTruyen fCrawlTruyenForm = new FCrawlTruyen();
            fCrawlTruyenForm.Size = this.ClientSize;
            fCrawlTruyenForm.MdiParent = this;
            fCrawlTruyenForm.Location = new Point(0, 0);
            fCrawlTruyenForm.Show();
        }

        protected void crawlTruyenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.MdiChildren.Where(e => e.GetType() == typeof(FCrawlTruyen)).Any())
            {
                FCrawlTruyen crawlTruyeForm = new FCrawlTruyen();
                crawlTruyeForm.MdiParent = this;
                crawlTruyeForm.Location = new Point(0, 0);
                crawlTruyeForm.Show();
            }
            else
            {
                var fcrawlTruyen = this.MdiChildren.FirstOrDefault(e => e.GetType() == typeof(FCrawlTruyen));
                fcrawlTruyen?.BringToFront();
            }
        }

        private void changeTextToMp4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.MdiChildren.Where(e => e.GetType() == typeof(FChangeImageToVoice)).Any())
            {
                FChangeImageToVoice fChangeImage = new FChangeImageToVoice();
                fChangeImage.MdiParent = this;
                fChangeImage.Location = new Point(0,0);
                fChangeImage.Show();
            }
            else
            {
                var fChangeImage = this.MdiChildren.FirstOrDefault(e => e.GetType() == typeof(FChangeImageToVoice));
                fChangeImage?.BringToFront();
            }
        }
    }
}
