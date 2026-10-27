using AccesoDatos.Data;
using AccesoDatos.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;
using System.Data;

namespace VideoClubWinForms;

public class MainForm : Form
{
    private readonly TabControl tabs = new() { Dock = DockStyle.Fill };
    private readonly Dictionary<Type, DataGridView> grids = new();
    private readonly Dictionary<Type, string[]> columns = new()
    {
        [typeof(Socio)] = new[] { "Id", "Nombre", "Apellido", "Dni", "Telefono" },
        [typeof(Pelicula)] = new[] { "Id", "Titulo", "Autor", "Cantidad", "Disponible" },
        [typeof(Proveedor)] = new[] { "Id", "Nombre", "Direccion" },
        [typeof(Promociones)] = new[] { "Id", "Nombre", "Descuento", "Activa" },
        [typeof(Alquiler)] = new[] { "Id", "Socioid", "Fechaentrega", "Fechadevolucion", "Monto", "Estado" },
        [typeof(Devolucion)] = new[] { "Id", "FechaDevolucion", "AlquilerId" }
    };

    public MainForm()
    {
        Text = "Gestión de Videoclub - Alemani / Baigorria";
        Width = 1150; Height = 740; StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(850, 550);
        foreach (var (type, title) in new (Type, string)[]
        {
            (typeof(Socio), "Socios"), (typeof(Pelicula), "Películas"),
            (typeof(Proveedor), "Proveedores"), (typeof(Promociones), "Promociones"),
            (typeof(Alquiler), "Alquileres"), (typeof(Devolucion), "Devoluciones")
        }) AddTab(type, title);
        AddReportsTab();
        Controls.Add(tabs);
        RefreshAll();
        ModernUI.Apply(this, tabs);
    }

