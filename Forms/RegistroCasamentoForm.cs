using System;
using System.Drawing;
using System.Windows.Forms;
using DeMaria.Data;
using DeMaria.Models;
using Microsoft.EntityFrameworkCore;

namespace DeMaria.Forms
{
    public partial class RegistroCasamentoForm : Form
    {
        private readonly CartorioContext _context;
        private DateTimePicker dtpDataCasamento;
        private GroupBox grpConjuge1;
        private GroupBox grpConjuge2;
        private Button btnSalvar;
        private ErrorProvider errorProvider;

        // Campos do Cônjuge 1
        private TextBox txtNomeConjuge1;
        private DateTimePicker dtpDataNascimentoConjuge1;
        private TextBox txtNomePaiConjuge1;
        private TextBox txtNomeMaeConjuge1;
        private DateTimePicker dtpDataNascimentoPaiConjuge1;
        private DateTimePicker dtpDataNascimentoMaeConjuge1;
        private TextBox txtCpfPaiConjuge1;
        private TextBox txtCpfMaeConjuge1;

        // Campos do Cônjuge 2
        private TextBox txtNomeConjuge2;
        private DateTimePicker dtpDataNascimentoConjuge2;
        private TextBox txtNomePaiConjuge2;
        private TextBox txtNomeMaeConjuge2;
        private DateTimePicker dtpDataNascimentoPaiConjuge2;
        private DateTimePicker dtpDataNascimentoMaeConjuge2;
        private TextBox txtCpfPaiConjuge2;
        private TextBox txtCpfMaeConjuge2;

        public RegistroCasamentoForm(CartorioContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            InitializeComponent();
            InitializeUI();
            SetupValidation();
        }

        private void InitializeComponent()
        {
            this.Text = "Registro de Casamento";
            this.Size = new Size(950, 800);
            this.MinimumSize = new Size(950, 600);
            ThemeHelper.ApplyFormStyle(this);
            errorProvider = new ErrorProvider(this);
        }

        private void SetupValidation()
        {
            // Data do Casamento
            dtpDataCasamento.Validating += (s, e) => ValidationHelper.ValidateDate(dtpDataCasamento, "Data do Casamento", errorProvider);

            // Cônjuge 1
            txtNomeConjuge1.Validating += (s, e) => ValidationHelper.ValidateRequired(txtNomeConjuge1, "Nome do Cônjuge 1", errorProvider);
            dtpDataNascimentoConjuge1.Validating += (s, e) => ValidationHelper.ValidateDate(dtpDataNascimentoConjuge1, "Data de Nascimento do Cônjuge 1", errorProvider);
            
            // CPFs do Cônjuge 1
            ValidationHelper.SetupCpfMask(txtCpfPaiConjuge1);
            ValidationHelper.SetupCpfMask(txtCpfMaeConjuge1);
            txtCpfPaiConjuge1.Validating += (s, e) => ValidationHelper.ValidateCpf(txtCpfPaiConjuge1, errorProvider);
            txtCpfMaeConjuge1.Validating += (s, e) => ValidationHelper.ValidateCpf(txtCpfMaeConjuge1, errorProvider);

            // Datas dos pais do Cônjuge 1
            dtpDataNascimentoPaiConjuge1.Validating += (s, e) => ValidationHelper.ValidateParentDate(dtpDataNascimentoPaiConjuge1, dtpDataNascimentoConjuge1, "pai do Cônjuge 1", errorProvider);
            dtpDataNascimentoMaeConjuge1.Validating += (s, e) => ValidationHelper.ValidateParentDate(dtpDataNascimentoMaeConjuge1, dtpDataNascimentoConjuge1, "mãe do Cônjuge 1", errorProvider);

            // Cônjuge 2
            txtNomeConjuge2.Validating += (s, e) => ValidationHelper.ValidateRequired(txtNomeConjuge2, "Nome do Cônjuge 2", errorProvider);
            dtpDataNascimentoConjuge2.Validating += (s, e) => ValidationHelper.ValidateDate(dtpDataNascimentoConjuge2, "Data de Nascimento do Cônjuge 2", errorProvider);
            
            // CPFs do Cônjuge 2
            ValidationHelper.SetupCpfMask(txtCpfPaiConjuge2);
            ValidationHelper.SetupCpfMask(txtCpfMaeConjuge2);
            txtCpfPaiConjuge2.Validating += (s, e) => ValidationHelper.ValidateCpf(txtCpfPaiConjuge2, errorProvider);
            txtCpfMaeConjuge2.Validating += (s, e) => ValidationHelper.ValidateCpf(txtCpfMaeConjuge2, errorProvider);

            // Datas dos pais do Cônjuge 2
            dtpDataNascimentoPaiConjuge2.Validating += (s, e) => ValidationHelper.ValidateParentDate(dtpDataNascimentoPaiConjuge2, dtpDataNascimentoConjuge2, "pai do Cônjuge 2", errorProvider);
            dtpDataNascimentoMaeConjuge2.Validating += (s, e) => ValidationHelper.ValidateParentDate(dtpDataNascimentoMaeConjuge2, dtpDataNascimentoConjuge2, "mãe do Cônjuge 2", errorProvider);
        }

