using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Globalization;

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
                    sb.AppendLine("select p.idProducto, p.nombre,p.descripcion,");
                    sb.AppendLine("m.idMarca,m.descripcion[DesMarca],");
                    sb.AppendLine("c.idCategoria,c.descripcion[DesCategoria],");
                    sb.AppendLine("p.precio,p.stock,p.rutaImagen,p.nombreImagen,p.activo");
                    sb.AppendLine("from PRODUCTO p");
                    sb.AppendLine("inner join MARCA m on m.idMarca = p.idMarca");
                    sb.AppendLine("inner join CATEGORIA C on c.idCategoria = p.idCategoria");
                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NomProducto = dr["nombre"].ToString(),
                                DesProducto = dr["descripcion"].ToString(),
                                oMarca = new Marca() {IdMarca = Convert.ToInt32(dr["idMarca"]), Descripcion = dr["descripcion"].ToString()},
                                oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(dr["idCategoria"]), Descripcion = dr["DesCategoria"].ToString() },
                                Precio = Convert.ToDecimal(dr["precio"], new CultureInfo("es-PE")),
                                Stock = Convert.ToInt32(dr["stock"]),
                                RutaImagen = dr["rutaImagen"].ToString(),
                                NombreImagen = dr["nombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(dr["activo"])
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

        public int Registrar(Producto obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using(SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarProducto", oconexion);
                    cmd.Parameters.AddWithValue ("nombre",obj.NomProducto);
                    cmd.Parameters.AddWithValue ("descripcion",obj.DesProducto);
                    cmd.Parameters.AddWithValue ("idMarca",obj.oMarca);
                    cmd.Parameters.AddWithValue ("idCategoria",obj.oCategoria);
                    cmd.Parameters.AddWithValue ("precio",obj.Precio);
                    cmd.Parameters.AddWithValue ("stock",obj.Stock);
                    cmd.Parameters.AddWithValue ("activo",obj.Activo);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    idautogenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch(Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_EditarProducto", oconexion);
                    cmd.Parameters.AddWithValue("idProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("nombre", obj.NomProducto);
                    cmd.Parameters.AddWithValue("descripcion", obj.DesProducto);
                    cmd.Parameters.AddWithValue("idMarca", obj.oMarca);
                    cmd.Parameters.AddWithValue("idCategoria", obj.oCategoria);
                    cmd.Parameters.AddWithValue("precio", obj.Precio);
                    cmd.Parameters.AddWithValue("stock", obj.Stock);
                    cmd.Parameters.AddWithValue("activo", obj.Activo);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                string query = "update producto set rutaImagen = @RutaImagen, nombreImagen=@NombreImagen where idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand("usp_RegistrarProducto", oconexion);
                cmd.Parameters.AddWithValue("@RutaImagen", obj.RutaImagen);
                cmd.Parameters.AddWithValue("@NombreImagen", obj.NombreImagen);
                cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar imagen";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }   



        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarProducto", oconexion);
                    cmd.Parameters.AddWithValue("idProducto", id);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
