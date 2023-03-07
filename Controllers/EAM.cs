using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;
using ConsultasSQL.Logic;


namespace ConsultasSQL.Controllers{

    [ApiController]
    [Route("EAM")]

    public class EAMController : ControllerBase
    { 
        private EAM eam = new EAM();

        [HttpGet]
        [Route("ObtenerEquiposEAM/{CentroCosto}")]
        public dynamic ObtenerEquiposEAM(string CentroCosto){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(eam.ObtenerEquiposEAMSegunCentroDeCosto(CentroCosto));
            return JSONString;
        }
    }
}