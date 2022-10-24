using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;
using ConsultasSQL.Logic;
namespace ConsultasSQL.Controllers{
    [ApiController]
    [Route("OEE")]
    [Route("Ing")]

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

        private BPCS bpcs = new BPCS();

        private Gespline gespline = new Gespline();

        SqlDataReader? DataReaderSIPDATABASE;
        
        
        // [HttpGet]
        // [Route("objePorHoraProductoActualEstandar1turno")]
        // public dynamic objePorHoraProductoActualEstandar1turno(){
        //     Dictionary<string,int> diccionario = new Dictionary<string,int>();
        //     CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
        //     CommandBPCS.CommandText = @"
        //                 SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY) AS Produccion 
        //                 FROM C20A237W.VENLX835F.ITH ITH 
        //                 WHERE (ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") +@") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000) 
        //                 GROUP BY ITH.THWRKC, ITH.TPROD 
        //                 ORDER BY ITH.THWRKC";
        //     DataReaderBPCS = CommandBPCS.ExecuteReader();
            
        //     while(DataReaderBPCS.Read())
        //     {
        //         CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
        //         CommandIngDoc.CommandText = @"
        //                 SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, dbo.ObPrConver.OcObjEfic  AS [Propia] 
        //                 FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD 
        //                 where dbo.ObPrConver.OcCentro = '"+ DataReaderBPCS.GetValue(0).ToString() +"' AND dbo.ObPrConver.OcCprod = '"+ DataReaderBPCS.GetString(1) +"' ORDER BY OcFecha desc;";
        //         DataReaderIngDoc = CommandIngDoc.ExecuteReader();
        //         if(DataReaderIngDoc.Read()){
        //             diccionario.Add(DataReaderIngDoc.GetValue(0).ToString(),DataReaderIngDoc.GetInt32(3));
        //         }
        //         CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
        //         //break;
        //     }
        //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
        //     //dataTable.Load(objResult);

        //     string JSONString = string.Empty;
        //     JSONString = JsonConvert.SerializeObject(diccionario);
        //     return JSONString;
        // }

        [HttpGet]
        [Route("objePorHoraProductoActualEstandar/{periodo}")]
        public dynamic objePorHoraProductoActualEstandar(int periodo){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(bpcs.ObjetivoPorHoraSegunProducto(periodo));
            return JSONString;
        }


        // [HttpGet]
        // [Route("objePorHoraProductoActualPropia")]
        // public dynamic objePorHoraProductoActualPropia(){
        //     Dictionary<string,decimal> diccionario = new Dictionary<string,decimal>();
        //     CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
        //     CommandBPCS.CommandText = @"
        //                 SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY) AS Produccion 
        //                 FROM C20A237W.VENLX835F.ITH ITH 
        //                 WHERE (ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") +@") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000) 
        //                 GROUP BY ITH.THWRKC, ITH.TPROD 
        //                 ORDER BY ITH.THWRKC";
        //     DataReaderBPCS = CommandBPCS.ExecuteReader();
            
        //     while(DataReaderBPCS.Read())
        //     {
        //         CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
        //         CommandIngDoc.CommandText = @"
        //                 SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] 
        //                 FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD 
        //                 where dbo.ObPrConver.OcCentro = '"+ DataReaderBPCS.GetValue(0).ToString() +"' AND dbo.ObPrConver.OcCprod = '"+ DataReaderBPCS.GetString(1) +"' ORDER BY OcFecha desc;";
        //         DataReaderIngDoc = CommandIngDoc.ExecuteReader();
        //         if(DataReaderIngDoc.Read()){
        //             diccionario.Add(DataReaderIngDoc.GetValue(0).ToString(),DataReaderIngDoc.GetDecimal(3));
        //         }
        //         CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
        //         //break;
        //     }
        //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
        //     //dataTable.Load(objResult);

        //     string JSONString = string.Empty;
        //     JSONString = JsonConvert.SerializeObject(diccionario);
        //     return JSONString;
        // }

