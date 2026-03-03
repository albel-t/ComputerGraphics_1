namespace ComputerGraphics_1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonRebuildWindow = new System.Windows.Forms.Button();
            this.Labelx1 = new System.Windows.Forms.Label();
            this.textBoxX1 = new System.Windows.Forms.TextBox();
            this.textBoxY1 = new System.Windows.Forms.TextBox();
            this.Labely1 = new System.Windows.Forms.Label();
            this.textBoxX2 = new System.Windows.Forms.TextBox();
            this.Labelx2 = new System.Windows.Forms.Label();
            this.textBoxY2 = new System.Windows.Forms.TextBox();
            this.Labely2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelScale = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRebuildWindow
            // 
            this.buttonRebuildWindow.Location = new System.Drawing.Point(547, 415);
            this.buttonRebuildWindow.Name = "buttonRebuildWindow";
            this.buttonRebuildWindow.Size = new System.Drawing.Size(119, 23);
            this.buttonRebuildWindow.TabIndex = 0;
            this.buttonRebuildWindow.Text = "Rebuild Window";
            this.buttonRebuildWindow.UseVisualStyleBackColor = true;
            this.buttonRebuildWindow.Click += new System.EventHandler(this.buttonRebuildWindow_Click);
            // 
            // Labelx1
            // 
            this.Labelx1.AutoSize = true;
            this.Labelx1.Location = new System.Drawing.Point(544, 132);
            this.Labelx1.Name = "Labelx1";
            this.Labelx1.Size = new System.Drawing.Size(20, 13);
            this.Labelx1.TabIndex = 1;
            this.Labelx1.Text = "X1";
            // 
            // textBoxX1
            // 
            this.textBoxX1.Location = new System.Drawing.Point(585, 129);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(46, 20);
            this.textBoxX1.TabIndex = 2;
            // 
            // textBoxY1
            // 
            this.textBoxY1.Location = new System.Drawing.Point(585, 155);
            this.textBoxY1.Name = "textBoxY1";
            this.textBoxY1.Size = new System.Drawing.Size(46, 20);
            this.textBoxY1.TabIndex = 4;
            // 
            // Labely1
            // 
            this.Labely1.AutoSize = true;
            this.Labely1.Location = new System.Drawing.Point(544, 158);
            this.Labely1.Name = "Labely1";
            this.Labely1.Size = new System.Drawing.Size(20, 13);
            this.Labely1.TabIndex = 3;
            this.Labely1.Text = "Y1";
            // 
            // textBoxX2
            // 
            this.textBoxX2.Location = new System.Drawing.Point(696, 129);
            this.textBoxX2.Name = "textBoxX2";
            this.textBoxX2.Size = new System.Drawing.Size(46, 20);
            this.textBoxX2.TabIndex = 8;
            // 
            // Labelx2
            // 
            this.Labelx2.AutoSize = true;
            this.Labelx2.Location = new System.Drawing.Point(655, 132);
            this.Labelx2.Name = "Labelx2";
            this.Labelx2.Size = new System.Drawing.Size(20, 13);
            this.Labelx2.TabIndex = 7;
            this.Labelx2.Text = "X2";
            // 
            // textBoxY2
            // 
            this.textBoxY2.Location = new System.Drawing.Point(696, 155);
            this.textBoxY2.Name = "textBoxY2";
            this.textBoxY2.Size = new System.Drawing.Size(46, 20);
            this.textBoxY2.TabIndex = 6;
            // 
            // Labely2
            // 
            this.Labely2.AutoSize = true;
            this.Labely2.Location = new System.Drawing.Point(655, 158);
            this.Labely2.Name = "Labely2";
            this.Labely2.Size = new System.Drawing.Size(20, 13);
            this.Labely2.TabIndex = 5;
            this.Labely2.Text = "Y2";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "dot",
            "line",
            "circle",
            "selection",
            "eraser"});
            this.comboBox1.Location = new System.Drawing.Point(547, 68);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(84, 21);
            this.comboBox1.TabIndex = 9;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Bresenham\'s line algorithm",
            "Xiaolin Wu\'s line algorithm"});
            this.comboBox2.Location = new System.Drawing.Point(658, 68);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(84, 21);
            this.comboBox2.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(499, 426);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(696, 415);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(92, 23);
            this.buttonApply.TabIndex = 12;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(585, 233);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(67, 20);
            this.textBox1.TabIndex = 14;
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(544, 236);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(34, 13);
            this.labelScale.TabIndex = 13;
            this.labelScale.Text = "Scale";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(707, 233);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(67, 20);
            this.textBox2.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(666, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Round";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBoxX2);
            this.Controls.Add(this.Labelx2);
            this.Controls.Add(this.textBoxY2);
            this.Controls.Add(this.Labely2);
            this.Controls.Add(this.textBoxY1);
            this.Controls.Add(this.Labely1);
            this.Controls.Add(this.textBoxX1);
            this.Controls.Add(this.Labelx1);
            this.Controls.Add(this.buttonRebuildWindow);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRebuildWindow;
        private System.Windows.Forms.Label Labelx1;
        private System.Windows.Forms.TextBox textBoxX1;
        private System.Windows.Forms.TextBox textBoxY1;
        private System.Windows.Forms.Label Labely1;
        private System.Windows.Forms.TextBox textBoxX2;
        private System.Windows.Forms.Label Labelx2;
        private System.Windows.Forms.TextBox textBoxY2;
        private System.Windows.Forms.Label Labely2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
    }
}

