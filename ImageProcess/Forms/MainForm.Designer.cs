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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            TSMFile = new ToolStripMenuItem();
            TSMOpenFile = new ToolStripMenuItem();
            TSMSaveAs = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            TSMExit = new ToolStripMenuItem();
            TSMFilter = new ToolStripMenuItem();
            TSMHelper = new ToolStripMenuItem();
            OpenFileDialog = new OpenFileDialog();
            SaveFileDialog = new SaveFileDialog();
            splitContainer1 = new SplitContainer();
            pictureBoxOriginal = new PictureBox();
            pictureBoxProcessed = new PictureBox();
            PnlChild = new Panel();
            TSMEdit = new ToolStripMenuItem();
            TSMUndo = new ToolStripMenuItem();
            TSMRedo = new ToolStripMenuItem();
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { TSMFile, TSMEdit, TSMFilter, TSMHelper });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(984, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // TSMFile
            // 
            TSMFile.DropDownItems.AddRange(new ToolStripItem[] { TSMOpenFile, TSMSaveAs, toolStripSeparator1, TSMExit });
            TSMFile.Name = "TSMFile";
            TSMFile.Size = new Size(37, 20);
            TSMFile.Text = "File";
            // 
            // TSMOpenFile
            // 
            TSMOpenFile.Name = "TSMOpenFile";
            TSMOpenFile.Size = new Size(180, 22);
            TSMOpenFile.Text = "Open...";
            // 
            // TSMSaveAs
            // 
            TSMSaveAs.Name = "TSMSaveAs";
            TSMSaveAs.Size = new Size(180, 22);
            TSMSaveAs.Text = "Save As";

            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // TSMExit
            // 
            TSMExit.Name = "TSMExit";
            TSMExit.Size = new Size(180, 22);
            TSMExit.Text = "Exit";
            // 
            // TSMFilter
            // 
            TSMFilter.Name = "TSMFilter";
            TSMFilter.Size = new Size(50, 20);
            TSMFilter.Text = "Filters";
            // 
            // TSMHelper
            // 
            TSMHelper.Name = "TSMHelper";
            TSMHelper.Size = new Size(54, 20);
            TSMHelper.Text = "Helper";
            // 
            // OpenFileDialog
            // 
            OpenFileDialog.FileName = "openFileDialog1";
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
            splitContainer1.Size = new Size(784, 426);
            splitContainer1.SplitterDistance = 391;
            splitContainer1.TabIndex = 1;
            // 
            // pictureBoxOriginal
            // 
            pictureBoxOriginal.Dock = DockStyle.Fill;
            pictureBoxOriginal.Location = new Point(0, 0);
            pictureBoxOriginal.Name = "pictureBoxOriginal";
            pictureBoxOriginal.Size = new Size(391, 426);
            pictureBoxOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxOriginal.TabIndex = 0;
            pictureBoxOriginal.TabStop = false;
            // 
            // pictureBoxProcessed
            // 
            pictureBoxProcessed.Dock = DockStyle.Fill;
            pictureBoxProcessed.Location = new Point(0, 0);
            pictureBoxProcessed.Name = "pictureBoxProcessed";
            pictureBoxProcessed.Size = new Size(389, 426);
            pictureBoxProcessed.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxProcessed.TabIndex = 1;
            pictureBoxProcessed.TabStop = false;
            // 
            // PnlChild
            // 
            PnlChild.Dock = DockStyle.Right;
            PnlChild.Location = new Point(784, 24);
            PnlChild.Name = "PnlChild";
            PnlChild.Size = new Size(200, 426);
            PnlChild.TabIndex = 2;
            // 
            // TSMEdit
            // 
            TSMEdit.DropDownItems.AddRange(new ToolStripItem[] { TSMUndo, TSMRedo });
            TSMEdit.Name = "TSMEdit";
            TSMEdit.Size = new Size(39, 20);
            TSMEdit.Text = "Edit";
            // 
            // TSMUndo
            // 
            TSMUndo.Name = "TSMUndo";
            TSMUndo.Size = new Size(180, 22);
            TSMUndo.Text = "Undo";
            // 
            // TSMRedo
            // 
            TSMRedo.Name = "TSMRedo";
            TSMRedo.Size = new Size(180, 22);
            TSMRedo.Text = "Redo";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 450);
            Controls.Add(splitContainer1);
            Controls.Add(PnlChild);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "Image Process";
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
        private OpenFileDialog OpenFileDialog;
        private SaveFileDialog SaveFileDialog;
        private SplitContainer splitContainer1;
        private PictureBox pictureBoxOriginal;
        private PictureBox pictureBoxProcessed;
        private ToolStripMenuItem TSMFile;
        private ToolStripMenuItem TSMOpenFile;
        private ToolStripMenuItem TSMSaveAs;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem TSMExit;
        private ToolStripMenuItem TSMFilter;
        private ToolStripMenuItem TSMHelper;
        private Panel PnlChild;
        private ToolStripMenuItem TSMEdit;
        private ToolStripMenuItem TSMUndo;
        private ToolStripMenuItem TSMRedo;
    }
}