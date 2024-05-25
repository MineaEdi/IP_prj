using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;
using VideoFunctions;

namespace MyVideoPlayer
{
    public partial class FormMain : Form
    {
        private Video video;
        private string[] videoPaths;
        private string folderPath = @"C:\Users\minea\Desktop";
        private int selectedIndex = 0;
        private Size formSize;
        private Size pnlSize;

        public FormMain()
        {
            InitializeComponent();

            this.KeyPreview = true;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // Check if the video is already initialized
            if (video == null)
            {
                formSize = new Size(this.Width, this.Height);
                pnlSize = new Size(panelMainVideo.Width, panelMainVideo.Height);

                Functionalities.LoadVideos(listBoxOfVideos, folderPath, out videoPaths, out selectedIndex);
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            Functionalities.PlayPauseVideo(video, timerVideo, buttonPlay);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            NextVideo();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            PreviousVideo();
        }

        private void buttonFullscreen_Click(object sender, EventArgs e)
        {
            Functionalities.ToggleFullscreen(this, video);
        }

        private void buttonVolume_Click(object sender, EventArgs e)
        {
            Functionalities.ToggleVolumeTrackBar(trackBarVolume);
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            Functionalities.AdjustVolume(video, trackBarVolume);
        }

        private void listBoxOfVideos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functionalities.SelectVideo(listBoxOfVideos, panelMainVideo, pnlSize, ref video, videoPaths, labelVideo, buttonPlay, timerVideo, NextVideo);
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Functionalities.ExitFullscreen(this, video, formSize, panelMainVideo, pnlSize);
            }
        }

        private void timerVideo_Tick(object sender, EventArgs e)
        {
            Functionalities.UpdateVideoPosition(video, labelVideoPosition);
        }

        private void NextVideo()
        {
            Functionalities.NextVideo(listBoxOfVideos, videoPaths.Length, index => listBoxOfVideos.SelectedIndex = index);
        }

        private void PreviousVideo()
        {
            Functionalities.PreviousVideo(listBoxOfVideos, videoPaths.Length, index => listBoxOfVideos.SelectedIndex = index);
        }
    }
}