using Amazon.Polly;
using Common;
using Google.Cloud.TextToSpeech.V1;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace CrawlDataService
{
    public class ChangeTextToVoice
    {
        public async Task RequestCreateSpeechAWS(string longText, string path, string nameFile)
        {
            try
            {
                var pathCombie = Path.Combine(path.Trim(), nameFile.RemoveInvalidPathChars().Trim()).Trim();
                var client = RuntimeContext._serviceProvider.GetRequiredService<IAmazonPolly>();
                using (var fileStream = new FileStream($"{pathCombie}.mp3", FileMode.Create, FileAccess.Write))
                {
                    int chunkSize = 3000;
                    int start = 0;
                    while (start < longText.Length)
                    {
                        int length = Math.Min(chunkSize, longText.Length - start);
                        if (start + length < longText.Length && !char.IsWhiteSpace(longText[start + length]))
                        {
                            int lastSpace = longText.LastIndexOf(' ', start + length);
                            if (lastSpace > start)
                            {
                                length = lastSpace - start;
                            }
                        }
                        string textChunk = longText.Substring(start, length);
                        var request = new Amazon.Polly.Model.SynthesizeSpeechRequest
                        {
                            Text = textChunk,
                            OutputFormat = OutputFormat.Mp3,
                            VoiceId = VoiceId.Joanna
                        };
                        var response = await client.SynthesizeSpeechAsync(request);
                        response.AudioStream.CopyTo(fileStream);
                        start += length;
                    }
                }
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"Error while create file mp3, msg :{ex}");
            }
        }
        public async Task<List<byte[]>?> RequestChangeTextToSpeech(string longText = "")
        {
            try
            {
                longText = longText.RemoveCharSpecial();
                var parts = SplitTextByBytes(longText, 5000);
                var client = new TextToSpeechClientBuilder
                {
                    CredentialsPath = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "KeyAPIGGCloud"), "*.json").FirstOrDefault()
                }.Build();


                List<byte[]> audioParts = new List<byte[]>();

                foreach (var part in parts)
                {
                    SynthesisInput input = new SynthesisInput
                    {
                        Text = part
                    };

                    VoiceSelectionParams voice = new VoiceSelectionParams
                    {
                        LanguageCode = "vi-VN",
                        Name = "vi-VN-Wavenet-C",
                        SsmlGender = SsmlVoiceGender.Female
                    };

                    AudioConfig config = new AudioConfig
                    {
                        AudioEncoding = AudioEncoding.Mp3
                    };

                    var response = client.SynthesizeSpeech(input, voice, config);
                    audioParts.Add(response.AudioContent.ToByteArray());
                }
                return audioParts;
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"Error while create file mp3, msg :{ex}");
            }
            return null;
        }

        public async Task RequestCreateSpeechGoogleCloudy(string longText = "", string path = "", string nameFile = "")
        {
            try
            {
                longText = longText.RemoveCharSpecial();
                RuntimeContext.logger.Info($"Start create file mp3 for chapter {nameFile}");
                var pathCombie = Path.Combine(path.Trim(), nameFile.RemoveInvalidPathChars().Trim()).Trim();
                var parts = SplitTextByBytes(longText, 5000);
                var client = new TextToSpeechClientBuilder
                {
                    CredentialsPath = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "KeyAPIGGCloud"), "*.json").FirstOrDefault()
                }.Build();

                List<byte[]> audioParts = new List<byte[]>();

                foreach (var part in parts)
                {
                    SynthesisInput input = new SynthesisInput
                    {
                        Text = part
                    };

                    VoiceSelectionParams voice = new VoiceSelectionParams
                    {
                        LanguageCode = "vi-VN",
                        SsmlGender = SsmlVoiceGender.Female
                    };

                    AudioConfig config = new AudioConfig
                    {
                        AudioEncoding = AudioEncoding.Mp3
                    };

                    var response = client.SynthesizeSpeech(input, voice, config);
                    audioParts.Add(response.AudioContent.ToByteArray());
                }
                using (var output = File.Create($"{pathCombie}.mp3"))
                {
                    foreach (var part in audioParts)
                    {
                        output.Write(part, 0, part.Length);
                    }
                }
                RuntimeContext.logger.Info($"End create file mp3 for chapter {nameFile}");
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"Error while create file mp3, msg :{ex}");
            }
        }

        public List<string> SplitTextByBytes(string text, int maxBytes)
        {
            var result = new List<string>();
            int textLength = text.Length;
            int start = 0;

            while (start < textLength)
            {
                int length = 1;
                while (length <= maxBytes && (start + length) <= textLength)
                {
                    string part = text.Substring(start, length);
                    byte[] partBytes = Encoding.UTF8.GetBytes(part);

                    if (partBytes.Length > maxBytes)
                    {
                        break;
                    }

                    length++;
                }

                result.Add(text.Substring(start, length - 1));
                start += length - 1;
            }

            return result;
        }
    }
}
