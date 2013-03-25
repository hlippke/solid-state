namespace WizardSample.Pages
{
    partial class WorkInfoPage
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.comboKindOfWork = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(4, 31);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(359, 28);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "What kind of work do you have?";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(243, 23);
            this.lblHeader.TabIndex = 2;
            this.lblHeader.Text = "Work information (page 1 / 1)";
            // 
            // comboKindOfWork
            // 
            this.comboKindOfWork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboKindOfWork.FormattingEnabled = true;
            this.comboKindOfWork.Items.AddRange(new object[] {
            "I\'m a motorcycle police on the information highway",
            "I convert bits for a living",
            "I work as a leather merchant in Azeroth",
            "I sell vacuum cleaners"});
            this.comboKindOfWork.Location = new System.Drawing.Point(3, 62);
            this.comboKindOfWork.Name = "comboKindOfWork";
            this.comboKindOfWork.Size = new System.Drawing.Size(360, 23);
            this.comboKindOfWork.TabIndex = 4;
            // 
            // WorkInfoPage
            // 
            this.Controls.Add(this.comboKindOfWork);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblHeader);
            this.Name = "WorkInfoPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ComboBox comboKindOfWork;
    }
}
