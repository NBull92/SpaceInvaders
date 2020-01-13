using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using SpaceInvaders.GameObjects;

namespace SpaceInvaders.Game_Managers
{
    public class AlienManager
    {
        private static readonly Random ShootingRandom = new Random();
        private const int AlienColumns = 11;
        private const int AlienRows = 5;
        private Alien[,] _aliens;
        private Movement _currentMovement;
        private int _freezeAlienShootUpdate = 25;

        public AlienManager()
        {
            Populate();
            _currentMovement = Movement.Right;
        }

        public void Populate()
        {
            _aliens = new Alien[AlienColumns, AlienRows];
            var startPosX = 250;
            var startPosY = 15;

            for (var i = 0; i < AlienColumns; i++)
            {
                var currentX = startPosX + (i + 1) * 25;
                for (var j = 0; j < AlienRows; j++)
                {
                    var currentY = startPosY + (j + 2) * 25;

                    _aliens[i, j] = new Alien(currentX, currentY);
                }
            }
        }

        public void Update()
        {
            if (AnyHitRightBounds())
            {
                for (var i = 0; i < AlienColumns; i++)
                {
                    for (var j = 0; j < AlienRows; j++)
                    {
                        _aliens[i, j].Update(Movement.Down);
                    }
                }

                _currentMovement = Movement.Right;
            }
            else if(AnyHitLeftBounds())
            {
                for (var i = 0; i < AlienColumns; i++)
                {
                    for (var j = 0; j < AlienRows; j++)
                    {
                        _aliens[i, j].Update(Movement.Down);
                    }
                }

                _currentMovement = Movement.Left;
            }

            var aliensAlive = AliveAliens();
            var count = aliensAlive.Count();
            var speedMultiplier = 1;
            
            if (count <= 11)
            {
                speedMultiplier = 5;
            }
            else if (count <= 22)
            {
                speedMultiplier = 4;
            }
            else if (count <= 33)
            {
                speedMultiplier = 3;
            }
            else if (count <= 44)
            {
                speedMultiplier = 2;
            }

            for (var i = 0; i < AlienColumns; i++)
            {
                for (var j = 0; j < AlienRows; j++)
                {
                    _aliens[i, j].Update(_currentMovement, speedMultiplier);
                    if (_aliens[i, j].Bullets.Any(o => o.IsAlive))
                    {
                        _aliens[i, j].Bullets.ForEach(o => o.Update());
                    }
                }
            }

            RandomAlienShoot();
        }

        private void RandomAlienShoot()
        {
            if (_freezeAlienShootUpdate != 0)
            {
                _freezeAlienShootUpdate--;
                return;
            }

            var shootingAliens = new List<Alien>();
            for (var i = 0; i < AlienColumns; i++)
            {
                bool proceed = false;
                var currentRow = AlienRows - 1;

                while (!proceed)
                {
                    if (_aliens[i, currentRow].IsAlive)
                    {
                        shootingAliens.Add(_aliens[i, currentRow]);
                        proceed = true;
                    }
                    else
                    {
                        if (currentRow != 0)
                        {
                            currentRow--;
                        }
                        else
                        {
                            proceed = true;
                        }
                    }
                }
            }

            if (shootingAliens.Any())
            {
                var r = ShootingRandom.Next(shootingAliens.Count);
                shootingAliens[r].Shoot();
            }

            _freezeAlienShootUpdate = 25;
        }

        public void Render(UIElementCollection canvasChildren)
        {
            for (var i = 0; i < AlienColumns; i++)
            {
                for (var j = 0; j < AlienRows; j++)
                {
                    var alien = _aliens[i, j];
                    var rendered = alien.Render();

                    if (rendered != null)
                        canvasChildren.Add(rendered);

                    var bullets = alien.BulletRender();
                    if (bullets != null && bullets.Any())
                    {
                        bullets.ForEach(o => canvasChildren.Add(o));
                    }
                }
            }
        }

        private bool AnyHitRightBounds()
        {
            for (var i = 0; i < AlienColumns; i++)
            {
                for (var j = 0; j < AlienRows; j++)
                {
                    if (_aliens[i, j].IsAlive && _aliens[i, j].Position.X < 51)
                        return true;
                }
            }

            return false;
        }

        private bool AnyHitLeftBounds()
        {
            for (var i = 0; i < AlienColumns; i++)
            {
                for (var j = 0; j < AlienRows; j++)
                {
                    if (_aliens[i, j].IsAlive && _aliens[i, j].Position.X > 750)
                        return true;
                }
            }

            return false;
        }
        
        public bool AnyAlive()
        {
            for (var i = 0; i < AlienColumns; i++)
            {
                for (var j = 0; j < AlienRows; j++)
                {
                    if (_aliens[i, j].IsAlive)
                        return true;
                }
            }

            return false;
        }

        public List<Alien> AliveAliens()
        {
            var list = new List<Alien>();

            for (var i = 0; i < AlienColumns; i++)
            {
                for (var j = 0; j < AlienRows; j++)
                {
                    if (_aliens[i, j].IsAlive)
                        list.Add(_aliens[i, j]);
                }
            }

            return list;
        }

        public void ReActivate()
        {
            Populate();
        }

        public bool Landed()
        {
            for (var i = 0; i < AlienColumns; i++)
            {
                for (var j = 0; j < AlienRows; j++)
                {
                    if (_aliens[i, j].IsAlive && _aliens[i, j].Position.Y >= 310)
                        return true;
                }
            }

            return false;
        }

        public void OnGameOver(object sender, EventArgs e)
        {
            for (var i = 0; i < AlienColumns; i++)
            {
                for (var j = 0; j < AlienRows; j++)
                {
                    _aliens[i, j].IsAlive = false;
                }
            }
        }
    }
}