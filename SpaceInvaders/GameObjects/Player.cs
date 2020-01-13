using System;
using System.Linq;
using System.Windows;
using SpaceInvaders.GameObjects.Bullets;
using SpaceInvaders.Game_Managers;
using Unity;

namespace SpaceInvaders.GameObjects
{
    public class Player : Character
    {
        private readonly IHudManager _hudManager;
        private readonly InputHandler _inputHandler;
        private int _freezePlayerShoot;

        public event EventHandler PlayerResetGameEvent = delegate { };
        public int Lives;

        public Player(double x, double y) : base(x, y)
        {
            _hudManager = App.Container.Resolve<IHudManager>();
            Lives = 3;
            _hudManager.UpdateLives(Lives);
            CurrentSpeed = 4;
            _inputHandler = new InputHandler();
            _inputHandler.MovementUpdated += OnMovementUpdated;
            _inputHandler.ShootEvent += OnShoot;
            _inputHandler.ResetGameEvent += OnResetGame;
        }

        private void OnResetGame(object sender, EventArgs e)
        {
            PlayerResetGameEvent.Invoke(this, EventArgs.Empty);
        }

        private void OnShoot(object sender, EventArgs e)
        {
            if (_freezePlayerShoot != 0)
                return;

            Bullets.Add(new PlayerBullet(Collider.Center.X, Collider.Center.Y - Collider.Height / 2));
            _freezePlayerShoot = 15;
        }

        private void OnMovementUpdated(object sender, Movement updatedMovement)
        {
            switch (updatedMovement)
            {
                case Movement.Left:
                    Position = new Point(Position.X - CurrentSpeed, Position.Y);
                    break;

                case Movement.Right:
                    Position = new Point(Position.X + CurrentSpeed, Position.Y);
                    break;
            }
        }
        
        public override void Update()
        {
            _inputHandler.Update();

            if (_freezePlayerShoot != 0)
                _freezePlayerShoot--;

            if (Bullets.Any())
                Bullets.ForEach(o => o.Update());

            base.Update();
        }

        public void OnGameOver(object sender, EventArgs e)
        {
            _inputHandler.Disabled = true;
        }

        public override void HandleCollision()
        {
            if (Lives > 1)
            {
                Lives--;
                _hudManager.UpdateLives(Lives);
            }
            else
            {
                IsAlive = false;
                Bullets.Clear();
                _hudManager.UpdateLives(0);
            }
            base.HandleCollision();
        }

        public void Reset()
        {
            Position = new Point(400, 300);
            Lives = 3;
            _hudManager.UpdateLives(Lives);
        }
    }
}