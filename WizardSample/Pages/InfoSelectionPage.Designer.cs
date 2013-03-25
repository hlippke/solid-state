namespace WizardSample.Pages
{
    partial class InfoSelectionPage
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.radioFamily = new System.Windows.Forms.RadioButton();
            this.radioHome = new System.Windows.Forms.RadioButton();
            this.radioWork = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(117, 23);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Info selection";
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(4, 31);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(359, 72);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "On this page you must select which info you want to input into the wizard. Depend" +
    "ing on the selection you make, different paths of the state machine will be foll" +
    "owed.";
            // 
            // radioFamily
            // 
            this.radioFamily.AutoSize = true;
            this.radioFamily.Location = new System.Drawing.Point(20, 106);
            this.radioFamily.Name = "radioFamily";
            this.radioFamily.Size = new System.Drawing.Size(225, 19);
            this.radioFamily.TabIndex = 3;
            this.radioFamily.TabStop = true;
            this.radioFamily.Text = "I want to tell a little about my family";
            this.radioFamily.UseVisualStyleBackColor = true;
            // 
            // radioHome
            // 
            this.radioHome.AutoSize = true;
            this.radioHome.Location = new System.Drawing.Point(20, 131);
            this.radioHome.Name = "radioHome";
            this.radioHome.Size = new System.Drawing.Size(160, 19);
            this.radioHome.TabIndex = 4;
            this.radioHome.TabStop = true;
            this.radioHome.Text = "I want to tell where I live";
            this.radioHome.UseVisualStyleBackColor = true;
            // 
            // radioWork
            // 
            this.radioWork.AutoSize = true;
            this.radioWork.Location = new System.Drawing.Point(20, 156);
            this.radioWork.Name = "radioWork";
            this.radioWork.Size = new System.Drawing.Size(201, 19);
            this.radioWork.TabIndex = 5;
            this.radioWork.TabStop = true;
            this.radioWork.Text = "I want to tell you about my work";
            this.radioWork.UseVisualStyleBackColor = true;
            // 
            // InfoSelectionPage
            // 
            this.Controls.Add(this.radioWork);
            this.Controls.Add(this.radioHome);
            this.Controls.Add(this.radioFamily);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblHeader);
            this.Name = "InfoSelectionPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.RadioButton radioFamily;
        private System.Windows.Forms.RadioButton radioHome;
        private System.Windows.Forms.RadioButton radioWork;
    }
}
