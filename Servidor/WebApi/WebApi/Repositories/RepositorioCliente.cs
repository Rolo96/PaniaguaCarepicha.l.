using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la tabla de clientes 
    /// </summary>
    public class RepositorioCliente
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        /// <summary>
        /// Verifica si existe un cliente en base de datos
        /// </summary>
        /// <param name="cedula">cedula del cliente ingresada</param>
        /// <param name="contrasena">contraseña del cliente ingresada</param>
        /// <returns>true si el cliente existe
        /// false en caso contrario</returns>
        public static bool LogearCliente(int cedula, string contrasena)
        {
            //query de la solicitud
            var query = "SELECT [Nombre1], [Apellido1] FROM[dbo].[CLIENTE] " +
                "WHERE Activo=1 AND Cedula= @cedula AND Contrasena= @contrasena";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.Parameters.AddWithValue("@contrasena", contrasena);
                var reader = comando.ExecuteReader();
                //No se encuentra el cliente
                if (!reader.HasRows) { conexion.Close(); return false; }
                //Si se encontro el cliente
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }


        //Consulta todos los clientes de la base de datos
        public static List<Cliente> ConsultarClientes()
        {
            //Lista que almacena los clientes
            var lista = new List<Cliente>();
            //Query que consulta la base de datos
            var query = "SELECT [Cedula], [Nombre1], [Nombre2], [Apellido1], [Apellido2], [Provincia]," +
                "[Cuidad], [Senas], [FechaNacimiento], [Prioridad] FROM[dbo].[CLIENTE] WHERE Activo=1";

            //Se ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Cliente {cedula = reader.GetInt32(0),
                            nombre1 = reader.GetString(1),
                            nombre2 = reader.GetString(2),
                            apellido1 = reader.GetString(3),
                            apellido2 = reader.GetString(4),
                            provincia = reader.GetString(5),
                            ciudad = reader.GetString(6),
                            senas = reader.GetString(7),
                            fechaNacimiento = reader.GetDateTime(8).ToString(),
                            contrasena = "",
                            activo = true
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }         
        }

        /// <summary>
        /// Crea un cliente nuevo
        /// </summary>
        /// <param name="cliente">Modelo de cliente a insertar</param>
        /// <returns></returns>
        public static bool AgregarCliente(Cliente cliente)
        {

            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[CLIENTE] ([Cedula] ,[Nombre1], [Nombre2] ,[Apellido1], " +
                "[Apellido2] ,[Provincia] ,[Cuidad] ,[Senas] ,[FechaNacimiento] ,[Prioridad] , " +
                "[Contrasena] ,[Activo]) VALUES(@Cedula, @Nombre1, @Nombre2, @Apellido1," +
                "@Apellido2, @Provincia, @Cuidad, @Senas, @FechaNacimiento, @Prioridad," +
                "@Contraseña, @Activo)";

            //Se ejecuta el query
            try
            {

                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cedula", cliente.cedula);
                comando.Parameters.AddWithValue("@Contraseña", cliente.contrasena);
                comando.Parameters.AddWithValue("@Nombre1", cliente.nombre1);
                comando.Parameters.AddWithValue("@Nombre2", cliente.nombre2);
                comando.Parameters.AddWithValue("@Apellido1", cliente.apellido1);
                comando.Parameters.AddWithValue("@Apellido2", cliente.apellido2);
                comando.Parameters.AddWithValue("@Provincia", cliente.provincia);
                comando.Parameters.AddWithValue("@Cuidad", cliente.ciudad);
                comando.Parameters.AddWithValue("@Senas", cliente.senas);
                comando.Parameters.AddWithValue("@FechaNacimiento", cliente.fechaNacimiento);
                comando.Parameters.AddWithValue("@Prioridad", 1);
                comando.Parameters.AddWithValue("@Activo", 1);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();

                if (AgregarTelefonos(cliente)) {
                    if (AgregarPadecimientos(cliente)){return true;}
                    else {return false;}
                }
                else {return false;}
            }
            catch (Exception){return false;}
        }

        public static bool BorrarCliente(int cedula)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[CLIENTE] SET [ACTIVO]=0" +
                "WHERE Cedula= @cedula";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        public static bool ActualizarContraseña(int cedula, string contrasenaActual, string contrasenaNueva)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[CLIENTE] SET [Contrasena]=@nueva " +
                "WHERE Cedula= @cedula AND Contrasena = @vieja";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.Parameters.AddWithValue("@nueva", contrasenaNueva);
                comando.Parameters.AddWithValue("@vieja", contrasenaActual);
                var reader = comando.ExecuteReader();
                //No se encuentra el cliente
                if (reader.RecordsAffected<1) { conexion.Close(); return false; }
                //Si se encontro el cliente
                conexion.Close();
                return true;
            }
            catch (Exception) { conexion.Close(); return false; }
        }


        public static bool AgregarTelefonos(Cliente cliente)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            String[] telefonos = js.Deserialize<String[]>(cliente.telefonos);


            foreach (string telefono in telefonos)
            {
                //Query que consulta la base de datos
                var query = "INSERT INTO [dbo].[TELEFONOXCLIENTE] ([Cliente] ,[Telefono], [Activo]) " +
                    " VALUES(@Cliente, @Telefono, @Activo)";
                //Se ejecuta el query
                try
                {
                    conexion.Close();
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Cliente", cliente.cedula);
                    comando.Parameters.AddWithValue("@Telefono", int.Parse(telefono));
                    comando.Parameters.AddWithValue("@Activo", 1);
                    comando.ExecuteNonQuery();
                    comando.Dispose();
                    conexion.Close();
                }
                catch (Exception) { return false; }
            }
            return true;
        }

        public static bool AgregarPadecimientos(Cliente cliente)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Padecimiento[] padecimientos = js.Deserialize<Padecimiento[]>(cliente.padecimientos);

            foreach (Padecimiento padecimiento in padecimientos)
            {
                //Query que consulta la base de datos
                var query = "INSERT INTO [dbo].[PADECIMIENTO] ([Cliente] ,[Padecimiento], [Año], [Activo]) " +
                    " VALUES(@Cliente, @Padecimiento, @Año, @Activo)";
                //Se ejecuta el query
                try
                {
                    conexion.Close();
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Cliente", cliente.cedula);
                    comando.Parameters.AddWithValue("@Padecimiento",padecimiento.padecimiento);
                    comando.Parameters.AddWithValue("@Año", padecimiento.año);
                    comando.Parameters.AddWithValue("@Activo", 1);
                    comando.ExecuteNonQuery();
                    comando.Dispose();
                    conexion.Close();
                }
                catch (Exception) { return false; }
            }
            return true;
        }

        public static List<Padecimiento> ConsultarPadecimientos(int cedula)
        {
            var lista = new List<Padecimiento>();
            //Query que consulta la base de datos
            var query = "SELECT [Padecimiento], [Año] FROM[dbo].[PADECIMIENTO] " +
                "WHERE Activo=1 AND Cliente=@Cliente";

            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cliente", cedula);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Padecimiento
                        {
                            padecimiento = reader.GetString(0),
                            año = reader.GetInt32(1)
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        public static List<string> ConsultarTelefonos(int cedula)
        {
            var lista = new List<String>();
            //Query que consulta la base de datos
            var query = "SELECT [Telefono] FROM[dbo].[TELEFONOXCLIENTE] " +
                "WHERE Activo=1 AND Cliente=@Cliente";

            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cliente", cedula);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(
                            reader.GetInt32(0).ToString()
                        );
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        public static List<ClienteCompleto> ConsultarModClientes(int cedula)
        {
            //Lista que almacena los clientes
            var lista = new List<ClienteCompleto>();
            //Query que consulta la base de datos
            var query = "SELECT [Cedula], [Nombre1], [Nombre2], [Apellido1], [Apellido2], [Provincia]," +
                "[Cuidad], [Senas], [FechaNacimiento], [Prioridad] FROM[dbo].[CLIENTE] WHERE Activo=1 " +
                "AND Cedula=@cedula";

            //Se ejecuta el query
            try
            {
                List<Padecimiento> padecimientos = ConsultarPadecimientos(cedula);
                List<string> telefonos = ConsultarTelefonos(cedula);
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                var reader = comando.ExecuteReader();
                reader.Read();
                lista.Add(new ClienteCompleto
                {
                    cedula = reader.GetInt32(0),
                    nombre1 = reader.GetString(1),
                    nombre2 = reader.GetString(2),
                    apellido1 = reader.GetString(3),
                    apellido2 = reader.GetString(4),
                    provincia = reader.GetString(5),
                    ciudad = reader.GetString(6),
                    senas = reader.GetString(7),
                    fechaNacimiento = reader.GetDateTime(8).ToString(),
                    contrasena = "",
                    activo = true,
                    telefonos = telefonos,
                    padecimientos = padecimientos
                });
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        public static bool BorradoFisicoTelefono(int cedula)
        {
            //query de la solicitud
            var query = "DELETE FROM [dbo].[TELEFONOXCLIENTE] "+
                "WHERE Cliente= @cedula";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        public static bool BorradoFisicoPadecimiento(int cedula)
        {
            //query de la solicitud
            var query = "DELETE FROM [dbo].[PADECIMIENTO] " +
                "WHERE Cliente= @cedula";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        public static bool ActualizarCliente(Cliente cliente)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[CLIENTE] SET [Nombre1] = @nombre1, [Nombre2]=@nombre2, " +
                "[Apellido1]=@apellido1, [Apellido2]=@apellido2, [Provincia]=@provincia," +
                "[Cuidad]=@ciudad ,[Senas]=@senas ,[FechaNacimiento]=@fecha " +
                "WHERE Cedula=@cedula AND Activo=1";

            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula",cliente.cedula);
                comando.Parameters.AddWithValue("@nombre1", cliente.nombre1);
                comando.Parameters.AddWithValue("@nombre2", cliente.nombre2);
                comando.Parameters.AddWithValue("@apellido1", cliente.apellido1);
                comando.Parameters.AddWithValue("@apellido2", cliente.apellido2);
                comando.Parameters.AddWithValue("@provincia", cliente.provincia);
                comando.Parameters.AddWithValue("@ciudad", cliente.ciudad);
                comando.Parameters.AddWithValue("@senas", cliente.senas);
                comando.Parameters.AddWithValue("@fecha", cliente.fechaNacimiento);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();


                if (BorradoFisicoTelefono(cliente.cedula) && AgregarTelefonos(cliente))
                {
                    if (BorradoFisicoPadecimiento(cliente.cedula) && AgregarPadecimientos(cliente)) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            catch (Exception) { return false; }
        }

    }
}