using System;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Check if we can connect to the database
            try
            {
                using (var db = new Database.DatabaseManager())
                {
                    if (db.TestConnection())
                    {
                        // Start with the login form
                        Application.Run(new Forms.LoginForm());
                    }
                    else
                    {
                        MessageBox.Show("Cannot connect to the database. Please check your MySQL connection settings.", 
                            "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Application Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}
