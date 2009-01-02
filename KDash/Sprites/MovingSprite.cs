#region

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
        public float Speed = 500f;
        private GameTime spriteTime = new GameTime();
        public Vector2 Destination;

        /// <summary>
        /// Constructor to create the sprite
        /// </summary>
        /// <param name="game">Reference to the main game</param>
        /// <param name="Tex">The texture to draw</param>
        /// <param name="position">The initial position of the sprite in game coordinates</param>
        public MovingSprite(XNADash game, Texture2D Tex, Vector2 position)
            : base(game, Tex, position)
        {
            gameInstance = game;
            Destination = new Vector2();
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
            currentMovement.VerticalVelocity += Speed*spriteTime.ElapsedGameTime.Milliseconds/1000;
            currentMovement.YDirection = MovementVector.DirectionY.Up;
        }

        /// <summary>
        /// Move the sprite to the down.
        /// </summary>
        public void MoveDown()
        {
            currentMovement.VerticalVelocity += Speed*spriteTime.ElapsedGameTime.Milliseconds/1000;
            currentMovement.YDirection = MovementVector.DirectionY.Down;
        }

        /// <summary>
        /// Move the sprite to the left.
        /// </summary>
        public void MoveLeft()
        {
            currentMovement.HorizontalVelocity += Speed*spriteTime.ElapsedGameTime.Milliseconds/1000;
            currentMovement.XDirection = MovementVector.DirectionX.Left;
        }

        /// <summary>
        /// Move the sprite to the right.
        /// </summary>
        public void MoveRight()
        {
            currentMovement.HorizontalVelocity += Speed*spriteTime.ElapsedGameTime.Milliseconds/1000;
            currentMovement.XDirection = MovementVector.DirectionX.Right;
        }

        /// <summary>
        /// Calculates where the sprite moves to in the next frame.
        /// </summary>
        /// <param name="time"></param>
        public void Move(GameTime time)
        {
            spriteTime = time;

            // Only change direction if we are at destination and are standing still
            if (currentMovement.IsMoving())
            {
                // Change position
                if (currentMovement.XDirection == MovementVector.DirectionX.Left)
                    Position.X -= currentMovement.HorizontalVelocity;
                if (currentMovement.XDirection == MovementVector.DirectionX.Right)
                    Position.X += currentMovement.HorizontalVelocity;

                if (currentMovement.YDirection == MovementVector.DirectionY.Up)
                    Position.Y -= currentMovement.VerticalVelocity;
                if (currentMovement.YDirection == MovementVector.DirectionY.Down)
                    Position.Y += currentMovement.VerticalVelocity;
            }

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
                ResolveCollision(tile);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Detetmines the outcome of the sprite collision with a tile.
        /// </summary>
        /// <param name="tile">The tile to check</param>
        public virtual void ResolveCollision(Tile tile)
        {
            if (tile.TileType == TileTypeEnum.MagicWall || tile.TileType == TileTypeEnum.Rock ||
                tile.TileType == TileTypeEnum.TitaniumWall || tile.TileType == TileTypeEnum.Wall)
            {
                // If we collide with a tile we should be "pushed back" to 
                // a place as close as possible to the tile.
                // This is done by getting the tile position and calculate
                // where the player should be relative to it.
                if (currentMovement.XDirection == MovementVector.DirectionX.Left)
                {
                    Position.X = tile.Position.X + tile.Width;
                    MoveStandStill();
                }
                if (currentMovement.XDirection == MovementVector.DirectionX.Right)
                {
                    Position.X = tile.Position.X - tile.Width;
                    MoveStandStill();
                }
                if (currentMovement.YDirection == MovementVector.DirectionY.Up)
                {
                    Position.Y = tile.Position.Y + tile.Height;
                    MoveStandStill();
                }
                if (currentMovement.YDirection == MovementVector.DirectionY.Down)
                {
                    Position.Y = tile.Position.Y - tile.Height;
                    MoveStandStill();
                }
            }

            if (tile.TileType == TileTypeEnum.Diamond)
            {
                tile.TileType = TileTypeEnum.Space;
                tile.Texture = gameInstance.Content.Load<Texture2D>("space");
                gameInstance.DiamondCollected();
            }

            if (tile.TileType == TileTypeEnum.Earth)
            {
                tile.TileType = TileTypeEnum.Space;
                tile.Texture = gameInstance.Content.Load<Texture2D>("space");
            }

            if (tile.TileType == TileTypeEnum.Exit)
            {
                if (gameInstance.CurrentLevel.DiamondsToCollect < gameInstance.Score)
                    gameInstance.Exit();
            }
        }
    }
}