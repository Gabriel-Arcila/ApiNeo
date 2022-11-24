using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;
using ConsultasSQL.Logic;


namespace ConsultasSQL.Controllers{
    [ApiController]
    [Route("Gespline")]

    public class GesplineController : ControllerBase
    {
        private DbSIPDATABASE conexionSIPDATABASE = new DbSIPDATABASE();
        private SqlCommand comandSIPDATABASE = new SqlCommand();
        private Gespline gespline = new Gespline();

        [HttpGet]
        [Route("ObtenerParadasSegundoTurnoPorMaquina/{CentroCosto}")]
        public dynamic ObtenerParadasSegundoTurnoPorMaquina(string CentroCosto){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(gespline.obtenerParadasActuales2turno(CentroCosto));
            return JSONString;
        }

        [HttpGet]
        [Route("ObtenerParadasGesplienActualesAgrupados1turno/{CentroCosto}")]
        public dynamic ObtenerParadasGesplienActualesAgrupados1truno(string CentroCosto){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(gespline.obtenerParadasActuales1turnoAgrupados(CentroCosto));
            return JSONString;
        }

        [HttpGet]
        [Route("ObtenerParadasGesplienActualesAgrupados2turnoAntesDeLas0am/{CentroCosto}")]
        public dynamic ObtenerParadasGesplienActualesAgrupados2turnoAntesDeLas0am(string CentroCosto){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(gespline.obtenerParadasActuales2turnoAntesDeLas0amAgupados(CentroCosto));
            return JSONString;
        }

        [HttpGet]
        [Route("ObtenerParadasGesplienActualesAgrupados2turnoDespuesDeLas0am/{CentroCosto}")]
        public dynamic ObtenerParadasGesplienActualesAgrupados2turnoDespuesDeLas0am(string CentroCosto){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(gespline.obtenerParadasActuales2turnoDespuesDeLas0amAgrupados(CentroCosto));
            return JSONString;
        }

        [HttpGet]
        [Route("ObtenerParadasSegundoTurnoPorMaquina/{CentroCosto}/{cadenas}")]
        public dynamic ObtenerParadasSegundoTurnoPorMaquina(string CentroCosto,string cadenas){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(gespline.obtenerParadasActualesConFiltro(gespline.obtenerParadasActuales2turno(CentroCosto),cadenas));
            return JSONString;
        }

        [HttpGet]
        [Route("obtenerParadasActuales1turnoPorLinea/{centroCosto}")]
        public dynamic obtenerParadasActuales1turnoPorLinea(string centroCosto){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(gespline.obtenerParadasActuales1turno(centroCosto));
            return JSONString;
        }

        [HttpGet]
        [Route("obtenerParadasActuales1turnoPorLinea/{centroCosto}/{cadenas}")]
        public dynamic obtenerParadasActuales1turnoPorLineaPorFiltroYaRegistradosenLibroNove(string centroCosto,string cadenas){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(gespline.obtenerParadasActualesConFiltro(gespline.obtenerParadasActuales1turno(centroCosto),cadenas));
            return JSONString;
        }
    }
}