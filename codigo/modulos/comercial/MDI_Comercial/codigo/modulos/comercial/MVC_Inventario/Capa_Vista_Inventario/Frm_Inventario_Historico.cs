using System;
using System.Data;
using System.Windows.Forms;
using Capa_Controlador_Inventario; // Usamos la capa Controlador
using iTextSharp.text; // Para PDF
using iTextSharp.text.pdf; // Para PDF
using System.IO; // Para PDF

namespace Capa_Vista_Inventario
{
    // ==================== Stevens Cambranes 05/11/2025 ====================
    // ==================== Formulario Histórico de Inventario ====================
    public partial class Frm_Inventario_Historico : Form
    {
        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Variable del Controlador ====================
        // (Instancia global del controlador para comunicarse con la capa Modelo)
        private Cls_Controlador_Inventario controlador;

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Constructor del Formulario ====================
        public Frm_Inventario_Historico()
        {
            InitializeComponent();
            controlador = new Cls_Controlador_Inventario();
            // Asigna el evento 'Load' del formulario
            this.Load += new System.EventHandler(this.Frm_Inventario_Historico_Load);

            // Asigna el evento 'DataBindingComplete' para formatear el DGV
            this.Dgv_Vista_Inventarios_Pasados.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.Dgv_Vista_Inventarios_Pasados_DataBindingComplete);
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Formateo de Celdas (DGV) ====================
        private void Dgv_Vista_Inventarios_Pasados_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Formatea la cantidad a 4 decimales (N4)
                if (Dgv_Vista_Inventarios_Pasados.Columns.Contains("Cmp_Cantidad"))
                {
                    Dgv_Vista_Inventarios_Pasados.Columns["Cmp_Cantidad"].DefaultCellStyle.Format = "N4";
                }

                // Formatea el costo a 2 decimales (N2)
                if (Dgv_Vista_Inventarios_Pasados.Columns.Contains("CostoEnEseMomento"))
                {
                    Dgv_Vista_Inventarios_Pasados.Columns["CostoEnEseMomento"].DefaultCellStyle.Format = "N2";
                }

                // Formatea el valor total a 2 decimales (N2)
                if (Dgv_Vista_Inventarios_Pasados.Columns.Contains("ValorTotal"))
                {
                    Dgv_Vista_Inventarios_Pasados.Columns["ValorTotal"].DefaultCellStyle.Format = "N2";
                }

