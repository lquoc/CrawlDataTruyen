namespace CrawlDataTruyen
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_Start = new Button();
            label1 = new Label();
            txtLinkCrawl = new TextBox();
            label2 = new Label();
            txtThreadNumber = new TextBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            label3 = new Label();
            btnFolder = new Button();
            txtFolderPath = new TextBox();
            btnReCrawl = new Button();
            cbChangeTextIntoVoice = new CheckBox();
            label5 = new Label();
            txtPathFolderVoice = new TextBox();
            btnChoiceFolderVoice = new Button();
            cboCrawlFromPage = new ComboBox();
            label4 = new Label();
            SuspendLayout();
            // 
            // btn_Start
            // 
            btn_Start.Location = new Point(466, 240);
            btn_Start.Name = "btn_Start";
            btn_Start.Size = new Size(94, 29);
            btn_Start.TabIndex = 0;
            btn_Start.Text = "Start";
            btn_Start.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn_Start.UseVisualStyleBackColor = true;
            btn_Start.Click += btnStart_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 68);
            label1.Name = "label1";
            label1.Size = new Size(72, 20);
            label1.TabIndex = 1;
            label1.Text = "LinkCrawl";
            // 
            // txtLinkCrawl
            // 
            txtLinkCrawl.Location = new Point(120, 63);
            txtLinkCrawl.Name = "txtLinkCrawl";
            txtLinkCrawl.PlaceholderText = "Link Crawl";
            txtLinkCrawl.Size = new Size(293, 27);
            txtLinkCrawl.TabIndex = 2;
            txtLinkCrawl.TextChanged += txtLinkCrawl_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 129);
            label2.Name = "label2";
            label2.Size = new Size(55, 20);
            label2.TabIndex = 3;
            label2.Text = "Thread";
            // 
            // txtThreadNumber
            // 
            txtThreadNumber.Location = new Point(120, 124);
            txtThreadNumber.Name = "txtThreadNumber";
            txtThreadNumber.PlaceholderText = "Số luồng";
            txtThreadNumber.Size = new Size(125, 27);
            txtThreadNumber.TabIndex = 4;
            txtThreadNumber.TextChanged += txtThreadNumber_TextChanged;
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.HelpRequest += folderBrowserDialog1_HelpRequest;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 180);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 5;
            label3.Text = "Save Path";
            // 
            // btnFolder
            // 
            btnFolder.Location = new Point(419, 171);
            btnFolder.Name = "btnFolder";
            btnFolder.Size = new Size(128, 29);
            btnFolder.TabIndex = 6;
            btnFolder.Text = "Choice Folder";
            btnFolder.UseVisualStyleBackColor = true;
            btnFolder.Click += btnFolder_Click;
            // 
            // txtFolderPath
            // 
            txtFolderPath.Location = new Point(120, 173);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.PlaceholderText = "Path Folder";
            txtFolderPath.Size = new Size(293, 27);
            txtFolderPath.TabIndex = 7;
            txtFolderPath.TextChanged += txtFolderPath_TextChanged;
            // 
            // btnReCrawl
            // 
            btnReCrawl.Location = new Point(588, 240);
            btnReCrawl.Name = "btnReCrawl";
            btnReCrawl.Size = new Size(94, 29);
            btnReCrawl.TabIndex = 8;
            btnReCrawl.Text = "ReCrawl";
            btnReCrawl.UseVisualStyleBackColor = true;
            btnReCrawl.Click += btnReCrawl_Click;
            // 
            // cbChangeTextIntoVoice
            // 
            cbChangeTextIntoVoice.AutoSize = true;
            cbChangeTextIntoVoice.Location = new Point(588, 20);
            cbChangeTextIntoVoice.Name = "cbChangeTextIntoVoice";
            cbChangeTextIntoVoice.Size = new Size(251, 24);
            cbChangeTextIntoVoice.TabIndex = 9;
            cbChangeTextIntoVoice.Text = "Chuyển Văn Bản Thành Âm Thanh";
            cbChangeTextIntoVoice.UseVisualStyleBackColor = true;
            cbChangeTextIntoVoice.CheckedChanged += cbChangeTextIntoVoice_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(588, 71);
            label5.Name = "label5";
            label5.Size = new Size(72, 20);
            label5.TabIndex = 12;
            label5.Text = "Save Path";
            // 
            // txtPathFolderVoice
            // 
            txtPathFolderVoice.Location = new Point(669, 64);
            txtPathFolderVoice.Name = "txtPathFolderVoice";
            txtPathFolderVoice.PlaceholderText = "Path Folder";
            txtPathFolderVoice.Size = new Size(276, 27);
            txtPathFolderVoice.TabIndex = 13;
            txtPathFolderVoice.TextChanged += txtPathFolderVoice_TextChanged;
            // 
            // btnChoiceFolderVoice
            // 
            btnChoiceFolderVoice.Location = new Point(951, 62);
            btnChoiceFolderVoice.Name = "btnChoiceFolderVoice";
            btnChoiceFolderVoice.Size = new Size(116, 29);
            btnChoiceFolderVoice.TabIndex = 14;
            btnChoiceFolderVoice.Text = "Choice Folder";
            btnChoiceFolderVoice.UseVisualStyleBackColor = true;
            btnChoiceFolderVoice.Click += txtChoiceFolderVoice_Click;
            // 
            // cboCrawlFromPage
            // 
            cboCrawlFromPage.FormattingEnabled = true;
            cboCrawlFromPage.Location = new Point(120, 12);
            cboCrawlFromPage.Name = "cboCrawlFromPage";
            cboCrawlFromPage.Size = new Size(151, 28);
            cboCrawlFromPage.TabIndex = 15;
            cboCrawlFromPage.SelectedIndexChanged += cboCrawlFromPage_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(24, 20);
            label4.Name = "label4";
            label4.Size = new Size(84, 20);
            label4.TabIndex = 16;
            label4.Text = "Crawl From";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1093, 343);
            Controls.Add(label4);
            Controls.Add(cboCrawlFromPage);
            Controls.Add(btnChoiceFolderVoice);
            Controls.Add(txtPathFolderVoice);
            Controls.Add(label5);
            Controls.Add(cbChangeTextIntoVoice);
            Controls.Add(btnReCrawl);
            Controls.Add(txtFolderPath);
            Controls.Add(btnFolder);
            Controls.Add(label3);
            Controls.Add(txtThreadNumber);
            Controls.Add(label2);
            Controls.Add(txtLinkCrawl);
            Controls.Add(label1);
            Controls.Add(btn_Start);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_Start;
        private Label label1;
        private TextBox txtLinkCrawl;
        private Label label2;
        private TextBox txtThreadNumber;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label3;
        private Button btnFolder;
        private TextBox txtFolderPath;
        private Button btnReCrawl;
        private CheckBox cbChangeTextIntoVoice;
        private Label label5;
        private TextBox txtPathFolderVoice;
        private Button btnChoiceFolderVoice;
        private ComboBox cboCrawlFromPage;
        private Label label4;
    }
}
