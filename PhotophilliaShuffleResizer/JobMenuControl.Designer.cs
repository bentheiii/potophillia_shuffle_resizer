namespace PhotophilliaShuffleResizer
{
    partial class JobMenuControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.poolButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.RAWButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // poolButton
            // 
            this.poolButton.Location = new System.Drawing.Point(3, 35);
            this.poolButton.Name = "poolButton";
            this.poolButton.Size = new System.Drawing.Size(75, 52);
            this.poolButton.TabIndex = 1;
            this.poolButton.Text = "Re-pool";
            this.poolButton.UseVisualStyleBackColor = true;
            this.poolButton.Click += new System.EventHandler(this.poolButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(190, 3);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(180, 55);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start!";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(0, 132);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(78, 46);
            this.backButton.TabIndex = 4;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(109, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 55);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "Save Job As";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RAWButton
            // 
            this.RAWButton.Location = new System.Drawing.Point(3, 93);
            this.RAWButton.Name = "RAWButton";
            this.RAWButton.Size = new System.Drawing.Size(75, 33);
            this.RAWButton.TabIndex = 7;
            this.RAWButton.Text = "Open RAW";
            this.RAWButton.UseVisualStyleBackColor = true;
            this.RAWButton.Click += new System.EventHandler(this.RAWButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(84, 64);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(286, 117);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // JobMenuControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.RAWButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.poolButton);
            this.Controls.Add(this.label1);
            this.Name = "JobMenuControl";
            this.Size = new System.Drawing.Size(375, 184);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.JobMenuControl_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.JobMenuControl_DragEnter);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button poolButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button RAWButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
