#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNADash.Animation;

#endregion

namespace XNADash.Sprites
{
    /// <summary>
    /// The basic sprite that has information on position, texture and collision check
    /// </summary>
    public class Sprite
    {
        public Rectangle bounds;
        public Color color;
        protected XNADash gameInstance;
        public Vector2 Position;
        public Texture2D texture;

        /// <summary>
        /// Default constructor, not used
        /// </summary>
        public Sprite()
        {
        }

        /// <summary>
        /// Initializes the sprite
        /// </summary>
        /// <param name="game">Reference to the main game</param>
        /// <param name="Tex">The sprite texture</param>
        /// <param name="position">The position in world coodinates</param>
        public Sprite(XNADash game, Texture2D Tex, Vector2 position)
        {
            gameInstance = game;
            texture = Tex;
            Position = position;
            color = Color.White;
            CalculateBounds();
        }

        /// <summary>
        /// Returns the center of the sprite texture.
        /// </summary>
        public Vector2 CenterPosition
        {
            get { return new Vector2((int) Position.X + (texture.Width/2), (int) Position.Y + (texture.Height/2)); }
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        public void Dispose()
        {
            texture.Dispose();
        }

        /// <summary>
        /// Adds the sprite to the <see cref="SceneGraph"/> object for rendering
        /// </summary>
        /// <param name="batch">The sprite batch reference</param>
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, Position, Color.Black);
        }

        /// <summary>
        /// Updates the collision bounds for the sprite.
        /// </summary>
        public void CalculateBounds()
        {
            bounds = new Rectangle((int) Position.X, (int) Position.Y, texture.Width, texture.Height);
        }

        /// <summary>
        /// Adds the sprite texture to the scene
        /// </summary>
        /// <returns>A <see cref="Scene2DNode"/> to use in the scene</returns>
        public Scene2DNode ToSceneGraphNode()
        {
            Scene2DNode returnValue = new Scene2DNode(texture, Position);

            return returnValue;
        }
    }
}