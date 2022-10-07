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

        Texture2D houseTexture;
        public House(Texture2D texture)
        {
            houseTexture = texture;
        }


        public void Draw(SpriteBatch sb, float cameraOffsetX, float cameraOffsetY)
        {
            Rectangle rect = new Rectangle((int)(X - Width / 2 - cameraOffsetX),
                (int)(Y - Height / 2 - cameraOffsetY), (int)Width, (int)Height);
            sb.Draw(houseTexture, rect, null, Color.White);
        }
    }
}

