namespace ImageProcess.Forms
{
    partial class MainForm
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
            açToolStripMenuItem = new ToolStripMenuItem();
            açToolStripMenuItem1 = new ToolStripMenuItem();
            farklıKaydetToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            çıkışToolStripMenuItem = new ToolStripMenuItem();
            filtrelerToolStripMenuItem = new ToolStripMenuItem();
            yardımToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            splitContainer1 = new SplitContainer();
            pictureBoxOriginal = new PictureBox();
            pictureBoxProcessed = new PictureBox();
            laplacian3x3ToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOriginal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProcessed).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { açToolStripMenuItem, filtrelerToolStripMenuItem, yardımToolStripMenuItem, laplacian3x3ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // açToolStripMenuItem
            // 
            açToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { açToolStripMenuItem1, farklıKaydetToolStripMenuItem, toolStripSeparator1, çıkışToolStripMenuItem });
            açToolStripMenuItem.Name = "açToolStripMenuItem";
            açToolStripMenuItem.Size = new Size(51, 20);
            açToolStripMenuItem.Text = "Dosya";
            // 
            // açToolStripMenuItem1
            // 
            açToolStripMenuItem1.Name = "açToolStripMenuItem1";
            açToolStripMenuItem1.Size = new Size(141, 22);
            açToolStripMenuItem1.Text = "Aç...";
            açToolStripMenuItem1.Click += açToolStripMenuItem1_Click;
            // 
            // farklıKaydetToolStripMenuItem
            // 
            farklıKaydetToolStripMenuItem.Name = "farklıKaydetToolStripMenuItem";
            farklıKaydetToolStripMenuItem.Size = new Size(141, 22);
            farklıKaydetToolStripMenuItem.Text = "Farklı Kaydet";
            farklıKaydetToolStripMenuItem.Click += farklıKaydetToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(138, 6);
            // 
            // çıkışToolStripMenuItem
            // 
            çıkışToolStripMenuItem.Name = "çıkışToolStripMenuItem";
            çıkışToolStripMenuItem.Size = new Size(141, 22);
            çıkışToolStripMenuItem.Text = "Çıkış";
            çıkışToolStripMenuItem.Click += çıkışToolStripMenuItem_Click;
            // 
            // filtrelerToolStripMenuItem
            // 
            filtrelerToolStripMenuItem.Name = "filtrelerToolStripMenuItem";
            filtrelerToolStripMenuItem.Size = new Size(58, 20);
            filtrelerToolStripMenuItem.Text = "Filtreler";
            // 
            // yardımToolStripMenuItem
            // 
            yardımToolStripMenuItem.Name = "yardımToolStripMenuItem";
            yardımToolStripMenuItem.Size = new Size(56, 20);
            yardımToolStripMenuItem.Text = "Yardım";
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pictureBoxOriginal);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pictureBoxProcessed);
            splitContainer1.Size = new Size(800, 426);
            splitContainer1.SplitterDistance = 400;
            splitContainer1.TabIndex = 1;
            // 
            // pictureBoxOriginal
            // 
            pictureBoxOriginal.Dock = DockStyle.Fill;
            pictureBoxOriginal.Location = new Point(0, 0);
            pictureBoxOriginal.Name = "pictureBoxOriginal";
            pictureBoxOriginal.Size = new Size(400, 426);
            pictureBoxOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxOriginal.TabIndex = 0;
            pictureBoxOriginal.TabStop = false;
            // 
            // pictureBoxProcessed
            // 
            pictureBoxProcessed.Dock = DockStyle.Fill;
            pictureBoxProcessed.Location = new Point(0, 0);
            pictureBoxProcessed.Name = "pictureBoxProcessed";
            pictureBoxProcessed.Size = new Size(396, 426);
            pictureBoxProcessed.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxProcessed.TabIndex = 1;
            pictureBoxProcessed.TabStop = false;
            // 
            // laplacian3x3ToolStripMenuItem
            // 
            laplacian3x3ToolStripMenuItem.Name = "laplacian3x3ToolStripMenuItem";
            laplacian3x3ToolStripMenuItem.Size = new Size(86, 20);
            laplacian3x3ToolStripMenuItem.Text = "Laplacian3x3";
            laplacian3x3ToolStripMenuItem.Click += laplacian3x3ToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxOriginal).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProcessed).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private SplitContainer splitContainer1;
        private PictureBox pictureBoxOriginal;
        private PictureBox pictureBoxProcessed;
        private ToolStripMenuItem açToolStripMenuItem;
        private ToolStripMenuItem açToolStripMenuItem1;
        private ToolStripMenuItem farklıKaydetToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem çıkışToolStripMenuItem;
        private ToolStripMenuItem filtrelerToolStripMenuItem;
        private ToolStripMenuItem yardımToolStripMenuItem;
        private ToolStripMenuItem laplacian3x3ToolStripMenuItem;
    }
}