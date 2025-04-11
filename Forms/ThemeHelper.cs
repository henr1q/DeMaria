using System.Drawing;
using System.Windows.Forms;

namespace DeMaria.Forms
{
    public static class ThemeHelper
    {
        // Color palette
        public static class Colors
        {
            public static Color Primary = Color.FromArgb(0, 120, 215);      // Modern blue
            public static Color Secondary = Color.FromArgb(240, 240, 240);  // Light gray
            public static Color Accent = Color.FromArgb(255, 255, 255);     // White
            public static Color Text = Color.FromArgb(51, 51, 51);          // Dark gray
            public static Color Error = Color.FromArgb(220, 53, 69);        // Red
            public static Color Success = Color.FromArgb(40, 167, 69);      // Green
            public static Color Warning = Color.FromArgb(255, 193, 7);      // Yellow
        }

        // Fonts
        public static class Fonts
        {
            public static Font Title = new Font("Segoe UI", 16, FontStyle.Bold);
            public static Font Header = new Font("Segoe UI", 12, FontStyle.Bold);
            public static Font Normal = new Font("Segoe UI", 10, FontStyle.Regular);
            public static Font Small = new Font("Segoe UI", 9, FontStyle.Regular);
        }

        // Button styles
        public static void ApplyButtonStyle(Button button, bool isPrimary = false)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = isPrimary ? Colors.Primary : Colors.Secondary;
            button.BackColor = isPrimary ? Colors.Primary : Colors.Secondary;
            button.ForeColor = isPrimary ? Colors.Accent : Colors.Text;
            button.Font = Fonts.Normal;
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(10, 5, 10, 5);

            // Hover effects
            button.MouseEnter += (s, e) => {
                button.BackColor = isPrimary ? Color.FromArgb(0, 102, 204) : Color.FromArgb(230, 230, 230);
            };
            button.MouseLeave += (s, e) => {
                button.BackColor = isPrimary ? Colors.Primary : Colors.Secondary;
            };
        }

        // TextBox styles
        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = Fonts.Normal;
            textBox.BackColor = Colors.Accent;
            textBox.ForeColor = Colors.Text;
            textBox.Padding = new Padding(5);
        }

        // DateTimePicker styles
        public static void ApplyDateTimePickerStyle(DateTimePicker dateTimePicker)
        {
            dateTimePicker.Font = Fonts.Normal;
            dateTimePicker.CalendarForeColor = Colors.Text;
            dateTimePicker.CalendarTitleForeColor = Colors.Primary;
            dateTimePicker.CalendarTrailingForeColor = Colors.Secondary;
        }

        // Label styles
        public static void ApplyLabelStyle(Label label, bool isHeader = false)
        {
            label.Font = isHeader ? Fonts.Header : Fonts.Normal;
            label.ForeColor = Colors.Text;
        }

        // Form styles
        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = Colors.Accent;
            form.Font = Fonts.Normal;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;
        }

        // MenuStrip styles
        public static void ApplyMenuStripStyle(MenuStrip menuStrip)
        {
            menuStrip.BackColor = Colors.Primary;
            menuStrip.ForeColor = Colors.Accent;
            menuStrip.Font = Fonts.Normal;
            menuStrip.Renderer = new ModernMenuRenderer();
        }

        // DataGridView styles
        public static void ApplyDataGridViewStyle(DataGridView grid)
        {
            grid.BackgroundColor = Colors.Accent;
            grid.BorderStyle = BorderStyle.None;
            grid.Font = Fonts.Normal;
            grid.ForeColor = Colors.Text;
            grid.GridColor = Colors.Secondary;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Colors.Primary;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Colors.Accent;
            grid.ColumnHeadersDefaultCellStyle.Font = Fonts.Header;
            grid.ColumnHeadersHeight = 40;
            grid.RowTemplate.Height = 35;
            grid.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 102, 204);
            grid.RowTemplate.DefaultCellStyle.SelectionForeColor = Colors.Accent;
        }
    }

    // Custom menu renderer for modern look
    public class ModernMenuRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected)
            {
                base.OnRenderMenuItemBackground(e);
            }
            else
            {
                var rect = new Rectangle(Point.Empty, e.Item.Size);
                e.Graphics.FillRectangle(new SolidBrush(ThemeHelper.Colors.Primary), rect);
            }
        }
    }
} 