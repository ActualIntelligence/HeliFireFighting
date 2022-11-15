using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    internal class WaterParticle : Particle
    {
        const float TERMINAL_VELOCITY = -5;
        const float MAX_SIZE = 35;
        const float DISPERSAL_RATE = 0.5f;

        World world;
        Texture2D waterTexture;
        public WaterParticle(Texture2D texture, World gameWorld)
        {
            waterTexture = texture;
            world = gameWorld;
        }

        public override void Update()
        {
            DeltaX *= 0.95f;
            DeltaY += (TERMINAL_VELOCITY - DeltaY) / 20;

            X += DeltaX;
            Y += DeltaY;
            if (Size < MAX_SIZE)
            {
                Size += DISPERSAL_RATE;
                if (Size > MAX_SIZE)
                {
                    Size = MAX_SIZE;
                }
            }

        }


        public override void Draw()
        {
            float alpha = (1 - Size / MAX_SIZE);
            Color waterColor = Color.White*alpha;

            world.DrawInWorld(waterTexture, X, Y, Size, Size, 0, 0);

        }

        public override bool ShouldBeRemoved()
        {
            return Size >= MAX_SIZE;
        }

    }

}

