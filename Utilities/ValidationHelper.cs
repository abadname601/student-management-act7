using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StudentManagementSystem.Utilities
{
    public class ValidationHelper
    {
        /// <summary>
        /// Validates that a text field is not empty
        /// </summary>
        public static bool IsNotEmpty(TextBox textBox, string fieldName, ErrorProvider errorProvider)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                errorProvider.SetError(textBox, $"{fieldName} is required");
                return false;
            }
            
            errorProvider.SetError(textBox, "");
            return true;
        }

        /// <summary>
        /// Validates that a combo box has a selected item
        /// </summary>
        public static bool HasSelectedItem(ComboBox comboBox, string fieldName, ErrorProvider errorProvider)
        {
            if (comboBox.SelectedItem == null || comboBox.SelectedIndex == -1)
            {
                errorProvider.SetError(comboBox, $"Please select a {fieldName}");
                return false;
            }
            
            errorProvider.SetError(comboBox, "");
            return true;
        }

        /// <summary>
        /// Validates that a numeric field contains a valid integer value within range
        /// </summary>
        public static bool IsValidInteger(TextBox textBox, string fieldName, ErrorProvider errorProvider, 
            int? minValue = null, int? maxValue = null)
        {
            if (!int.TryParse(textBox.Text, out int value))
            {
                errorProvider.SetError(textBox, $"{fieldName} must be a valid integer");
                return false;
            }
            
            if (minValue.HasValue && value < minValue.Value)
            {
                errorProvider.SetError(textBox, $"{fieldName} must be at least {minValue.Value}");
                return false;
            }
            
            if (maxValue.HasValue && value > maxValue.Value)
            {
                errorProvider.SetError(textBox, $"{fieldName} must be at most {maxValue.Value}");
                return false;
            }
            
            errorProvider.SetError(textBox, "");
            return true;
        }

        /// <summary>
        /// Validates that a numeric field contains a valid decimal value within range
        /// </summary>
        public static bool IsValidDecimal(TextBox textBox, string fieldName, ErrorProvider errorProvider, 
            decimal? minValue = null, decimal? maxValue = null)
        {
            if (!decimal.TryParse(textBox.Text, out decimal value))
            {
                errorProvider.SetError(textBox, $"{fieldName} must be a valid number");
                return false;
            }
            
            if (minValue.HasValue && value < minValue.Value)
            {
                errorProvider.SetError(textBox, $"{fieldName} must be at least {minValue.Value}");
                return false;
            }
            
            if (maxValue.HasValue && value > maxValue.Value)
            {
                errorProvider.SetError(textBox, $"{fieldName} must be at most {maxValue.Value}");
                return false;
            }
            
            errorProvider.SetError(textBox, "");
            return true;
        }

        /// <summary>
        /// Validates that a date picker has a valid date
        /// </summary>
        public static bool IsValidDate(DateTimePicker datePicker, string fieldName, ErrorProvider errorProvider, 
            DateTime? minDate = null, DateTime? maxDate = null)
        {
            DateTime value = datePicker.Value;
            
            if (minDate.HasValue && value < minDate.Value)
            {
                errorProvider.SetError(datePicker, $"{fieldName} must be on or after {minDate.Value.ToShortDateString()}");
                return false;
            }
            
            if (maxDate.HasValue && value > maxDate.Value)
            {
                errorProvider.SetError(datePicker, $"{fieldName} must be on or before {maxDate.Value.ToShortDateString()}");
                return false;
            }
            
            errorProvider.SetError(datePicker, "");
            return true;
        }

        /// <summary>
        /// Validates that an email address has a valid format
        /// </summary>
        public static bool IsValidEmail(TextBox textBox, ErrorProvider errorProvider)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            
            if (!regex.IsMatch(textBox.Text))
            {
                errorProvider.SetError(textBox, "Please enter a valid email address");
                return false;
            }
            
            errorProvider.SetError(textBox, "");
            return true;
        }

        /// <summary>
        /// Validates password strength (minimum 8 characters with at least one uppercase, one lowercase, one digit)
        /// </summary>
        public static bool IsStrongPassword(TextBox textBox, ErrorProvider errorProvider)
        {
            string password = textBox.Text;
            bool isValid = true;
            string error = "";
            
            if (password.Length < 8)
            {
                error = "Password must be at least 8 characters long";
                isValid = false;
            }
            else if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                error = "Password must contain at least one uppercase letter";
                isValid = false;
            }
            else if (!Regex.IsMatch(password, @"[a-z]"))
            {
                error = "Password must contain at least one lowercase letter";
                isValid = false;
            }
            else if (!Regex.IsMatch(password, @"[0-9]"))
            {
                error = "Password must contain at least one digit";
                isValid = false;
            }
            
            errorProvider.SetError(textBox, error);
            return isValid;
        }

        /// <summary>
        /// Validates that the two passwords match
        /// </summary>
        public static bool PasswordsMatch(TextBox passwordBox, TextBox confirmBox, ErrorProvider errorProvider)
        {
            if (passwordBox.Text != confirmBox.Text)
            {
                errorProvider.SetError(confirmBox, "Passwords do not match");
                return false;
            }
            
            errorProvider.SetError(confirmBox, "");
            return true;
        }
    }
}
