using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ab4645_Breakout
{
    class GameplayManager
    {
        List<GameObject> gameObjects = new List<GameObject>();
        World world;
        Body leftBorder,rightBorder,bottomBorder;

        readonly float sideMargin = 400.0f;
        readonly float bottomMargin = 400.0f;
        readonly float screenWidth = 1920.0f;
        readonly float screenHeight = 1080.0f;
        readonly int tilesX = 20;
        readonly int tilesY = 20;
        
        float tileWidth;
        float tileHeight;

        public GameplayManager(string contentRoot) {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64.0f);
            world = new World(Vector2.Zero);

            //Vertices borders = new Vertices(4);
            //borders.Add(new Vector2(ConvertUnits.ToSimUnits(sideMargin), 0));
            //borders.Add(new Vector2(ConvertUnits.ToSimUnits(sideMargin), ConvertUnits.ToSimUnits(screenHeight)));
            //borders.Add(new Vector2(ConvertUnits.ToSimUnits(screenWidth - sideMargin), ConvertUnits.ToSimUnits(screenHeight)));
            //borders.Add(new Vector2(ConvertUnits.ToSimUnits(screenWidth - sideMargin), 0));

            //border = BodyFactory.CreateLoopShape(world, borders);
            //border.CollisionCategories = Category.All;
            //border.CollidesWith = Category.All;
            //border.BodyType = BodyType.Static;
            //border.Friction = 0.0f;
            //border.Restitution = 1.0f;
            //border.

            leftBorder = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(sideMargin), ConvertUnits.ToSimUnits(screenHeight),1, ConvertUnits.ToSimUnits(new Vector2(sideMargin/2.0f,screenHeight/2.0f)));
            leftBorder.CollisionCategories = Category.All;
            leftBorder.CollidesWith = Category.All;
            leftBorder.BodyType = BodyType.Static;
            leftBorder.Friction = 0.0f;
            leftBorder.Restitution = 1.0f;
            rightBorder = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(sideMargin), ConvertUnits.ToSimUnits(screenHeight), 100.0f, ConvertUnits.ToSimUnits(new Vector2(screenWidth - sideMargin/2.0f, screenHeight/2.0f)));
            rightBorder.CollisionCategories = Category.All;
            rightBorder.CollidesWith = Category.All;
            rightBorder.BodyType = BodyType.Static;
            rightBorder.Friction = 0.0f;
            rightBorder.Restitution = 1.0f;
            bottomBorder = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(screenWidth), ConvertUnits.ToSimUnits(10.0f), 100.0f, ConvertUnits.ToSimUnits(new Vector2(screenWidth/2.0f, screenHeight + 5.0f)));
            bottomBorder.CollisionCategories = Category.All;
            bottomBorder.CollidesWith = Category.All;
            bottomBorder.BodyType = BodyType.Static;
            bottomBorder.Friction = 0.0f;
            bottomBorder.Restitution = 1.0f;

            gameObjects.Add(new Ball(ConvertUnits.ToSimUnits(new Vector2(screenWidth/2.0f, screenHeight-400.0f)), ConvertUnits.ToSimUnits(10.0f), world));
            gameObjects.Add(new Paddle(ConvertUnits.ToSimUnits(screenWidth / 2.0f, screenHeight - 100.0f), ConvertUnits.ToSimUnits(400.0f), ConvertUnits.ToSimUnits(20.0f), world));

            tileWidth = (screenWidth - 2 * sideMargin) / (float)tilesX;
            tileHeight = (screenHeight - bottomMargin) / (float)tilesY;

            LoadLevel(contentRoot + "/Levels/level1.txt");
        }

        public void Update(GameTime gameTime) {
            foreach (GameObject go in gameObjects)
                go.Update(gameTime);

            //Run physics
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, 1.0f / 30.0f));
        }

        public void Draw(SpriteBatch spriteBatch) {
            
            spriteBatch.Draw(AssetManager.GetTexture("pixel"), new Rectangle(0, 0, (int)sideMargin, (int)screenHeight), Color.Black);
            spriteBatch.Draw(AssetManager.GetTexture("pixel"), new Rectangle((int)(screenWidth - sideMargin), 0, (int)sideMargin, (int)screenHeight), Color.Black);
            foreach (GameObject go in gameObjects)
                go.Draw(spriteBatch);
        }

        private void LoadLevel(string path) {
            using (StreamReader sr = new StreamReader(path)) {
                int j = 0;
                while (!sr.EndOfStream) {
                    string line = sr.ReadLine();
                    for (int i = 0; i < line.Length; i++) {
                        switch (line[i]) {
                            case 'b':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f)*tileWidth, (j+0.5f)*tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Teal));
                                break;
                            case '-':
                                break;
                            default:
                                break;
                        }
                    }
                    j++;
                }
            }
        }

    }
}
