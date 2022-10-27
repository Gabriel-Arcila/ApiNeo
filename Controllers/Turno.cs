using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;
using ConsultasSQL.Logic;

namespace ConsultasSQL.Controllers{
    [ApiController]
    [Route("Turno")]

    public class TurnoController : ControllerBase
    {
        private TurnoLogic turnoLogic = new TurnoLogic();

        [HttpGet]
        [Route("ObtenerTurnoYGrupoActual")]
        public dynamic ObtenerTurnoYGrupoActual(){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(turnoLogic.ObtenerGrupoYTurno());
            return JSONString;
        }
    }
}