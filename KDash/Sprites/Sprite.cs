#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNADash.Animation;
using XNADash.Collision;

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
        }

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
        /// <param name="scene">The scene graph reference</param>
        /// <param name="batch">The sprite batch reference</param>
        public void Draw(SceneGraph scene, SpriteBatch batch)
        {
            Scene2DNode spriteNode = new Scene2DNode(texture, Position);
            scene.AddToScene(spriteNode);
        }

        /// <summary>
        /// Performs a rectangle check to see if the two sprites are overlapping.
        /// This is NOT a per pixel collision check.
        /// </summary>
        /// <param name="targetSprite">The target sprite</param>
        /// <param name="sourceSprite">The source sprite</param>
        /// <returns>True if the sprites are overlapping, false if not</returns>
        public bool Intersects(Sprite targetSprite, Sprite sourceSprite)
        {
            targetSprite.CalculateBounds();
            sourceSprite.CalculateBounds();

            if (CollisionUtility.Intersects(targetSprite.bounds, sourceSprite.bounds))
            {
                uint[] bitsA = new uint[targetSprite.texture.Width*targetSprite.texture.Height];
                targetSprite.texture.GetData(bitsA);

                uint[] bitsB = new uint[sourceSprite.texture.Width*sourceSprite.texture.Height];
                sourceSprite.texture.GetData(bitsB);

                int x1 = Math.Max(targetSprite.bounds.X, sourceSprite.bounds.X);
                int x2 = Math.Min(targetSprite.bounds.X + targetSprite.bounds.Width,
                                  sourceSprite.bounds.X + sourceSprite.bounds.Width);

                int y1 = Math.Max(targetSprite.bounds.Y, sourceSprite.bounds.Y);
                int y2 = Math.Min(targetSprite.bounds.Y + targetSprite.bounds.Height,
                                  sourceSprite.bounds.Y + sourceSprite.bounds.Height);

                for (int y = y1; y < y2; ++y)
                {
                    for (int x = x1; x < x2; ++x)
                    {
                        if (
                            ((bitsA[(x - targetSprite.bounds.X) + (y - targetSprite.bounds.Y)*targetSprite.texture.Width
                                  ] & 0xFF000000) >> 24) > 20 &&
                            ((bitsB[(x - sourceSprite.bounds.X) + (y - sourceSprite.bounds.Y)*sourceSprite.texture.Width
                                  ] & 0xFF000000) >> 24) > 20)
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the sprite is colliding with a list of sprite
        /// </summary>
        /// <param name="sprites">The list of sprites</param>
        /// <returns>True if there's a collision</returns>
        public bool CollidesWith(List<Sprite> sprites)
        {
            foreach (Sprite mightCollide in sprites)
                if (mightCollide.CollidesWith(this))
                    return true;

            return false;
        }

        /// <summary>
        /// Checks if there's a collision with the sprite specified
        /// </summary>
        /// <param name="sprite">The sprite to check collision with</param>
        /// <returns>True if there's a collision, false if not</returns>
        public bool CollidesWith(Sprite sprite)
        {
            return Intersects(this, sprite);
        }

        /// <summary>
        /// What should happend if the collision occours
        /// </summary>
        /// <param name="sprite">The sprite we have collided with</param>
        public void ResolveCollision(Sprite sprite)
        {
            if (sprite is EnemySprite)
            {
                //Die();
            }
            //else if (sprite is ShotSprite)
            //{
            //    Health -= (sprite as ShotSprite).Damage;
            //    (sprite as ShotSprite).MarkForDeletion();
            //}
        }

        /// <summary>
        /// Updates the collision bounds for the sprite.
        /// </summary>
        public void CalculateBounds()
        {
            bounds = new Rectangle((int) Position.X, (int) Position.Y, texture.Width, texture.Height);
        }
    }
}