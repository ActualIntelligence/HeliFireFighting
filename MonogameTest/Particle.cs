using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    internal abstract class Particle
    {
        public float X;
        public float Y;
        public float DeltaX;
        public float DeltaY;
        public float Size;

        public abstract void Update();
        public abstract void Draw(SpriteBatch sb);
        public abstract bool ShouldBeRemoved();
    }
}

