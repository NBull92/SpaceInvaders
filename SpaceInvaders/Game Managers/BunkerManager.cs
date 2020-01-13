using System;
using System.Collections.Generic;
using System.Windows.Controls;
using SpaceInvaders.GameObjects;

namespace SpaceInvaders.Game_Managers
{
    public class BunkerManager
    {
        private Bunker[] _bunkers;

        public BunkerManager()
        {
            Populate();
        }

        public void Populate()
        {
            _bunkers = new Bunker[3];
            var startPosX = 65;
            var posY = 250;

            for (var i = 0; i < 3; i++)
            {
                var currentX = startPosX + (i + 1) * 150;
                _bunkers[i] = new Bunker(currentX, posY);
            }
        }

        public void Render(UIElementCollection canvasChildren)
        {
            for (var i = 0; i < 3; i++)
            {
                var rendered = _bunkers[i].Render();

                if (rendered != null)
                    canvasChildren.Add(rendered);
            }
        }
        
        public void OnGameOver(object sender, EventArgs e)
        {
            for (var i = 0; i < 3; i++)
            {
                _bunkers[i].IsAlive = false;
            }
        }

        public IEnumerable<GameObject> AliveBunkers()
        {
            var list = new List<Bunker>();

            for (var i = 0; i < 3; i++)
            {
                if (_bunkers[i].IsAlive)
                        list.Add(_bunkers[i]);
            }

            return list;
        }

        public void Update()
        {
            var alive = AliveBunkers();
            foreach (var bunker in alive)
                bunker.Update();
        }
    }
}
