namespace WizardSample.Pages
{
    partial class FamilyInfoPage1
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
            this.comboChildren = new System.Windows.Forms.ComboBox();
            this.lblChildren = new System.Windows.Forms.Label();
            this.lblAdults = new System.Windows.Forms.Label();
            this.comboAdults = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboChildren
            // 
            this.comboChildren.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboChildren.FormattingEnabled = true;
            this.comboChildren.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.comboChildren.Location = new System.Drawing.Point(112, 91);
            this.comboChildren.Name = "comboChildren";
            this.comboChildren.Size = new System.Drawing.Size(68, 23);
            this.comboChildren.TabIndex = 11;
            // 
            // lblChildren
            // 
            this.lblChildren.AutoSize = true;
            this.lblChildren.Location = new System.Drawing.Point(49, 94);
            this.lblChildren.Name = "lblChildren";
            this.lblChildren.Size = new System.Drawing.Size(57, 15);
            this.lblChildren.TabIndex = 10;
            this.lblChildren.Text = "Children:";
            // 
            // lblAdults
            // 
            this.lblAdults.AutoSize = true;
            this.lblAdults.Location = new System.Drawing.Point(61, 65);
            this.lblAdults.Name = "lblAdults";
            this.lblAdults.Size = new System.Drawing.Size(45, 15);
            this.lblAdults.TabIndex = 8;
            this.lblAdults.Text = "Adults:";
            // 
            // comboAdults
            // 
            this.comboAdults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAdults.FormattingEnabled = true;
            this.comboAdults.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.comboAdults.Location = new System.Drawing.Point(112, 62);
            this.comboAdults.Name = "comboAdults";
            this.comboAdults.Size = new System.Drawing.Size(68, 23);
            this.comboAdults.TabIndex = 7;
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(4, 31);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(359, 28);
            this.lblInfo.TabIndex = 6;
            this.lblInfo.Text = "How many members are there in your family?";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(252, 23);
            this.lblHeader.TabIndex = 5;
            this.lblHeader.Text = "Family information (page 1 / 2)";
            // 
            // FamilyInfoPage1
            // 
            this.Controls.Add(this.comboChildren);
            this.Controls.Add(this.lblChildren);
            this.Controls.Add(this.lblAdults);
            this.Controls.Add(this.comboAdults);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblHeader);
            this.Name = "FamilyInfoPage1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboAdults;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblAdults;
        private System.Windows.Forms.Label lblChildren;
        private System.Windows.Forms.ComboBox comboChildren;
    }
}
