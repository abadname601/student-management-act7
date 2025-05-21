-- phpMyAdmin SQL Dump
-- version 5.2.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3307
-- Generation Time: May 13, 2025 at 01:48 AM
-- Server version: 8.4.3
-- PHP Version: 8.3.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `studentmanagement`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `EnrollStudent` (IN `student_id` INT, IN `course_id` INT)   BEGIN
    INSERT INTO enrollments (student_id, course_id, enrollment_date) 
    VALUES (student_id, course_id, CURDATE());
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetTopStudents` ()   BEGIN
    DECLARE done INT DEFAULT 0;
    DECLARE student_name VARCHAR(100);
    DECLARE student_score DECIMAL(5,2);
    DECLARE cur CURSOR FOR 
    SELECT s.student_name, AVG(ex.score) 
    FROM students s
    JOIN examresults ex ON s.student_id = ex.student_id
    GROUP BY s.student_id
    HAVING AVG(ex.score) > 90;
    
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
    
    OPEN cur;
    read_loop: LOOP
        FETCH cur INTO student_name, student_score;
        IF done THEN
            LEAVE read_loop;
        END IF;
        SELECT CONCAT(student_name, ' - ', student_score) AS TopStudent;
    END LOOP;
    
    CLOSE cur;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ListEnrolledStudents` (IN `course_id` INT)   BEGIN
    DECLARE done INT DEFAULT 0;
    DECLARE student_name VARCHAR(100);
    DECLARE cur CURSOR FOR 
    SELECT s.student_name 
    FROM students s
    JOIN enrollments e ON s.student_id = e.student_id
    WHERE e.course_id = course_id;
    
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
    
    OPEN cur;
    read_loop: LOOP
        FETCH cur INTO student_name;
        IF done THEN
            LEAVE read_loop;
        END IF;
        SELECT student_name AS EnrolledStudent;
    END LOOP;
    
    CLOSE cur;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ShowTopScorer` (IN `exam_id` INT)   BEGIN
    DECLARE top_scorer VARCHAR(100);
    SET top_scorer = GetTopScorerExam(exam_id);
    SELECT CONCAT('Top scorer in exam ', exam_id, ' is ', top_scorer) AS Result;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ShowTotalEnrolled` (IN `course_id` INT)   BEGIN
    DECLARE total INT;
    SET total = CalculateTotalEnrolled(course_id);
    SELECT CONCAT('Total students enrolled in course ', course_id, ': ', total) AS Result;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateStudentEmail` (IN `student_id` INT, IN `new_email` VARCHAR(100))   BEGIN
    UPDATE students
    SET email = new_email
    WHERE student_id = student_id;
END$$

--
-- Functions
--
CREATE DEFINER=`root`@`localhost` FUNCTION `CalculateAverageScore` (`student_id` INT) RETURNS DOUBLE DETERMINISTIC BEGIN
    DECLARE avg_score DOUBLE;
    SELECT AVG(score) INTO avg_score FROM examresults WHERE student_id = student_id;
    RETURN COALESCE(avg_score, 0);
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `CalculateTotalEnrolled` (`course_id` INT) RETURNS INT DETERMINISTIC BEGIN
    DECLARE total INT;
    SELECT COUNT(*) INTO total FROM enrollments WHERE course_id = course_id;
    RETURN total;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `GetStudentInitials` (`student_id` INT) RETURNS CHAR(3) CHARSET utf8mb4 COLLATE utf8mb4_general_ci DETERMINISTIC BEGIN
    DECLARE initials CHAR(3);
    
    SELECT UPPER(CONCAT(LEFT(student_name, 1), MID(student_name, INSTR(student_name, ' ') + 1, 1))) 
    INTO initials
    FROM students 
    WHERE students.student_id = student_id
    LIMIT 1;  -- Ensures only one row is returned

    RETURN initials;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `GetTopScorerExam` (`exam_id` INT) RETURNS VARCHAR(100) CHARSET utf8mb4 COLLATE utf8mb4_general_ci DETERMINISTIC BEGIN
    DECLARE top_scorer VARCHAR(100);
    SELECT student_name INTO top_scorer
    FROM students
    JOIN examresults ON students.student_id = examresults.student_id
    WHERE examresults.exam_id = exam_id
    ORDER BY examresults.score DESC
    LIMIT 1;
    RETURN top_scorer;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `GetTotalStudents` () RETURNS INT DETERMINISTIC BEGIN
    DECLARE total_students INT;
    SELECT COUNT(*) INTO total_students FROM students;
    RETURN total_students;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `audit_log`
--

CREATE TABLE `audit_log` (
  `id` int NOT NULL,
  `action_type` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `table_name` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `affected_id` int DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `audit_log`
--

INSERT INTO `audit_log` (`id`, `action_type`, `table_name`, `affected_id`, `timestamp`) VALUES
(1, 'INSERT', 'students', 101, '2025-03-24 13:57:15'),
(2, 'DELETE', 'students', 101, '2025-03-24 14:04:27'),
(3, 'INSERT', 'students', 101, '2025-03-24 14:05:04'),
(4, 'DELETE', 'students', 101, '2025-03-24 14:11:17'),
(5, 'INSERT', 'students', 101, '2025-03-24 14:12:12'),
(6, 'INSERT', 'students', 102, '2025-03-24 14:20:34'),
(7, 'UPDATE', 'students', 101, '2025-03-24 14:24:36'),
(8, 'UPDATE', 'students', 101, '2025-03-24 14:27:42'),
(9, 'DELETE', 'students', 102, '2025-03-24 14:29:54'),
(10, 'DELETE', 'students', 101, '2025-03-24 14:32:09');

-- --------------------------------------------------------

--
-- Table structure for table `courses`
--

CREATE TABLE `courses` (
  `course_id` int NOT NULL,
  `course_name` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `department_id` int DEFAULT NULL,
  `professor_id` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `courses`
--

INSERT INTO `courses` (`course_id`, `course_name`, `department_id`, `professor_id`) VALUES
(1, 'Data Structures', 1, 1),
(2, 'Business Ethics', 2, 2),
(3, 'Thermodynamics', 3, 3),
(4, 'Circuit Analysis', 4, 4),
(5, 'Calculus III', 5, 5),
(6, 'Quantum Mechanics', 6, 6),
(7, 'Organic Chemistry', 7, 7),
(8, 'Machine Learning', 1, 8),
(9, 'Financial Management', 2, 9),
(10, 'Robotics', 3, 10);

-- --------------------------------------------------------

--
-- Table structure for table `departments`
--

CREATE TABLE `departments` (
  `department_id` int NOT NULL,
  `department_name` varchar(100) COLLATE utf8mb4_general_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `departments`
--

INSERT INTO `departments` (`department_id`, `department_name`) VALUES
(1, 'Computer Science'),
(2, 'Business Administration'),
(3, 'Mechanical Engineering'),
(4, 'Electrical Engineering'),
(5, 'Mathematics'),
(6, 'Physics'),
(7, 'Chemistry');

-- --------------------------------------------------------

--
-- Table structure for table `enrollments`
--

CREATE TABLE `enrollments` (
  `enrollment_id` int NOT NULL,
  `student_id` int DEFAULT NULL,
  `course_id` int DEFAULT NULL,
  `enrollment_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `enrollments`
