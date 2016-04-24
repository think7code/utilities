namespace filesmanagement
{
    partial class frmMain
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
            this.btnCreateWS = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnAnalysisWS = new System.Windows.Forms.Button();
            this.btnOrganize = new System.Windows.Forms.Button();
            this.richTxtBoxStatus = new System.Windows.Forms.RichTextBox();
            this.btnClean = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateWS
            // 
            this.btnCreateWS.Location = new System.Drawing.Point(131, 12);
            this.btnCreateWS.Name = "btnCreateWS";
            this.btnCreateWS.Size = new System.Drawing.Size(104, 23);
            this.btnCreateWS.TabIndex = 0;
            this.btnCreateWS.Text = "Create workspace";
            this.btnCreateWS.UseVisualStyleBackColor = true;
            this.btnCreateWS.Click += new System.EventHandler(this.btnCreateWS_Click);
            // 
            // btnAnalysisWS
            // 
            this.btnAnalysisWS.Location = new System.Drawing.Point(12, 12);
            this.btnAnalysisWS.Name = "btnAnalysisWS";
            this.btnAnalysisWS.Size = new System.Drawing.Size(113, 23);
            this.btnAnalysisWS.TabIndex = 1;
            this.btnAnalysisWS.Text = "Analysis workspace";
            this.btnAnalysisWS.UseVisualStyleBackColor = true;
            this.btnAnalysisWS.Click += new System.EventHandler(this.btnAnalysisWS_Click);
            // 
            // btnOrganize
            // 
            this.btnOrganize.Location = new System.Drawing.Point(241, 12);
            this.btnOrganize.Name = "btnOrganize";
            this.btnOrganize.Size = new System.Drawing.Size(124, 23);
            this.btnOrganize.TabIndex = 2;
            this.btnOrganize.Text = "Organize in workspace";
            this.btnOrganize.UseVisualStyleBackColor = true;
            this.btnOrganize.Click += new System.EventHandler(this.btnOrganize_Click);
            // 
            // richTxtBoxStatus
            // 
            this.richTxtBoxStatus.Location = new System.Drawing.Point(12, 41);
            this.richTxtBoxStatus.Name = "richTxtBoxStatus";
            this.richTxtBoxStatus.Size = new System.Drawing.Size(1020, 596);
            this.richTxtBoxStatus.TabIndex = 4;
            this.richTxtBoxStatus.Text = "";
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(371, 12);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(124, 23);
            this.btnClean.TabIndex = 5;
            this.btnClean.Text = "Clean workspace";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 649);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.richTxtBoxStatus);
            this.Controls.Add(this.btnOrganize);
            this.Controls.Add(this.btnAnalysisWS);
            this.Controls.Add(this.btnCreateWS);
            this.Name = "frmMain";
            this.Text = "File Management";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateWS;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnAnalysisWS;
        private System.Windows.Forms.Button btnOrganize;
        private System.Windows.Forms.RichTextBox richTxtBoxStatus;
        private System.Windows.Forms.Button btnClean;
    }
}

