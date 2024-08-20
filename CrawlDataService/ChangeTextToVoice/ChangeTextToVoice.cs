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
                var pathCombie = Path.Combine(path, nameFile);
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
                var parts = SplitTextByBytes(longText, 5000);
                var client = new TextToSpeechClientBuilder
                {
                    CredentialsPath = Path.Combine(AppContext.BaseDirectory, "KeyAPIGGCloud", "soy-extension-432808-v1-3442f5169050.json")
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
                return audioParts;
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"Error while create file mp3, msg :{ex}");
            }
            return null;
        }

        public async Task RequestCreateSpeechGoogleCloudy(string longText = "", string path = "", string nameFile = "", int indexChapter = 0)
        {
            try
            {
                RuntimeContext.logger.Info($"Start create file mp3 for chapter {indexChapter}");
                var pathCombie = Path.Combine(path, nameFile);
                var parts = SplitTextByBytes(longText, 5000);
                var client = new TextToSpeechClientBuilder
                {
                    CredentialsPath = Path.Combine(AppContext.BaseDirectory, "KeyAPIGGCloud", "soy-extension-432808-v1-3442f5169050.json")
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
                RuntimeContext.logger.Info($"End create file mp3 for chapter {indexChapter}");
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"Error while create file mp3, msg :{ex}");
            }
        }

        static List<string> SplitTextByBytes(string text, int maxBytes)
        {
            List<string> parts = new List<string>();
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            int currentIndex = 0;
            while (currentIndex < textBytes.Length)
            {
                int bytesToTake = Math.Min(maxBytes, textBytes.Length - currentIndex);
                int endIndex = currentIndex + bytesToTake;
                while (endIndex > currentIndex && (textBytes[endIndex - 1] & 0xC0) == 0x80)
                {
                    endIndex--;
                }
                if (endIndex == currentIndex)
                {
                    throw new Exception("Unable to split the text without breaking a UTF-8 character.");
                }
                string part = Encoding.UTF8.GetString(textBytes, currentIndex, endIndex - currentIndex);
                parts.Add(part);
                currentIndex = endIndex;
            }

            return parts;
        }
    }
}
