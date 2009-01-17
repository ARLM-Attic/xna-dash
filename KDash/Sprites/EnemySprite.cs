#region

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
        #region EnemyEnum enum

        /// <summary>
        /// The enemy type
        /// </summary>
        public enum EnemyEnum
        {
            Butterfly,
            Firefly
        }

        #endregion

        private readonly EnemyEnum enemyType;
        public MovementVector previousMovement = new MovementVector();
        private Tile[] surroundingTiles;

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

            Speed = 500f;

            MoveUp();
            gameInstance = game;
            Position = position;
            Destination = position;
        }

        public EnemyEnum EnemyType
        {
            get { return enemyType; }
        }

        #region NPCMovement Members

        /// <summary>
        /// Moves the sprite automatically since it's a NPC.
        /// Should be called from the update method of the game.
        /// The NPC movement is implemented as a "follow the wall" algorithm, meaning
        /// the NPC will always have the wall on the left (or right) and when encountering
        /// an obstacle, it turns left (or right)
        /// </summary>
        public void NextMove()
        {
            //TODO: Implement follow wall algorithm
            if (currentMovement.IsMoving() == false)
            {
                if (CanMove())
                {
                    // Select next tile to go to if we're done moving
                    if (surroundingTiles != null)
                    {
                        // If we were going up, then try right if we can't go there
                        if (previousMovement.YDirection == MovementVector.DirectionY.Up)
                        {
                            if (surroundingTiles[0].TileType == TileTypeEnum.Space)
                                Destination = surroundingTiles[0].Position;
                            else
                                MoveRight();
                        }
                        
                            // If we were going left, then try right if we can't go there
                        else if (previousMovement.XDirection == MovementVector.DirectionX.Right)
                        {
                            if (surroundingTiles[2].TileType == TileTypeEnum.Space)
                                Destination = surroundingTiles[2].Position;
                            else
                                MoveDown();
                        }

                            // If we were going left, then try right if we can't go there
                        else if (previousMovement.YDirection == MovementVector.DirectionY.Down)
                        {
                            if (surroundingTiles[3].TileType == TileTypeEnum.Space)
                                Destination = surroundingTiles[3].Position;
                            else
                                MoveLeft();
                        }

                            // If we were going left, then try right if we can't go there
                        else if (previousMovement.XDirection == MovementVector.DirectionX.Left)
                        {
                            if (surroundingTiles[1].TileType == TileTypeEnum.Space)
                                Destination = surroundingTiles[1].Position;
                            else
                                MoveUp();
                        }

                        DetermineDirection();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Determines in what direction the NPC should move.
        /// </summary>
        private void DetermineDirection()
        {
            if (previousMovement.IsMoving())
            {
                if (Position.Y > Destination.Y)
                    MoveUp();
                else if (Position.X < Destination.X)
                    MoveRight();
                else if (Position.Y < Destination.Y)
                    MoveDown();
                else if (Position.X > Destination.X)
                    MoveLeft();

                previousMovement.YDirection = currentMovement.YDirection;
                previousMovement.XDirection = currentMovement.XDirection;
                previousMovement.HorizontalVelocity = currentMovement.HorizontalVelocity;
                previousMovement.VerticalVelocity = currentMovement.VerticalVelocity;
            }
            else
            {
                if (previousMovement.IsMoving() == false && CanMove())
                {
                    if (surroundingTiles[0].TileType == TileTypeEnum.Space)
                        MoveUp();
                    if (surroundingTiles[1].TileType == TileTypeEnum.Space)
                        MoveLeft();
                    if (surroundingTiles[2].TileType == TileTypeEnum.Space)
                        MoveRight();
                    if (surroundingTiles[3].TileType == TileTypeEnum.Space)
                        MoveDown();
                }

                previousMovement.YDirection = currentMovement.YDirection;
                previousMovement.XDirection = currentMovement.XDirection;
                previousMovement.HorizontalVelocity = currentMovement.HorizontalVelocity;
                previousMovement.VerticalVelocity = currentMovement.VerticalVelocity;
            }
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

        /// <summary>
        /// Deterimes if the NPC can move.
        /// </summary>
        /// <returns>True if it is possible to move, false if not</returns>
        private bool CanMove()
        {
            surroundingTiles = gameInstance.CurrentLevel.GetSurroundingTiles(Position);

            foreach (Tile tile in surroundingTiles)
            {
                if (tile != null && tile.TileType == TileTypeEnum.Space)
                    return true;
            }

            return false;
        }
    }
}