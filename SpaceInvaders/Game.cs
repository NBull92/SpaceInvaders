using System;
using System.Windows.Controls;
using System.Windows.Media;
using SpaceInvaders.Game_Managers;
using Unity;

namespace SpaceInvaders
{
    public class Game
    {
        private readonly World _characterManager;
        private readonly IHudManager _hudManager;

        public Canvas Canvas { get; set; }

        public Game()
        {
            Canvas = new Canvas { Background = new SolidColorBrush(Colors.Black) };
            _characterManager = new World();
            _hudManager = App.Container.Resolve<IHudManager>();
            _characterManager.GameOver += _hudManager.OnGameOver;
            _characterManager.ResetGame += _hudManager.ResetGame;

            var renderHandler = new FrameRenderer();
            renderHandler.RenderUpdate += OnRenderUpdate;
            renderHandler.Start();
        }

        private void OnRenderUpdate(object sender, EventArgs e)
        {
            _characterManager.Update();
            Canvas.Children.RemoveRange(0, Canvas.Children.Count+1);
            _characterManager.Render(Canvas.Children);
            _hudManager.Render(Canvas.Children);
        }
    }
}