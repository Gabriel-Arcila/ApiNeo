using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;

namespace ConsultasSQL.Controllers{
    [ApiController]
    [Route("OEE")]

    public class IngDocController : ControllerBase
    {
        private DbIngDoc conexionIngDoc = new DbIngDoc();
        private SqlCommand CommandIngDoc = new SqlCommand();
        private SqlDataReader? DataReaderIngDoc;

        private DBconexionBPCS conexionBPCS = new DBconexionBPCS();
        private OleDbCommand CommandBPCS = new OleDbCommand();
        OleDbDataReader? DataReaderBPCS;


        private DbSIPDATABASE conexionSIPDATABASE = new DbSIPDATABASE();
        private SqlCommand comandSIPDATABASE = new SqlCommand();

        SqlDataReader? DataReaderSIPDATABASE;

        [HttpGet]
        [Route("objePorHoraProductoActualEstandar")]
        public dynamic objePorHoraProductoActualEstandar(){
            Dictionary<string,int> diccionario = new Dictionary<string,int>();
            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                        SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY) AS Produccion 
                        FROM C20A237W.VENLX835F.ITH ITH 
                        WHERE (ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") +@") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000) 
                        GROUP BY ITH.THWRKC, ITH.TPROD 
                        ORDER BY ITH.THWRKC";
            DataReaderBPCS = CommandBPCS.ExecuteReader();
            
