#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace XNADash.Animation
{
    /// <summary>
    /// This represents a node in the scene graph
    /// </summary>
    public class Scene2DNode
    {
        private readonly Texture2D texture;
        public Vector2 Position;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">The texture to draw</param>
        /// <param name="position">The world position</param>
        public Scene2DNode(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            Position = position;
        }

        /// <summary>
        /// called by our camera class.
        /// </summary>
        /// <param name="renderer">The sprite batch reference</param>
        /// <param name="drawPosition"></param>
        public void Draw(SpriteBatch renderer, Vector2 drawPosition)
        {
            renderer.Draw(texture, drawPosition, Color.White);
        }
    }
}