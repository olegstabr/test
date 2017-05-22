using System;
using System.Drawing;
using System.Windows.Forms;

namespace RGR_IS_
{
    public partial class MainForm : Form
    {
        CatalogForm catalogForm = new CatalogForm();

        Button catalogButton = new Button();
        Button publisherButton = new Button();
        Button readerButton = new Button();
        Button refButton = new Button();
        Button extraditionButton = new Button();
        Button exitButton = new Button();

        private const int MARGIN = 10;
        public MainForm()
        {
            Text = "РГР: Вариант №14";
            StartPosition = FormStartPosition.CenterScreen;
            BackgroundImage = new Bitmap(Properties.Resources.mainback);
            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(500, 530);

            catalogForm.ParentForm = this;

            InitUI();
        }

        void InitUI()
        {
            Size buttonSize = new Size((int)(Width / 1.8), (int)(Height / 7.07)); // 266x75 #7a5757

            int x = (ClientSize.Width - buttonSize.Width) / 2;
            int y = (ClientSize.Height - 6 * (buttonSize.Height + MARGIN)) / 2;

            catalogButton.Size = buttonSize;
            catalogButton.Location = new Point(x, y);
            catalogButton.BackgroundImage = new Bitmap(Properties.Resources.catalog);
            catalogButton.Click += OnCatalogButtonClick;

            publisherButton.Size = buttonSize;
            publisherButton.Location = new Point(x, y += buttonSize.Height + MARGIN);
            publisherButton.BackgroundImage = new Bitmap(Properties.Resources.publisher);
            publisherButton.Click += OnPublisherButtonClick;

            readerButton.Size = buttonSize;
            readerButton.Location = new Point(x, y += buttonSize.Height + MARGIN);
            readerButton.BackgroundImage = new Bitmap(Properties.Resources.reader);
            readerButton.Click += OnReaderButtonClick;

            extraditionButton.Size = buttonSize;
            extraditionButton.Location = new Point(x, y += buttonSize.Height + MARGIN);
            extraditionButton.BackgroundImage = new Bitmap(Properties.Resources.extradition);
            extraditionButton.Click += OnExtraditionButtonClick;

            refButton.Size = buttonSize;
            refButton.Location = new Point(x, y += buttonSize.Height + MARGIN);
            refButton.BackgroundImage = new Bitmap(Properties.Resources._ref);
            refButton.Click += OnRefButtonClick;

            exitButton.Size = buttonSize;
            exitButton.Location = new Point(x, y += buttonSize.Height + MARGIN);
            exitButton.BackgroundImage = new Bitmap(Properties.Resources.exit);
            exitButton.Click += OnExitButtonClick;
            
            Controls.Add(catalogButton);
            Controls.Add(publisherButton);
            Controls.Add(readerButton);
            Controls.Add(extraditionButton);
            Controls.Add(refButton);
            Controls.Add(exitButton);
        }

        private void OnExitButtonClick(object sender, EventArgs eventArgs)
        {
            Close();
        }

        void OnRefButtonClick(object sender, EventArgs eventArgs)
        {
            RefForm refForm = new RefForm(this);
            refForm.Show();
            Hide();
        }

        void OnCatalogButtonClick(object sender, EventArgs eventArgs)
        {
            catalogForm.SelectedTab = 0;
            catalogForm.Show();
            Hide();
        }

        void OnPublisherButtonClick(object sender, EventArgs eventArgs)
        {
            catalogForm.SelectedTab = 1;
            catalogForm.Show();
            Hide();
        }

        void OnReaderButtonClick(object sender, EventArgs eventArgs)
        {
            catalogForm.SelectedTab = 2;
            catalogForm.Show();
            Hide();
        }

        void OnExtraditionButtonClick(object sender, EventArgs eventArgs)
        {
            catalogForm.SelectedTab = 3;
            catalogForm.Show();
            Hide();
        }
    }
}
