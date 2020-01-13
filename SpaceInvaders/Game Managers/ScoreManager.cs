namespace SpaceInvaders.Game_Managers
{
    public class ScoreManager : IScoreManager
    {
        private readonly ScoreFile _scoreFile;
        private int _currentScore;
        private int _highScore;

        public ScoreManager()
        {
            _scoreFile = new ScoreFile();
            _highScore = _scoreFile.GetHighScore();
        }

        public void SaveHighScore()
        {
            if (_currentScore <= _highScore)
                return;

            _highScore = _currentScore;

            _scoreFile.Save(_highScore);
        }

        public string HighScore()
        {
            return _highScore.ToString();
        }

        public void UpdateScore()
        {
            _currentScore += 10;
        }

        public void ResetScore()
        {
            _currentScore = 0;
        }

        public string CurrentScore()
        {
            return _currentScore.ToString();
        }
    }
}