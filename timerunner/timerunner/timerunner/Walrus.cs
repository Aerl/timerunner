using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace timerunner
{
    class Walrus : Monster
    {
        string assetName = "walrus";
        string deadAssetName = "deadWalrus";
        int moveLeft = -2;
        int moveRight = 1;
        int health = 2;

        public Walrus(int speed)
        {
            Vector2 mStartingPosition = new Vector2(900, 0);
            Setup(speed, assetName, deadAssetName, moveLeft, moveRight, 0, 0, health, mStartingPosition, State.Falling);
        }
    }
}
