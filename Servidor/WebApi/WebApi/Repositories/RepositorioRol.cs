using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la tabla de empleados 
    /// </summary>
    public class RepositorioRol
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);
        
        //Consulta todos los empleado de la base de datos
        public static List<Rol> ConsultarRoles()
        {
            //Lista que almacena los empleado
            var lista = new List<Rol>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Descripcion] " +
                "FROM[dbo].[ROL] WHERE Activo=1";

            //Se ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Rol
                        {
                            nombre = reader.GetString(0),
                            descripcion = reader.GetString(1)
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        public static bool BorrarRol(string nombre)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[ROL] SET [ACTIVO]=0" +
                "WHERE Nombre= @nombre";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombre);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        public static bool AgregarRol(Rol rol)
        {
            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[ROL] ([Nombre] ,[Descripcion], [Activo]) " +
                "VALUES(@Nombre, @Descripcion, @Activo)";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Nombre", rol.nombre);
                comando.Parameters.AddWithValue("@Descripcion", rol.descripcion);
                comando.Parameters.AddWithValue("@Activo", 1);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        public static bool ActualizarRol(Rol rol)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[ROL] SET [Descripcion] = @Descripcion " +
                "WHERE Nombre=@Nombre AND Activo=1";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Descripcion", rol.descripcion);
                comando.Parameters.AddWithValue("@Nombre", rol.nombre);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}