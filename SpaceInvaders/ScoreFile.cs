using System;
using System.IO;

namespace SpaceInvaders
{
    public class ScoreFile
    {
        private FileInfo _fileInfo;

        public ScoreFile()
        {
            CreateFile();
        }

        private void CreateFile()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SpaceInvaders",
                "HighScore.txt");

            _fileInfo = new FileInfo(fileName);

            if (!_fileInfo.Directory.Exists)
            {
                _fileInfo.Directory.Create();
            }

            if (!_fileInfo.Exists)
            {
                var stream = _fileInfo.Create();
                stream.Dispose();
            }
        }

        public int GetHighScore()
        {
            using (var sr = _fileInfo.OpenText())
            {
                int.TryParse(sr.ReadLine(), out var highScore);
                return highScore;
            }
        }

        public void Save(int highScore)
        {

            if (_fileInfo.Directory != null && !_fileInfo.Directory.Exists)
                _fileInfo.Directory.Create();

            if (_fileInfo.Exists)
                _fileInfo.Delete();

            using (var sw = _fileInfo.CreateText())
            {
                sw.WriteLine(highScore);
            }
        }
    }
}