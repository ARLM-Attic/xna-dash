
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNADash
{
    /// <summary>
    /// The HUD (Heads Up Display) shows the current game state for the player.
    /// How many lives are left, what the score is and so on.
    /// This is the first very simple version!!
    /// </summary>
    class HUD
    {
        public string Score;
        public string Time;
        public string TimeLeft;
        private readonly SpriteBatch batch;
        private string HUDString;

        /// <summary>
        /// Constructor to initialize the HUD
        /// </summary>
        /// <param name="sb">The sprite batch use draw with</param>
        public HUD(SpriteBatch sb)
        {
            batch = sb;
            Score = "Score ";
            Time = " Time ";
            TimeLeft = " Time left ";
            HUDString = "";
        }

        /// <summary>
        /// Updates the HUD with the current game state
        /// </summary>
        /// <param name="currentScore">The players score</param>
        /// <param name="currentTime">How much time has gone</param>
        /// <param name="currentTimeLeft">How much time is left to complete</param>
        public void Update(string currentScore, string currentTime, string currentTimeLeft)
        {
            HUDString = Score + currentScore + Time + currentTime + TimeLeft + currentTimeLeft;
        }

        /// <summary>
        /// Draws the HUD
        /// </summary>
        /// <param name="font">The font to use</param>
        /// <param name="position">The screen posision to draw at</param>
        public void Draw(SpriteFont font, Vector2 position)
        {
            batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            batch.DrawString(font, HUDString, position, Color.Turquoise);
            batch.End();
        }
    }
}
