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
        public static void LoadVideos(ListBox lstVideos, string folderPath, out string[] videoPaths, out int selectedIndex)
        {
            videoPaths = System.IO.Directory.GetFiles(folderPath, "*.wmv");
            selectedIndex = 0;

            if (videoPaths != null)
            {
                foreach (string path in videoPaths)
                {
                    string vid = path.Replace(folderPath, string.Empty);
                    vid = vid.Replace(".wmv", string.Empty);
                    lstVideos.Items.Add(vid);
                }
            }
            lstVideos.SelectedIndex = selectedIndex;
        }

        public static void SelectVideo(ListBox lstVideos, Panel pnlVideo, Size pnlSize, ref Video video,
                                        string[] videoPaths, System.Windows.Forms.Label lblVideo, 
                                        Button btnPlayPause, Timer tmrVideo, Action nextVideo)
        {
            try
            {
                video.Stop();
                video.Dispose();
            }
            catch { }

            int index = lstVideos.SelectedIndex;
            video = new Video(videoPaths[index], false);
            video.Owner = pnlVideo;
            pnlVideo.Size = pnlSize;
            video.Play();
            tmrVideo.Enabled = true;
            btnPlayPause.Text = "Pause";
            video.Ending += (sender, e) => Video_Ending(nextVideo);
            lblVideo.Text = lstVideos.Text;
        }

        private static void Video_Ending(Action nextVideo)
        {
            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(2000);
                nextVideo();
            });
        }

        public static void NextVideo(ListBox lstVideos, int videoCount, Action<int> setSelectedIndex)
        {
            int index = lstVideos.SelectedIndex;
            index = (index + 1) % videoCount;
            setSelectedIndex(index);
        }

        public static void PreviousVideo(ListBox lstVideos, int videoCount, Action<int> setSelectedIndex)
        {
            int index = lstVideos.SelectedIndex;
            index = (index - 1 + videoCount) % videoCount;
            setSelectedIndex(index);
        }

        public static void PlayPauseVideo(Video video, Timer tmrVideo, Button btnPlayPause)
        {
            if (video == null)
            {
                MessageBox.Show("No video is currently loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

        public static void ToggleFullscreen(Form form, Video video)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.WindowState = FormWindowState.Maximized;
            video.Owner = form;
        }

        public static void ExitFullscreen(Form form, Video video, Size formSize, Panel pnlVideo, Size pnlSize)
        {
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.WindowState = FormWindowState.Normal;
            form.Size = formSize;
            video.Owner = pnlVideo;
            pnlVideo.Size = pnlSize;
        }

        public static void AdjustVolume(Video video, TrackBar trackVolume)
        {
            video.Audio.Volume = trackVolume.Value;
        }

        public static void ToggleVolumeTrackBar(TrackBar trackVolume)
        {
            trackVolume.Visible = !trackVolume.Visible;
        }

        public static void UpdateVideoPosition(Video video, System.Windows.Forms.Label lblVideoPosition)
        {
            if (video == null)
            {
                lblVideoPosition.Text = "00:00:00 / 00:00:00";
                return;
            }

            try
            {
                int currentTime = Convert.ToInt32(video.CurrentPosition);
                int maxTime = Convert.ToInt32(video.Duration);

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
