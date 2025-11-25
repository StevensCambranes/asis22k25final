using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace Capa_Modelo_Inventario
{
    // ==================== Stevens Cambranes 05/11/2025 ====================
    // ==================== Clase Modelo Inventario ====================
    // (Esta clase ejecuta la lógica de BD. Habla con Cls_Conexion y Cls_Sentencias)
    public class Cls_Modelo_Inventario
    {
        // (Instancia de la clase de Conexión a BD)
        private Cls_Conexion cnx;
        // (Instancia de la clase de Sentencias SQL)
        private Cls_Sentencias_Inventario snt;

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Constructor ====================
        // (Inicializa las clases de conexión y sentencias)
        public Cls_Modelo_Inventario()
        {
            cnx = new Cls_Conexion();
            snt = new Cls_Sentencias_Inventario();
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Obtener Histórico (Movimientos) ====================
        public DataTable Mdl_ObtenerHistorico(
            string sTipoMovimiento,
            int? iIdAlmacen,
            int? iIdEstado,
            bool bUsarRangoFechas,
            DateTime dFechaInicio,
            DateTime dFechaFin,
            string sOrdenarPor)
        {
            DataTable dtResultado = new DataTable();

            try
            {
                // Pide a la clase de sentencias que construya el SQL
                List<object> lstObParametros;
                string sSql = snt.Snt_ConstruirSqlHistorico(
                    sTipoMovimiento, iIdAlmacen, iIdEstado,
                    bUsarRangoFechas, dFechaInicio, dFechaFin,
                    sOrdenarPor, out lstObParametros
                );

                // Usamos la estructura 'using' para garantizar el cierre y liberar recursos
                using (OdbcConnection conn = cnx.conexion())
                {
                    // Prepara el comando ODBC con el SQL y la conexión
                    OdbcCommand cmd = new OdbcCommand(sSql, conn);

                    // Agrega los parámetros a la consulta
                    foreach (var obParametro in lstObParametros)
                    {
                        cmd.Parameters.Add(new OdbcParameter(null, obParametro));
                    }

                    // Ejecuta la consulta y llena el DataTable
                    OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                    adapter.Fill(dtResultado);
                }
            }
            catch (Exception ex)
            {
                // Lanza el error para que el Controlador lo atrape
                throw new Exception("Error en Capa Modelo (Mdl_ObtenerHistorico): " + ex.Message);
            }
            return dtResultado; // Devuelve los datos
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Ejecutar Consulta Simple (Privado) ====================
        private DataTable Mdl_EjecutarConsultaSimple(string sSql)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                // Usamos la estructura 'using' para garantizar el cierre y liberar recursos
                using (OdbcConnection conn = cnx.conexion())
                {
                    OdbcCommand cmd = new OdbcCommand(sSql, conn);
                    OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                    adapter.Fill(dtResultado); // Llena la tabla con los resultados
                }
            }
            catch (Exception ex)
            {
                // Lanza el error para que el Controlador lo atrape
                throw new Exception("Error en Modelo al ejecutar consulta simple: " + ex.Message);
            }
            return dtResultado;
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Ejecutar Sentencia No Select (INSERT, UPDATE, DELETE) ====================
        private bool Mdl_EjecutarSentenciaNoSelect(string sSql, List<object> lstObParametros = null)
        {
            try
            {
                using (OdbcConnection conn = cnx.conexion())
                {
                    OdbcCommand cmd = new OdbcCommand(sSql, conn);

                    if (lstObParametros != null)
                    {
                        foreach (var obParametro in lstObParametros)
                        {
                            cmd.Parameters.Add(new OdbcParameter(null, obParametro));
                        }
                    }

                    int iFilasAfectadas = cmd.ExecuteNonQuery();
                    return iFilasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Modelo al ejecutar sentencia no SELECT: " + ex.Message);
            }
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Cargar Catálogos para Frm_Inventario ====================

        public DataTable Mdl_CargarCategorias()
        {
            // Nota: Se asume que Tbl_Categoria_Producto tiene Pk_ID y Cmp_Nombre
            string sSql = "SELECT Cmp_Id_Categoria_Producto AS Pk_ID, Cmp_Nombre_Categoria AS Cmp_Nombre FROM Tbl_Categoria_Producto ORDER BY Cmp_Nombre_Categoria;";
            return Mdl_EjecutarConsultaSimple(sSql);
        }

        public DataTable Mdl_CargarUnidadesMedida()
        {
            // Nota: Se asume que Tbl_Unidad_Medida tiene Pk_ID y Cmp_Nombre
            string sSql = "SELECT Cmp_Id_Unidad_Medida AS Pk_ID, Cmp_Nombre_Unidad AS Cmp_Nombre FROM Tbl_Unidad_Medida ORDER BY Cmp_Nombre_Unidad;";
            return Mdl_EjecutarConsultaSimple(sSql);
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Cargar ComboBox Almacenes (Mantenido) ====================
        public DataTable Mdl_CargarAlmacenes()
        {
            string sSql = snt.Snt_CargarAlmacenes();
            return Mdl_EjecutarConsultaSimple(sSql);
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Cargar ComboBox Estados (Mantenido) ====================
        public DataTable Mdl_CargarEstadosProducto()
        {
            string sSql = snt.Snt_CargarEstadosProducto();
            return Mdl_EjecutarConsultaSimple(sSql);
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Cargar ComboBox Tipo Movimiento (Mantenido) ====================
        public DataTable Mdl_CargarTiposMovimiento()
        {
            string sSql = snt.Snt_CargarTiposMovimiento();
            return Mdl_EjecutarConsultaSimple(sSql);
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Cargar DGV por Defecto (Mantenido) ====================
        public DataTable Mdl_CargarHistoricoDefault()
        {
            string sSql = snt.Snt_CargarHistoricoDefault();
            return Mdl_EjecutarConsultaSimple(sSql);
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Cargar Todos los Cierres (Mantenido) ====================
        public DataTable Mdl_CargarTodosLosCierres()
        {
            string sSql = snt.Snt_CargarTodosLosCierres();
            return Mdl_EjecutarConsultaSimple(sSql);
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Funciones CRUD de Productos ====================
        public bool Mdl_InsertarProducto(string sCodigo, string sNombre, string sMarca, string sDescripcion, DateTime dFechaVencimiento, int iIdCategoria, int iIdUnidad, double doPrecioUnitario,
                                        int iCantidad, int iExistMin, int iExistMax, int iIdAlmacen) // FIRMA FINAL
        {
            // 1. INSERT en Tbl_Producto
            string sSqlInsertProducto = "INSERT INTO Tbl_Producto (Cmp_Codigo_Producto, Cmp_Nombre_Producto, Cmp_Marca, Cmp_Descripcion, Cmp_Fecha_Vencimiento, Cmp_Id_Categoria_Producto, Cmp_Id_Unidad_Base, Cmp_PrecioUnitario, Cmp_Activo) VALUES (?, ?, ?, ?, ?, ?, ?, ?, 1)";
            // 2. Obtener el ID insertado
            string sSqlGetLastId = "SELECT LAST_INSERT_ID();";
            // 3. INSERT en Tbl_Existencia
            string sSqlInsertExistencia = "INSERT INTO Tbl_Existencia (Cmp_Id_Producto, Cmp_Id_Almacen, Cmp_Cantidad, Cmp_Cantidad_Minima, Cmp_Cantidad_Maxima) VALUES (?, ?, ?, ?, ?)";

            OdbcConnection conn = null;
            OdbcTransaction trans = null;
            try
            {
                conn = cnx.conexion();
                trans = conn.BeginTransaction();
                int iLastId = 0;

                // A. Insertar Producto y obtener ID
                using (OdbcCommand cmd1 = new OdbcCommand(sSqlInsertProducto, conn, trans))
                {
                    cmd1.Parameters.Add(new OdbcParameter(null, sCodigo));
                    cmd1.Parameters.Add(new OdbcParameter(null, sNombre));
                    cmd1.Parameters.Add(new OdbcParameter(null, sMarca));
                    cmd1.Parameters.Add(new OdbcParameter(null, sDescripcion));
                    cmd1.Parameters.Add(new OdbcParameter(null, dFechaVencimiento));
                    cmd1.Parameters.Add(new OdbcParameter(null, iIdCategoria));
                    cmd1.Parameters.Add(new OdbcParameter(null, iIdUnidad));
                    cmd1.Parameters.Add(new OdbcParameter(null, doPrecioUnitario));
                    cmd1.ExecuteNonQuery();

                    using (OdbcCommand cmdGetId = new OdbcCommand(sSqlGetLastId, conn, trans))
                    {
                        iLastId = Convert.ToInt32(cmdGetId.ExecuteScalar());
                    }
                }

                // B. Insertar Existencia Inicial (Usando el ID obtenido)
                using (OdbcCommand cmd2 = new OdbcCommand(sSqlInsertExistencia, conn, trans))
                {
                    cmd2.Parameters.Add(new OdbcParameter(null, iLastId));
                    cmd2.Parameters.Add(new OdbcParameter(null, iIdAlmacen));
                    cmd2.Parameters.Add(new OdbcParameter(null, iCantidad)); // Usando INT
                    cmd2.Parameters.Add(new OdbcParameter(null, iExistMin)); // Usando INT
                    cmd2.Parameters.Add(new OdbcParameter(null, iExistMax)); // Usando INT
                    cmd2.ExecuteNonQuery();
                }

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                throw new Exception("Error al insertar producto y existencia inicial (Transacción revertida). Detalles: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

            public bool Mdl_ModificarProducto(int iIdProducto, string sCodigo, string sNombre, string sMarca, string sDescripcion, DateTime dFechaVencimiento, int iIdCategoria, int iIdUnidad, double doPrecioUnitario)
        {
            string sSql = "UPDATE Tbl_Producto SET Cmp_Codigo_Producto = ?, Cmp_Nombre_Producto = ?, Cmp_Marca = ?, Cmp_Descripcion = ?, Cmp_Fecha_Vencimiento = ?, Cmp_Id_Categoria_Producto = ?, Cmp_Id_Unidad_Base = ?, Cmp_PrecioUnitario = ? WHERE Cmp_Id_Producto = ?";

            List<object> lstObParametros = new List<object> { sCodigo, sNombre, sMarca, sDescripcion, dFechaVencimiento, iIdCategoria, iIdUnidad, doPrecioUnitario, iIdProducto };

            return Mdl_EjecutarSentenciaNoSelect(sSql, lstObParametros);
        }

        // ==================== Stevens Cambranes 13/11/2025 ====================
        // ==================== Funciones CRUD de Productos ====================

        public bool Mdl_EliminarProducto(int iIdProducto)
        {
            // SENTENCIAS DE ELIMINACIÓN (Orden: Hija -> Padre)
            // 1. Eliminar referencias en Tbl_VentaDet (Nueva tabla hija)
            string sSqlDeleteVentaDet = "DELETE FROM Tbl_VentaDet WHERE Cmp_Id_Producto = ?";
            // 2. Eliminar referencias en Tbl_Mov_Inv_Det (Hija de Producto)
            string sSqlDeleteMovDet = "DELETE FROM Tbl_Mov_Inv_Det WHERE Cmp_Id_Producto = ?";
            // 3. Eliminar referencias en Tbl_Existencia (Hija de Producto)
            string sSqlDeleteExistencia = "DELETE FROM Tbl_Existencia WHERE Cmp_Id_Producto = ?";
            // 4. Eliminar Tbl_Producto (Padre)
            string sSqlDeleteProducto = "DELETE FROM Tbl_Producto WHERE Cmp_Id_Producto = ?";

            OdbcConnection conn = null;
            OdbcTransaction trans = null;
            try
            {
                // 1. Conexión y Transacción
                conn = cnx.conexion();
                trans = conn.BeginTransaction();

                // 2. Ejecutar DELETE en Tbl_VentaDet
                using (OdbcCommand cmd1 = new OdbcCommand(sSqlDeleteVentaDet, conn, trans))
                {
                    cmd1.Parameters.Add(new OdbcParameter(null, iIdProducto));
                    cmd1.ExecuteNonQuery();
                }

                // 3. Ejecutar DELETE en Tbl_Mov_Inv_Det
                using (OdbcCommand cmd2 = new OdbcCommand(sSqlDeleteMovDet, conn, trans))
                {
                    cmd2.Parameters.Add(new OdbcParameter(null, iIdProducto));
                    cmd2.ExecuteNonQuery();
                }

                // 4. Ejecutar DELETE en Tbl_Existencia
                using (OdbcCommand cmd3 = new OdbcCommand(sSqlDeleteExistencia, conn, trans))
                {
                    cmd3.Parameters.Add(new OdbcParameter(null, iIdProducto));
                    cmd3.ExecuteNonQuery();
                }

                // 5. Ejecutar DELETE en Tbl_Producto (Tabla padre)
                using (OdbcCommand cmd4 = new OdbcCommand(sSqlDeleteProducto, conn, trans))
                {
                    cmd4.Parameters.Add(new OdbcParameter(null, iIdProducto));
                    cmd4.ExecuteNonQuery();
                }

                trans.Commit(); // Confirma todas las operaciones
                return true;
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    // Si hay un error en cualquier paso, deshacer todo.
                    try { trans.Rollback(); } catch (Exception rbEx) { /* Log rollback failure */ }
                }
                // Relanza el error con más detalle.
                throw new Exception("Error al intentar eliminar el producto (Transacción revertida). Detalles: " + ex.Message);
            }
            finally
            {
                // Cierra la conexión
                if (conn != null) conn.Close();
            }
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Cargar Todos los Productos para DGV ====================
        public DataTable Mdl_CargarTodosProductos()
        {
            // ...
            string sSql = @"SELECT
        p.Cmp_Id_Producto AS Pk_ID,
        p.Cmp_Codigo_Producto AS Codigo,
        p.Cmp_Nombre_Producto AS Nombre,
        p.Cmp_Marca AS Marca,
        p.Cmp_Descripcion AS Descripcion,
        p.Cmp_PrecioUnitario AS 'Costo Unitario',
        um.Cmp_Abreviatura AS Unidad,
        cat.Cmp_Nombre_Categoria AS Categoria,
        p.Cmp_Fecha_Vencimiento AS Vencimiento,
        ex.Cmp_Cantidad AS Cantidad,
        ex.Cmp_Cantidad_Minima AS ExistenciasMinimas,
        ex.Cmp_Cantidad_Maxima AS ExistenciasMaximas,
        alm.Cmp_Nombre_Almacen AS Almacen,
        p.Cmp_PrecioUnitario * 1.25 AS 'Precio Venta',
        CASE WHEN p.Cmp_Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
        FROM Tbl_Producto p
        LEFT JOIN Tbl_Existencia ex ON p.Cmp_Id_Producto = ex.Cmp_Id_Producto
        LEFT JOIN Tbl_Almacen alm ON ex.Cmp_Id_Almacen = alm.Cmp_Id_Almacen
        JOIN Tbl_Categoria_Producto cat ON p.Cmp_Id_Categoria_Producto = cat.Cmp_Id_Categoria_Producto
        JOIN Tbl_Unidad_Medida um ON p.Cmp_Id_Unidad_Base = um.Cmp_Id_Unidad_Medida
        ORDER BY p.Cmp_Id_Producto DESC;";

            return Mdl_EjecutarConsultaSimple(sSql);
        }

    }
}