        [HttpGet]
        [Route("ProduccionCajaActualPropia1turno")]
        public dynamic ProduccionCajaActualPorHoraPropia1turno(){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(bpcs.MaquinaProductosProduccionActual1turno());
            return JSONString;
        }

        [HttpGet]
        [Route("ProduccionCajaActualEstandar1turno")]
        public dynamic ProduccionCajaActualPorHoraEstandar1turno(){
            string JSONString = string.Empty;
            Dictionary<string, Dictionary<string, int>>  a = bpcs.MaquinaProductosProduccionActual1turno();
            JSONString = JsonConvert.SerializeObject(bpcs.conversionTotalAEstandarPormaquinaYproducto(a));
            return JSONString;
        }

        [HttpGet]
        [Route("ProduccionCajaActualPropia2turno/{band}")]
        public dynamic ProduccionCajaActualPorHoraPropia2turno(bool band){
            string JSONString = string.Empty;
            if(band){
                JSONString = JsonConvert.SerializeObject(bpcs.MaquinaProductosProduccionActual2turnoAntes0am());
            }else{
                JSONString = JsonConvert.SerializeObject(bpcs.MaquinaProductosProduccionActual2turnoDespues0am());
            }
            return JSONString;
        }

        [HttpGet]
        [Route("ProduccionCajaActualEstanadar2turno/{band}")]
        public dynamic ProduccionCajaActualPorHoraEstanadar2turno(bool band){
            string JSONString = string.Empty;
            Dictionary<string, Dictionary<string, int>>  a;
            if(band){
                a = bpcs.MaquinaProductosProduccionActual2turnoAntes0am();
            }else{
                a = bpcs.MaquinaProductosProduccionActual2turnoDespues0am();
            }
            JSONString = JsonConvert.SerializeObject(bpcs.conversionTotalAEstandarPormaquinaYproducto(a));
            return JSONString;
        }

        [HttpGet]
        [Route("ProduccionCajaActualPorHoraPropia/{tiempo}")]
        public dynamic ProduccionCajaActualPorHoraPropia(int tiempo){
            string JSONString = string.Empty;
            Dictionary<string,Dictionary<string,int>> produccion = new Dictionary<string,Dictionary<string,int>>();
            if(tiempo == 1){ //* si es igual a 1 es primer turno
                produccion = bpcs.MaquinaProductosProduccionActual1turno();
            }else if(tiempo == 2){//* si es igual es 2 es primer turno antes de 0 am
                produccion = bpcs.MaquinaProductosProduccionActual2turnoAntes0am();
            }else if(tiempo == 3){ //* si es igual es 2 es primer turno despues de 0 am
                produccion = bpcs.MaquinaProductosProduccionActual2turnoDespues0am();
            }else{
                return JSONString;
            }
            JSONString = JsonConvert.SerializeObject(bpcs.ProduccionActualPorMaquinaPorHora(produccion,tiempo));
            return JSONString;
        }

        [HttpGet]
        [Route("ProduccionCajaActualPorHoraEstandar/{tiempo}")]
        public dynamic ProduccionCajaActualPorHoraEstanadar(int tiempo){
            string JSONString = string.Empty;
            Dictionary<string,Dictionary<string,int>> produccion = new Dictionary<string,Dictionary<string,int>>();
            if(tiempo == 1){ //* si es igual a 1 es primer turno
                produccion = bpcs.MaquinaProductosProduccionActual1turno();
            }else if(tiempo == 2){//* si es igual es 2 es primer turno antes de 0 am
                produccion = bpcs.MaquinaProductosProduccionActual2turnoAntes0am();
            }else if(tiempo == 3){ //* si es igual es 2 es primer turno despues de 0 am
                produccion = bpcs.MaquinaProductosProduccionActual2turnoDespues0am();
            }else{
                return JSONString;
            }
            produccion = bpcs.conversionTotalAEstandarPormaquinaYproducto(produccion);
            JSONString = JsonConvert.SerializeObject(bpcs.ProduccionActualPorMaquinaPorHora(produccion,tiempo));
            return JSONString;
        }

