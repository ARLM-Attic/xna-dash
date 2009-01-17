#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNADash.Level;

#endregion

namespace XNADash
{
    /// <summary>
    /// The HUD (Heads Up Display) shows the current game state for the player.
    /// How many lives are left, what the score is and so on.
    /// This is the first very simple version!!
    /// </summary>
    internal class HUD
    {
        private readonly SpriteBatch batch;
        private readonly CountDownTimer timer;
        private string HUDString;
        public string Score;
        public string TimeLeft;

        /// <summary>
        /// Constructor to initialize the HUD
        /// </summary>
        /// <param name="sb">The sprite batch use draw with</param>
        public HUD(SpriteBatch sb)
        {
            batch = sb;
            Score = "Score ";
            TimeLeft = " Time left ";
            HUDString = "";
            timer = new CountDownTimer();
        }

        public CountDownTimer LevelTimer
        {
            get { return timer; }
        }

        /// <summary>
        /// Updates the HUD with the current game state
        /// </summary>
        /// <param name="currentScore">The players score</param>
        public void Update(string currentScore)
        {
            HUDString = Score + currentScore + TimeLeft + LevelTimer;
        }

        /// <summary>
        /// Draws the HUD
        /// </summary>
        /// <param name="font">The font to use</param>
        /// <param name="position">The screen posision to draw at</param>
        public void Draw(SpriteFont font, Vector2 position)
        {
            batch.DrawString(font, HUDString, position, Color.Turquoise);
        }
    }
}