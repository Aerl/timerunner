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
    class Dragon: Monster
    {
        string assetName = "dragon";
        string deadAssetName = "deadDragon";
        string soundName = "DragonSound";
        int moveLeft = -1;
        int moveRight = 1;
        int moveUp = -2;
        int moveDown = 2;
        int health = 1;

        public Dragon(int speed)
        {
            Random r = new Random();
            Vector2 mStartingPosition = new Vector2(1200, r.Next(200,800));
            Setup(speed, assetName, deadAssetName, moveLeft, moveRight, moveUp, moveDown, health, mStartingPosition, State.Flying, soundName);
        }
    }
}
