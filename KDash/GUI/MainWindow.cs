using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TomShane.Neoforce.Controls;
using XNADash.SoundFx;

namespace XNADash.GUI
{
    class MainWindow : Window
    {
        private Button startGameButton;
        private Button highScoreButton;
        private Button optionsButton;
        private Button exitButton;
        private Manager GUIManager;
        private ImageBox background;

        public MainWindow(Manager manager) : base(manager)
        {
            GUIManager = manager;
            initializeGUI();
        }

        private void initializeGUI()
        {
            // Setup window
            Init();
            Text = "XNA Dash - Main menu";
            Width = 1024;
            Height = 768;
            Center();
            Resizable = false;
            Visible = true;

            background = new ImageBox(GUIManager);
            background.Init();
            background.Width = Width;
            background.Height = Height;
            background.Anchor = Anchors.All;
            background.Top = 0;
            background.Left = 0;
            background.Image = Manager.Content.Load<Texture2D>("Content\\GUI\\MainWindowBackgound");
            Add(background);

            // Setup start game button
            startGameButton = new Button(GUIManager);
            startGameButton.Init();
            startGameButton.Height = 65;
            startGameButton.Width = 365;
            startGameButton.Text = "Start Game";
            startGameButton.Left = (Width - startGameButton.Width) / 2;
            startGameButton.Top = 200;
            startGameButton.Click += new MouseEventHandler(startGameButton_Click);
            Add(startGameButton);

            // Setup highscore button
            highScoreButton = new Button(GUIManager);
            highScoreButton.Init();
            highScoreButton.Height = 65;
            highScoreButton.Width = 365;
            highScoreButton.Text = "Highscore";
            highScoreButton.Left = (Width - highScoreButton.Width) / 2;
            highScoreButton.Top = 280;
            highScoreButton.Click += new MouseEventHandler(highScoreButton_Click);
            Add(highScoreButton);

            // Setup options button
            optionsButton = new Button(GUIManager);
            optionsButton.Init();
            optionsButton.Height = 65;
            optionsButton.Width = 365;
            optionsButton.Text = "Options";
            optionsButton.Left = (Width - optionsButton.Width) / 2;
            optionsButton.Top = 360;
            optionsButton.Click += new MouseEventHandler(optionsButton_Click);
            Add(optionsButton);

            // Setup exit button
            exitButton = new Button(GUIManager);
            exitButton.Init();
            exitButton.Height = 65;
            exitButton.Width = 365;
            exitButton.Text = "Exit";
            exitButton.Left = (Width - exitButton.Width) / 2;
            exitButton.Top = 440;
            exitButton.Click += new MouseEventHandler(exitButton_Click);
            Add(exitButton);

            GUIManager.Add(this);
        }

        void optionsButton_Click(object sender, MouseEventArgs e)
        {
            OptionsWindow options = new OptionsWindow(GUIManager);
            options.Parent = this;
            options.Show();
        }

        void startGameButton_Click(object sender, MouseEventArgs e)
        {
            Hide();
            SoundFxManager.Instance.PlaySound(SoundFxManager.CueEnums.start);
            GameStateManager.CurrentGameState = GameStateManager.GameState.Game;
        }

        void highScoreButton_Click(object sender, MouseEventArgs e)
        {
            HighscoreWindow highscore = new HighscoreWindow(GUIManager);
            highscore.Parent = this;
            highscore.BringToFront();
            highscore.Show();
        }

        void exitButton_Click(object sender, MouseEventArgs e)
        {
            ExitWindow exit = new ExitWindow(GUIManager);
            exit.Parent = this;
            exit.BringToFront();
            exit.Show();
        }
    }
}