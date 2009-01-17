#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNADash.Animation;
using XNADash.Sprites;

#endregion

namespace XNADash.Level
{
    public class Level
    {
        private readonly TileFactory factory;
        public readonly XNADash game;
        private readonly NPCContainer npcContainer;
        private int cellHeight;
        private int cellWidth;
        private int diamondsToCollect;
        private int finishTime;
        private int height;
        private string levelName;
        private Vector2 startPosition;
        private Tile[,] tileGrid;
        private int width;
        public int WorldHeight;
        public int WorldWidth;

        /// <summary>
        /// Constructor of the level
        /// </summary>
        /// <param name="gameInstance">Reference to the game</param>
        public Level(XNADash gameInstance)
        {
            game = gameInstance;
            factory = new TileFactory(game);
            npcContainer = new NPCContainer();
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// The level name
        /// </summary>
        public string LevelName
        {
            get { return levelName; }
        }

        /// <summary>
        /// Number of diamonds to collect in order to complete the level
        /// </summary>
        public int DiamondsToCollect
        {
            get { return diamondsToCollect; }
        }

        /// <summary>
        /// How many seconds does the player have to finish the level
        /// </summary>
        public int FinishTime
        {
            get { return finishTime; }
        }

        /// <summary>
        /// The height of a tile in the level
        /// </summary>
        public int CellHeight
        {
            get { return cellHeight; }
        }

        /// <summary>
        /// The width of a tile in the level
        /// </summary>
        public int CellWidth
        {
            get { return cellWidth; }
        }

        /// <summary>
        /// The position (in tile coordinates) where the player should start
        /// </summary>
        public Vector2 StartPosition
        {
            get { return startPosition; }
        }

        public NPCContainer NpcContainer
        {
            get { return npcContainer; }
        }

        /// <summary>
        /// Returns the <see cref="Tile"/> at the specified world position
        /// </summary>
        /// <param name="position">The world coordinate</param>
        /// <returns>The tile, but can be null if outside the level</returns>
        public Tile GetTile(ref Vector2 position)
        {
            return GetTile((int) position.X, (int) position.Y, ref tileGrid);
        }

        /// <summary>
        /// Returns the <see cref="Tile"/> at the specified world position
        /// </summary>
        /// <param name="x">The x world coordinate</param>
        /// <param name="y">The y world coordinate</param>
        /// <param name="level">Reference to the level</param>
        /// <returns>The tile, but can be null if outside the level</returns>
        private Tile GetTile(int x, int y, ref Tile[,] level)
        {
            Tile result = null;
            if (x > 0 && x < WorldWidth &&
                y > 0 && y < WorldHeight)
            {
                Vector2 co = ToTileCoordinate(new Vector2(x, y));
                result = level[(int) co.X, (int) co.Y];
            }

            return result;
        }

        /// <summary>
        /// Gets the tiles within the reactangle specified
        /// </summary>
        /// <param name="tileBounds">The bounds of the rectangle</param>
        /// <returns>A list of tiles in the bounds of the rectangle</returns>
        public List<Tile> GetTiles(Rectangle tileBounds)
        {
            List<Tile> result = new List<Tile>(4);

            Tile topleft = GetTile(tileBounds.X, tileBounds.Y, ref tileGrid);
            Tile topright = GetTile(tileBounds.X + CellWidth, tileBounds.Y, ref tileGrid);
            Tile buttomLeft = GetTile(tileBounds.X, tileBounds.Y + CellHeight, ref tileGrid);
            Tile buttomRight = GetTile(tileBounds.X + CellWidth, tileBounds.Y + CellHeight, ref tileGrid);

            result.Add(topleft);
            result.Add(topright);
            result.Add(buttomLeft);
            result.Add(buttomRight);

            return result;
        }

        /// <summary>
        /// Loads the level.
        /// </summary>
        /// <param name="relativeLevelPath">Name of the level.</param>
        public void LoadLevel(string relativeLevelPath)
        {
            int currentHeight = 0;

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                StreamReader sr = new StreamReader(relativeLevelPath);
                string line;

                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("name"))
                        SetLevelName(line);
                    else if (line.StartsWith("diamonds"))
                        SetNumberOfDiamondToCollect(line);
                    else if (line.StartsWith("time"))
                        SetTime(line);
                    else if (line.StartsWith("height"))
                        SetLevelHeight(line);
                    else if (line.StartsWith("width"))
                        SetLevelWidth(line);
                    else if (line.StartsWith("start"))
                        SetStartPosition(line);
                    else
                    {
                        ParseLevelDefinition(line, currentHeight++);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to load level " + relativeLevelPath + ". " + e.Message);
            }

            if (CellWidth == 0)
                cellWidth = tileGrid[0, 0].Width;
            if (CellHeight == 0)
                cellHeight = tileGrid[0, 0].Height;

            WorldWidth = cellWidth*width;
            WorldHeight = cellHeight*height;
        }