                // Formatea la fecha a solo día/mes/año (ShortDate)
                if (Dgv_Vista_Inventarios_Pasados.Columns.Contains("Cmp_Fecha"))
                {
                    Dgv_Vista_Inventarios_Pasados.Columns["Cmp_Fecha"].DefaultCellStyle.Format = "d"; // 'd' es para ShortDatePattern
                }
            }
            catch (Exception ex)
            {
                // No es un error crítico, solo informa en la consola de depuración
                System.Diagnostics.Debug.WriteLine("Error al formatear DGV: " + ex.Message);
            }
        }

        // ==================== Stevens Cambranes 28/10/25 ====================
        // ==================== Botón Nuevo Inventario ====================
        private void Btn_Nuevo_Inventario_Click(object sender, EventArgs e)
        {
            Frm_Inventario frmNuevoInventario = new Frm_Inventario(); // Estandarizado
            frmNuevoInventario.Show();
            this.Hide();
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Botón Imprimir PDF ====================
        private void Btn_Imprimir_PDF_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que quiere generar el PDF?", "Generar PDF", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (Dgv_Vista_Inventarios_Pasados.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog sfdGuardar = new SaveFileDialog // Estandarizado
                {
                    Filter = "Archivo PDF (*.pdf)|*.pdf",
                    Title = "Guardar Reporte de Histórico",
                    FileName = $"Reporte_De_Inventario_{DateTime.Now:dd-MM-yyyy}.pdf"
                };

                if (sfdGuardar.ShowDialog() == DialogResult.OK) // Estandarizado
                {
                    try
                    {
                        Document docReporte = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 10f); // Estandarizado
                        PdfWriter writer = PdfWriter.GetInstance(docReporte, new FileStream(sfdGuardar.FileName, FileMode.Create)); // Estandarizado
                        docReporte.Open();

                        Paragraph titulo = new Paragraph("Reporte Histórico de Inventario",
                            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK));
                        titulo.Alignment = Element.ALIGN_CENTER;
                        docReporte.Add(titulo); // Estandarizado
                        docReporte.Add(Chunk.NEWLINE);

                        PdfPTable pdfTable = new PdfPTable(Dgv_Vista_Inventarios_Pasados.Columns.Count);
                        pdfTable.WidthPercentage = 100;

                        // Agrega los encabezados
                        foreach (DataGridViewColumn colReporte in Dgv_Vista_Inventarios_Pasados.Columns) // Estandarizado
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(colReporte.HeaderText, // Estandarizado
                                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE)));
                            cell.BackgroundColor = new BaseColor(45, 75, 125);
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cell);
                        }

                        // Agrega las filas (aplicando el mismo formato que el DGV)
                        foreach (DataGridViewRow rowReporte in Dgv_Vista_Inventarios_Pasados.Rows) // Estandarizado
                        {
                            foreach (DataGridViewCell cellReporte in rowReporte.Cells) // Estandarizado
                            {
                                string sCellValue = cellReporte.Value != null ? cellReporte.Value.ToString() : ""; // Estandarizado

                                // Aplica formato a los números y fechas para el PDF
                                if (cellReporte.OwningColumn.Name == "Cmp_Cantidad")
                                {
                                    sCellValue = string.Format("{0:N4}", cellReporte.Value);
                                }
                                else if (cellReporte.OwningColumn.Name == "CostoEnEseMomento" || cellReporte.OwningColumn.Name == "ValorTotal")
                                {
                                    sCellValue = string.Format("{0:N2}", cellReporte.Value);
                                }
                                else if (cellReporte.OwningColumn.Name == "Cmp_Fecha" || cellReporte.OwningColumn.Name == "Cmp_FechaCierre")
                                {
                                    sCellValue = string.Format("{0:d}", cellReporte.Value);
                                }

                                pdfTable.AddCell(new Phrase(sCellValue, // Estandarizado
                                    FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK)));
                            }
                        }

                        docReporte.Add(pdfTable); // Estandarizado
                        docReporte.Close(); // Estandarizado
                        writer.Close();

                        MessageBox.Show("Reporte PDF generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(sfdGuardar.FileName); // Abre el PDF
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al generar el PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Botón Buscar Inventario (Actualizado) ====================
        private void Btn_Buscar_Inventario_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtResultado = new DataTable(); // Estandarizado

                if (Rdb_Ver_Historicos.Checked)
                {
                    // MODO 1: BÚSQUEDA EN CIERRES
                    dtResultado = controlador.Ctr_CargarTodosLosCierres(); // Estandarizado
                }
                else if (Rdb_Usar_Filtros_Busqueda.Checked)
                {
                    // MODO 2: BÚSQUEDA EN MOVIMIENTOS

                    // Validación: Asegura que se seleccionó un Tipo de Movimiento
                    if (Cbo_Tipo_Operacion.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debe seleccionar un Tipo de Movimiento para buscar.", "Validación Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cbo_Tipo_Operacion.Focus();
                        return;
                    }

                    // Recolecta todos los filtros
                    string sTipoMovimiento = Cbo_Tipo_Operacion.Text; // Estandarizado
                    if (sTipoMovimiento == "Ver Todos Los Movimientos")
                    {
                        sTipoMovimiento = null;
                    }

                    int? iIdAlmacen = (Rdb_Filtro_Almacen.Checked && Cbo_Filtro_Almacen.SelectedValue != null) ?
                                      (int?)Cbo_Filtro_Almacen.SelectedValue : null; // Estandarizado

                    int? iIdEstado = (Rdb_Filtro_Estado.Checked && Cbo_Filtro_Estado.SelectedValue != null) ?
                                     (int?)Cbo_Filtro_Estado.SelectedValue : null; // Estandarizado

                    bool bUsarRango = Rdb_Filtro_Rango_Fecha.Checked; // Estandarizado
                    DateTime dFechaInicio = Dtp_Filtro_Rango_Fecha_Inicio.Value.Date; // Estandarizado
                    DateTime dFechaFin = Dtp_Filtro_Rango_Fecha_Fin.Value.Date; // Estandarizado

                    string sOrden = "mov.Cmp_Fecha_Movimiento DESC"; // Estandarizado
                    if (Rdb_Filtro_Menos_Recientes.Checked) sOrden = "mov.Cmp_Fecha_Movimiento ASC"; // Estandarizado
                    else if (Rdb_Filtro_Valor_Mas_Alto.Checked) sOrden = "ValorTotal DESC"; // Estandarizado
                    else if (Rdb_Filtro_Valor_Mas_Bajo.Checked) sOrden = "ValorTotal ASC"; // Estandarizado

                    // Llama al controlador. 
                    dtResultado = controlador.Ctr_ObtenerHistorico( // Estandarizado
                        sTipoMovimiento, iIdAlmacen, iIdEstado,
                        bUsarRango, dFechaInicio, dFechaFin, sOrden
                    );
                }

                // Muestra los resultados en el DGV
                Dgv_Vista_Inventarios_Pasados.DataSource = null;
                Dgv_Vista_Inventarios_Pasados.DataSource = dtResultado; // Estandarizado

                if (dtResultado != null && dtResultado.Rows.Count == 0) // Estandarizado
                {
                    MessageBox.Show("No se encontraron resultados para esta búsqueda.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Limpia los ComboBoxes después de la búsqueda
                Cbo_Tipo_Operacion.SelectedIndex = -1;
                Cbo_Filtro_Almacen.SelectedIndex = -1;
                Cbo_Filtro_Estado.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar el histórico: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ==================== Stevens Cambranes 28/10/25 ====================
        // ==================== Habilitar / Deshabilitar Filtros ====================
        // (Estos métodos activan/desactivan los ComboBox según su RadioButton)
        private void Rdb_Filtro_Almacen_CheckedChanged(object sender, EventArgs e)
        {
            Cbo_Filtro_Almacen.Enabled = Rdb_Filtro_Almacen.Checked;
        }

        private void Rdb_Filtro_Estado_CheckedChanged(object sender, EventArgs e)
        {
            Cbo_Filtro_Estado.Enabled = Rdb_Filtro_Estado.Checked;
        }

        private void Rdb_Filtro_Rango_Fecha_CheckedChanged(object sender, EventArgs e)
        {
            Dtp_Filtro_Rango_Fecha_Inicio.Enabled = Rdb_Filtro_Rango_Fecha.Checked;
            Dtp_Filtro_Rango_Fecha_Fin.Enabled = Rdb_Filtro_Rango_Fecha.Checked;
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Cargar ComboBox Tipo Movimiento ====================
        private void CargarComboBoxTiposMovimiento()
        {
            try
            {
                DataTable dtResultado = controlador.Ctr_CargarTiposMovimiento(); // Estandarizado
                Cbo_Tipo_Operacion.DataSource = dtResultado; // Estandarizado
                Cbo_Tipo_Operacion.DisplayMember = "Cmp_Nombre";
                Cbo_Tipo_Operacion.ValueMember = "Pk_ID_TipoMovimiento";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar tipos de movimiento: " + ex.Message, "Error");
            }
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Cargar DGV por Defecto ====================
        private void CargarGridPorDefecto()
        {
            try
            {
                DataTable dtResultado = controlador.Ctr_CargarHistoricoDefault(); // Estandarizado
                Dgv_Vista_Inventarios_Pasados.DataSource = null;
                Dgv_Vista_Inventarios_Pasados.DataSource = dtResultado; // Estandarizado
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el histórico por defecto: " + ex.Message, "Error");
            }
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Evento Load del Formulario ====================
        private void Frm_Inventario_Historico_Load(object sender, EventArgs e)
        {
            try
            {
                // Llena todos los ComboBox
                CargarComboBoxAlmacenes();
                CargarComboBoxEstados();
                CargarComboBoxTiposMovimiento();

                // Limpia la selección inicial
                Cbo_Filtro_Almacen.SelectedIndex = -1;
                Cbo_Filtro_Estado.SelectedIndex = -1;
                Cbo_Tipo_Operacion.SelectedIndex = -1;

                // Conecta los RadioButtons al método de control unificado
                this.Rdb_Ver_Historicos.CheckedChanged += new System.EventHandler(this.ModoDeBusqueda_CheckedChanged);
                this.Rdb_Usar_Filtros_Busqueda.CheckedChanged += new System.EventHandler(this.ModoDeBusqueda_CheckedChanged);

                // Establece el modo de búsqueda por defecto
                Rdb_Usar_Filtros_Busqueda.Checked = true;

                // Carga el DGV con los movimientos por defecto
                CargarGridPorDefecto();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos iniciales: " + ex.Message, "Error fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Cargar ComboBox Almacenes ====================
        private void CargarComboBoxAlmacenes()
        {
            try
            {
                DataTable dtResultado = controlador.Ctr_CargarAlmacenes(); // Estandarizado
                Cbo_Filtro_Almacen.DataSource = dtResultado; // Estandarizado
                Cbo_Filtro_Almacen.DisplayMember = "Cmp_Nombre";
                Cbo_Filtro_Almacen.ValueMember = "Pk_ID_Almacen";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar almacenes: " + ex.Message, "Error");
            }
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Cargar ComboBox Estados ====================
        private void CargarComboBoxEstados()
        {
            try
            {
                DataTable dtResultado = controlador.Ctr_CargarEstadosProducto(); // Estandarizado
                Cbo_Filtro_Estado.DataSource = dtResultado; // Estandarizado
                Cbo_Filtro_Estado.DisplayMember = "Cmp_Nombre";
                Cbo_Filtro_Estado.ValueMember = "Pk_ID_EstadoProducto";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar estados: " + ex.Message, "Error");
            }
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Control de Modos de Búsqueda (RadioButtons) ====================
        private void ModoDeBusqueda_CheckedChanged(object sender, EventArgs e)
        {
            // Verifica si el modo "Ver Cierres" está activo
            bool bVerCierres = Rdb_Ver_Historicos.Checked; // Estandarizado

            // Habilita o deshabilita los filtros basado en el modo
            Cbo_Tipo_Operacion.Enabled = !bVerCierres; // Estandarizado
            // Asumiendo que 'Gpb_Filtros' es el GroupBox
            // Gpb_Filtros.Enabled = !bVerCierres; 

            // Si se activa "Ver Cierres", limpia el ComboBox de movimientos
            if (bVerCierres) // Estandarizado
            {
                Cbo_Tipo_Operacion.SelectedIndex = -1;
            }
        }
    }
}