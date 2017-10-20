using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using VoiceTextWebAPI.Client.Internal;

namespace VoiceTextWebAPI.Client
{
    public class VoiceTextClient
    {
        public Speaker Speaker { get; set; } = Speaker.Haruka;

        public Emotion Emotion { get; set; } = Emotion.Default;

        public EmotionLevel EmotionLevel { get; set; } = EmotionLevel.Default;

        public int Pitch { get; set; } = 100;

        public int Speed { get; set; } = 100;

        public int Volume { get; set; } = 100;

        public string APIKey { get; set; } = string.Empty;

        public Uri APIEndPoint { get; set; } = new Uri("https://api.voicetext.jp/v1/tts");

        public byte[] GetVoice(string text)
        {
            var httpClinet = CreateHttpClient();
            var content = BuildHttpRequestContent(text);
            var response = httpClinet.PostAsync(this.APIEndPoint, content).Result;
            if (response.StatusCode != HttpStatusCode.OK) ThrowVoiceTextException(response);
            var bytes = response.Content.ReadAsByteArrayAsync().Result;

            return bytes;
        }

        public Task<byte[]> GetVoiceAsync(string text)
        {
            var httpClinet = CreateHttpClient();
            var content = BuildHttpRequestContent(text);
            return httpClinet.PostAsync(this.APIEndPoint, content)
                .ContinueWith(t =>
                {
                    var response = t.Result;
                    if (response.StatusCode != HttpStatusCode.OK) ThrowVoiceTextException(response);
                    return response.Content.ReadAsByteArrayAsync();
                })
                .Unwrap();
        }

        private HttpClient CreateHttpClient()
        {
            var httpClinet = new HttpClient();
            httpClinet.DefaultRequestHeaders.Add(
                "Authorization",
                "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(this.APIKey)));
            return httpClinet;
        }

        private FormUrlEncodedContent BuildHttpRequestContent(string text)
        {
            var param = new Dictionary<string, string> {
                {"text", text},
                {"speaker", this.Speaker.ToString().ToLower()},
                {"pitch", this.Pitch.ToString()},
                {"speed", this.Speed.ToString()},
                {"volume", this.Volume.ToString()},
            };
            if (this.Emotion != Emotion.Default)
            {
                param.Add("emotion", this.Emotion.ToString().ToLower());
            }
            if (this.EmotionLevel != EmotionLevel.Default)
            {
                param.Add("emotion_level", ((int)this.EmotionLevel).ToString());
            }

            var content = new FormUrlEncodedContent(param);
            return content;
        }

        private static void ThrowVoiceTextException(HttpResponseMessage response)
        {
            var errResponse = new DataContractJsonSerializer(typeof(VoiceTextErrorResponse)).ReadObject(response.Content.ReadAsStreamAsync().Result) as VoiceTextErrorResponse;
            throw new VoiceTextException(errResponse.Error.Message, response.StatusCode);
        }
    }
}
