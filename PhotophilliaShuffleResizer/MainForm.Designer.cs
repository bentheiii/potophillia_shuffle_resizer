namespace PhotophilliaShuffleResizer
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
            this.jobMenuControl1 = new PhotophilliaShuffleResizer.JobMenuControl();
            this.jobSelectControl1 = new PhotophilliaShuffleResizer.JobSelectControl();
            this.SuspendLayout();
            // 
            // jobMenuControl1
            // 
            this.jobMenuControl1.Location = new System.Drawing.Point(16, 13);
            this.jobMenuControl1.Name = "jobMenuControl1";
            this.jobMenuControl1.Size = new System.Drawing.Size(373, 184);
            this.jobMenuControl1.TabIndex = 1;
            this.jobMenuControl1.Visible = false;
            this.jobMenuControl1.BackPressed += new System.EventHandler(this.jobMenuControl1_BackPressed);
            // 
            // jobSelectControl1
            // 
            this.jobSelectControl1.Location = new System.Drawing.Point(12, 12);
            this.jobSelectControl1.Name = "jobSelectControl1";
            this.jobSelectControl1.Size = new System.Drawing.Size(373, 184);
            this.jobSelectControl1.TabIndex = 0;
            this.jobSelectControl1.JobSelected += new PhotophilliaShuffleResizer.JobSelectedHandler(this.jobSelectControl1_OnJobSelected);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 209);
            this.Controls.Add(this.jobMenuControl1);
            this.Controls.Add(this.jobSelectControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "ShuffleResizer";
            this.ResumeLayout(false);

        }

        #endregion
        private JobSelectControl jobSelectControl1;
        private JobMenuControl jobMenuControl1;
    }
}

