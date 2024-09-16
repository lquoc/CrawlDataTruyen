namespace CrawlDataTruyen
{
    partial class FMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            crawlTruyenToolStripMenuItem = new ToolStripMenuItem();
            changeTextToMp4ToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { crawlTruyenToolStripMenuItem, changeTextToMp4ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1383, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // crawlTruyenToolStripMenuItem
            // 
            crawlTruyenToolStripMenuItem.Name = "crawlTruyenToolStripMenuItem";
            crawlTruyenToolStripMenuItem.Size = new Size(103, 24);
            crawlTruyenToolStripMenuItem.Text = "CrawlTruyen";
            crawlTruyenToolStripMenuItem.Click += crawlTruyenToolStripMenuItem_Click;
            // 
            // changeTextToMp4ToolStripMenuItem
            // 
            changeTextToMp4ToolStripMenuItem.Name = "changeTextToMp4ToolStripMenuItem";
            changeTextToMp4ToolStripMenuItem.Size = new Size(188, 24);
            changeTextToMp4ToolStripMenuItem.Text = "Chuyển Ảnh Thành Video";
            // 
            // FMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1383, 516);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "FMain";
            Text = "Main";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem crawlTruyenToolStripMenuItem;
        private ToolStripMenuItem changeTextToMp4ToolStripMenuItem;
    }
}