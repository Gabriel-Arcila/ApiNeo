using Microsoft.AspNetCore.Mvc;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;

namespace ConsultasSQL.Controllers{
    [ApiController]
    [Route("BPCS")]
    public class BPCSController : ControllerBase
    {
        private DBconexionBPCS conexionBPCS = new DBconexionBPCS();
        private OleDbCommand obj = new OleDbCommand();
        OleDbDataReader objResult;

        [HttpGet]
        [Route("Disponibilidad")]
        public dynamic obtenerDisponibilidad(){
            obj.Connection = conexionBPCS.CodAbrirConex();
            obj.CommandText =  "SELECT ITH.TPROD, ITH.TTYPE, ITH.TTDTE, ITH.TQTY, ITH.TWHS, ITH.TRES, ITH.THTIME, ITH.THORD, ITH.THWRKC FROM C20A237W.VENLX835F.ITH ITH WHERE (ITH.TTYPE='R') AND (ITH.TTDTE>=20220804) AND (ITH.TWHS='PT ')";
            objResult = obj.ExecuteReader();

            var dataTable = new DataTable();

            dataTable.Load(objResult);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dataTable);

            return JSONString;
        }
    }
}