using System;
using System.Collections.Generic;
using System.Text;
using TomShane.Neoforce.Controls;

namespace XNADash.GUI
{
    class HighscoreWindow : Window
    {
        private Manager GUIManager;
        private Button closeButton;

        public HighscoreWindow(Manager manager) : base(manager)
        {
            GUIManager = manager;
            InitControls();
        }

        private void InitControls()
        {
            // Setup window
            Init();
            Text = "XNA Dash - Highscore";
            Width = 600;
            Height = 400;
            Center();
            Resizable = false;
            IconVisible = true;
            Visible = true;

            // Setup listbox
            ListBox list = new ListBox(GUIManager);
            list.Init();
            list.Height = 250;
            list.Width = 500;
            list.Top = 50;
            list.Left = (Width - list.Width) / 2;

            for (int i = 1; i < 11; i++)
                list.Items.Add(i + ". ");
            
            Add(list);

            // Setup close button
            closeButton = new Button(GUIManager);
            closeButton.Init();
            closeButton.Height = 30;
            closeButton.Width = 50;
            closeButton.Text = "Close";
            closeButton.Left = (Width - closeButton.Width) - 20;
            closeButton.Top = (Height - closeButton.Width) - 20;
            closeButton.ModalResult = ModalResult.Ok;
            closeButton.Click += new MouseEventHandler(closeButton_Click);
            Add(closeButton);
        }

        void closeButton_Click(object sender, MouseEventArgs e)
        {
            Hide();
            Dispose();
        }
    }
}