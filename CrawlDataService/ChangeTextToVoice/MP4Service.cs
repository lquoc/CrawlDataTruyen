using Common;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using System.Diagnostics;

namespace CrawlDataService
{
    public class MP4Service
    {
        readonly ChangeTextToVoice changeTextToSpeechService;
        public MP4Service(IServiceProvider serviceProvider)
        {
            changeTextToSpeechService = serviceProvider.GetRequiredService<ChangeTextToVoice>();
        }

        public async Task CreateVideoFromMp3AndImages(string contentChapter, string imgNovel, string pathSave, string nameFile)
        {
            try
            {
                var newFileName = nameFile.RemoveInvalidPathChars();
                if (!Directory.Exists(pathSave))
                {
                    Directory.CreateDirectory(pathSave.Replace("\\\\", "\\"));
                }
                var pathCombie = Path.Combine(pathSave, $"{newFileName.Trim()}.mp4");

                // Tạo tệp tạm thời từ MemoryStream của MP3
                var listBytesMp3 = await changeTextToSpeechService.RequestChangeTextToSpeech(contentChapter);
                string tempMp3Path = Path.GetTempFileName() + ".mp3";
                var mp3Stream = CombineMp3FilesToStream(listBytesMp3);
                using (var fileStream = new FileStream(tempMp3Path, FileMode.Create, FileAccess.Write))
                {
                    mp3Stream.CopyTo(fileStream);
                }

                // Tạo tệp tạm thời từ MemoryStream của hình ảnh]
                Image image = imgNovel.ResizeImageToEvenDimensions();
                string tempImagePath = Path.GetTempFileName() + ".png";
                using (var imageStream = ImageToStream(image, new PngEncoder()))
                using (var fileStream = new FileStream(tempImagePath, FileMode.Create, FileAccess.Write))
                {
                    imageStream.CopyTo(fileStream);
                }

                // Tạo video từ hình ảnh và MP3
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = $"-loop 1 -i \"{tempImagePath}\" -i \"{tempMp3Path}\" -c:v libx264 -tune stillimage -c:a aac -b:a 192k -pix_fmt yuv420p -shortest \"{pathCombie}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    process.OutputDataReceived += (sender, e) => RuntimeContext.logger.Info(e.Data);
                    process.ErrorDataReceived += (sender, e) => RuntimeContext.logger.Info(e.Data);

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();


                    // Chờ cho tiến trình FFmpeg kết thúc
                    process.WaitForExit();
                }
                // Xóa các tệp tạm thời
                File.Delete(tempImagePath);
                File.Delete(tempMp3Path);
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"Error while create file mp4, chapter {nameFile}, msg: {ex}");
            }
        }

        public async void TestCreateMP4()
        {
            var tempImagePath = "D:\\TestVoice\\tmpojg5sx.tmp.png";
            var tempMp3Path = "D:\\TestVoice\\tmpaz3ieg.tmp.mp3";
            var pathCombie = "D:\\TestVoice\\output.mp4";
            Stopwatch stopwatch = Stopwatch.StartNew();
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-loop 1 -i \"{tempImagePath}\" -i \"{tempMp3Path}\" -c:v libx264 -tune stillimage -c:a aac -b:a 192k -pix_fmt yuv420p -shortest \"{pathCombie}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine($"Error: {e.Data}");

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();


                // Chờ cho tiến trình FFmpeg kết thúc
                process.WaitForExit();
            }
            stopwatch.Stop();
            RuntimeContext.logger.Info($"Conversion complete! Time taken: {stopwatch.Elapsed.TotalSeconds} seconds");
        }
        public MemoryStream CombineMp3FilesToStream(List<byte[]> mp3Chunks)
        {
            try
            {
                var outputStream = new MemoryStream();
                foreach (var chunk in mp3Chunks)
                {
                    outputStream.Write(chunk, 0, chunk.Length);
                }
                // Đặt con trỏ đến đầu stream để sẵn sàng đọc
                outputStream.Seek(0, SeekOrigin.Begin);
                return outputStream;
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error("Error while Combine Mp3 file to stream");
            }
            return null;
        }
        public MemoryStream ImageToStream(Image image, IImageEncoder encoder)
        {
            try
            {
                var stream = new MemoryStream();
                image.Save(stream, encoder);
                stream.Seek(0, SeekOrigin.Begin); // Đặt con trỏ đến đầu stream
                return stream;
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error("Error while Combine img file to stream");
            }
            return null;
        }
    }
}
