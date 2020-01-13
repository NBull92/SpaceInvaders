using System;
using System.Windows.Controls;

namespace SpaceInvaders.Game_Managers
{
    public interface IHudManager
    {
        void Render(UIElementCollection canvasChildren);
        void OnGameOver(object sender, EventArgs e);
        void ResetGame(object sender, EventArgs e);
        void UpdateLives(int lives);
    }
}