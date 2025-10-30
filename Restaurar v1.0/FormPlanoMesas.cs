using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL_Negocio;
using BE;

namespace Restaurar_v1._0
{
    public partial class FormPlanoMesas : Form
    {
        private readonly BLLSector bllSector = new BLLSector();
        private readonly BLLMesa bllMesa = new BLLMesa();
        private readonly BLLReserva bllReserva = new BLLReserva();
        private readonly BLLCobranza bllCobranza = new BLLCobranza();
        private readonly BLL_Negocio.BLLComanda bllComanda = new BLL_Negocio.BLLComanda();
        private readonly BLL_Negocio.BLLProducto bllProducto = new BLL_Negocio.BLLProducto();
        private readonly BLLFactura bllFactura = new BLLFactura();
        private readonly BLLCaja bllCaja = new BLLCaja();
        private readonly BLLFamilia bllFamilia = new BLLFamilia();

        // utilitario simple
        private string UsuarioActual() => Seguridad.Sesion.UsuarioActual?.Usuario ?? "SYSTEM";

        // Ventana “operativa” para pintar reservas (hoy, ±15’ para check-in y duración 150’)
        private readonly TimeSpan DURACION = TimeSpan.FromMinutes(150);
        private readonly TimeSpan TOL_CHECKIN = TimeSpan.FromMinutes(15);

        public FormPlanoMesas()
        {
            InitializeComponent();
        }

        private void FormPlanoMesas_Load(object sender, EventArgs e)
        {
            ConstruirLeyenda();
            CargarPlano();
        }

        private void tmrRefresco_Tick(object sender, EventArgs e)
        {
            RefrescarColores();
        }
        //para probr
        private BEProducto AsegurarProductoDemo()
        {
            // 1) ¿ya hay algún producto activo?
            var prods = bllProducto.ListarTodo() ?? new System.Collections.Generic.List<BEProducto>();
            var prod = prods.FirstOrDefault(p => p.Activo);
            if (prod != null) return prod;

            // 2) buscamos/creamos familia activa
            var familias = bllFamilia.ListarTodo() ?? new System.Collections.Generic.List<BEFamilia>();
            var fam = familias.FirstOrDefault(f => f.Activa);
            if (fam == null)
            {
                fam = new BEFamilia { Nombre = "General", Activa = true };
                bllFamilia.Guardar(fam);
            }

            // 3) creamos producto demo
            prod = new BEProducto
            {
                Nombre = "Agua 500ml",
                FamiliaId = fam.Id,
                PrecioVenta = 1500m,   // usa tu propiedad de precio (ajustá el nombre si fuera distinto)
                Activo = true
            };
            bllProducto.Guardar(prod);
            return prod;
        }