        // [HttpGet]
        // [Route("ProduccionCajaActualPorHoraEstanadar")]
        // public dynamic ProduccionCajaActualPorHoraEstanadar(){
        //     Dictionary<string,decimal> diccionario = new Dictionary<string,decimal>();  
        //     var dataTable = new DataTable();

        //     CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
        //     CommandBPCS.CommandText = @"
        //             SELECT ITH.THWRKC, ITH.TPROD, IIM.IDESC, Sum(ITH.TQTY) As Cajas
        //             FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
        //             WHERE (ITH.TPROD = IIM.IPROD) AND ((ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") + @") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000))
        //             GROUP BY ITH.THWRKC, ITH.TPROD, IIM.IDESC
        //             ORDER BY ITH.THWRKC";
        //     DataReaderBPCS = CommandBPCS.ExecuteReader();


        //     while(DataReaderBPCS.Read())
        //     {
        //         CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
        //         CommandIngDoc.CommandText = @"
        //                 SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha,BD_SeguimientoPlanta.BPCS.IIM.IMFLPF
        //                 FROM [DOC_IngI].[dbo].[ObPrConver] 
        //                 INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD 
        //                 Where dbo.ObPrConver.OcCentro = '"+ DataReaderBPCS.GetValue(0).ToString() +"' AND dbo.ObPrConver.OcCprod = '"+ DataReaderBPCS.GetString(1) +"' ORDER BY OcFecha desc;";
        //         DataReaderIngDoc = CommandIngDoc.ExecuteReader();
        //         if(DataReaderIngDoc.Read()){
        //             diccionario.Add(DataReaderIngDoc.GetValue(0).ToString(),DataReaderIngDoc.GetDecimal(3) * Decimal.Parse(DataReaderBPCS.GetValue(3).ToString()));
        //         }
        //         CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
        //         //break;
        //     }
        //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
        //     // obj.Connection = conexionIngDoc.OpeAbrirConex();
        //     // obj.CommandText = "SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD ORDER BY OcFecha desc;";
        //     // objResult = obj.ExecuteReader();
    
        //     ProduccionCajaActualEstandar();
        //     //dataTable.Load(DataReaderBPCS);

        //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
        //     comandSIPDATABASE.CommandText = @"
        //             SELECT PROCESO.CODIGOPROCESO, ((AVG(ENTRADAEJECUCION.HORASEJECUTADAS)) - (ISNULL(SUM(CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float))* 24,0))) AS [Tiempo Trabajado]
        //             FROM SIPDATABASE.dbo.ENTRADAEJECUCION ENTRADAEJECUCION
        //             INNER JOIN TRANSMICIONWEB ON TRANSMICIONWEB.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION 
        //             INNER JOIN TUPLAEJECUCION ON TUPLAEJECUCION.CODIGOTUPLA = ENTRADAEJECUCION.CODIGOTUPLA
        //             INNER JOIN PROCESO ON PROCESO.CODIGOPROCESO = TUPLAEJECUCION.CODIGOPROCESO
        //             INNER JOIN PARADAS ON PARADAS.CODIGOPARADA = TRANSMICIONWEB.CODIGOPARADA
        //             LEFT  JOIN PARADASEJECUTADAS ON PARADASEJECUTADAS.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION
        //             GROUP BY PROCESO.CODIGOPROCESO
        //             ORDER BY PROCESO.CODIGOPROCESO;
        //         ";
        //     DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();

        //     dataTable.Load(DataReaderSIPDATABASE);

        //     string? codigoBPCS;
        //     decimal cajasProducidas;
            
        //     foreach(DataRow row in dataTable.Rows)
            
        //     {
        //         try
        //         {
        //             decimal a = diccionario[row["CODIGOPROCESO"].ToString()];

        //             decimal b = Decimal.Parse((row["Tiempo Trabajado"].ToString()));

