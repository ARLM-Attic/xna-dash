#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace XNADash.Level
{
    /// <summary>
    /// The tile class holds information on the specific point in the level.
    /// The primary information would be the <see cref="Texture"/> attributte that holds the
    /// graphics displayed.
    /// </summary>
    public class Tile
    {
        public bool Collision;
        public Vector2 Position;
        public Texture2D Texture;
        public TileTypeEnum TileType;

        /// <summary>
        /// The height of the tile.
        /// </summary>
        public int Height
        {
            get
            {
                if (Texture != null)
                    return Texture.Width;

                return 0;
            }
        }

        /// <summary>
        /// The width of the tile.
        /// </summary>
        public int Width
        {
            get
            {
                if (Texture != null)
                    return Texture.Width;

                return 0;
            }
        }

        /// <summary>
        /// Returns the tile index
        /// </summary>
        /// <returns>The index</returns>
        public Vector2 ToTileIndex()
        {
            Vector2 result = Position;

            result.X = result.X/Width;
            result.Y = result.Y/Height;

            return result;
        }

        /// <summary>
        /// Gets the bounds of the tile.
        /// </summary>
        /// <returns>A rectangle representing the mass of the tile</returns>
        public Rectangle GetBounds()
        {
            Rectangle result = new Rectangle((int) Position.X, (int) Position.Y, Texture.Width, Texture.Height);

            return result;
        }
    }
}