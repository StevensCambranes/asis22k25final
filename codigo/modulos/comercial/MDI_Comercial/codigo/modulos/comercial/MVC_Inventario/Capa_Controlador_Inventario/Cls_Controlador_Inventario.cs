using System;
using System.Collections.Generic;
using System.Data;
using Capa_Modelo_Inventario; // Usamos la capa Modelo

namespace Capa_Controlador_Inventario
{
    // ==================== Stevens Cambranes 05/11/2025 ====================
    // ==================== Clase Controlador Inventario ====================
    // (Esta clase actúa como intermediario entre la Vista y el Modelo)
    public class Cls_Controlador_Inventario
    {
        // (Instancia global de la capa Modelo para acceder a la BD)
        private Cls_Modelo_Inventario modelo;

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Constructor ====================
        // (Inicializa el objeto 'modelo' al crear el controlador)
        public Cls_Controlador_Inventario()
        {
            modelo = new Cls_Modelo_Inventario();
        }

        // ==================== Stevens Cambranes 05/11/2025 ====================
        // ==================== Obtener Histórico (Movimientos) ====================
        // (Pasa la solicitud de la Vista al Modelo para buscar Movimientos filtrados)
        public DataTable Ctr_ObtenerHistorico(
            string sTipoMovimiento, // Estandarizado
            int? iIdAlmacen, // Estandarizado
            int? iIdEstado, // Estandarizado
            bool bUsarRangoFechas, // Estandarizado
            DateTime dFechaInicio, // Estandarizado
            DateTime dFechaFin, // Estandarizado
            string sOrdenarPor) // Estandarizado
        {
            try
            {
                // Llama al método correspondiente en el Modelo
                return modelo.Mdl_ObtenerHistorico(
                    sTipoMovimiento, iIdAlmacen, iIdEstado,
                    bUsarRangoFechas, dFechaInicio, dFechaFin,
                    sOrdenarPor
                );
            }
            catch (Exception ex)
            {
                // Captura y relanza el error para que la Vista lo muestre
                throw new Exception("Error en Capa Controlador: " + ex.Message);
            }
        }

        // Los siguientes métodos no requieren cambio en la firma, solo en la implementación si fuera necesario.

        public DataTable Ctr_CargarAlmacenes()
        {
            try
            {
                return modelo.Mdl_CargarAlmacenes();
            }
            catch (Exception ex)
            {
                // Pasa el error a la Vista
                throw new Exception("Error en Controlador al cargar almacenes: " + ex.Message);
            }
        }

        public DataTable Ctr_CargarEstadosProducto()
        {
            try
            {
                return modelo.Mdl_CargarEstadosProducto();
            }
            catch (Exception ex)
            {
                // Pasa el error a la Vista
                throw new Exception("Error en Controlador al cargar estados: " + ex.Message);
            }
        }

        public DataTable Ctr_CargarTodosLosCierres()
        {
            try
            {
                return modelo.Mdl_CargarTodosLosCierres();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Controlador (Ctr_CargarTodosLosCierres): " + ex.Message);
            }
        }

        public DataTable Ctr_CargarTiposMovimiento()
        {
            try
            {
                return modelo.Mdl_CargarTiposMovimiento();
            }
            catch (Exception ex)
            {
                // Pasa el error a la Vista
                throw new Exception("Error en Controlador al cargar tipos de movimiento: " + ex.Message);
            }
        }

        public DataTable Ctr_CargarHistoricoDefault()
        {
            try
            {
                return modelo.Mdl_CargarHistoricoDefault();
            }
            catch (Exception ex)
            {
                // Pasa el error a la Vista
                throw new Exception("Error en Controlador al cargar histórico default: " + ex.Message);
            }
        }

        // Para Frm_Inventario

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Cargar Catálogos para Frm_Inventario ====================
        public DataTable Ctr_CargarCategorias()
        {
            try
            {
                return modelo.Mdl_CargarCategorias();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Controlador al cargar categorías: " + ex.Message);
            }
        }

        public DataTable Ctr_CargarUnidadesMedida()
        {
            try
            {
                return modelo.Mdl_CargarUnidadesMedida();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Controlador al cargar unidades de medida: " + ex.Message);
            }
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Funciones CRUD de Productos ====================

        public bool Ctr_InsertarProducto(string sCodigo, string sNombre, string sMarca, string sDescripcion, DateTime dFechaVencimiento, int iIdCategoria, int iIdUnidad, double doPrecioUnitario,
                                int iCantidad, int iExistMin, int iExistMax, int iIdAlmacen) // FIRMA FINAL
        {
            try
            {
                // Validación 1: Campos obligatorios de texto (ya validados en la Vista, esto es un respaldo)
                if (string.IsNullOrEmpty(sCodigo) || string.IsNullOrEmpty(sNombre))
                {
                    throw new Exception("El Código y el Nombre del producto son campos obligatorios.");
                }
                // Validación 2: Evitar que el código sea solo el prefijo (evita el error 'Duplicate Entry')
                if (sCodigo.Length <= 5)
                {
                    throw new Exception("El Código del Producto es demasiado corto. Debe ser completado (Ej: LIMP-001).");
                }
                if (iIdCategoria <= 0)
                {
                    throw new Exception("Debe seleccionar una Categoría de Producto.");
                }

                return modelo.Mdl_InsertarProducto(sCodigo, sNombre, sMarca, sDescripcion, dFechaVencimiento, iIdCategoria, iIdUnidad, doPrecioUnitario,
                                                    iCantidad, iExistMin, iExistMax, iIdAlmacen);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Controlador al insertar producto: " + ex.Message);
            }
        }

        public bool Ctr_ModificarProducto(int iIdProducto, string sCodigo, string sNombre, string sMarca, string sDescripcion, DateTime dFechaVencimiento, int iIdCategoria, int iIdUnidad, double doPrecioUnitario)
        {
            try
            {
                if (iIdProducto <= 0)
                {
                    throw new Exception("Debe seleccionar un producto válido para modificar.");
                }
                return modelo.Mdl_ModificarProducto(iIdProducto, sCodigo, sNombre, sMarca, sDescripcion, dFechaVencimiento, iIdCategoria, iIdUnidad, doPrecioUnitario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Controlador al modificar producto: " + ex.Message);
            }
        }

        public bool Ctr_EliminarProducto(int iIdProducto)
        {
            try
            {
                if (iIdProducto <= 0)
                {
                    throw new Exception("Debe seleccionar un producto válido para eliminar.");
                }
                return modelo.Mdl_EliminarProducto(iIdProducto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Controlador al eliminar producto: " + ex.Message);
            }
        }

        // ==================== Stevens Cambranes 12/11/2025 ====================
        // ==================== Cargar Todos los Productos (Paso al Modelo) ====================
        public DataTable Ctr_CargarTodosProductos()
        {
            try
            {
                return modelo.Mdl_CargarTodosProductos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Controlador al cargar la lista de productos: " + ex.Message);
            }
        }
    }
}