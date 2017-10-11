using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones y demas de la tabla empleados
    /// </summary>
    public class ProcesadorSucursal
    {
        /// <summary>
        /// Consulta todos los empleados
        /// </summary>
        /// <returns>Lista de todos los empleados</returns>
        public static List<Sucursal> ProcesoConsultarSucursal()
        { return RepositorioSucursal.ConsultarSucursal(); }

        public static bool ProcesoBorrarSucursal(string nombre)
        { return RepositorioSucursal.BorrarSucursal(nombre); }

        public static bool ProcesoAgregarSucursal(Sucursal sucursal)
        { return RepositorioSucursal.AgregarSucursal(sucursal); }

        public static bool ProcesoActualizarSucursal(Sucursal sucursal)
        { return RepositorioSucursal.ActualizarSucursal(sucursal); }
    }
}