        //             diccionario[row["CODIGOPROCESO"].ToString()] = diccionario[row["CODIGOPROCESO"].ToString()] / Decimal.Parse((row["Tiempo Trabajado"].ToString()));
        //         }
        //         catch (KeyNotFoundException)
        //         {
        //             diccionario.Add(row["CODIGOPROCESO"].ToString(),0);
        //         }
        //         // if(row["CODIGOPROCESO"].ToString().Equals(codigoBPCS)){
        //         //     diccionario.Add(row["CODIGOPROCESO"].ToString(), cajasProducidas/Decimal.Parse((row["Tiempo Trabajado"].ToString())));
        //         //     break;
        //         // }
        //     }   
            

        //     string JSONString = string.Empty;
        //     JSONString = JsonConvert.SerializeObject(diccionario);
        //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
        //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
        //     return JSONString;
        // }

        // [HttpGet]
        // [Route("ProduccionCajaActualPorHoraPropiaBeta")]
        // public dynamic ProduccionCajaActualPorHoraPropia1turnoBeta(){
        //     string JSONString = string.Empty;
        //     JSONString = JsonConvert.SerializeObject(diccionario);
        //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
        //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
        //     return JSONString;
        // }
        // [HttpGet]
        // [Route("ProduccionCajaActualPorHoraPropia")]
        // public dynamic ProduccionCajaActualPorHoraPropia(){
        //     Dictionary<string,decimal> diccionario = new Dictionary<string,decimal>();  
        //     var dataTable = new DataTable();
        //     // obj.Connection = conexionIngDoc.OpeAbrirConex();
        //     // obj.CommandText = "SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD ORDER BY OcFecha desc;";
        //     // objResult = obj.ExecuteReader();
    
        //     CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
        //     CommandBPCS.CommandText = @"
        //             SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY) AS Produccion 
        //             FROM C20A237W.VENLX835F.ITH ITH 
        //             WHERE (ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") + @") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000) 
        //             GROUP BY ITH.THWRKC, ITH.TPROD 
        //             ORDER BY ITH.THWRKC";
        //     DataReaderBPCS = CommandBPCS.ExecuteReader();
        //     //dataTable.Load(DataReaderBPCS);

        //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
        //     comandSIPDATABASE.CommandText = @"
        //             SELECT PROCESO.CODIGOPROCESO, ((AVG(ENTRADAEJECUCION.HORASEJECUTADAS)) - (ISNULL(SUM(CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float))* 24,0))) AS [Tiempo Trabajado]
        //             FROM SIPDATABASE.dbo.ENTRADAEJECUCION ENTRADAEJECUCION
        //             INNER JOIN TRANSMICIONWEB ON TRANSMICIONWEB.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION 
        //             INNER JOIN TUPLAEJECUCION ON TUPLAEJECUCION.CODIGOTUPLA = ENTRADAEJECUCION.CODIGOTUPLA
        //             INNER JOIN PROCESO ON PROCESO.CODIGOPROCESO = TUPLAEJECUCION.CODIGOPROCESO
        //             INNER JOIN PARADAS ON PARADAS.CODIGOPARADA = TRANSMICIONWEB.CODIGOPARADA
        //             LEFT  JOIN PARADASEJECUTADAS ON PARADASEJECUTADAS.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION
        //             GROUP BY PROCESO.CODIGOPROCESO
        //             ORDER BY PROCESO.CODIGOPROCESO;
        //         ";
        //     DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();

        //     dataTable.Load(DataReaderSIPDATABASE);

        //     string? codigoBPCS;
        //     decimal cajasProducidas;
        //     while(DataReaderBPCS.Read()){
        //         codigoBPCS = DataReaderBPCS.GetValue(0).ToString();
        //         cajasProducidas = Decimal.Parse(DataReaderBPCS.GetValue(2).ToString());

        //         foreach(DataRow row in dataTable.Rows)
        //         {
        //             if(row["CODIGOPROCESO"].ToString().Equals(codigoBPCS)){
        //                 diccionario.Add(row["CODIGOPROCESO"].ToString(), cajasProducidas/Decimal.Parse((row["Tiempo Trabajado"].ToString())));
        //                 break;
        //             }
        //         }   
        //     }

        //     string JSONString = string.Empty;
        //     JSONString = JsonConvert.SerializeObject(diccionario);
        //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
        //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
        //     return JSONString;
        // }

