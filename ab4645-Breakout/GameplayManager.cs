using ab4645_Breakout.PowerUps;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        SpriteFont mainFont;
        int blocksRemaining = 0;
        float powerUpSpawnChance = 0.08f;

        bool twoPlayers = false;
        float player2WaitTime = 0.0f;

        string[] levels = new string[] { "Content/Levels/level1.txt", "Content/Levels/level4.txt", "Content/Levels/level3.txt", "Content/Levels/spiral.txt", "Content/Levels/rainbow.txt", "Content/Levels/fortress.txt" };
        int levelIndex = 0;

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

            //bgFrom = new Color(Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), 255);
            //bgTo = new Color(Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), Game1.rand.Next(0, 256), 255);

            mainFont = AssetManager.GetFont("main");

            RestartGame(PlayerIndex.One);
            //JoinGame(PlayerIndex.Two);

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

        public void ArmPlayer(Player p){
            p.Paddle.ActivateGun(15);
        }

        private void JoinGame(PlayerIndex playerIndex) {
            switch (playerIndex)
            {
                case PlayerIndex.One:
                    if (player1 == null)
                    {
                        Paddle paddle1 = new Paddle(PlayerIndex.One, this, ConvertUnits.ToSimUnits(screenWidth / 2.0f, screenHeight - 100.0f), ConvertUnits.ToSimUnits(150.0f), ConvertUnits.ToSimUnits(21.0f), ConvertUnits.ToSimUnits(screenWidth / 2.0f - sideMargin), world);
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
                        Paddle paddle2 = new Paddle(PlayerIndex.Two, this, ConvertUnits.ToSimUnits(screenWidth / 2.0f, screenHeight - 100.0f), ConvertUnits.ToSimUnits(150.0f), ConvertUnits.ToSimUnits(21.0f), ConvertUnits.ToSimUnits(screenWidth / 2.0f - sideMargin), world);
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
            //Run physics
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

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
            
            
        }

        public void SpawnBullet(Vector2 position) {
            gameObjects.Add(new Bullet(position, world));
        }

        private void SpawnRandomPowerUp(Vector2 position) {

            List<Type> types = new List<Type> {
                typeof(BallSizeUp),
                typeof(BallSpeedUp),
                typeof(ExtraBall),
                typeof(ExtraLife),
                typeof(PaddleSizeDown),
                typeof(PaddleSizeUp),
                typeof(PaddleSpeedDown),
                typeof(PaddleSpeedUp),
                typeof(PowerUps.SplitBalls),
                typeof(TemporaryGun)
            };
            

            List<double> weights = new List<double> { 
                /*BallSizeUp.Weight*/ 6.0,
                /*BallSpeedUp.Weight*/ 2.0,
                /*ExtraBall.Weight*/ 3.0,
                /*ExtraLife.Weight*/ 2.0,
                /*PaddleSizeDown.Weight*/3.0,
                /*PaddleSizeUp.Weight*/3.0,
                /*PaddleSpeedDown.Weight*/1.0,
                /*PaddleSpeedUp.Weight*/1.0,
                /*PowerUps.SplitBalls.Weight*/ 6.0,
                /*TemporaryGun*/ 4.0f
            };

            double powerUp = Game1.rand.NextDouble()* weights.Sum();

            double runningTotal = 0.0;
            for(int i = 0; i < weights.Count; i++)
            {
                runningTotal += weights[i];
                if(powerUp <= runningTotal)
                {
                    ConstructorInfo ctor = types[i].GetConstructor(new[] { typeof(GameplayManager), typeof(World), typeof(Vector2) });
                    gameObjects.Add((GameObject)ctor.Invoke(new object[] { this, world, position }));
                    break;
                }
            }


            //PowerUp.PowerUpType type = (PowerUp.PowerUpType)Game1.rand.Next(PowerUp.NumberOfPowerUps);
            //switch (type)
            //{
            //    case PowerUp.PowerUpType.PaddleSizeUp:
            //        gameObjects.Add(new PaddleSizeUp(this, world, position));
            //        break;
            //    case PowerUp.PowerUpType.PaddleSizeDown:
            //        gameObjects.Add(new PaddleSizeDown(this, world, position));
            //        break;
            //    case PowerUp.PowerUpType.BallSizeUp:
            //        gameObjects.Add(new BallSizeUp(this, world, position));
            //        break;
            //    case PowerUp.PowerUpType.BallSizeDown:
            //        gameObjects.Add(new BallSizeDown(this, world, position));
            //        break;
            //    case PowerUp.PowerUpType.SplitBalls:
            //        gameObjects.Add(new SplitBalls(this, world, position));
            //        break;
            //    case PowerUp.PowerUpType.BallSpeedUp:
            //        gameObjects.Add(new BallSpeedUp(this, world, position));
            //        break;
            //    case PowerUp.PowerUpType.BallSpeedDown:
            //        gameObjects.Add(new BallSpeedDown(this, world, position));
            //        break;
            //    case PowerUp.PowerUpType.PaddleSpeedUp:
            //        gameObjects.Add(new PaddleSpeedUp(this, world, position));
            //        break;
            //    case PowerUp.PowerUpType.PaddleSpeedDown:
            //        gameObjects.Add(new PaddleSpeedDown(this, world, position));
            //        break;
            //    //case PowerUp.PowerUpType.StickyPaddle:
            //    //    gameObjects.Add(new StickyPaddle(this, world, position));
            //    //    break;
            //    case PowerUp.PowerUpType.ExtraBall:
            //        gameObjects.Add(new ExtraBall(this, world, position));
            //        break;
            //    case PowerUp.PowerUpType.ExtraLife:
            //        gameObjects.Add(new ExtraLife(this, world, position));
            //        break;
            //    default:
            //        break;
            //}
        }

        private void UpdatePlaying(GameTime gameTime)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
                if (gameObjects[i].Dead)
                {
                    if (gameObjects[i] is Ball)
                    {
                        (gameObjects[i] as Ball).Owner.RemoveBall();
                    }
                    if (gameObjects[i] is Block) {
                        blocksRemaining--;
                        if (Game1.rand.NextDouble() < powerUpSpawnChance)
                            SpawnRandomPowerUp(gameObjects[i].Position);
                    }
                    gameObjects[i].CleanUp();
                    gameObjects.RemoveAt(i--);
                }
            }

            if (blocksRemaining <= 0 && levelIndex < levels.Length - 1) {
                NextLevel();
            }

            if (player1 != null && !player1.Alive)
            {
                player1.Paddle.CleanUp();
                gameObjects.Remove(player1.Paddle);
                player1 = null;
                player2WaitTime = 20.0f;
            }
            if (player2 != null && !player2.Alive)
            {
                player2.Paddle.CleanUp();
                gameObjects.Remove(player2.Paddle);
                player2 = null;
                player2WaitTime = 20.0f;
            }

            if (!APlayerIsAlive())
            {
                GameOver();
            }

            DropInPlayer2(gameTime);

        }

        private void DropInPlayer2(GameTime gameTime)
        {
            if (twoPlayers && player2WaitTime > 0) {
                player2WaitTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Start) && player1 == null)
            {
                twoPlayers = true;
                JoinGame(PlayerIndex.One);
            }
            else if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Start) && player2 == null)
            {
                twoPlayers = true;
                JoinGame(PlayerIndex.Two);
            }
        }

        private void GameOver() {
            AudioManager.PlayDeath();
            currentGameState = GameState.GameOver;
        }

        private void NextLevel(){
            levelIndex++;
            levelIndex = levelIndex % levels.Length;
            
            for (int i = 0; i < gameObjects.Count; i++){
                if (gameObjects[i] is Ball) {
                    (gameObjects[i] as Ball).Owner.Paddle.Attach(gameObjects[i] as Ball);
                }
                else if (gameObjects[i] is Block || gameObjects[i] is PowerUp) {
                    gameObjects[i].CleanUp();
                    gameObjects.RemoveAt(i--);
                }
                else if (gameObjects[i] is Paddle)
                {
                    Vector2 v = gameObjects[i].Position;
                    v.X = ConvertUnits.ToSimUnits(screenWidth / 2);
                    gameObjects[i].Position = v;
                }

            }

            LoadLevel(levels[levelIndex]);
            currentGameState = GameState.Playing;
        }

        private void UpdateGameOver(GameTime gameTime) {
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Start)) {
                RestartGame(PlayerIndex.One);
            } else if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Start))
            {
                RestartGame(PlayerIndex.Two);
            }
        }

        private void RestartGame(PlayerIndex player) {
            twoPlayers = false;
            levelIndex = 0;
            player1 = null;
            player2 = null;
            ClearGameObjects();
            LoadLevel(levels[levelIndex]);
            JoinGame(player);
            currentGameState = GameState.Playing;
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

        public void SplitBalls(Player player)
        {
            for(int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i] is Ball && (gameObjects[i] as Ball).Owner == player) {
                    

                    float angle = (float)Game1.rand.NextDouble() * MathHelper.TwoPi;
                    Vector2 heading = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    (gameObjects[i] as Ball).Launch(heading);

                    for(int j = 0; j < Game1.rand.Next(2,5); j++)
                    {
                        if (player.Balls >= 15)
                            continue;
                        angle = (float)Game1.rand.NextDouble() * MathHelper.TwoPi;
                        heading = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));



                        player.AddBall();
                        Ball b = new Ball(player, gameObjects[i].Position + new Vector2(0, 0.001f), ConvertUnits.ToSimUnits(10.0f), ConvertUnits.ToSimUnits(screenHeight), world);
                        gameObjects.Add(b);
                        b.Launch(heading);
                    }
                }
            }
        }

        public void SpeedUpBalls(Player player) {
            player.BallSpeed = MathHelper.Clamp(player.BallSpeed * 1.25f, 5.0f, 30.0f);
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i] is Ball && (gameObjects[i] as Ball).Owner == player)
                {
                    (gameObjects[i] as Ball).UpdateSpeed(player.BallSpeed);
                }
            }
        }

        public void SlowDownBalls(Player player)
        {
            player.BallSpeed = MathHelper.Clamp(player.BallSpeed * 0.75f, 5.0f, 30.0f);
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i] is Ball && (gameObjects[i] as Ball).Owner == player)
                {
                    (gameObjects[i] as Ball).UpdateSpeed(player.BallSpeed);
                }
            }
        }

        public void EnlargeBalls(Player player)
        {
            player.BallRadius = MathHelper.Clamp(player.BallRadius * 1.5f, 10f, 50.0f);
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i] is Ball && (gameObjects[i] as Ball).Owner == player)
                {
                    (gameObjects[i] as Ball).UpdateRadius(player.BallRadius);
                }
            }
        }

        public void ShrinkBalls(Player player)
        {
            player.BallRadius = MathHelper.Clamp(player.BallRadius * 0.75f, 3.0f, 30.0f);
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i] is Ball && (gameObjects[i] as Ball).Owner == player)
                {
                    (gameObjects[i] as Ball).UpdateRadius(player.BallRadius);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(AssetManager.GetTexture("pixel"), new Rectangle(0, 0, (int)sideMargin, (int)screenHeight), Color.Black/*new Color(12,12,71,255)/*Color.Lerp(bgFrom,bgTo,bgLerpTime/bgLerpLength)*/);
            spriteBatch.Draw(AssetManager.GetTexture("pixel"), new Rectangle((int)(screenWidth - sideMargin), 0, (int)sideMargin, (int)screenHeight), Color.Black/*new Color(12, 12, 71, 255)/*Color.Lerp(bgFrom, bgTo, bgLerpTime / bgLerpLength)*/);
            foreach (GameObject go in gameObjects)
                go.Draw(spriteBatch);

            


            switch (currentGameState)
            {
                case GameState.Playing:
                    //Draw lives:
                    Texture2D heart = AssetManager.GetTexture("heart");
                    if (player1 != null)
                    {
                        for (int i = 0; i < player1.Lives; i++)
                        {
                            spriteBatch.Draw(heart, new Vector2(sideMargin + i * (heart.Width + 10.0f) + 10.0f, screenHeight - heart.Height - 10.0f), Color.White);
                        }
                    }
                    else if (twoPlayers && player2WaitTime > 0)
                    {
                        string wait = "Wait " + (int)player2WaitTime + " seconds";
                        Vector2 waitSize = mainFont.MeasureString(wait);
                        spriteBatch.DrawString(mainFont, wait, new Vector2(sideMargin + 10.0f, screenHeight - waitSize.Y - 10.0f), Color.White);
                    }
                    else
                    {
                        string pressStart = "Press start to join";
                        Vector2 pressStartSize = mainFont.MeasureString(pressStart);
                        spriteBatch.DrawString(mainFont, pressStart, new Vector2(sideMargin + 10.0f, screenHeight - pressStartSize.Y - 10.0f), Color.White);
                    }

                    Texture2D heart2 = AssetManager.GetTexture("heart_blue");
                    if (player2 != null)
                    {
                        for (int i = 0; i < player2.Lives; i++)
                        {
                            spriteBatch.Draw(heart2, new Vector2(screenWidth - sideMargin - heart2.Width - i * (heart2.Width + 10.0f) - 10.0f, screenHeight - heart2.Height - 10.0f), Color.White);
                        }
                    }
                    else if (twoPlayers && player2WaitTime > 0)
                    {
                        string wait = "Wait " + (int)player2WaitTime + " seconds";
                        Vector2 waitSize = mainFont.MeasureString(wait);
                        spriteBatch.DrawString(mainFont, wait, new Vector2(screenWidth - sideMargin - waitSize.X - 10.0f, screenHeight - waitSize.Y - 10.0f), Color.White);
                    }
                    else
                    {
                        string pressStart = "Press start to join";
                        Vector2 pressStartSize = mainFont.MeasureString(pressStart);
                        spriteBatch.DrawString(mainFont, pressStart, new Vector2(screenWidth - sideMargin - pressStartSize.X - 10.0f, screenHeight - pressStartSize.Y - 10.0f), Color.White);
                    }
                    break;
                case GameState.GameOver:
                    string gameOver = "GAME OVER";
                    string retry = "Press start to retry";
                    Vector2 gameOverSize = mainFont.MeasureString(gameOver);
                    Vector2 retrySize = mainFont.MeasureString(retry);
                    float verticalSpacing = 10.0f;
                    spriteBatch.DrawString(mainFont, gameOver, new Vector2(screenWidth / 2.0f - gameOverSize.X / 2.0f, screenHeight / 2.0f - (gameOverSize.Y + retrySize.Y + verticalSpacing) / 2.0f), Color.White);
                    spriteBatch.DrawString(mainFont, retry, new Vector2(screenWidth / 2.0f - retrySize.X / 2.0f, screenHeight / 2.0f - (gameOverSize.Y + retrySize.Y + verticalSpacing) / 2.0f + gameOverSize.Y + verticalSpacing), Color.White);
                    break;
                default:
                    break;
            }
            
        }

        private void ClearGameObjects() {
            foreach(GameObject go in gameObjects)
                go.CleanUp();
            gameObjects.Clear();
        }

        public Player GetPlayer(PlayerIndex playerIndex) {
            switch (playerIndex)
            {
                case PlayerIndex.One:
                    return player1;
                case PlayerIndex.Two:
                    return player2;
                default:
                    return null;
            }
        }

        private void LoadLevel(string path) {
            
            blocksRemaining = 0;
            using (StreamReader sr = new StreamReader(path)) {
                int j = 0;
                while (!sr.EndOfStream) {
                    string line = sr.ReadLine();
                    for (int i = 0; i < line.Length; i++) {
                        switch (line[i]) {
                            case '1':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f)*tileWidth, (j+0.5f)*tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Teal, 1, Block.BlockType.OneHit));
                                blocksRemaining++;
                                break;
                            case '2':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f) * tileWidth, (j + 0.5f) * tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Yellow, 2, Block.BlockType.TwoHits));
                                blocksRemaining++;
                                break;
                            case '3':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f) * tileWidth, (j + 0.5f) * tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Violet, 3, Block.BlockType.ThreeHits));
                                blocksRemaining++;
                                break;
                            case '4':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f) * tileWidth, (j + 0.5f) * tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Cyan, 4, Block.BlockType.FourHits));
                                blocksRemaining++;
                                break;
                            case '5':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f) * tileWidth, (j + 0.5f) * tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Orange, 5, Block.BlockType.FiveHits));
                                blocksRemaining++;
                                break;
                            case '0':
                                gameObjects.Add(new Block(ConvertUnits.ToSimUnits(new Vector2(sideMargin + (i + 0.5f) * tileWidth, (j + 0.5f) * tileHeight)), ConvertUnits.ToSimUnits(tileWidth), ConvertUnits.ToSimUnits(tileHeight), world, Color.Orange, -1, Block.BlockType.Indestructible));
                               // blocksRemaining++;
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
