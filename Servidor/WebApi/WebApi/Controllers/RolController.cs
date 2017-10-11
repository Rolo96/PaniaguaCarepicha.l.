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
    public class RolController : ApiController
    {
       
        [HttpGet]
        [Route("consultarRoles")]
        public HttpResponseMessage ConsultarRol()
        {
            List<Rol> roles = ProcesadorRol.ProcesoConsultarRol();
            if (roles == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de empleados 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, roles);
            return response;
        }

        [HttpPost]
        [Route("borrarRol")]
        public HttpResponseMessage BorrarRol(ObjGeneral obj)
        {
            if (!ProcesadorRol.ProcesoBorrarRol(obj.opcion))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [HttpPost]
        [Route("agregarRol")]
        public HttpResponseMessage AgregarRol(Rol rol)
        {
            if (rol == null || !ProcesadorRol.ProcesoAgregarRol(rol))
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [HttpPost]
        [Route("actualizarRol")]
        public HttpResponseMessage ActualizarRol(Rol rol)
        {
            if (!ProcesadorRol.ProcesoActualizarRol(rol))
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