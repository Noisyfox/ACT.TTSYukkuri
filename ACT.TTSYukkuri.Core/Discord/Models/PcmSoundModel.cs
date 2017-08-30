using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NAudio.Wave;

namespace ACT.TTSYukkuri.Discord.Models
{
    public class PcmSoundModel
    {
        #region Singleton

        private static PcmSoundModel instance = new PcmSoundModel();

        public static PcmSoundModel Instance => instance;

        #endregion Singleton

        private string FFMpeg =>
            Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ffmpeg.exe");

        private readonly WaveFormat PcmWaveFormat = new WaveFormat(48000, 16, 2);

        private Dictionary<string, byte[]> pcms = new Dictionary<string, byte[]>();

        public byte[] GetPcmSound(
            string file)
        {
            if (!File.Exists(file))
            {
                return null;
            }

            var pcmBuffer = new byte[3840];

            lock (this)
            {
                byte[] data;
                if (this.pcms.TryGetValue(file, out data))
                {
                    return data;
                }

                var pi = new ProcessStartInfo
                {
                    FileName = this.FFMpeg,
                    Arguments = $"-i \"{file}\" -ac 2 -f s16le -ar 48000 pipe:1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };

                var ffmpeg = Process.Start(pi);
                var ffout = ffmpeg.StandardOutput.BaseStream;

                // lets buffer ffmpeg output
                using (var ms = new MemoryStream())
                {
                    ffout.CopyTo(ms);
                    ms.Position = 0;

                    // buffer to hold the PCM data
                    var br = 0;
                    while ((br = ms.Read(pcmBuffer, 0, pcmBuffer.Length)) > 0)
                    {
                        // it's possible we got less than expected, let's null the remaining part of the buffer
                        if (br < pcmBuffer.Length)
                        {
                            for (var i = br; i < pcmBuffer.Length; i++)
                            {
                                pcmBuffer[i] = 0;
                            }
                        }
                    }
                }

                this.pcms.Add(file, pcmBuffer);
            }

            return pcmBuffer;
        }
    }
}
