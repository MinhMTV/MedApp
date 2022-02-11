using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace CBMTraining.Extensions
{
    public static class FileExtensions
    {
        private static readonly string LocalFolder;

        static FileExtensions()
        {
            // Gets the target platform's valid save location
            LocalFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        public static string getFolderPathDoc()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
        public static string getFolderPathShare()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        public static string getFolderPathPictures()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }




        // Byte[] extension methods

        public static async Task<string> SaveToLocalFolderAsync(this byte[] dataBytes, string fileName,string root)
        {
            return await Task.Run(() =>
            {
                // Use Combine so that the correct file path slashes are used
                var filePath = Path.Combine(root, fileName);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                File.WriteAllBytes(filePath, dataBytes);

                return filePath;
            });
        }

        public static async Task<byte[]> LoadFileBytesAsync(string filePath)
        {
            return await Task.Run(() => File.ReadAllBytes(filePath));
        }

        // Stream extension methods

        public static async Task<string> SaveToLocalFolderAsync(this Stream dataStream, string fileName, string root)
        {
            // Use Combine so that the correct file path slashes are used
            var filePath = Path.Combine(root, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (var fileStream = File.OpenWrite(filePath))
            {
                if (dataStream.CanSeek)
                    dataStream.Position = 0;

                await dataStream.CopyToAsync(fileStream);

                return filePath;
            }
        }

        public static async Task<Stream> LoadFileStreamAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    return fileStream;
                }
            });
        }
    }
}