        private void MostrarMenuMesa(BEMesa m, Control anchor, System.Drawing.Point posPantalla)
        {
            var estado = ObtenerEstadoMesa(m);

            var menu = new ContextMenuStrip();

            // Acción 1: Abrir comanda (mozo)
            var itmAbrir = new ToolStripMenuItem("Abrir comanda");
            itmAbrir.Enabled = (estado == "Libre" || estado == "Servicio" || estado == "Reservada");
            itmAbrir.Click += (s, e) =>
            {
                try
                {
                    var c = bllComanda.Abrir(m.Id, UsuarioActual());
                    MessageBox.Show($"Comanda #{c.Id} abierta en mesa {m.Numero}.", "OK");
                    RefrescarColores();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Comanda", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            };
            menu.Items.Add(itmAbrir);

            // (Opcional) Agregar ítem rápido para probar sin UI
            var itmAdd = new ToolStripMenuItem("Agregar ítem (rápido)");
            itmAdd.Click += (s, e) =>
            {
                try
                {
                    var cAbierta = bllComanda.ObtenerAbiertaPorMesa(m.Id) ?? bllComanda.Abrir(m.Id, UsuarioActual());
                    var prod = bllProducto.ListarActivos().FirstOrDefault();
                   // var prod = AsegurarProductoDemo();

                    if (prod == null) throw new Exception("No hay productos activos en el catálogo.");
                    bllComanda.AgregarItem(cAbierta.Id, prod.Id, 1m);
                    MessageBox.Show($"Agregado: {prod.Nombre} x1 a comanda #{cAbierta.Id}.", "OK");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Comanda", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            };
            menu.Items.Add(itmAdd);



            // Acción 2: Pedir factura (cierra comanda)
            var itmPedir = new ToolStripMenuItem("Pedir factura");
            var cOpen = bllComanda.ObtenerAbiertaPorMesa(m.Id);
            itmPedir.Enabled = (cOpen != null && cOpen.Items != null && cOpen.Items.Count > 0);

            itmPedir.Click += (s, e) =>
            {
                try
                {
                    var cAbierta = bllComanda.ObtenerAbiertaPorMesa(m.Id);
                    if (cAbierta == null)
                        throw new Exception("No hay comanda abierta para esta mesa.");

                    var cCerrada = bllComanda.PedirFactura(cAbierta.Id);
                    MessageBox.Show($"Comanda #{cCerrada.Id} cerrada y factura solicitada.", "OK");
                    RefrescarColores();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Comanda", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            };
            // habilito pedir factura solo si tiene comanda abierta (la BLL valida total>0)
           
            menu.Items.Add(itmPedir);

            // (Podemos sumar “Agregar item” cuando armemos FrmComanda)
            // ===== Caja: Emitir factura desde comanda =====
            var itmEmitir = new ToolStripMenuItem("Emitir factura (Caja)");
            itmEmitir.Click += (s, e) =>
            {
                try
                {
                    // 1) Debe existir al menos una Caja abierta
                    var hayCajaAbierta = bllCaja.ListarAbiertas()?.Any() == true;
                    if (!hayCajaAbierta)
                        throw new Exception("No hay caja abierta. Abrí una caja para emitir.");

                    // 2) Debe existir comanda CERRADA con solicitud para esta mesa
                    var cCerrada = bllComanda.ObtenerParaFacturarPorMesa(m.Id);
                    if (cCerrada == null)
                        throw new Exception("No hay comanda cerrada con factura solicitada.");

                    // 3) Emitir la factura desde comanda
                    var fac = bllFactura.EmitirDesdeComanda(cCerrada.Id, puntoVenta: 1, tipoCbte: "B");

                    MessageBox.Show($"Factura {fac} emitida para mesa {m.Numero}.", "Caja");
                    RefrescarColores();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            // Habilito solo si (hay caja) y (hay comanda para facturar)
            var hayCaja = bllCaja.ListarAbiertas()?.Any() == true;
            var cParaFac = bllComanda.ObtenerParaFacturarPorMesa(m.Id);
            itmEmitir.Enabled = hayCaja && (cParaFac != null);

            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(itmEmitir);
            // ===== Mozo: Cobrar =====
            var itmCobrar = new ToolStripMenuItem("Cobrar (Mozo)");
            itmCobrar.Click += (s, e) =>
            {
                try
                {
                    // Debe existir al menos una factura 'Emitida' para esta mesa
                    var pendientes = bllCobranza.FacturasPendientesPorMesa(m.Id);
                    if (pendientes == null || pendientes.Count == 0)
                        throw new Exception("No hay facturas emitidas pendientes para esta mesa.");

                    using (var frm = new FrmCobroMesa())
                    {
                        frm.MesaIdPrefijada = m.Id; // <-- se autocompleta y busca
                        frm.ShowDialog(this);
                    }

                    // Refresca estado/color de la mesa (Cuenta -> Libre/Servicio según corresponda)
                    RefrescarColores();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Cobro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            // Habilitar solo si hay facturas 'Emitida' pendientes
            var hayPendientes = bllCobranza.FacturasPendientesPorMesa(m.Id)?.Any() == true;
            itmCobrar.Enabled = hayPendientes;

            menu.Items.Add(itmCobrar);


            menu.Show(anchor, anchor.PointToClient(posPantalla));
        }

        private void CargarPlano()
        {
            tabSectores.TabPages.Clear();

            var sectores = bllSector.ListarActivos();         // sólo activos
            var mesas = bllMesa.ListarTodo() ?? new System.Collections.Generic.List<BEMesa>();

            foreach (var s in sectores)
            {
                var tab = new TabPage(s.Nombre);
                var flow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Padding = new Padding(8),
                    WrapContents = true
                };
                tab.Controls.Add(flow);
                tabSectores.TabPages.Add(tab);

                var mesasDelSector = mesas
                    .Where(m => m.Habilitada && string.Equals((m.Sector ?? "").Trim(), s.Nombre.Trim(), StringComparison.OrdinalIgnoreCase))
                    .OrderBy(m => m.Numero)
                    .ToList();

                foreach (var m in mesasDelSector)
                {
                    var btn = new Button
                    {
                        Width = 72,
                        Height = 72,
                        Text = m.Numero.ToString(),
                        Tag = m.Id,
                        BackColor = ColorDeMesa(m),
                        ForeColor = Color.White,
                        Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold),
                        Margin = new Padding(6)
                    };
                    toolTip1.SetToolTip(btn, TooltipDeMesa(m));
                    btn.MouseUp += (sdr, ev) =>
                    {
                        if (ev.Button == MouseButtons.Right)
                        {
                            MostrarMenuMesa(m, btn, Cursor.Position);
                        }
                    };

                    btn.Click += (sdr, ev) => OnMesaClick(m);
                    flow.Controls.Add(btn);
                }
            }
        }

        private void RefrescarColores()
        {
            // Recorre todos los botones y los repinta según estado actual
            foreach (TabPage tab in tabSectores.TabPages)
            {
                if (tab.Controls.Count == 0) continue;
                var flow = tab.Controls[0] as FlowLayoutPanel;
                if (flow == null) continue;

                foreach (var ctrl in flow.Controls.OfType<Button>())
                {
                    if (ctrl.Tag == null) continue;

                    long mesaId;
                    try { mesaId = Convert.ToInt64(ctrl.Tag); }
                    catch { continue; }

                    var m = bllMesa.ListarObjeto(new BEMesa { Id = mesaId });
                    if (m != null)
                    {
                        ctrl.BackColor = ColorDeMesa(m);
                        toolTip1.SetToolTip(ctrl, TooltipDeMesa(m));
                    }
                }
            }
        }

        // ========== Estados y colores ==========
        // Libre = Verde, Reservada = Azul, Servicio (Ocupada/Check-In) = Naranja,
        // Cuenta (factura emitida sin cobro) = Violeta, Bloqueada/Inactiva = Gris
        private Color ColorDeMesa(BEMesa m)
        {
            var estado = ObtenerEstadoMesa(m);

            switch (estado)
            {
                case "Bloqueada":
                    return Color.Gray;
                case "Cuenta":
                    return Color.MediumPurple;
                case "Servicio":
                    return Color.FromArgb(255, 153, 0); // naranja
                case "Reservada":
                    return Color.FromArgb(0, 102, 204); // azul
                case "Libre":
                    return Color.FromArgb(0, 153, 51);  // verde
                default:
                    return Color.DimGray;
            }
        }


        private string TooltipDeMesa(BEMesa m)
        {
            var estado = ObtenerEstadoMesa(m);
            return $"Mesa {m.Numero} — {estado}\nSector: {m.Sector}\nCapacidad: {m.Capacidad}";
        }

        private string ObtenerEstadoMesa(BEMesa m)
        {
            if (m == null) return "Desconocido";
            if (!m.Habilitada || string.Equals(m.Estado, EstadosMesa.Bloqueada, StringComparison.OrdinalIgnoreCase))
                return "Bloqueada";

            // ¿Cuenta pedida? => hay facturas "Emitida" pendientes de cobro
            var factPend = bllCobranza.FacturasPendientesPorMesa(m.Id);
            if (factPend != null && factPend.Count > 0)
                return "Cuenta";

            // ¿Ocupada/Servicio por Check-In?
            if (string.Equals(m.Estado, EstadosMesa.Ocupada, StringComparison.OrdinalIgnoreCase))
                return "Servicio";

            // ¿Reservada en ventana (hoy)?
            var hoy = DateTime.Today;
            var reservasHoy = bllReserva.ListarTodo()
                ?.Where(r => r.MesaId == m.Id && r.Fecha.Date == hoy && !string.Equals(r.Estado, EstadosReserva.Cancelada, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (reservasHoy != null && reservasHoy.Count > 0)
            {
                var ahora = DateTime.Now;
                foreach (var r in reservasHoy)
                {
                    if (!TimeSpan.TryParse(r.Hora, out var hhmm)) continue;
                    var inicio = r.Fecha.Date + hhmm;
                    var fin = inicio + DURACION;

                    var enVentana = ahora >= (inicio - TOL_CHECKIN) && ahora <= fin;
                    if (enVentana)
                    {
                        // Si tiene Check-In, la mesa debería estar Ocupada (ya tratado arriba),
                        // si no, la pintamos como Reservada.
                        if (!string.Equals(r.Estado, EstadosReserva.CheckIn, StringComparison.OrdinalIgnoreCase))
                            return "Reservada";
                    }
                }
            }

            // Por defecto
            return "Libre";
        }

        // Click en una mesa (por ahora info y atajos simples; acciones detalladas las agregamos luego)
        private void OnMesaClick(BEMesa m)
        {
            var estado = ObtenerEstadoMesa(m);

            // Atajos mínimos:
            // - Si Libre/Reservada: abrir gestión de Reservas (si querés prefiltrar por mesa)
            // - Si Cuenta: abrir Cobro Mesa
            // - Si Servicio: podríamos abrir Facturación o (cuando exista) Comanda

            if (estado == "Cuenta")
            {
                MessageBox.Show($"Mesa {m.Numero}: hay factura emitida pendiente de cobro.", "Cuenta pedida");
                // new FrmCobroMesa(m.Id).ShowDialog(this); // cuando quieras enlazarlo
            }
            else if (estado == "Libre" || estado == "Reservada")
            {
                MessageBox.Show($"Mesa {m.Numero}: podés gestionar una reserva o hacer check-in.", "Reservas");
                // new FrmReservas().ShowDialog(this); // enlazalo cuando prefieras
            }
            else if (estado == "Servicio")
            {
                MessageBox.Show($"Mesa {m.Numero}: en servicio. Podés emitir factura o (luego) abrir comanda.", "Servicio");
                // new FrmFactura(m.Id).ShowDialog(this); // si tenés constructor con mesa
            }
            else
            {
                MessageBox.Show($"Mesa {m.Numero}: {estado}.", "Información");
            }
        }
        private void ConstruirLeyenda()
        {
            flowLeyenda.SuspendLayout();
            flowLeyenda.Controls.Clear();

            AgregarItemLeyenda("Libre", Color.FromArgb(0, 153, 51));
            AgregarItemLeyenda("Reservada", Color.FromArgb(0, 102, 204));
            AgregarItemLeyenda("Servicio", Color.FromArgb(255, 153, 0));
            AgregarItemLeyenda("Cuenta", Color.MediumPurple);
            AgregarItemLeyenda("Bloqueada", Color.Gray);

            flowLeyenda.ResumeLayout();
        }

        private void AgregarItemLeyenda(string texto, Color color)
        {
            var cont = new Panel
            {
                AutoSize = true,
                Margin = new Padding(6, 6, 12, 6)
            };

            var swatch = new Panel
            {
                BackColor = color,
                Width = 16,
                Height = 16,
                Margin = new Padding(0, 2, 6, 0),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lbl = new Label
            {
                Text = texto,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 0)
            };

            // contenedor horizontal para el par [cuadradito + texto]
            var row = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false
            };
            row.Controls.Add(swatch);
            row.Controls.Add(lbl);

            cont.Controls.Add(row);
            flowLeyenda.Controls.Add(cont);
        }

    }
}
