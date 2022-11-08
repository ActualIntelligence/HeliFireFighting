using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    internal class Tree
    {
        public int X;
        public int Y;
        public int Width = 64;
        public int Height = 120;

        World world;
        Texture2D treeTexture;
        public Tree(Texture2D texture, World gameWorld)
        {
            treeTexture = texture;
            world = gameWorld;
        }

        
        public void Draw()
        {
            world.DrawInWorld(treeTexture, X, Y, Width, Height, 0, 0);
        }
    }
}
