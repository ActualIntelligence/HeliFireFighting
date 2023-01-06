using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeliFireFighting
{
    public class World
    {
        const int CLOUD_COUNT = 42;
        const int TREE_COUNT = 500;
        const int MIN_TREE_WIDTH = 20;
        const int MAX_TREE_WIDTH = 50;
        const int MIN_TREE_HEIGHT = 30;
        const int MAX_TREE_HEIGHT = 100;
        const int HOUSE_COUNT = 10;
        const int MIN_HOUSE_WIDTH = 50;
        const int MAX_HOUSE_WIDTH = 90;
        const int MIN_HOUSE_HEIGHT = 30;
        const int MAX_HOUSE_HEIGHT = 60;
        const int CLEARING_MULTIPLIER = 2;
        public const int TERRAIN_WIDTH = 2400;
        public const int TERRAIN_HEIGHT = 500;
        public const int BOUNDRY_WIND_WIDTH = 100;
        public const int TERRAIN_CELL_SIZE = 20;

        public float cameraOffsetX = 0;
        public float cameraOffsetY = 0;

        int sunX = 200;
        int sunY = 50;
        int sunRadius = 21;

        int ScreenWidth = 0;
        int ScreenHeight = 0;


        #region classes
        public Helicopter playerHeli;

        public GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;

        Texture2D helicopterTexture;// = Texture2D.FromFile("Art/helicopter.png");
        Texture2D cloudTexture;
        Texture2D fireTexture;
        Texture2D waterTexture;
        Texture2D tree1Texture;
        Texture2D houseTexture;
        Texture2D grassTexture;
        // the points that define the shape of the terrain
        Terrain terrain;

        Cloud[] clouds = new Cloud[CLOUD_COUNT];

        House[] houses = new House[HOUSE_COUNT];

        Tree[] trees = new Tree[TREE_COUNT];
        List<Particle> particles = new List<Particle>();

        Random random = new Random();
        #endregion

        public World(GraphicsDevice gd, SpriteBatch sb, int screenWidth, int screenHeight)
        {
            graphicsDevice = gd;
            spriteBatch = sb;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;

            LoadContent();

            playerHeli = new Helicopter(helicopterTexture, this);
            terrain = new Terrain(grassTexture, this);
            terrain.Generate(gd, 5, TERRAIN_WIDTH, TERRAIN_HEIGHT,TERRAIN_CELL_SIZE);

            // makes clouds
            for (int i = 0; i < CLOUD_COUNT; i++)
            {
                Cloud cloud = new Cloud(cloudTexture, this);
                cloud.X = random.Next(0, TERRAIN_WIDTH);
                cloud.Y = random.Next(0, 200);
                cloud.Width = random.Next(50, 120);
                cloud.IsForeGound = random.NextDouble() > 0.5;
                clouds[i] = cloud;
            }

            // build houses
            for (int i = 0; i < HOUSE_COUNT; i++)
            {
                House foundation = new House(houseTexture, this);
                foundation.Width = random.Next(MIN_HOUSE_WIDTH, MAX_HOUSE_WIDTH);
                foundation.Height = random.Next(MIN_HOUSE_HEIGHT, MAX_HOUSE_HEIGHT);

                bool isSafeToBuild = false;
                do
                {
                    foundation.X = random.Next(TERRAIN_WIDTH);
                    foundation.Y = terrain.HeightOfTerrainAtX(foundation.X) - random.Next(5, 125);

                    int leftYDiff = foundation.Y - terrain.HeightOfTerrainAtX(foundation.X - foundation.Width);
                    int rightYDiff = foundation.Y - terrain.HeightOfTerrainAtX(foundation.X + foundation.Width);
                    int minYDiff = Math.Min(leftYDiff, rightYDiff);
                    if (minYDiff < 0)
                    {
                        foundation.Y += minYDiff;
                    }

                    isSafeToBuild = true;
                    for (int j = 0; j < i; j++)
                    {
                        House otherHouse = houses[j];
                        double distance = Math.Sqrt(Math.Pow(foundation.X - otherHouse.X, 2) + Math.Pow(foundation.Y - otherHouse.Y, 2));
                        if (distance < (foundation.Width + otherHouse.Width) / 2)
                        {
                            isSafeToBuild = false;
                            break;
                        }
                    }
                } while (!isSafeToBuild);

                houses[i] = foundation;
            }

            // plant trees
            for (int i = 0; i < trees.Length; i++)
            {
                Tree sapling = new Tree(tree1Texture,this);

                bool isInTheClear = false;
                while (isInTheClear == false)
                {
                    sapling.X = random.Next(TERRAIN_WIDTH);
                    sapling.Y = terrain.HeightOfTerrainAtX(sapling.X) - random.Next(5, 250);
                    isInTheClear = true;
                    foreach (House house in houses)
                    {
                        int maxDimension = Math.Max(house.Width, house.Height);
                        int clearingRadius = maxDimension * CLEARING_MULTIPLIER / 2;
                        int xOffset = house.X - sapling.X;
                        int yOffset = house.Y - sapling.Y;
                        int distanceFromHouse = (int)Math.Sqrt(xOffset * xOffset + yOffset * yOffset);
                        if (distanceFromHouse < clearingRadius)
                        {
                            isInTheClear = false;
                            break;
                        }
                    }
                }
                sapling.Width = random.Next(MIN_TREE_WIDTH, MAX_TREE_WIDTH);
                sapling.Height = random.Next(MIN_TREE_HEIGHT, MAX_TREE_HEIGHT);
                trees[i] = sapling;
            }

            Array.Sort(trees, (a, b) => a.Y.CompareTo(b.Y));

        }
        
        public void LoadContent()
        {
            helicopterTexture = Texture2D.FromFile(graphicsDevice, "Art/Helicopter.png");
            cloudTexture = Texture2D.FromFile(graphicsDevice, "Art/Cloud.png");
            fireTexture = Texture2D.FromFile(graphicsDevice, "Art/Fire.png");
            waterTexture = Texture2D.FromFile(graphicsDevice, "Art/Water.png");
            tree1Texture = Texture2D.FromFile(graphicsDevice, "Art/Tree1.png");
            houseTexture = Texture2D.FromFile(graphicsDevice, "Art/LogCabin.png");
            grassTexture = Texture2D.FromFile(graphicsDevice, "Art/RockGrassyTexture.jpg");
        }

        /// <summary>
        /// Update the world and everything in it.
        /// </summary>
        /// <param name="mouseX">Mouse X position</param>
        /// <param name="mouseY">Mouse Y position</param>
        /// <param name="isLeftButtonDown">Left mouse button pressed state.</param>
        public void Update(MouseState mouseState,KeyboardState keyboardState)
        {
            playerHeli.Update(keyboardState);

            cameraOffsetX = (int)(playerHeli.X - ScreenWidth / 2 + playerHeli.Width / 2);
            cameraOffsetY = (int)(-playerHeli.Y - ScreenHeight / 2 + playerHeli.Height / 2);

            if (cameraOffsetX <= 0)
            {
                cameraOffsetX = 0;
            }
            if (cameraOffsetX >= TERRAIN_WIDTH - ScreenWidth)
            {
                cameraOffsetX = TERRAIN_WIDTH - ScreenWidth;
            }
            if (cameraOffsetY >= -ScreenHeight)
            {
                cameraOffsetY = -ScreenHeight;
            }

            //Adds water.
            if (keyboardState.GetPressedKeys().Contains(Keys.Space))
            {
                WaterParticle particle = new WaterParticle(waterTexture, this);
                particle.X = playerHeli.X;
                particle.Y = playerHeli.Y;
                particle.DeltaX = playerHeli.DeltaX;
                particle.DeltaY = playerHeli.DeltaY;
                particle.Size = random.Next(2, 6);
                particles.Add(particle);

            }
            //Adds fire.
            /*FireParticle fireParticle = new(fireTexture, random.Next(20, 30));
            fireParticle.X = playerHeli.X;
            fireParticle.Y = playerHeli.Y;
            fireParticle.DeltaX = playerHeli.DeltaX;
            fireParticle.DeltaY = playerHeli.DeltaY;
            particles.Add(fireParticle);*/

            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update();
                if (particles[i].Y > ScreenHeight || particles[i].ShouldBeRemoved())
                {
                    particles.RemoveAt(i);
                }
            }



            for (int i = 0; i < CLOUD_COUNT; i++)
            {
                Cloud puffy = clouds[i];

                if (puffy.IsForeGound)
                {
                    puffy.X += 1;
                }
                else
                {
                    puffy.X += 0.4;
                }

                if (puffy.X > TERRAIN_WIDTH + puffy.Width / 2)
                {
                    puffy.X = -puffy.Width / 2;
                }
                else if (puffy.X < -puffy.Width / 2)
                {
                    puffy.X = TERRAIN_WIDTH + puffy.Width / 2;
                }
            }
        }

        public void DrawWorld(SpriteBatch sb)
        {
            foreach (Cloud cloud in clouds)
            {
                if (!cloud.IsForeGound)
                {
                    cloud.Draw();
                }
            }

            terrain.Draw();

            foreach (House house in houses)
            {
                house.Draw();
            }

            foreach (Tree tree in trees)
            {
                tree.Draw();
            }

            playerHeli.Draw();
            foreach (Particle particle in particles)
            {
                particle.Draw();
            }

            foreach (Cloud cloud in clouds)
            {
                if (cloud.IsForeGound)
                {
                    cloud.Draw();
                }
            }
        }

        public void DrawInWorld(Texture2D texture,
            float worldX, float worldY,
            float width, float height, float rotationDegrees, float layerDepth = 0)
        {
            Vector2 scale = new Vector2(width / texture.Width, height / texture.Height);
            spriteBatch.Draw(texture,
                new Vector2(worldX - cameraOffsetX,
                -(worldY + cameraOffsetY)),
                null, Color.White, rotationDegrees*MathF.PI/180,
               new Vector2(texture.Width/2, texture.Height /2), scale, SpriteEffects.None, layerDepth);
        }
    }
}

