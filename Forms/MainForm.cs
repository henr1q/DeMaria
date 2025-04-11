using System;
using System.Drawing;
using System.Windows.Forms;
using DeMaria.Data;

namespace DeMaria.Forms
{
    public partial class MainForm : Form
    {
        private readonly CartorioContext _context;

        public MainForm()
        {
            InitializeComponent();
            _context = new CartorioContext();
        }

        private void InitializeComponent()
        {
            this.Text = "Sistema de Registro Civil";
            this.Size = new Size(1024, 768);
            ThemeHelper.ApplyFormStyle(this);

            // Create modern menu strip
            var menuStrip = new MenuStrip();
            ThemeHelper.ApplyMenuStripStyle(menuStrip);

            var registrosMenu = new ToolStripMenuItem("Registros");
            var relatoriosMenu = new ToolStripMenuItem("Relatórios");

            var registroNascimentoItem = new ToolStripMenuItem("Registro de Nascimento");
            registroNascimentoItem.Click += (s, e) => new RegistroNascimentoForm(_context).Show();

            var registroCasamentoItem = new ToolStripMenuItem("Registro de Casamento");
            registroCasamentoItem.Click += (s, e) => new RegistroCasamentoForm(_context).Show();

            var registroObitoItem = new ToolStripMenuItem("Registro de Óbito");
            registroObitoItem.Click += (s, e) => new RegistroObitoForm(_context).Show();

            var relatorioNascimentosItem = new ToolStripMenuItem("Relatório de Nascimentos");
            relatorioNascimentosItem.Click += (s, e) => new RelatorioNascimentosForm(_context).Show();

            var relatorioCasamentosItem = new ToolStripMenuItem("Relatório de Casamentos");
            relatorioCasamentosItem.Click += (s, e) => new RelatorioCasamentosForm(_context).Show();

            var relatorioObitosItem = new ToolStripMenuItem("Relatório de Óbitos");
            relatorioObitosItem.Click += (s, e) => new RelatorioObitosForm(_context).Show();

            registrosMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                registroNascimentoItem,
                registroCasamentoItem,
                registroObitoItem
            });

            relatoriosMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                relatorioNascimentosItem,
                relatorioCasamentosItem,
                relatorioObitosItem
            });

            menuStrip.Items.AddRange(new ToolStripItem[]
            {
                registrosMenu,
                relatoriosMenu
            });

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Create main container panel
            var containerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = ThemeHelper.Colors.Accent
            };

            // Create welcome label panel
            var welcomePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 80,
                ColumnCount = 1,
                RowCount = 1,
                BackColor = ThemeHelper.Colors.Accent
            };
            welcomePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            welcomePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var welcomeLabel = new Label
            {
                Text = "Bem-vindo ao Sistema de Registro Civil",
                Font = ThemeHelper.Fonts.Title,
                ForeColor = ThemeHelper.Colors.Text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            welcomePanel.Controls.Add(welcomeLabel, 0, 0);

            // Create cards container panel
            var cardsContainerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = ThemeHelper.Colors.Accent
            };

            // Create cards panel
            var cardsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 3,
                Padding = new Padding(10),
                Margin = new Padding(0),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            // Configure column and row styles
            for (int i = 0; i < 3; i++)
                cardsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            for (int i = 0; i < 2; i++)
                cardsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // Create cards with their respective actions
            var cards = new[]
            {
                (text: "Registro de Nascimento", emoji: "👶", color: ThemeHelper.Colors.Primary, action: new Action(() => new RegistroNascimentoForm(_context).Show())),
                (text: "Registro de Casamento", emoji: "💍", color: Color.FromArgb(255, 105, 180), action: new Action(() => new RegistroCasamentoForm(_context).Show())),
                (text: "Registro de Óbito", emoji: "🕯️", color: Color.FromArgb(128, 128, 128), action: new Action(() => new RegistroObitoForm(_context).Show())),
                (text: "Relatório de Nascimentos", emoji: "📊", color: Color.FromArgb(0, 150, 136), action: new Action(() => new RelatorioNascimentosForm(_context).Show())),
                (text: "Relatório de Casamentos", emoji: "📈", color: Color.FromArgb(233, 30, 99), action: new Action(() => new RelatorioCasamentosForm(_context).Show())),
                (text: "Relatório de Óbitos", emoji: "📉", color: Color.FromArgb(96, 125, 139), action: new Action(() => new RelatorioObitosForm(_context).Show()))
            };

            // Add cards to panel
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int index = row * 3 + col;
                    var card = cards[index];
                    var cardPanel = CreateCardButton(card.text, card.emoji, card.color, card.action);
                    cardsPanel.Controls.Add(cardPanel, col, row);
                }
            }

            cardsContainerPanel.Controls.Add(cardsPanel);
            containerPanel.Controls.Add(welcomePanel);
            containerPanel.Controls.Add(cardsContainerPanel);
            this.Controls.Add(containerPanel);
        }

        private Panel CreateCardButton(string text, string emoji, Color color, Action clickAction)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                BackColor = ThemeHelper.Colors.Accent,
                Cursor = Cursors.Hand
            };

            // Create table layout for content
            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Color.Transparent
            };
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Add emoji label
            var emojiLabel = new Label
            {
                Text = emoji,
                Font = new Font("Segoe UI Emoji", 48),
                ForeColor = color,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomCenter
            };
            tableLayout.Controls.Add(emojiLabel, 0, 0);

            // Add text label
            var textLabel = new Label
            {
                Text = text,
                Font = ThemeHelper.Fonts.Header,
                ForeColor = ThemeHelper.Colors.Text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter
            };
            tableLayout.Controls.Add(textLabel, 0, 1);

            panel.Controls.Add(tableLayout);

            // Add hover effect
            panel.MouseEnter += (s, e) =>
            {
                panel.BackColor = Color.FromArgb(245, 245, 245);
            };
            panel.MouseLeave += (s, e) =>
            {
                panel.BackColor = ThemeHelper.Colors.Accent;
            };

            // Make everything in the panel clickable
            panel.Click += (s, e) => clickAction();
            tableLayout.Click += (s, e) => clickAction();
            emojiLabel.Click += (s, e) => clickAction();
            textLabel.Click += (s, e) => clickAction();

            return panel;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _context.Dispose();
        }
    }
} 