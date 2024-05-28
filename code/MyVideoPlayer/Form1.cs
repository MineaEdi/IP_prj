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
        private string folderPath;
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
            formSize = new Size(this.Width, this.Height);
            pnlSize = new Size(panelMainVideo.Width, panelMainVideo.Height);
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
            try
            {
                Functionalities.SelectVideo(listBoxOfVideos, panelMainVideo, pnlSize, ref video, videoPaths, labelVideo, buttonPlay, timerVideo, NextVideo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("A apărut o eroare la selectarea videoclipului: " + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Acesta este un program ce poate reda fișiere media cu extensia .wmv", "Despre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "PlayerVideo.chm");
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    folderPath = folderDialog.SelectedPath;
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        try
                        {
                            listBoxOfVideos.Items.Clear(); // Clear the list before loading new videos
                            ClearCurrentVideo(); // Clear the panel and reset the video
                            Functionalities.LoadVideos(listBoxOfVideos, folderPath, out videoPaths, out selectedIndex);

                            if (videoPaths.Length == 0)
                            {
                                MessageBox.Show("Folderul selectat nu conține videoclipuri .wmv.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                video = null;
                                labelVideo.Text = string.Empty;
                                labelVideoPosition.Text = string.Empty;
                                buttonPlay.Text = "Play";
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("A apărut o eroare la încărcarea videoclipurilor: " + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Calea către folder nu poate fi goală. Vă rugăm să selectați un folder valid.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void ClearCurrentVideo()
        {
            if (video != null)
            {
                video.Stop();
                video.Dispose();
                video = null;
            }
            panelMainVideo.Controls.Clear();
        }
    }
}