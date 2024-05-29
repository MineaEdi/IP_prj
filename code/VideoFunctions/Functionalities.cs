/**************************************************************************
 *                                                                        *
 *  File:        Functionalities.cs                                       *
 *  Copyright:   (c) 2024, Bulgaru, Constantin, Minea, Zaharia            *
 *  Description: Această clasă conține funcționalitățile principale ale   *
 *               aplicației. Include metode pentru: încărcarea            *
 *               videoclipurilor dintr-un director specificat, redarea,   *
 *               oprirea, navigarea între videoclipuri, modul fullscreen  *
 *               și ajustarea volumului. Îmbunătățește modularitatea și   *
 *               organizarea codului prin separarea funcționalităților    *
 *               principale într-o clasă dedicată.                        *
 *                                                                        *
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;

namespace VideoFunctions
{
    public class Functionalities
    {
        /// <summary>
        /// Loads all .wmv video files from a specified directory into the given ListBox
        /// </summary>
        /// <param name="lstVideos">The ListBox to display the videos</param>
        /// <param name="folderPath">The path to the directory containing the videos</param>
        /// <param name="videoPaths">Array of strings containing the file paths</param>
        /// <param name="selectedIndex">Index of the selected video</param>
        public static void LoadVideos(ListBox lstVideos, string folderPath, out string[] videoPaths, out int selectedIndex)
        {
            // Get all .wmv files in the specified directory
            videoPaths = System.IO.Directory.GetFiles(folderPath, "*.wmv");
            selectedIndex = 0;

            // If video paths are found, add them to the ListBox
            if (videoPaths != null)
            {
                foreach (string path in videoPaths)
                {
                    // Extract video name from path and add to ListBox
                    string vid = path.Replace(folderPath, string.Empty);
                    vid = vid.Replace(".wmv", string.Empty);
                    lstVideos.Items.Add(vid);
                }
            }
            // Set the initial selected index in the ListBox
            lstVideos.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// Selects and plays the video from the given ListBox
        /// </summary>
        /// <param name="lstVideos">The ListBox containing the video list</param>
        /// <param name="pnlVideo">The panel where the video will be played</param>
        /// <param name="pnlSize">The size of the video panel</param>
        /// <param name="video">Reference to the video object</param>
        /// <param name="videoPaths">Array of strings containing the file paths</param>
        /// <param name="lblVideo">The label to display the video name</param>
        /// <param name="btnPlayPause">The Play/Pause button</param>
        /// <param name="tmrVideo">Timer for updating the video position</param>
        /// <param name="nextVideo">Action to play the next video</param>
        public static void SelectVideo(ListBox lstVideos, Panel pnlVideo, Size pnlSize, ref Video video,
                                        string[] videoPaths, System.Windows.Forms.Label lblVideo, 
                                        Button btnPlayPause, Timer tmrVideo, Action nextVideo)
        {
            try
            {
                // Stop and dispose of the current video if it exists
                video.Stop();
                video.Dispose();
            }
            catch { }

            // Get the selected index and play the corresponding video
            int index = lstVideos.SelectedIndex;
            video = new Video(videoPaths[index], false);
            video.Owner = pnlVideo;
            pnlVideo.Size = pnlSize;
            video.Play();

            // Enable the timer for video position updates and set button text to "Pause"
            tmrVideo.Enabled = true;
            btnPlayPause.Text = "Pause";

            // Set up the video ending event to play the next video
            video.Ending += (sender, e) => Video_Ending(nextVideo);

            // Display the selected video name in the label
            lblVideo.Text = lstVideos.Text;
        }

        /// <summary>
        /// Triggers at the end of a video and plays the next video after a wait time
        /// </summary>
        /// <param name="nextVideo">Action to play the next video</param>
        private static void Video_Ending(Action nextVideo)
        {
            // Wait for 2 seconds and then play the next video
            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(2000);
                nextVideo();
            });
        }

        /// <summary>
        /// Plays the next video from the ListBox
        /// </summary>
        /// <param name="lstVideos">The ListBox containing the video list</param>
        /// <param name="videoCount">The total number of videos</param>
        /// <param name="setSelectedIndex">Action to set the selected index</param>
        public static void NextVideo(ListBox lstVideos, int videoCount, Action<int> setSelectedIndex)
        {
            // Increment the index and wrap around if it exceeds the total number of videos
            int index = lstVideos.SelectedIndex;
            index = (index + 1) % videoCount;
            setSelectedIndex(index);
        }

        /// <summary>
        /// Plays the previous video from the ListBox
        /// </summary>
        /// <param name="lstVideos">The ListBox containing the video list</param>
        /// <param name="videoCount">The total number of videos</param>
        /// <param name="setSelectedIndex">Action to set the selected index</param>
        public static void PreviousVideo(ListBox lstVideos, int videoCount, Action<int> setSelectedIndex)
        {
            // Decrement the index and wrap around if it goes below zero
            int index = lstVideos.SelectedIndex;
            index = (index - 1 + videoCount) % videoCount;
            setSelectedIndex(index);
        }

        /// <summary>
        /// Plays or pauses the current video
        /// </summary>
        /// <param name="video">The video object</param>
        /// <param name="tmrVideo">Timer for updating the video position</param>
        /// <param name="btnPlayPause">The Play/Pause button</param>
        public static void PlayPauseVideo(Video video, Timer tmrVideo, Button btnPlayPause)
        {
            // Check if a video is loaded
            if (video == null)
            {
                MessageBox.Show("No video is currently loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Play or pause the video based on its current state
            if (!video.Playing)
            {
                video.Play();
                tmrVideo.Enabled = true;
                btnPlayPause.Text = "Pause";
            }
            else
            {
                video.Pause();
                tmrVideo.Enabled = false;
                btnPlayPause.Text = "Play";
            }
        }

        /// <summary>
        /// Activates fullscreen mode for video playback
        /// </summary>
        /// <param name="form">The main form</param>
        /// <param name="video">The video object</param>
        public static void ToggleFullscreen(Form form, Video video)
        {
            // Set form to fullscreen mode and assign the video to the form
            form.FormBorderStyle = FormBorderStyle.None;
            form.WindowState = FormWindowState.Maximized;
            video.Owner = form;
        }

        /// <summary>
        /// Exits fullscreen mode and returns to the normal window size
        /// </summary>
        /// <param name="form">The main form</param>
        /// <param name="video">The video object</param>
        /// <param name="formSize">The initial size of the form</param>
        /// <param name="pnlVideo">The video panel</param>
        /// <param name="pnlSize">The size of the video panel</param>
        public static void ExitFullscreen(Form form, Video video, Size formSize, Panel pnlVideo, Size pnlSize)
        {
            // Restore form to its original size and assign the video back to the panel
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.WindowState = FormWindowState.Normal;
            form.Size = formSize;
            video.Owner = pnlVideo;
            pnlVideo.Size = pnlSize;
        }

        /// <summary>
        /// Adjusts the volume of the video
        /// </summary>
        /// <param name="video">The video object</param>
        /// <param name="trackVolume">The volume TrackBar</param>
        public static void AdjustVolume(Video video, TrackBar trackVolume)
        {
            // Set the video volume based on the TrackBar value
            video.Audio.Volume = trackVolume.Value;
        }

        /// <summary>
        /// Toggles the visibility of the volume TrackBar
        /// </summary>
        /// <param name="trackVolume">The volume TrackBar</param>
        public static void ToggleVolumeTrackBar(TrackBar trackVolume)
        {
            // Toggle the visibility of the volume TrackBar
            trackVolume.Visible = !trackVolume.Visible;
        }

        /// <summary>
        /// Updates the label displaying the current position and duration of the video
        /// </summary>
        /// <param name="video">The video object</param>
        /// <param name="lblVideoPosition">The label displaying the video position</param>
        public static void UpdateVideoPosition(Video video, System.Windows.Forms.Label lblVideoPosition)
        {
            // If no video is loaded, display default time
            if (video == null)
            {
                lblVideoPosition.Text = "00:00:00 / 00:00:00";
                return;
            }

            try
            {
                // Get current position and total duration of the video
                int currentTime = Convert.ToInt32(video.CurrentPosition);
                int maxTime = Convert.ToInt32(video.Duration);

                // Format and display the video position
                lblVideoPosition.Text = string.Format("{0:00}:{1:00}:{2:00}", currentTime / 3600, (currentTime / 60) % 60, currentTime % 60)
                                        + " / " +
                                        string.Format("{0:00}:{1:00}:{2:00}", maxTime / 3600, (maxTime / 60) % 60, maxTime % 60);
            }
            catch (Exception ex)
            {
                lblVideoPosition.Text = "00:00:00 / 00:00:00";
            }
        }
    }
}
