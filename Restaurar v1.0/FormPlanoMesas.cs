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

        // Ventana “operativa” para pintar reservas (hoy, ±15’ para check-in y duración 150’)
        private readonly TimeSpan DURACION = TimeSpan.FromMinutes(150);
        private readonly TimeSpan TOL_CHECKIN = TimeSpan.FromMinutes(15);
        public FormPlanoMesas()
        {
            InitializeComponent();
        }

        private void FormPlanoMesas_Load(object sender, EventArgs e)
        {
            CargarPlano();
        }

        private void tmrRefresco_Tick(object sender, EventArgs e)
        {
            RefrescarColores();
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
    }
}
