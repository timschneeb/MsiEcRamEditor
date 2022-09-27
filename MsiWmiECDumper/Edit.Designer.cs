namespace MsiWmiECDumper
{
    partial class Edit
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
            this.addr = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.value = new System.Windows.Forms.NumericUpDown();
            this.button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.addr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.value)).BeginInit();
            this.SuspendLayout();
            // 
            // addr
            // 
            this.addr.Hexadecimal = true;
            this.addr.Location = new System.Drawing.Point(12, 25);
            this.addr.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.addr.Name = "addr";
            this.addr.Size = new System.Drawing.Size(61, 20);
            this.addr.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Value";
            // 
            // value
            // 
            this.value.Location = new System.Drawing.Point(79, 25);
            this.value.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.value.Name = "value";
            this.value.Size = new System.Drawing.Size(61, 20);
            this.value.TabIndex = 3;
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(12, 54);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(128, 23);
            this.button.TabIndex = 4;
            this.button.Text = "Set";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // Edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(154, 89);
            this.Controls.Add(this.button);
            this.Controls.Add(this.value);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Edit";
            this.Text = "Edit";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.addr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.value)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown addr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown value;
        private System.Windows.Forms.Button button;
    }
}