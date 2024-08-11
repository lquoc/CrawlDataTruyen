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
            SuspendLayout();
            // 
            // btn_Start
            // 
            btn_Start.Location = new Point(43, 230);
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
            label1.Location = new Point(41, 68);
            label1.Name = "label1";
            label1.Size = new Size(72, 20);
            label1.TabIndex = 1;
            label1.Text = "LinkCrawl";
            // 
            // txtLinkCrawl
            // 
            txtLinkCrawl.Location = new Point(150, 61);
            txtLinkCrawl.Name = "txtLinkCrawl";
            txtLinkCrawl.PlaceholderText = "Link Crawl";
            txtLinkCrawl.Size = new Size(293, 27);
            txtLinkCrawl.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 129);
            label2.Name = "label2";
            label2.Size = new Size(55, 20);
            label2.TabIndex = 3;
            label2.Text = "Thread";
            // 
            // txtThreadNumber
            // 
            txtThreadNumber.Location = new Point(150, 122);
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
            label3.Location = new Point(41, 180);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 5;
            label3.Text = "Save Path";
            // 
            // btnFolder
            // 
            btnFolder.Location = new Point(449, 169);
            btnFolder.Name = "btnFolder";
            btnFolder.Size = new Size(128, 29);
            btnFolder.TabIndex = 6;
            btnFolder.Text = "Choice Folder";
            btnFolder.UseVisualStyleBackColor = true;
            btnFolder.Click += btnFolder_Click;
            // 
            // txtFolderPath
            // 
            txtFolderPath.Location = new Point(150, 171);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.PlaceholderText = "Path Folder";
            txtFolderPath.Size = new Size(293, 27);
            txtFolderPath.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}
