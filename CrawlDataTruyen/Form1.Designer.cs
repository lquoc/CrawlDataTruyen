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
            cbChangeTextIntoVoice = new CheckBox();
            label5 = new Label();
            txtPathFolderVoice = new TextBox();
            btnChoiceFolderVoice = new Button();
            cboCrawlFromPage = new ComboBox();
            label4 = new Label();
            radibtnMp3 = new RadioButton();
            radiobtnMP4 = new RadioButton();
            groupChangeText = new GroupBox();
            groupCrawlDataInfo = new GroupBox();
            cbChangeProxy = new CheckBox();
            txtKeyAPIProxy = new TextBox();
            radioMultiNovel = new RadioButton();
            radiobtnOneNovel = new RadioButton();
            btnCancel = new Button();
            groupChangeText.SuspendLayout();
            groupCrawlDataInfo.SuspendLayout();
            SuspendLayout();
            // 
            // btn_Start
            // 
            btn_Start.Location = new Point(472, 381);
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
            label1.Location = new Point(17, 111);
            label1.Name = "label1";
            label1.Size = new Size(72, 20);
            label1.TabIndex = 1;
            label1.Text = "LinkCrawl";
            // 
            // txtLinkCrawl
            // 
            txtLinkCrawl.Location = new Point(113, 106);
            txtLinkCrawl.Name = "txtLinkCrawl";
            txtLinkCrawl.PlaceholderText = "Link Crawl";
            txtLinkCrawl.Size = new Size(293, 27);
            txtLinkCrawl.TabIndex = 2;
            txtLinkCrawl.TextChanged += txtLinkCrawl_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 172);
            label2.Name = "label2";
            label2.Size = new Size(55, 20);
            label2.TabIndex = 3;
            label2.Text = "Thread";
            // 
            // txtThreadNumber
            // 
            txtThreadNumber.Location = new Point(113, 167);
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
            label3.Location = new Point(17, 223);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 5;
            label3.Text = "Save Path";
            // 
            // btnFolder
            // 
            btnFolder.Location = new Point(412, 214);
            btnFolder.Name = "btnFolder";
            btnFolder.Size = new Size(128, 29);
            btnFolder.TabIndex = 6;
            btnFolder.Text = "Choice Folder";
            btnFolder.UseVisualStyleBackColor = true;
            btnFolder.Click += btnFolder_Click;
            // 
            // txtFolderPath
            // 
            txtFolderPath.Location = new Point(113, 216);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.PlaceholderText = "Path Folder";
            txtFolderPath.Size = new Size(293, 27);
            txtFolderPath.TabIndex = 7;
            txtFolderPath.TextChanged += txtFolderPath_TextChanged;
            // 
            // cbChangeTextIntoVoice
            // 
            cbChangeTextIntoVoice.AutoSize = true;
            cbChangeTextIntoVoice.Location = new Point(8, 60);
            cbChangeTextIntoVoice.Name = "cbChangeTextIntoVoice";
            cbChangeTextIntoVoice.Size = new Size(180, 24);
            cbChangeTextIntoVoice.TabIndex = 9;
            cbChangeTextIntoVoice.Text = "Chuyển Văn Bản Thành";
            cbChangeTextIntoVoice.UseVisualStyleBackColor = true;
            cbChangeTextIntoVoice.CheckedChanged += cbChangeTextIntoVoice_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 111);
            label5.Name = "label5";
            label5.Size = new Size(72, 20);
            label5.TabIndex = 12;
            label5.Text = "Save Path";
            // 
            // txtPathFolderVoice
            // 
            txtPathFolderVoice.Location = new Point(89, 104);
            txtPathFolderVoice.Name = "txtPathFolderVoice";
            txtPathFolderVoice.PlaceholderText = "Path Folder";
            txtPathFolderVoice.Size = new Size(276, 27);
            txtPathFolderVoice.TabIndex = 13;
            txtPathFolderVoice.TextChanged += txtPathFolderVoice_TextChanged;
            // 
            // btnChoiceFolderVoice
            // 
            btnChoiceFolderVoice.Location = new Point(371, 102);
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
            cboCrawlFromPage.Location = new Point(113, 55);
            cboCrawlFromPage.Name = "cboCrawlFromPage";
            cboCrawlFromPage.Size = new Size(151, 28);
            cboCrawlFromPage.TabIndex = 15;
            cboCrawlFromPage.SelectedIndexChanged += cboCrawlFromPage_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 63);
            label4.Name = "label4";
            label4.Size = new Size(84, 20);
            label4.TabIndex = 16;
            label4.Text = "Crawl From";
            // 
            // radibtnMp3
            // 
            radibtnMp3.AutoSize = true;
            radibtnMp3.Location = new Point(197, 60);
            radibtnMp3.Name = "radibtnMp3";
            radibtnMp3.Size = new Size(59, 24);
            radibtnMp3.TabIndex = 17;
            radibtnMp3.TabStop = true;
            radibtnMp3.Text = "MP3";
            radibtnMp3.UseVisualStyleBackColor = true;
            radibtnMp3.CheckedChanged += radibtnMp3_CheckedChanged;
            // 
            // radiobtnMP4
            // 
            radiobtnMP4.AutoSize = true;
            radiobtnMP4.Location = new Point(262, 60);
            radiobtnMP4.Name = "radiobtnMP4";
            radiobtnMP4.Size = new Size(59, 24);
            radiobtnMP4.TabIndex = 18;
            radiobtnMP4.TabStop = true;
            radiobtnMP4.Text = "MP4";
            radiobtnMP4.UseVisualStyleBackColor = true;
            radiobtnMP4.CheckedChanged += radiobtnMP4_CheckedChanged;
            // 
            // groupChangeText
            // 
            groupChangeText.Controls.Add(radiobtnMP4);
            groupChangeText.Controls.Add(btnChoiceFolderVoice);
            groupChangeText.Controls.Add(radibtnMp3);
            groupChangeText.Controls.Add(cbChangeTextIntoVoice);
            groupChangeText.Controls.Add(label5);
            groupChangeText.Controls.Add(txtPathFolderVoice);
            groupChangeText.Location = new Point(594, 24);
            groupChangeText.Name = "groupChangeText";
            groupChangeText.Size = new Size(487, 339);
            groupChangeText.TabIndex = 19;
            groupChangeText.TabStop = false;
            groupChangeText.Text = "Change Text To";
            // 
            // groupCrawlDataInfo
            // 
            groupCrawlDataInfo.Controls.Add(cbChangeProxy);
            groupCrawlDataInfo.Controls.Add(txtKeyAPIProxy);
            groupCrawlDataInfo.Controls.Add(radioMultiNovel);
            groupCrawlDataInfo.Controls.Add(radiobtnOneNovel);
            groupCrawlDataInfo.Controls.Add(label4);
            groupCrawlDataInfo.Controls.Add(btnFolder);
            groupCrawlDataInfo.Controls.Add(cboCrawlFromPage);
            groupCrawlDataInfo.Controls.Add(label1);
            groupCrawlDataInfo.Controls.Add(txtLinkCrawl);
            groupCrawlDataInfo.Controls.Add(txtFolderPath);
            groupCrawlDataInfo.Controls.Add(label2);
            groupCrawlDataInfo.Controls.Add(txtThreadNumber);
            groupCrawlDataInfo.Controls.Add(label3);
            groupCrawlDataInfo.Location = new Point(26, 24);
            groupCrawlDataInfo.Name = "groupCrawlDataInfo";
            groupCrawlDataInfo.Size = new Size(549, 339);
            groupCrawlDataInfo.TabIndex = 20;
            groupCrawlDataInfo.TabStop = false;
            groupCrawlDataInfo.Text = "Crawl Data Info";
            // 
            // cbChangeProxy
            // 
            cbChangeProxy.AutoSize = true;
            cbChangeProxy.Location = new Point(17, 273);
            cbChangeProxy.Name = "cbChangeProxy";
            cbChangeProxy.Size = new Size(121, 24);
            cbChangeProxy.TabIndex = 21;
            cbChangeProxy.Text = "Change Proxy";
            cbChangeProxy.UseVisualStyleBackColor = true;
            cbChangeProxy.CheckedChanged += cbChangeProxy_CheckedChanged;
            // 
            // txtKeyAPIProxy
            // 
            txtKeyAPIProxy.Location = new Point(147, 270);
            txtKeyAPIProxy.Name = "txtKeyAPIProxy";
            txtKeyAPIProxy.PlaceholderText = "API Key của mproxy.vn";
            txtKeyAPIProxy.Size = new Size(259, 27);
            txtKeyAPIProxy.TabIndex = 20;
            txtKeyAPIProxy.TextChanged += txtKeyAPIProxy_TextChanged;
            // 
            // radioMultiNovel
            // 
            radioMultiNovel.AutoSize = true;
            radioMultiNovel.Location = new Point(380, 55);
            radioMultiNovel.Name = "radioMultiNovel";
            radioMultiNovel.Size = new Size(116, 24);
            radioMultiNovel.TabIndex = 18;
            radioMultiNovel.TabStop = true;
            radioMultiNovel.Text = "Nhiều Truyện";
            radioMultiNovel.UseVisualStyleBackColor = true;
            radioMultiNovel.CheckedChanged += NhiềuMultiNovel_CheckedChanged;
            // 
            // radiobtnOneNovel
            // 
            radiobtnOneNovel.AutoSize = true;
            radiobtnOneNovel.Location = new Point(270, 55);
            radiobtnOneNovel.Name = "radiobtnOneNovel";
            radiobtnOneNovel.Size = new Size(104, 24);
            radiobtnOneNovel.TabIndex = 17;
            radiobtnOneNovel.TabStop = true;
            radiobtnOneNovel.Text = "Một Truyện";
            radiobtnOneNovel.UseVisualStyleBackColor = true;
            radiobtnOneNovel.CheckedChanged += radiobtnOneNovel_CheckedChanged;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(580, 381);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 21;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1093, 422);
            Controls.Add(btnCancel);
            Controls.Add(btn_Start);
            Controls.Add(groupChangeText);
            Controls.Add(groupCrawlDataInfo);
            Name = "Form1";
            Text = "Form1";
            groupChangeText.ResumeLayout(false);
            groupChangeText.PerformLayout();
            groupCrawlDataInfo.ResumeLayout(false);
            groupCrawlDataInfo.PerformLayout();
            ResumeLayout(false);
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
        private CheckBox cbChangeTextIntoVoice;
        private Label label5;
        private TextBox txtPathFolderVoice;
        private Button btnChoiceFolderVoice;
        private ComboBox cboCrawlFromPage;
        private Label label4;
        private RadioButton radibtnMp3;
        private RadioButton radiobtnMP4;
        private GroupBox groupChangeText;
        private GroupBox groupCrawlDataInfo;
        private RadioButton radioMultiNovel;
        private RadioButton radiobtnOneNovel;
        private Button btnCancel;
        private CheckBox cbChangeProxy;
        private TextBox txtKeyAPIProxy;
    }
}
