using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Capa_Controlador_Inventario; // Importante para usar el controlador

namespace Capa_Vista_Inventario
{
    public partial class Frm_Inventario : Form
    {
        private Cls_Controlador_Inventario controlador; // Estandarizado
        private const char SeparadorTag = '|'; // Usado para separar ID y Prefijo en el Tag

        public Frm_Inventario()
        {
            InitializeComponent();
            controlador = new Cls_Controlador_Inventario(); // Inicializa el controlador
            this.Load += new System.EventHandler(this.Frm_Inventario_Load); // Conecta el evento Load

            // *** CONEXIÓN DE EVENTOS CRÍTICOS ***
            this.Dgv_Vista_Producto.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_Vista_Producto_CellClick);
            this.Btn_Agregar_Producto.Click += new System.EventHandler(this.Btn_Agregar_Producto_Click); // Conexión del botón Agregar
            this.Btn_Eliminar_Producto.Click += new System.EventHandler(this.Btn_Eliminar_Producto_Click); // Conexión del botón Eliminar
            this.Btn_Modificar_Producto.Click += new System.EventHandler(this.Btn_Modificar_Producto_Click); // Conexión del botón Modificar
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Evento Load para carga inicial de ComboBox ====================
        private void Frm_Inventario_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargar Catálogos Iniciales
                CargarUnidadesMedida();
                CargarAlmacenes();
                CargarComboBoxEstados();

                // Carga IDs, Prefijos y conecta los eventos de asistencia
                CargarCategoriasARadioButtons();

                // Limpiar selecciones iniciales 
                Cbo_Unidad_Medida_Producto.SelectedIndex = -1;
                Cbo_Almacen_Producto.SelectedIndex = -1;
                Cbo_Estado_Producto.SelectedIndex = -1;

