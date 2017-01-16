using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DyczkiPhotos
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = File.ReadAllText("C:\\fb.txt");
            List<string> splitText = text.Split('\"').ToList();

            List<string> links = (from line in splitText where line.StartsWith("https://scontent") select line.Replace("amp;", "")).ToList();

            Task[] tasks = new Task[4];
            int count = links.Count/4;
            for (int i = 0; i < 4;)
            {
                i++;
                List<string> partLinks = links.Take(count).ToList();
                if(i != 4) links.RemoveRange(0, count);
                tasks[i-1] = new Task(() => DownloadPhotos(partLinks));
                tasks[i-1].Start();
            }

            Task.WaitAll(tasks);

            Console.WriteLine("Done");

            Console.ReadLine();
        }

        private static void DownloadPhotos(List<string> links)
        {
            foreach (string image in links)
            {
                string ending = ".jpg";
                if (image.Contains("png")) ending = ".png";
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(image), @"C:\fbphotos\" + Guid.NewGuid() + ending);
                    Console.WriteLine("Downloaded - " + image);
                }
            }
        }
    }
}