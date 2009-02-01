using System;
using System.Collections.Generic;
using System.Text;
using TomShane.Neoforce.Controls;

namespace XNADash.GUI
{
    class OptionsWindow : Window
    {
        private Manager GUIManager;
        private Button closeButton;
        private TabControl tab;
        private Label graphicsLabel;

        public OptionsWindow(Manager manager) : base(manager)
        {
            GUIManager = manager;
            InitControls();
        }

        private void InitControls()
        {
            // Setup window
            Init();
            Text = "XNA Dash - Options";
            Width = 600;
            Height = 400;
            Center();
            Resizable = false;
            Visible = true;

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

            // Setup tab
            tab = new TabControl(GUIManager);
            tab.Init();
            tab.Anchor = Anchors.Top;
            tab.Height = Height - 20;
            tab.Width = Width - 8;
            tab.AddPage();
            tab.TabPages[0].Text = "Graphics";
            tab.AddPage();
            tab.TabPages[1].Text = "Sound";
            tab.AddPage();
            tab.TabPages[2].Text = "About";

            graphicsLabel = new Label(GUIManager);
            graphicsLabel.Init();
            graphicsLabel.Text = "Graphics settings";
            graphicsLabel.Width = 100;
            tab.TabPages[0].Add(graphicsLabel);

            Add(tab);
        }

        void closeButton_Click(object sender, MouseEventArgs e)
        {
            Hide();
            Dispose();
        }
    }
}