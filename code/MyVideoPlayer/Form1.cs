/**************************************************************************
 *                                                                        *
 *  File:        Form1.cs                                                 *
 *  Copyright:   (c) 2024, Bulgaru, Constantin, Minea, Zaharia            *
 *  Description: Această clasă reprezintă clasa principală a aplicației.  *
 *               Gestionează evenimentele și interacțiunea utilizatorului *
 *               cu interfața grafică. Inițializează componentele vizuale *
 *               ale interfeței și definește comportamentul acestora în   *
 *               răspuns la acțiunile utilizatorului.                     *
 *                                                                        *
 **************************************************************************/

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
        // Variables necessary for the video player to function
        private Video video;
        private string[] videoPaths;
        private string folderPath;
        private int selectedIndex = 0;
        private Size formSize;
        private Size pnlSize;

        /// <summary>
        /// Constructor of the main form
        /// </summary>
        public FormMain()
        {
            InitializeComponent();

            // Allows handling keyboard events
            this.KeyPreview = true;
        }

        /// <summary>
        /// Event triggered when the form is loaded
        /// </summary>
        private void FormMain_Load(object sender, EventArgs e)
        {
            // Save the initial dimensions of the form and the video panel
            formSize = new Size(this.Width, this.Height);
            pnlSize = new Size(panelMainVideo.Width, panelMainVideo.Height);
        }

        /// <summary>
        /// Event triggered when the Play/Pause button is clicked
        /// </summary>
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            Functionalities.PlayPauseVideo(video, timerVideo, buttonPlay);
        }

        /// <summary>
        /// Event triggered when the Next button is clicked
        /// </summary>
        private void buttonNext_Click(object sender, EventArgs e)
        {
            NextVideo();
        }

        /// <summary>
        /// Event triggered when the Previous button is clicked
        /// </summary>
        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            PreviousVideo();
        }

        /// <summary>
        /// Event triggered when the Fullscreen button is clicked
        /// </summary>
        private void buttonFullscreen_Click(object sender, EventArgs e)
        {
            Functionalities.ToggleFullscreen(this, video);
        }

        /// <summary>
        /// Event triggered when the Volume button is clicked
        /// </summary>
        private void buttonVolume_Click(object sender, EventArgs e)
        {
            Functionalities.ToggleVolumeTrackBar(trackBarVolume);
        }

        /// <summary>
        /// Event triggered when the volume TrackBar is scrolled
        /// </summary>
        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            Functionalities.AdjustVolume(video, trackBarVolume);
        }

        /// <summary>
        /// Event triggered when the selection in the video ListBox changes
        /// </summary>
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

        /// <summary>
        /// Event triggered when a key is pressed
        /// </summary>
        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            // exit fullscreen when ESC key is pressed
            if (e.KeyCode == Keys.Escape)
            {
                Functionalities.ExitFullscreen(this, video, formSize, panelMainVideo, pnlSize);
            }
        }

        /// <summary>
        /// Event triggered by the timer for updating the position of the video
        /// </summary>
        private void timerVideo_Tick(object sender, EventArgs e)
        {
            Functionalities.UpdateVideoPosition(video, labelVideoPosition);
        }

        /// <summary>
        /// Method to play the next video
        /// </summary>
        private void NextVideo()
        {
            Functionalities.NextVideo(listBoxOfVideos, videoPaths.Length, index => listBoxOfVideos.SelectedIndex = index);
        }

        /// <summary>
        /// Method to play the previous video
        /// </summary>
        private void PreviousVideo()
        {
            Functionalities.PreviousVideo(listBoxOfVideos, videoPaths.Length, index => listBoxOfVideos.SelectedIndex = index);
        }

        /// <summary>
        /// Event triggered when the About button is clicked
        /// </summary>
        private void buttonAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Acesta este un program ce poate reda fișiere media cu extensia .wmv", "Despre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Event triggered when the Help button is clicked
        /// </summary>
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "PlayerVideo.chm");
        }

        /// <summary>
        /// Event triggered when the Open Folder button is clicked
        /// </summary>
        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    folderPath = folderDialog.SelectedPath;
                    // check if the folderPath is not empty
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        try
                        {
                            listBoxOfVideos.Items.Clear(); // clear the list before loading new videos
                            ClearCurrentVideo(); // clear the panel and reset the video
                            Functionalities.LoadVideos(listBoxOfVideos, folderPath, out videoPaths, out selectedIndex);

                            // check if in the videoPaths are no videos
                            if (videoPaths.Length == 0)
                            {
                                // show message to user in order to inform
                                MessageBox.Show("Folderul selectat nu conține videoclipuri .wmv.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                video = null;
                                labelVideo.Text = string.Empty;
                                labelVideoPosition.Text = string.Empty;
                                buttonPlay.Text = "Play";
                            }
                        }
                        catch (Exception ex)
                        {
                            // show error message if exists
                            MessageBox.Show("A apărut o eroare la încărcarea videoclipurilor: " + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // show message if the path to the folder is empty
                        MessageBox.Show("Calea către folder nu poate fi goală. Vă rugăm să selectați un folder valid.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        /// <summary>
        /// Method to clear the current video
        /// </summary>
        private void ClearCurrentVideo()
        {
            // check if one video is loaded in order to clear it
            if (video != null)
            {
                video.Stop(); // stop the video that is playing
                video.Dispose(); // clear resources taken by video
                video = null; // reset video to null
            }
            panelMainVideo.Controls.Clear(); // clear all controls in main panel
        }
    }
}