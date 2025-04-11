using System;
using System.Drawing;
using System.Windows.Forms;
using DeMaria.Data;
using DeMaria.Models;
using Microsoft.EntityFrameworkCore;

namespace DeMaria.Forms
{
    public partial class RegistroNascimentoForm : Form
    {
        private readonly CartorioContext _context;
        private TextBox txtNome;
        private DateTimePicker dtpDataNascimento;
        private TextBox txtNomePai;
        private TextBox txtNomeMae;
        private DateTimePicker dtpDataNascimentoPai;
        private DateTimePicker dtpDataNascimentoMae;
        private TextBox txtCpfPai;
        private TextBox txtCpfMae;
        private Button btnSalvar;
        private ErrorProvider errorProvider;

        public RegistroNascimentoForm(CartorioContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            InitializeComponent();
            InitializeUI();
            SetupValidation();
        }

        private void InitializeComponent()
        {
            this.Text = "Registro de Nascimento";
            this.Size = new Size(900, 700);
            ThemeHelper.ApplyFormStyle(this);
            errorProvider = new ErrorProvider(this);
        }

        private void SetupValidation()
        {
            // Setup validation events for required fields
            txtNome.Validating += (s, e) => ValidationHelper.ValidateRequired(txtNome, "Nome", errorProvider);
            dtpDataNascimento.Validating += (s, e) => ValidationHelper.ValidateDate(dtpDataNascimento, "Data de Nascimento", errorProvider);

            // Setup CPF validation and mask
            ValidationHelper.SetupCpfMask(txtCpfPai);
            ValidationHelper.SetupCpfMask(txtCpfMae);
            txtCpfPai.Validating += (s, e) => ValidationHelper.ValidateCpf(txtCpfPai, errorProvider);
            txtCpfMae.Validating += (s, e) => ValidationHelper.ValidateCpf(txtCpfMae, errorProvider);

            // Setup parent date validation
            dtpDataNascimentoPai.Validating += (s, e) => ValidationHelper.ValidateParentDate(dtpDataNascimentoPai, dtpDataNascimento, "pai", errorProvider);
            dtpDataNascimentoMae.Validating += (s, e) => ValidationHelper.ValidateParentDate(dtpDataNascimentoMae, dtpDataNascimento, "mãe", errorProvider);
        }

        private void InitializeUI()
        {
            // Create main panel with padding
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = ThemeHelper.Colors.Accent
            };

            // Create header label
            var headerLabel = new Label
            {
                Text = "Registro de Nascimento",
                Font = ThemeHelper.Fonts.Title,
                ForeColor = ThemeHelper.Colors.Text,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            mainPanel.Controls.Add(headerLabel);

            // Create form panel
            var formPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 8,
                Padding = new Padding(20, 80, 20, 20),
                Margin = new Padding(0),
                AutoSize = true
            };

            // Configure column styles
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Configure row styles
            for (int i = 0; i < 8; i++)
                formPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            int row = 0;

            // Add form fields
            AddFormField(formPanel, "Nome do Registrado:", out txtNome, row++);
            AddFormField(formPanel, "Data de Nascimento:", out dtpDataNascimento, row++);
            AddFormField(formPanel, "Nome do Pai:", out txtNomePai, row++);
            AddFormField(formPanel, "Nome da Mãe:", out txtNomeMae, row++);
            AddFormField(formPanel, "Data de Nascimento do Pai:", out dtpDataNascimentoPai, row++);
            AddFormField(formPanel, "Data de Nascimento da Mãe:", out dtpDataNascimentoMae, row++);
            AddFormField(formPanel, "CPF do Pai:", out txtCpfPai, row++);
            AddFormField(formPanel, "CPF da Mãe:", out txtCpfMae, row++);

            mainPanel.Controls.Add(formPanel);

            // Create button panel
            var buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(20)
            };

            // Create and style save button
            btnSalvar = new Button
            {
                Text = "Salvar",
                Width = 120,
                Height = 40,
                Anchor = AnchorStyles.Right
            };
            ThemeHelper.ApplyButtonStyle(btnSalvar, true);
            btnSalvar.Click += BtnSalvar_Click;

            buttonPanel.Controls.Add(btnSalvar);
            mainPanel.Controls.Add(buttonPanel);

            // Add main panel to form
            this.Controls.Add(mainPanel);
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
                var registrado = new Pessoa
                {
                    Nome = txtNome.Text,
                    DataNascimento = DateTime.SpecifyKind(dtpDataNascimento.Value.Date, DateTimeKind.Utc),
                    NomePai = txtNomePai.Text,
                    NomeMae = txtNomeMae.Text,
                    DataNascimentoPai = dtpDataNascimentoPai.Value.Date != DateTime.MinValue ? 
                        DateTime.SpecifyKind(dtpDataNascimentoPai.Value.Date, DateTimeKind.Utc) : null,
                    DataNascimentoMae = dtpDataNascimentoMae.Value.Date != DateTime.MinValue ? 
                        DateTime.SpecifyKind(dtpDataNascimentoMae.Value.Date, DateTimeKind.Utc) : null,
                    CpfPai = !string.IsNullOrWhiteSpace(txtCpfPai.Text) ? ValidationHelper.GetUnmaskedCpf(txtCpfPai.Text) : null,
                    CpfMae = !string.IsNullOrWhiteSpace(txtCpfMae.Text) ? ValidationHelper.GetUnmaskedCpf(txtCpfMae.Text) : null
                };

                var registro = new RegistroNascimento
                {
                    DataRegistro = DateTime.UtcNow,
                    Registrado = registrado
                };

                _context.RegistrosNascimento.Add(registro);
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

            // Validate required fields
            isValid &= ValidationHelper.ValidateRequired(txtNome, "Nome", errorProvider);
            isValid &= ValidationHelper.ValidateDate(dtpDataNascimento, "Data de Nascimento", errorProvider);

            // Validate CPFs if provided
            if (!string.IsNullOrWhiteSpace(txtCpfPai.Text))
                isValid &= ValidationHelper.ValidateCpf(txtCpfPai, errorProvider);
            if (!string.IsNullOrWhiteSpace(txtCpfMae.Text))
                isValid &= ValidationHelper.ValidateCpf(txtCpfMae, errorProvider);

            // Validate parent dates if provided
            if (dtpDataNascimentoPai.Value.Date != DateTime.MinValue)
                isValid &= ValidationHelper.ValidateParentDate(dtpDataNascimentoPai, dtpDataNascimento, "pai", errorProvider);
            if (dtpDataNascimentoMae.Value.Date != DateTime.MinValue)
                isValid &= ValidationHelper.ValidateParentDate(dtpDataNascimentoMae, dtpDataNascimento, "mãe", errorProvider);

            return isValid;
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
    }
} 