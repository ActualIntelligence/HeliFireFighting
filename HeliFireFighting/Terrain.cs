﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliFireFighting
{
    internal class Terrain
    {
        Point[] terrainPoints = new Point[2];
        World world;
        Texture2D grassTexture;
        Texture2D terrainTex;
        public Terrain(Texture2D texture,World gameWorld)
        {
            grassTexture = texture;
            world = gameWorld;
        }

        public void Generate( GraphicsDevice graphics, int subdivideCount,
            int terrainWidth, int terrainHeight)
        {
            terrainPoints[0] = new Point(0, terrainHeight / 2);
            terrainPoints[1] = new Point(terrainWidth, terrainHeight / 2
                );

            // create terrain
            int verticalDelta = terrainHeight / 2;
            for (int i = 0; i < subdivideCount; i++)
            {
                terrainPoints = SubdividePoints(terrainPoints,verticalDelta);
                verticalDelta /= 2;
            }
            Color[] terrainColors = new Color[terrainWidth*terrainHeight];
            terrainTex = new Texture2D(graphics, terrainWidth, terrainHeight);
            terrainTex.GetData(terrainColors);

            Color[] grassColors = new Color[grassTexture.Width * grassTexture.Height];
            grassTexture.GetData(grassColors);

            for(int x = 0; x<terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    int index = y * terrainWidth + x;
                    if (y > HeightOfTerrainAtX(x))
                    {
                        int grassX = x % grassTexture.Width;
                        int grassY = y % grassTexture.Height;
                        terrainColors[index] = grassColors[grassY*grassTexture.Width+grassX];
                    }
                    else
                    {
                        terrainColors[index] = Color.Transparent;
                    }
                } 
            }
            terrainTex.SetData(terrainColors);

        }



        /// <summary>
        /// Subdivides an array of points while adding random Y variations
        /// to the interpolated points.
        /// </summary>
        /// <param name="inputPoints">The array of points to subdivide.</param>
        /// <returns>The array of subdivided points.</returns>
        Point[] SubdividePoints(Point[] inputPoints, int maxVerticalDelta)
        {
            Random random = new Random();
            Point[] outputPoints = new Point[inputPoints.Length * 2 - 1];

            // loop through all points in the output array
            for (int i = 0; i < inputPoints.Length * 2 - 1; i++)
            {
                // if index is even, copy the corresponding point from the input array to the output array
                if (i % 2 == 0)
                {
                    outputPoints[i] = inputPoints[i / 2];
                }
                // otherwise, create a new point in the middle of the its neighbor points
                else
                {
                    Point firstPoint = inputPoints[i / 2];
                    Point secondPoint = inputPoints[i / 2 + 1];
                    int xDistance = secondPoint.X - firstPoint.X;
                    int x = (firstPoint.X + secondPoint.X) / 2;
                    int y = (firstPoint.Y + secondPoint.Y) / 2 +
                        random.Next(-maxVerticalDelta, maxVerticalDelta+1);
                    Point midPoint = new Point(x, y);
                    outputPoints[i] = midPoint;
                }
            }
            return outputPoints;
        }


        public void Draw()
        {
            if(terrainTex != null)
            {
                world.DrawInWorld(terrainTex,terrainTex.Width/2,terrainTex.Height/2,terrainTex.Width,
                    terrainTex.Height,0);
            }
                
        }

        public int HeightOfTerrainAtX(int x)
        {
            float xSpacing = (float)terrainPoints[^1].X / (terrainPoints.Length - 1);
            int leftIndex = (int)(x / xSpacing);
            if (leftIndex >= terrainPoints.Length)
            {
                leftIndex = terrainPoints.Length - 1;
            }
            else if (leftIndex < 0)
            {
                leftIndex = 0;
            }
            int rightIndex = leftIndex >= terrainPoints.Length - 1 ? leftIndex : leftIndex + 1;
            int leftX = terrainPoints[leftIndex].X;
            int rightX = terrainPoints[rightIndex].X;
            int leftY = terrainPoints[leftIndex].Y;
            int rightY = terrainPoints[rightIndex].Y;
            double frac = 0;
            if (rightX != leftX)
            {
                frac = (double)(x - leftX) / (rightX - leftX);
            }

            int height = (int)(leftY * (1 - frac) + rightY * frac);
            return height;

        }

        public int NearestTerrainIndexAtX(int x)
        {
            int terrainWidth = terrainPoints[^1].X - terrainPoints[0].X;

            float xSpacing = (float)terrainWidth / (terrainPoints.Length - 1);
            int index = Convert.ToInt32(x / xSpacing);
            if (index < 0)
            {
                index = 0;
            }
            if (index >= terrainPoints.Length)
            {
                index = terrainPoints.Length - 1;
            }
            return index;
        }

        public void FlattenTerrainAtX(int x, int y, int width)
        {
            int leftSideX = x - width / 2;
            int rightSideX = x + width / 2;
            int leftSideIndex = NearestTerrainIndexAtX(leftSideX);
            int rightSideIndex = NearestTerrainIndexAtX(rightSideX);

            for (int i = leftSideIndex; i <= rightSideIndex; i++)
            {
                terrainPoints[i].Y = y;
            }
        }
    }
}
