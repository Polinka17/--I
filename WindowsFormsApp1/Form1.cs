using BusinesLogical;
using Model;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Logic Logic { get; set; }
        private bool isGrouped = false;
        private bool isFiltered = false;

        public Form1()
        {
            InitializeComponent();
            Logic = new Logic();
            ConfigureDataGridView();
            UpdateDataGridView();
            this.Resize += Form1_Resize;
            UpdateToolStripButtons();

            button6.Click += new EventHandler(button6_Click);
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("Name", "ФИО");
            dataGridView1.Columns.Add("Group", "Группа");
            dataGridView1.Columns.Add("Speciality", "Специальность");

            UpdateColumnWidths();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void UpdateColumnWidths()
        {
            if (dataGridView1.Columns.Count >= 3)
            {
                dataGridView1.Columns["Name"].Width = (int)(dataGridView1.Width * 0.4);
                dataGridView1.Columns["Group"].Width = (int)(dataGridView1.Width * 0.2);
                dataGridView1.Columns["Speciality"].Width = (int)(dataGridView1.Width * 0.4);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            UpdateColumnWidths();
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            Logic.AddStudent(textBox1.Text, textBox2.Text, textBox3.Text);
            UpdateDataGridView();
            ClearTextBoxes();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите студента!");
                return;
            }

            string name = dataGridView1.SelectedRows[0].Cells["Name"].Value.ToString();

            if (Logic.DeleteStudent(name))
            {
                UpdateDataGridView();
                ClearTextBoxes();
            }
            else
            {
                MessageBox.Show("Ошибка удаления.");
            }
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите студента!");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Заполните поля!");
                return;
            }

            string oldName = dataGridView1.SelectedRows[0].Cells["Name"].Value.ToString();

            if (Logic.UpdateStudent(oldName, textBox1.Text, textBox2.Text, textBox3.Text))
            {
                UpdateDataGridView();
                ClearTextBoxes();
            }
            else
            {
                MessageBox.Show("Ошибка изменения.");
            }
        }

        private void tsbCancelEdit_Click(object sender, EventArgs e)
        {
            CancelEditing();
        }

        private void CancelEditing()
        {
            ClearTextBoxes();
            dataGridView1.ClearSelection();
        }

        // Группировка по специальности
        private void tsbGroup_Click(object sender, EventArgs e)
        {
            PerformGrouping();
        }

        private void PerformGrouping()
        {
            dataGridView1.Rows.Clear();

            var groups = Logic.GroupBySpeciality();
            if (groups == null || groups.Count == 0)
            {
                MessageBox.Show("Нет данных для группировки.");
                UpdateDataGridView();
                return;
            }

            // Отображаем всех студентов, сгруппированных по специальности
            // Все студены одной специальности идут друг за другом
            foreach (var group in groups)
            {
                foreach (var student in group.Value)
                {
                    dataGridView1.Rows.Add(
                        student.Name ?? "",
                        student.Group ?? "",
                        student.Speciality ?? ""
                    );
                }
            }

            isGrouped = true;
            isFiltered = false;
            UpdateToolStripButtons();
            ClearTextBoxes();
        }

        // Поиск по группе
        private void tsbSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Введите группу в поле 2!");
                return;
            }

            dataGridView1.Rows.Clear();

            var matches = Logic.GetStudentsByGroup(textBox2.Text);
            if (matches == null || matches.Count == 0)
            {
                MessageBox.Show("Студенты не найдены.");
                UpdateDataGridView();
                return;
            }

            // Отображаем студентов из нужной группы
            foreach (var student in matches)
            {
                dataGridView1.Rows.Add(
                    student.Name ?? "",
                    student.Group ?? "",
                    student.Speciality ?? ""
                );
            }

            isGrouped = false;
            isFiltered = true;
            UpdateToolStripButtons();
        }

        private void tsbReset_Click(object sender, EventArgs e)
        {
            ResetView();
        }

        private void ResetView()
        {
            UpdateDataGridView();

            if (isGrouped || isFiltered)
            {
                MessageBox.Show("Представление сброшено. Отображен полный список студентов.",
                    "Сброс", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            textBox1.Text = row.Cells["Name"].Value?.ToString()?.Trim() ?? "";
            textBox2.Text = row.Cells["Group"].Value?.ToString()?.Trim() ?? "";
            textBox3.Text = row.Cells["Speciality"].Value?.ToString()?.Trim() ?? "";
        }

        private void UpdateDataGridView()
        {
            dataGridView1.Rows.Clear();

            var students = Logic.GetAll();
            if (students == null || students.Count == 0)
            {
                isGrouped = false;
                isFiltered = false;
                UpdateToolStripButtons();
                return;
            }

            foreach (var student in students)
            {
                if (student != null)
                {
                    dataGridView1.Rows.Add(
                        student.Name ?? "",
                        student.Group ?? "",
                        student.Speciality ?? ""
                    );
                }
            }

            isGrouped = false;
            isFiltered = false;
            UpdateToolStripButtons();
        }

        private void ClearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void UpdateToolStripButtons()
        {
            button7.Enabled = (isGrouped || isFiltered);

            if (isGrouped)
            {
                button7.Text = "Сбросить группировку";
            }
            else if (isFiltered)
            {
                button7.Text = "Сбросить фильтр";
            }
            else
            {
                button7.Text = "Сброс";
            }
        }

        // Обработчики для кнопок формы
        private void button1_Click(object sender, EventArgs e)
        {
            tsbAdd_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tsbDelete_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tsbEdit_Click(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PerformGrouping();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CancelEditing();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            ResetView();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Обработчик клика по метке
        }
    }
}