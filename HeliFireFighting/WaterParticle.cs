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
        const float TERMINAL_VELOCITY = 5;
        const float MAX_SIZE = 35;
        const float DISPERSAL_RATE = 0.5f;

        Texture2D waterTexture;
        public WaterParticle(Texture2D texture)
        {
            waterTexture = texture;
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


        public override void Draw(SpriteBatch sb, float cameraOffsetX, float cameraOffsetY)
        {
            float alpha = (1 - Size / MAX_SIZE);
            Color waterColor = Color.White*alpha;
            
            sb.Draw(waterTexture, 
                new Rectangle((int)(X - Size / 2 - cameraOffsetX), (int)(Y - Size / 2 - cameraOffsetY),
                (int)Size, (int)Size), 
                waterColor);

        }

        public override bool ShouldBeRemoved()
        {
            return Size >= MAX_SIZE;
        }

    }

}

