using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StudentManagement_act7
{
    public partial class Form1 : Form
    {
        string connectionString = "server=localhost;uid=root;pwd=;database=studentmanagement;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        void LoadStudents()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM students";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvStudents.DataSource = table;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO students (student_id, first_name, last_name, course) VALUES (@id, @fn, @ln, @course)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtStudentID.Text);
                cmd.Parameters.AddWithValue("@fn", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@ln", txtLastName.Text);
                cmd.Parameters.AddWithValue("@course", txtCourse.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Student added!");
                LoadStudents();
                ClearFields();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "UPDATE students SET first_name=@fn, last_name=@ln, course=@course WHERE student_id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtStudentID.Text);
                cmd.Parameters.AddWithValue("@fn", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@ln", txtLastName.Text);
                cmd.Parameters.AddWithValue("@course", txtCourse.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Student updated!");
                LoadStudents();
                ClearFields();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM students WHERE student_id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtStudentID.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Student deleted!");
                LoadStudents();
                ClearFields();
            }
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                txtStudentID.Text = row.Cells["student_id"].Value.ToString();
                txtFirstName.Text = row.Cells["first_name"].Value.ToString();
                txtLastName.Text = row.Cells["last_name"].Value.ToString();
                txtCourse.Text = row.Cells["course"].Value.ToString();
            }
        }

        void ClearFields()
        {
            txtStudentID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtCourse.Clear();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
