#region

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNADash.Animation;
using XNADash.Level;
using XNADash.SoundFx;
using XNADash.Sprites;

#endregion

namespace XNADash
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XNADash : Game
    {
        public static GraphicsDeviceManager graphics;
        private readonly Vector2 HUDPosition;
        private bool playerIsDead;

        public Camera2D camera;
        private string collisionDebugString;
        private KeyboardState currentKeyboardState;
        private Level.Level currentLevel;
        private Vector2 displaySize;
        public SpriteFont font;
        public SpriteFont bigFont;
        private HUD gameHUD;
        private MovingSprite playerSprite;
        private SceneGraph sceneGraph;
        private int score;
        private int diamondsCollected;
        private SpriteBatch spriteBatch;
        public bool visibilityChanged;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public XNADash()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

            Content.RootDirectory = "Content";
            score = 0;
            HUDPosition = new Vector2(30, graphics.PreferredBackBufferHeight - 75);

            playerIsDead = false;
        }

        /// <summary>
        /// Adds the points to the score when collecting a diamond
        /// </summary>
        /// <param name="points">The points given</param>
        void OnDiamondCollected(int points)
        {
            if (points > 0)
            {
                score += points;
                DiamondsCollected++;
            }
            else
                throw new ArgumentException("Score must be higher than 0");
        }

        public Level.Level CurrentLevel
        {
            get { return currentLevel; }
        }

        /// <summary>
        /// The players current score
        /// </summary>
        public int Score
        {
            get { return score; }
        }

        public int DiamondsCollected
        {
            get { return diamondsCollected; }
            set { diamondsCollected = value; }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.DeviceReset += OnGraphicsComponentDeviceReset;
            OnGraphicsComponentDeviceReset(this, new EventArgs());

            // Add Frame per second counter
            Components.Add(new FrameRateCounter(this, new Vector2(800, 0)));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // MUST be set as one of the first things!! 
            //TODO: Find a better way of doing this resource management
            GraphicsResourceManager.Instance.contentManager = Content;
            font = Content.Load<SpriteFont>("david");
            bigFont = Content.Load<SpriteFont>("DashFontBig2");

            currentLevel = new Level.Level(this);
            CurrentLevel.LoadLevel("level\\level01.lvl");
            //level1.LoadLevel("level\\TESTLEVEL.lvl");
            //gameLevels.Add(level1);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Prepare scene
            sceneGraph = new SceneGraph(this, spriteBatch);

            // Set up player sprite
            playerSprite = new MovingSprite(this, Content.Load<Texture2D>("player2"), CurrentLevel.StartPosition);
            MovingSprite.DiamondCollected += new MovingSprite.DiamondEventHandler(OnDiamondCollected);

            Tile firstTile = CurrentLevel.GetTile(ref playerSprite.Position);
            Tile.TileCollision += new Tile.LevelEventHandler(OnTileCollision);

            // Set Up a 2D Camera
            camera = new Camera2D(spriteBatch);

            // Start in the middle of the level
            camera.Position = playerSprite.Position;
            camera.worldWidth = CurrentLevel.WorldWidth;
            camera.worldHeight = CurrentLevel.WorldHeight;
            camera.HasMoved = true;
            visibilityChanged = true;

            // Set up the game HUD
            gameHUD = new HUD(spriteBatch);
            gameHUD.LevelTimer.SetTimer(CurrentLevel.FinishTime);
            gameHUD.LevelTimer.Start();

            SoundFxManager.Instance.PlaySound(SoundFxManager.CueEnums.start);
        }

        void OnTileCollision(TileTypeEnum tileType)
        {
            if (CurrentLevel.DiamondsToCollect == DiamondsCollected)
            {
                SoundFxManager.Instance.PlaySound(SoundFxManager.CueEnums.applause);
                Thread.Sleep(6000);
                Exit();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            playerSprite.Move(gameTime);

            CurrentLevel.NpcContainer.Update(gameTime);

            if (playerIsDead == false)
            {
                // Check if player touches an enemy
                foreach (EnemySprite sprite in CurrentLevel.NpcContainer.NPCList)
                {
                    if (playerSprite.CollidesWith(sprite))
                    {
                        playerIsDead = true;
                        Thread.Sleep(3000);
                        Exit();
                    }
                }
            }

            // Check the tiles the player touches
            List<Tile> tileToCheck = CurrentLevel.GetTiles(playerSprite.bounds);
            foreach (Tile tile in tileToCheck)
            {
                if (tile != null && playerSprite.CollidesWith(tile))
                {
                    collisionDebugString = "Player collides with tile " + tile.Position + " " + tile.TileType;
                    tile.Collision = true;
                }
            }

            // Make camera follow player
            int movement = (int) (playerSprite.Speed*(float) gameTime.ElapsedGameTime.TotalMilliseconds/1000);
            camera.Speed = movement;
            camera.CenterAt(playerSprite.CenterPosition);

            // Update the HUD
            gameHUD.Update(Score.ToString(), DiamondsCollected, currentLevel.DiamondsToCollect);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Since we're drawing in order from back to front, depth buffer is disabled
            graphics.GraphicsDevice.RenderState.DepthBufferEnable = false;
            graphics.GraphicsDevice.Clear(Color.Black);

            // Prepare scene
            sceneGraph.NewScene();

            CurrentLevel.ResetCollisionDebugInfo();
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            // AddToScene the background
            CurrentLevel.Draw(sceneGraph, spriteBatch, Color.Black);
            // AddToScene the player
            sceneGraph.AddToScene(playerSprite.ToSceneGraphNode());
            // AddToScene the enemies
            sceneGraph.AddToScene(CurrentLevel.NpcContainer.AddToScene());

            // Write debug info
            sceneGraph.AddText("Player position: " + playerSprite.Position + " Player destination: " +
                               playerSprite.Destination);
            sceneGraph.AddText("Tile position:" + CurrentLevel.ToTileCoordinate(playerSprite.Position));
            sceneGraph.AddText("Player is dead:" + playerIsDead);
            sceneGraph.AddText("Player keypress: " + playerSprite.currentMovement.XDirection + " " +
                               playerSprite.currentMovement.YDirection);
            sceneGraph.AddText(collisionDebugString);
            sceneGraph.AddText("Cam position:" + camera.Position);

            sceneGraph.Draw();

            gameHUD.Draw(bigFont, HUDPosition);

            base.Draw(gameTime);
            spriteBatch.End();
        }

        /// <summary>
        /// Handles the input from the player so it is possible to move the player sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public void HandleInput(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            // Check for exit.
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // Move player
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (playerSprite.currentMovement.IsMoving() == false)
                {
                    playerSprite.Destination = playerSprite.Position;
                    playerSprite.Destination.X -= CurrentLevel.CellWidth;
                    playerSprite.MoveLeft();
                    return;
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (playerSprite.currentMovement.IsMoving() == false)
                {
                    playerSprite.Destination = playerSprite.Position;
                    playerSprite.Destination.X += CurrentLevel.CellWidth;
                    playerSprite.MoveRight();
                    return;
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                if (playerSprite.currentMovement.IsMoving() == false)
                {
                    playerSprite.Destination = playerSprite.Position;
                    playerSprite.Destination.Y -= CurrentLevel.CellHeight;
                    playerSprite.MoveUp();
                    return;
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (playerSprite.currentMovement.IsMoving() == false)
                {
                    playerSprite.Destination = playerSprite.Position;
                    playerSprite.Destination.Y += CurrentLevel.CellHeight;
                    playerSprite.MoveDown();
                    return;
                }
            }
        }

        /// <summary>
        /// Sets the correct screen size
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event details</param>
        private void OnGraphicsComponentDeviceReset(object sender, EventArgs e)
        {
            displaySize.X = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            displaySize.Y = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            visibilityChanged = true;
        }
    }
}