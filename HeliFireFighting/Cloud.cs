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
        public Cloud(Texture2D texture)
        {
            cloudTexture = texture;
        }


        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(X - Width / 2), (int)(Y - Height / 2), (int)Width, (int)Height);
            sb.Draw(cloudTexture, rect, null, IsForeGound ? Color.White : Color.LightGray);
        }
    }
}