        // [HttpGet]
        // [Route("ProduccionCajaActualPropia")]
        // public dynamic ProduccionCajaActualPropia(){
        //     var dataTable = new DataTable();
        //     // obj.Connection = conexionIngDoc.OpeAbrirConex();
        //     // obj.CommandText = "SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD ORDER BY OcFecha desc;";
        //     // objResult = obj.ExecuteReader();
        //     CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
        //     CommandBPCS.CommandText = @"
        //             SELECT ITH.THWRKC, ITH.TPROD, IIM.IDESC, Sum(ITH.TQTY) As Cajas
        //             FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
        //             WHERE (ITH.TPROD = IIM.IPROD) AND ((ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") + @") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000))
        //             GROUP BY ITH.THWRKC, ITH.TPROD, IIM.IDESC
        //             ORDER BY ITH.THWRKC";
        //     DataReaderBPCS = CommandBPCS.ExecuteReader();
        //     dataTable.Load(DataReaderBPCS);

        //     string JSONString = string.Empty;
        //     JSONString = JsonConvert.SerializeObject(dataTable);
        //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
        //     return JSONString;
        //     }
        
        // [HttpGet]
        // [Route("ProduccionCajaActualEstandar")]
        // public dynamic ProduccionCajaActualEstandar(){
        //     Dictionary<string,decimal> diccionario = new Dictionary<string,decimal>(); 
        //     //var dataTable = new DataTable();
        //     // obj.Connection = conexionIngDoc.OpeAbrirConex();
        //     // obj.CommandText = "SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha, (dbo.ObPrConver.OcObjEfic/BD_SeguimientoPlanta.BPCS.IIM.IMFLPF) AS [Propia] FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD ORDER BY OcFecha desc;";
        //     // objResult = obj.ExecuteReader();
        //     CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
        //     CommandBPCS.CommandText = @"
        //             SELECT ITH.THWRKC, ITH.TPROD, IIM.IDESC, Sum(ITH.TQTY) As Cajas
        //             FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
        //             WHERE (ITH.TPROD = IIM.IPROD) AND ((ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") + @") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000))
        //             GROUP BY ITH.THWRKC, ITH.TPROD, IIM.IDESC
        //             ORDER BY ITH.THWRKC";
        //     DataReaderBPCS = CommandBPCS.ExecuteReader();


        //     while(DataReaderBPCS.Read())
        //     {
        //         CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
        //         CommandIngDoc.CommandText = @"
        //                 SELECT dbo.ObPrConver.OcCentro,dbo.ObPrConver.OcCprod,dbo.ObPrConver.OcFecha,BD_SeguimientoPlanta.BPCS.IIM.IMFLPF
        //                 FROM [DOC_IngI].[dbo].[ObPrConver] 
        //                 INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD 
        //                 Where dbo.ObPrConver.OcCentro = '"+ DataReaderBPCS.GetValue(0).ToString() +"' AND dbo.ObPrConver.OcCprod = '"+ DataReaderBPCS.GetString(1) +"' ORDER BY OcFecha desc;";
        //         DataReaderIngDoc = CommandIngDoc.ExecuteReader();
        //         if(DataReaderIngDoc.Read()){
        //             diccionario.Add(DataReaderIngDoc.GetValue(0).ToString(),DataReaderIngDoc.GetDecimal(3) * Decimal.Parse(DataReaderBPCS.GetValue(3).ToString()));
        //         }
        //         CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
        //         //break;
        //     }
        //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();


        //     string JSONString = string.Empty;
        //     JSONString = JsonConvert.SerializeObject(diccionario);
        //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
        //     return JSONString;
        //     }
                
