using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;

namespace ConsultasSQL.Logic{
    public class TurnoLogic 
    {
        private DbIngDoc conexionTurno = new DbIngDoc();

        private SqlCommand comandTurno = new SqlCommand();
        SqlDataReader? DataReaderTurno;

        public List<string> ObtenerGrupoYTurno(){
            int hora = DateTime.Now.Hour;
            string turno = "";
            string fecha = "";
            List<string> datos = new List<string>(2);
            if(hora >= 6 && hora < 18){
                turno = "1";
                fecha  = DateTime.Now.ToString("yyyyMMdd");
            }else if(hora >= 0 && hora < 6){
                turno = "2";
                fecha = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            }else{
                turno = "2";
                fecha = DateTime.Now.ToString("yyyyMMdd");
            }
            var dataTable = new DataTable();
            comandTurno.Connection = conexionTurno.OpeAbrirConex();
            comandTurno.CommandText = @"
                SELECT RCTurno,RCGrupo
                FROM dbo.RotaCalida
                WHERE RCFecha = '" + fecha + "' AND RCTurno = '"+ turno + "';";
            DataReaderTurno = comandTurno.ExecuteReader();
            dataTable.Load(DataReaderTurno);
            foreach (DataRow row in dataTable.Rows)
            {
                datos.Add(row["RCTurno"].ToString());
                datos.Add(row["RCGrupo"].ToString());
                break;
            }
            return datos;
        }
    }
}