            while(DataReaderBPCS.Read())
            {
                CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
                CommandIngDoc.CommandText = @"
                        SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, dbo.ObPrConver.OcObjEfic  AS [Propia] 
                        FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD 
                        where dbo.ObPrConver.OcCentro = '"+ DataReaderBPCS.GetValue(0).ToString() +"' AND dbo.ObPrConver.OcCprod = '"+ DataReaderBPCS.GetString(1) +"' ORDER BY OcFecha desc;";
                DataReaderIngDoc = CommandIngDoc.ExecuteReader();
                if(DataReaderIngDoc.Read()){
                    diccionario.Add(DataReaderIngDoc.GetValue(0).ToString(),DataReaderIngDoc.GetInt32(3));
                }
                CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
                //break;
            }
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            //dataTable.Load(objResult);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(diccionario);
            return JSONString;
        }

        [HttpGet]
        [Route("objePorHoraProductoActualPropia")]
        public dynamic objePorHoraProductoActualPropia(){
            Dictionary<string,decimal> diccionario = new Dictionary<string,decimal>();
            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                        SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY) AS Produccion 
                        FROM C20A237W.VENLX835F.ITH ITH 
                        WHERE (ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") +@") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000) 
                        GROUP BY ITH.THWRKC, ITH.TPROD 
                        ORDER BY ITH.THWRKC";
            DataReaderBPCS = CommandBPCS.ExecuteReader();
            
            while(DataReaderBPCS.Read())
            {
                CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
                CommandIngDoc.CommandText = @"
                        SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] 
                        FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD 
                        where dbo.ObPrConver.OcCentro = '"+ DataReaderBPCS.GetValue(0).ToString() +"' AND dbo.ObPrConver.OcCprod = '"+ DataReaderBPCS.GetString(1) +"' ORDER BY OcFecha desc;";
                DataReaderIngDoc = CommandIngDoc.ExecuteReader();
                if(DataReaderIngDoc.Read()){
                    diccionario.Add(DataReaderIngDoc.GetValue(0).ToString(),DataReaderIngDoc.GetDecimal(3));
                }
                CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
                //break;
            }
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            //dataTable.Load(objResult);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(diccionario);
            return JSONString;
        }

        [HttpGet]
        [Route("ProduccionCajaActualPorHoraEstanadar")]
        public dynamic ProduccionCajaActualPorHoraEstanadar(){
            Dictionary<string,decimal> diccionario = new Dictionary<string,decimal>();  
            var dataTable = new DataTable();

            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                    SELECT ITH.THWRKC, ITH.TPROD, IIM.IDESC, Sum(ITH.TQTY) As Cajas
                    FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
                    WHERE (ITH.TPROD = IIM.IPROD) AND ((ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") + @") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000))
                    GROUP BY ITH.THWRKC, ITH.TPROD, IIM.IDESC
                    ORDER BY ITH.THWRKC";
            DataReaderBPCS = CommandBPCS.ExecuteReader();


            while(DataReaderBPCS.Read())
            {
                CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
                CommandIngDoc.CommandText = @"
                        SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha,BD_SeguimientoPlanta.BPCS.IIM.IMFLPF
                        FROM [DOC_IngI].[dbo].[ObPrConver] 
                        INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD 
                        Where dbo.ObPrConver.OcCentro = '"+ DataReaderBPCS.GetValue(0).ToString() +"' AND dbo.ObPrConver.OcCprod = '"+ DataReaderBPCS.GetString(1) +"' ORDER BY OcFecha desc;";
                DataReaderIngDoc = CommandIngDoc.ExecuteReader();
                if(DataReaderIngDoc.Read()){
                    diccionario.Add(DataReaderIngDoc.GetValue(0).ToString(),DataReaderIngDoc.GetDecimal(3) * Decimal.Parse(DataReaderBPCS.GetValue(3).ToString()));
                }
                CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
                //break;
            }
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            // obj.Connection = conexionIngDoc.OpeAbrirConex();
            // obj.CommandText = "SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD ORDER BY OcFecha desc;";
            // objResult = obj.ExecuteReader();
    
            ProduccionCajaActualEstandar();
            //dataTable.Load(DataReaderBPCS);

            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                    SELECT PROCESO.CODIGOPROCESO, ((AVG(ENTRADAEJECUCION.HORASEJECUTADAS)) - (ISNULL(SUM(CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float))* 24,0))) AS [Tiempo Trabajado]
                    FROM SIPDATABASE.dbo.ENTRADAEJECUCION ENTRADAEJECUCION
                    INNER JOIN TRANSMICIONWEB ON TRANSMICIONWEB.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION 
                    INNER JOIN TUPLAEJECUCION ON TUPLAEJECUCION.CODIGOTUPLA = ENTRADAEJECUCION.CODIGOTUPLA
                    INNER JOIN PROCESO ON PROCESO.CODIGOPROCESO = TUPLAEJECUCION.CODIGOPROCESO
                    INNER JOIN PARADAS ON PARADAS.CODIGOPARADA = TRANSMICIONWEB.CODIGOPARADA
                    LEFT  JOIN PARADASEJECUTADAS ON PARADASEJECUTADAS.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION
                    GROUP BY PROCESO.CODIGOPROCESO
                    ORDER BY PROCESO.CODIGOPROCESO;
                ";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();

            dataTable.Load(DataReaderSIPDATABASE);

            string? codigoBPCS;
            decimal cajasProducidas;
            
            foreach(DataRow row in dataTable.Rows)
            
            {
                try
                {
                    decimal a = diccionario[row["CODIGOPROCESO"].ToString()];

                    decimal b = Decimal.Parse((row["Tiempo Trabajado"].ToString()));

                    diccionario[row["CODIGOPROCESO"].ToString()] = diccionario[row["CODIGOPROCESO"].ToString()] / Decimal.Parse((row["Tiempo Trabajado"].ToString()));
                }
                catch (KeyNotFoundException)
                {
                    diccionario.Add(row["CODIGOPROCESO"].ToString(),0);
                }
                // if(row["CODIGOPROCESO"].ToString().Equals(codigoBPCS)){
                //     diccionario.Add(row["CODIGOPROCESO"].ToString(), cajasProducidas/Decimal.Parse((row["Tiempo Trabajado"].ToString())));
                //     break;
                // }
            }   
            

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(diccionario);
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            return JSONString;
        }

        [HttpGet]
        [Route("ProduccionCajaActualPorHoraPropia")]
        public dynamic ProduccionCajaActualPorHoraPropia(){
            Dictionary<string,decimal> diccionario = new Dictionary<string,decimal>();  
            var dataTable = new DataTable();
            // obj.Connection = conexionIngDoc.OpeAbrirConex();
            // obj.CommandText = "SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD ORDER BY OcFecha desc;";
            // objResult = obj.ExecuteReader();
    
            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                    SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY) AS Produccion 
                    FROM C20A237W.VENLX835F.ITH ITH 
                    WHERE (ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") + @") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000) 
                    GROUP BY ITH.THWRKC, ITH.TPROD 
                    ORDER BY ITH.THWRKC";
            DataReaderBPCS = CommandBPCS.ExecuteReader();
            //dataTable.Load(DataReaderBPCS);

            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                    SELECT PROCESO.CODIGOPROCESO, ((AVG(ENTRADAEJECUCION.HORASEJECUTADAS)) - (ISNULL(SUM(CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float))* 24,0))) AS [Tiempo Trabajado]
                    FROM SIPDATABASE.dbo.ENTRADAEJECUCION ENTRADAEJECUCION
                    INNER JOIN TRANSMICIONWEB ON TRANSMICIONWEB.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION 
                    INNER JOIN TUPLAEJECUCION ON TUPLAEJECUCION.CODIGOTUPLA = ENTRADAEJECUCION.CODIGOTUPLA
                    INNER JOIN PROCESO ON PROCESO.CODIGOPROCESO = TUPLAEJECUCION.CODIGOPROCESO
                    INNER JOIN PARADAS ON PARADAS.CODIGOPARADA = TRANSMICIONWEB.CODIGOPARADA
                    LEFT  JOIN PARADASEJECUTADAS ON PARADASEJECUTADAS.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION
                    GROUP BY PROCESO.CODIGOPROCESO
                    ORDER BY PROCESO.CODIGOPROCESO;
                ";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();

            dataTable.Load(DataReaderSIPDATABASE);

            string? codigoBPCS;
            decimal cajasProducidas;
            while(DataReaderBPCS.Read()){
                codigoBPCS = DataReaderBPCS.GetValue(0).ToString();
                cajasProducidas = Decimal.Parse(DataReaderBPCS.GetValue(2).ToString());

                foreach(DataRow row in dataTable.Rows)
                {
                    if(row["CODIGOPROCESO"].ToString().Equals(codigoBPCS)){
                        diccionario.Add(row["CODIGOPROCESO"].ToString(), cajasProducidas/Decimal.Parse((row["Tiempo Trabajado"].ToString())));
                        break;
                    }
                }   
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(diccionario);
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            return JSONString;
        }
        [HttpGet]
        [Route("ProduccionCajaActualPropia")]
        public dynamic ProduccionCajaActualPropia(){
            var dataTable = new DataTable();
            // obj.Connection = conexionIngDoc.OpeAbrirConex();
            // obj.CommandText = "SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD ORDER BY OcFecha desc;";
            // objResult = obj.ExecuteReader();
            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                    SELECT ITH.THWRKC, ITH.TPROD, IIM.IDESC, Sum(ITH.TQTY) As Cajas
                    FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
                    WHERE (ITH.TPROD = IIM.IPROD) AND ((ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") + @") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000))
                    GROUP BY ITH.THWRKC, ITH.TPROD, IIM.IDESC
                    ORDER BY ITH.THWRKC";
            DataReaderBPCS = CommandBPCS.ExecuteReader();
            dataTable.Load(DataReaderBPCS);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dataTable);
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            return JSONString;
            }
        
        [HttpGet]
        [Route("ProduccionCajaActualEstandar")]
        public dynamic ProduccionCajaActualEstandar(){
            Dictionary<string,decimal> diccionario = new Dictionary<string,decimal>(); 
            //var dataTable = new DataTable();
            // obj.Connection = conexionIngDoc.OpeAbrirConex();
            // obj.CommandText = "SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD ORDER BY OcFecha desc;";
            // objResult = obj.ExecuteReader();
            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                    SELECT ITH.THWRKC, ITH.TPROD, IIM.IDESC, Sum(ITH.TQTY) As Cajas
                    FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
                    WHERE (ITH.TPROD = IIM.IPROD) AND ((ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") + @") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000))
                    GROUP BY ITH.THWRKC, ITH.TPROD, IIM.IDESC
                    ORDER BY ITH.THWRKC";
            DataReaderBPCS = CommandBPCS.ExecuteReader();


            while(DataReaderBPCS.Read())
            {
                CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
                CommandIngDoc.CommandText = @"
                        SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha,BD_SeguimientoPlanta.BPCS.IIM.IMFLPF
                        FROM [DOC_IngI].[dbo].[ObPrConver] 
                        INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD 
                        Where dbo.ObPrConver.OcCentro = '"+ DataReaderBPCS.GetValue(0).ToString() +"' AND dbo.ObPrConver.OcCprod = '"+ DataReaderBPCS.GetString(1) +"' ORDER BY OcFecha desc;";
                DataReaderIngDoc = CommandIngDoc.ExecuteReader();
                if(DataReaderIngDoc.Read()){
                    diccionario.Add(DataReaderIngDoc.GetValue(0).ToString(),DataReaderIngDoc.GetDecimal(3) * Decimal.Parse(DataReaderBPCS.GetValue(3).ToString()));
                }
                CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
                //break;
            }
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();


            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(diccionario);
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            return JSONString;
            }
                
            [HttpGet]
            [Route("PrimeraParadaPorLinea")]
            public dynamic obtennerPrimeraParadaPorLinea(){
                var dataTable = new DataTable();
                Dictionary<string,List<DateTime>> diccionario = new Dictionary<string,List<DateTime>>(); 

                comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
                comandSIPDATABASE.CommandText = @"
                        SELECT  CUADROPNFINAL.CODIGOPROCESO
                        FROM SIPDATABASE.dbo.PARADASEJECUTADAS INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL  ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
                        WHERE CUADROPNFINAL.FECHAENTRADA >= CONVERT(DATE,GETDATE())
                        Group BY CUADROPNFINAL.CODIGOPROCESO
                        ORDER BY CUADROPNFINAL.CODIGOPROCESO;
                    ";
                DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
                dataTable.Load(DataReaderSIPDATABASE);
                comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();

                foreach(DataRow row in dataTable.Rows)
                {
                    comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
                    comandSIPDATABASE.CommandText = @"
                        SELECT Top 1 CUADROPNFINAL.CODIGOPROCESO,PARADASEJECUTADAS.FECHAYHORAPARADA,PARADASEJECUTADAS.TIMESPAN
                        FROM SIPDATABASE.dbo.PARADASEJECUTADAS INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL  ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
                        WHERE CUADROPNFINAL.FECHAENTRADA >= CONVERT(DATE,GETDATE()) And CUADROPNFINAL.CODIGOPROCESO = '"+ row["CODIGOPROCESO"].ToString() + @"'
                        ORDER BY PARADASEJECUTADAS.FECHAYHORAPARADA;
                    ";  
                    DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();

                    if(DataReaderSIPDATABASE.Read()){
                        List<DateTime> tiempos = new List<DateTime>();
                        tiempos.Add(DataReaderSIPDATABASE.GetDateTime(1));
                        tiempos.Add(DataReaderSIPDATABASE.GetDateTime(2));
                        diccionario.Add(row["CODIGOPROCESO"].ToString(),tiempos);
                    }
                    comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
                }

                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(diccionario);
                return JSONString;
            }
        //private SqlClient obj = new SqlClient();
        //private OleDbCommand obj = new OleDbCommand();
    }
}