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
    public class Helicopter
    {
        const float ROTATION_RATE = 2;
        const float MAX_ANGLE = 45;
        const float MAX_THROTTLE = 0.2f;
        const float NORMAL_THROTTLE = 0.09f;
        const float MIN_THROTTLE = -0.1f;
        const float GRAVITY = -0.1f;

        public float Height = 40;
        public float Width = 100;
        public float X = 200;
        public float Y = 200;
        public float Rotation = 0;
        public float Throttle = 0;
        public float DeltaX;
        public float DeltaY;

        World world;
        Texture2D helicopterTexture;

        public Helicopter(Texture2D texture, World gameWorld)
        {
            helicopterTexture = texture;
            world = gameWorld;
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
                Rotation -= ROTATION_RATE;
            }
            else if (keyboardState.GetPressedKeys().Contains(Keys.Right))
            {
                Rotation += ROTATION_RATE;
            }
            float spaceAltitude = 250;
            float denseAltitude = 125;


            float airDensity = (Y - denseAltitude) / (-spaceAltitude - denseAltitude) + 1;
            airDensity = Math.Clamp(airDensity, 0, 1);

            DeltaX += Throttle * (float)Math.Cos((Rotation - 90) / 180 * Math.PI) *
                airDensity;
            DeltaY += Throttle * (float)Math.Sin((Rotation + 90) / 180 * Math.PI) *
                airDensity + GRAVITY;
            DeltaX *= 0.995f;
            DeltaY *= 0.995f;
            X += DeltaX;
            Y += DeltaY;

            if (X < World.BOUNDRY_WIND_WIDTH)
            {
                float windSpeed = (World.BOUNDRY_WIND_WIDTH - X) / World.BOUNDRY_WIND_WIDTH;
                DeltaX += windSpeed;
            }
            int eastWindStartX = World.TERRAIN_WIDTH - World.BOUNDRY_WIND_WIDTH;
            if (X > eastWindStartX)
            {
                float windSpeed = (X - eastWindStartX)/(World.TERRAIN_WIDTH - eastWindStartX);
                DeltaX -= windSpeed;
            }


            int groundLevel = 0;
            if (Y  < groundLevel + Height / 2)

            {
                Y = groundLevel + Height / 2;
                DeltaY = 0;
                Rotation = 0;
                DeltaX *= 0.9f;
            }
             
        }

        public void Draw()
        {
            world.DrawInWorld(helicopterTexture, X, Y, Width, Height,
                Rotation, 0);
        }
    }
}


