using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GasStationPharmacy.Models
{
    public class Sucursal
    {
        public string nombre { get; set; }
        public string provincia { get; set; }
        public string ciudad { get; set; }
        public string senas { get; set; }
        public string descripcion { get; set; }
        public string compania { get; set; }
        public int administrador { get; set; }
    }
}