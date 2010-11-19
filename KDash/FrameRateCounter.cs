#region

using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace XNADash
{
    /// <summary>
    /// Component to draw the current framerate.
    /// </summary>
    public class FrameRateCounter : DrawableGameComponent
    {
        make build fail
        private readonly ContentManager content;
        private readonly Vector2 drawPosition;
        private readonly Vector2 drawPositionShadow;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private int frameCounter;
        private int frameRate;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game">Reference to the game</param>
        /// <param name="position">Screen posision to draw the component at</param>
        public FrameRateCounter(Game game, Vector2 position)
            : base(game)
        {
            if (game != null && game.Services != null) 
                content = new ContentManager(game.Services);
            content.RootDirectory = "Content";
            drawPosition = position;
            drawPositionShadow = new Vector2(position.X - 1, position.Y - 1);
        }

        /// <summary>
        /// Loads the content used by the component.
        /// Uses the font 'david'.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("david");
        }

        /// <summary>
        /// Unloads the content used by the component.
        /// </summary>
        protected override void UnloadContent()
        {
            content.Unload();
        }

        /// <summary>
        /// Updates the component.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        /// <summary>
        /// Draws the component.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format(CultureInfo.CurrentCulture, "fps: {0}", frameRate);

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, fps, drawPosition, Color.Black);
            spriteBatch.DrawString(spriteFont, fps, drawPositionShadow, Color.White);

            spriteBatch.End();
        }
    }
}