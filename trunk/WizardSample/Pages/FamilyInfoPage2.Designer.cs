namespace WizardSample.Pages
{
    partial class FamilyInfoPage2
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
            this.comboPets = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboPets
            // 
            this.comboPets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPets.FormattingEnabled = true;
            this.comboPets.Items.AddRange(new object[] {
            "Nope",
            "A guinea pig",
            "A hedgehog",
            "A goldfish",
            "A unicorn"});
            this.comboPets.Location = new System.Drawing.Point(159, 62);
            this.comboPets.Name = "comboPets";
            this.comboPets.Size = new System.Drawing.Size(148, 23);
            this.comboPets.TabIndex = 14;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(24, 65);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(129, 15);
            this.lblInfo.TabIndex = 13;
            this.lblInfo.Text = "Do you have any pets?";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(252, 23);
            this.lblHeader.TabIndex = 12;
            this.lblHeader.Text = "Family information (page 2 / 2)";
            // 
            // FamilyInfoPage2
            // 
            this.Controls.Add(this.comboPets);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblHeader);
            this.Name = "FamilyInfoPage2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboPets;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblHeader;
    }
}
