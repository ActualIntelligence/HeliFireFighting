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

        World world;
        Texture2D fireTexture;

        public FireParticle(Texture2D texture, float startingSize, World gameWorld)
        {
            StartSize = startingSize;
            Size = startingSize;
            fireTexture = texture;
            world = gameWorld;
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


        public override void Draw()
        {
            float alpha = (Size - MIN_SIZE) / (StartSize - MIN_SIZE);
            Color fireColor = new Color(Color.OrangeRed, alpha);

            world.DrawInWorld(fireTexture, X, Y, Size, Size, 0, 0);

        }

        public override bool ShouldBeRemoved()
        {
            return Size <= MIN_SIZE;
        }
    }
}

