
namespace CrawlDataTruyen
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
        }

        protected void crawlTruyenToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            if (!this.MdiChildren.Where(e => e.Name.Equals(nameof(FCrawlTruyen))).Any())
            {
                FCrawlTruyen crawlTruyeForm = new FCrawlTruyen();
                crawlTruyeForm.MdiParent = this;
                crawlTruyeForm.FormBorderStyle = FormBorderStyle.None;
                crawlTruyeForm.Size = this.Size;
                crawlTruyeForm.Show();
                var trets = this.MdiChildren;
            }
        }
    }
}
