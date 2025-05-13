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
            
            // Check if we should use in-memory mode
            bool useInMemoryMode = Environment.GetEnvironmentVariable("DB_TEST_MODE")?.ToLower() == "true";
            
            try
            {
                if (useInMemoryMode)
                {
                    // Use in-memory mode without attempting database connection
                    MessageBox.Show("Running in test mode with in-memory database.", 
                        "Test Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Run(new Forms.LoginForm());
                }
                else
                {
                    // Check if we can connect to the database
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