    private void AddTab(Type type, string title)
    {
        var page = new TabPage(title);
        var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(7) };
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = SystemColors.Window
        };
        grids[type] = grid;
        AddButton(bar, "Consultar", () => ShowConsultation(type, title));
        if (type != typeof(Devolucion) && type != typeof(Alquiler))
        {
            AddButton(bar, "Alta", () => EditEntity(type, false));
            AddButton(bar, "Modificar", () => EditEntity(type, true));
            AddButton(bar, "Baja", () => DeleteEntity(type));
        }
        if (type == typeof(Alquiler))
        {
            AddButton(bar, "Registrar alquiler", RegisterRental);
            AddButton(bar, "Registrar devolución", RegisterReturn);
        }
        if (type == typeof(Devolucion)) AddButton(bar, "Registrar devolución", RegisterReturn);
        page.Controls.Add(grid); page.Controls.Add(bar); tabs.TabPages.Add(page);
    }

    // Reportes originales: se conserva la lógica de consulta existente.
    private void AddReportsTab()
    {
        var page = new TabPage("Reportes");
        var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(7) };
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = SystemColors.Window
        };
        var comboReportes = new ComboBox { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
        comboReportes.Items.AddRange(new object[] {
            "1. Alquileres Activos y Vencidos", "2. Top 10 Películas Más Alquiladas",
            "3. Promociones Más Utilizadas", "4. Stock por Proveedor", "5. Historial por Socio"
        });
        comboReportes.SelectedIndex = 0;
        bar.Controls.Add(new Label { Text = "Seleccionar Reporte:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        bar.Controls.Add(comboReportes);
        var btnGenerar = new Button { Text = "Generar Reporte", AutoSize = true, Height = 28 };
        btnGenerar.Click += (_, _) =>
        {
            try
            {
                using var db = new AplicationDbContext();
                var table = new DataTable();
                switch (comboReportes.SelectedIndex)
                {
                    case 0:
                        {
                            table.Columns.Add("AlquilerId"); table.Columns.Add("Socio"); table.Columns.Add("DNI");
                            table.Columns.Add("Fecha Entrega"); table.Columns.Add("Vencimiento");
                            table.Columns.Add("Estado"); table.Columns.Add("Monto"); table.Columns.Add("Situación");
                            var list = (from a in db.Alquiler
                                        join s in db.Socio on a.Socioid equals s.Id
                                        where a.Estado == "Activo" || a.Estado == "Alquilado" || a.Fechadevolucion < DateTime.Now
                                        orderby a.Fechadevolucion
                                        select new { a, s }).ToList();
                            foreach (var item in list)
                            {
                                string situacion = item.a.Fechadevolucion < DateTime.Now && item.a.Estado != "Devuelto" ? "VENCIDO" : "ACTIVO";
                                table.Rows.Add(item.a.Id, $"{item.s.Nombre} {item.s.Apellido}", item.s.Dni,
                                    item.a.Fechaentrega.ToShortDateString(), item.a.Fechadevolucion.ToShortDateString(),
                                    item.a.Estado, item.a.Monto, situacion);
                            }
                            break;
                        }
                    case 1:
                        {
                            table.Columns.Add("Título"); table.Columns.Add("Autor"); table.Columns.Add("Veces Alquilada");
                            var list = db.Alquiler.Include(a => a.listapeliculas)
                                .SelectMany(a => a.listapeliculas)
                                .GroupBy(p => new { p.Titulo, p.Autor })
                                .Select(g => new { g.Key.Titulo, g.Key.Autor, Count = g.Count() })
                                .OrderByDescending(x => x.Count).Take(10).ToList();
                            foreach (var item in list) table.Rows.Add(item.Titulo, item.Autor, item.Count);
                            break;
                        }
                    case 2:
                        {
                            table.Columns.Add("Id"); table.Columns.Add("Nombre"); table.Columns.Add("Descuento (%)");
                            table.Columns.Add("Activa"); table.Columns.Add("Veces Usada");
                            table.Columns.Add("Monto Total Generado");
                            var queryPromo = from pr in db.Promociones
                                             join a in db.Alquiler on pr.Id equals a.PromocionId into gj
                                             select new
                                             {
                                                 pr.Id,
                                                 pr.Nombre,
                                                 pr.Descuento,
                                                 pr.Activa,
                                                 VecesUsada = gj.Count(),
                                                 MontoTotal = gj.Sum(x => (decimal?)x.Monto) ?? 0
                                             };
                            foreach (var item in queryPromo.OrderByDescending(x => x.VecesUsada))
                                table.Rows.Add(item.Id, item.Nombre, item.Descuento,
                                    item.Activa ? "Sí" : "No", item.VecesUsada, item.MontoTotal);
                            break;
                        }
                    case 3:
                        {
                            table.Columns.Add("ProveedorId"); table.Columns.Add("Proveedor");
                            table.Columns.Add("Dirección"); table.Columns.Add("Cantidad Títulos");
                            table.Columns.Add("Stock Total"); table.Columns.Add("Stock Disponible");
                            var proveedores = db.Proveedor.ToList();
                            foreach (var pv in proveedores) table.Rows.Add(pv.Id, pv.Nombre, pv.Direccion, 0, 0, 0);
                            break;
                        }
                    case 4:
                        {
                            table.Columns.Add("SocioId"); table.Columns.Add("Socio"); table.Columns.Add("DNI");
                            table.Columns.Add("AlquilerId"); table.Columns.Add("Fecha Entrega");
                            table.Columns.Add("Fecha Prevista"); table.Columns.Add("Estado");
                            table.Columns.Add("Monto"); table.Columns.Add("Cumplimiento");
                            var queryHist = from s in db.Socio
                                            join a in db.Alquiler on s.Id equals a.Socioid
                                            orderby s.Apellido, s.Nombre, a.Fechaentrega descending
                                            select new { s, a };
                            foreach (var item in queryHist)
                            {
                                string cumplimiento;
                                if (item.a.Estado == "Devuelto" && item.a.Fechadevolucion >= DateTime.Now) cumplimiento = "A tiempo";
                                else if (item.a.Estado == "Devuelto") cumplimiento = "Con atraso";
                                else if (item.a.Fechadevolucion < DateTime.Now) cumplimiento = "Vencido (no devuelto)";
                                else cumplimiento = "En curso";
                                table.Rows.Add(item.s.Id, $"{item.s.Nombre} {item.s.Apellido}", item.s.Dni,
                                    item.a.Id, item.a.Fechaentrega.ToShortDateString(),
                                    item.a.Fechadevolucion.ToShortDateString(), item.a.Estado,
                                    item.a.Monto, cumplimiento);
                            }
                            break;
                        }
                }
                grid.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };
        bar.Controls.Add(btnGenerar); page.Controls.Add(grid); page.Controls.Add(bar);
        tabs.TabPages.Add(page);
    }

    private static void AddButton(FlowLayoutPanel bar, string caption, Action action)
    {
        var button = new Button { Text = caption, AutoSize = true, Height = 34 };
        button.Click += (_, _) =>
        {
            try { action(); }
            catch (Exception ex) { MessageBox.Show(ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        };
        bar.Controls.Add(button);
    }

    private void RefreshAll()
    {
        foreach (var type in grids.Keys) RefreshEntity(type);
    }

    private void RefreshEntity(Type type)
    {
        using var db = new AplicationDbContext();
        IEnumerable<object> entities = type == typeof(Socio) ? db.Socio.AsNoTracking().Cast<object>().ToList()
            : type == typeof(Pelicula) ? db.Pelicula.AsNoTracking().Cast<object>().ToList()
            : type == typeof(Proveedor) ? db.Proveedor.AsNoTracking().Cast<object>().ToList()
            : type == typeof(Promociones) ? db.Promociones.AsNoTracking().Cast<object>().ToList()
            : type == typeof(Alquiler) ? db.Alquiler.AsNoTracking().Cast<object>().ToList()
            : db.Devolucion.AsNoTracking().Cast<object>().ToList();
        var props = columns[type].Select(n => type.GetProperty(n)
            ?? throw new InvalidOperationException($"La propiedad {n} no existe en {type.Name}.")).ToArray();
        var table = new DataTable();
        foreach (var prop in props) table.Columns.Add(prop.Name);
        foreach (var entity in entities)
            table.Rows.Add(props.Select(p => p.GetValue(entity)?.ToString() ?? "").ToArray());
        grids[type].DataSource = table;
    }

    private void ShowConsultation(Type type, string title)
    {
        RefreshEntity(type);
        var source = (DataTable)grids[type].DataSource!;
        using var dialog = new Form
        {
            Text = $"Consultar {title}",
            Width = 900,
            Height = 550,
            MinimumSize = new Size(650, 400),
            StartPosition = FormStartPosition.CenterParent
        };
        var consultationGrid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = SystemColors.Window,
            DataSource = source.Copy()
        };
        var footer = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 48,
            Padding = new Padding(10),
            FlowDirection = FlowDirection.RightToLeft
        };
        var closeButton = new Button { Text = "Cerrar", Width = 100, Height = 28 };
        closeButton.Click += (_, _) => dialog.Close();
        footer.Controls.Add(closeButton);
        footer.Controls.Add(new Label
        {
            Text = source.Rows.Count == 0 ? "No hay registros para mostrar."
                : $"Registros encontrados: {source.Rows.Count}",
            AutoSize = true,
            Padding = new Padding(8, 5, 12, 0)
        });
        dialog.Controls.Add(consultationGrid); dialog.Controls.Add(footer);
        dialog.ShowDialog(this);
    }

    private int? SelectedId(Type type)
    {
        var grid = grids[type];
        if (grid.CurrentRow == null) { MessageBox.Show("Seleccioná un registro de la tabla."); return null; }
        if (int.TryParse(grid.CurrentRow.Cells["Id"].Value?.ToString(), out var id)) return id;
        MessageBox.Show("Seleccioná un registro válido."); return null;
    }

    private void EditEntity(Type type, bool editing)
    {
        int? id = editing ? SelectedId(type) : null;
        if (editing && id == null) return;
        using var db = new AplicationDbContext();
        var entity = editing ? db.Find(type, id!.Value) : Activator.CreateInstance(type);
        if (entity == null) return;
        var props = columns[type].Where(n => n != "Id").Select(n => type.GetProperty(n)!).ToArray();
        using var dialog = new Form
        {
            Text = (editing ? "Modificar " : "Agregar ") + type.Name,
            Width = 460,
            Height = 125 + props.Length * 46,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false
        };
        var inputs = new Dictionary<PropertyInfo, Control>();
        int y = 15;
        foreach (var prop in props)
        {
            dialog.Controls.Add(new Label { Text = prop.Name, Left = 12, Top = y + 5, Width = 140 });
            Control control;
            if (prop.PropertyType == typeof(bool))
                control = new CheckBox { Left = 165, Top = y + 4, Checked = (bool)(prop.GetValue(entity) ?? false) };
            else
                control = new TextBox
                {
                    Left = 165,
                    Top = y,
                    Width = 255,
                    Text = editing ? Convert.ToString(prop.GetValue(entity), CultureInfo.CurrentCulture) ?? "" : ""
                };
            inputs[prop] = control; dialog.Controls.Add(control); y += 46;
        }
        var save = new Button { Text = "Guardar", Left = 315, Top = y, Width = 105, DialogResult = DialogResult.None };
        save.Click += (_, _) =>
        {
            try
            {
                BasicValidation.Check(inputs);
                foreach (var (prop, control) in inputs)
                {
                    object value = control is CheckBox check ? check.Checked :
                        prop.PropertyType == typeof(int) ? int.Parse(control.Text) :
                        prop.PropertyType == typeof(decimal) ? decimal.Parse(control.Text, CultureInfo.CurrentCulture) :
                        control.Text.Trim();
                    if (value is string s && string.IsNullOrWhiteSpace(s)) throw new Exception($"Completá {prop.Name}.");
                    if (prop.Name == "Cantidad" && (int)value < 0) throw new Exception("La cantidad no puede ser negativa.");
                    if (prop.Name == "Descuento" && ((decimal)value < 0 || (decimal)value > 100))
                        throw new Exception("El descuento debe estar entre 0 y 100.");
                    prop.SetValue(entity, value);
                }
                if (editing) db.Update(entity!); else db.Add(entity!);
                db.SaveChanges(); dialog.DialogResult = DialogResult.OK; dialog.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.GetBaseException().Message, "No se pudo guardar"); }
        };
        dialog.Controls.Add(save); dialog.ShowDialog(this); RefreshAll();
    }

    private void DeleteEntity(Type type)
    {
        var id = SelectedId(type); if (id == null) return;
        if (MessageBox.Show("¿Eliminar el registro seleccionado?", "Confirmar baja", MessageBoxButtons.YesNo)
            != DialogResult.Yes) return;
        using var db = new AplicationDbContext();
        var entity = db.Find(type, id.Value);
        if (entity != null) { db.Remove(entity); db.SaveChanges(); }
        RefreshAll();
    }

    private void RegisterRental()
    {
        using var db = new AplicationDbContext();
        var socios = db.Socio.OrderBy(s => s.Id).ToList();
        var pelis = db.Pelicula.Where(p => p.Disponible && p.Cantidad > 0).OrderBy(p => p.Titulo).ToList();
        if (socios.Count == 0 || pelis.Count == 0)
        {
            MessageBox.Show("Primero registrá socios y películas disponibles."); return;
        }
        using var form = new Form { Text = "Registrar alquiler", Width = 540, Height = 460, StartPosition = FormStartPosition.CenterParent };
        var socio = new ComboBox { Left = 15, Top = 40, Width = 485, DropDownStyle = ComboBoxStyle.DropDownList };
        socio.DataSource = socios.Select(s => new Choice(s.Id, $"{s.Id} - {s.Nombre} {s.Apellido}")).ToList();
        form.Controls.Add(new Label { Left = 15, Top = 15, Text = "Socio", AutoSize = true }); form.Controls.Add(socio);
        var movies = new CheckedListBox { Left = 15, Top = 105, Width = 485, Height = 190, CheckOnClick = true };
        foreach (var p in pelis) movies.Items.Add(new Choice(p.Id, $"{p.Id} - {p.Titulo} (stock: {p.Cantidad})"));
        form.Controls.Add(new Label { Left = 15, Top = 82, Text = "Películas (seleccioná una o más)", AutoSize = true });
        form.Controls.Add(movies);
        var amount = new NumericUpDown { Left = 170, Top = 310, Width = 140, Maximum = 100000000, DecimalPlaces = 2 };
        form.Controls.Add(new Label { Left = 15, Top = 315, Text = "Monto del alquiler", AutoSize = true }); form.Controls.Add(amount);
        var promo = new ComboBox { Left = 170, Top = 345, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
        var options = new List<Choice> { new(0, "Sin promoción") };
        options.AddRange(db.Promociones.Where(p => p.Activa).Select(p => new Choice(p.Id, $"{p.Nombre} ({p.Descuento}%)")));
        promo.DataSource = options;
        form.Controls.Add(new Label { Left = 15, Top = 350, Text = "Promoción", AutoSize = true }); form.Controls.Add(promo);
        var save = new Button { Text = "Registrar", Left = 390, Top = 380, Width = 110 };
        save.Click += (_, _) =>
        {
            try
            {
                if (movies.CheckedItems.Count == 0) throw new Exception("Seleccioná al menos una película.");
                var ids = movies.CheckedItems.Cast<Choice>().Select(x => x.Id).ToList();
                var selected = db.Pelicula.Where(p => ids.Contains(p.Id)).ToList();
                if (selected.Any(p => !p.Disponible || p.Cantidad < 1)) throw new Exception("Hay películas sin stock.");
                foreach (var p in selected) p.Cantidad--;
                var promoId = ((Choice)promo.SelectedItem!).Id;
                var promotion = promoId == 0 ? null : db.Promociones.Find(promoId);
                var total = amount.Value;
                if (promotion != null) total -= total * promotion.Descuento / 100;
                db.Alquiler.Add(new Alquiler
                {
                    Socioid = ((Choice)socio.SelectedItem!).Id,
                    Fechaentrega = DateTime.Now,
                    Fechadevolucion = DateTime.Now.AddDays(7),
                    listapeliculas = selected,
                    Monto = total,
                    Estado = "Activo",
                    Promocion = promotion
                });
                db.SaveChanges(); form.DialogResult = DialogResult.OK; form.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.GetBaseException().Message); }
        };
        form.Controls.Add(save); form.ShowDialog(this); RefreshAll();
    }

    private void RegisterReturn()
    {
        using var db = new AplicationDbContext();
        var active = db.Alquiler.Include(a => a.listapeliculas).Where(a => a.Estado == "Activo").ToList();
        if (active.Count == 0) { MessageBox.Show("No hay alquileres activos."); return; }
        using var form = new Form { Text = "Registrar devolución", Width = 480, Height = 220, StartPosition = FormStartPosition.CenterParent };
        form.Controls.Add(new Label { Left = 15, Top = 15, Text = "Alquiler activo", AutoSize = true });
        var combo = new ComboBox { Left = 15, Top = 40, Width = 430, DropDownStyle = ComboBoxStyle.DropDownList };
        combo.DataSource = active.Select(a => new Choice(a.Id, $"Alquiler {a.Id} - Socio {a.Socioid} - {a.listapeliculas.Count} películas")).ToList();
        form.Controls.Add(combo);
        var description = new TextBox { Left = 15, Top = 100, Width = 430, Text = "Devolución de películas" };
        form.Controls.Add(new Label { Left = 15, Top = 80, Text = "Descripción", AutoSize = true }); form.Controls.Add(description);
        var button = new Button { Left = 320, Top = 135, Width = 125, Text = "Registrar" };
        button.Click += (_, _) =>
        {
            try
            {
                var id = ((Choice)combo.SelectedItem!).Id;
                var rental = db.Alquiler.Include(a => a.listapeliculas).Single(a => a.Id == id);
                if (rental.Estado != "Activo") throw new Exception("El alquiler ya fue devuelto.");
                foreach (var p in rental.listapeliculas) p.Cantidad++;
                rental.Estado = "Devuelto";
                rental.Fechadevolucion = DateTime.Now;
                db.Devolucion.Add(new Devolucion { AlquilerId = id, FechaDevolucion = DateTime.Now });
                db.SaveChanges(); form.DialogResult = DialogResult.OK; form.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.GetBaseException().Message); }
        };
        form.Controls.Add(button); form.ShowDialog(this); RefreshAll();
    }

    private record Choice(int Id, string Label) { public override string ToString() => Label; }
}
