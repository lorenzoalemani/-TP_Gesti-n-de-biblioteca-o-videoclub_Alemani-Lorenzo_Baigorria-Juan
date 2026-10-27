
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VideoClubWinForms;

public static class ModernUI
{
    private static readonly Color Blue =
        Color.FromArgb(29, 78, 216);

    private static readonly Color Navy =
        Color.FromArgb(15, 35, 75);

    private static readonly Color Background =
        Color.FromArgb(244, 247, 252);

    public static void Apply(Form form, TabControl tabs)
    {
        form.SuspendLayout();

        form.Text = "Videoclub | Gestión";
        form.MinimumSize = new Size(1050, 650);
        form.Size = new Size(1280, 780);
        form.BackColor = Background;

        // Configuración de las pestañas originales
        tabs.Appearance = TabAppearance.FlatButtons;
        tabs.ItemSize = new Size(0, 1);
        tabs.SizeMode = TabSizeMode.Fixed;
        tabs.Multiline = true;
        tabs.Dock = DockStyle.Fill;

        // Contenedores principales
        var shell = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Background
        };

        var sidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 226,
            BackColor = Navy
        };

        var top = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = Color.White
        };

        var content = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            BackColor = Background
        };

        // Título de la sección
        var heading = new Label
        {
            Text = "Inicio",
            Font = new Font(
                "Segoe UI", 19, FontStyle.Bold),
            ForeColor = Navy,
            AutoSize = true,
            Location = new Point(24, 15)
        };

        // Subtítulo corregido
        var subtitle = new Label
        {
            Text = "Gestión del videoclub",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(26, 65)
        };

        top.Controls.Add(heading);
        top.Controls.Add(subtitle);

        // Encabezado del menú lateral
        var brand = new Label
        {
            Text = "  VIDEOCLUB",
            ForeColor = Color.White,
            Font = new Font(
                "Segoe UI", 17, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 100,
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Menú lateral
        var menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(10, 8, 8, 10)
        };

        sidebar.Controls.Add(menu);
        sidebar.Controls.Add(brand);

        // Pantalla de inicio
        var welcome = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(28)
        };

        var welcomeTitle = new Label
        {
            Text = "Bienvenido al sistema de videoclub",
            Font = new Font(
                "Segoe UI", 19, FontStyle.Bold),
            ForeColor = Navy,
            Dock = DockStyle.Top,
            Height = 65
        };

        var welcomeInfo = new Label
        {
            Text = "Seleccioná una sección desde el menú lateral para administrar registros, alquileres y reportes.",
            Font = new Font("Segoe UI", 11),
            ForeColor = Color.DimGray,
            Dock = DockStyle.Top,
            Height = 80
        };

        welcome.Controls.Add(welcomeInfo);
        welcome.Controls.Add(welcomeTitle);

        // Contenido de la aplicación
        content.Controls.Add(tabs);
        content.Controls.Add(welcome);

        tabs.Visible = false;

        // Botones de navegación
        var navButtons =
            new System.Collections.Generic.List<Button>();

        void MarkActive(Button active)
        {
            foreach (var button in navButtons)
            {
                button.BackColor =
                    button == active ? Blue : Navy;
            }
        }

        Button AddNav(string title, Action action)
        {
            var button = new Button
            {
                Text = "   " + title,
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 195,
                Height = 44,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Navy,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 3, 0, 3),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Blue;

            button.Click += (_, _) =>
            {
                action();
                MarkActive(button);
            };

            menu.Controls.Add(button);
            navButtons.Add(button);

            return button;
        }

        // Botón Inicio
        var homeButton = AddNav("Inicio", () =>
        {
            tabs.Visible = false;
            welcome.Visible = true;
            welcome.BringToFront();
            heading.Text = "Inicio";
        });

        // Crear botones para las secciones existentes
        foreach (TabPage page in tabs.TabPages)
        {
            var selectedPage = page;

            AddNav(page.Text, () =>
            {
                welcome.Visible = false;
                tabs.Visible = true;
                tabs.SelectedTab = selectedPage;
                tabs.BringToFront();
                heading.Text = selectedPage.Text;
            });

            // Estilo moderno para las tablas
            foreach (var grid in
                page.Controls.OfType<DataGridView>())
            {
                grid.BorderStyle = BorderStyle.None;
                grid.BackgroundColor = Color.White;
                grid.EnableHeadersVisualStyles = false;

                grid.ColumnHeadersDefaultCellStyle.BackColor =
                    Navy;

                grid.ColumnHeadersDefaultCellStyle.ForeColor =
                    Color.White;

                grid.ColumnHeadersDefaultCellStyle.Font =
                    new Font(
                        "Segoe UI", 10, FontStyle.Bold);

                grid.DefaultCellStyle.Font =
                    new Font("Segoe UI", 10);

                grid.RowTemplate.Height = 32;

                grid.AlternatingRowsDefaultCellStyle.BackColor =
                    Color.FromArgb(241, 246, 255);
            }
        }

        // Inicio seleccionado por defecto
        MarkActive(homeButton);

        // Construcción final
        shell.Controls.Add(content);
        shell.Controls.Add(top);
        shell.Controls.Add(sidebar);

        form.Controls.Remove(tabs);
        form.Controls.Add(shell);

        form.ResumeLayout();
    }
}