        /// <summary>
        /// Sets the players startposition on the level
        /// </summary>
        /// <param name="line">The level definition</param>
        private void SetStartPosition(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new Exception("Level player start position could not be set, level input was " + line);

            line = line.Replace("start=", "");
            string[] tempString = line.Split(',');
            float xTileCoordinate = (float) Convert.ToDecimal(tempString[0])*100;
            float yTileCoordinate = (float) Convert.ToDecimal(tempString[1])*100;
            startPosition = new Vector2(xTileCoordinate, yTileCoordinate);
        }

        /// <summary>
        /// Sets the width of the level
        /// </summary>
        /// <param name="line">The level definition</param>
        private void SetLevelWidth(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new Exception("Level width could not be set, level input was " + line);

            string tempString = line.Replace("width=", "");
            if (string.IsNullOrEmpty(tempString))
                throw new Exception("Unable to set level width, got " + tempString);

            int iWidth = Convert.ToInt16(tempString, CultureInfo.CurrentCulture);
            width = iWidth;
        }

        /// <summary>
        /// Sets the height of the level
        /// </summary>
        /// <param name="line">The level definition</param>
        private void SetLevelHeight(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new Exception("Level height could not be set, level input was " + line);

            string tempString = line.Replace("height=", "");
            if (string.IsNullOrEmpty(tempString))
                throw new Exception("Unable to set level height, got " + tempString);

            int iHeight = Convert.ToInt16(tempString);
            height = iHeight;
        }

        /// <summary>
        /// Parses the level definition by reading the level and sending it off to 
        /// the <see cref="TileFactory"/> and then insert it into the level
        /// </summary>
        /// <param name="line">The level definision</param>
        /// <param name="currentHeight">The current height</param>
        private void ParseLevelDefinition(string line, int currentHeight)
        {
            if (string.IsNullOrEmpty(line))
                throw new Exception("Tile could not be created, level input was " + line);

            // Check if the level has been initialized
            if (tileGrid == null)
            {
                tileGrid = new Tile[line.Length,height];
                width = line.Length;
            }

            // Create width index counter;
            int currentWidth = 0;
            foreach (char currentChar in line)
            {
                Tile newTile = factory.CreateTile(currentChar);
                if (tileGrid != null)
                {
                    newTile.Position = new Vector2(currentWidth*newTile.Texture.Width,
                                                   currentHeight*newTile.Texture.Height);
                    tileGrid[currentWidth, currentHeight] = newTile;
                }

                if (currentChar.Equals('B') || currentChar.Equals('F'))
                    AddEnemy(newTile.Position, currentChar);

                // Add another tile to the width 
                currentWidth++;
            }
        }

        private void AddEnemy(Vector2 position, char enemyType)
        {
            EnemySprite newEnemy;

            if (enemyType.Equals('B'))
                newEnemy = new EnemySprite(game, EnemySprite.EnemyEnum.Butterfly, position);
            else
                newEnemy = new EnemySprite(game, EnemySprite.EnemyEnum.Firefly, position);

            game.CurrentLevel.NpcContainer.AddNPC(newEnemy);
        }

