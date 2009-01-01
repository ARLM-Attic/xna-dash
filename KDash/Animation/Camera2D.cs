#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace XNADash.Animation
{
    public class Camera2D
    {
        private readonly SpriteBatch spriteRenderer;
        private Vector2 cameraPosition;
        public bool HasMoved;
        public float Speed = 5.0f;
        public int worldHeight;
        public int worldWidth;

        public Camera2D(SpriteBatch renderer)
        {
            spriteRenderer = renderer;
            cameraPosition = new Vector2(0, 0);
        }

        /// <summary>
        /// Returns the top left position of the camera
        /// </summary>
        public Vector2 Position
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }

        /// <summary>
        /// Draws the specified scene node
        /// </summary>
        /// <param name="node">The scene element</param>
        public void DrawNode(Scene2DNode node)
        {
            // get the screen position of the node
            Vector2 drawPosition = ApplyTransformations(node.Position);
            node.Draw(spriteRenderer, drawPosition);
        }

        /// <summary>
        /// Draws an entire scene
        /// </summary>
        /// <param name="sceneList">A list of elements to be drawn</param>
        public void DrawScene(List<Scene2DNode> sceneList)
        {
            spriteRenderer.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None);

            foreach (Scene2DNode node in sceneList)
            {
                DrawNode(node);
            }

            spriteRenderer.End();
        }

        private Vector2 ApplyTransformations(Vector2 nodePosition)
        {
            // apply translation
            Vector2 finalPosition = nodePosition - cameraPosition;
            // you can apply scaling and rotation here also
            //.....
            //--------------------------------------------
            return finalPosition;
        }

        /// <summary>
        /// Moves the camera to a specified world position
        /// </summary>
        /// <param name="moveVector">The position in world coordnates</param>
        public void Translate(Vector2 moveVector)
        {
            cameraPosition += moveVector;
            CheckCameraBounds();
        }

        /// <summary>
        /// Moves the camera using the up, down, left, right keys on the keyboard
        /// </summary>
        public void MoveCamera()
        {
            // make keyboard move the camera
            KeyboardState ks = Keyboard.GetState();
            Keys[] keys = ks.GetPressedKeys();

            foreach (Keys key in keys)
            {
                switch (key)
                {
                    case Keys.Up:
                        Translate(new Vector2(0, -Speed));
                        break;
                    case Keys.Right:
                        Translate(new Vector2(Speed, 0));
                        break;
                    case Keys.Down:
                        Translate(new Vector2(0, Speed));
                        break;
                    case Keys.Left:
                        Translate(new Vector2(-Speed, 0));
                        break;
                }
            }
        }

        /// <summary>
        /// Performs a check to make sure we are not going beyond the game world
        /// </summary>
        private void CheckCameraBounds()
        {
            if (cameraPosition.X > worldWidth - spriteRenderer.GraphicsDevice.Viewport.Width)
                cameraPosition.X = worldWidth - spriteRenderer.GraphicsDevice.Viewport.Width;
            if (cameraPosition.X < 0)
                cameraPosition.X = 0;
            if (cameraPosition.Y > worldHeight - spriteRenderer.GraphicsDevice.Viewport.Height)
                cameraPosition.Y = worldHeight - spriteRenderer.GraphicsDevice.Viewport.Height;
            if (cameraPosition.Y < 0)
                cameraPosition.Y = 0;
        }

        /// <summary>
        /// Centers the camera on the specified position
        /// </summary>
        /// <param name="screenPosition">Position to center camera on</param>
        public void CenterAt(Vector2 screenPosition)
        {
            Vector2 tempPos = screenPosition - Position;
            //tempPos.X = screenPosition.X - spriteRenderer.GraphicsDevice.Viewport.Width/2 + screenPosition.X/2;
            //tempPos.Y = screenPosition.Y - spriteRenderer.GraphicsDevice.Viewport.Height / 2 + screenPosition.Y / 2;
            tempPos.X = screenPosition.X - (spriteRenderer.GraphicsDevice.Viewport.Width/2);
            tempPos.Y = screenPosition.Y - (spriteRenderer.GraphicsDevice.Viewport.Height/2);

            // Check for pan left
            if (tempPos.X < cameraPosition.X)
                Translate(new Vector2(-Speed, 0));
            // Check for pan right
            if (tempPos.X > cameraPosition.X)
                Translate(new Vector2(Speed, 0));
            // Check for pan up
            if (tempPos.Y < cameraPosition.Y)
                Translate(new Vector2(0, -Speed));
            // Check for pan down
            if (tempPos.Y > cameraPosition.Y)
                Translate(new Vector2(0, Speed));
        }
    }
}