using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DeMaria.Data;
using DeMaria.Models;
using Microsoft.Reporting.WinForms;
using Microsoft.EntityFrameworkCore;

namespace DeMaria.Forms
{
    public partial class RelatorioObitosForm : Form
    {
        private readonly CartorioContext _context;
        private readonly ReportViewer reportViewer;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFim;
        private Button btnGerar;
        private Button btnExportar;

        public RelatorioObitosForm(CartorioContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            reportViewer = new ReportViewer { Dock = DockStyle.Fill };
            InitializeComponent();
            ConfigureReportViewer();
        }

        private void InitializeComponent()
        {
            this.Text = "Relatório de Óbitos";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(640, 480); // Set minimum size to prevent controls from overlapping

            // Create a TableLayoutPanel for better control organization
            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
                ColumnCount = 6,
                RowCount = 1
            };

            // Set column percentages
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // "Data Início:" label
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // dtpInicio
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // "Data Fim:" label
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // dtpFim
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F)); // btnGerar
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F)); // btnExportar

            var lblInicio = new Label
            {
                Text = "Data Início:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleRight
            };

            dtpInicio = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };

            var lblFim = new Label
            {
                Text = "Data Fim:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleRight
            };

            dtpFim = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };

            btnGerar = new Button
            {
                Text = "Gerar Relatório",
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                AutoSize = true
            };
            btnGerar.Click += BtnGerar_Click;

            btnExportar = new Button
            {
                Text = "Exportar XML",
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                AutoSize = true
            };
            btnExportar.Click += BtnExportar_Click;

            // Add controls to TableLayoutPanel
            tableLayout.Controls.Add(lblInicio, 0, 0);
            tableLayout.Controls.Add(dtpInicio, 1, 0);
            tableLayout.Controls.Add(lblFim, 2, 0);
            tableLayout.Controls.Add(dtpFim, 3, 0);
            tableLayout.Controls.Add(btnGerar, 4, 0);
            tableLayout.Controls.Add(btnExportar, 5, 0);

            // Configure ReportViewer
            reportViewer.Dock = DockStyle.Fill;

            // Add controls to form
            this.Controls.Add(reportViewer);
            this.Controls.Add(tableLayout);
        }

        private void ConfigureReportViewer()
        {
            reportViewer.ShowToolBar = false; // Hide the toolbar
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = "Reports/RelatorioObitos.rdlc";
            reportViewer.SetDisplayMode(DisplayMode.Normal);
            reportViewer.ZoomMode = ZoomMode.PageWidth;

            // Set default parameter values to avoid the "parameter missing" message
            var parameters = new[]
            {
                new ReportParameter("DataInicio", DateTime.Now.ToShortDateString()),
                new ReportParameter("DataFim", DateTime.Now.ToShortDateString()),
                new ReportParameter("TotalRegistros", "0")
            };
            reportViewer.LocalReport.SetParameters(parameters);

            // Add empty data source to avoid the "data source instance is not supplied" message
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", new List<object>()));
            
            reportViewer.RefreshReport();
        }

        private void BtnGerar_Click(object sender, EventArgs e)
        {
            try
            {
                // Convert local dates to UTC for database query
                var dataInicio = DateTime.SpecifyKind(dtpInicio.Value.Date, DateTimeKind.Local).ToUniversalTime();
                var dataFim = DateTime.SpecifyKind(dtpFim.Value.Date.AddDays(1).AddSeconds(-1), DateTimeKind.Local).ToUniversalTime();

                var registros = _context.RegistrosObito
                    .Include(r => r.Falecido)
                    .Where(r => r.DataRegistro >= dataInicio && r.DataRegistro <= dataFim)
                    .Select(r => new
                    {
                        DataRegistro = r.DataRegistro.ToLocalTime(),
                        DataObito = r.DataObito.ToLocalTime(),
                        Nome = r.Falecido.Nome,
                        DataNascimento = r.Falecido.DataNascimento.ToLocalTime(),
                        NomePai = r.Falecido.NomePai,
                        NomeMae = r.Falecido.NomeMae,
                        DataNascimentoPai = r.Falecido.DataNascimentoPai.HasValue ? r.Falecido.DataNascimentoPai.Value.ToLocalTime() : (DateTime?)null,
                        DataNascimentoMae = r.Falecido.DataNascimentoMae.HasValue ? r.Falecido.DataNascimentoMae.Value.ToLocalTime() : (DateTime?)null
                    })
                    .ToList();

                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", registros));

                // Add report parameters for header
                var parameters = new[]
                {
                    new ReportParameter("DataInicio", dtpInicio.Value.ToShortDateString()),
                    new ReportParameter("DataFim", dtpFim.Value.ToShortDateString()),
                    new ReportParameter("TotalRegistros", registros.Count.ToString())
                };

                reportViewer.LocalReport.SetParameters(parameters);
                reportViewer.LocalReport.Refresh();
                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Arquivo XML|*.xml";
                saveDialog.Title = "Salvar Relatório XML";
                saveDialog.DefaultExt = "xml";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var registros = _context.RegistrosObito
                            .Include(r => r.Falecido)
                            .ToList();

                        var xml = new System.Xml.Serialization.XmlSerializer(typeof(List<RegistroObito>));
                        using (var writer = new System.IO.StreamWriter(saveDialog.FileName))
                        {
                            xml.Serialize(writer, registros);
                        }

                        MessageBox.Show("Relatório exportado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao exportar relatório: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
} 