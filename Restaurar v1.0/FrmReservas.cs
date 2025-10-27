using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL_Negocio;

namespace Restaurar_v1._0
{
    public partial class FrmReservas : Form
    {
        private readonly BLLReserva bll = new BLLReserva();
        private readonly BLLMesa bllMesa = new BLLMesa();

        // Snapshot del cliente (no hay textboxes en el form)
        private string _cliNombre = "";
        private string _cliTelefono = "";
        private string _cliEmail = "";


        public FrmReservas()
        {
            InitializeComponent();
        }

        private void FrmReservas_Load(object sender, EventArgs e)
        {
            InicializarGrilla();
            LimpiarCampos();
            CargarGrilla();

           
        }
        private void InicializarGrilla()
        {
            dgvReservas.AutoGenerateColumns = false;
            dgvReservas.Columns.Clear();

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFecha",
                HeaderText = "Fecha",
                DataPropertyName = "Fecha",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colHora",
                HeaderText = "Hora",
                DataPropertyName = "Hora",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colComensales",
                HeaderText = "Comensales",
                DataPropertyName = "Comensales",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            // ▼ Esta columna sigue enlazada a MesaId, pero la mostramos como NÚMERO via CellFormatting
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMesaId",
                HeaderText = "Mesa N°",
                DataPropertyName = "MesaId",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEstado",
                HeaderText = "Estado",
                DataPropertyName = "Estado",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colObs",
                HeaderText = "Observaciones",
                DataPropertyName = "Observaciones",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCliente",
                HeaderText = "Cliente",
                DataPropertyName = "ClienteNombreCompleto",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTel",
                HeaderText = "Teléfono",
                DataPropertyName = "ClienteTelefono",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });


