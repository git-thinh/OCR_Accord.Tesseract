namespace ImageOcrExplorer
{
	partial class TestFormIconFont
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.btnForeColor = new System.Windows.Forms.Button();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.btnBackColor = new System.Windows.Forms.Button();
			this.btnBorderColor = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.chkShowBorder = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button1.Location = new System.Drawing.Point(38, 34);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(156, 50);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button2.Location = new System.Drawing.Point(200, 34);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(156, 50);
			this.button2.TabIndex = 1;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button3.Location = new System.Drawing.Point(362, 43);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(127, 33);
			this.button3.TabIndex = 2;
			this.button3.Text = "button3";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox1.Location = new System.Drawing.Point(255, 108);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(256, 256);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(81, 108);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(168, 21);
			this.comboBox1.TabIndex = 4;
			// 
			// btnForeColor
			// 
			this.btnForeColor.Location = new System.Drawing.Point(81, 138);
			this.btnForeColor.Name = "btnForeColor";
			this.btnForeColor.Size = new System.Drawing.Size(75, 23);
			this.btnForeColor.TabIndex = 5;
			this.btnForeColor.Text = "ForeColor";
			this.btnForeColor.UseVisualStyleBackColor = true;
			this.btnForeColor.Click += new System.EventHandler(this.btnForeColor_Click);
			// 
			// btnGenerate
			// 
			this.btnGenerate.Location = new System.Drawing.Point(12, 222);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(237, 47);
			this.btnGenerate.TabIndex = 11;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// btnBackColor
			// 
			this.btnBackColor.Location = new System.Drawing.Point(173, 138);
			this.btnBackColor.Name = "btnBackColor";
			this.btnBackColor.Size = new System.Drawing.Size(75, 23);
			this.btnBackColor.TabIndex = 6;
			this.btnBackColor.Text = "BackColor";
			this.btnBackColor.UseVisualStyleBackColor = true;
			this.btnBackColor.Click += new System.EventHandler(this.btnBackColor_Click);
			// 
			// btnBorderColor
			// 
			this.btnBorderColor.Location = new System.Drawing.Point(173, 193);
			this.btnBorderColor.Name = "btnBorderColor";
			this.btnBorderColor.Size = new System.Drawing.Size(75, 23);
			this.btnBorderColor.TabIndex = 10;
			this.btnBorderColor.Text = "BorderColor";
			this.btnBorderColor.UseVisualStyleBackColor = true;
			this.btnBorderColor.Click += new System.EventHandler(this.btnBorderColor_Click);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(81, 167);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(168, 20);
			this.numericUpDown1.TabIndex = 8;
			this.numericUpDown1.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 111);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Type:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 169);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(30, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Size:";
			// 
			// chkShowBorder
			// 
			this.chkShowBorder.AutoSize = true;
			this.chkShowBorder.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkShowBorder.Checked = true;
			this.chkShowBorder.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowBorder.Location = new System.Drawing.Point(12, 193);
			this.chkShowBorder.Name = "chkShowBorder";
			this.chkShowBorder.Size = new System.Drawing.Size(89, 17);
			this.chkShowBorder.TabIndex = 9;
			this.chkShowBorder.Text = "Show border:";
			this.chkShowBorder.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(520, 381);
			this.Controls.Add(this.chkShowBorder);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.btnBorderColor);
			this.Controls.Add(this.btnBackColor);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.btnForeColor);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.Button btnForeColor;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Button btnBackColor;
		private System.Windows.Forms.Button btnBorderColor;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkShowBorder;
	}
}

