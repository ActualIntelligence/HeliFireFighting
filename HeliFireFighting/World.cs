using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeliFireFighting
{
    internal class World
    {
        const int CLOUD_COUNT = 42;
        const int TREE_COUNT = 100;
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

        float cameraOffsetX = 0;
        float cameraOffsetY = 0;

        int sunX = 200;
        int sunY = 50;
        int sunRadius = 21;

        int ScreenWidth = 0;
        int ScreenHeight = 0;

        #region classes
        Helicopter playerHeli;

        GraphicsDevice graphicsdevice;

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

        public World(GraphicsDevice gd, int screenWidth, int screenHeight)
        {
            graphicsdevice = gd;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;

            LoadContent();

            playerHeli = new Helicopter(helicopterTexture);
            terrain = new Terrain(grassTexture);
            terrain.Generate(gd, 5, ScreenWidth, ScreenHeight);

            // makes clouds
            for (int i = 0; i < CLOUD_COUNT; i++)
            {
                Cloud cloud = new Cloud(cloudTexture);
                cloud.X = random.Next(0, ScreenWidth);
                cloud.Y = random.Next(0, 200);
                cloud.Width = random.Next(50, 120);
                cloud.IsForeGound = random.NextDouble() > 0.5;
                clouds[i] = cloud;
            }

            // build houses
            for (int i = 0; i < HOUSE_COUNT; i++)
            {
                House foundation = new House(houseTexture);
                foundation.Width = random.Next(MIN_HOUSE_WIDTH, MAX_HOUSE_WIDTH);
                foundation.Height = random.Next(MIN_HOUSE_HEIGHT, MAX_HOUSE_HEIGHT);

                bool isSafeToBuild = false;
                do
                {
                    foundation.X = random.Next(ScreenWidth);
                    foundation.Y = terrain.HeightOfTerrainAtX(foundation.X) + random.Next(5, 250);

                    int leftYDiff = foundation.Y - terrain.HeightOfTerrainAtX(foundation.X - foundation.Width);
                    int rightYDiff = foundation.Y - terrain.HeightOfTerrainAtX(foundation.X + foundation.Width);
                    int minYDiff = Math.Min(leftYDiff, rightYDiff);
                    if (minYDiff < 0)
                    {
                        foundation.Y -= minYDiff;
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
                Tree sapling = new Tree(tree1Texture);

                bool isInTheClear = false;
                while (isInTheClear == false)
                {
                    sapling.X = random.Next(ScreenWidth);
                    sapling.Y = terrain.HeightOfTerrainAtX(sapling.X) + random.Next(5, 250);
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
            helicopterTexture = Texture2D.FromFile(graphicsdevice, "Art/Helicopter.png");
            cloudTexture = Texture2D.FromFile(graphicsdevice, "Art/Cloud.png");
            fireTexture = Texture2D.FromFile(graphicsdevice, "Art/Fire.png");
            waterTexture = Texture2D.FromFile(graphicsdevice, "Art/Water.png");
            tree1Texture = Texture2D.FromFile(graphicsdevice, "Art/Tree1.png");
            houseTexture = Texture2D.FromFile(graphicsdevice, "Art/LogCabin.png");
            grassTexture = Texture2D.FromFile(graphicsdevice, "Art/RockGrassyTexture.jpg");
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

            cameraOffsetX = playerHeli.X - ScreenWidth/2;
            cameraOffsetY = playerHeli.Y - ScreenHeight/2;

            //Adds water.
            if (keyboardState.GetPressedKeys().Contains(Keys.Space))
            {
                WaterParticle particle = new WaterParticle(waterTexture);
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

                if (puffy.X > ScreenWidth + puffy.Width / 2)
                {
                    puffy.X = -puffy.Width / 2;
                }
                else if (puffy.X < -puffy.Width / 2)
                {
                    puffy.X = ScreenWidth + puffy.Width / 2;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Cloud cloud in clouds)
            {
                if (!cloud.IsForeGound)
                {
                    cloud.Draw(sb,cameraOffsetX,cameraOffsetY);
                }
            }

            terrain.Draw(sb, cameraOffsetX, cameraOffsetY);

            foreach (House house in houses)
            {
                house.Draw(sb, cameraOffsetX, cameraOffsetY);
            }

            foreach (Tree tree in trees)
            {
                tree.Draw(sb, cameraOffsetX, cameraOffsetY);
            }

            playerHeli.Draw(sb, cameraOffsetX, cameraOffsetY);
            foreach (Particle particle in particles)
            {
                particle.Draw(sb, cameraOffsetX, cameraOffsetY);
            }

            foreach (Cloud cloud in clouds)
            {
                if (cloud.IsForeGound)
                {
                    cloud.Draw(sb, cameraOffsetX, cameraOffsetY);
                }
            }
        }
    }
}

