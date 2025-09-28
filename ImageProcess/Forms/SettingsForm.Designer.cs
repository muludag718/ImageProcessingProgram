namespace ImageProcess.Forms
{
    partial class SettingsForm
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
            splitContainer1 = new SplitContainer();
            BtnOK = new Button();
            BtnCancel = new Button();
            propertyGrid1 = new PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Bottom;
            splitContainer1.Location = new Point(0, 317);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(BtnOK);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(BtnCancel);
            splitContainer1.Size = new Size(184, 44);
            splitContainer1.SplitterDistance = 90;
            splitContainer1.TabIndex = 2;
            // 
            // BtnOK
            // 
            BtnOK.Dock = DockStyle.Fill;
            BtnOK.FlatAppearance.BorderColor = Color.MediumSeaGreen;
            BtnOK.FlatStyle = FlatStyle.Flat;
            BtnOK.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            BtnOK.ForeColor = Color.MediumSeaGreen;
            BtnOK.Location = new Point(0, 0);
            BtnOK.Name = "BtnOK";
            BtnOK.Size = new Size(90, 44);
            BtnOK.TabIndex = 2;
            BtnOK.Text = "Apply";
            BtnOK.UseVisualStyleBackColor = false;
            BtnOK.Click += BtnOK_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.BackColor = SystemColors.Control;
            BtnCancel.Dock = DockStyle.Fill;
            BtnCancel.FlatAppearance.BorderColor = Color.IndianRed;
            BtnCancel.FlatStyle = FlatStyle.Flat;
            BtnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            BtnCancel.ForeColor = Color.IndianRed;
            BtnCancel.Location = new Point(0, 0);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(90, 44);
            BtnCancel.TabIndex = 3;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = false;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Dock = DockStyle.Fill;
            propertyGrid1.Location = new Point(0, 0);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new Size(184, 317);
            propertyGrid1.TabIndex = 3;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(184, 361);
            Controls.Add(propertyGrid1);
            Controls.Add(splitContainer1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SettingsForm";
            Text = "SettingsForm";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private Button BtnOK;
        private Button BtnCancel;
        private PropertyGrid propertyGrid1;
    }
}