        private void InitializeUI()
        {
            // Create main panel with padding
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = ThemeHelper.Colors.Accent,
                AutoScroll = true
            };

            // Create header label
            var headerLabel = new Label
            {
                Text = "Registro de Casamento",
                Font = ThemeHelper.Fonts.Title,
                ForeColor = ThemeHelper.Colors.Text,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            mainPanel.Controls.Add(headerLabel);

            // Create form panel
            var formPanel = new TableLayoutPanel
            {
                Dock = DockStyle.None,
                ColumnCount = 2,
                RowCount = 19,
                Padding = new Padding(20, 80, 20, 20),
                Margin = new Padding(0),
                AutoSize = true,
                AutoScroll = true,
                Width = 850
            };

            // Configure column styles
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 600F));

            // Configure row styles
            for (int i = 0; i < 19; i++)
                formPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            int row = 0;

            // Data do Casamento
            AddFormField(formPanel, "Data do Casamento:", out dtpDataCasamento, row++);

            // Separator for Cônjuge 1
            AddSeparator(formPanel, "Cônjuge 1", row++);

            // Fields for Cônjuge 1
            AddFormField(formPanel, "Nome:", out txtNomeConjuge1, row++);
            AddFormField(formPanel, "Data de Nascimento:", out dtpDataNascimentoConjuge1, row++);
            AddFormField(formPanel, "Nome do Pai:", out txtNomePaiConjuge1, row++);
            AddFormField(formPanel, "Nome da Mãe:", out txtNomeMaeConjuge1, row++);
            AddFormField(formPanel, "Data de Nascimento do Pai:", out dtpDataNascimentoPaiConjuge1, row++);
            AddFormField(formPanel, "Data de Nascimento da Mãe:", out dtpDataNascimentoMaeConjuge1, row++);
            AddFormField(formPanel, "CPF do Pai:", out txtCpfPaiConjuge1, row++);
            AddFormField(formPanel, "CPF da Mãe:", out txtCpfMaeConjuge1, row++);

            // Separator for Cônjuge 2
            AddSeparator(formPanel, "Cônjuge 2", row++);

            // Fields for Cônjuge 2
            AddFormField(formPanel, "Nome:", out txtNomeConjuge2, row++);
            AddFormField(formPanel, "Data de Nascimento:", out dtpDataNascimentoConjuge2, row++);
            AddFormField(formPanel, "Nome do Pai:", out txtNomePaiConjuge2, row++);
            AddFormField(formPanel, "Nome da Mãe:", out txtNomeMaeConjuge2, row++);
            AddFormField(formPanel, "Data de Nascimento do Pai:", out dtpDataNascimentoPaiConjuge2, row++);
            AddFormField(formPanel, "Data de Nascimento da Mãe:", out dtpDataNascimentoMaeConjuge2, row++);
            AddFormField(formPanel, "CPF do Pai:", out txtCpfPaiConjuge2, row++);
            AddFormField(formPanel, "CPF da Mãe:", out txtCpfMaeConjuge2, row++);

            mainPanel.Controls.Add(formPanel);

