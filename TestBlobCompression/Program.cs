namespace TestBlobCompression
{
    using System;
    using System.IO;

    class Program
    {

        static void Main(string[] args)
        {
            ConvertToChunks();
            Console.Read();
        }

        private static void ConvertToChunks()
        {

            string file = "microsoft-wallpaper-6.jpg";
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            using (var stream = new BufferedStream(fileStream))
            {
                int chunkSize = 1024;

                string fileName = Guid.NewGuid() + Path.GetExtension(file);
                int totalChunks = (int)Math.Ceiling((double)stream.Length / chunkSize);

                for (int i = 0; i < totalChunks; i++)
                {
                    int startIndex = i * chunkSize;
                    int endIndex = (int)(startIndex + chunkSize > stream.Length ? stream.Length : startIndex + chunkSize);
                    int length = endIndex - startIndex;

                    byte[] bytes = new byte[length];
                    stream.Read(bytes, 0, bytes.Length);
                    Console.WriteLine("Length: " + bytes.Length + " - " + i + " out of " + totalChunks);
                    var data = Convert.ToBase64String(bytes);
                    ChunkRequest("new_" + fileName, data);
                }
                Console.WriteLine("Completed. Filename: " + fileName);
            }
        }

        private static void ChunkRequest(string fileName, string data)
        {
            byte[] buffer = Convert.FromBase64String(data);
            SaveFile(fileName, buffer);
        }


        public static void SaveFile(string fileName, byte[] buffer)
        {
            using(FileStream writer = new FileStream(fileName, File.Exists(fileName) ? FileMode.Append : FileMode.Create, FileAccess.Write))
            {
                writer.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
