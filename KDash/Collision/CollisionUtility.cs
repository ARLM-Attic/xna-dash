#region

using Microsoft.Xna.Framework;
using XNADash.Animation;
using XNADash.Level;
using XNADash.SoundFx;
using XNADash.Sprites;

#endregion

namespace XNADash.Collision
{
    public class CollisionUtility
    {
        /// <summary>
        /// Checks if rectangle A intersects rectangle B
        /// </summary>
        /// <param name="a">Rectangle A</param>
        /// <param name="b">Rectangle B</param>
        /// <returns>True is they intersect, false if not</returns>
        public static bool Intersects(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect
            return (a.Right > b.Left && a.Left < b.Right &&
                    a.Bottom > b.Top && a.Top < b.Bottom);
        }

        public static bool Touches(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect or touch sides
            return (a.Right >= b.Left && a.Left <= b.Right &&
                    a.Bottom >= b.Top && a.Top <= b.Bottom);
        }

        public static void ResolvePlayerTileCollision(MovingSprite player, Tile tile)
        {
            if (tile.TileType == TileTypeEnum.MagicWall || tile.TileType == TileTypeEnum.Rock ||
                tile.TileType == TileTypeEnum.TitaniumWall || tile.TileType == TileTypeEnum.Wall)
            {
                // If we collide with a tile we should be "pushed back" to 
                // a place as close as possible to the tile.
                // This is done by getting the tile position and calculate
                // where the player should be relative to it.
                if (player.currentMovement.XDirection == MovementVector.DirectionX.Left)
                {
                    player.Position.X = tile.Position.X + tile.Width;
                    player.MoveStandStill();
                }
                if (player.currentMovement.XDirection == MovementVector.DirectionX.Right)
                {
                    player.Position.X = tile.Position.X - tile.Width;
                    player.MoveStandStill();
                }
                if (player.currentMovement.YDirection == MovementVector.DirectionY.Up)
                {
                    player.Position.Y = tile.Position.Y + tile.Height;
                    player.MoveStandStill();
                }
                if (player.currentMovement.YDirection == MovementVector.DirectionY.Down)
                {
                    player.Position.Y = tile.Position.Y - tile.Height;
                    player.MoveStandStill();
                }

                // Player bumped into something, play the bump cue
                SoundFxManager.Instance.PlaySound(SoundFxManager.CueEnums.bump);
            }

            if (tile.TileType == TileTypeEnum.Diamond)
            {
                tile.TileType = TileTypeEnum.Space;
                tile.Texture = GraphicsResourceManager.Instance.LoadTexture(GraphicsResourceManager.GraphicsEnum.Space);
                player.FireDiamondCollectedEvent();
                
                SoundFxManager.Instance.PlaySound(SoundFxManager.CueEnums.diamond);
            }

            if (tile.TileType == TileTypeEnum.Earth)
            {
                tile.TileType = TileTypeEnum.Space;
                tile.Texture = GraphicsResourceManager.Instance.LoadTexture(GraphicsResourceManager.GraphicsEnum.Space);
                SoundFxManager.Instance.PlaySound(SoundFxManager.CueEnums.move);
            }

            //TODO: Implement level complete
            if (tile.TileType == TileTypeEnum.Exit)
            {
                tile.FireTileCollisionEvent(tile.TileType);
            }
        }

        /// <summary>
        /// Performs what need to be done if the player touches another NPC
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="npc">The NPC</param>
        public static void ResolvePlayerNpcCollision(MovingSprite player, EnemySprite npc)
        {
            if (Intersects(player.bounds, npc.bounds))
            {
                // Player bumped into something, play the bump cue
                SoundFxManager.Instance.PlaySound(SoundFxManager.CueEnums.exit);
            }
        }

        public static void ResolveNPCTileCollision(EnemySprite enemy, Tile tile)
        {
            if (tile.TileType != TileTypeEnum.Space)
            {
                if (enemy.currentMovement.XDirection == MovementVector.DirectionX.Left)
                {
                    enemy.MoveStandStill();
                    enemy.Position.X = tile.Position.X + tile.Width;
                }
                if (enemy.currentMovement.XDirection == MovementVector.DirectionX.Right)
                {
                    enemy.MoveStandStill();
                    enemy.Position.X = tile.Position.X - tile.Width;
                }
                if (enemy.currentMovement.YDirection == MovementVector.DirectionY.Up)
                {
                    enemy.MoveStandStill();
                    enemy.Position.Y = tile.Position.Y + tile.Height;
                }
                if (enemy.currentMovement.YDirection == MovementVector.DirectionY.Down)
                {
                    enemy.MoveStandStill();
                    enemy.Position.Y = tile.Position.Y - tile.Height;
                }
            }
        }
    }
}