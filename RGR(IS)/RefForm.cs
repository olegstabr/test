using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace RGR_IS_
{
    class RefForm : Form
    {
        Form parentForm;

        DataGridView dataGridView = new DataGridView();
        DataSet firstDataSet = new DataSet();
        DataSet secondDataSet = new DataSet();
        DataSet thirdDataSet = new DataSet();

        Label headLabel = new Label();
        Label publishersLabel = new Label();
        Label penaltyReadersLabel = new Label();
        Label themeReadersLabel = new Label();
        Label infoLabel = new Label();

        DateTimePicker dateTimePicker = new DateTimePicker();

        ComboBox publishersComboBox = new ComboBox();
        ComboBox themeReadersComboBox = new ComboBox();

        Button showPublishersButton = new Button();
        Button showPenaltyReadersButton = new Button();
        Button showThemeReadersButton = new Button();

        Font font = new Font(FontFamily.GenericSerif, 14, FontStyle.Regular);

        const int MARGIN = 10;
        string connectionString;

        public RefForm(Form parentForm)
        {
            this.parentForm = parentForm;

            Text = "Справки";
            Size = new Size(1024, 512);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ShowIcon = false;

            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            InitUI();
            LoadData();

            FormClosing += OnFormClosing;
        }

        void InitUI()
        {
            int x = MARGIN;
            int y = MARGIN;

            infoLabel = GetLabel("Информация о справке:",
                new Size((ClientSize.Width - 6 * MARGIN) / 2, ClientSize.Height / 10),
                new Point((ClientSize.Width - 2 * MARGIN) / 2, y), font);

            dataGridView.Size = new Size((ClientSize.Width) / 2, ClientSize.Height - 3 * MARGIN - infoLabel.Height);
            dataGridView.Location = new Point((ClientSize.Width - 2 * MARGIN) / 2, y += infoLabel.Height + MARGIN);
            dataGridView.BackgroundColor = Color.WhiteSmoke;
            dataGridView.AllowUserToAddRows = false;

            headLabel = GetLabel("СПРАВКИ", new Size(ClientSize.Width / 8, ClientSize.Height / 10),
                    new Point((ClientSize.Width - 3 * MARGIN - headLabel.Width - dataGridView.Width) / 2, y), font);
            publishersLabel = GetLabel("1. Справки по книгам заданного издательства",
                    new Size((ClientSize.Width - 6 * MARGIN) / 2, ClientSize.Height / 10),
                    new Point(x, y += headLabel.Height + MARGIN), font);

            publishersComboBox.Text = "Выберите изд-во...";
            publishersComboBox.Width = ClientSize.Width / 4;
            publishersComboBox.Location = new Point(x, y += publishersLabel.Height + MARGIN);

            showPublishersButton.Text = "Показать";
            showPublishersButton.Size = new Size(ClientSize.Width / 8, publishersComboBox.Height);
            showPublishersButton.Location = new Point(x += publishersComboBox.Width + MARGIN, y);
            showPublishersButton.Click += OnShowPublishersButtonClick;

            penaltyReadersLabel = GetLabel("2. Справки по читателям, просрочившим сдачу книги от заданной даты",
                    new Size((ClientSize.Width - 6 * MARGIN) / 2, ClientSize.Height / 10),
                    new Point(x = MARGIN, y += publishersComboBox.Height + MARGIN), font);

            dateTimePicker.Width = ClientSize.Width / 4;
            dateTimePicker.Location = new Point(x, y += penaltyReadersLabel.Height + MARGIN);

            showPenaltyReadersButton.Text = "Показать";
            showPenaltyReadersButton.Size = new Size(ClientSize.Width / 8, publishersComboBox.Height);
            showPenaltyReadersButton.Location = new Point(x += dateTimePicker.Width + MARGIN, y);
            showPenaltyReadersButton.Click += OnShowPenaltyReadersButtonClick;

            themeReadersLabel = GetLabel("3. Списки читателей по заданной тематике",
                new Size((ClientSize.Width - 6 * MARGIN) / 2, ClientSize.Height / 10),
                new Point(x = MARGIN, y += dateTimePicker.Height + MARGIN), font);

            themeReadersComboBox.Text = "Выберите тематику...";
            themeReadersComboBox.Width = ClientSize.Width / 4;
            themeReadersComboBox.Location = new Point(x, y += themeReadersLabel.Height + MARGIN);

            showThemeReadersButton.Text = "Показать";
            showThemeReadersButton.Size = new Size(ClientSize.Width / 8, publishersComboBox.Height);
            showThemeReadersButton.Location = new Point(x += themeReadersComboBox.Width + MARGIN, y);
            showThemeReadersButton.Click += OnShowThemeReadersButtonClick;

            Controls.Add(infoLabel);
            Controls.Add(dataGridView);
            Controls.Add(headLabel);
            Controls.Add(publishersLabel);
            Controls.Add(publishersComboBox);
            Controls.Add(showPublishersButton);
            Controls.Add(penaltyReadersLabel);
            Controls.Add(dateTimePicker);
            Controls.Add(showPenaltyReadersButton);
            Controls.Add(themeReadersLabel);
            Controls.Add(themeReadersComboBox);
            Controls.Add(showThemeReadersButton);
        }

        void LoadData()
        {
            List<string> list = ExecuteQuery("SELECT NAME FROM PUBLISHERS");
            publishersComboBox.DataSource = list;
            list = ExecuteQuery("SELECT NAME FROM THEME");
            themeReadersComboBox.DataSource = list;
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
                    list.Add(dataReader.GetString(0));
                }
                dataReader.Close();

                return list;
            }
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

        Label GetLabel(string text, Size size, Point point, Font fnt)
        {
            Label label = new Label();
            label.Text = text;
            label.Size = size;
            label.Location = point;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Font = fnt;
            return label;
        }

        void OnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            parentForm.Show();
        }

        void OnShowPublishersButtonClick(object sender, EventArgs eventArgs)
        {
            firstDataSet.Clear();
            firstDataSet = SelectRows(firstDataSet, $"SELECT * FROM BOOKS WHERE PUBLISH_HOUSE = '{publishersComboBox.SelectedItem}'");
            dataGridView.AutoGenerateColumns = true;
            dataGridView.DataSource = firstDataSet;
            dataGridView.DataMember = firstDataSet.Tables[0].TableName;
            dataGridView.ReadOnly = true;
            dataGridView.Columns[0].HeaderText = "ID";
            dataGridView.Columns[1].HeaderText = "Название";
            dataGridView.Columns[1].Width = ClientSize.Width / 8;
            dataGridView.Columns[2].HeaderText = "Автор";
            dataGridView.Columns[2].Width = ClientSize.Width / 8;
            dataGridView.Columns[3].HeaderText = "Изд-во";
            dataGridView.Columns[4].HeaderText = "Год";
            dataGridView.Columns[5].HeaderText = "Библ-ый номер";
            dataGridView.Columns[6].HeaderText = "Шифр тематики";
            dataGridView.Columns[7].HeaderText = "Кол-во";
            dataGridView.Columns[8].HeaderText = "Чит-ый зал";

            infoLabel.Text = $"Книги издательства \"{publishersComboBox.SelectedItem.ToString().Trim()}\":";
        }

        void OnShowPenaltyReadersButtonClick(object sender, EventArgs eventArgs)
        {
            secondDataSet.Clear();

            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT DISTINCT r.CARD_NUMBER, r.SURNAME, e.DATE, e.DELIVERY_DATE FROM EXTRADITION e, READERS r ");
            builder.Append($"WHERE e.DELIVERY_DATE < '{dateTimePicker.Value}' AND e.CARD_NUMBER = r.CARD_NUMBER");
            string query = builder.ToString();

            secondDataSet = SelectRows(secondDataSet, query);
            dataGridView.AutoGenerateColumns = true;
            dataGridView.DataSource = secondDataSet;
            dataGridView.DataMember = secondDataSet.Tables[0].TableName;
            dataGridView.ReadOnly = true;
            dataGridView.Columns[0].HeaderText = "Номер карты";
            dataGridView.Columns[0].Width = dataGridView.Width / 5;
            dataGridView.Columns[1].HeaderText = "Фамилия";
            dataGridView.Columns[1].Width = dataGridView.Width / 5;
            dataGridView.Columns[2].HeaderText = "Дата выдачи";
            dataGridView.Columns[2].Width = dataGridView.Width / 5;
            dataGridView.Columns[3].HeaderText = "Дата возврата";
            dataGridView.Columns[3].Width = dataGridView.Width / 5;

            infoLabel.Text = $"Книги издательства \"{publishersComboBox.SelectedItem.ToString().Trim()}\":";
        }

        void OnShowThemeReadersButtonClick(object sender, EventArgs eventArgs)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT SURNAME, CARD_NUMBER FROM READERS WHERE CARD_NUMBER in (");
            builder.Append("SELECT CARD_NUMBER FROM EXTRADITION WHERE LIBRARY_NUMBER in (");
            builder.Append("SELECT LIBRARY_NUMBER FROM BOOKS WHERE THEME_NUMBER = (");
            builder.Append($"SELECT ID FROM THEME WHERE NAME = '{themeReadersComboBox.SelectedItem}')))");
            string query = builder.ToString();

            thirdDataSet.Clear();
            thirdDataSet = SelectRows(thirdDataSet, query);
            dataGridView.AutoGenerateColumns = true;
            dataGridView.DataSource = thirdDataSet;
            dataGridView.DataMember = thirdDataSet.Tables[0].TableName;
            dataGridView.ReadOnly = true;
            dataGridView.Columns[0].HeaderText = "Фамилия";
            dataGridView.Columns[1].HeaderText = "Номер карты";
            dataGridView.Columns[1].Width = ClientSize.Width / 8;

            infoLabel.Text = $"Читатели по тематике \"{themeReadersComboBox.SelectedItem.ToString().Trim()}\":";
            infoLabel.Invalidate();
        }
    }
}
