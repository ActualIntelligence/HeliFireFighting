using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    internal class FireParticle : Particle
    {
        const float MAX_RISE_VELOCITY = -5;
        const float MIN_SIZE = 5;
        const float SHRINK_RATE = 0.5f;
        float StartSize = 0;

        Texture2D fireTexture;

        public FireParticle(Texture2D texture, float startingSize)
        {
            StartSize = startingSize;
            Size = startingSize;
            fireTexture = texture;
        }

        public override void Update()
        {
            DeltaX *= 0.95f;
            DeltaY += (MAX_RISE_VELOCITY - DeltaY) / 20;

            X += DeltaX;
            Y += DeltaY;
            if (Size > MIN_SIZE)
            {
                Size -= SHRINK_RATE;
                if (Size < MIN_SIZE)
                {
                    Size = MIN_SIZE;
                }
            }

        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, float cameraOffsetX, float cameraOffsetY)
        {
            float alpha = (Size - MIN_SIZE) / (StartSize - MIN_SIZE);
            Color fireColor = new Color(Color.OrangeRed, alpha);

            sb.Draw(fireTexture,
                new Rectangle((int)(X - Size / 2 - cameraOffsetX), (int)(Y - Size / 2 - cameraOffsetY), (int)Size, (int)Size),
                fireColor);
        }

        public override bool ShouldBeRemoved()
        {
            return Size <= MIN_SIZE;
        }
    }
}

