#region

using System;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace XNADash.Level
{
    /// <summary>
    /// The TileFactory can create a <see cref="Tile"/> object based on the input provided in
    /// the <see cref="CreateTile"/> method.<br/>. Please look at <see cref="CreateTile"/>
    /// for more information on what types of tiles can be created.<p/>
    /// </summary>
    public class TileFactory
    {
        private readonly XNADash gameInstance;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">Instance of the main game class</param>
        public TileFactory(XNADash game)
        {
            gameInstance = game;
        }

        /// <summary>
        /// Creates a <see cref="Tile"/> object if possible.
        /// Possible values are:<br/>
        /// R = Rock<br/>
        /// D = Diamond<br/>
        /// E = Earth<br/>
        /// S = Space<br/>
        /// X = Exit<br/>
        /// M = Magic wall<br/>
        /// T = Titanium wall<br/>
        /// W = Wall<br/>
        /// </summary>
        /// <param name="tileId">R = Rock, D = Diamond, E = Dirt, S = Empty Space, X = Exit, M = Magic wall, T = Titanium wall, W = Wall</param>
        /// <returns>The specified tile</returns>
        public Tile CreateTile(char tileId)
        {
            Tile result = new Tile();

            switch (tileId)
            {
                case 'R':
                    result.TileType = TileTypeEnum.Rock;
                    break;
                case 'D':
                    result.TileType = TileTypeEnum.Diamond;
                    break;
                case 'E':
                    result.TileType = TileTypeEnum.Earth;
                    break;
                case 'S':
                    result.TileType = TileTypeEnum.Space;
                    break;
                case 'X':
                    result.TileType = TileTypeEnum.Exit;
                    break;
                case 'M':
                    result.TileType = TileTypeEnum.MagicWall;
                    break;
                case 'T':
                    result.TileType = TileTypeEnum.TitaniumWall;
                    break;
                case 'W':
                    result.TileType = TileTypeEnum.Wall;
                    break;
                default:
                    throw new Exception("Unknown tile type, can not build tile of type '" + tileId + "'");
            }

            ApplyTexture(result);

            return result;
        }

        /// <summary>
        /// Applies the correct texture to the tile
        /// </summary>
        /// <param name="tile">A tile that needs a texture</param>
        private void ApplyTexture(Tile tile)
        {
            switch (tile.TileType)
            {
                case TileTypeEnum.Rock:
                    tile.Texture = gameInstance.Content.Load<Texture2D>("rock");
                    break;
                case TileTypeEnum.Diamond:
                    tile.Texture = gameInstance.Content.Load<Texture2D>("diamond");
                    break;
                case TileTypeEnum.Earth:
                    tile.Texture = gameInstance.Content.Load<Texture2D>("dirt");
                    break;
                case TileTypeEnum.Space:
                    tile.Texture = gameInstance.Content.Load<Texture2D>("space");
                    break;
                case TileTypeEnum.Exit:
                    tile.Texture = gameInstance.Content.Load<Texture2D>("exit");
                    break;
                case TileTypeEnum.MagicWall:
                    tile.Texture = gameInstance.Content.Load<Texture2D>("magicwall");
                    break;
                case TileTypeEnum.TitaniumWall:
                    tile.Texture = gameInstance.Content.Load<Texture2D>("titanium");
                    break;
                case TileTypeEnum.Wall:
                    tile.Texture = gameInstance.Content.Load<Texture2D>("wall");
                    break;
            }
        }
    }
}