using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    enum EnvironmentType
    {
        Sky,
        EmptyLand,
        Tree,
        House
    }
    internal class TerrainCell
    {
        public float X;
        public float Y;
        public EnvironmentType Type;

        /// <summary>
        /// The temperature of the cell is in Kelvin.
        /// </summary>
        public float Temperature;

        /// <summary>
        /// The Moisture Content of the cell. Range 0 to 100.
        /// </summary>
        public float MoistureContent;
    }


}