            // Create button panel
            var buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                Padding = new Padding(20),
                BackColor = ThemeHelper.Colors.Accent
            };

            // Create and style save button
            btnSalvar = new Button
            {
                Text = "Salvar",
                Width = 120,
                Height = 40,
                Location = new Point(buttonPanel.Width - 140, 20),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            ThemeHelper.ApplyButtonStyle(btnSalvar, true);
            btnSalvar.Click += BtnSalvar_Click;

            buttonPanel.Controls.Add(btnSalvar);
            mainPanel.Controls.Add(buttonPanel);

            // Add spacing panel above button panel to ensure content doesn't get hidden
            var spacingPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 20,
                BackColor = ThemeHelper.Colors.Accent
            };
            mainPanel.Controls.Add(spacingPanel);

            // Add main panel to form
            this.Controls.Add(mainPanel);
        }

        private void AddFormField(TableLayoutPanel panel, string labelText, out TextBox textBox, int row)
        {
            var label = new Label { Text = labelText, AutoSize = true };
            ThemeHelper.ApplyLabelStyle(label, true);
            textBox = new TextBox { Width = 300, Dock = DockStyle.Fill };
            ThemeHelper.ApplyTextBoxStyle(textBox);
            
            panel.Controls.Add(label, 0, row);
            panel.Controls.Add(textBox, 1, row);
        }

        private void AddFormField(TableLayoutPanel panel, string labelText, out DateTimePicker datePicker, int row)
        {
            var label = new Label { Text = labelText, AutoSize = true };
            ThemeHelper.ApplyLabelStyle(label, true);
            datePicker = new DateTimePicker { Width = 300, Dock = DockStyle.Fill };
            ThemeHelper.ApplyDateTimePickerStyle(datePicker);
            
            panel.Controls.Add(label, 0, row);
            panel.Controls.Add(datePicker, 1, row);
        }

        private void AddSeparator(TableLayoutPanel panel, string text, int row)
        {
            var separator = new Label
            {
                Text = text,
                Font = new Font(ThemeHelper.Fonts.Header.FontFamily, 12, FontStyle.Bold),
                ForeColor = ThemeHelper.Colors.Primary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 20, 0, 10)
            };
            
            panel.Controls.Add(separator, 0, row);
            panel.SetColumnSpan(separator, 2);
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            // Validate all fields before saving
            if (!ValidateAll())
            {
                MessageBox.Show("Por favor, corrija os erros antes de salvar.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var conjuge1 = new Pessoa
                {
                    Nome = txtNomeConjuge1.Text,
                    DataNascimento = DateTime.SpecifyKind(dtpDataNascimentoConjuge1.Value.Date, DateTimeKind.Utc),
                    NomePai = txtNomePaiConjuge1.Text,
                    NomeMae = txtNomeMaeConjuge1.Text,
                    DataNascimentoPai = dtpDataNascimentoPaiConjuge1.Value.Date != DateTime.MinValue ? 
                        DateTime.SpecifyKind(dtpDataNascimentoPaiConjuge1.Value.Date, DateTimeKind.Utc) : null,
                    DataNascimentoMae = dtpDataNascimentoMaeConjuge1.Value.Date != DateTime.MinValue ? 
                        DateTime.SpecifyKind(dtpDataNascimentoMaeConjuge1.Value.Date, DateTimeKind.Utc) : null,
                    CpfPai = !string.IsNullOrWhiteSpace(txtCpfPaiConjuge1.Text) ? ValidationHelper.GetUnmaskedCpf(txtCpfPaiConjuge1.Text) : null,
                    CpfMae = !string.IsNullOrWhiteSpace(txtCpfMaeConjuge1.Text) ? ValidationHelper.GetUnmaskedCpf(txtCpfMaeConjuge1.Text) : null
                };

                var conjuge2 = new Pessoa
                {
                    Nome = txtNomeConjuge2.Text,
                    DataNascimento = DateTime.SpecifyKind(dtpDataNascimentoConjuge2.Value.Date, DateTimeKind.Utc),
                    NomePai = txtNomePaiConjuge2.Text,
                    NomeMae = txtNomeMaeConjuge2.Text,
                    DataNascimentoPai = dtpDataNascimentoPaiConjuge2.Value.Date != DateTime.MinValue ? 
                        DateTime.SpecifyKind(dtpDataNascimentoPaiConjuge2.Value.Date, DateTimeKind.Utc) : null,
                    DataNascimentoMae = dtpDataNascimentoMaeConjuge2.Value.Date != DateTime.MinValue ? 
                        DateTime.SpecifyKind(dtpDataNascimentoMaeConjuge2.Value.Date, DateTimeKind.Utc) : null,
                    CpfPai = !string.IsNullOrWhiteSpace(txtCpfPaiConjuge2.Text) ? ValidationHelper.GetUnmaskedCpf(txtCpfPaiConjuge2.Text) : null,
                    CpfMae = !string.IsNullOrWhiteSpace(txtCpfMaeConjuge2.Text) ? ValidationHelper.GetUnmaskedCpf(txtCpfMaeConjuge2.Text) : null
                };

                var registro = new RegistroCasamento
                {
                    DataRegistro = DateTime.UtcNow,
                    DataCasamento = DateTime.SpecifyKind(dtpDataCasamento.Value.Date, DateTimeKind.Utc),
                    Conjuge1 = conjuge1,
                    Conjuge2 = conjuge2
                };

                _context.RegistrosCasamento.Add(registro);
                _context.SaveChanges();

                MessageBox.Show("Registro salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                MessageBox.Show($"Erro ao salvar no banco de dados: {innerMessage}", "Erro de Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                var fullMessage = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Erro ao salvar registro: {fullMessage}\n\nStack Trace:\n{ex.StackTrace}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateAll()
        {
            bool isValid = true;

            // Data do Casamento
            isValid &= ValidationHelper.ValidateDate(dtpDataCasamento, "Data do Casamento", errorProvider);

            // Cônjuge 1
            isValid &= ValidationHelper.ValidateRequired(txtNomeConjuge1, "Nome do Cônjuge 1", errorProvider);
            isValid &= ValidationHelper.ValidateDate(dtpDataNascimentoConjuge1, "Data de Nascimento do Cônjuge 1", errorProvider);
            
            if (!string.IsNullOrWhiteSpace(txtCpfPaiConjuge1.Text))
                isValid &= ValidationHelper.ValidateCpf(txtCpfPaiConjuge1, errorProvider);
            if (!string.IsNullOrWhiteSpace(txtCpfMaeConjuge1.Text))
                isValid &= ValidationHelper.ValidateCpf(txtCpfMaeConjuge1, errorProvider);

            if (dtpDataNascimentoPaiConjuge1.Value.Date != DateTime.MinValue)
                isValid &= ValidationHelper.ValidateParentDate(dtpDataNascimentoPaiConjuge1, dtpDataNascimentoConjuge1, "pai do Cônjuge 1", errorProvider);
            if (dtpDataNascimentoMaeConjuge1.Value.Date != DateTime.MinValue)
                isValid &= ValidationHelper.ValidateParentDate(dtpDataNascimentoMaeConjuge1, dtpDataNascimentoConjuge1, "mãe do Cônjuge 1", errorProvider);

            // Cônjuge 2
            isValid &= ValidationHelper.ValidateRequired(txtNomeConjuge2, "Nome do Cônjuge 2", errorProvider);
            isValid &= ValidationHelper.ValidateDate(dtpDataNascimentoConjuge2, "Data de Nascimento do Cônjuge 2", errorProvider);
            
            if (!string.IsNullOrWhiteSpace(txtCpfPaiConjuge2.Text))
                isValid &= ValidationHelper.ValidateCpf(txtCpfPaiConjuge2, errorProvider);
            if (!string.IsNullOrWhiteSpace(txtCpfMaeConjuge2.Text))
                isValid &= ValidationHelper.ValidateCpf(txtCpfMaeConjuge2, errorProvider);

            if (dtpDataNascimentoPaiConjuge2.Value.Date != DateTime.MinValue)
                isValid &= ValidationHelper.ValidateParentDate(dtpDataNascimentoPaiConjuge2, dtpDataNascimentoConjuge2, "pai do Cônjuge 2", errorProvider);
            if (dtpDataNascimentoMaeConjuge2.Value.Date != DateTime.MinValue)
                isValid &= ValidationHelper.ValidateParentDate(dtpDataNascimentoMaeConjuge2, dtpDataNascimentoConjuge2, "mãe do Cônjuge 2", errorProvider);

            return isValid;
        }
    }
} 