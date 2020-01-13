using System;
using System.Windows;
using System.Windows.Input;

namespace SpaceInvaders
{
    public class InputHandler
    {
        private bool _isSpacePressed;
        public event EventHandler<Movement> MovementUpdated = delegate { };
        public event EventHandler ShootEvent = delegate { };
        public event EventHandler ResetGameEvent = delegate { };
        public bool Disabled { get; set; }

        public void Update()
        {
            if (!App.ApplicationIsActivated())
            {
                _isSpacePressed = false;
                return;
            }

            if (Keyboard.IsKeyDown(Key.Escape))
                Application.Current.Shutdown();

            if (Disabled)
            {
                if (Keyboard.IsKeyDown(Key.R))
                {
                    Disabled = false;
                    ResetGameEvent.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.A))
                {
                    MovementUpdated.Invoke(this, Movement.Left);
                    CheckShooting();
                }
                else if (Keyboard.IsKeyDown(Key.D))
                {
                    MovementUpdated.Invoke(this, Movement.Right);
                    CheckShooting();
                }
                else
                {
                    CheckShooting();
                }
            }
        }

        private void CheckShooting()
        {
            if (Keyboard.IsKeyDown(Key.Space) && !_isSpacePressed)
            {
                _isSpacePressed = true;
                ShootEvent.Invoke(this, EventArgs.Empty);
            }
            else if (Keyboard.IsKeyUp(Key.Space) && _isSpacePressed)
            {
                _isSpacePressed = false;
            }
        }
    }
}