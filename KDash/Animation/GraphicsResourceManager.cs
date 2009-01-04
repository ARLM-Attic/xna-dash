#region

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace XNADash.Animation
{
    internal class GraphicsResourceManager
    {
        #region GraphicsEnum enum

        public enum GraphicsEnum
        {
            Butterfly,
            Diamond,
            Dirt,
            Exit,
            Firefly,
            MagicWall,
            Player,
            Rock,
            Scoreboard,
            Space,
            Titanium,
            Wall
        }

        #endregion

        private static GraphicsResourceManager graphicsManager;
        public ContentManager contentManager;

        private GraphicsResourceManager()
        {
        }

        public static GraphicsResourceManager Instance
        {
            get
            {
                if (graphicsManager == null)
                    graphicsManager = new GraphicsResourceManager();

                return graphicsManager;
            }
        }

        public Texture2D LoadTexture(GraphicsEnum texture)
        {
            Texture2D result = null;

            switch (texture)
            {
                case GraphicsEnum.Butterfly:
                    result = contentManager.Load<Texture2D>("butterfly");
                    break;
                case GraphicsEnum.Diamond:
                    result = contentManager.Load<Texture2D>("diamond");
                    break;
                case GraphicsEnum.Dirt:
                    result = contentManager.Load<Texture2D>("dirt");
                    break;
                case GraphicsEnum.Exit:
                    result = contentManager.Load<Texture2D>("exit");
                    break;
                case GraphicsEnum.Firefly:
                    result = contentManager.Load<Texture2D>("firefly");
                    break;
                case GraphicsEnum.MagicWall:
                    result = contentManager.Load<Texture2D>("magicwall");
                    break;
                case GraphicsEnum.Player:
                    result = contentManager.Load<Texture2D>("player2");
                    break;
                case GraphicsEnum.Rock:
                    result = contentManager.Load<Texture2D>("rock");
                    break;
                case GraphicsEnum.Scoreboard:
                    result = contentManager.Load<Texture2D>("scoreboard");
                    break;
                case GraphicsEnum.Space:
                    result = contentManager.Load<Texture2D>("space");
                    break;
                case GraphicsEnum.Titanium:
                    result = contentManager.Load<Texture2D>("titanium");
                    break;
                case GraphicsEnum.Wall:
                    result = contentManager.Load<Texture2D>("wall");
                    break;
            }

            return result;
        }
    }
}