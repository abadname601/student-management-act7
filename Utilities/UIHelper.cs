using System;
using System.Windows.Forms;
using System.Drawing;

namespace StudentManagementSystem.Utilities
{
    public class UIHelper
    {
        /// <summary>
        /// Applies standard styling to a form
        /// </summary>
        public static void StyleForm(Form form)
        {
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.BackColor = SystemColors.Window;
            form.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            form.Icon = form.Owner?.Icon;
        }

        /// <summary>
        /// Applies standard styling to a DataGridView
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(87, 166, 212);
            dgv.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dgv.BackgroundColor = SystemColors.Window;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.RowHeadersWidth = 25;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
        }

        /// <summary>
        /// Styles common input controls
        /// </summary>
        public static void StyleInputControls(Control container)
        {
            foreach (Control control in container.Controls)
            {
                if (control is TextBox)
                {
                    var textBox = (TextBox)control;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.Padding = new Padding(3);
                }
                else if (control is ComboBox)
                {
                    var comboBox = (ComboBox)control;
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                else if (control is Button && control.Tag?.ToString() != "Skip")
                {
                    var button = (Button)control;
                    StyleButton(button);
                }
                else if (control.HasChildren)
                {
                    StyleInputControls(control);
                }
            }
        }

        /// <summary>
        /// Styles a button with standard colors and appearance
        /// </summary>
        public static void StyleButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.BackColor = Color.FromArgb(41, 128, 185);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.BorderColor = Color.FromArgb(36, 113, 163);
            
            // Add hover effect
            button.MouseEnter += (sender, e) => 
            {
                button.BackColor = Color.FromArgb(36, 113, 163);
            };
            
            button.MouseLeave += (sender, e) => 
            {
                button.BackColor = Color.FromArgb(41, 128, 185);
            };
        }

        /// <summary>
        /// Creates a primary action button
        /// </summary>
        public static Button CreatePrimaryButton(string text, Point location, Size size, EventHandler clickHandler)
        {
            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 1 },
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            
            button.Click += clickHandler;
            
            // Add hover effect
            button.MouseEnter += (sender, e) => 
            {
                button.BackColor = Color.FromArgb(36, 113, 163);
            };
            
            button.MouseLeave += (sender, e) => 
            {
                button.BackColor = Color.FromArgb(41, 128, 185);
            };
            
            return button;
        }

        /// <summary>
        /// Creates a secondary action button
        /// </summary>
        public static Button CreateSecondaryButton(string text, Point location, Size size, EventHandler clickHandler)
        {
            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 1 },
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.FromArgb(64, 64, 64),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            
            button.Click += clickHandler;
            
            // Add hover effect
            button.MouseEnter += (sender, e) => 
            {
                button.BackColor = Color.Gainsboro;
            };
            
            button.MouseLeave += (sender, e) => 
            {
                button.BackColor = Color.WhiteSmoke;
            };
            
            return button;
        }

        /// <summary>
        /// Configures an error provider with standard settings
        /// </summary>
        public static ErrorProvider ConfigureErrorProvider(Control owner)
        {
            ErrorProvider errorProvider = new ErrorProvider
            {
                BlinkStyle = ErrorBlinkStyle.NeverBlink,
                Icon = SystemIcons.Exclamation
            };
            
            return errorProvider;
        }

        /// <summary>
        /// Adds a watermark to a TextBox
        /// </summary>
        public static void SetWatermark(TextBox textBox, string watermarkText)
        {
            textBox.Text = watermarkText;
            textBox.ForeColor = Color.Gray;
            
            textBox.GotFocus += (sender, e) => 
            {
                if (textBox.Text == watermarkText)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };
            
            textBox.LostFocus += (sender, e) => 
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = watermarkText;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }
    }
}
