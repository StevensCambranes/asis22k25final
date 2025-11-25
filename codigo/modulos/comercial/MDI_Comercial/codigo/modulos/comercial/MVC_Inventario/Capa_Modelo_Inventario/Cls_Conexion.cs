// ==================== Stevens Cambranes 12/11/2025 ====================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace Capa_Modelo_Inventario
{
    public class Cls_Conexion
    {
        public OdbcConnection conexion()
        {
            // La cadena de conexión debe ser movida a Web.config o App.config
            OdbcConnection conn = new OdbcConnection("Dsn=bd_hoteleria");
            try
            {
                conn.Open();
                return conn;
            }
            catch (OdbcException ex)
            {
                // para que la Capa Modelo sepa que falló la conexión.
                throw new Exception("Error al conectar con la base de datos ODBC (DSN=bd_hoteleria).", ex);
            }
        }
    }
}