            [HttpGet]
            [Route("PrimeraParadaPorLinea")]
            public dynamic obtenerPrimeraParadaPorLinea(){
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

            [HttpGet]
            [Route("obtenerCajasPorHoraPropia/{tiempo}")]
            public dynamic obtenerCajasPorHoraPropia(int tiempo){
                string JSONString = string.Empty;
                Dictionary<string, Dictionary<string, List<int>>> produccion = new Dictionary<string, Dictionary<string, List<int>>>();
                
                if(tiempo == 1){
                    produccion = bpcs.obtenerLaProduccionActual1turno();
                }else if(tiempo == 2){
                    produccion = bpcs.obtenerLaProduccionActual2turno(true);
                }else if(tiempo == 3){
                    produccion = bpcs.obtenerLaProduccionActual2turno(false);
                }else{
                    return JSONString;
                }
                JSONString = JsonConvert.SerializeObject(produccion);
                return JSONString;
            }

            [HttpGet]
            [Route("obtenerCajasPorHoraEstandar/{tiempo}")]
            public dynamic obtenerCajasPorHoraEstandar(int tiempo){
                string JSONString = string.Empty;
                Dictionary<string, Dictionary<string, List<int>>> produccion = new Dictionary<string, Dictionary<string, List<int>>>();
                
                if(tiempo == 1){
                    produccion = bpcs.obtenerLaProduccionActual1turno();
                }else if(tiempo == 2){
                    produccion = bpcs.obtenerLaProduccionActual2turno(true);
                }else if(tiempo == 3){
                    produccion = bpcs.obtenerLaProduccionActual2turno(false);
                }else{
                    return JSONString;
                }
                JSONString = JsonConvert.SerializeObject(this.bpcs.conversionTotalAEstandarPormaquinaYproducto(produccion));
                return JSONString;
            }

        [HttpGet]
        [Route("obtenerProductosActuales")]
        public dynamic obtenerProductosActuales(){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(bpcs.obtenerLosProductosActuales());
            return JSONString;
        }

        [HttpGet]
        [Route("obtenerProductosActualesDeLaLiena/{centroCosto}")]
        public dynamic obtenerProductosActualesDeLaLiena(string centroCosto){
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(bpcs.obtenerLosProductosActualesDeLinea(centroCosto));
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
            JSONString = JsonConvert.SerializeObject(gespline.obtenerParadasActuales1turnoConFiltro(centroCosto,cadenas));
            return JSONString;
        }

            // [HttpGet]
            // [Route("obtenerCajasPorHorayLinea1turno")]
            // public dynamic obtenerCajasPorHorayLinea1turno(){
            //     var dataTable = new DataTable();
            //     Dictionary<string, Dictionary<string,List<int>>> nombreProducto = new Dictionary<string,Dictionary<string,List<int>>>(); 
            //     Dictionary<string,List<int>> listaProSuma;

            //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            //     comandSIPDATABASE.CommandText = @"
            //             SELECT CUADROPNFINAL.CODIGOPROCESO
            //             FROM SIPDATABASE.dbo.PARADASEJECUTADAS INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL  ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
            //             WHERE PARADASEJECUTADAS.FECHAYHORAPARADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '05:50:00' AND PARADASEJECUTADAS.FECHAYHORAPARADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '18:00:00'
            //             GROUP BY CUADROPNFINAL.CODIGOPROCESO
            //             ORDER BY CUADROPNFINAL.CODIGOPROCESO;
            //     ";  
            //     DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            //     dataTable.Load(DataReaderSIPDATABASE);
                
            //     foreach (DataRow row in dataTable.Rows)
            //     {
            //         listaProSuma = new Dictionary<string,List<int>>();
            //         nombreProducto.Add(row["CODIGOPROCESO"].ToString(),listaProSuma);
            //     }

            //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
                
            //     CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            //     CommandBPCS.CommandText = @"
            //             SELECT ITH.THWRKC, ITH.TPROD, IIM.IDESC, ITH.TQTY, ITH.THTIME
            //             FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
            //             WHERE ITH.TPROD = IIM.IPROD AND ((ITH.TTYPE='R') AND (ITH.TTDTE>=" + DateTime.Now.ToString("yyyyMMdd") + @") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<=180000))
            //             ORDER BY ITH.THWRKC";
            //     DataReaderBPCS = CommandBPCS.ExecuteReader();
            //     dataTable = new DataTable();
            //     dataTable.Load(DataReaderBPCS);
                
            //     var a = DateTime.Now.ToString("yyyyMMdd");

            //     List<int> itemCaja;
            //     Dictionary<string,List<int>> item;
            //     int hora;
            //     List<int> cajas;
            //     foreach (DataRow row in dataTable.Rows)
            //     {
            //         try
            //         {
            //             item = nombreProducto[row["THWRKC"].ToString()];
            //             if (row["THWRKC"].ToString() == "441208")
            //             {
            //                 int p = 1;
            //             }
            //         }
            //         catch (KeyNotFoundException)
            //         {
            //             continue;
            //         }

                    
                    
            //         if(item.ContainsKey(row["TPROD"].ToString()))
            //         {
            //             cajas = item[row["TPROD"].ToString()];
            //         }
            //         else
            //         {
            //             itemCaja = new List<int>() {0,0,0,0,0,0,0,0,0,0,0,0};
            //             item.Add(row["TPROD"].ToString(),itemCaja);
            //             cajas = item[row["TPROD"].ToString()];
            //         }

            //         hora = int.Parse(row["THTIME"].ToString());
            //             if (hora >= 60000 && hora < 70000)
            //             {
            //                 cajas[0] = cajas[0] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 80000){
            //                 cajas[1] = cajas[1] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 90000){
            //                 cajas[2] = cajas[2] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 100000){
            //                 cajas[3] = cajas[3] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 110000){
            //                 cajas[4] = cajas[4] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 120000){
            //                 cajas[5] = cajas[5] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 130000){
            //                 cajas[6] = cajas[6] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 140000){
            //                 cajas[7] = cajas[7] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 150000){
            //                 cajas[8] = cajas[8] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 160000){
            //                 cajas[9] = cajas[9] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 170000){
            //                 cajas[10] = cajas[10] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora <= 180000){
            //                 cajas[11] = cajas[11] + int.Parse(row["TQTY"].ToString());
            //             }

            //         nombreProducto[row["THWRKC"].ToString()] = item;
            //     }

            //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            //     string JSONString = string.Empty;
            //     JSONString = JsonConvert.SerializeObject(nombreProducto);
            //     return JSONString;
            // }

            // [HttpGet]
            // [Route("obtenerCajasPorHorayLinea2turno")]
            // public dynamic obtenerCajasPorHorayLinea2turno(){
            //     var dataTable = new DataTable();
            //     Dictionary<string, Dictionary<string,List<int>>> nombreProducto = new Dictionary<string,Dictionary<string,List<int>>>(); 
            //     Dictionary<string,List<int>> listaProSuma;

            //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            //     comandSIPDATABASE.CommandText = @"
            //             SELECT CUADROPNFINAL.CODIGOPROCESO
            //             FROM SIPDATABASE.dbo.PARADASEJECUTADAS INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL  ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
            //             WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),-1) + '17:55:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '6:00:00'
            //             GROUP BY CUADROPNFINAL.CODIGOPROCESO
            //             ORDER BY CUADROPNFINAL.CODIGOPROCESO;
            //     ";  
            //     DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            //     dataTable.Load(DataReaderSIPDATABASE);
                
            //     foreach (DataRow row in dataTable.Rows)
            //     {
            //         listaProSuma = new Dictionary<string,List<int>>();
            //         nombreProducto.Add(row["CODIGOPROCESO"].ToString(),listaProSuma);
            //     }

            //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
                
            //     CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            //     CommandBPCS.CommandText = @"
            //             SELECT ITH.THWRKC, ITH.TPROD, IIM.IDESC, ITH.TQTY, ITH.THTIME
            //             FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
            //             WHERE ITH.TPROD = IIM.IPROD AND ((ITH.TTYPE='R') AND (ITH.TTDTE>='"+ DateTime.Now.AddDays(-1).ToString("yyyyMMdd") +@"') AND (ITH.TWHS='PT ') AND (ITH.THTIME>=180000 And ITH.THTIME < 240000))
            //             ORDER BY ITH.THWRKC";
            //     DataReaderBPCS = CommandBPCS.ExecuteReader();
            //     dataTable = new DataTable();
            //     dataTable.Load(DataReaderBPCS);


            //     List<int> itemCaja;
            //     Dictionary<string,List<int>> item;
            //     int hora;
            //     List<int> cajas;
            //     foreach (DataRow row in dataTable.Rows)
            //     {
            //         try
            //         {
            //             item = nombreProducto[row["THWRKC"].ToString()];
            //         }
            //         catch (KeyNotFoundException)
            //         {
            //             continue;
            //         }   

                    
                    
            //         if(item.ContainsKey(row["TPROD"].ToString()))
            //         {
            //             cajas = item[row["TPROD"].ToString()];
            //         }
            //         else
            //         {
            //             itemCaja = new List<int>() {0,0,0,0,0,0,0,0,0,0,0,0};
            //             item.Add(row["TPROD"].ToString(),itemCaja);
            //             cajas = item[row["TPROD"].ToString()];
            //         }

            //         hora = int.Parse(row["THTIME"].ToString());
            //             if (hora >= 180000 && hora < 190000)
            //             {
            //                 cajas[0] = cajas[0] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 200000){
            //                 cajas[1] = cajas[1] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 210000){
            //                 cajas[2] = cajas[2] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 220000){
            //                 cajas[3] = cajas[3] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 230000){
            //                 cajas[4] = cajas[4] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 240000){
            //                 cajas[5] = cajas[5] + int.Parse(row["TQTY"].ToString());
            //             }

            //         nombreProducto[row["THWRKC"].ToString()] = item;
            //     }
            //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();

            //     CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            //     CommandBPCS.CommandText = @"
            //             SELECT ITH.THWRKC, ITH.TPROD, IIM.IDESC, ITH.TQTY, ITH.THTIME
            //             FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
            //             WHERE ITH.TPROD = IIM.IPROD AND ((ITH.TTYPE='R') AND (ITH.TTDTE>='"+ DateTime.Now.ToString("yyyyMMdd") +@"') AND (ITH.TWHS='PT ') AND (ITH.THTIME >= 0 And ITH.THTIME < 60000))
            //             ORDER BY ITH.THWRKC";
            //     DataReaderBPCS = CommandBPCS.ExecuteReader();
            //     dataTable = new DataTable();
            //     dataTable.Load(DataReaderBPCS);

                
            //     foreach (DataRow row in dataTable.Rows)
            //     {
            //         try
            //         {
            //             item = nombreProducto[row["THWRKC"].ToString()];
            //         }
            //         catch (KeyNotFoundException)
            //         {
            //             break;
            //         }   

                    
                    
            //         if(item.ContainsKey(row["TPROD"].ToString()))
            //         {
            //             cajas = item[row["TPROD"].ToString()];
            //         }
            //         else
            //         {
            //             itemCaja = new List<int>() {0,0,0,0,0,0,0,0,0,0,0,0};
            //             item.Add(row["TPROD"].ToString(),itemCaja);
            //             cajas = item[row["TPROD"].ToString()];
            //         }

            //         hora = int.Parse(row["THTIME"].ToString());
            //             if(hora > 0 && hora < 10000){
            //                 cajas[6] = cajas[6] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 20000){
            //                 cajas[7] = cajas[7] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 30000){
            //                 cajas[8] = cajas[8] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 40000){
            //                 cajas[9] = cajas[9] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora < 50000){
            //                 cajas[10] = cajas[10] + int.Parse(row["TQTY"].ToString());
            //             }else if(hora <= 60000){
            //                 cajas[11] = cajas[11] + int.Parse(row["TQTY"].ToString());
            //             }

            //         nombreProducto[row["THWRKC"].ToString()] = item;
            //     }

            //     CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            //     string JSONString = string.Empty;
            //     comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            //     JSONString = JsonConvert.SerializeObject(nombreProducto);
            //     return JSONString;
            // }
        //private SqlClient obj = new SqlClient();
        //private OleDbCommand obj = new OleDbCommand();
    }
}