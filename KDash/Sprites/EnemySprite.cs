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
        public enum EnemyEnum
        {
            Butterfly,
            Firefly
        }

        private readonly EnemyEnum enemyType;

        /// <summary>
        /// Initializes the sprite
        /// </summary>
        /// <param name="game">Reference to the game</param>
        /// <param name="enemy">The type enemy</param>
        /// <param name="position">The starting position</param>
        public EnemySprite(XNADash game, EnemyEnum enemy, Vector2 position)
        {
            enemyType = enemy;
            if (enemyType == EnemyEnum.Butterfly)
                texture = game.Content.Load<Texture2D>("butterfly");
            else if (enemyType == EnemyEnum.Firefly)
                texture = game.Content.Load<Texture2D>("firefly");

            visitedPositions = new List<Vector2>();
            Speed = 500f;

            movementVector = new MovementVector();
            MoveStandStill();
            gameInstance = game;
            Position = position;
        }

        public EnemyEnum EnemyType
        {
            get { return enemyType; }
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
                CollisionUtility.ResolveNPCTileCollision(this, tile);
                result = true;
            }

            return result;
        }

        private void MarkAsVisited(Vector2 position)
        {
            if (!visitedPositions.Contains(position))
                visitedPositions.Add(position);
        }
    }
}