        /// <summary>
        /// Sets how many seconds the player has to finish the level
        /// </summary>
        /// <param name="line">The level definition</param>
        private void SetTime(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new Exception("Time could not be set, level input was " + line);

            string tempString = line.Replace("time=", "");
            if (string.IsNullOrEmpty(tempString))
                throw new Exception("Unable to set time, got " + tempString);

            int time = Convert.ToInt16(tempString);
            finishTime = time;
        }

        /// <summary>
        /// Sets how many diamonds the player has to collect in order to finish the level
        /// </summary>
        /// <param name="line">The level definition</param>
        private void SetNumberOfDiamondToCollect(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new Exception("Number of diamonds to collect could not be set, level input was " + line);

            string tempString = line.Replace("diamonds=", "");
            if (string.IsNullOrEmpty(tempString))
                throw new Exception("Unable to set number of diamonds to collect, got " + tempString);

            int diamonds = Convert.ToInt16(tempString, CultureInfo.CurrentCulture);
            diamondsToCollect = diamonds;
        }

        /// <summary>
        /// Sets the level name
        /// </summary>
        /// <param name="line">The level definition entry for level name</param>
        private void SetLevelName(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new Exception("Level name could not be set, level input was " + line);

            string tempString = line.Replace("name=", "");
            levelName = tempString;
        }

        public bool SaveLevel()
        {
            return false;
        }

        public void Draw(SceneGraph scene, SpriteBatch batch, Color layerColor)
        {
            List<Scene2DNode> tileNodes = GetVisibleTiles();
            scene.AddToScene(tileNodes);
        }

        /// <summary>
        /// This function determines which tiles are visible on the screen,
        /// given the current camera position
        /// </summary>
        private List<Scene2DNode> GetVisibleTiles()
        {
            List<Scene2DNode> visibleTileList = new List<Scene2DNode>();
            Tile tileReference;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    tileReference = tileGrid[x, y];

                    if (tileReference != null)
                    {
                        if (tileReference.Position.X >= game.camera.Position.X - tileReference.Width &&
                            tileReference.Position.X <= game.camera.Position.X + game.GraphicsDevice.Viewport.Width &&
                            tileReference.Position.Y >= game.camera.Position.Y - tileReference.Height &&
                            tileReference.Position.Y <= game.camera.Position.Y + game.GraphicsDevice.Viewport.Height)
                        {
                            Scene2DNode node = new Scene2DNode(tileReference.Texture, tileReference.Position);
                            visibleTileList.Add(node);
                        }
                    }
                }
            }

            return visibleTileList;
        }

        /// <summary>
        /// Concerts the position to a tile position
        /// </summary>
        /// <param name="worldPosition">The world position</param>
        /// <returns>The tile position in the level map</returns>
        public Vector2 ToTileCoordinate(Vector2 worldPosition)
        {
            Vector2 result = worldPosition;
            result.X = result.X/CellWidth;
            result.Y = result.Y/CellHeight;

            return result;
        }

        /// <summary>
        /// Returns all the tiles surrounding the tile specified.
        /// <remarks>
        /// The tiles found is in this order:
        ///  0
        /// 1x2
        ///  3
        /// Where x is the position specified
        /// </remarks>
        /// </summary>
        /// <param name="position">The initial position</param>
        /// <returns>A list of 4 tiles</returns>
        public Tile[] GetSurroundingTiles(Vector2 position)
        {
            Tile[] result = new Tile[4];

            result[0] = GetTile((int) position.X, (int) position.Y - cellHeight, ref tileGrid);
            result[1] = GetTile((int) position.X - cellWidth, (int) position.Y, ref tileGrid);
            result[2] = GetTile((int) position.X + cellWidth, (int) position.Y + cellHeight, ref tileGrid);
            result[3] = GetTile((int) position.X, (int) position.Y + cellHeight, ref tileGrid);

            return result;
        }

        /// <summary>
        /// Sets the collision flag to false. Used when a scene is about to be drawn
        /// </summary>
        public void ResetCollisionDebugInfo()
        {
            foreach (Tile tile in tileGrid)
            {
                tile.Collision = false;
            }
        }
    }
}