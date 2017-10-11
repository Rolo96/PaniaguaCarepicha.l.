using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la tabla de empleados 
    /// </summary>
    public class RepositorioSucursal
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);
        
        //Consulta todos los empleado de la base de datos
        public static List<Sucursal> ConsultarSucursal()
        {
            //Lista que almacena los empleado
            var lista = new List<Sucursal>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Provincia], [Cuidad], [Señas], [Descripcion], " +
                "[Compañia]" +
                "FROM[dbo].[SUCURSAL] WHERE Activo=1";
            //Se ejecuta el query
            try
            {

                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Sucursal
                        {
                            nombre = reader.GetString(0),
                            provincia = reader.GetString(1),
                            ciudad = reader.GetString(2),
                            senas = reader.GetString(3),
                            descripcion = reader.GetString(4),
                            compania = reader.GetString(5)
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e) { return null; }
        }

        public static bool BorrarSucursal(string nombre)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[SUCURSAL] SET [ACTIVO]=0" +
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

        public static bool AgregarSucursal(Sucursal sucursal)
        {
                //Query que consulta la base de datos
                var query = "INSERT INTO [dbo].[SUCURSAL] ([Nombre] ,[Provincia], [Cuidad], " +
                    "[Señas], [Descripcion] ,[Compañia] ,[Activo]) VALUES(@Nombre, @Provincia," +
                    " @Cuidad, @Señas, @Descripcion, @Compañia, @Activo)";
                //Se ejecuta el query
                try
                {
                    conexion.Close();
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Nombre", sucursal.nombre);
                    comando.Parameters.AddWithValue("@Provincia", sucursal.provincia);
                    comando.Parameters.AddWithValue("@Cuidad", sucursal.ciudad);
                    comando.Parameters.AddWithValue("@Señas", sucursal.senas);
                    comando.Parameters.AddWithValue("@Descripcion", sucursal.descripcion);
                    comando.Parameters.AddWithValue("@Compañia", sucursal.compania);
                    comando.Parameters.AddWithValue("@Activo", 1);
                    comando.ExecuteNonQuery();
                    comando.Dispose();
                    conexion.Close();
                if (AgregarAdministrador(sucursal)) { return true; }
                else { return false; }
                    
                }
                catch (Exception e) { return false; }
        }

        public static bool AgregarAdministrador(Sucursal sucursal)
        {
            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[ADMINISTRADORXSUCURSAL] ([Administrador] ,[Sucursal], " +
                "[Activo]) VALUES(@Admin, @Sucursal,@Activo)";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Admin", sucursal.administrador);
                comando.Parameters.AddWithValue("@Sucursal", sucursal.nombre);
                comando.Parameters.AddWithValue("@Activo", 1);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception e) { return false; }
        }


        public static bool ActualizarSucursal(Sucursal sucursal)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[SUCURSAL] SET [Provincia] = @Provincia, [Cuidad]=@Ciudad, " +
                "[Señas]=@Senas, [Descripcion]=@Descripcion, [Compañia]=@Compania " +
                "WHERE Nombre=@Nombre AND Activo=1";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Provincia", sucursal.provincia);
                comando.Parameters.AddWithValue("@Ciudad", sucursal.ciudad);
                comando.Parameters.AddWithValue("@Senas", sucursal.senas);
                comando.Parameters.AddWithValue("@Descripcion", sucursal.descripcion);
                comando.Parameters.AddWithValue("@Compania", sucursal.compania);
                comando.Parameters.AddWithValue("@Nombre", sucursal.nombre);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

    }
}