namespace SliceFile
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class StartUp
    {
        public static void Main()
        {
            var file = Console.ReadLine();
            var destinationFolder = Console.ReadLine();
            var pieces = int.Parse(Console.ReadLine());

            SliceAsync(file, destinationFolder, pieces);
            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void SliceAsync(string file, string destinationFolder, int pieces)
        {
            Task.Run(() =>
            {
                Slice(file, destinationFolder, pieces);
            });
        }

        private static void Slice(string file, string destinationFolder, int pieces)
        {
            var directoryPath = $"{Directory.GetCurrentDirectory()}\\{destinationFolder}";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var fileStream = new FileStream(file, FileMode.Open))
            {
                var fileInfo = new FileInfo(file);
                var partLength = (fileStream.Length / pieces) + 1;
                var currentByte = 0;

                for (int i = 1; i <= pieces; i++)
                {
                    var path = string.Format($"{directoryPath}\\Part-{i}{fileInfo.Extension}");
                    using (var destination = new FileStream(path, FileMode.Create))
                    {
                        byte[] buffer = new byte[4096];
                        while (currentByte <= partLength * i)
                        {
                            var readBytesCount = fileStream.Read(buffer, 0, buffer.Length);
                            if (readBytesCount == 0)
                            {
                                break;
                            }

                            destination.Write(buffer, 0, readBytesCount);
                            currentByte += readBytesCount;
                        }
                    }
                }
            }

            Console.WriteLine("Slice complete.");
        }
    }
}