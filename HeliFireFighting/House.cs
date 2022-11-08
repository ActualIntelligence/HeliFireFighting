using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    internal class House
    {
        public int X;
        public int Y;
        public int Width = 64;
        public int Height = 32;

        World world;
        Texture2D houseTexture;

        public House(Texture2D texture, World gameWorld)
        {
            houseTexture = texture;
            world = gameWorld;
        }

        
        public void Draw()
        {
            world.DrawInWorld(houseTexture, X, Y, Width, Height, 0, 0);
        }
    }
}

