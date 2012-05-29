using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WPExtensions.Audio
{
    public delegate void ContentEventHandler(object sender, byte[] content);

    public class MicrophoneWrapper : IMicrophoneWrapper
    {
        private Microphone microphone = Microphone.Default;     
        private byte[] buffer;                                  
        private MemoryStream stream = new MemoryStream();       
        private SoundEffectInstance soundInstance;              
        private bool soundIsPlaying = false;                    

       
        

        public MicrophoneWrapper()
        {
            
            var dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();

            microphone.BufferReady += microphone_BufferReady;
        }

       
        void dt_Tick(object sender, EventArgs e)
        {
            try
            {
                FrameworkDispatcher.Update();
            }
            catch (NotSupportedException)
            { }

            if (true == soundIsPlaying)
            {
                if (soundInstance.State != SoundState.Playing)
                {
                    // Audio has finished playing
                    soundIsPlaying = false;
                    if(PlayComplete!=null)
                    {
                        PlayComplete(this, null);
                    }
                }
            }
        }

        public event RoutedEventHandler PlayComplete;

       
        void microphone_BufferReady(object sender, EventArgs e)
        {
            // Retrieve audio data
            microphone.GetData(buffer);

            // Store the audio data in a stream
            stream.Write(buffer, 0, buffer.Length);

            

            if (MicrophoneEventLevel != null)
            {
                MicrophoneEventLevel(this, buffer);
            }
        }


        public event ContentEventHandler MicrophoneEventLevel;

       
        public void Record()
        {
            // Get audio data in 1/2 second chunks
            microphone.BufferDuration = TimeSpan.FromMilliseconds(500);

            // Allocate memory to hold the audio data
            buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];

            // Set the stream back to zero in case there is already something in it
            stream.SetLength(0);

            microphone.Start();
        }

        public MicrophoneState MicrophoneState
        {
            get { return microphone.State; }
        }

       
        public void Stop()
        {
            if (microphone.State == MicrophoneState.Started)
            {
                // In RECORD mode, user clicked the 
                // stop button to end recording
                microphone.Stop();

                // Start recording
                if (MicrophoneEventLevel != null)
                {
                    var memoryStream = new MemoryStream();
                    WriteWavHeader(memoryStream, microphone.SampleRate);
                    MicrophoneEventLevel(this, memoryStream.ToArray());
                }

                //UpdateWavHeader(stream);
            }
            else if (soundInstance.State == SoundState.Playing)
            {
                // In PLAY mode, user clicked the 
                // stop button to end playing back
                soundInstance.Stop();
            }

        }

        
        public void Play()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => { 
                if (stream.Length > 0)
                {
               
                    // Play the audio in a new thread so the UI can update.
                    var soundThread = new Thread(new ThreadStart(playSound));
                    soundThread.Start();
                }
            });
        }

        public byte[] GetContent()
        {
            return stream.ToArray();
        }

        public byte[] GetWavContent()
        {
            var memoryStream = new MemoryStream();
            WriteWavHeader(memoryStream,microphone.SampleRate);
            byte[] array = stream.ToArray();
            memoryStream.Write(array,0,array.Length);
            UpdateWavHeader(memoryStream);
            return memoryStream.ToArray();
        }

        
        private void playSound()
        {
            // Play audio using SoundEffectInstance so we can monitor it's State 
            // and update the UI in the dt_Tick handler when it is done playing.
            var sound = new SoundEffect(stream.ToArray(), microphone.SampleRate, AudioChannels.Mono);
            soundInstance = sound.CreateInstance();
            soundIsPlaying = true;
            soundInstance.Play();
        }

        


        public void WriteWavHeader(Stream stream, int sampleRate)
        {
            const int bitsPerSample = 16;
            const int bytesPerSample = bitsPerSample / 8;
            var encoding = System.Text.Encoding.UTF8;
            // ChunkID Contains the letters "RIFF" in ASCII form (0x52494646 big-endian form).
            stream.Write(encoding.GetBytes("RIFF"), 0, 4);

            // NOTE this will be filled in later
            stream.Write(BitConverter.GetBytes(0), 0, 4);

            // Format Contains the letters "WAVE"(0x57415645 big-endian form).
            stream.Write(encoding.GetBytes("WAVE"), 0, 4);

            // Subchunk1ID Contains the letters "fmt " (0x666d7420 big-endian form).
            stream.Write(encoding.GetBytes("fmt "), 0, 4);

            // Subchunk1Size 16 for PCM.  This is the size of therest of the Subchunk which follows this number.
            stream.Write(BitConverter.GetBytes(16), 0, 4);

            // AudioFormat PCM = 1 (i.e. Linear quantization) Values other than 1 indicate some form of compression.
            stream.Write(BitConverter.GetBytes((short)1), 0, 2);

            // NumChannels Mono = 1, Stereo = 2, etc.
            stream.Write(BitConverter.GetBytes((short)1), 0, 2);

            // SampleRate 8000, 44100, etc.
            stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);

            // ByteRate =  SampleRate * NumChannels * BitsPerSample/8
            stream.Write(BitConverter.GetBytes(sampleRate * bytesPerSample), 0, 4);

            // BlockAlign NumChannels * BitsPerSample/8 The number of bytes for one sample including all channels.
            stream.Write(BitConverter.GetBytes((short)(bytesPerSample)), 0, 2);

            // BitsPerSample    8 bits = 8, 16 bits = 16, etc.
            stream.Write(BitConverter.GetBytes((short)(bitsPerSample)), 0, 2);

            // Subchunk2ID Contains the letters "data" (0x64617461 big-endian form).
            stream.Write(encoding.GetBytes("data"), 0, 4);

            // NOTE to be filled in later
            stream.Write(BitConverter.GetBytes(0), 0, 4);
        }

        public void UpdateWavHeader(Stream stream)
        {
            if (!stream.CanSeek) throw new Exception("Can't seek stream to update wav header");

            var oldPos = stream.Position;

            // ChunkSize  36 + SubChunk2Size
            stream.Seek(4, SeekOrigin.Begin);
            stream.Write(BitConverter.GetBytes((int)stream.Length - 8), 0, 4);

            // Subchunk2Size == NumSamples * NumChannels * BitsPerSample/8 This is the number of bytes in the data.
            stream.Seek(40, SeekOrigin.Begin);
            stream.Write(BitConverter.GetBytes((int)stream.Length - 44), 0, 4);

            stream.Seek(oldPos, SeekOrigin.Begin);
        }

    }
}
