using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string cnx = "select idUsuario, nombres, apellidos, correo, clave, reestablecer, activo from USUARIOS";
                    SqlCommand cmd = new SqlCommand(cnx, oconexion);
                    cmd.CommandType=CommandType.Text;
                    oconexion.Open();
                    using(SqlDataReader dr = cmd.ExecuteReader()) 
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Usuario()
                                {
                                    IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                                    NomUsuario = dr["nombres"].ToString(),
                                    ApeUsuario = dr["apellidos"].ToString(),
                                    Correo = dr["correo"].ToString(),
                                    Clave = dr["clave"].ToString(),
                                    Reestablecer = Convert.ToBoolean(dr["reestablecer"]),
                                    Activo = Convert.ToBoolean(dr["activo"])
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Usuario>();
            }
            return lista;
        }
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using(SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("nombres", obj.NomUsuario);
                    cmd.Parameters.AddWithValue("apellidos", obj.ApeUsuario);
                    cmd.Parameters.AddWithValue("correo", obj.Correo);
                    cmd.Parameters.AddWithValue("clave", obj.Clave);
                    cmd.Parameters.AddWithValue("activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }
        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_EditarUsuario1", oconexion);
                    cmd.Parameters.AddWithValue("idUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("nombres", obj.NomUsuario);
                    cmd.Parameters.AddWithValue("apellidos", obj.ApeUsuario);
                    cmd.Parameters.AddWithValue("correo", obj.Correo);
                    cmd.Parameters.AddWithValue("activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
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
        public bool Eliminar (int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top (1) from USUARIOS where idUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }catch (Exception ex) 
            { 
                resultado = false; 
                Mensaje = ex.Message; 
            }
            return resultado;
        }

    }
}
