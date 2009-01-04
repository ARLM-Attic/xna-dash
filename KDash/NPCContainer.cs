using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNADash.Animation;
using XNADash.Level;
using XNADash.Sprites;

namespace XNADash
{
    /// <summary>
    /// This class can hold any number of NPCs
    /// </summary>
    public class NPCContainer
    {
        private List<EnemySprite> npcList;

        /// <summary>
        /// Constructor
        /// </summary>
        public NPCContainer()
        {
            NPCList = new List<EnemySprite>();
        }

        public List<EnemySprite> NPCList
        {
            get { return npcList; }
            set { npcList = value; }
        }

        /// <summary>
        /// Adds a single NPC to the list of NPCs in the gameworld
        /// </summary>
        /// <param name="npc">An NPC</param>
        public void AddNPC(EnemySprite npc)
        {
            if (!NPCList.Contains(npc))
            {
                NPCList.Add(npc);
            }
        }

        /// <summary>
        /// Updates all the NPCs in the list to make sure they move correctly
        /// </summary>
        public void Update(GameTime time)
        {
            foreach (EnemySprite sprite in NPCList)
            {
                // Check if we are moving or need to find a new place to go to
                if (!sprite.currentMovement.IsMoving())
                    sprite.NextMove();

                sprite.Move(time);
            }            
        }

        /// <summary>
        /// Adds all the NPCs in the game world to the current scene
        /// </summary>
        /// <returns>A list of NPCs to draw</returns>
        public List<Scene2DNode> AddToScene()
        {
            List<Scene2DNode> returnValue = new List<Scene2DNode>();

            //TODO: Add logic to perform culling so we don't have to draw them all
            foreach (EnemySprite sprite in NPCList)
            {
                returnValue.Add(sprite.ToSceneGraphNode());
            }

            return returnValue;
        }
    }
}
