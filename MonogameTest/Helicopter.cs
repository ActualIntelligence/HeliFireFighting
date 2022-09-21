using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    internal class Helicopter
    {
        const float MAX_ANGLE = 45;
        const float MAX_ANGLE_X_DIFF = 300;
        const float SPEED_DIVISOR = 42;
        public float Height = 40;
        public float Width = 100;
        public float X = 200;
        public float Y = 200;
        public float Angle = 30;
        public float TargetX = 600;
        public float TargetY = 300;
        public float DeltaX;
        public float DeltaY;

        Texture2D helicopterTexture;
         
        public Helicopter(Texture2D texture)
        {
            helicopterTexture = texture;
        }

        public void Update(float mouseX, float mouseY)
        {
            float diffX = mouseX - X;
            float diffY = mouseY - Y;
            DeltaX = diffX / SPEED_DIVISOR;
            DeltaY = diffY / SPEED_DIVISOR;
            X += DeltaX;
            Y += DeltaY;
            if (Math.Abs(diffX) > MAX_ANGLE_X_DIFF)
            {
                Angle = MAX_ANGLE;
                if (X > mouseX)
                {
                    Angle *= -1;
                }
            }
            else
            {
                Angle = diffX / MAX_ANGLE_X_DIFF * MAX_ANGLE;
            }

        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(X), (int)(Y), (int)Width, (int)Height);
            sb.Draw(helicopterTexture,rect,null,Color.White,
               (float)(Angle*Math.PI/180),new Vector2(helicopterTexture.Width/2, helicopterTexture.Height /2),SpriteEffects.None,0);
        }
    }
}


