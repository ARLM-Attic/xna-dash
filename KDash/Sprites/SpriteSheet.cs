#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace XNADash.Sprites
{
    /// <summary>
    /// Stores entries for individual sprites on a single texture.
    /// </summary>
    public class SpriteSheet
    {
        #region Fields

        private readonly Dictionary<int, Rectangle> spriteDefinitions;
        private readonly Texture2D texture;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Sprite Sheet
        /// </summary>
        public SpriteSheet(Texture2D sheetTexture)
        {
            texture = sheetTexture;
            spriteDefinitions = new Dictionary<int, Rectangle>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the source sprite texture
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }

        /// <summary>
        /// Get the rectangle that defines the source sprite
        /// on the sheet.
        /// </summary>
        public Rectangle this[int i]
        {
            get { return spriteDefinitions[i]; }
        }

        /// <summary>
        /// Add a source sprite for fast retrieval
        /// </summary>
        public void AddSourceSprite(int key, Rectangle rect)
        {
            spriteDefinitions.Add(key, rect);
        }

        /// <summary>
        /// A faster lookup using refs to avoid stack copies.
        /// </summary>
        public void GetRectangle(ref int i, out Rectangle rect)
        {
            rect = spriteDefinitions[i];
        }

        #endregion
    }
}