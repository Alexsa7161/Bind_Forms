using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Serialization;

namespace Bind_Forms
{
    public partial class Info : Form
    {
        BindingSource bs = new BindingSource();
        public int num_books;
        public Info()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeData();
            InitializePictureBox();
            InitializePropertyGrid();
            InitializeChart();
            InitializeNavigatorBinding();
        }
        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;

            var cityNameColumn = new DataGridViewTextBoxColumn();
            cityNameColumn.DataPropertyName = "Name";
            cityNameColumn.HeaderText = "Имя";
            cityNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(cityNameColumn);

            var New_Column1 = new DataGridViewTextBoxColumn();
            New_Column1.DataPropertyName = "SurName";
            New_Column1.HeaderText = "Фамилия";
            New_Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(New_Column1);

            var New_Column2 = new DataGridViewTextBoxColumn();
            New_Column2.DataPropertyName = "Birthdate";
            New_Column2.HeaderText = "Дата рождения";
            New_Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(New_Column2);

            var New_Column3 = new DataGridViewTextBoxColumn();
            New_Column3.DataPropertyName = "Deathdate";
            New_Column3.HeaderText = "Дата смерти";
            New_Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(New_Column3);

            var New_Column4 = new DataGridViewTextBoxColumn();
            New_Column4.DataPropertyName = "Num_books";
            New_Column4.HeaderText = "Число книг";
            New_Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(New_Column4);

            var New_Column5 = new DataGridViewComboBoxColumn();
            New_Column5.DataPropertyName = "Gender";
            New_Column5.HeaderText = "Пол";
            New_Column5.DataSource = Enum.GetValues(typeof(Gender));
            New_Column5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(New_Column5);
        }
        private void InitializeData()
        {
            Writer w1 = new Writer("Александр", "Пушкин", "C:\\Users\\Alexsa7161\\source\\repos\\Bind Forms\\Images\\1638840756_2.jpg", new DateTime(1799, 5, 26), new DateTime(1837, 2, 10), 150,Gender.male);
            Writer w2 = new Writer("Александр", "Грибоедов", "C:\\Users\\Alexsa7161\\source\\repos\\Bind Forms\\Images\\dw8asu0xgaaekv5_1.jpg", new DateTime(1799, 5, 26), new DateTime(1795, 1, 15), 100,Gender.male);
            Writer w3 = new Writer("Сергей", "Лукьяненко", "C:\\Users\\Alexsa7161\\source\\repos\\Bind Forms\\Images\\lukyanenko-sergej.jpg", new DateTime(1968, 4, 11), null, 15,Gender.male);
            Writer w4 = new Writer("Чарльз", "Диккенс", "C:\\Users\\Alexsa7161\\source\\repos\\Bind Forms\\Images\\8af789afc21ce9e8320aba939ca6db8d.jpeg", new DateTime(1812, 2, 7), new DateTime(1870, 6, 9), 3, Gender.male);
            Writer w5 = new Writer("Джоан", "Роулинг", "C:\\Users\\Alexsa7161\\source\\repos\\Bind Forms\\Images\\Роулинг.jpeg", new DateTime(1965, 7, 31), null, 8, Gender.female);
            List<Writer> mas = new List<Writer>() { w1, w2, w3, w4, w5 };
            bs.DataSource = mas;
            dataGridView1.DataSource = bs;
        }
        private void InitializePictureBox()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.DataBindings.Add("ImageLocation", bs, "author_photo", true);
            pictureBox1.DoubleClick += pictureBox1_Click;
        }
        private void InitializePropertyGrid()
        {
            propertyGrid1.DataBindings.Add("SelectedObject", bs, "");
        }
        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "Картинка в формате jpg|*.jpg|Картинка в формате jpeg|*.jpeg"
            };
            if (opd.ShowDialog() == DialogResult.OK)
            {
                (bs.Current as Writer).author_photo = opd.FileName;
                bs.ResetBindings(false);
            }
        }
        private void InitializeChart()
        {
            chart1.DataSource = from u in bs.DataSource as List<Writer>
                               group u by u.StrEnum into g
                               select new { Type = g.Key, Avg = g.Average(u => u.num_books) };
            chart1.Series[0].XValueMember = "Type";
            chart1.Series[0].YValueMembers = "Avg";
            chart1.Legends.Clear();
            chart1.Titles.Add("Среднее число книг на пол");
            bs.CurrentChanged += (o, e) => chart1.DataBind();
        }
        private void InitializeNavigatorBinding()
        {
            bindingNavigator1.BindingSource = bs;
        }
        private void TextBox1_Changed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                // Получаем значение из ToolStripTextBox.
                var searchValue = textBox1.Text;

                // Очищаем выделение строк в DataGridView.
                dataGridView1.ClearSelection();
                // Выделяем строки в DataGridView, у которых значение выбранного столбца равно значению из ToolStripTextBox.
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataGridViewColumn column = dataGridView1.Columns[4];
                    if (row.Cells[column.Index].Value != null)
                    {
                        if (row.Cells[column.Index].Value != null && int.Parse(row.Cells[column.Index].Value.ToString()) >= int.Parse(searchValue))
                            row.Visible = true;
                        else
                        {
                            dataGridView1.CurrentCell = null;
                            row.Visible = false;
                        }
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    row.Visible = true;
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.InitialDirectory = System.Environment.CurrentDirectory;
            sfd.Filter = "Файл в bin|*.bin|Файл в xml|*.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var isBinaryFormat = sfd.FilterIndex == 1 ? true : false;
                SerializeAndSave(sfd.FileName, isBinaryFormat);
            }
        }

        private void SerializeAndSave(string file, bool isBinaryFormat)
        {
            using (var stream = new FileStream(file, FileMode.Create))
            {
                if (isBinaryFormat)
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, bs.DataSource);
                }
                else
                {
                    var ser = new XmlSerializer(typeof(List<Writer>));
                    ser.Serialize(stream, bs.DataSource);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = System.Environment.CurrentDirectory;
            ofd.Filter = "Файл в bin|*.bin|Файл в xml|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var isBinaryFormat = ofd.FilterIndex == 1 ? true : false;
                DeserializeAndLoad(ofd, isBinaryFormat);
            }
        }

        private void DeserializeAndLoad(OpenFileDialog ofd, bool isBinaryFormat)
        {
            using (var stream = new FileStream(ofd.FileName, FileMode.Open))
            {
                if (isBinaryFormat)
                {
                    var formatter = new BinaryFormatter();
                    try
                    {
                        bs.DataSource = (List<Writer>)formatter.Deserialize(stream);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Файл содержит ошибку");
                    }
                }
                else
                {
                    var ser = new XmlSerializer(typeof(List<Writer>));
                    try
                    {
                        bs.DataSource = (List<Writer>)ser.Deserialize(stream);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Файл содержит ошибку");
                    }

                }
            }
        }
    }
}