using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using SpaceInvaders.GameObjects;
using SpaceInvaders.Iterator;

namespace SpaceInvaders.Game_Managers
{
    public class AlienManager
    {
        private static readonly Random ShootingRandom = new Random();
        private const int AlienColumns = 11;
        private const int AlienRows = 5;
        private readonly IIterator<Alien> _iterator;
        private Alien[,] _aliens;
        private Movement _currentMovement;
        private int _freezeAlienShootUpdate = 25;

        public AlienManager()
        {
            Populate();
            _currentMovement = Movement.Right;
            _iterator = new TwoDimensionalArrayIterator(this);
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
                while (_iterator.HasNext())
                {
                    var current = _iterator.Current();
                    current.Update(Movement.Down);
                    _iterator.Next();
                }

                _iterator.Reset();

                _currentMovement = Movement.Right;
            }
            else if(AnyHitLeftBounds())
            {
                while (_iterator.HasNext())
                {
                    var current = _iterator.Current();
                    current.Update(Movement.Down);
                    _iterator.Next();
                }
                _iterator.Reset();

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

            while (_iterator.HasNext())
            {
                var current = _iterator.Current();
                current.Update(_currentMovement, speedMultiplier);
                if (current.Bullets.Any(o => o.IsAlive))
                {
                    current.Bullets.ForEach(o => o.Update());
                }
                _iterator.Next();
            }
            _iterator.Reset();

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
            while (_iterator.HasNext())
            {
                var current = _iterator.Current();
                var rendered = current.Render();

                if (rendered != null)
                    canvasChildren.Add(rendered);

                var bullets = current.BulletRender();
                if (bullets != null && bullets.Any())
                {
                    bullets.ForEach(o => canvasChildren.Add(o));
                }

                _iterator.Next();
            }
            _iterator.Reset();
        }

        private bool AnyHitRightBounds()
        {
            while (_iterator.HasNext())
            {
                var current = _iterator.Current();
                if (current.IsAlive && current.Position.X < 51)
                    return true;

                _iterator.Next();
            }
            _iterator.Reset();

            return false;
        }

        private bool AnyHitLeftBounds()
        {
            while (_iterator.HasNext())
            {
                var current = _iterator.Current();
                if (current.IsAlive && current.Position.X > 750)
                    return true;

                _iterator.Next();
            }
            _iterator.Reset();

            return false;
        }
        
        public bool AnyAlive()
        {
            while (_iterator.HasNext())
            {
                var current = _iterator.Current();
                if (current.IsAlive)
                    return true;

                _iterator.Next();
            }
            _iterator.Reset();

            return false;
        }

        public List<Alien> AliveAliens()
        {
            var list = new List<Alien>();

            while (_iterator.HasNext())
            {
                var current = _iterator.Current();
                if (current.IsAlive)
                    list.Add(current);

                _iterator.Next();
            }
            _iterator.Reset();

            return list;
        }

        public void ReActivate()
        {
            Populate();
        }

        public bool Landed()
        {
            while (_iterator.HasNext())
            {
                var current = _iterator.Current();
                if (current.IsAlive && current.Position.Y >= 310)
                    return true;

                _iterator.Next();
            }
            _iterator.Reset();

            return false;
        }

        public void OnGameOver(object sender, EventArgs e)
        {
            while (_iterator.HasNext())
            {
                var current = _iterator.Current();
                current.IsAlive = false;
                _iterator.Next();
            }
            _iterator.Reset();
        }

        public class TwoDimensionalArrayIterator : IIterator<Alien>
        {
            private readonly AlienManager _alienManager;
            private int _arrayIndex;
            private int _columnIndex;
            private int _rowIndex;

            public TwoDimensionalArrayIterator(AlienManager alienManager)
            {
                _alienManager = alienManager;
            }

            public void Next()
            {
                if (_columnIndex < AlienColumns-1)
                {
                    _columnIndex++;
                }
                else
                {
                    _columnIndex = 0;
                    if (_rowIndex < AlienRows-1)
                    {
                        _rowIndex++;
                    }
                }

                _arrayIndex++;
            }

            public Alien Current()
            {
                return _alienManager._aliens[_columnIndex, _rowIndex];
            }

            public bool HasNext()
            {
                return _arrayIndex < _alienManager._aliens.Length;
            }

            public void Reset()
            {
                _arrayIndex = 0;
                _columnIndex = 0;
                _rowIndex = 0;
            }
        }
    }
}