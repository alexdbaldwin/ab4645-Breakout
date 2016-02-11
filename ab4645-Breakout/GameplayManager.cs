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

        public enum GameState
        {
            Playing,
            GameOver
        }

        Color bgFrom, bgTo;
        float bgLerpTime = 0.0f;
        float bgLerpLength = 0.5f;

        GameState currentGameState = GameState.Playing;

        List<GameObject> gameObjects = new List<GameObject>();
        Player player1 = null, player2 = null;
        World world;
        Body border;
        //Body leftBorder,rightBorder,bottomBorder,topBorder;

        float sideMargin = 400.0f;
        float bottomMargin = 400.0f;
        readonly float screenWidth = 1920.0f;
        readonly float screenHeight = 1080.0f;
        readonly int tilesX = 20;
        readonly int tilesY = 20;
        
        float tileWidth = 80;
        float tileHeight = 40;

        public GameplayManager(string contentRoot)
        {
            //sideMargin = (screenWidth - tilesX * tileWidth) / 2.0f;
            //bottomMargin = screenHeight - tilesY * tileHeight;

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64.0f);
            world = new World(Vector2.Zero);
            MakeBorders();

            tileWidth = (screenWidth - 2 * sideMargin) / (float)tilesX;
            tileHeight = (screenHeight - bottomMargin) / (float)tilesY;

            LoadLevel(contentRoot + "/Levels/level1.txt");

            JoinGame(PlayerIndex.One);
            //JoinGame(PlayerIndex.Two);

            bgFrom = new Color(Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), 255);
            bgTo = new Color(Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), 255);

        }

        private void MakeBorders()
        {
            Vertices borders = new Vertices(4);
            borders.Add(new Vector2(ConvertUnits.ToSimUnits(sideMargin), 0));
            borders.Add(new Vector2(ConvertUnits.ToSimUnits(sideMargin), ConvertUnits.ToSimUnits(screenHeight + 200.0f)));
            borders.Add(new Vector2(ConvertUnits.ToSimUnits(screenWidth - sideMargin), ConvertUnits.ToSimUnits(screenHeight + 200.0f)));
            borders.Add(new Vector2(ConvertUnits.ToSimUnits(screenWidth - sideMargin), 0));

            border = BodyFactory.CreateLoopShape(world, borders);
            border.CollisionCategories = Category.Cat2;
            border.CollidesWith = Category.All;
            border.BodyType = BodyType.Static;
            border.Friction = 0.0f;
            border.Restitution = 1.0f;
        }

        private void JoinGame(PlayerIndex playerIndex) {
            switch (playerIndex)
            {
                case PlayerIndex.One:
                    if (player1 == null)
                    {
                        Paddle paddle1 = new Paddle(PlayerIndex.One, ConvertUnits.ToSimUnits(screenWidth / 2.0f, screenHeight - 100.0f), ConvertUnits.ToSimUnits(150.0f), ConvertUnits.ToSimUnits(21.0f), ConvertUnits.ToSimUnits(screenWidth / 2.0f - sideMargin - 75.0f), world);
                        gameObjects.Add(paddle1);
                        player1 = new Player(this, paddle1, PlayerIndex.One);
                        SpawnBall(player1);

                        //gameObjects.Add(new Ball(player1, ConvertUnits.ToSimUnits(new Vector2(screenWidth / 2.0f, screenHeight - 400.0f)), ConvertUnits.ToSimUnits(10.0f), world));
                    }
                    else {
                        throw new InvalidOperationException("Player One is already in the game.");
                    }
                    break;
                case PlayerIndex.Two:
                    if (player2 == null)
                    {
                        Paddle paddle2 = new Paddle(PlayerIndex.Two, ConvertUnits.ToSimUnits(screenWidth / 2.0f, screenHeight - 100.0f), ConvertUnits.ToSimUnits(150.0f), ConvertUnits.ToSimUnits(21.0f), ConvertUnits.ToSimUnits(screenWidth / 2.0f - sideMargin - 75.0f), world);
                        gameObjects.Add(paddle2);
                        player2 = new Player(this, paddle2, PlayerIndex.Two);
                        SpawnBall(player2);
                        //gameObjects.Add(new Ball(player2, ConvertUnits.ToSimUnits(new Vector2(screenWidth / 2.0f, screenHeight - 400.0f)), ConvertUnits.ToSimUnits(10.0f), world));
                    }
                    else {
                        throw new InvalidOperationException("Player Two is already in the game.");
                    }
                    break;
                default:
                    break;
            }
        }

        public void Update(GameTime gameTime) {
            switch (currentGameState)
            {
                case GameState.Playing:
                    UpdatePlaying(gameTime);
                    break;
                case GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
                default:
                    break;
            }


            bgLerpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (bgLerpTime > bgLerpLength) {
                bgFrom = bgTo;
                bgTo = new Color(Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), 255);
                bgLerpTime = 0.0f;
            }
            
            //Run physics
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void UpdatePlaying(GameTime gameTime)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
                if (gameObjects[i].Dead)
                {
                    if (gameObjects[i] is Ball) {
                        (gameObjects[i] as Ball).Owner.RemoveBall();
                    }
                    gameObjects[i].CleanUp();
                    gameObjects.RemoveAt(i--);
                }
            }

            if (!APlayerIsAlive())
                currentGameState = GameState.GameOver;
        }

        private void UpdateGameOver(GameTime gameTime) {
            //Don't do anything yet
        }

        private bool APlayerIsAlive() {
            return (player1 != null && player1.Alive) || (player2 != null && player2.Alive);
        }

        /// <summary>
        /// Spawns a ball and attaches it to player's paddle
        /// </summary>
        /// <param name="player"></param>
        public void SpawnBall(Player player) {
            player.AddBall();
            Ball b = new Ball(player, ConvertUnits.ToSimUnits(new Vector2(screenWidth / 2.0f, screenHeight - 400.0f)), ConvertUnits.ToSimUnits(10.0f), ConvertUnits.ToSimUnits(screenHeight), world);
            gameObjects.Add(b);
            player.Paddle.Attach(b);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(AssetManager.GetTexture("pixel"), new Rectangle(0, 0, (int)sideMargin, (int)screenHeight), new Color(12,12,71,255)/*Color.Lerp(bgFrom,bgTo,bgLerpTime/bgLerpLength)*/);
            spriteBatch.Draw(AssetManager.GetTexture("pixel"), new Rectangle((int)(screenWidth - sideMargin), 0, (int)sideMargin, (int)screenHeight), new Color(12, 12, 71, 255)/*Color.Lerp(bgFrom, bgTo, bgLerpTime / bgLerpLength)*/);
            foreach (GameObject go in gameObjects)
                go.Draw(spriteBatch);

            switch (currentGameState)
            {
                case GameState.Playing:
                    break;
                case GameState.GameOver:
                    break;
                default:
                    break;
            }
            
        }

        private void LoadLevel(string path) {
            using (StreamReader sr = new StreamReader(path)) {
                int j = 0;
                while (!sr.EndOfStream) {
                    string line = sr.ReadLine();
                    for (int i = 0; i < line.Length; i++) {
                        switch (line[i]) {
                            case '1':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f)*tileWidth, (j+0.5f)*tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Teal, 1, Block.BlockType.OneHit));
                                break;
                            case '2':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f) * tileWidth, (j + 0.5f) * tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Yellow, 2, Block.BlockType.TwoHits));
                                break;
                            case '3':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f) * tileWidth, (j + 0.5f) * tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Violet, 3, Block.BlockType.ThreeHits));
                                break;
                            case '4':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f) * tileWidth, (j + 0.5f) * tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Cyan, 4, Block.BlockType.FourHits));
                                break;
                            case '5':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f) * tileWidth, (j + 0.5f) * tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Orange, 5, Block.BlockType.FiveHits));
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
