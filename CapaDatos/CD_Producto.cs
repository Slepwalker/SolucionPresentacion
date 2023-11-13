using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Producto
    {
        public List<Producto> listar()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select p.idProducto, p.nombre,p.descripcion");
                    sb.AppendLine("m.idMarca,m.descripcion[DesMarca],");
                    sb.AppendLine("c.idCategoria,c,descripcion[DesCategoria],");
                    sb.AppendLine("p.precio,p.stock,p.rutaImagen,p.nombreImagen,p.activo");
                    sb.AppendLine("from PRODUCTO p");
                    sb.AppendLine("inner join MARCA m on m.idMarca = p.idMarca");
                    sb.AppendLine("inner join CATEGORIA C on m.idCategoria = p.idCategoria");
                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {

                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Producto>();
            }
            return lista;
        }
        
    }
}
