namespace MyVideoPlayer
{
    partial class Form1
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
            this.panelMainVideo = new System.Windows.Forms.Panel();
            this.listBoxOfVideos = new System.Windows.Forms.ListBox();
            this.trackBarVolume = new System.Windows.Forms.TrackBar();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonFullscreen = new System.Windows.Forms.Button();
            this.buttonVolume = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMainVideo
            // 
            this.panelMainVideo.Location = new System.Drawing.Point(193, 12);
            this.panelMainVideo.Name = "panelMainVideo";
            this.panelMainVideo.Size = new System.Drawing.Size(596, 330);
            this.panelMainVideo.TabIndex = 0;
            // 
            // listBoxOfVideos
            // 
            this.listBoxOfVideos.FormattingEnabled = true;
            this.listBoxOfVideos.Location = new System.Drawing.Point(13, 13);
            this.listBoxOfVideos.Name = "listBoxOfVideos";
            this.listBoxOfVideos.Size = new System.Drawing.Size(174, 329);
            this.listBoxOfVideos.TabIndex = 1;
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Location = new System.Drawing.Point(685, 348);
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Size = new System.Drawing.Size(104, 45);
            this.trackBarVolume.TabIndex = 2;
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(21, 377);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonPlay.TabIndex = 3;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(21, 348);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 4;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Location = new System.Drawing.Point(102, 348);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(75, 23);
            this.buttonPrevious.TabIndex = 5;
            this.buttonPrevious.Text = "Previous";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            // 
            // buttonFullscreen
            // 
            this.buttonFullscreen.Location = new System.Drawing.Point(102, 377);
            this.buttonFullscreen.Name = "buttonFullscreen";
            this.buttonFullscreen.Size = new System.Drawing.Size(75, 23);
            this.buttonFullscreen.TabIndex = 6;
            this.buttonFullscreen.Text = "Fullscreen";
            this.buttonFullscreen.UseVisualStyleBackColor = true;
            // 
            // buttonVolume
            // 
            this.buttonVolume.Location = new System.Drawing.Point(604, 348);
            this.buttonVolume.Name = "buttonVolume";
            this.buttonVolume.Size = new System.Drawing.Size(75, 23);
            this.buttonVolume.TabIndex = 7;
            this.buttonVolume.Text = "Volume";
            this.buttonVolume.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 443);
            this.Controls.Add(this.buttonVolume);
            this.Controls.Add(this.buttonFullscreen);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.trackBarVolume);
            this.Controls.Add(this.listBoxOfVideos);
            this.Controls.Add(this.panelMainVideo);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMainVideo;
        private System.Windows.Forms.ListBox listBoxOfVideos;
        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonFullscreen;
        private System.Windows.Forms.Button buttonVolume;
    }
}

