using System;
using System.Collections.Generic;
using System.Text;

namespace XNADash
{
    class GameStateManager
    {
        public enum GameState
        {
            Menu,
            Game
        }

        private static GameStateManager instance;
        private static GameState currentGameState;

        private GameStateManager()
        {
            
        }

        public static GameState CurrentGameState
        {
            get
            {
                
                    return currentGameState;

            }
            set { currentGameState = value; }
        }

        public static GameStateManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameStateManager();
                currentGameState = GameState.Menu;
            }

            return instance;
        }
    }
}