            // Importante: enganchar el formateo para mostrar NÚMERO en la columna colMesaId
            //dgvReservas.CellFormatting += dgvReservas_CellFormatting;
        }


        private void CargarGrilla()
        {
            try
            {
                var lista = (bll.ListarTodo() ?? new List<BEReserva>())
                    .OrderBy(x => x.Fecha.Date).ThenBy(x => x.Hora).ToList();

                dgvReservas.DataSource = lista;
                dgvReservas.ClearSelection();

                if (dgvReservas.Rows.Count > 0)
                {
                    dgvReservas.CurrentCell = dgvReservas.Rows[0].Cells["colFecha"];
                    dgvReservas.Rows[0].Selected = true;
                    if (dgvReservas.Rows[0].DataBoundItem is BEReserva r0) MapearAlFormulario(r0);
                }
                else LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar reservas: " + ex.Message, "Reservas",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtId.Text = "";
            dtpFecha.Value = DateTime.Today;
            mskHora.Text = "";
            txtComensales.Text = "";
            txtMesaId.Text = "";
            txtMesaId.Tag = null;
            txtObservaciones.Text = "";
            _cliNombre = "";
            _cliTelefono = "";
            _cliEmail = "";

            lblEstadoValor.Text = BE.EstadosReserva.Pendiente;
            mskHora.Focus();
        }
        /// <summary>
        /// Abre el popup de cliente. Si es edición, precarga con datos actuales de la reserva seleccionada.
        /// Devuelve false si el usuario cancela.
        /// </summary>
        private bool SolicitarDatosCliente(bool esEdicion)
        {
            if (esEdicion)
            {
                // Traer reserva actual para precompletar
                var id = IdSeleccionado();
                if (id > 0)
                {
                    var r = bll.ListarObjeto(new BEReserva { Id = id });
                    using (var f = new FrmCliente (r?.ClienteNombreCompleto, r?.ClienteTelefono, r?.ClienteEmail))
                    {
                        if (f.ShowDialog(this) != DialogResult.OK) return false;
                        _cliNombre = f.ClienteNombre;
                        _cliTelefono = f.ClienteTelefono;
                        _cliEmail = f.ClienteEmail;
                        return true;
                    }
                }
            }

            // Alta o no hay reserva seleccionada: popup vacío
            using (var f = new FrmCliente())
            {
                if (f.ShowDialog(this) != DialogResult.OK) return false;
                _cliNombre = f.ClienteNombre;
                _cliTelefono = f.ClienteTelefono;
                _cliEmail = f.ClienteEmail;
                return true;
            }
        }


        private static bool HoraValida(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            s = s.Trim();
            if (s.Length == 5 && s[2] == ':')
            {
                var hh = s.Substring(0, 2); var mm = s.Substring(3, 2);
                return int.TryParse(hh, out var h) && int.TryParse(mm, out var m) && h >= 0 && h < 24 && m >= 0 && m < 60;
            }
            if (s.Length == 4 && int.TryParse(s, out _))
            {
                var h = int.Parse(s.Substring(0, 2)); var m = int.Parse(s.Substring(2, 2));
                return h >= 0 && h < 24 && m >= 0 && m < 60;
            }
            return false;
        }

        private BEReserva MapearDesdeFormulario()
        {
            var r = new BEReserva();

            if (long.TryParse(txtId.Text, out var id)) r.Id = id;

            var hora = (mskHora.Text ?? "").Trim();
            if (!HoraValida(hora)) throw new Exception("Hora inválida (HH:mm).");
            if (hora.Length == 4) hora = hora.Substring(0, 2) + ":" + hora.Substring(2, 2);

            if (!int.TryParse(txtComensales.Text, out var com) || com <= 0)
                throw new Exception("Comensales inválido.");

            // ► Resolver MesaId:
            long mesaId = 0;

            // 1) Si ya tenemos el Id en Tag, usarlo
            if (txtMesaId.Tag is long tagId && tagId > 0)
            {
                mesaId = tagId;
            }
            else
            {
                // 2) Si el usuario escribió el NÚMERO, buscar el Id de esa mesa
                if (int.TryParse(txtMesaId.Text, out var numero) && numero > 0)
                {
                    var mesa = (bllMesa.ListarTodo() ?? new List<BEMesa>())
                                .FirstOrDefault(m => m.Numero == numero);
                    mesaId = mesa?.Id ?? 0;
                }
            }

            r.Fecha = dtpFecha.Value.Date;
            r.Hora = hora;
            r.Comensales = com;
            r.MesaId = mesaId;
            r.Observaciones = txtObservaciones.Text?.Trim();

            // En alta siempre Pendiente (BLL lo refuerza), en edición se conserva (BLL)
            r.Estado = BE.EstadosReserva.Pendiente;
            // Snapshot cliente (vienen del popup, no del formulario)
            r.ClienteNombreCompleto = _cliNombre;
            r.ClienteTelefono = _cliTelefono;
            r.ClienteEmail = _cliEmail;

            return r;
        }


        private void MapearAlFormulario(BEReserva r)
        {
            if (r == null) return;

            txtId.Text = r.Id.ToString();
            dtpFecha.Value = r.Fecha == default ? DateTime.Today : r.Fecha;
            mskHora.Text = r.Hora ?? "";
            txtComensales.Text = r.Comensales.ToString();

            // ► Obtenemos la mesa por Id y mostramos su NÚMERO en el textbox
            var mesa = r.MesaId > 0 ? bllMesa.ListarObjeto(new BEMesa { Id = r.MesaId }) : null;
            txtMesaId.Text = mesa?.Numero.ToString() ?? ""; // visible = NÚMERO
            txtMesaId.Tag = r.MesaId;                      // interno = Id en Tag

            txtObservaciones.Text = r.Observaciones;
            lblEstadoValor.Text = string.IsNullOrWhiteSpace(r.Estado) ? BE.EstadosReserva.Pendiente : r.Estado;
        }


        private void dgvReservas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReservas.CurrentRow?.DataBoundItem is BEReserva r) MapearAlFormulario(r);
            else if (dgvReservas.Rows.Count == 1)
            {
                dgvReservas.CurrentCell = dgvReservas.Rows[0].Cells["colFecha"];
                dgvReservas.Rows[0].Selected = true;
                if (dgvReservas.Rows[0].DataBoundItem is BEReserva r0) MapearAlFormulario(r0);
            }
        }
       

        private void SoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void btnCrearReserva_Click(object sender, EventArgs e)
        {
            try
            {
                var activos = new BLL_Negocio.BLLSector().ListarActivos();
                if (activos == null || activos.Count == 0)
                {
                    MessageBox.Show("No hay sectores habilitados. No se pueden crear/modificar reservas.", "Reservas",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // SIEMPRE pedir datos de cliente al crear
                if (!SolicitarDatosCliente(esEdicion: false)) return; // canceló
                txtId.Text = ""; // asegurar alta

                var r = MapearDesdeFormulario();
                if (!MesaElegida())
                {
                    MessageBox.Show("Elegí una mesa (usá 'Elegir mesa…').", "Reservas");
                    return;
                }
                if (r.MesaId == 0) bll.Guardar(r);
                else bll.Guardar(r);

                CargarGrilla();

                // Seleccionar recién creada por Id
                var lista = dgvReservas.DataSource as List<BEReserva>;
                var idx = lista?.FindIndex(x => x.Id == r.Id) ?? -1;
                if (idx >= 0)
                {
                    dgvReservas.ClearSelection();
                    dgvReservas.Rows[idx].Selected = true;
                    dgvReservas.CurrentCell = dgvReservas.Rows[idx].Cells["colFecha"];
                    MapearAlFormulario(lista[idx]);
                }

                MessageBox.Show("Reserva creada (Pendiente).", "Reservas",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo crear: " + ex.Message, "Reservas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnModificarReserva_Click(object sender, EventArgs e)
        {
            try
            {
                var activos = new BLL_Negocio.BLLSector().ListarActivos();
                if (activos == null || activos.Count == 0)
                {
                    MessageBox.Show("No hay sectores habilitados. No se pueden crear/modificar reservas.", "Reservas",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!long.TryParse(txtId.Text, out var id) || id <= 0)
                { MessageBox.Show("Seleccioná una reserva.", "Reservas"); return; }

                // Pedir datos de cliente con precarga de la reserva actual
                if (!SolicitarDatosCliente(esEdicion: true)) return; // canceló

                var r = MapearDesdeFormulario();
                r.Id = id; // edición
                if (!MesaElegida())
                {
                    MessageBox.Show("Elegí una mesa (usá 'Elegir mesa…').", "Reservas");
                    return;
                }
                if (r.MesaId == 0) bll.Guardar(r);
                else bll.Guardar(r);

                CargarGrilla();
                MessageBox.Show("Reserva modificada.", "Reservas",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo modificar: " + ex.Message, "Reservas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminarReserva_Click(object sender, EventArgs e)
        {
            try
            {
                if (!long.TryParse(txtId.Text, out var id) || id <= 0)
                { MessageBox.Show("Seleccioná una reserva.", "Reservas"); return; }

                var ok = MessageBox.Show("¿Eliminar la reserva seleccionada?",
                    "Reservas", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ok != DialogResult.Yes) return;

                bll.Eliminar(new BEReserva { Id = id });
                CargarGrilla();
                LimpiarCampos();

                MessageBox.Show("Reserva eliminada.", "Reservas",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar: " + ex.Message, "Reservas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            dgvReservas.ClearSelection();
            LimpiarCampos();

        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();

        private void btnSugerirMesa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtComensales.Text, out var com) || com <= 0)
                {
                    MessageBox.Show("Ingresá comensales.", "Reservas");
                    return;
                }

                var hora = (mskHora.Text ?? "").Trim();
                if (!HoraValida(hora))
                {
                    MessageBox.Show("Ingresá una hora válida (HH:mm).", "Reservas");
                    return;
                }
                if (hora.Length == 4) hora = hora.Substring(0, 2) + ":" + hora.Substring(2, 2);

                var mesaId = bll.SugerirMesaId(dtpFecha.Value.Date, hora, com);
                if (mesaId == 0)
                {
                    MessageBox.Show("No hay mesas disponibles para esa fecha/hora.", "Reservas");
                    return;
                }

                // ► Mostramos NÚMERO y guardamos Id en Tag
                var mesa = bllMesa.ListarObjeto(new BEMesa { Id = mesaId });
                txtMesaId.Text = mesa?.Numero.ToString() ?? "";
                txtMesaId.Tag = mesaId;

                // Forzar ALTA (no editar la seleccionada accidentalmente)
                txtId.Text = "";
                lblEstadoValor.Text = BE.EstadosReserva.Pendiente;
                dgvReservas.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al sugerir mesa: " + ex.Message, "Reservas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private long IdSeleccionado()
        {
            if (long.TryParse(txtId.Text, out var id) && id > 0) return id;
            if (dgvReservas.CurrentRow?.DataBoundItem is BEReserva r && r.Id > 0) return r.Id;
            return 0;
        }

        private void RefrescarYSeleccionar(long id)
        {
            CargarGrilla();
            var lista = dgvReservas.DataSource as List<BEReserva>;
            var idx = lista?.FindIndex(x => x.Id == id) ?? -1;
            if (idx >= 0)
            {
                dgvReservas.ClearSelection();
                dgvReservas.Rows[idx].Selected = true;
                dgvReservas.CurrentCell = dgvReservas.Rows[idx].Cells["colFecha"];
                MapearAlFormulario(lista[idx]);
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                var id = IdSeleccionado(); if (id == 0) { MessageBox.Show("Seleccioná una reserva."); return; }
                bll.CambiarEstado(id, BE.EstadosReserva.Confirmada);
                RefrescarYSeleccionar(id);
                MessageBox.Show("Reserva confirmada.", "Reservas");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Reservas"); }
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            try
            {
                var id = IdSeleccionado(); if (id == 0) { MessageBox.Show("Seleccioná una reserva."); return; }
                bll.CambiarEstado(id, BE.EstadosReserva.CheckIn);
                RefrescarYSeleccionar(id);
                MessageBox.Show("Check-In realizado.", "Reservas");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Reservas"); }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                var id = IdSeleccionado(); if (id == 0) { MessageBox.Show("Seleccioná una reserva."); return; }
                var ok = MessageBox.Show("¿Cancelar la reserva seleccionada?", "Reservas", MessageBoxButtons.YesNo);
                if (ok != DialogResult.Yes) return;

                bll.CambiarEstado(id, BE.EstadosReserva.Cancelada);
                RefrescarYSeleccionar(id);
                MessageBox.Show("Reserva cancelada.", "Reservas");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Reservas"); }
        }

        private void dgvReservas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvReservas.Columns[e.ColumnIndex].Name == "colMesaId")
            {
                if (e.Value != null && long.TryParse(e.Value.ToString(), out var id) && id > 0)
                {
                    var mesa = bllMesa.ListarObjeto(new BEMesa { Id = id });
                    if (mesa != null)
                    {
                        e.Value = mesa.Numero.ToString(); // mostramos NÚMERO
                        e.FormattingApplied = true;
                    }
                }
            }
        }
        private bool MesaElegida() => (txtMesaId.Tag is long id && id > 0);

        private void btnElegirMesa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtComensales.Text, out var com) || com <= 0)
                { MessageBox.Show("Ingresá comensales.", "Reservas"); return; }

                var hora = (mskHora.Text ?? "").Trim();
                if (!HoraValida(hora))
                { MessageBox.Show("Hora inválida (HH:mm).", "Reservas"); return; }
                if (hora.Length == 4) hora = hora.Substring(0, 2) + ":" + hora.Substring(2, 2);

                var hh = int.Parse(hora.Substring(0, 2));
                var mm = int.Parse(hora.Substring(3, 2));
                var fechaHora = new DateTime(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, hh, mm, 0);

                if (fechaHora < DateTime.Now)
                {
                    MessageBox.Show("No podés elegir mesa para una fecha/hora pasada.", "Reservas",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var activos = new BLL_Negocio.BLLSector().ListarActivos();
                if (activos == null || activos.Count == 0)
                {
                    MessageBox.Show("No hay sectores habilitados. Cree o active uno en ABM de Sectores.", "Reservas",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var sel = new FrmSeleccionMesa(dtpFecha.Value.Date, hora, com))
                {
                    
                    if (sel.ShowDialog(this) == DialogResult.OK && sel.MesaSeleccionadaId > 0)
                    {
                        var mesa = bllMesa.ListarObjeto(new BE.BEMesa { Id = sel.MesaSeleccionadaId });
                        txtMesaId.Text = mesa?.Numero.ToString() ?? "";
                        txtMesaId.Tag = sel.MesaSeleccionadaId;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir la selección: " + ex.Message, "Reservas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
