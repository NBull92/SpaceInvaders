using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Unity;

namespace SpaceInvaders.Game_Managers
{
    public class HudManager : IHudManager
    {
        private readonly IScoreManager _scoreManager;
        private readonly TextBlock _scoreLabel;
        private readonly TextBlock _score;
        private readonly TextBlock _highScoreLabel;
        private readonly TextBlock _highScore;
        private readonly TextBlock _livesLabel;
        private readonly TextBlock _gameOverLabel;
        private readonly TextBlock _resetLabel;

        private bool _gameOver;
        private int _lives;

        public HudManager()
        {
            _scoreManager = App.Container.Resolve<IScoreManager>();

            _scoreLabel = new TextBlock
            {
                Text = "Score:",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White,
                Margin = new Thickness(325, 10, 0, 0)
            };

            _score = new TextBlock
            {
                Text = "0000",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White,
                Margin = new Thickness(325, 25, 0, 0)
            };

            _highScoreLabel = new TextBlock
            {
                Text = "HighScore:",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White,
                Margin = new Thickness(420, 10, 0, 0)
            };

            _highScore = new TextBlock
            {
                Text = "0000:",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White,
                Margin = new Thickness(420, 25, 0, 0)
            };

            _livesLabel = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White,
                Margin = new Thickness(10, 335, 0, 0)
            };
            
            _gameOverLabel = new TextBlock
            {
                Text = "Game Over",
                FontSize = 30,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White,
                Margin = new Thickness(310, 150, 0, 0)
            };

            _resetLabel = new TextBlock
            {
                Text = "To replay, press R",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White,
                Margin = new Thickness(340, 180, 0, 0)
            };
        }

        public void Render(UIElementCollection canvasChildren)
        {
            if (_gameOver)
            {
                canvasChildren.Add(_resetLabel);
                canvasChildren.Add(_gameOverLabel);
            }

            canvasChildren.Add(_scoreLabel);
            canvasChildren.Add(_highScoreLabel);
            _score.Text = _scoreManager.CurrentScore();
            _highScore.Text = _scoreManager.HighScore();
            canvasChildren.Add(_score);
            canvasChildren.Add(_highScore);

            _livesLabel.Text = $"Lives: {_lives}";

            canvasChildren.Add(_livesLabel);
        }

        public void OnGameOver(object sender, EventArgs e)
        {
            _gameOver = true;
        }

        public void ResetGame(object sender, EventArgs e)
        {
            _gameOver = false;
        }

        public void UpdateLives(int lives)
        {
            _lives = lives;
        }
    }
}