--

INSERT INTO `enrollments` (`enrollment_id`, `student_id`, `course_id`, `enrollment_date`) VALUES
(1, 1, 1, '2024-01-15'),
(2, 2, 2, '2024-01-16'),
(3, 3, 3, '2024-01-17'),
(4, 4, 4, '2024-01-18'),
(5, 5, 5, '2024-01-19'),
(6, 6, 6, '2024-01-20'),
(7, 7, 7, '2024-01-21'),
(8, 8, 8, '2024-01-22'),
(9, 9, 9, '2024-01-23'),
(10, 10, 10, '2024-01-24');

-- --------------------------------------------------------

--
-- Table structure for table `examresults`
--

CREATE TABLE `examresults` (
  `result_id` int NOT NULL,
  `student_id` int DEFAULT NULL,
  `exam_id` int DEFAULT NULL,
  `score` decimal(5,2) NOT NULL,
  `exam_name` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `examresults`
--

INSERT INTO `examresults` (`result_id`, `student_id`, `exam_id`, `score`, `exam_name`) VALUES
(1, 1, 1, 85.50, 'Math Exam'),
(2, 2, 2, 78.00, 'Science Quiz'),
(3, 3, 3, 92.30, 'English Test'),
(4, 4, 4, 88.70, 'History Exam'),
(5, 5, 5, 76.50, 'Physics Assessment'),
(6, 6, 6, 89.10, 'Chemistry Practical'),
(7, 7, 7, 95.00, 'Biology Midterm'),
(8, 8, 8, 82.40, 'Computer Science Test'),
(9, 9, 9, 79.90, 'Economics Final'),
(10, 10, 10, 90.20, 'Geography Quiz');

-- --------------------------------------------------------

--
-- Table structure for table `exams`
--

CREATE TABLE `exams` (
  `exam_id` int NOT NULL,
  `course_id` int DEFAULT NULL,
  `exam_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `exams`
--

INSERT INTO `exams` (`exam_id`, `course_id`, `exam_date`) VALUES
(1, 1, '2024-02-01'),
(2, 2, '2024-02-02'),
(3, 3, '2024-02-03'),
(4, 4, '2024-02-04'),
(5, 5, '2024-02-05'),
(6, 6, '2024-02-06'),
(7, 7, '2024-02-07'),
(8, 8, '2024-02-08'),
(9, 9, '2024-02-09'),
(10, 10, '2024-02-10');

-- --------------------------------------------------------

--
-- Table structure for table `logs`
--

CREATE TABLE `logs` (
  `id` int NOT NULL,
  `action` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `description` text COLLATE utf8mb4_general_ci,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `logs`
--

INSERT INTO `logs` (`id`, `action`, `description`, `timestamp`) VALUES
(1, 'DELETE ATTEMPT', 'Trying to delete student ID: 101', '2025-03-24 14:04:27'),
(2, 'DELETE ATTEMPT', 'Trying to delete student ID: 101', '2025-03-24 14:11:17'),
(3, 'DELETE ATTEMPT', 'Trying to delete student ID: 102', '2025-03-24 14:29:54'),
(4, 'DELETE ATTEMPT', 'Trying to delete student ID: 101', '2025-03-24 14:32:09');

-- --------------------------------------------------------

--
-- Table structure for table `professors`
--

CREATE TABLE `professors` (
  `professor_id` int NOT NULL,
  `professor_name` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `department_id` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `professors`
--

INSERT INTO `professors` (`professor_id`, `professor_name`, `department_id`) VALUES
(1, 'Dr. John Doe', 1),
(2, 'Dr. Sarah Connor', 2),
(3, 'Dr. Alan Turing', 3),
(4, 'Dr. Nikola Tesla', 4),
(5, 'Dr. Albert Einstein', 5),
(6, 'Dr. Marie Curie', 6),
(7, 'Dr. Richard Feynman', 7),
(8, 'Dr. Jane Smith', 1),
(9, 'Dr. Robert Oppenheimer', 2),
(10, 'Dr. Elon Musk', 3);

-- --------------------------------------------------------

--
-- Table structure for table `students`
--

CREATE TABLE `students` (
  `student_id` int NOT NULL,
  `student_name` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `email` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `department_id` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `students`
--

INSERT INTO `students` (`student_id`, `student_name`, `email`, `department_id`) VALUES
(1, 'Alice Johnson', 'alice@gmail.com', 1),
(2, 'Bob Smith', 'bob@gmail.com', 2),
(3, 'Charlie Brown', 'charlie@gmail.com', 3),
(4, 'David White', 'david@gmail.com', 4),
(5, 'Emma Wilson', 'emma@gmail.com', 5),
(6, 'Frank Thomas', 'frank@gmail.com', 6),
(7, 'Grace Miller', 'grace@gmail.com', 7),
(8, 'Hannah Lee', 'hannah@gmail.com', 1),
(9, 'Isaac Scott', 'isaac@gmail.com', 2),
(10, 'Jack Turner', 'jack@gmail.com', 3);

--
-- Triggers `students`
--
DELIMITER $$
CREATE TRIGGER `after_student_delete` AFTER DELETE ON `students` FOR EACH ROW INSERT INTO audit_log (action_type, table_name, affected_id)
VALUES ('DELETE', 'students', OLD.student_id)
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `after_student_insert` AFTER INSERT ON `students` FOR EACH ROW INSERT INTO audit_log (action_type, table_name, affected_id)
VALUES ('INSERT', 'students', NEW.student_id)
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `after_student_update` AFTER UPDATE ON `students` FOR EACH ROW INSERT INTO audit_log (action_type, table_name, affected_id)
VALUES ('UPDATE', 'students', NEW.student_id)
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `before_student_delete` BEFORE DELETE ON `students` FOR EACH ROW INSERT INTO logs (action, description)
VALUES ('DELETE ATTEMPT', CONCAT('Trying to delete student ID: ', OLD.student_id))
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `before_student_insert` BEFORE INSERT ON `students` FOR EACH ROW SET NEW.student_name = UPPER(NEW.student_name)
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `before_student_update` BEFORE UPDATE ON `students` FOR EACH ROW SET NEW.student_name = UPPER(NEW.student_name)
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_course_assignments`
-- (See below for the actual view)
--
CREATE TABLE `view_course_assignments` (
`course_id` int
,`course_name` varchar(100)
,`professor_id` int
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_department_statistics`
-- (See below for the actual view)
--
CREATE TABLE `view_department_statistics` (
`department_id` int
,`department_name` varchar(100)
,`total_students` bigint
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_enrollment_summary`
-- (See below for the actual view)
--
CREATE TABLE `view_enrollment_summary` (
`student_id` int
,`student_name` varchar(100)
,`course_name` varchar(100)
,`enrollment_date` date
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_student_average`
-- (See below for the actual view)
--
CREATE TABLE `view_student_average` (
`student_id` int
,`student_name` varchar(100)
,`avg_score` double
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_student_details`
-- (See below for the actual view)
--
CREATE TABLE `view_student_details` (
`student_id` int
,`student_name` varchar(100)
,`email` varchar(100)
);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `audit_log`
--
ALTER TABLE `audit_log`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `courses`
--
ALTER TABLE `courses`
  ADD PRIMARY KEY (`course_id`),
  ADD KEY `department_id` (`department_id`),
  ADD KEY `professor_id` (`professor_id`);

--
-- Indexes for table `departments`
--
ALTER TABLE `departments`
  ADD PRIMARY KEY (`department_id`);

--
-- Indexes for table `enrollments`
--
ALTER TABLE `enrollments`
  ADD PRIMARY KEY (`enrollment_id`),
  ADD KEY `student_id` (`student_id`),
  ADD KEY `course_id` (`course_id`);

--
-- Indexes for table `examresults`
--
ALTER TABLE `examresults`
  ADD PRIMARY KEY (`result_id`),
  ADD KEY `student_id` (`student_id`),
  ADD KEY `exam_id` (`exam_id`);

--
-- Indexes for table `exams`
--
ALTER TABLE `exams`
  ADD PRIMARY KEY (`exam_id`),
  ADD KEY `course_id` (`course_id`);

--
-- Indexes for table `logs`
--
ALTER TABLE `logs`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `professors`
--
ALTER TABLE `professors`
  ADD PRIMARY KEY (`professor_id`),
  ADD KEY `department_id` (`department_id`);

--
-- Indexes for table `students`
--
ALTER TABLE `students`
  ADD PRIMARY KEY (`student_id`),
  ADD UNIQUE KEY `email` (`email`),
  ADD KEY `department_id` (`department_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `audit_log`
--
ALTER TABLE `audit_log`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `courses`
--
ALTER TABLE `courses`
  MODIFY `course_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `departments`
--
ALTER TABLE `departments`
  MODIFY `department_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `enrollments`
--
ALTER TABLE `enrollments`
  MODIFY `enrollment_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `examresults`
--
ALTER TABLE `examresults`
  MODIFY `result_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `exams`
--
ALTER TABLE `exams`
  MODIFY `exam_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `logs`
--
ALTER TABLE `logs`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `professors`
--
ALTER TABLE `professors`
  MODIFY `professor_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `students`
--
ALTER TABLE `students`
  MODIFY `student_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=103;

-- --------------------------------------------------------

--
-- Structure for view `view_course_assignments`
--
DROP TABLE IF EXISTS `view_course_assignments`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_course_assignments`  AS SELECT `courses`.`course_id` AS `course_id`, `courses`.`course_name` AS `course_name`, `courses`.`professor_id` AS `professor_id` FROM `courses`WITH CASCADED CHECK OPTION  ;

-- --------------------------------------------------------

--
-- Structure for view `view_department_statistics`
--
DROP TABLE IF EXISTS `view_department_statistics`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_department_statistics`  AS SELECT `d`.`department_id` AS `department_id`, `d`.`department_name` AS `department_name`, count(`s`.`student_id`) AS `total_students` FROM (`departments` `d` left join `students` `s` on((`d`.`department_id` = `s`.`department_id`))) GROUP BY `d`.`department_id` ;

-- --------------------------------------------------------

--
-- Structure for view `view_enrollment_summary`
--
DROP TABLE IF EXISTS `view_enrollment_summary`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_enrollment_summary`  AS SELECT `s`.`student_id` AS `student_id`, `s`.`student_name` AS `student_name`, `c`.`course_name` AS `course_name`, `e`.`enrollment_date` AS `enrollment_date` FROM ((`students` `s` join `enrollments` `e` on((`s`.`student_id` = `e`.`student_id`))) join `courses` `c` on((`e`.`course_id` = `c`.`course_id`))) ;

-- --------------------------------------------------------

--
-- Structure for view `view_student_average`
--
DROP TABLE IF EXISTS `view_student_average`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_student_average`  AS SELECT `students`.`student_id` AS `student_id`, `students`.`student_name` AS `student_name`, `CalculateAverageScore`(`students`.`student_id`) AS `avg_score` FROM `students` ;

-- --------------------------------------------------------

--
-- Structure for view `view_student_details`
--
DROP TABLE IF EXISTS `view_student_details`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_student_details`  AS SELECT `students`.`student_id` AS `student_id`, `students`.`student_name` AS `student_name`, `students`.`email` AS `email` FROM `students`WITH CASCADED CHECK OPTION  ;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `courses`
--
ALTER TABLE `courses`
  ADD CONSTRAINT `courses_ibfk_1` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `courses_ibfk_2` FOREIGN KEY (`professor_id`) REFERENCES `professors` (`professor_id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `enrollments`
--
ALTER TABLE `enrollments`
  ADD CONSTRAINT `enrollments_ibfk_1` FOREIGN KEY (`student_id`) REFERENCES `students` (`student_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `enrollments_ibfk_2` FOREIGN KEY (`course_id`) REFERENCES `courses` (`course_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `examresults`
--
ALTER TABLE `examresults`
  ADD CONSTRAINT `examresults_ibfk_1` FOREIGN KEY (`student_id`) REFERENCES `students` (`student_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `examresults_ibfk_2` FOREIGN KEY (`exam_id`) REFERENCES `exams` (`exam_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `exams`
--
ALTER TABLE `exams`
  ADD CONSTRAINT `exams_ibfk_1` FOREIGN KEY (`course_id`) REFERENCES `courses` (`course_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `professors`
--
ALTER TABLE `professors`
  ADD CONSTRAINT `professors_ibfk_1` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`) ON UPDATE CASCADE;

--
-- Constraints for table `students`
--
ALTER TABLE `students`
  ADD CONSTRAINT `students_ibfk_1` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
