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
        public MySqlConnection? _connection;
        private bool _disposed = false;
        private readonly bool _inMemoryMode;

        // Connection string components
        private readonly string _server;
        private readonly string _database;
        private readonly string _port;
        private readonly string _userId;
        private readonly string _password;

        private const string DEFAULT_HOST = "localhost";
        private const string DEFAULT_DATABASE = "studentmanagement";
        private const string DEFAULT_PORT = "3307";
        private const string DEFAULT_USER = "root";
        private const string DEFAULT_PASSWORD = "";

        public DatabaseManager()
        {
            // Check whether to use in-memory mode
            string? testMode = Environment.GetEnvironmentVariable("DB_TEST_MODE");
            _inMemoryMode = !string.IsNullOrEmpty(testMode) && testMode.ToLower() == "true";

            // Initialize connection string components
            _server = Environment.GetEnvironmentVariable("DB_HOST") ?? DEFAULT_HOST;
            _database = Environment.GetEnvironmentVariable("DB_NAME") ?? DEFAULT_DATABASE;
            _port = Environment.GetEnvironmentVariable("DB_PORT") ?? DEFAULT_PORT;
            _userId = Environment.GetEnvironmentVariable("DB_USER") ?? DEFAULT_USER;
            _password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? DEFAULT_PASSWORD;

            if (!_inMemoryMode)
            {
                InitializeConnection();
            }
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
            // In-memory mode always returns true
            if (_inMemoryMode)
            {
                return true;
            }

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
                if (_connection != null && _connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public void OpenConnection()
        {
            if (_inMemoryMode)
                return;

            if (_connection != null && _connection.State == ConnectionState.Closed)
                _connection.Open();
        }

        public void CloseConnection()
        {
            if (_inMemoryMode)
                return;

            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }

        #region Student Methods

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            if (_inMemoryMode)
            {
                // Return test data
                students.Add(new Student { StudentId = 1, StudentName = "Alice Johnson", Email = "alice@gmail.com", DepartmentId = 1, DepartmentName = "Computer Science" });
                students.Add(new Student { StudentId = 2, StudentName = "Bob Smith", Email = "bob@gmail.com", DepartmentId = 2, DepartmentName = "Business Administration" });
                students.Add(new Student { StudentId = 3, StudentName = "Charlie Brown", Email = "charlie@gmail.com", DepartmentId = 3, DepartmentName = "Mechanical Engineering" });
                students.Add(new Student { StudentId = 4, StudentName = "David White", Email = "david@gmail.com", DepartmentId = 4, DepartmentName = "Electrical Engineering" });
                students.Add(new Student { StudentId = 5, StudentName = "Emma Wilson", Email = "emma@gmail.com", DepartmentId = 5, DepartmentName = "Mathematics" });
                return students;
            }

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

            if (_inMemoryMode)
            {
                // Return test data
                exams.Add(new Exam
                {
                    ExamId = 1,
                    CourseId = 1,
                    CourseName = "Introduction to Programming",
                    ExamDate = DateTime.Now.AddDays(-10),
                    Description = "Midterm Exam"
                });
                exams.Add(new Exam
                {
                    ExamId = 2,
                    CourseId = 2,
                    CourseName = "Database Management",
                    ExamDate = DateTime.Now.AddDays(-5),
                    Description = "Final Exam"
                });
                exams.Add(new Exam
                {
                    ExamId = 3,
                    CourseId = 3,
                    CourseName = "Data Structures and Algorithms",
                    ExamDate = DateTime.Now.AddDays(-2),
                    Description = "Quiz 1"
                });
                exams.Add(new Exam
                {
                    ExamId = 4,
                    CourseId = 4,
                    CourseName = "Web Development",
                    ExamDate = DateTime.Now.AddDays(-1),
                    Description = "Practical Test"
                });
                return exams;
            }

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

            if (_inMemoryMode)
            {
                // Return test data
                results.Add(new ExamResult
                {
                    ResultId = 1,
                    StudentId = 1,
                    ExamId = 1,
                    Score = 85.5m,
                    ExamName = "Midterm Exam - Introduction to Programming",
                    StudentName = "Alice Johnson"
                });
                results.Add(new ExamResult
                {
                    ResultId = 2,
                    StudentId = 2,
                    ExamId = 1,
                    Score = 78.0m,
                    ExamName = "Midterm Exam - Introduction to Programming",
                    StudentName = "Bob Smith"
                });
                results.Add(new ExamResult
                {
                    ResultId = 3,
                    StudentId = 1,
                    ExamId = 2,
                    Score = 92.0m,
                    ExamName = "Final Exam - Database Management",
                    StudentName = "Alice Johnson"
                });
                results.Add(new ExamResult
                {
                    ResultId = 4,
                    StudentId = 3,
                    ExamId = 3,
                    Score = 95.5m,
                    ExamName = "Quiz 1 - Data Structures and Algorithms",
                    StudentName = "Charlie Brown"
                });
                results.Add(new ExamResult
                {
                    ResultId = 5,
                    StudentId = 4,
                    ExamId = 4,
                    Score = 88.0m,
                    ExamName = "Practical Test - Web Development",
                    StudentName = "David White"
                });
                return results;
            }

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
            if (_inMemoryMode)
            {
                // Return test data based on the provided exam ID
                switch (examId)
                {
                    case 1:
                        return "Alice Johnson (Score: 85.5)";
                    case 2:
                        return "Alice Johnson (Score: 92.0)";
                    case 3:
                        return "Charlie Brown (Score: 95.5)";
                    case 4:
                        return "David White (Score: 88.0)";
                    default:
                        return "No results found for this exam.";
                }
            }

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

        public DataTable GetStudentListReport()
        {
            DataTable reportTable = new DataTable();

            if (_inMemoryMode)
            {
                // Create columns
                reportTable.Columns.Add("student_id", typeof(int));
                reportTable.Columns.Add("student_name", typeof(string));
                reportTable.Columns.Add("email", typeof(string));
                reportTable.Columns.Add("department_name", typeof(string));
                reportTable.Columns.Add("enrollment_count", typeof(int));

                // Add test data
                reportTable.Rows.Add(1, "Alice Johnson", "alice@gmail.com", "Computer Science", 2);
                reportTable.Rows.Add(2, "Bob Smith", "bob@gmail.com", "Business Administration", 1);
                reportTable.Rows.Add(3, "Charlie Brown", "charlie@gmail.com", "Mechanical Engineering", 1);
                reportTable.Rows.Add(4, "David White", "david@gmail.com", "Electrical Engineering", 1);
                reportTable.Rows.Add(5, "Emma Wilson", "emma@gmail.com", "Mathematics", 0);

                return reportTable;
            }

            string query = "SELECT s.student_id, s.student_name, s.email, d.department_name, " +
                           "COUNT(e.enrollment_id) as enrollment_count " +
                           "FROM students s " +
                           "LEFT JOIN departments d ON s.department_id = d.department_id " +
                           "LEFT JOIN enrollments e ON s.student_id = e.student_id " +
                           "GROUP BY s.student_id, s.student_name, s.email, d.department_name";

            try
            {
                OpenConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, _connection);
                adapter.Fill(reportTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating student list report: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return reportTable;
        }

        public DataTable GetCourseListReport()
        {
            DataTable reportTable = new DataTable();

            if (_inMemoryMode)
            {
                // Create columns
                reportTable.Columns.Add("course_id", typeof(int));
                reportTable.Columns.Add("course_name", typeof(string));
                reportTable.Columns.Add("department_name", typeof(string));
                reportTable.Columns.Add("professor_name", typeof(string));
                reportTable.Columns.Add("student_count", typeof(int));

                // Add test data
                reportTable.Rows.Add(1, "Introduction to Programming", "Computer Science", "Dr. Johnson", 15);
                reportTable.Rows.Add(2, "Database Management", "Computer Science", "Dr. Smith", 12);
                reportTable.Rows.Add(3, "Data Structures and Algorithms", "Computer Science", "Dr. Wilson", 10);
                reportTable.Rows.Add(4, "Web Development", "Computer Science", "Dr. Roberts", 18);
                reportTable.Rows.Add(5, "Business Ethics", "Business Administration", "Dr. Thompson", 25);

                return reportTable;
            }

            string query = "SELECT c.course_id, c.course_name, d.department_name, " +
                           "p.professor_name, COUNT(e.enrollment_id) as student_count " +
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
                throw new Exception("Error generating course list report: " + ex.Message);
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

            if (_inMemoryMode)
            {
                // Create columns
                reportTable.Columns.Add("exam_name", typeof(string));
                reportTable.Columns.Add("exam_date", typeof(DateTime));
                reportTable.Columns.Add("course_name", typeof(string));
                reportTable.Columns.Add("average_score", typeof(decimal));
                reportTable.Columns.Add("highest_score", typeof(decimal));
                reportTable.Columns.Add("lowest_score", typeof(decimal));
                reportTable.Columns.Add("student_count", typeof(int));

                // Add test data
                reportTable.Rows.Add("Midterm Exam - Introduction to Programming", DateTime.Now.AddDays(-10),
                                    "Introduction to Programming", 82.5, 91.0, 75.0, 2);
                reportTable.Rows.Add("Final Exam - Database Management", DateTime.Now.AddDays(-5),
                                    "Database Management", 88.0, 94.5, 81.5, 3);
                reportTable.Rows.Add("Quiz 1 - Data Structures and Algorithms", DateTime.Now.AddDays(-2),
                                    "Data Structures and Algorithms", 85.5, 95.5, 75.5, 5);
                reportTable.Rows.Add("Practical Test - Web Development", DateTime.Now.AddDays(-1),
                                    "Web Development", 79.8, 88.0, 71.5, 4);

                return reportTable;
            }

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

        public DataTable GetDepartmentSummaryReport()
        {
            DataTable reportTable = new DataTable();

            if (_inMemoryMode)
            {
                // Create columns
                reportTable.Columns.Add("department_name", typeof(string));
                reportTable.Columns.Add("student_count", typeof(int));
                reportTable.Columns.Add("course_count", typeof(int));
                reportTable.Columns.Add("professor_count", typeof(int));
                reportTable.Columns.Add("avg_exam_score", typeof(decimal));

                // Add test data
                reportTable.Rows.Add("Computer Science", 25, 8, 5, 84.5);
                reportTable.Rows.Add("Business Administration", 30, 6, 4, 79.8);
                reportTable.Rows.Add("Mechanical Engineering", 20, 10, 6, 82.3);
                reportTable.Rows.Add("Electrical Engineering", 18, 7, 5, 80.7);
                reportTable.Rows.Add("Mathematics", 15, 5, 3, 86.2);

                return reportTable;
            }

            string query = "SELECT d.department_name, " +
                           "COUNT(DISTINCT s.student_id) as student_count, " +
                           "COUNT(DISTINCT c.course_id) as course_count, " +
                           "COUNT(DISTINCT p.professor_id) as professor_count, " +
                           "AVG(er.score) as avg_exam_score " +
                           "FROM departments d " +
                           "LEFT JOIN students s ON d.department_id = s.department_id " +
                           "LEFT JOIN courses c ON d.department_id = c.department_id " +
                           "LEFT JOIN professors p ON d.department_id = p.department_id " +
                           "LEFT JOIN exams e ON c.course_id = e.course_id " +
                           "LEFT JOIN examresults er ON e.exam_id = er.exam_id " +
                           "GROUP BY d.department_name";

            try
            {
                OpenConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, _connection);
                adapter.Fill(reportTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating department summary report: " + ex.Message);
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
