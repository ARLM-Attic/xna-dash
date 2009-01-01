#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace XNADash.Animation
{
    public class SceneGraph
    {
        private readonly SpriteBatch batch;
        private readonly List<Scene2DNode> currentSceneList;
        private readonly XNADash game;
        private readonly List<string> outputTextList;

        /// <summary>
        /// Overloaded, please use other constructor.
        /// </summary>
        public SceneGraph()
        {
            throw new ArgumentException("Do not use this constructor, use SceneGraph(XNADash, SpriteBatch) instead.");
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="gameReference">Reference to the main game</param>
        /// <param name="sb">The sprite batch</param>
        public SceneGraph(XNADash gameReference, SpriteBatch sb)
        {
            currentSceneList = new List<Scene2DNode>();
            game = gameReference;
            batch = sb;
            outputTextList = new List<string>();
        }

        /// <summary>
        /// Adds a node to the scene graph
        /// </summary>
        /// <param name="node">The node to add</param>
        public void AddToScene(Scene2DNode node)
        {
            if (node != null)
                currentSceneList.Add(node);
        }

        /// <summary>
        /// Initializes the object to draw a new scene
        /// </summary>
        public void NewScene()
        {
            currentSceneList.Clear();
            outputTextList.Clear();
        }

        /// <summary>
        /// Adds more then more nodes to the scene at once.
        /// </summary>
        /// <param name="nodes">A list of Scene2DNodes</param>
        public void AddToScene(List<Scene2DNode> nodes)
        {
            if (nodes != null)
                currentSceneList.AddRange(nodes);
        }

        public void AddText(string text)
        {
            outputTextList.Add(text);
        }

        /// <summary>
        /// Draws the current scene iterating though the <see cref="currentSceneList"/>
        /// </summary>
        public void Draw()
        {
            batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

            // Draw game graphics
            foreach (Scene2DNode node in currentSceneList)
            {
                game.camera.DrawNode(node);
            }

            int yTextOffset = 0;
            // Write text
            foreach (string currentText in outputTextList)
            {
                if (!string.IsNullOrEmpty(currentText))
                {
                    batch.DrawString(game.font, currentText, new Vector2(0, yTextOffset), Color.White);
                    yTextOffset += 10;
                }
            }

            batch.End();
        }
    }
}