using System;
using System.Windows.Forms;
using BusinesLogical;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Logic Logic { get; set; }

        public Form1()
        {
            InitializeComponent();
            Logic = new Logic();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            Logic.AddStudent(textBox1.Text, textBox2.Text, textBox3.Text);
            UpdateListBox();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите студента!");
                return;
            }

            string selected = listBox1.SelectedItem.ToString();
            string name = selected.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

            if (Logic.DeleteStudent(name))
            {
                UpdateListBox();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
            else
            {
                MessageBox.Show("Ошибка удаления.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
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

            string selected = listBox1.SelectedItem.ToString();
            string oldName = selected.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

            if (Logic.UpdateStudent(oldName, textBox1.Text, textBox2.Text, textBox3.Text))
            {
                UpdateListBox();
            }
            else
            {
                MessageBox.Show("Ошибка изменения.");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;

            string[] parts = listBox1.SelectedItem.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 3)
            {
                textBox1.Text = parts[0];
                textBox2.Text = parts[1];
                textBox3.Text = parts[2];
            }
        }

        private void UpdateListBox()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Logic.GetAll());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            var groups = Logic.GroupBySpeciality(); // и тут ошибка Logic.GroupBySpeciality()

            foreach (var group in groups) // тут groups
            {
                listBox1.Items.Add($"=== Специальность: {group.Key} ==="); // тут {group.Key}
                foreach (var student in group.Value) // тут group.Value
                {
                    listBox1.Items.Add($"  {student.Name} - Группа: {student.Group}");
                }
                listBox1.Items.Add(""); // Пустая строка для разделения
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Введите группу в поле 2!");
                return;
            }

            listBox1.Items.Clear();

            var matches = Logic.GetStudentsByGroup(textBox2.Text); // тут Logic.GetStudentsByGroup

            if (matches.Count == 0) // тут matches.Count
            {
                MessageBox.Show("Студенты не найдены.");
                return;
            }

            listBox1.Items.Add($"=== Студенты группы {textBox2.Text} ===");
            foreach (var student in matches) // теперь тут ошибка а именно matches
            {
                listBox1.Items.Add($"{student.Name} - {student.Speciality}");
            }
        }
    }
}