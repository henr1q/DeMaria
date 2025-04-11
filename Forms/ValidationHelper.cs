using System;
using System.Windows.Forms;
using System.Linq;

namespace DeMaria.Forms
{
    public static class ValidationHelper
    {
        public static bool ValidateRequired(Control control, string fieldName, ErrorProvider errorProvider)
        {
            if (string.IsNullOrWhiteSpace(control.Text))
            {
                errorProvider.SetError(control, $"{fieldName} é obrigatório.");
                return false;
            }
            errorProvider.SetError(control, "");
            return true;
        }

        public static bool ValidateCpf(Control control, ErrorProvider errorProvider)
        {
            string cpf = control.Text.Trim().Replace(".", "").Replace("-", "");
            
            if (string.IsNullOrWhiteSpace(cpf))
            {
                errorProvider.SetError(control, "");
                return true; // CPF is optional
            }

            if (!IsValidCpf(cpf))
            {
                errorProvider.SetError(control, "CPF inválido.");
                return false;
            }

            errorProvider.SetError(control, "");
            return true;
        }

        public static void SetupCpfMask(TextBox textBox)
        {
            textBox.MaxLength = 14; // 11 digits + 2 dots + 1 dash
            textBox.TextChanged += (s, e) =>
            {
                string text = textBox.Text.Replace(".", "").Replace("-", "");
                
                // Only allow digits
                text = new string(text.Where(char.IsDigit).ToArray());
                
                if (text.Length > 11)
                    text = text.Substring(0, 11);

                // Format for display only
                if (text.Length > 9)
                    text = text.Insert(9, "-");
                if (text.Length > 6)
                    text = text.Insert(6, ".");
                if (text.Length > 3)
                    text = text.Insert(3, ".");

                if (text != textBox.Text)
                {
                    int cursorPosition = textBox.SelectionStart;
                    textBox.Text = text;
                    textBox.SelectionStart = cursorPosition;
                }
            };

            // When the control loses focus, store only the numbers
            textBox.Leave += (s, e) =>
            {
                string numbers = new string(textBox.Text.Where(char.IsDigit).ToArray());
                if (numbers.Length == 11)
                {
                    // Reformat with mask for display
                    string formatted = numbers.Insert(9, "-").Insert(6, ".").Insert(3, ".");
                    textBox.Text = formatted;
                }
            };
        }

        public static string GetUnmaskedCpf(string cpf)
        {
            return new string(cpf.Where(char.IsDigit).ToArray());
        }

        public static bool ValidateDate(DateTimePicker control, string fieldName, ErrorProvider errorProvider)
        {
            if (control.Value.Date == DateTime.MinValue)
            {
                errorProvider.SetError(control, $"{fieldName} é obrigatório.");
                return false;
            }
            errorProvider.SetError(control, "");
            return true;
        }

        public static bool ValidateParentDate(DateTimePicker parentDate, DateTimePicker childDate, string parentField, ErrorProvider errorProvider)
        {
            if (parentDate.Value.Date != DateTime.MinValue && parentDate.Value.Date >= childDate.Value.Date)
            {
                errorProvider.SetError(parentDate, $"Data de nascimento do {parentField} deve ser anterior à data de nascimento do registrado.");
                return false;
            }
            errorProvider.SetError(parentDate, "");
            return true;
        }

        private static bool IsValidCpf(string cpf)
        {
            if (cpf.Length != 11) return false;

            // Check if all digits are the same
            if (cpf.Distinct().Count() == 1) return false;

            // Validate first check digit
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += (cpf[i] - '0') * (10 - i);
            }
            int remainder = sum % 11;
            int checkDigit1 = remainder < 2 ? 0 : 11 - remainder;
            if (checkDigit1 != (cpf[9] - '0')) return false;

            // Validate second check digit
            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += (cpf[i] - '0') * (11 - i);
            }
            remainder = sum % 11;
            int checkDigit2 = remainder < 2 ? 0 : 11 - remainder;
            if (checkDigit2 != (cpf[10] - '0')) return false;

            return true;
        }
    }
} 