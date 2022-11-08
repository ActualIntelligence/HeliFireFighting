using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    internal class Cloud
    {
        public double X;
        public double Y;
        public int Width = 64;
        public int Height = 32;
        public bool IsForeGound = false;

        Texture2D cloudTexture;
        World world;

        public Cloud(Texture2D texture, World gameWorld)
        {
           cloudTexture = texture;
            world = gameWorld;
        }


        public void Draw()
        {
            world.DrawInWorld(cloudTexture, (float)X, (float) Y, Width, Height, 0, 0);
        }
    }
}

