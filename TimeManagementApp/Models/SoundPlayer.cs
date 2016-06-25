using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using log4net;
namespace TimeManagementApp.Models
{
    public class SoundPlayer
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SoundPlayer));
        private Uri soundUri;
        private IWavePlayer waveOutDevice = new WaveOut();
        private Mp3FileReader audioFileReader;
        private Stream soundStream;
        public SoundPlayer(Uri soundUri)
        {
            this.soundUri = soundUri;
        }
        
        public void Initialize()
        {
            try
            {
                SetupPlayback();
            }
            catch(Exception ex)
            {
                logger.Error("Sound Player Error", ex);
                //TODO: Logging
                isValid = false;
            }
        }

        private void SetupPlayback()
        {
            try
            {
                if(soundStream != null)
                {
                    soundStream.Dispose();
                    soundStream = null;
                }

                if(audioFileReader != null)
                {
                    audioFileReader.Dispose();
                    audioFileReader = null;
                }

                var soundStreamInfo = System.Windows.Application.GetResourceStream(soundUri);
                soundStream = soundStreamInfo.Stream;

                audioFileReader = new Mp3FileReader(soundStream);
                waveOutDevice.Init(audioFileReader);
                isValid = true;
            }
            catch (Exception ex)
            {
                logger.Error("Sound Player Error", ex);
                //TODO: Logging
                isValid = false;
            }
        }

        public void Cleanup()
        {
            isValid = false;
            if(soundStream != null)
            {
                soundStream.Dispose();
                soundStream = null;
            }

            if(audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }

            if(waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
        }

        public void Reset()
        {
            hasPlayed = false;
            SetupPlayback();
        }

        public void Play()
        {
            if (!isValid || hasPlayed)
            {
                logger.Debug(String.Format("SoundPlayer is not valid or has played: {0} : {1}", isValid, hasPlayed));
                return;
            }

            logger.Debug("Playing sound");
            waveOutDevice.Stop();
            waveOutDevice.Play();
            hasPlayed = true;
        }

        private bool isValid = false;
        public bool IsValid { get { return isValid; } }

        private bool hasPlayed = false;
        public bool HasPlayed { get { return hasPlayed; } }
    }
}
