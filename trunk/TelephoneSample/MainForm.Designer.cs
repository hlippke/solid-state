namespace TelephoneSample
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
            this.gboxTriggers = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnIncomingCall = new System.Windows.Forms.Button();
            this.btnPickUpPhone = new System.Windows.Forms.Button();
            this.gboxConditions = new System.Windows.Forms.GroupBox();
            this.checkIsLineBusy = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlState = new System.Windows.Forms.Panel();
            this.lblState = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.gboxTriggers.SuspendLayout();
            this.gboxConditions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlState.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxTriggers
            // 
            this.gboxTriggers.Controls.Add(this.button7);
            this.gboxTriggers.Controls.Add(this.button6);
            this.gboxTriggers.Controls.Add(this.button5);
            this.gboxTriggers.Controls.Add(this.button4);
            this.gboxTriggers.Controls.Add(this.button3);
            this.gboxTriggers.Controls.Add(this.button2);
            this.gboxTriggers.Controls.Add(this.button1);
            this.gboxTriggers.Controls.Add(this.btnIncomingCall);
            this.gboxTriggers.Controls.Add(this.btnPickUpPhone);
            this.gboxTriggers.Location = new System.Drawing.Point(12, 12);
            this.gboxTriggers.Name = "gboxTriggers";
            this.gboxTriggers.Size = new System.Drawing.Size(200, 322);
            this.gboxTriggers.TabIndex = 0;
            this.gboxTriggers.TabStop = false;
            this.gboxTriggers.Text = " Triggers ";
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.Location = new System.Drawing.Point(6, 181);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(188, 25);
            this.button7.TabIndex = 8;
            this.button7.Text = "No answer in other end";
            this.button7.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(6, 286);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(188, 25);
            this.button6.TabIndex = 7;
            this.button6.Text = "Other end is hanging up";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(6, 255);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(188, 25);
            this.button5.TabIndex = 6;
            this.button5.Text = "I\'m hanging up";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(6, 212);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(188, 25);
            this.button4.TabIndex = 5;
            this.button4.Text = "The line is busy";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(6, 150);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(188, 25);
            this.button3.TabIndex = 4;
            this.button3.Text = "The other end answered";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(6, 119);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(188, 25);
            this.button2.TabIndex = 3;
            this.button2.Text = "I have finished dialling";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(6, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(188, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "There is no answer";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btnIncomingCall
            // 
            this.btnIncomingCall.Enabled = false;
            this.btnIncomingCall.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncomingCall.Location = new System.Drawing.Point(6, 57);
            this.btnIncomingCall.Name = "btnIncomingCall";
            this.btnIncomingCall.Size = new System.Drawing.Size(188, 25);
            this.btnIncomingCall.TabIndex = 1;
            this.btnIncomingCall.Text = "Incoming call";
            this.btnIncomingCall.UseVisualStyleBackColor = false;
            // 
            // btnPickUpPhone
            // 
            this.btnPickUpPhone.Enabled = false;
            this.btnPickUpPhone.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPickUpPhone.Location = new System.Drawing.Point(6, 26);
            this.btnPickUpPhone.Name = "btnPickUpPhone";
            this.btnPickUpPhone.Size = new System.Drawing.Size(188, 25);
            this.btnPickUpPhone.TabIndex = 0;
            this.btnPickUpPhone.Text = "Pick up phone";
            this.btnPickUpPhone.UseVisualStyleBackColor = false;
            // 
            // gboxConditions
            // 
            this.gboxConditions.Controls.Add(this.checkIsLineBusy);
            this.gboxConditions.Location = new System.Drawing.Point(218, 267);
            this.gboxConditions.Name = "gboxConditions";
            this.gboxConditions.Size = new System.Drawing.Size(267, 67);
            this.gboxConditions.TabIndex = 1;
            this.gboxConditions.TabStop = false;
            this.gboxConditions.Text = " Conditions ";
            // 
            // checkIsLineBusy
            // 
            this.checkIsLineBusy.AutoSize = true;
            this.checkIsLineBusy.Location = new System.Drawing.Point(14, 27);
            this.checkIsLineBusy.Name = "checkIsLineBusy";
            this.checkIsLineBusy.Size = new System.Drawing.Size(197, 23);
            this.checkIsLineBusy.TabIndex = 0;
            this.checkIsLineBusy.Text = "Telephone number is busy";
            this.checkIsLineBusy.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TelephoneSample.Properties.Resources.telephone;
            this.pictureBox1.Location = new System.Drawing.Point(218, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(267, 203);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // pnlState
            // 
            this.pnlState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.pnlState.Controls.Add(this.lblState);
            this.pnlState.Location = new System.Drawing.Point(218, 233);
            this.pnlState.Name = "pnlState";
            this.pnlState.Size = new System.Drawing.Size(267, 28);
            this.pnlState.TabIndex = 3;
            // 
            // lblState
            // 
            this.lblState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblState.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.ForeColor = System.Drawing.Color.White;
            this.lblState.Location = new System.Drawing.Point(0, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(267, 28);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "Current state";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 341);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(472, 225);
            this.textBox1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(499, 578);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pnlState);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gboxConditions);
            this.Controls.Add(this.gboxTriggers);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "solid-state telephone sample";
            this.gboxTriggers.ResumeLayout(false);
            this.gboxConditions.ResumeLayout(false);
            this.gboxConditions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlState.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxTriggers;
        private System.Windows.Forms.GroupBox gboxConditions;
        private System.Windows.Forms.CheckBox checkIsLineBusy;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlState;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Button btnPickUpPhone;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnIncomingCall;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
    }
}

