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
            List<string> datos = new List<string>(2);
            if(hora >= 6 && hora < 18){
                turno = "1";
            }else{
                turno = "2";
            }
            var dataTable = new DataTable();
            var a = DateTime.Now.ToString("yyyyMMdd");
            comandTurno.Connection = conexionTurno.OpeAbrirConex();
            comandTurno.CommandText = @"
                SELECT RCTurno,RCGrupo
                FROM dbo.RotaCalida
                WHERE RCFecha = '" + DateTime.Now.ToString("yyyyMMdd") + "' AND RCTurno = '"+ turno + "';";
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