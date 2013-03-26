namespace WizardSample.Pages
{
    partial class FinishPage
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
            this.tbSummary = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbSummary
            // 
            this.tbSummary.BackColor = System.Drawing.SystemColors.Window;
            this.tbSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSummary.Location = new System.Drawing.Point(0, 0);
            this.tbSummary.Multiline = true;
            this.tbSummary.Name = "tbSummary";
            this.tbSummary.ReadOnly = true;
            this.tbSummary.Size = new System.Drawing.Size(366, 241);
            this.tbSummary.TabIndex = 0;
            // 
            // FinishPage
            // 
            this.Controls.Add(this.tbSummary);
            this.Name = "FinishPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSummary;
    }
}
