#region

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNADash.Collision;
using XNADash.Level;

#endregion

namespace XNADash.Sprites
{
    /// <summary>
    /// New position formular: x = v * t, where x = position v = velocity t = time
    /// </summary>
    public class MovingSprite : Sprite
    {
        public MovementVector currentMovement = new MovementVector();
        public Vector2 Destination;
        public float Speed = 500f;
        private GameTime spriteTime = new GameTime();

        public delegate void DashHandler(string msg);
        public static event DashHandler CollisionEvent;

        /// <summary>
        /// Default contructor
        /// </summary>
        public MovingSprite()
        {
        }

        /// <summary>
        /// Constructor to create the sprite
        /// </summary>
        /// <param name="game">Reference to the main game</param>
        /// <param name="Tex">The texture to draw</param>
        /// <param name="position">The initial position of the sprite in game coordinates</param>
        public MovingSprite(XNADash game, Texture2D Tex, Vector2 position) : base(game, Tex, position)
        {
            gameInstance = game;
            Destination = position;
            MoveStandStill();
        }

        /// <summary>
        /// Resets the movement of the sprite.
        /// </summary>
        public void MoveStandStill()
        {
            currentMovement.HorizontalVelocity = 0;
            currentMovement.VerticalVelocity = 0;
            currentMovement.XDirection = MovementVector.DirectionX.None;
            currentMovement.YDirection = MovementVector.DirectionY.None;
        }

        /// <summary>
        /// Move the sprite to the up.
        /// </summary>
        public void MoveUp()
        {
            if (spriteTime.ElapsedGameTime.Milliseconds > 0)
            {
                currentMovement.VerticalVelocity += Speed*spriteTime.ElapsedGameTime.Milliseconds/1000;
                currentMovement.YDirection = MovementVector.DirectionY.Up;
            }
        }

        /// <summary>
        /// Move the sprite to the down.
        /// </summary>
        public void MoveDown()
        {
            if (spriteTime.ElapsedGameTime.Milliseconds > 0)
            {
                currentMovement.VerticalVelocity += Speed*spriteTime.ElapsedGameTime.Milliseconds/1000;
                currentMovement.YDirection = MovementVector.DirectionY.Down;
            }
        }

        /// <summary>
        /// Move the sprite to the left.
        /// </summary>
        public void MoveLeft()
        {
            if (spriteTime.ElapsedGameTime.Milliseconds > 0)
            {
                currentMovement.HorizontalVelocity += Speed*spriteTime.ElapsedGameTime.Milliseconds/1000;
                currentMovement.XDirection = MovementVector.DirectionX.Left;
            }
        }

        /// <summary>
        /// Move the sprite to the right.
        /// </summary>
        public void MoveRight()
        {
            if (spriteTime.ElapsedGameTime.Milliseconds > 0)
            {
                currentMovement.HorizontalVelocity += Speed*spriteTime.ElapsedGameTime.Milliseconds/1000;
                currentMovement.XDirection = MovementVector.DirectionX.Right;
            }
        }

        /// <summary>
        /// Calculates where the sprite moves to in the next frame.
        /// </summary>
        /// <param name="time"></param>
        public void Move(GameTime time)
        {
            spriteTime = time;

            // Only change direction if we are at destination and are standing still
            //if (currentMovement.IsMoving())
            //{
            // Change position
            if (currentMovement.XDirection == MovementVector.DirectionX.Left)
                Position.X -= currentMovement.HorizontalVelocity;
            if (currentMovement.XDirection == MovementVector.DirectionX.Right)
                Position.X += currentMovement.HorizontalVelocity;

            if (currentMovement.YDirection == MovementVector.DirectionY.Up)
                Position.Y -= currentMovement.VerticalVelocity;
            if (currentMovement.YDirection == MovementVector.DirectionY.Down)
                Position.Y += currentMovement.VerticalVelocity;
            //}

            // Make sprite stop at the correct position
            if (currentMovement.XDirection == MovementVector.DirectionX.Left && Position.X < Destination.X)
            {
                Position.X = Destination.X;
                MoveStandStill();
            }
            if (currentMovement.XDirection == MovementVector.DirectionX.Right && Position.X > Destination.X)
            {
                Position.X = Destination.X;
                MoveStandStill();
            }
            if (currentMovement.YDirection == MovementVector.DirectionY.Up && Position.Y < Destination.Y)
            {
                Position.Y = Destination.Y;
                MoveStandStill();
            }
            if (currentMovement.YDirection == MovementVector.DirectionY.Down && Position.Y > Destination.Y)
            {
                Position.Y = Destination.Y;
                MoveStandStill();
            }

            CalculateBounds();
        }

        /// <summary>
        /// Checks if the sprite is colliding with the tile
        /// </summary>
        /// <param name="tile">The sprite to check if</param>
        /// <returns>True if the tile collides with the sprite, false if not</returns>
        public virtual bool CollidesWith(Tile tile)
        {
            bool result = false;

            if (CollisionUtility.Intersects(bounds, tile.GetBounds()))
            {
                CollisionUtility.ResolvePlayerTileCollision(this, tile);
                result = true;
            }

            return result;
        }

        public void DiamondCollected()
        {
            CollisionEvent("inverted by z-axis");
        }
    }
}