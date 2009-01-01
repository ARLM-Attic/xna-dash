#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNADash.Collision;
using XNADash.Level;

#endregion

namespace XNADash.Sprites
{
    public class EnemySprite : MovingSprite, NPCMovement
    {
        private readonly MovementVector movementVector;
        private Vector2 moveToPosition;
        private List<Vector2> myPath;
        private Tile[] surroundingTiles;
        private List<Vector2> visitedPositions;

        /// <summary>
        /// Initializes the sprite
        /// </summary>
        /// <param name="game">Reference to the game</param>
        /// <param name="Tex">The sprite texture</param>
        /// <param name="position">The starting position</param>
        public EnemySprite(XNADash game, Texture2D Tex, Vector2 position) : base(game, Tex, position)
        {
            visitedPositions = new List<Vector2>();
            myPath = new List<Vector2>();
            Speed = 10;

            movementVector = new MovementVector();
            movementVector.XDirection = MovementVector.DirectionX.None;
            movementVector.YDirection = MovementVector.DirectionY.None;
            movementVector.HorizontalVelocity = 0;
            movementVector.VerticalVelocity = 0;
        }

        #region NPCMovement Members

        /// <summary>
        /// Moves the sprite automatically since it's a NPC.
        /// Should be called from the update method of the game
        /// </summary>
        public void NextMove()
        {
            // Update visitedPostions
            // Update path
            // Get sourrounding tiles
            surroundingTiles = gameInstance.CurrentLevel.GetSurroundingTiles(Position);
            // Select next tile to go to if we're done moving
            if (surroundingTiles != null &&
                movementVector.XDirection == MovementVector.DirectionX.None &&
                movementVector.YDirection == MovementVector.DirectionY.None)
            {
                if (surroundingTiles[0] != null && surroundingTiles[0].TileType == TileTypeEnum.Space)
                    moveToPosition = surroundingTiles[0].Position;
                else if (surroundingTiles[1] != null && surroundingTiles[1].TileType == TileTypeEnum.Space)
                    moveToPosition = surroundingTiles[1].Position;
                else if (surroundingTiles[2] != null && surroundingTiles[2].TileType == TileTypeEnum.Space)
                    moveToPosition = surroundingTiles[2].Position;
                else if (surroundingTiles[3] != null && surroundingTiles[3].TileType == TileTypeEnum.Space)
                    moveToPosition = surroundingTiles[3].Position;

                DetermineDirection();
            }
            // If nowhere to go, select previous
        }

        #endregion

        private void DetermineDirection()
        {
            if (Position.Y > moveToPosition.Y)
                MoveUp();
            else if (Position.X < moveToPosition.X)
                MoveRight();
            else if (Position.Y < moveToPosition.Y)
                MoveDown();
            else if (Position.X > moveToPosition.X)
                MoveLeft();
        }

        public override bool CollidesWith(Tile tile)
        {
            bool result = false;

            if (CollisionUtility.Intersects(bounds, tile.GetBounds()))
            {
                ResolveCollision(tile);
                result = true;
            }

            return result;
        }

        public override void ResolveCollision(Tile tile)
        {
            if (tile.TileType != TileTypeEnum.Space)
            {
                if (currentMovement.XDirection == MovementVector.DirectionX.Left)
                    Position.X = tile.Position.X + tile.Width;
                if (currentMovement.XDirection == MovementVector.DirectionX.Right)
                    Position.X = tile.Position.X - tile.Width;
                if (currentMovement.YDirection == MovementVector.DirectionY.Up)
                    Position.Y = tile.Position.Y + tile.Height;
                if (currentMovement.YDirection == MovementVector.DirectionY.Down)
                    Position.Y = tile.Position.Y - tile.Height;
            }
        }
    }
}