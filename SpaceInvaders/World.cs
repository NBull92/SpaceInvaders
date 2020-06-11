using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using SpaceInvaders.GameObjects;
using SpaceInvaders.Game_Managers;
using Unity;

namespace SpaceInvaders
{
    public class World
    {
        private readonly IScoreManager _score;
        private readonly AlienManager _aliens;
        private readonly BunkerManager _bunkers;
        private readonly Player _player;
        private bool _waveResetting;

        public event EventHandler GameOver = delegate { };
        public event EventHandler ResetGame = delegate { };

        public World()
        {
            _score = App.Container.Resolve<IScoreManager>();
            _aliens = new AlienManager();
            _bunkers = new BunkerManager();
            _player = new Player(380, 300);

            GameOver += _bunkers.OnGameOver;
            GameOver += _aliens.OnGameOver;
            GameOver += _player.OnGameOver;

            _player.PlayerResetGameEvent += OnResetGame;
        }

        public void Render(UIElementCollection canvasChildren)
        {
            if(_waveResetting)
                return;
            
            Detect();

            var rendered = _player.Render();

            if (rendered != null)
                canvasChildren.Add(rendered);

            var bullets = _player.BulletRender();
            if (bullets != null && bullets.Any())
                bullets.ForEach(o => canvasChildren.Add(o));

            _aliens.Render(canvasChildren);
            _bunkers.Render(canvasChildren);
        }

        public void Update()
        {
            _player.Update();

            if (_waveResetting)
                return;

            _bunkers.Update();
            _aliens.Update();

            if (!_aliens.Landed())
                return;

            _score.SaveHighScore();
            GameOver?.Invoke(this, EventArgs.Empty);
        }

        private void Detect()
        {
            if (_player.Bullets.Any() && _aliens.AnyAlive())
            {
                if (CollisionDetection.CharacterBulletCollided(_player, _aliens.AliveAliens()))
                {
                    _score.UpdateScore();

                    if (!_aliens.AnyAlive())
                    {
                        _waveResetting = true;
                        _aliens.ReActivate();
                        _waveResetting = false;
                    }
                }

                CollisionDetection.PlayerBulletBunkerCollision(_player, _bunkers.AliveBunkers());
            }

            if (!_aliens.AnyAlive())
                return;

            foreach (var alien in _aliens.AliveAliens())
            {
                if (!CollisionDetection.CharacterBulletCollided(alien, new List<Character> {_player}))
                    continue;

                if(!_player.IsAlive)
                    GameOver?.Invoke(this, EventArgs.Empty);
            }

            foreach (var alien in _aliens.AliveAliens())
            {
                CollisionDetection.CharacterBulletCollided(alien, _bunkers.AliveBunkers());
            }
        }

        private void OnResetGame(object sender, EventArgs e)
        {
            _waveResetting = true;
            _aliens.Populate();
            _bunkers.Populate();
            _player.Reset();
            _score.ResetScore();
            ResetGame?.Invoke(this, EventArgs.Empty);
            _waveResetting = false;
        }
    }
}