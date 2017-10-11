using GasStationPharmacy.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Processors;

namespace GasStationPharmacy.Controllers
{
    /// <summary>
    /// Controlador que acepta peticiones http sobre la tabla de empleados
    /// </summary>
    public class SucursalController : ApiController
    {
       
        [HttpGet]
        [Route("consultarSucursales")]
        public HttpResponseMessage ConsultarSucursal()
        {
            List<Sucursal> sucursales = ProcesadorSucursal.ProcesoConsultarSucursal();
            if (sucursales == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de empleados 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sucursales);
            return response;
        }

        [HttpPost]
        [Route("borrarSucursal")]
        public HttpResponseMessage BorrarSucursal(ObjGeneral obj)
        {
            if (!ProcesadorSucursal.ProcesoBorrarSucursal(obj.opcion))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [HttpPost]
        [Route("agregarSucursal")]
        public HttpResponseMessage AgregarSucursal(Sucursal sucursal)
        {
            if (sucursal == null || !ProcesadorSucursal.ProcesoAgregarSucursal(sucursal))
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [HttpPost]
        [Route("actualizarSucursal")]
        public HttpResponseMessage ActualizarSucursal(Sucursal sucursal)
        {
            if (!ProcesadorSucursal.ProcesoActualizarSucursal(sucursal))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}