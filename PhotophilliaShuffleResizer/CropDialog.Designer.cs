namespace PhotophilliaShuffleResizer
{
    partial class CropDialog
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.RightBar = new System.Windows.Forms.TrackBar();
            this.LeftBar = new System.Windows.Forms.TrackBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.BottomBar = new System.Windows.Forms.TrackBar();
            this.TopBar = new System.Windows.Forms.TrackBar();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopBar)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(561, 567);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // RightBar
            // 
            this.RightBar.Location = new System.Drawing.Point(123, 244);
            this.RightBar.Maximum = 100;
            this.RightBar.Name = "RightBar";
            this.RightBar.Size = new System.Drawing.Size(90, 45);
            this.RightBar.TabIndex = 4;
            this.RightBar.TickFrequency = 2;
            this.RightBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.RightBar.Value = 100;
            this.RightBar.Scroll += new System.EventHandler(this.RightBar_Scroll);
            // 
            // LeftBar
            // 
            this.LeftBar.Location = new System.Drawing.Point(6, 244);
            this.LeftBar.Maximum = 100;
            this.LeftBar.Name = "LeftBar";
            this.LeftBar.Size = new System.Drawing.Size(90, 45);
            this.LeftBar.TabIndex = 5;
            this.LeftBar.TickFrequency = 2;
            this.LeftBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.LeftBar.Scroll += new System.EventHandler(this.LeftBar_Scroll);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(102, 254);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // BottomBar
            // 
            this.BottomBar.Location = new System.Drawing.Point(6, 134);
            this.BottomBar.Maximum = 100;
            this.BottomBar.Name = "BottomBar";
            this.BottomBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.BottomBar.Size = new System.Drawing.Size(45, 104);
            this.BottomBar.TabIndex = 7;
            this.BottomBar.TickFrequency = 2;
            this.BottomBar.Scroll += new System.EventHandler(this.BottomBar_Scroll);
            // 
            // TopBar
            // 
            this.TopBar.Location = new System.Drawing.Point(6, 12);
            this.TopBar.Maximum = 100;
            this.TopBar.Name = "TopBar";
            this.TopBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TopBar.Size = new System.Drawing.Size(45, 104);
            this.TopBar.TabIndex = 8;
            this.TopBar.TickFrequency = 2;
            this.TopBar.Value = 100;
            this.TopBar.Scroll += new System.EventHandler(this.TopBar_Scroll);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(7, 119);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.RightBar);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.LeftBar);
            this.panel1.Controls.Add(this.TopBar);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.BottomBar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(561, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 567);
            this.panel1.TabIndex = 10;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(138, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(138, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CropDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 567);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Name = "CropDialog";
            this.Text = "CropDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CropDialog_FormClosing);
            this.Load += new System.EventHandler(this.CropDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopBar)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar RightBar;
        private System.Windows.Forms.TrackBar LeftBar;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TrackBar BottomBar;
        private System.Windows.Forms.TrackBar TopBar;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}