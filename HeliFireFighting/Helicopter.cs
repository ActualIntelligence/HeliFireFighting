using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    internal class Helicopter
    {
        const float ROTATION_RATE = 2;
        const float MAX_ANGLE = 45;
        const float MAX_THROTTLE = 0.2f;
        const float NORMAL_THROTTLE = 0.09f;
        const float MIN_THROTTLE = 0;
        const float GRAVITY = -0.1f;

        public float Height = 40;
        public float Width = 100;
        public float X = 200;
        public float Y = 200;
        public float Angle = 0;
        public float Throttle = 0;
        public float DeltaX;
        public float DeltaY;


        Texture2D helicopterTexture;
         
        public Helicopter(Texture2D texture)
        {
            helicopterTexture = texture;
        }

        public void Update(KeyboardState keyboardState)
        {
            Throttle = NORMAL_THROTTLE;
            if (keyboardState.GetPressedKeys().Contains(Keys.Up))
            {
                Throttle = MAX_THROTTLE;
            }
            else if (keyboardState.GetPressedKeys().Contains(Keys.Down))
                {
                Throttle = MIN_THROTTLE;
                }

            if (keyboardState.GetPressedKeys().Contains(Keys.Left))
            {
                Angle -= ROTATION_RATE;
            }
            else if (keyboardState.GetPressedKeys().Contains(Keys.Right))
            {
                Angle += ROTATION_RATE;
            }
            float spaceAltitude = -250;
            float denseAltitude = 125;


            float airDensity = (Y - denseAltitude) / (-spaceAltitude - denseAltitude) + 1;
            airDensity = Math.Clamp(airDensity, 0, 1);

            DeltaX += Throttle*(float)Math.Cos((Angle - 90) / 180 * Math.PI)*
                airDensity;
            DeltaY += Throttle * (float)Math.Sin((Angle - 90) / 180 * Math.PI)*
                airDensity- GRAVITY;
            DeltaX *= 0.995f;
            DeltaY *= 0.995f;
            X += DeltaX;
            Y += DeltaY;

            if(Y > 460)
            {
                Y = 460;
                DeltaY = 0;
                Angle = 0;
                DeltaX *= 0.9f;
            }

        }

        public void Draw(SpriteBatch sb, float cameraOffsetX, float cameraOffsetY)
        {
            Rectangle rect = new Rectangle((int)(X - cameraOffsetX), (int)(Y - cameraOffsetY), (int)Width, (int)Height);
            sb.Draw(helicopterTexture,rect,null,Color.White,
               (float)(Angle*Math.PI/180),new Vector2(helicopterTexture.Width/2, helicopterTexture.Height /2),SpriteEffects.None,0);
        }
    }
}