                // Carga la tabla de productos
                CargarDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos iniciales: " + ex.Message, "Error");
            }
        }

        // ==================== Stevens Cambranes 13/11/2025 ====================
        // ==================== Carga dinámica de Categorías a RadioButtons ====================
        private void CargarCategoriasARadioButtons()
        {
            try
            {
                DataTable dtCategorias = controlador.Ctr_CargarCategorias();
                GroupBox gpbTipoProducto = this.Gpb_Tipo_Producto;
                const string PrefijoComunRdb = "Rdb_Producto_"; // Prefijo esperado
                const string PrefijoRopaBlanca = "Rdb_Ropa_Blanca_Lenceria"; // Nombre especial

                // --- Configuración de Mapeo de Prefijos (Lógica de UI) ---
                Dictionary<string, string> dictPrefijos = new Dictionary<string, string>
        {
          {"Limpieza_Mantenimiento", "LIMP-"},
          {"Alimentos_Bebidas", "ALIM-"},
          {"Mobiliario_Equipo", "MOB-"},
          {"Equipos_Cocina", "COC-"},
          {"Suministros_Oficina", "SUM-"},
          {"Ropa_Blanca_Lenceria", "LENC-"},
          {"Cortesia", "COR-"},
          {"Seguridad_Emergencia", "SEG-"}
        };

                Dictionary<string, RadioButton> dictRdb = new Dictionary<string, RadioButton>();
                foreach (Control control in gpbTipoProducto.Controls)
                {
                    if (control is RadioButton rdb)
                    {
                        // Conectar el evento de cambio a la función que inserta el prefijo
                        rdb.CheckedChanged += new EventHandler(this.Rdb_TipoProducto_CheckedChanged);

                        // FIX: Normalizamos el nombre del control para la clave de búsqueda, manejando el caso especial de Ropa Blanca.
                        string sControlName = rdb.Name;
                        string sClaveRdb;

                        if (sControlName.StartsWith(PrefijoComunRdb))
                        {
                            sClaveRdb = sControlName.Replace(PrefijoComunRdb, "");
                        }
                        else if (sControlName.Equals(PrefijoRopaBlanca))
                        {
                            sClaveRdb = "Ropa_Blanca_Lenceria"; // Clave que necesitamos para el mapeo
                        }
                        else
                        {
                            continue;
                        }

                        dictRdb.Add(sClaveRdb, rdb);
                    }
                }

                // --- 2. Asignar ID de la BD y Prefijo concatenado en la propiedad Tag ---
                foreach (DataRow dr in dtCategorias.Rows)
                {
                    int iId = Convert.ToInt32(dr["Pk_ID"]);
                    string sNombre = dr["Cmp_Nombre"].ToString();

                    // Normalización del nombre de la BD para la búsqueda de la clave.
                    string sClaveBusqueda = sNombre.Replace(" y ", "_").Replace(" de ", "_").Replace(" ", "_").Replace("í", "i").Replace("é", "e").Replace("ó", "o");

                    if (dictRdb.TryGetValue(sClaveBusqueda, out RadioButton rdbTarget))
                    {
                        rdbTarget.Text = sNombre;

                        // Almacena ID y Prefijo en el Tag, separados por '|'
                        string sPrefijo = dictPrefijos.ContainsKey(sClaveBusqueda) ? dictPrefijos[sClaveBusqueda] : "";
                        rdbTarget.Tag = $"{iId}|{sPrefijo}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mapear categorías en RadioButtons: " + ex.Message, "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== Stevens Cambranes 13/11/2025 ====================
        // ==================== MANEJADOR: Asigna Prefijo al Código de Producto ====================
        // ARCHIVO: Frm_Inventario.cs (Fragmento a usar)

        // ==================== Stevens Cambranes 14/11/2025 ====================
        // ==================== MANEJADOR: Asigna Prefijo y CÓDIGO SUGERIDO ====================
        private void Rdb_TipoProducto_CheckedChanged(object sender, EventArgs e)
        {
            const string CodigoSugerido = "001"; // Valor por defecto

            if (sender is RadioButton rdb && rdb.Checked)
            {
                if (rdb.Tag != null)
                {
                    string sTagCompleto = rdb.Tag.ToString();

                    // Extrae el prefijo (todo después del '|')
                    string sPrefijo = sTagCompleto.Contains(SeparadorTag) ? sTagCompleto.Substring(sTagCompleto.IndexOf(SeparadorTag) + 1) : string.Empty;

                    if (Txt_Codigo_Producto != null && !string.IsNullOrEmpty(sPrefijo))
                    {
                        // *** FIX: Asigna el prefijo COMPLETO con el código sugerido ***
                        Txt_Codigo_Producto.Text = sPrefijo + CodigoSugerido;
                        Txt_Codigo_Producto.Focus();
                    }
                    else
                    {
                        Txt_Codigo_Producto.Text = string.Empty;
                    }
                }
            }
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Funciones de Carga de ComboBox (Existentes) ====================

        private void CargarUnidadesMedida()
        {
            try
            {
                DataTable dtResultado = controlador.Ctr_CargarUnidadesMedida();
                Cbo_Unidad_Medida_Producto.DataSource = dtResultado;
                Cbo_Unidad_Medida_Producto.DisplayMember = "Cmp_Nombre";
                Cbo_Unidad_Medida_Producto.ValueMember = "Pk_ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar unidades de medida: " + ex.Message, "Error");
            }
        }

        private void CargarAlmacenes()
        {
            try
            {
                DataTable dtResultado = controlador.Ctr_CargarAlmacenes();
                Cbo_Almacen_Producto.DataSource = dtResultado;
                Cbo_Almacen_Producto.DisplayMember = "Cmp_Nombre";
                Cbo_Almacen_Producto.ValueMember = "Pk_ID_Almacen";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar almacenes: " + ex.Message, "Error");
            }
        }

        private void CargarComboBoxEstados()
        {
            try
            {
                DataTable dtResultado = controlador.Ctr_CargarEstadosProducto();
                Cbo_Estado_Producto.DataSource = dtResultado;
                Cbo_Estado_Producto.DisplayMember = "Cmp_Nombre";
                Cbo_Estado_Producto.ValueMember = "Pk_ID_EstadoProducto";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar estados de producto: " + ex.Message, "Error");
            }
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Lógica para obtener ID de Categoría (RadioButtons) ====================
        private int ObtenerIdCategoriaSeleccionada()
        {
            GroupBox gpbTipoProducto = this.Gpb_Tipo_Producto;

            foreach (Control control in gpbTipoProducto.Controls)
            {
                if (control is RadioButton rdb)
                {
                    if (rdb.Checked)
                    {
                        if (rdb.Tag != null)
                        {
                            string sTagCompleto = rdb.Tag.ToString();
                            string sIdString = sTagCompleto.Substring(0, sTagCompleto.IndexOf(SeparadorTag));

                            if (int.TryParse(sIdString, out int iIdCategoria))
                            {
                                return iIdCategoria;
                            }
                        }
                        // Fallback (Mapeo manual de IDs)
                        else if (rdb.Name.Contains("Limpieza_Mantenimiento")) return 1;
                        else if (rdb.Name.Contains("Alimentos_Bebidas")) return 2;
                        else if (rdb.Name.Contains("Mobiliario_Equipo")) return 3;
                        else if (rdb.Name.Contains("Equipos_Cocina")) return 4;
                        else if (rdb.Name.Contains("Suministros_Oficina")) return 5;
                        else if (rdb.Name.Contains("Ropa_Blanca_Lenceria")) return 6;
                        else if (rdb.Name.Contains("Cortesia")) return 7;
                        else if (rdb.Name.Contains("Seguridad_Emergencia")) return 8;
                    }
                }
            }
            return 0;
        }

        // ==================== Stevens Cambranes 13/11/2025 ====================
        // ==================== VALIDACIÓN DE CAMPOS VACÍOS ====================
        private bool ValidarCamposVacios()
        {
            // Validar TextBoxes
            if (string.IsNullOrWhiteSpace(Txt_Codigo_Producto.Text) ||
        string.IsNullOrWhiteSpace(Txt_Nombre_Producto.Text) ||
        string.IsNullOrWhiteSpace(Txt_Costo_Unitario_Producto.Text) ||
        string.IsNullOrWhiteSpace(Txt_Cantidad_Producto.Text) ||
        string.IsNullOrWhiteSpace(Txt_Existencias_Minimas_Producto.Text) ||
        string.IsNullOrWhiteSpace(Txt_Existencias_Maximas_Producto.Text))
            {
                MessageBox.Show("Debe llenar todos los campos de Código, Nombre, Costo, Cantidad y Límites de Existencia.", "Validación de Entrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar ComboBoxes (si no están en -1 o nulo)
            if (Cbo_Unidad_Medida_Producto.SelectedValue == null ||
        Cbo_Almacen_Producto.SelectedValue == null ||
        Cbo_Estado_Producto.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar una Unidad de Medida, un Almacén y un Estado.", "Validación de Selección", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar Radio Button (Categoría)
            if (ObtenerIdCategoriaSeleccionada() == 0)
            {
                MessageBox.Show("Debe seleccionar un Tipo de Producto (Categoría).", "Validación de Categoría", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Botón Agregar Producto ====================
        private void Btn_Agregar_Producto_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. VALIDACIÓN DE CAPA VISTA: Detiene si hay campos vacíos o selecciones incompletas.
                if (!ValidarCamposVacios())
                {
                    return;
                }

                // **2. Recolección de Datos**
                // Datos del Producto (Tbl_Producto)
                string sCodigo = Txt_Codigo_Producto.Text;
                string sNombre = Txt_Nombre_Producto.Text;
                string sMarca = Txt_Marca_Producto.Text;
                string sDescripcion = Txt_Descripcion_Producto.Text;
                DateTime dFechaVencimiento = Dtp_Fecha_Vencimiento_Producto.Value;

                // Conversión a ID/Double
                int iIdCategoria = ObtenerIdCategoriaSeleccionada();
                int iIdUnidad = Convert.ToInt32(Cbo_Unidad_Medida_Producto.SelectedValue);
                double doPrecioUnitario = Convert.ToDouble(Txt_Costo_Unitario_Producto.Text);

                // Datos de Existencia (Tbl_Existencia) - CONVERSIÓN A INT
                int iCantidad = Convert.ToInt32(Txt_Cantidad_Producto.Text);
                int iExistMin = Convert.ToInt32(Txt_Existencias_Minimas_Producto.Text);
                int iExistMax = Convert.ToInt32(Txt_Existencias_Maximas_Producto.Text);
                int iIdAlmacen = Convert.ToInt32(Cbo_Almacen_Producto.SelectedValue);

                // **3. Invocación al Controlador (Usando la firma final con INT)**
                bool bResultado = controlador.Ctr_InsertarProducto(
          sCodigo, sNombre, sMarca, sDescripcion, dFechaVencimiento, iIdCategoria, iIdUnidad, doPrecioUnitario,
          iCantidad, iExistMin, iExistMax, iIdAlmacen); // <-- Se pasan los tipos INT

                if (bResultado)
                {
                    MessageBox.Show("Producto registrado exitosamente. Se ha registrado la existencia inicial.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDataGrid();
                    // Aquí se llamaría a una función para limpiar campos.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar agregar producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== Stevens Cambranes 26/10/25 ====================
        private void Btn_Cierre_Inventario_Click(object sender, EventArgs e)
        {
            Frm_Cierre_Inventario frmCierreInventario = new Frm_Cierre_Inventario();
            frmCierreInventario.Show();
            this.Hide();
        }

        // ==================== Stevens Cambranes 13/11/2025 ====================
        // ==================== Botón Modificar Producto ====================
        private void Btn_Modificar_Producto_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validar que haya un producto seleccionado y que los campos no estén vacíos.
                if (Btn_Modificar_Producto.Tag == null || !ValidarCamposVacios())
                {
                    MessageBox.Show("Debe seleccionar un producto de la tabla y llenar todos los campos para modificar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int iIdProducto = Convert.ToInt32(Btn_Modificar_Producto.Tag);

                // 2. Recolección de Datos Actualizados
                string sCodigo = Txt_Codigo_Producto.Text;
                string sNombre = Txt_Nombre_Producto.Text;
                string sMarca = Txt_Marca_Producto.Text;
                string sDescripcion = Txt_Descripcion_Producto.Text;
                DateTime dFechaVencimiento = Dtp_Fecha_Vencimiento_Producto.Value;
                int iIdCategoria = ObtenerIdCategoriaSeleccionada();
                int iIdUnidad = Convert.ToInt32(Cbo_Unidad_Medida_Producto.SelectedValue);
                double doPrecioUnitario = Convert.ToDouble(Txt_Costo_Unitario_Producto.Text);

                // 3. Invocación al Controlador
                bool bResultado = controlador.Ctr_ModificarProducto(
          iIdProducto, sCodigo, sNombre, sMarca, sDescripcion, dFechaVencimiento, iIdCategoria, iIdUnidad, doPrecioUnitario);

                if (bResultado)
                {
                    MessageBox.Show($"Producto modificado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDataGrid(); // Recargar DGV
                    // Aquí se llamaría a una función para limpiar campos.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar modificar producto: " + ex.Message, "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ==================== Stevens Cambranes 13/11/2025 ====================
        // ==================== Botón Eliminar Producto (Con Alerta) ====================
        private void Btn_Eliminar_Producto_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Obtener ID del producto (guardado en el Tag de Modificar)
                if (Btn_Modificar_Producto.Tag == null)
                {
                    MessageBox.Show("Debe seleccionar un producto de la tabla para eliminar.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int iIdProducto = Convert.ToInt32(Btn_Modificar_Producto.Tag);
                string sNombreProducto = Txt_Nombre_Producto.Text;

                // 2. Mostrar la alerta de confirmación
                DialogResult dialogResult = MessageBox.Show($"¿Seguro que quiere eliminar el producto: {sNombreProducto}?","Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    // 3. Ejecutar la eliminación en el Controlador
                    bool bResultado = controlador.Ctr_EliminarProducto(iIdProducto);

                    if (bResultado)
                    {
                        MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDataGrid(); // Recargar DGV
                        // Aquí se llamaría a una función para limpiar campos de entrada.
                    }
                    else
                    {
                        MessageBox.Show("El producto no pudo ser eliminado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar eliminar producto: " + ex.Message, "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== Stevens Cambranes 13/11/2025 ====================
        // ==================== Evento CellClick: Carga la fila seleccionada a los campos ====================
        private void Dgv_Vista_Producto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = Dgv_Vista_Producto.Rows[e.RowIndex];

                    // Función auxiliar para manejar DBNull y devolver string.Empty o 0
                    Func<object, string> GetStringValue = (value) =>
            value == DBNull.Value ? string.Empty : value.ToString();

                    Func<object, string> GetNumericValue = (value) =>
                      value == DBNull.Value ? "0.00" : value.ToString();

                    Func<object, DateTime> GetDateValue = (value) =>
                      value == DBNull.Value ? DateTime.Now : Convert.ToDateTime(value);


                    // Guardamos el ID en el Tag del botón Modificar para usarlo en el CRUD
                    Btn_Modificar_Producto.Tag = row.Cells["Pk_ID"].Value;

                    // Cargar datos a los TextBoxes (Información Tbl_Producto)
                    Txt_Codigo_Producto.Text = GetStringValue(row.Cells["Codigo"].Value);
                    Txt_Nombre_Producto.Text = GetStringValue(row.Cells["Nombre"].Value);
                    Txt_Marca_Producto.Text = GetStringValue(row.Cells["Marca"].Value);
                    Txt_Descripcion_Producto.Text = GetStringValue(row.Cells["Descripcion"].Value);
                    Txt_Costo_Unitario_Producto.Text = GetNumericValue(row.Cells["Costo Unitario"].Value);

                    // NUEVOS CAMPOS: Información Tbl_Existencia
                    Txt_Cantidad_Producto.Text = GetNumericValue(row.Cells["Cantidad"].Value);
                    Txt_Existencias_Minimas_Producto.Text = GetNumericValue(row.Cells["ExistenciasMinimas"].Value);
                    Txt_Existencias_Maximas_Producto.Text = GetNumericValue(row.Cells["ExistenciasMaximas"].Value);
                    Txt_Precio_Venta_Producto.Text = GetNumericValue(row.Cells["Precio Venta"].Value);

                    // Cargar Fechas
                    Dtp_Fecha_Vencimiento_Producto.Value = GetDateValue(row.Cells["Vencimiento"].Value);

                    // Cargar ComboBoxes y Operación
                    Cbo_Unidad_Medida_Producto.Text = GetStringValue(row.Cells["Unidad"].Value);
                    Cbo_Estado_Producto.Text = GetStringValue(row.Cells["Estado"].Value);
                    Cbo_Almacen_Producto.Text = GetStringValue(row.Cells["Almacen"].Value);

                    // 3. Selección de Categoría (Radio Buttons)
                    string sCategoriaSeleccionada = GetStringValue(row.Cells["Categoria"].Value);

                    GroupBox gpbTipoProducto = this.Gpb_Tipo_Producto;
                    foreach (Control control in gpbTipoProducto.Controls)
                    {
                        if (control is RadioButton rdb)
                        {
                            // Buscamos la coincidencia con el Text (que cargamos con el nombre de la BD)
                            if (rdb.Text.Equals(sCategoriaSeleccionada, StringComparison.OrdinalIgnoreCase))
                            {
                                rdb.Checked = true;
                                // Forzar el prefijo de código si un Rdb es seleccionado
                                if (rdb.Checked) Rdb_TipoProducto_CheckedChanged(rdb, new EventArgs());
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos del producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Método para Cargar el DGV con Productos ====================
        private void CargarDataGrid()
        {
            try
            {
                DataTable dtResultado = controlador.Ctr_CargarTodosProductos();
                Dgv_Vista_Producto.DataSource = null;
                Dgv_Vista_Producto.DataSource = dtResultado;

                // Opcional: Autoajustar las columnas
                Dgv_Vista_Producto.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la tabla de productos: " + ex.Message, "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}