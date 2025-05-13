using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Database
{
    public class DatabaseManager : IDisposable
    {
        private MySqlConnection _connection;
        private bool _disposed = false;

        // Connection string components
        private string _server = "localhost";
        private string _database = "studentmanagement";
        private string _port = "3307"; // Based on the SQL dump file
        private string _userId = "root"; // Default, should be changed for production
        private string _password = ""; // Default, should be changed for production

        public DatabaseManager()
        {
            InitializeConnection();
        }

        public DatabaseManager(string server, string database, string port, string userId, string password)
        {
            _server = server;
            _database = database;
            _port = port;
            _userId = userId;
            _password = password;
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            string connectionString = $"Server={_server};Database={_database};Port={_port};User ID={_userId};Password={_password};";
            _connection = new MySqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }

        #region Student Methods

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            string query = "SELECT s.*, d.department_name FROM students s " +
                           "LEFT JOIN departments d ON s.department_id = d.department_id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            StudentId = Convert.ToInt32(reader["student_id"]),
                            StudentName = reader["student_name"].ToString(),
                            Email = reader["email"].ToString(),
                            DepartmentId = reader["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["department_id"]),
                            DepartmentName = reader["department_name"] == DBNull.Value ? null : reader["department_name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving students: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return students;
        }

        public Student GetStudentById(int id)
        {
            Student student = null;
            string query = "SELECT s.*, d.department_name FROM students s " +
                           "LEFT JOIN departments d ON s.department_id = d.department_id " +
                           "WHERE s.student_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        student = new Student
                        {
                            StudentId = Convert.ToInt32(reader["student_id"]),
                            StudentName = reader["student_name"].ToString(),
                            Email = reader["email"].ToString(),
                            DepartmentId = reader["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["department_id"]),
                            DepartmentName = reader["department_name"] == DBNull.Value ? null : reader["department_name"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving student: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return student;
        }

        public bool AddStudent(Student student)
        {
            string query = "INSERT INTO students (student_id, student_name, email, department_id) VALUES (@id, @name, @email, @deptId)";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", student.StudentId);
                cmd.Parameters.AddWithValue("@name", student.StudentName);
                cmd.Parameters.AddWithValue("@email", student.Email);
                cmd.Parameters.AddWithValue("@deptId", student.DepartmentId == null ? DBNull.Value : (object)student.DepartmentId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding student: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool UpdateStudent(Student student)
        {
            string query = "UPDATE students SET student_name = @name, email = @email, department_id = @deptId WHERE student_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", student.StudentId);
                cmd.Parameters.AddWithValue("@name", student.StudentName);
                cmd.Parameters.AddWithValue("@email", student.Email);
                cmd.Parameters.AddWithValue("@deptId", student.DepartmentId == null ? DBNull.Value : (object)student.DepartmentId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating student: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DeleteStudent(int studentId)
        {
            string query = "DELETE FROM students WHERE student_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", studentId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting student: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public double GetStudentAverageScore(int studentId)
        {
            double avgScore = 0;
            string query = "SELECT CalculateAverageScore(@studentId) as average";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@studentId", studentId);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    avgScore = Convert.ToDouble(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error calculating average score: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return avgScore;
        }

        #endregion

        #region Course Methods

        public List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();
            string query = "SELECT c.*, d.department_name, p.professor_name FROM courses c " +
                           "LEFT JOIN departments d ON c.department_id = d.department_id " +
                           "LEFT JOIN professors p ON c.professor_id = p.professor_id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(new Course
                        {
                            CourseId = Convert.ToInt32(reader["course_id"]),
                            CourseName = reader["course_name"].ToString(),
                            DepartmentId = reader["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["department_id"]),
                            ProfessorId = reader["professor_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["professor_id"]),
                            DepartmentName = reader["department_name"] == DBNull.Value ? null : reader["department_name"].ToString(),
                            ProfessorName = reader["professor_name"] == DBNull.Value ? null : reader["professor_name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving courses: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return courses;
        }

        public Course GetCourseById(int id)
        {
            Course course = null;
            string query = "SELECT c.*, d.department_name, p.professor_name FROM courses c " +
                           "LEFT JOIN departments d ON c.department_id = d.department_id " +
                           "LEFT JOIN professors p ON c.professor_id = p.professor_id " +
                           "WHERE c.course_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        course = new Course
                        {
                            CourseId = Convert.ToInt32(reader["course_id"]),
                            CourseName = reader["course_name"].ToString(),
                            DepartmentId = reader["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["department_id"]),
                            ProfessorId = reader["professor_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["professor_id"]),
                            DepartmentName = reader["department_name"] == DBNull.Value ? null : reader["department_name"].ToString(),
                            ProfessorName = reader["professor_name"] == DBNull.Value ? null : reader["professor_name"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving course: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return course;
        }

        public bool AddCourse(Course course)
        {
            string query = "INSERT INTO courses (course_id, course_name, department_id, professor_id) " +
                           "VALUES (@id, @name, @deptId, @profId)";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", course.CourseId);
                cmd.Parameters.AddWithValue("@name", course.CourseName);
                cmd.Parameters.AddWithValue("@deptId", course.DepartmentId == null ? DBNull.Value : (object)course.DepartmentId);
                cmd.Parameters.AddWithValue("@profId", course.ProfessorId == null ? DBNull.Value : (object)course.ProfessorId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding course: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool UpdateCourse(Course course)
        {
            string query = "UPDATE courses SET course_name = @name, department_id = @deptId, " +
                           "professor_id = @profId WHERE course_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", course.CourseId);
                cmd.Parameters.AddWithValue("@name", course.CourseName);
                cmd.Parameters.AddWithValue("@deptId", course.DepartmentId == null ? DBNull.Value : (object)course.DepartmentId);
                cmd.Parameters.AddWithValue("@profId", course.ProfessorId == null ? DBNull.Value : (object)course.ProfessorId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating course: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DeleteCourse(int courseId)
        {
            string query = "DELETE FROM courses WHERE course_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", courseId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting course: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetTotalEnrolled(int courseId)
        {
            int total = 0;
            string query = "SELECT CalculateTotalEnrolled(@courseId) as total";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@courseId", courseId);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    total = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error calculating total enrolled: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return total;
        }

        #endregion

        #region Department Methods

        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();
            string query = "SELECT * FROM departments";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            DepartmentId = Convert.ToInt32(reader["department_id"]),
                            DepartmentName = reader["department_name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving departments: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return departments;
        }

        #endregion

        #region Professor Methods

        public List<Professor> GetAllProfessors()
        {
            List<Professor> professors = new List<Professor>();
            string query = "SELECT p.*, d.department_name FROM professors p " +
                           "LEFT JOIN departments d ON p.department_id = d.department_id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        professors.Add(new Professor
                        {
                            ProfessorId = Convert.ToInt32(reader["professor_id"]),
                            ProfessorName = reader["professor_name"].ToString(),
                            DepartmentId = reader["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["department_id"]),
                            DepartmentName = reader["department_name"] == DBNull.Value ? null : reader["department_name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving professors: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return professors;
        }

        #endregion

        #region Enrollment Methods

        public List<Enrollment> GetAllEnrollments()
        {
            List<Enrollment> enrollments = new List<Enrollment>();
            string query = "SELECT e.*, s.student_name, c.course_name FROM enrollments e " +
                           "LEFT JOIN students s ON e.student_id = s.student_id " +
                           "LEFT JOIN courses c ON e.course_id = c.course_id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(new Enrollment
                        {
                            EnrollmentId = Convert.ToInt32(reader["enrollment_id"]),
                            StudentId = reader["student_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["student_id"]),
                            CourseId = reader["course_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["course_id"]),
                            EnrollmentDate = Convert.ToDateTime(reader["enrollment_date"]),
                            StudentName = reader["student_name"] == DBNull.Value ? null : reader["student_name"].ToString(),
                            CourseName = reader["course_name"] == DBNull.Value ? null : reader["course_name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving enrollments: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return enrollments;
        }

        public bool EnrollStudent(int studentId, int courseId)
        {
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand("EnrollStudent", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", studentId);
                cmd.Parameters.AddWithValue("@course_id", courseId);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error enrolling student: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion

        #region Exam and Results Methods

        public List<Exam> GetAllExams()
        {
            List<Exam> exams = new List<Exam>();
            string query = "SELECT e.*, c.course_name FROM exams e " +
                           "LEFT JOIN courses c ON e.course_id = c.course_id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        exams.Add(new Exam
                        {
                            ExamId = Convert.ToInt32(reader["exam_id"]),
                            CourseId = reader["course_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["course_id"]),
                            ExamDate = Convert.ToDateTime(reader["exam_date"]),
                            CourseName = reader["course_name"] == DBNull.Value ? null : reader["course_name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving exams: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return exams;
        }

        public List<ExamResult> GetExamResults()
        {
            List<ExamResult> results = new List<ExamResult>();
            string query = "SELECT r.*, s.student_name FROM examresults r " +
                           "LEFT JOIN students s ON r.student_id = s.student_id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new ExamResult
                        {
                            ResultId = Convert.ToInt32(reader["result_id"]),
                            StudentId = reader["student_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["student_id"]),
                            ExamId = reader["exam_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["exam_id"]),
                            Score = Convert.ToDecimal(reader["score"]),
                            ExamName = reader["exam_name"] == DBNull.Value ? null : reader["exam_name"].ToString(),
                            StudentName = reader["student_name"] == DBNull.Value ? null : reader["student_name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving exam results: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return results;
        }

        public bool AddExamResult(ExamResult result)
        {
            string query = "INSERT INTO examresults (result_id, student_id, exam_id, score, exam_name) " +
                           "VALUES (@id, @studentId, @examId, @score, @examName)";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", result.ResultId);
                cmd.Parameters.AddWithValue("@studentId", result.StudentId == null ? DBNull.Value : (object)result.StudentId);
                cmd.Parameters.AddWithValue("@examId", result.ExamId == null ? DBNull.Value : (object)result.ExamId);
                cmd.Parameters.AddWithValue("@score", result.Score);
                cmd.Parameters.AddWithValue("@examName", result.ExamName == null ? DBNull.Value : (object)result.ExamName);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding exam result: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool UpdateExamResult(ExamResult result)
        {
            string query = "UPDATE examresults SET student_id = @studentId, exam_id = @examId, " +
                           "score = @score, exam_name = @examName WHERE result_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", result.ResultId);
                cmd.Parameters.AddWithValue("@studentId", result.StudentId == null ? DBNull.Value : (object)result.StudentId);
                cmd.Parameters.AddWithValue("@examId", result.ExamId == null ? DBNull.Value : (object)result.ExamId);
                cmd.Parameters.AddWithValue("@score", result.Score);
                cmd.Parameters.AddWithValue("@examName", result.ExamName == null ? DBNull.Value : (object)result.ExamName);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating exam result: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DeleteExamResult(int resultId)
        {
            string query = "DELETE FROM examresults WHERE result_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", resultId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting exam result: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public string GetTopScorerForExam(int examId)
        {
            string topScorer = "";
            string query = "SELECT GetTopScorerExam(@examId) as topScorer";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@examId", examId);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    topScorer = result.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving top scorer: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return topScorer;
        }

        #endregion

        #region User Methods

        public DataTable GetAllUsers()
        {
            DataTable usersTable = new DataTable();
            string query = "SELECT * FROM users";

            try
            {
                OpenConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, _connection);
                adapter.Fill(usersTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving users: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return usersTable;
        }

        public bool AddUser(string username, string passwordHash, string email, string role, string recoveryQuestion, string recoveryAnswer)
        {
            string query = "INSERT INTO users (username, password_hash, email, role, recovery_question, recovery_answer) " +
                           "VALUES (@username, @passwordHash, @email, @role, @question, @answer)";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@question", recoveryQuestion);
                cmd.Parameters.AddWithValue("@answer", recoveryAnswer);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding user: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool UpdateUser(int userId, string username, string email, string role)
        {
            string query = "UPDATE users SET username = @username, email = @email, role = @role WHERE user_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@role", role);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DeleteUser(int userId)
        {
            string query = "DELETE FROM users WHERE user_id = @id";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@id", userId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting user: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AuthenticateUser(string username, string passwordHash)
        {
            string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password_hash = @passwordHash";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public string GetRecoveryQuestion(string username)
        {
            string query = "SELECT recovery_question FROM users WHERE username = @username";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@username", username);

                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving recovery question: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool CheckRecoveryAnswer(string username, string answer)
        {
            string query = "SELECT COUNT(*) FROM users WHERE username = @username AND recovery_answer = @answer";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@answer", answer);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking recovery answer: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ResetPassword(string username, string newPasswordHash)
        {
            string query = "UPDATE users SET password_hash = @passwordHash WHERE username = @username";

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@passwordHash", newPasswordHash);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error resetting password: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion

        #region Report Methods

        public DataTable GetStudentPerformanceReport()
        {
            DataTable reportTable = new DataTable();
            string query = "SELECT s.student_id, s.student_name, s.email, d.department_name, " +
                           "AVG(e.score) as average_score, MAX(e.score) as highest_score, MIN(e.score) as lowest_score, " +
                           "COUNT(e.result_id) as exams_taken " +
                           "FROM students s " +
                           "LEFT JOIN departments d ON s.department_id = d.department_id " +
                           "LEFT JOIN examresults e ON s.student_id = e.student_id " +
                           "GROUP BY s.student_id, s.student_name, s.email, d.department_name";

            try
            {
                OpenConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, _connection);
                adapter.Fill(reportTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating student performance report: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return reportTable;
        }

        public DataTable GetCourseEnrollmentReport()
        {
            DataTable reportTable = new DataTable();
            string query = "SELECT c.course_id, c.course_name, d.department_name, p.professor_name, " +
                           "COUNT(e.enrollment_id) as enrollment_count " +
                           "FROM courses c " +
                           "LEFT JOIN departments d ON c.department_id = d.department_id " +
                           "LEFT JOIN professors p ON c.professor_id = p.professor_id " +
                           "LEFT JOIN enrollments e ON c.course_id = e.course_id " +
                           "GROUP BY c.course_id, c.course_name, d.department_name, p.professor_name";

            try
            {
                OpenConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, _connection);
                adapter.Fill(reportTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating course enrollment report: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return reportTable;
        }

        public DataTable GetExamResultsReport()
        {
            DataTable reportTable = new DataTable();
            string query = "SELECT er.exam_name, e.exam_date, c.course_name, " +
                           "AVG(er.score) as average_score, MAX(er.score) as highest_score, " +
                           "MIN(er.score) as lowest_score, COUNT(er.result_id) as student_count " +
                           "FROM examresults er " +
                           "LEFT JOIN exams e ON er.exam_id = e.exam_id " +
                           "LEFT JOIN courses c ON e.course_id = c.course_id " +
                           "GROUP BY er.exam_name, e.exam_date, c.course_name";

            try
            {
                OpenConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, _connection);
                adapter.Fill(reportTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating exam results report: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return reportTable;
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        if (_connection.State == ConnectionState.Open)
                        {
                            _connection.Close();
                        }
                        _connection.Dispose();
                    }
                }
                _disposed = true;
            }
        }
    }
}
