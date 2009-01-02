#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNADash.Collision;
using XNADash.Level;

#endregion

namespace XNADash.Sprites
{
    /// <summary>
    /// The EmemySprite object is an instance of the 
    /// </summary>
    public class EnemySprite : MovingSprite, NPCMovement
    {
        private readonly MovementVector movementVector;
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
            Speed = 500f;

            movementVector = new MovementVector();
            MoveStandStill();
        }

        #region NPCMovement Members

        /// <summary>
        /// Moves the sprite automatically since it's a NPC.
        /// Should be called from the update method of the game
        /// </summary>
        public void NextMove()
        {
            //TODO: Finish implementing movement algorithm
            // Update visitedPostions
            MarkAsVisited(Position);
            // Update path


            // Get sourrounding tiles
            surroundingTiles = gameInstance.CurrentLevel.GetSurroundingTiles(Position);
            // Select next tile to go to if we're done moving
            if (surroundingTiles != null &&
                movementVector.XDirection == MovementVector.DirectionX.None &&
                movementVector.YDirection == MovementVector.DirectionY.None)
            {
                if (surroundingTiles[0] != null && surroundingTiles[0].TileType == TileTypeEnum.Space && !visitedPositions.Contains(surroundingTiles[0].Position))
                    Destination = surroundingTiles[0].Position;
                else if (surroundingTiles[1] != null && surroundingTiles[1].TileType == TileTypeEnum.Space && !visitedPositions.Contains(surroundingTiles[1].Position))
                    Destination = surroundingTiles[1].Position;
                else if (surroundingTiles[3] != null && surroundingTiles[3].TileType == TileTypeEnum.Space && !visitedPositions.Contains(surroundingTiles[3].Position))
                    Destination = surroundingTiles[3].Position;
                else if (surroundingTiles[2] != null && surroundingTiles[2].TileType == TileTypeEnum.Space && !visitedPositions.Contains(surroundingTiles[2].Position))
                    Destination = surroundingTiles[2].Position;
                else
                    visitedPositions = new List<Vector2>();

                DetermineDirection();
            }

            // If nowhere to go, select previous
        }

        #endregion

        /// <summary>
        /// Determines in what direction the NPC should move.
        /// </summary>
        private void DetermineDirection()
        {
            if (Position.Y > Destination.Y)
                MoveUp();
            else if (Position.X < Destination.X)
                MoveRight();
            else if (Position.Y < Destination.Y)
                MoveDown();
            else if (Position.X > Destination.X)
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
                {
                    MoveStandStill();
                    Position.X = tile.Position.X + tile.Width;
                }
                if (currentMovement.XDirection == MovementVector.DirectionX.Right)
                {
                    MoveStandStill();
                    Position.X = tile.Position.X - tile.Width;
                }
                if (currentMovement.YDirection == MovementVector.DirectionY.Up)
                {
                    MoveStandStill();
                    Position.Y = tile.Position.Y + tile.Height;
                }
                if (currentMovement.YDirection == MovementVector.DirectionY.Down)
                {
                    MoveStandStill();
                    Position.Y = tile.Position.Y - tile.Height;
                }
            }
        }

        private void MarkAsVisited(Vector2 position)
        {
            if (!visitedPositions.Contains(position))
                visitedPositions.Add(position);
        }
    }
}