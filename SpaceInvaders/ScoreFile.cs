using System;
using System.IO;

namespace SpaceInvaders
{
    public class ScoreFile
    {
        private readonly string _fileName;

        public ScoreFile()
        {
            _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "SpaceInvaders",
                "HighScore.txt");
        }

        public int GetHighScore()
        {
            var fileInfo = new FileInfo(_fileName);
            using (var sr = fileInfo.OpenText())
            {
                int.TryParse(sr.ReadLine(), out var highScore);
                return highScore;
            }
        }

        public void Save(int highScore)
        {
            var fileInfo = new FileInfo(_fileName);

            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                fileInfo.Directory.Create();

            if (fileInfo.Exists)
                fileInfo.Delete();

            using (var sw = fileInfo.CreateText())
            {
                sw.WriteLine(highScore);
            }
        }
    }
}