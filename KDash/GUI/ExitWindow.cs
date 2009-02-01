using TomShane.Neoforce.Controls;

namespace XNADash.GUI
{
    class ExitWindow : Dialog
    {
        private Manager GUIManager;
        private Button okButton;
        private Button cancelButton;

        public ExitWindow(Manager manager) : base(manager)
        {
            GUIManager = manager;
            InitControls();
        }

        private void InitControls()
        {
            // Setup window
            Init();
            Text = "XNA Dash - Exit?";
            Width = 300;
            Height = 200;
            Center();
            Caption.Text = "Do you really want to quit?";
            Description.Text = "";
            Resizable = false;
            IconVisible = true;
            Visible = true;

            // Setup ok button
            okButton = new Button(GUIManager);
            okButton.Init();
            okButton.Height = 30;
            okButton.Width = 50;
            okButton.Text = "Ok";
            okButton.Left = Width - okButton.Width - 80;
            okButton.Top = 80;
            okButton.ModalResult = ModalResult.Ok;
            okButton.Click += new MouseEventHandler(okButton_Click);
            Add(okButton);

            // Setup cancel button
            cancelButton = new Button(GUIManager);
            cancelButton.Init();
            cancelButton.Height = 30;
            cancelButton.Width = 50;
            cancelButton.Text = "Cancel";
            cancelButton.Left = Width - cancelButton.Width - 20;
            cancelButton.Top = 80;
            cancelButton.ModalResult = ModalResult.Cancel;
            cancelButton.Click += new MouseEventHandler(cancelButton_Click);
            Add(cancelButton);
        }

        void cancelButton_Click(object sender, MouseEventArgs e)
        {
            Hide();
            Dispose();
        }

        void okButton_Click(object sender, MouseEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}