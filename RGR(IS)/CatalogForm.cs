using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace RGR_IS_
{
    class CatalogForm : Form
    {
        public Form ParentForm
        {
            set
            {
                if (value == null) { return; }
                parentForm = value;
            }
        }

        public int SelectedTab
        {
            set { tabControl.SelectedIndex = value; }
        }

        Form parentForm;

#region UI Defenitions

        DataGridView booksDataGridView = new DataGridView();
        DataGridView publishersDataGridView = new DataGridView();
        DataGridView readersDataGridView = new DataGridView();
        DataGridView extraditionDataGridView = new DataGridView();

        DataSet booksDataSet = new DataSet();
        DataSet publishersDataSet = new DataSet();
        DataSet readersDataSet = new DataSet();
        DataSet extraditionDataSet = new DataSet();

        TabControl tabControl= new TabControl();
        TabPage catalogTabPage = new TabPage("Книги");
        TabPage publishersTabPage = new TabPage("Издатели");
        TabPage readersTabPage = new TabPage("Читатели");
        TabPage extraditionTabPage = new TabPage("Выдачи книг");

        GroupBox groupBox = new GroupBox();
        GroupBox publishersGroupBox = new GroupBox();
        GroupBox readersGroupBox = new GroupBox();
        GroupBox extraditionGroupBox = new GroupBox();

        MenuStrip menuStrip = new MenuStrip();
        ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("Файл");
        ToolStripMenuItem backupDbMenuItem = new ToolStripMenuItem("Создать копию БД");
        ToolStripMenuItem restoreDbMenuItem = new ToolStripMenuItem("Восстановить БД");
        ToolStripMenuItem aboutMenuItem = new ToolStripMenuItem("О программе");

        Button insertBookButton = new Button();
        Button editBookButton = new Button();
        Button saveBookButton = new Button();
        Button cancelBookButton = new Button();
        Button addBookButton = new Button();
        Button deleteBookButton = new Button();

        Button insertPublishersButton = new Button();
        Button editPublishersButton = new Button();
        Button savePublishersButton = new Button();
        Button cancelPublishersButton = new Button();
        Button addPublishersButton = new Button();
        Button deletePublishersButton = new Button();

        Button insertReadersButton = new Button();
        Button editReadersButton = new Button();
        Button saveReadersButton = new Button();
        Button cancelReadersButton = new Button();
        Button addReadersButton = new Button();
        Button deleteReadersButton = new Button();

        Button insertExtraditionButton = new Button();
        Button editExtraditionButton = new Button();
        Button saveExtraditionButton = new Button();
        Button cancelExtraditionButton = new Button();
        Button addExtraditionButton = new Button();
        Button deleteExtraditionButton = new Button();
        Button extendExtraditionButton = new Button();

        Label booksLabel = new Label();
        Label bookInfoLabel = new Label();
        Label nameLabel=new Label();
        Label authorLabel=new Label();
        Label publishLabel=new Label();
        Label yearLabel=new Label();
        Label themeNumberLabel=new Label();
        Label countLabel=new Label();
        Label attrLabel=new Label();

        Label publishersLabel = new Label();
        Label publishersInfoLabel = new Label();
        Label namePublishLabel = new Label();
        Label addressLabel = new Label();

        Label readersLabel = new Label();
        Label readersInfoLabel = new Label();
        Label surnameLabel = new Label();
        Label cityLabel = new Label();

        Label extraditionLabel = new Label();
        Label extraditionInfoLabel = new Label();
        Label cardExtraLabel = new Label();
        Label libNumberExtraLabel = new Label();
        Label dateLabel = new Label();

        TextBox titleTextBox = new TextBox();
        TextBox authorTextBox = new TextBox();
        ComboBox publishersComboBox = new ComboBox();
        TextBox yearTextBox = new TextBox();
        TextBox themeNumberTextBox = new TextBox();
        TextBox countTextBox = new TextBox();
        RadioButton readerClassRadioButton = new RadioButton();
        RadioButton homeRadioButton = new RadioButton();

        TextBox namePublishersTextBox = new TextBox();
        TextBox addressPublishersTextBox = new TextBox();
        
        TextBox surnameTextBox = new TextBox();
        TextBox cityTextBox = new TextBox();

        TextBox cardExtraTextBox = new TextBox();
        ComboBox cardExtraComboBox = new ComboBox();
        TextBox libNumberExtraTextBox = new TextBox();
        ComboBox libNumberExtraComboBox = new ComboBox();

        DateTimePicker dateTimePicker = new DateTimePicker();

#endregion

        const int MARGIN = 10;
        const int BUTTON_HEIGHT = 50;
        const int LABEL_HEIGHT = 30;

        bool isAddBookAction = false;
        bool isAddPublishersAction = false;
        bool isAddReadersAction = false;
        bool isAddExtraditionAction = false;

        Font font = new Font(FontFamily.GenericSerif, 14, FontStyle.Regular);
        enum CommandType { CreateDb, Restore, DeleteDb }
        string connectionString;

        public CatalogForm()
        {
            Text = "Каталог книг";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1280, 720);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            InitCatalogUI();
            InitPublishersUI();
            InitReadersUI();
            InitExtraditionUI();

            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            LoadTables();

            Closing += OnClosing;
        }

        void InitCatalogUI()
        {
            menuStrip.Location = new Point(0, 0);
            menuStrip.Items.Add(fileMenuItem);
            menuStrip.Items.Add(aboutMenuItem);

            fileMenuItem.DropDownItems.Add(backupDbMenuItem);
            fileMenuItem.DropDownItems.Add(restoreDbMenuItem);

            backupDbMenuItem.Click += OnBackupDbMenuItemClick;
            restoreDbMenuItem.Click += OnRestoreDbMenuItemClick;

            tabControl.Location = new Point(MARGIN, menuStrip.Height + MARGIN);
            tabControl.Size = new Size(ClientSize.Width - 2 * MARGIN, ClientSize.Height - 2 * MARGIN - menuStrip.Height);
            tabControl.Controls.Add(catalogTabPage);
            tabControl.Controls.Add(publishersTabPage);
            tabControl.Controls.Add(readersTabPage);
            tabControl.Controls.Add(extraditionTabPage);

            int x = MARGIN;
            int y = MARGIN;

            booksLabel.Text = "Данные о книгах:";
            booksLabel.Font = font;
            booksLabel.AutoSize = true;
            booksLabel.Location = new Point(x, y);

            booksDataGridView.Size = new Size(ClientSize.Width / 2, 2 * Height / 3);
            booksDataGridView.Location = new Point(x, y += booksLabel.Height + MARGIN / 2);
            booksDataGridView.BackgroundColor = Color.WhiteSmoke;
            booksDataGridView.AllowUserToAddRows = false;
            booksDataGridView.CellClick += OnBooksDataGridViewCellClick;

            bookInfoLabel.Text = "Информация о книге:";
            bookInfoLabel.Font = font;
            bookInfoLabel.AutoSize = true;
            bookInfoLabel.Location = new Point(booksDataGridView.Width + 4 * MARGIN, booksLabel.Location.Y);

            groupBox.Size = new Size( tabControl.Width - booksDataGridView.Width - 7 * MARGIN, 2 * Height / 3);
            groupBox.Location = new Point(booksDataGridView.Width + 4 * MARGIN, booksDataGridView.Location.Y);

            editBookButton.Text = "Изменить информацию";
            editBookButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            editBookButton.Location = new Point(groupBox.Location.X - 4 * MARGIN + (ClientSize.Width - groupBox.Width - editBookButton.Width) / 2, y += booksDataGridView.Height + MARGIN / 2);
            editBookButton.Visible = false;
            editBookButton.Click += OnEditBookButtonClick;

            Size labelSize = new Size(ClientSize.Width / 8, LABEL_HEIGHT);
            int groupX = MARGIN;
            int groupY = MARGIN;

            nameLabel = GetLabel("Название:", labelSize, new Point(groupY, groupY), font);
            authorLabel = GetLabel("Автор:", labelSize, new Point(groupX, groupY += labelSize.Height+MARGIN), font);
            publishLabel = GetLabel("Издательство:", labelSize, new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            yearLabel = GetLabel("Год:", labelSize, new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            themeNumberLabel = GetLabel("Шифр тематики:", labelSize, new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            countLabel = GetLabel("Количество:", labelSize, new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            attrLabel = GetLabel("Чит-ый зал:", labelSize, new Point(groupX, groupY += labelSize.Height + MARGIN), font);

            groupX = labelSize.Width + MARGIN;
            groupY = MARGIN;

            titleTextBox = GetTextBox(new Size(ClientSize.Width / 4, labelSize.Height), new Point(groupX, groupY), font);
            titleTextBox.ReadOnly = true;
            authorTextBox = GetTextBox(new Size(ClientSize.Width / 4, labelSize.Height), new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            authorTextBox.ReadOnly = true;

            publishersComboBox.Enabled = false;
            publishersComboBox.Size = labelSize;
            publishersComboBox.Location = new Point(groupX, groupY += labelSize.Height + MARGIN);

            yearTextBox = GetTextBox(new Size(ClientSize.Width / 4, labelSize.Height), new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            yearTextBox.ReadOnly = true;
            yearTextBox.KeyPress += OnTextBoxDigitKeyPress;
            themeNumberTextBox = GetTextBox(new Size(ClientSize.Width / 4, labelSize.Height), new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            themeNumberTextBox.ReadOnly = true;
            themeNumberTextBox.KeyPress += OnTextBoxDigitKeyPress;
            countTextBox = GetTextBox(new Size(ClientSize.Width / 4, labelSize.Height), new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            countTextBox.ReadOnly = true;
            countTextBox.KeyPress += OnTextBoxDigitKeyPress;
            readerClassRadioButton.Size = new Size(labelSize.Width / 2, labelSize.Height);
            readerClassRadioButton.Location = new Point(groupX, groupY += labelSize.Height + MARGIN);
            readerClassRadioButton.Text = "Чит-ый зал";
            readerClassRadioButton.Enabled = false;
            homeRadioButton.Size = new Size(labelSize.Width / 2, labelSize.Height);
            homeRadioButton.Location = new Point(groupX += readerClassRadioButton.Width + MARGIN, groupY);
            homeRadioButton.Text = "На дом";
            homeRadioButton.Enabled = false;

            saveBookButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            saveBookButton.Location = new Point(groupX += homeRadioButton.Width + MARGIN, groupY);
            saveBookButton.Text = "Сохранить";
            saveBookButton.Visible = false;
            saveBookButton.Click += OnSaveBookButtonClick;

            insertBookButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            insertBookButton.Location = new Point(groupX, groupY);
            insertBookButton.Text = "Добавить";
            insertBookButton.Visible = false;
            insertBookButton.Click += OnInsertBookButtonClick;

            cancelBookButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            cancelBookButton.Location = new Point(groupX += saveBookButton.Width + MARGIN, groupY);
            cancelBookButton.Text = "Отмена";
            cancelBookButton.Visible = false;
            cancelBookButton.Click += OnCancelBookButtonClick;

            addBookButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            addBookButton.Location = new Point(x = MARGIN, y);
            addBookButton.Text = "Добавить книгу";
            addBookButton.Click += OnAddBookButtonClick;

            deleteBookButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            deleteBookButton.Location = new Point(x += addBookButton.Width + MARGIN, y);
            deleteBookButton.Text = "Удалить книгу";
            deleteBookButton.Visible = false;
            deleteBookButton.Click += OnDeleteBookButtonClick;

            groupBox.Controls.Add(nameLabel);
            groupBox.Controls.Add(authorLabel);
            groupBox.Controls.Add(publishLabel);
            groupBox.Controls.Add(yearLabel);
            groupBox.Controls.Add(themeNumberLabel);
            groupBox.Controls.Add(countLabel);
            groupBox.Controls.Add(attrLabel);
            groupBox.Controls.Add(titleTextBox);
            groupBox.Controls.Add(authorTextBox);
            groupBox.Controls.Add(publishersComboBox);
            groupBox.Controls.Add(yearTextBox);
            groupBox.Controls.Add(themeNumberTextBox);
            groupBox.Controls.Add(countTextBox);
            groupBox.Controls.Add(readerClassRadioButton);
            groupBox.Controls.Add(homeRadioButton);
            groupBox.Controls.Add(insertBookButton);
            groupBox.Controls.Add(saveBookButton);
            groupBox.Controls.Add(cancelBookButton);

            catalogTabPage.Controls.Add(booksDataGridView);
            catalogTabPage.Controls.Add(booksLabel);
            catalogTabPage.Controls.Add(editBookButton);
            catalogTabPage.Controls.Add(groupBox);
            catalogTabPage.Controls.Add(bookInfoLabel);
            catalogTabPage.Controls.Add(addBookButton);
            catalogTabPage.Controls.Add(deleteBookButton);

            Controls.Add(menuStrip);
            Controls.Add(tabControl);
        }

        void InitPublishersUI()
        {

            //
            // publishersTabPage
            //

            int x = MARGIN;
            int y = MARGIN;

            publishersLabel.Text = "Данные об издательствах:";
            publishersLabel.Font = font;
            publishersLabel.AutoSize = true;
            publishersLabel.Location = new Point(x, y);

            publishersDataGridView.Size = new Size(ClientSize.Width / 2, 2 * Height / 3);
            publishersDataGridView.Location = new Point(x, y += publishersLabel.Height + MARGIN / 2);
            publishersDataGridView.BackgroundColor = Color.WhiteSmoke;
            publishersDataGridView.AllowUserToAddRows = false;
            publishersDataGridView.CellClick += OnPublishersDataGridViewCellClick;

            publishersInfoLabel.Text = "Информация об издательстве:";
            publishersInfoLabel.Font = font;
            publishersInfoLabel.AutoSize = true;
            publishersInfoLabel.Location = new Point(publishersDataGridView.Width + 4 * MARGIN, publishersLabel.Location.Y);

            publishersGroupBox.Size = new Size(tabControl.Width - publishersDataGridView.Width - 7 * MARGIN, 2 * Height / 3);
            publishersGroupBox.Location = new Point(publishersDataGridView.Width + 4 * MARGIN, publishersDataGridView.Location.Y);

            editPublishersButton.Text = "Изменить информацию";
            editPublishersButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            editPublishersButton.Location = new Point(publishersGroupBox.Location.X - 4 * MARGIN + (ClientSize.Width - publishersGroupBox.Width - editPublishersButton.Width) / 2, y += publishersDataGridView.Height + MARGIN / 2);
            editPublishersButton.Click += OnEditPublishersButtonClick;
            editPublishersButton.Visible = false;

            Size labelSize = new Size(ClientSize.Width / 8, LABEL_HEIGHT);
            int groupX = MARGIN;
            int groupY = MARGIN;

            namePublishLabel = GetLabel("Название:", labelSize, new Point(groupY, groupY), font);
            addressLabel = GetLabel("Адрес:", labelSize, new Point(groupX, groupY += labelSize.Height + MARGIN), font);

            groupX = labelSize.Width + MARGIN;
            groupY = MARGIN;

            namePublishersTextBox = GetTextBox(new Size(ClientSize.Width / 4, labelSize.Height), new Point(groupX, groupY), font);
            namePublishersTextBox.ReadOnly = true;
            addressPublishersTextBox = GetTextBox(new Size(ClientSize.Width / 4, labelSize.Height), new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            addressPublishersTextBox.ReadOnly = true;

            savePublishersButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            savePublishersButton.Location = new Point(groupX, groupY += addressPublishersTextBox.Height + MARGIN);
            savePublishersButton.Text = "Сохранить";
            savePublishersButton.Visible = false;
            savePublishersButton.Click += OnSavePublishersButtonClick;

            insertPublishersButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            insertPublishersButton.Location = new Point(groupX, groupY);
            insertPublishersButton.Text = "Добавить";
            insertPublishersButton.Visible = false;
            insertPublishersButton.Click += OnInsertPublishersButtonClick;

            cancelPublishersButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            cancelPublishersButton.Location = new Point(groupX += savePublishersButton.Width + MARGIN, groupY);
            cancelPublishersButton.Text = "Отмена";
            cancelPublishersButton.Visible = false;
            cancelPublishersButton.Click += OnCancelPublishersButtonClick;

            addPublishersButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            addPublishersButton.Location = new Point(x = MARGIN, y);
            addPublishersButton.Text = "Добавить изд-во";
            addPublishersButton.Click += OnAddPublishersButtonClick;

            deletePublishersButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            deletePublishersButton.Location = new Point(x += addPublishersButton.Width + MARGIN, y);
            deletePublishersButton.Text = "Удалить изд-во";
            deletePublishersButton.Visible = false;
            deletePublishersButton.Click += OnDeletePublishersButtonClick;

            publishersGroupBox.Controls.Add(namePublishersTextBox);
            publishersGroupBox.Controls.Add(addressPublishersTextBox);
            publishersGroupBox.Controls.Add(namePublishLabel);
            publishersGroupBox.Controls.Add(addressLabel);
            publishersGroupBox.Controls.Add(savePublishersButton);
            publishersGroupBox.Controls.Add(insertPublishersButton);
            publishersGroupBox.Controls.Add(cancelPublishersButton);

            publishersTabPage.Controls.Add(publishersLabel);
            publishersTabPage.Controls.Add(publishersDataGridView);
            publishersTabPage.Controls.Add(editPublishersButton);
            publishersTabPage.Controls.Add(publishersGroupBox);
            publishersTabPage.Controls.Add(publishersInfoLabel);
            publishersTabPage.Controls.Add(addPublishersButton);
            publishersTabPage.Controls.Add(deletePublishersButton);
        }

        void InitReadersUI()
        {
            int x = MARGIN;
            int y = MARGIN;

            readersLabel.Text = "Данные о читателях:";
            readersLabel.Font = font;//todo dsds
            readersLabel.AutoSize = true;
            readersLabel.Location = new Point(x, y);

            readersDataGridView.Size = new Size(ClientSize.Width / 2, 2 * Height / 3);
            readersDataGridView.Location = new Point(x, y += readersLabel.Height + MARGIN / 2);
            readersDataGridView.BackgroundColor = Color.WhiteSmoke;
            readersDataGridView.AllowUserToAddRows = false;//dsds
            readersDataGridView.CellClick += OnReadersDataGridViewCellClick;

            readersInfoLabel.Text = "Информация о читателе:";
            readersInfoLabel.Font = font;
            readersInfoLabel.AutoSize = true;
            readersInfoLabel.Location = new Point(readersDataGridView.Width + 4 * MARGIN, readersLabel.Location.Y);

            readersGroupBox.Size = new Size(tabControl.Width - readersDataGridView.Width - 7 * MARGIN, 2 * Height / 3);
            readersGroupBox.Location = new Point(readersDataGridView.Width + 4 * MARGIN, readersDataGridView.Location.Y);

            editReadersButton.Text = "Изменить информацию";
            editReadersButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            editReadersButton.Location = new Point(readersGroupBox.Location.X - 4 * MARGIN + (ClientSize.Width - readersGroupBox.Width - editReadersButton.Width) / 2, y += readersDataGridView.Height + MARGIN / 2);
            editReadersButton.Click += OnEditReadersButtonClick;
            editReadersButton.Visible = false;

            Size labelSize = new Size(ClientSize.Width / 8, LABEL_HEIGHT);
            int groupX = MARGIN;
            int groupY = MARGIN;

            surnameLabel = GetLabel("Фамилия:", labelSize, new Point(groupY, groupY), font);
            cityLabel = GetLabel("Город:", labelSize, new Point(groupX, groupY += labelSize.Height + MARGIN), font);

            groupX = labelSize.Width + MARGIN;
            groupY = MARGIN;

            surnameTextBox = GetTextBox(new Size(ClientSize.Width / 4, labelSize.Height), new Point(groupX, groupY), font);
            surnameTextBox.ReadOnly = true;
            cityTextBox = GetTextBox(new Size(ClientSize.Width / 4, labelSize.Height), new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            cityTextBox.ReadOnly = true;

            saveReadersButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            saveReadersButton.Location = new Point(groupX, groupY += cityTextBox.Height + MARGIN);
            saveReadersButton.Text = "Сохранить";
            saveReadersButton.Visible = false;
            saveReadersButton.Click += OnSaveReadersButtonClick;

            insertReadersButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            insertReadersButton.Location = new Point(groupX, groupY);
            insertReadersButton.Text = "Добавить";
            insertReadersButton.Visible = false;
            insertReadersButton.Click += OnInsertReadersButtonClick;

            cancelReadersButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            cancelReadersButton.Location = new Point(groupX += saveReadersButton.Width + MARGIN, groupY);
            cancelReadersButton.Text = "Отмена";
            cancelReadersButton.Visible = false;
            cancelReadersButton.Click += OnCancelReadersButtonClick;

            addReadersButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            addReadersButton.Location = new Point(x = MARGIN, y);
            addReadersButton.Text = "Добавить читателя";
            addReadersButton.Click += OnAddReadersButtonClick;

            deleteReadersButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            deleteReadersButton.Location = new Point(x += addReadersButton.Width + MARGIN, y);
            deleteReadersButton.Text = "Удалить читателя";
            deleteReadersButton.Visible = false;
            deleteReadersButton.Click += OnDeleteReadersButtonClick;

            readersGroupBox.Controls.Add(surnameLabel);
            readersGroupBox.Controls.Add(cityLabel);
            readersGroupBox.Controls.Add(surnameTextBox);
            readersGroupBox.Controls.Add(cityTextBox);
            readersGroupBox.Controls.Add(saveReadersButton);
            readersGroupBox.Controls.Add(insertReadersButton);
            readersGroupBox.Controls.Add(cancelReadersButton);

            readersTabPage.Controls.Add(readersLabel);
            readersTabPage.Controls.Add(readersInfoLabel);
            readersTabPage.Controls.Add(readersDataGridView);
            readersTabPage.Controls.Add(readersGroupBox);
            readersTabPage.Controls.Add(addReadersButton);
            readersTabPage.Controls.Add(deleteReadersButton);
            readersTabPage.Controls.Add(editReadersButton);
        }

        void InitExtraditionUI()
        {
            int x = MARGIN;
            int y = MARGIN;

            extraditionLabel.Text = "Данные о выдачах книг:";
            extraditionLabel.Font = font;
            extraditionLabel.AutoSize = true;
            extraditionLabel.Location = new Point(x, y);

            extraditionDataGridView.Size = new Size(ClientSize.Width / 2, 2 * Height / 3);
            extraditionDataGridView.Location = new Point(x, y += extraditionLabel.Height + MARGIN / 2);
            extraditionDataGridView.BackgroundColor = Color.WhiteSmoke;
            extraditionDataGridView.AllowUserToAddRows = false;
            extraditionDataGridView.CellClick += OnExtraditionDataGridViewCellClick;
            extraditionDataGridView.DataBindingComplete += OnExtraditionDataGridViewDataBindingComplete;

            extraditionInfoLabel.Text = "Информация о выдаче книги:";
            extraditionInfoLabel.Font = font;
            extraditionInfoLabel.AutoSize = true;
            extraditionInfoLabel.Location = new Point(extraditionDataGridView.Width + 4 * MARGIN, extraditionLabel.Location.Y);

            extraditionGroupBox.Size = new Size(tabControl.Width - extraditionDataGridView.Width - 7 * MARGIN, 2 * Height / 3);
            extraditionGroupBox.Location = new Point(extraditionDataGridView.Width + 4 * MARGIN, extraditionDataGridView.Location.Y);

            editExtraditionButton.Text = "Изменить информацию";
            editExtraditionButton.Size = new Size(ClientSize.Width / 6, BUTTON_HEIGHT);
            editExtraditionButton.Location = new Point(extraditionGroupBox.Location.X - 4 * MARGIN + (ClientSize.Width - extraditionGroupBox.Width - editExtraditionButton.Width) / 2, y += extraditionDataGridView.Height + MARGIN / 2);
            editExtraditionButton.Click += OnEditExtraditionButtonClick;
            editExtraditionButton.Visible = false;

            Size labelSize = new Size(ClientSize.Width / 8, LABEL_HEIGHT);
            int groupX = MARGIN;
            int groupY = MARGIN;

            cardExtraLabel = GetLabel("Номер карты:", labelSize, new Point(groupY, groupY), font);
            libNumberExtraLabel = GetLabel("Библ-ый номер:", labelSize, new Point(groupX, groupY += labelSize.Height + MARGIN), font);
            dateLabel = GetLabel("Дата:", labelSize, new Point(groupX, groupY += labelSize.Height + MARGIN), font);

            groupX = labelSize.Width + MARGIN;
            groupY = MARGIN;

            cardExtraComboBox.Enabled = false;
            cardExtraComboBox.Size = new Size(ClientSize.Width / 4, labelSize.Height);
            cardExtraComboBox.Location = new Point(groupX, groupY);

            libNumberExtraComboBox.Enabled = false;
            libNumberExtraComboBox.Size = new Size(ClientSize.Width / 4, labelSize.Height);
            libNumberExtraComboBox.Location = new Point(groupX, groupY += labelSize.Height + MARGIN);

            dateTimePicker.Size = new Size(ClientSize.Width / 4, labelSize.Height);
            dateTimePicker.Location = new Point(groupX, groupY += labelSize.Height + MARGIN);
            dateTimePicker.Enabled = false;

            saveExtraditionButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            saveExtraditionButton.Location = new Point(groupX, groupY += dateTimePicker.Height + MARGIN); 
            saveExtraditionButton.Text = "Сохранить";
            saveExtraditionButton.Visible = false;
            saveExtraditionButton.Click += OnSaveExtraditionButtonClick;

            insertExtraditionButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            insertExtraditionButton.Location = new Point(groupX, groupY);
            insertExtraditionButton.Text = "Добавить";
            insertExtraditionButton.Visible = false;
            insertExtraditionButton.Click += OnInsertExtraditionButtonClick;

            cancelExtraditionButton.Size = new Size(ClientSize.Width / 14, labelSize.Height);
            cancelExtraditionButton.Location = new Point(groupX += saveExtraditionButton.Width + MARGIN, groupY);
            cancelExtraditionButton.Text = "Отмена";
            cancelExtraditionButton.Visible = false;
            cancelExtraditionButton.Click += OnCancelExtraditionButtonClick;

            addExtraditionButton.Size = new Size((extraditionDataGridView.Width - 2 * MARGIN) / 3, BUTTON_HEIGHT);
            addExtraditionButton.Location = new Point(x = MARGIN, y);
            addExtraditionButton.Text = "Добавить выдачу книги";
            addExtraditionButton.Click += OnAddExtraditionButtonClick;

            extendExtraditionButton.Size = new Size((extraditionDataGridView.Width - 2 * MARGIN) / 3, BUTTON_HEIGHT);
            extendExtraditionButton.Location = new Point(x += addExtraditionButton.Width + MARGIN, y);
            extendExtraditionButton.Visible = false;
            extendExtraditionButton.Text = "Продлить выдачу книги";
            extendExtraditionButton.Click += OnExtendExtraditionButtonClick;

            deleteExtraditionButton.Size = new Size((extraditionDataGridView.Width - 2 * MARGIN) / 3, BUTTON_HEIGHT);
            deleteExtraditionButton.Location = new Point(x += extendExtraditionButton.Width + MARGIN, y);
            deleteExtraditionButton.Text = "Удалить выдачу книги";
            deleteExtraditionButton.Visible = false;
            deleteExtraditionButton.Click += OnDeleteExtraditionButtonClick;

            extraditionGroupBox.Controls.Add(cardExtraLabel);
            extraditionGroupBox.Controls.Add(libNumberExtraLabel);
            extraditionGroupBox.Controls.Add(dateLabel);
            extraditionGroupBox.Controls.Add(cardExtraComboBox);
            extraditionGroupBox.Controls.Add(libNumberExtraComboBox);
            extraditionGroupBox.Controls.Add(dateTimePicker);
            extraditionGroupBox.Controls.Add(saveExtraditionButton);
            extraditionGroupBox.Controls.Add(insertExtraditionButton);
            extraditionGroupBox.Controls.Add(cancelExtraditionButton);

            extraditionTabPage.Controls.Add(extraditionLabel);
            extraditionTabPage.Controls.Add(extraditionInfoLabel);
            extraditionTabPage.Controls.Add(extraditionDataGridView);
            extraditionTabPage.Controls.Add(extraditionGroupBox);
            extraditionTabPage.Controls.Add(addExtraditionButton);
            extraditionTabPage.Controls.Add(extendExtraditionButton);
            extraditionTabPage.Controls.Add(deleteExtraditionButton);
            extraditionTabPage.Controls.Add(editExtraditionButton);
        }

        void LoadTables()
        {
            booksDataSet.Clear();
            booksDataSet = SelectRows(booksDataSet, "SELECT * FROM BOOKS");
            booksDataGridView.AutoGenerateColumns = true;
            booksDataGridView.DataSource = booksDataSet;
            booksDataGridView.DataMember = booksDataSet.Tables[0].TableName;
            booksDataGridView.ReadOnly = true;
            booksDataGridView.Columns[0].HeaderText = "ID";
            booksDataGridView.Columns[1].HeaderText = "Название";
            booksDataGridView.Columns[1].Width = ClientSize.Width / 8;
            booksDataGridView.Columns[2].HeaderText = "Автор";
            booksDataGridView.Columns[2].Width = ClientSize.Width / 8;
            booksDataGridView.Columns[3].HeaderText = "Изд-во";
            booksDataGridView.Columns[4].HeaderText = "Год";
            booksDataGridView.Columns[5].HeaderText = "Библ-ый номер";
            booksDataGridView.Columns[6].HeaderText = "Шифр тематики";
            booksDataGridView.Columns[7].HeaderText = "Кол-во";
            booksDataGridView.Columns[8].HeaderText = "Чит-ый зал";

            publishersDataSet.Clear();
            publishersDataSet = SelectRows(publishersDataSet, "SELECT * FROM PUBLISHERS");
            publishersDataGridView.AutoGenerateColumns = true;
            publishersDataGridView.DataSource = publishersDataSet;
            publishersDataGridView.DataMember = publishersDataSet.Tables[0].TableName;
            publishersDataGridView.ReadOnly = true;
            publishersDataGridView.Columns[0].HeaderText = "ID";
            publishersDataGridView.Columns[1].HeaderText = "Название";
            publishersDataGridView.Columns[2].HeaderText = "Адрес";
            publishersDataGridView.Columns[2].Width = ClientSize.Width / 4;

            readersDataSet.Clear();
            readersDataSet = SelectRows(readersDataSet, "SELECT r.ID, r.SURNAME, c.CITY, r.CARD_NUMBER FROM READERS r, CITY c WHERE r.CITY = c.ID");
            readersDataGridView.AutoGenerateColumns = true;
            readersDataGridView.DataSource = readersDataSet;
            readersDataGridView.DataMember = readersDataSet.Tables[0].TableName;
            readersDataGridView.ReadOnly = true;
            readersDataGridView.Columns[0].HeaderText = "ID";
            readersDataGridView.Columns[1].HeaderText = "Фамилия";
            readersDataGridView.Columns[2].HeaderText = "Город";
            readersDataGridView.Columns[2].Width = ClientSize.Width / 5;
            readersDataGridView.Columns[3].HeaderText = "Номер карты";

            extraditionDataSet.Clear();
            extraditionDataSet = SelectRows(extraditionDataSet, "SELECT * FROM EXTRADITION");
            extraditionDataGridView.AutoGenerateColumns = true;
            extraditionDataGridView.DataSource = extraditionDataSet;
            extraditionDataGridView.DataMember = extraditionDataSet.Tables[0].TableName;
            extraditionDataGridView.ReadOnly = true;
            extraditionDataGridView.Columns[0].HeaderText = "ID";
            extraditionDataGridView.Columns[1].HeaderText = "Номер карты";
            extraditionDataGridView.Columns[2].HeaderText = "Библ-ый номер";
            extraditionDataGridView.Columns[3].HeaderText = "Дата выдачи";
            extraditionDataGridView.Columns[3].Width = ClientSize.Width / 10;
            extraditionDataGridView.Columns[4].HeaderText = "Дата возврата";
            extraditionDataGridView.Columns[4].Width = ClientSize.Width / 10;

            ExtraditionColor();
        }

        void ExtraditionColor()
        {
            int rowCount = extraditionDataGridView.RowCount;
            for (int i = 0; i < rowCount; i++)
            {
                DateTime date = DateTime.Parse(extraditionDataGridView.Rows[i].Cells[4].Value.ToString());
                if (date > DateTime.Now)
                {
                    extraditionDataGridView.Rows[i].Cells[4].Style.BackColor = Color.Green;
                }
                else { extraditionDataGridView.Rows[i].Cells[4].Style.BackColor = Color.IndianRed; }
            }
        }
        
        void SetBookInformation()
        {
            DataGridViewRow row = booksDataGridView.CurrentRow;
            titleTextBox.Text = row.Cells[1].Value.ToString();
            authorTextBox.Text = row.Cells[2].Value.ToString();
            publishersComboBox.Text = row.Cells[3].Value.ToString();
            yearTextBox.Text = row.Cells[4].Value.ToString();
            themeNumberTextBox.Text = row.Cells[6].Value.ToString();
            countTextBox.Text = row.Cells[7].Value.ToString();
            if (row.Cells[8].Value.ToString() == "True") { readerClassRadioButton.Checked = true; }
            else { homeRadioButton.Checked = true; }
        }

        void SetPublishersInformation()
        {
            DataGridViewRow row = publishersDataGridView.CurrentRow;
            namePublishersTextBox.Text = row.Cells[1].Value.ToString();
            addressPublishersTextBox.Text = row.Cells[2].Value.ToString();
        }

        void SetExtraditionInformation()
        {
            DataGridViewRow row = extraditionDataGridView.CurrentRow;
            cardExtraComboBox.Text = row.Cells[1].Value.ToString();
            libNumberExtraComboBox.Text = row.Cells[2].Value.ToString();
            dateTimePicker.Text = row.Cells[3].Value.ToString();
        }

        void SetReadersInformation()
        {
            DataGridViewRow row = readersDataGridView.CurrentRow;
            surnameTextBox.Text = row.Cells[1].Value.ToString();
            cityTextBox.Text = row.Cells[2].Value.ToString();      
        }

        void LoadData(List<string> list, ComboBox comboBox, string query)
        {
            list = ExecuteQuery(query);
            comboBox.DataSource = list;
        }

        DataSet SelectRows(DataSet dataSet, string query)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                adapter.SelectCommand = new NpgsqlCommand(query, connection);
                adapter.Fill(dataSet);
                return dataSet;
            }
        }

        List<string> ExecuteQuery(string query)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();

                List<string> list = new List<string>();
                while (dataReader.Read())
                {
                    switch (dataReader.GetDataTypeName(0))
                    {
                        case "int4":
                            list.Add(dataReader.GetInt32(0).ToString());
                            break;
                        case "bpchar":
                            list.Add(dataReader.GetString(0));
                            break;
                    }
                }
                dataReader.Close();

                return list;
            }
        }

        void ExecuteNonQuery(string query)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }   
        }

        Label GetLabel(string text, Size size, Point point, Font fnt)
        {
            Label label = new Label();
            label.Text = text;
            label.Size = size;
            label.Location = point;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Font = fnt;
            return label;
        }

        TextBox GetTextBox(Size size, Point point, Font fnt)
        {
            TextBox textBox = new TextBox();
            textBox.Size = size;
            textBox.Location = point;
            textBox.Font = fnt;
            return textBox;
        }

        void ClearBooksGroupBox()
        {
            titleTextBox.Text = string.Empty;
            authorTextBox.Text = string.Empty;
            yearTextBox.Text = string.Empty;
            themeNumberTextBox.Text = string.Empty;
            countTextBox.Text = string.Empty;
        }

        void ClearPublishersGroupBox()
        {
            namePublishersTextBox.Text = string.Empty;
            addressPublishersTextBox.Text = string.Empty;
        }

        void ClearReadersGroupBox()
        {
            surnameTextBox.Text = string.Empty;
            cityTextBox.Text = string.Empty;
        }

        void ClearExtraditionGroupBox()
        {
            cardExtraTextBox.Text = string.Empty;
            libNumberExtraTextBox.Text = string.Empty;
            dateTimePicker.Value = DateTime.Now;
        }

        bool isCityExist(string city)
        {
            DataSet dataSet = new DataSet();
            dataSet = SelectRows(dataSet, $"SELECT COUNT(1) FROM CITY WHERE CITY = '{city}'");

            if (Convert.ToInt32(dataSet.Tables[0].Rows[0][0]) == 0) { return false; }

            return true;
        }

        void ExecuteCommand(CommandType commandType, string pathBackup, string dbName)
        {
            var userName = "postgres";
            var executablePath = @"D:\programs\Yniverskaya Parawa\PostgreSQL\9.5\bin\psql.exe";
            string arguments = null;

            switch (commandType)
            {
                case CommandType.CreateDb:
                    arguments = $"-c \"CREATE DATABASE {dbName};\" -U postgres";
                    break;
                case CommandType.Restore:
                    arguments = $"-d {dbName} -1 -f \"{pathBackup}\" -U {userName}";
                    break;
                case CommandType.DeleteDb:
                    arguments = $"-c \"DROP DATABASE {dbName};\" -U postgres";
                    break;
            }

            try
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                info.FileName = executablePath;
                info.Arguments = arguments;
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = info;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                //Console.writeline(ex.Message);
            }
        }

        void OnEditBookButtonClick(object sender, EventArgs eventArgs)
        {
            isAddBookAction = false;

            titleTextBox.ReadOnly = false;
            authorTextBox.ReadOnly = false;
            publishersComboBox.Enabled = true;
            LoadData(new List<string>(), publishersComboBox, "SELECT NAME FROM PUBLISHERS");

            DataGridViewRow row = booksDataGridView.CurrentRow;
            string publisher = row.Cells[3].Value.ToString();

            for (int i = 0; i < publishersComboBox.Items.Count; i++)
            {
                if (publishersComboBox.Items[i].ToString() == publisher) { publishersComboBox.SelectedItem = publishersComboBox.Items[i]; }
            }
            yearTextBox.ReadOnly = false;
            themeNumberTextBox.ReadOnly = false;
            countTextBox.ReadOnly = false;
            readerClassRadioButton.Enabled = true;
            homeRadioButton.Enabled = true;

            saveBookButton.Visible = true;
            cancelBookButton.Visible = true;

            booksDataGridView.CellClick -= OnBooksDataGridViewCellClick;
        }

        void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            parentForm.Show();
        }

        void OnBooksDataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetBookInformation();
            editBookButton.Visible = true;
            deleteBookButton.Visible = true;
        }

        void OnSaveBookButtonClick(object sender, EventArgs eventArgs)
        {
            DataGridViewRow row = booksDataGridView.CurrentRow;
            int id = Convert.ToInt32(row.Cells[0].Value);

            StringBuilder builder = new StringBuilder();
            builder.Append($"UPDATE BOOKS SET TITLE = '{titleTextBox.Text}', ");
            builder.Append($"AUTHOR = '{authorTextBox.Text}', PUBLISH_HOUSE = '{publishersComboBox.SelectedItem}',");
            builder.Append($"YEAR = {yearTextBox.Text}, ");
            builder.Append($"THEME_NUMBER = {themeNumberTextBox.Text}, COUNT = {countTextBox.Text},");
            builder.Append($"ATTRIBUTE = '1' WHERE ID = {id}");

            string query = builder.ToString();
            ExecuteNonQuery(query);

            LoadTables();

            saveBookButton.Visible = false;
            cancelBookButton.Visible = false;
            titleTextBox.ReadOnly = true;
            authorTextBox.ReadOnly = true;
            publishersComboBox.Enabled = false;
            yearTextBox.ReadOnly = true;
            themeNumberTextBox.ReadOnly = true;
            countTextBox.ReadOnly = true;
            readerClassRadioButton.Enabled = false;
            homeRadioButton.Enabled = false;
        }
        void OnCancelBookButtonClick(object sender, EventArgs eventArgs)
        {
            string text = "изменения";
            if (isAddBookAction) { text = "добавление";}

            if (MessageBox.Show($"Вы действительно хотите отменить {text}?", "Предупреждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                saveBookButton.Visible = false;
                cancelBookButton.Visible = false;
                titleTextBox.ReadOnly = true;
                authorTextBox.ReadOnly = true;
                publishersComboBox.Enabled = false;
                yearTextBox.ReadOnly = true;
                themeNumberTextBox.ReadOnly = true;
                countTextBox.ReadOnly = true;
                readerClassRadioButton.Enabled = false;
                homeRadioButton.Enabled = false;

                if (!isAddBookAction)
                {
                    SetBookInformation();
                    saveBookButton.Visible = false;
                    cancelBookButton.Visible = false;
                    editBookButton.Visible = false;
                }
                else
                {
                    insertBookButton.Visible = false;
                    cancelBookButton.Visible = false;
                    editBookButton.Visible = false;

                    ClearBooksGroupBox();
                }
                booksDataGridView.CellClick += OnBooksDataGridViewCellClick;
            }
        }

        void OnAddBookButtonClick(object sender, EventArgs eventArgs)
        {
            isAddBookAction = true;

            titleTextBox.ReadOnly = false;
            authorTextBox.ReadOnly = false;
            publishersComboBox.Enabled = true;
            LoadData(new List<string>(), publishersComboBox, "SELECT NAME FROM PUBLISHERS");

            yearTextBox.ReadOnly = false;
            themeNumberTextBox.ReadOnly = false;
            countTextBox.ReadOnly = false;
            readerClassRadioButton.Enabled = true;
            homeRadioButton.Enabled = true;

            insertBookButton.Visible = true;
            cancelBookButton.Visible = true;
            editBookButton.Visible = false;

            ClearBooksGroupBox();

            booksDataGridView.CellClick -= OnBooksDataGridViewCellClick;
        }
        void OnDeleteBookButtonClick(object sender, EventArgs eventArgs)
        {
            DataGridViewRow row = booksDataGridView.CurrentRow;
            string title = row.Cells[1].Value.ToString();

            if (MessageBox.Show($"Вы действительно хотите удалить книгу \"{title.Trim()}\" из каталога", "Предупреждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(row.Cells[0].Value);

                string query = $"DELETE FROM BOOKS WHERE ID = {id}";
                ExecuteNonQuery(query);
                LoadTables();

                ClearBooksGroupBox();

                deleteBookButton.Visible = false;
            }
        }
        void OnInsertBookButtonClick(object sender, EventArgs e)
        {
            if (titleTextBox.Text == ""
                    || authorTextBox.Text == ""
                    || publishersComboBox.SelectedIndex == -1
                    || yearTextBox.Text == ""
                    || themeNumberTextBox.Text == ""
                    || countTextBox.Text == "")
            {
                MessageBox.Show("Заполнены не все поля", "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO BOOKS (TITLE, AUTHOR, PUBLISH_HOUSE, YEAR, THEME_NUMBER, COUNT, ATTRIBUTE) ");
            builder.Append($"VALUES ('{titleTextBox.Text}', '{authorTextBox.Text}', '{publishersComboBox.SelectedItem}', ");
            builder.Append($"{yearTextBox.Text}, {themeNumberTextBox.Text}, ");
            if (readerClassRadioButton.Checked) { builder.Append($"{countTextBox.Text}, '1');"); }
            else { builder.Append($"{countTextBox.Text}, '0');"); }
            string query = builder.ToString();
            ExecuteNonQuery(query);

            booksDataSet.Clear();
            LoadTables();

            saveBookButton.Visible = false;
            cancelBookButton.Visible = false;
            titleTextBox.ReadOnly = true;
            authorTextBox.ReadOnly = true;
            publishersComboBox.Enabled = false;
            yearTextBox.ReadOnly = true;
            themeNumberTextBox.ReadOnly = true;
            countTextBox.ReadOnly = true;
            readerClassRadioButton.Enabled = false;
            homeRadioButton.Enabled = false;

            insertBookButton.Visible = false;
            cancelBookButton.Visible = false;
            editBookButton.Visible = false;

            ClearBooksGroupBox();

            booksDataGridView.CellClick += OnBooksDataGridViewCellClick;
        }

        void OnPublishersDataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetPublishersInformation();
            editPublishersButton.Visible = true;
            deletePublishersButton.Visible = true;
        }
        void OnEditPublishersButtonClick(object sender, EventArgs e)
        {
            namePublishersTextBox.ReadOnly = false;
            addressPublishersTextBox.ReadOnly = false;

            savePublishersButton.Visible = true;
            cancelPublishersButton.Visible = true;
        }

        void OnExtraditionDataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetExtraditionInformation();
            editExtraditionButton.Visible = true;
            extendExtraditionButton.Visible = true;
            deleteExtraditionButton.Visible = true;
        }
        void OnEditExtraditionButtonClick(object sender, EventArgs e)
        {
            cardExtraComboBox.Enabled = true;
            libNumberExtraComboBox.Enabled = true;
            dateTimePicker.Enabled = true;
            LoadData(new List<string>(), cardExtraComboBox, "SELECT CARD_NUMBER FROM READERS");
            LoadData(new List<string>(), libNumberExtraComboBox, "SELECT LIBRARY_NUMBER FROM BOOKS");

            DataGridViewRow row = extraditionDataGridView.CurrentRow;
            string cardNumber = row.Cells[1].Value.ToString();
            string libNumber = row.Cells[2].Value.ToString();
            string date = row.Cells[3].Value.ToString();
            DateTime dateFromString = DateTime.Parse(date);
            dateTimePicker.Value = dateFromString;

            for (int i = 0; i < cardExtraComboBox.Items.Count; i++)
            {
                if (cardExtraComboBox.Items[i].ToString() == cardNumber) { cardExtraComboBox.SelectedItem = cardExtraComboBox.Items[i]; }
            }

            for (int i = 0; i < libNumberExtraComboBox.Items.Count; i++)
            {
                if (libNumberExtraComboBox.Items[i].ToString() == libNumber) { libNumberExtraComboBox.SelectedItem = libNumberExtraComboBox.Items[i]; }
            }

            saveExtraditionButton.Visible = true;
            cancelExtraditionButton.Visible = true;
            
            extraditionDataGridView.CellClick -= OnExtraditionDataGridViewCellClick;
        }
        void OnReadersDataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetReadersInformation();
            editExtraditionButton.Visible = true;
            deleteExtraditionButton.Visible = true;

            editReadersButton.Visible = true;
            deleteReadersButton.Visible = true;
        }

        void OnEditReadersButtonClick(object sender, EventArgs e)
        {
            surnameTextBox.ReadOnly = false;
            cityTextBox.ReadOnly = false;

            saveReadersButton.Visible = true;
            cancelReadersButton.Visible = true;
        }

        void OnSavePublishersButtonClick(object sender, EventArgs eventArgs)
        {
            DataGridViewRow row = publishersDataGridView.CurrentRow;
            int id = Convert.ToInt32(row.Cells[0].Value);

            StringBuilder builder = new StringBuilder();
            builder.Append($"UPDATE PUBLISHERS SET NAME = '{namePublishersTextBox.Text}', ");
            builder.Append($"ADDRESS = '{addressPublishersTextBox.Text}' ");
            builder.Append($"WHERE ID = {id}");

            string query = builder.ToString();
            ExecuteNonQuery(query);

            LoadTables();

            savePublishersButton.Visible = false;
            cancelPublishersButton.Visible = false;
            namePublishersTextBox.ReadOnly = true;
            addressPublishersTextBox.ReadOnly = true;
        }

        void OnCancelPublishersButtonClick(object sender, EventArgs eventArgs)
        {
            string text = "изменения";
            if (isAddPublishersAction) { text = "добавление"; }

            if (MessageBox.Show($"Вы действительно хотите отменить {text}?", "Предупреждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                savePublishersButton.Visible = false;
                cancelPublishersButton.Visible = false;
                namePublishersTextBox.ReadOnly = true;
                addressPublishersTextBox.ReadOnly = true;

                if (!isAddPublishersAction)
                {
                    SetPublishersInformation();
                    savePublishersButton.Visible = false;
                    cancelPublishersButton.Visible = false;
                    editPublishersButton.Visible = false;
                }
                else
                {
                    insertPublishersButton.Visible = false; 
                    cancelPublishersButton.Visible = false;
                    editPublishersButton.Visible = false;

                    ClearPublishersGroupBox();

                    publishersDataGridView.CellClick += OnPublishersDataGridViewCellClick;
                }
            }
        }

        void OnAddPublishersButtonClick(object sender, EventArgs eventArgs)
        {
            isAddPublishersAction = true;

            namePublishersTextBox.ReadOnly = false;
            addressPublishersTextBox.ReadOnly = false;

            insertPublishersButton.Visible = true; 
            cancelPublishersButton.Visible = true;
            editPublishersButton.Visible = false;

            ClearPublishersGroupBox(); 

            publishersDataGridView.CellClick -= OnPublishersDataGridViewCellClick;
        }


        void OnInsertPublishersButtonClick(object sender, EventArgs eventArgs)
        {
            if (namePublishersTextBox.Text == "" || addressPublishersTextBox.Text == "")
            {
                MessageBox.Show("Заполнены не все поля", "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO PUBLISHERS (NAME, ADDRESS) ");
            builder.Append($"VALUES ('{namePublishersTextBox.Text}', '{addressPublishersTextBox.Text}')");
            string query = builder.ToString();
            ExecuteNonQuery(query);

            LoadTables();

            savePublishersButton.Visible = false;
            cancelPublishersButton.Visible = false;
            namePublishersTextBox.ReadOnly = true;
            addressPublishersTextBox.ReadOnly = true;

            insertPublishersButton.Visible = false;
            cancelPublishersButton.Visible = false;
            editPublishersButton.Visible = false;

            ClearPublishersGroupBox(); 

            publishersDataGridView.CellClick += OnPublishersDataGridViewCellClick;
        }

        void OnDeletePublishersButtonClick(object sender, EventArgs eventArgs)
        {
            DataGridViewRow row = publishersDataGridView.CurrentRow;
            string name = row.Cells[1].Value.ToString();

            if (MessageBox.Show($"Вы действительно хотите удалить изд-во \"{name.Trim()}\" из каталога", "Предупреждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(row.Cells[0].Value);

                string query = $"DELETE FROM PUBLISHERS WHERE ID = {id}";
                ExecuteNonQuery(query);
                LoadTables();

                ClearPublishersGroupBox();

                deletePublishersButton.Visible = false;
            }
        }

        void OnInsertReadersButtonClick(object sender, EventArgs eventArgs)
        {
            if (surnameTextBox.Text == "" || cityTextBox.Text == "")
            {
                MessageBox.Show("Заполнены не все поля", "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (!isCityExist(cityTextBox.Text)) { ExecuteNonQuery($"INSERT INTO CITY (CITY) VALUES ('{cityTextBox.Text}')"); }

            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO READERS (SURNAME, CITY) ");
            builder.Append($"VALUES ('{surnameTextBox.Text}',");
            builder.Append($"(SELECT ID FROM CITY WHERE CITY = '{cityTextBox.Text}'))");
            string query = builder.ToString();
            ExecuteNonQuery(query);

            LoadTables();

            saveReadersButton.Visible = false;
            cancelReadersButton.Visible = false;
            surnameTextBox.ReadOnly = true;
            cityTextBox.ReadOnly = true;

            insertReadersButton.Visible = false;
            cancelReadersButton.Visible = false;
            editReadersButton.Visible = false;

            ClearReadersGroupBox();

            readersDataGridView.CellClick += OnReadersDataGridViewCellClick;
        }

        void OnSaveReadersButtonClick(object sender, EventArgs eventArgs)
        {
            DataGridViewRow row = readersDataGridView.CurrentRow;
            int id = Convert.ToInt32(row.Cells[0].Value);

            StringBuilder builder = new StringBuilder();
            builder.Append($"UPDATE READERS SET SURNAME = '{surnameTextBox.Text}', ");
            builder.Append($"CITY = (SELECT ID FROM CITY WHERE CITY = '{cityTextBox.Text}') ");
            builder.Append($"WHERE ID = {id}");

            string query = builder.ToString();
            ExecuteNonQuery(query);

            LoadTables();

            saveReadersButton.Visible = false;
            cancelReadersButton.Visible = false;
            surnameTextBox.ReadOnly = true;
            cityTextBox.ReadOnly = true;
        }

        void OnCancelReadersButtonClick(object sender, EventArgs eventArgs)
        {
            string text = "изменения";
            if (isAddReadersAction) { text = "добавление"; }

            if (MessageBox.Show($"Вы действительно хотите отменить {text}?", "Предупреждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                saveReadersButton.Visible = false;
                cancelReadersButton.Visible = false;
                surnameTextBox.ReadOnly = true;
                cityTextBox.ReadOnly = true;

                if (!isAddReadersAction)
                {
                    SetReadersInformation();
                    saveReadersButton.Visible = false;
                    cancelReadersButton.Visible = false;
                    editReadersButton.Visible = false;
                }
                else
                {
                    insertReadersButton.Visible = false;
                    cancelReadersButton.Visible = false;
                    editReadersButton.Visible = false;

                    ClearReadersGroupBox();

                    readersDataGridView.CellClick += OnReadersDataGridViewCellClick;
                }
            }
        }

        void OnAddReadersButtonClick(object sender, EventArgs eventArgs)
        {
            isAddReadersAction = true;

            surnameTextBox.ReadOnly = false;
            cityTextBox.ReadOnly = false;

            insertReadersButton.Visible = true;
            cancelReadersButton.Visible = true;
            editReadersButton.Visible = false;

            ClearReadersGroupBox();

            readersDataGridView.CellClick -= OnReadersDataGridViewCellClick;
        }

        void OnDeleteReadersButtonClick(object sender, EventArgs eventArgs)
        {
            DataGridViewRow row = readersDataGridView.CurrentRow;
            string surname = row.Cells[1].Value.ToString();

            if (MessageBox.Show($"Вы действительно хотите удалить читателя \"{surname.Trim()}\" из каталога", "Предупреждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(row.Cells[0].Value);

                string query = $"DELETE FROM READERS WHERE ID = {id}";
                ExecuteNonQuery(query);
                LoadTables();

                ClearReadersGroupBox();

                deleteReadersButton.Visible = false;
            }
        }

        void OnInsertExtraditionButtonClick(object sender, EventArgs eventArgs)
        {
            if (cardExtraComboBox.SelectedIndex == -1 || libNumberExtraComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Заполнены не все поля", "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (dateTimePicker.Value > DateTime.Now)
            {
                MessageBox.Show("Выбрана некорректная дата", "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO EXTRADITION (CARD_NUMBER, LIBRARY_NUMBER, DATE, DELIVERY_DATE) ");
            builder.Append($"VALUES ({cardExtraComboBox.SelectedItem}, {libNumberExtraComboBox.SelectedItem}, '{dateTimePicker.Value.ToShortDateString()}', ");
            builder.Append($"'{dateTimePicker.Value.AddDays(14).ToShortDateString()}')");
            string query = builder.ToString();
            ExecuteNonQuery(query);

            LoadTables();

            saveExtraditionButton.Visible = false;
            cancelExtraditionButton.Visible = false;
            cardExtraComboBox.Enabled = false;
            libNumberExtraComboBox.Enabled = false;
            dateTimePicker.Enabled = false;

            insertExtraditionButton.Visible = false;
            cancelExtraditionButton.Visible = false;
            editExtraditionButton.Visible = false;

            ClearExtraditionGroupBox();

            extraditionDataGridView.CellClick += OnExtraditionDataGridViewCellClick;
        }

        void OnSaveExtraditionButtonClick(object sender, EventArgs eventArgs)
        {
            DataGridViewRow row = extraditionDataGridView.CurrentRow;
            int id = Convert.ToInt32(row.Cells[0].Value);

            StringBuilder builder = new StringBuilder();
            builder.Append($"UPDATE EXTRADITION SET CARD_NUMBER = {cardExtraComboBox.SelectedItem}, ");
            builder.Append($"LIBRARY_NUMBER = {libNumberExtraComboBox.SelectedItem}, ");
            builder.Append($"DATE = '{dateTimePicker.Value.ToShortDateString()}' ");
            builder.Append($"WHERE ID = {id}");

            string query = builder.ToString();
            ExecuteNonQuery(query);

            LoadTables();

            saveExtraditionButton.Visible = false;
            cancelExtraditionButton.Visible = false;
            cardExtraComboBox.Enabled = false;
            libNumberExtraComboBox.Enabled = false;
            dateTimePicker.Enabled = false;
        }

        void OnCancelExtraditionButtonClick(object sender, EventArgs eventArgs)
        {
            string text = "изменения";
            if (isAddExtraditionAction) { text = "добавление"; }

            if (MessageBox.Show($"Вы действительно хотите отменить {text}?", "Предупреждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                saveExtraditionButton.Visible = false;
                cancelExtraditionButton.Visible = false;
                cardExtraComboBox.Enabled = false;
                libNumberExtraComboBox.Enabled = false;
                dateTimePicker.Enabled = false;

                if (!isAddExtraditionAction)
                {
                    SetExtraditionInformation();
                    saveExtraditionButton.Visible = false;
                    cancelExtraditionButton.Visible = false;
                    editExtraditionButton.Visible = false;
                }
                else
                {
                    insertExtraditionButton.Visible = false;
                    cancelExtraditionButton.Visible = false;
                    editExtraditionButton.Visible = false;

                    ClearExtraditionGroupBox();
                }
                extraditionDataGridView.CellClick += OnExtraditionDataGridViewCellClick;
            }
        }

        void OnAddExtraditionButtonClick(object sender, EventArgs eventArgs)
        {
            isAddExtraditionAction = true;

            cardExtraComboBox.Enabled = true;
            libNumberExtraComboBox.Enabled = true;
            LoadData(new List<string>(), cardExtraComboBox, "SELECT CARD_NUMBER FROM READERS");
            LoadData(new List<string>(), libNumberExtraComboBox, "SELECT LIBRARY_NUMBER FROM BOOKS");
 
            dateTimePicker.Enabled = true;

            insertExtraditionButton.Visible = true;
            cancelExtraditionButton.Visible = true;
            editExtraditionButton.Visible = false;

            ClearExtraditionGroupBox();

            extraditionDataGridView.CellClick -= OnExtraditionDataGridViewCellClick;
        }

        void OnExtendExtraditionButtonClick(object sender, EventArgs eventArgs)
        {
            DataGridViewRow row = extraditionDataGridView.CurrentRow;
            int id = Convert.ToInt32(row.Cells[0].Value);

            if (MessageBox.Show($"Вы действительно хотите продлить выдачу книги с ID \"{id}\"?", "Предупреждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string query = $"UPDATE EXTRADITION SET DELIVERY_DATE = '{DateTime.Now.AddDays(14)}' WHERE ID = {id}";
                ExecuteNonQuery(query);
                LoadTables();

                extendExtraditionButton.Visible = false;
                deleteExtraditionButton.Visible = false;
            }
        }

        void OnDeleteExtraditionButtonClick(object sender, EventArgs eventArgs)
        {
            DataGridViewRow row = extraditionDataGridView.CurrentRow;
            string cardNumber = row.Cells[1].Value.ToString();
            int id = Convert.ToInt32(row.Cells[0].Value);

            if (MessageBox.Show($"Вы действительно хотите удалить выдачу с ID \"{id}\"?", "Предупреждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string query = $"DELETE FROM EXTRADITION WHERE ID = {id}";
                ExecuteNonQuery(query);
                LoadTables();

                ClearExtraditionGroupBox();

                extendExtraditionButton.Visible = false;
                deleteExtraditionButton.Visible = false;
            }
        }

        void OnExtraditionDataGridViewDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs dataGridViewBindingCompleteEventArgs)
        {
            ExtraditionColor();
        }

        void OnTextBoxDigitKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8) { e.Handled = true; }
        }

        void OnBackupDbMenuItemClick(object sender, EventArgs eventArgs)
        {
            string database = "rgris";
            string userName = "postgres";
            string backupFolder = Application.StartupPath;
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            string file = $"{timestamp}.backup";
            string fullPath = $@"{backupFolder}\{file}";
            string executablePath = @"D:\programs\Yniverskaya Parawa\PostgreSQL\9.5\bin\pg_dump.exe";
            string arguments = $"-d{database} -U{userName} -w -f\"{fullPath}\"";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "BackUp files(*.backup)|*.backup";
            saveFileDialog.FileName = file;
            saveFileDialog.InitialDirectory = backupFolder;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    info.FileName = executablePath;
                    info.Arguments = arguments;
                    info.CreateNoWindow = true;
                    info.UseShellExecute = false;
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo = info;
                    proc.Start();
                    proc.WaitForExit();
                }
                catch (Exception ex)
                {
                    //Console.writeline(ex.Message);
                }
            }
        }

        void OnRestoreDbMenuItemClick(object sender, EventArgs eventArgs)
        {
            string database = "rgris";
            string backupFolder = Application.StartupPath;
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            string file = $"{timestamp}.backup";


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BackUp files(*.backup)|*.backup";
            openFileDialog.FileName = file;
            openFileDialog.InitialDirectory = backupFolder;
            openFileDialog.Title = "Выбрать Backup файл";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExecuteCommand(CommandType.DeleteDb, null, database);
                ExecuteCommand(CommandType.CreateDb, null, database);
                ExecuteCommand(CommandType.Restore, openFileDialog.FileName, database);
                MessageBox.Show("БД восстановлена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
