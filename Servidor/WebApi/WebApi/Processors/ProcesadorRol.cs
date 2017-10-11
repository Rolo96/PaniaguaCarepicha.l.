using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones y demas de la tabla empleados
    /// </summary>
    public class ProcesadorRol
    {
        /// <summary>
        /// Consulta todos los empleados
        /// </summary>
        /// <returns>Lista de todos los empleados</returns>
        public static List<Rol> ProcesoConsultarRol()
        { return RepositorioRol.ConsultarRoles(); }

        public static bool ProcesoBorrarRol(string nombre)
        { return RepositorioRol.BorrarRol(nombre); }

        public static bool ProcesoAgregarRol(Rol rol)
        { return RepositorioRol.AgregarRol(rol); }

        public static bool ProcesoActualizarRol(Rol rol)
        { return RepositorioRol.ActualizarRol(rol); }
    }
}