using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;

namespace ConsultasSQL.Logic{
    public class BPCS 
    {
        private DbIngDoc conexionIngDoc = new DbIngDoc();
        private SqlCommand CommandIngDoc = new SqlCommand();
        private SqlDataReader? DataReaderIngDoc;

        private DBconexionBPCS conexionBPCS = new DBconexionBPCS();
        private OleDbCommand CommandBPCS = new OleDbCommand();
        OleDbDataReader? DataReaderBPCS;
        private DbSIPDATABASE conexionSIPDATABASE = new DbSIPDATABASE();
        private SqlCommand comandSIPDATABASE = new SqlCommand();
        private Gespline gespline = new Gespline();
        SqlDataReader? DataReaderSIPDATABASE;

        public Dictionary<string,Dictionary<string,int>> ObjetivoPorHoraSegunProducto(int tiempo){
            Dictionary<string,Dictionary<string,int>> produccion;
            if(tiempo == 1){
                produccion = MaquinaProductosProduccionActual1turno();
            }else if(tiempo == 2){
                produccion = MaquinaProductosProduccionActual2turnoAntes0am();
            }else if(tiempo == 3){
                produccion = MaquinaProductosProduccionActual2turnoDespues0am();
            }else{
                return null;
            }
            
            string maquina;
            string producto;
            Dictionary<string,int> produccionMaquina;

            foreach (var item in produccion)
            {
                maquina = item.Key;
                produccionMaquina = item.Value;
                foreach (var productosProduccion in produccionMaquina)
                {
                    producto = productosProduccion.Key;

                    CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
                    CommandIngDoc.CommandText = @"
                            SELECT dbo.ObPrConver.OcObjEfic  AS [ObjEstandar] 
                            FROM [DOC_IngI].[dbo].[ObPrConver] INNER JOIN [BD_SeguimientoPlanta].[BPCS].[IIM] ON [DOC_IngI].[dbo].[ObPrConver].OcCprod = [BD_SeguimientoPlanta].[BPCS].[IIM].IPROD 
                            where dbo.ObPrConver.OcCentro = '"+ maquina +"' AND dbo.ObPrConver.OcCprod = '"+ producto +"' ORDER BY OcFecha desc";
                    DataReaderIngDoc = CommandIngDoc.ExecuteReader();

                    if(DataReaderIngDoc.Read()){
                        produccion[maquina][producto] = int.Parse(DataReaderIngDoc.GetValue(0).ToString());
                    }else{
                        produccion[maquina][producto] = -1; 
                    }
                    CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
                }
            }

            return produccion;
        }

        public Dictionary<string,Dictionary<string,int>> MaquinaProductosProduccionActual1turno(){
            var dataTable = new DataTable();
            Dictionary<string,Dictionary<string,int>> producción = new Dictionary<string,Dictionary<string,int>>();
            List<string> maquinas = gespline.MaquinasGesplineActivos1turno();
            Dictionary<string,int> temporal;

            foreach(string maquina in maquinas){
                temporal = new Dictionary<string,int>();
                producción.Add(maquina,temporal);
            }

            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                        SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY) AS PRODUCCION 
                        FROM C20A237W.VENLX835F.ITH ITH 
                        WHERE (ITH.TTYPE='R') AND (ITH.TTDTE>="+ DateTime.Now.ToString("yyyyMMdd") +@") AND (ITH.TWHS='PT ') AND (ITH.THTIME>=60000 And ITH.THTIME<180000) 
                        GROUP BY ITH.THWRKC, ITH.TPROD 
                        ORDER BY ITH.THWRKC";
            DataReaderBPCS = CommandBPCS.ExecuteReader();
            dataTable.Load(DataReaderBPCS);
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();

            foreach (DataRow row in dataTable.Rows)
            {
                if (producción.ContainsKey(row["THWRKC"].ToString()))
                {
                    temporal = producción[row["THWRKC"].ToString()];
                    var a = row["TPROD"].ToString();
                    temporal.Add(row["TPROD"].ToString(), (int) float.Parse(row["PRODUCCION"].ToString()));
                    producción[row["THWRKC"].ToString()] = temporal;
                }else{
                    continue;
                }
            }
            return producción;
        }

        public Dictionary<string,Dictionary<string,int>> MaquinaProductosProduccionActual2turnoDespues0am(){
            var dataTable = new DataTable();
            Dictionary<string,Dictionary<string,int>> producción = new Dictionary<string,Dictionary<string,int>>();
            List<string> maquinas = gespline.MaquinasGesplineActivos2turnoDespues0am();
            Dictionary<string,int> temporal;

            foreach(string maquina in maquinas){
                temporal = new Dictionary<string,int>();
                producción.Add(maquina,temporal);
            }

            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                        SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY) AS PRODUCCION
                        FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
                        WHERE ITH.TPROD = IIM.IPROD AND ((ITH.TTYPE='R') AND (ITH.TTDTE>='"+ DateTime.Now.ToString("yyyyMMdd") +@"') AND (ITH.TWHS='PT ') AND (ITH.THTIME>=0 And ITH.THTIME<60000))
                        GROUP BY ITH.THWRKC, ITH.TPROD
                        ORDER BY ITH.THWRKC
                        ";
            DataReaderBPCS = CommandBPCS.ExecuteReader();
            dataTable.Load(DataReaderBPCS);

            foreach (DataRow row in dataTable.Rows)
            {
                if (producción.ContainsKey(row["THWRKC"].ToString()))
                {
                    temporal = producción[row["THWRKC"].ToString()];
                    temporal.Add(row["TPROD"].ToString(),(int) float.Parse(row["PRODUCCION"].ToString()));
                    producción[row["THWRKC"].ToString()] = temporal;
                }else{
                    continue;
                }
            }

            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            dataTable = new DataTable();

            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                        SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY) AS PRODUCCION
                        FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
                        WHERE ITH.TPROD = IIM.IPROD AND ((ITH.TTYPE='R') AND (ITH.TTDTE>='"+ DateTime.Now.AddDays(-1).ToString("yyyyMMdd") +@"') AND (ITH.TWHS='PT ') AND (ITH.THTIME>=180000 And ITH.THTIME<=235959))
                        GROUP BY ITH.THWRKC, ITH.TPROD
                        ";
            DataReaderBPCS = CommandBPCS.ExecuteReader();
            dataTable.Load(DataReaderBPCS);
            
            int temporalNumero;

            foreach (DataRow row in dataTable.Rows)
            {
                if (producción.ContainsKey(row["THWRKC"].ToString()))
                {
                    temporal = producción[row["THWRKC"].ToString()];

                    if(temporal.ContainsKey(row["TPROD"].ToString())){

                        temporalNumero = temporal[row["TPROD"].ToString()];

                        temporalNumero += (int) float.Parse(row["PRODUCCION"].ToString());

                        temporal[row["TPROD"].ToString()] = temporalNumero;

                    }else{
                        temporal.Add(row["TPROD"].ToString(),(int) float.Parse(row["PRODUCCION"].ToString()));
                    }
                    producción[row["THWRKC"].ToString()] = temporal;
                }else{
                    continue;
                }
            }
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            return producción;
        }

         public Dictionary<string,Dictionary<string,int>> MaquinaProductosProduccionActual2turnoAntes0am(){
            var dataTable = new DataTable();
            Dictionary<string,Dictionary<string,int>> producción = new Dictionary<string,Dictionary<string,int>>();
            List<string> maquinas = gespline.MaquinasGesplineActivos2turnoAntes0am();
            Dictionary<string,int> temporal;

            foreach(string maquina in maquinas){
                temporal = new Dictionary<string,int>();
                producción.Add(maquina,temporal);
            }


            CommandBPCS.Connection = conexionBPCS.CodAbrirConex();
            CommandBPCS.CommandText = @"
                        SELECT ITH.THWRKC, ITH.TPROD, Sum(ITH.TQTY)
                        FROM C20A237W.VENLX835F.IIM IIM, C20A237W.VENLX835F.ITH ITH
                        WHERE ITH.TPROD = IIM.IPROD AND ((ITH.TTYPE='R') AND (ITH.TTDTE>='"+ DateTime.Now.ToString("yyyyMMdd") +@"') AND (ITH.TWHS='PT ') AND (ITH.THTIME>=180000 And ITH.THTIME<=235959))
                        GROUP BY ITH.THWRKC, ITH.TPROD
                        ";
            DataReaderBPCS = CommandBPCS.ExecuteReader();
            dataTable.Load(DataReaderBPCS);
            
            int temporalNumero;

            foreach (DataRow row in dataTable.Rows)
            {
                if (producción.ContainsKey(row["THWRKC"].ToString()))
                {
                    temporal = producción[row["THWRKC"].ToString()];

                    if(temporal.ContainsKey(row["TPROD"].ToString())){

                        temporalNumero = temporal[row["TPROD"].ToString()];

                        temporalNumero += (int) float.Parse(row["PRODUCCION"].ToString());

                        temporal[row["TPROD"].ToString()] = temporalNumero;

                    }else{
                        temporal.Add(row["TPROD"].ToString(),(int) float.Parse(row["PRODUCCION"].ToString()));
                    }
                    producción[row["THWRKC"].ToString()] = temporal;
                }else{
                    continue;
                }
            }
            CommandBPCS.Connection = conexionBPCS.CodCerrarConex();
            return producción;
        }

        public Dictionary<string,Dictionary<string,int>> conversionTotalAEstandarPormaquinaYproducto(Dictionary<string,Dictionary<string,int>> produccion){
            Dictionary<string,int> productos;
            List<string> productosLlaves;
            int produccionActual;
            List<string> maquinasllaves = new List<string>(produccion.Keys);
            for (int i = 0; i < produccion.Count(); i++)
            {
                productos = produccion[maquinasllaves[i]];
                productosLlaves = new List<string>(productos.Keys);
                for (int j = 0; j < productosLlaves.Count(); j++)
                {   
                    produccionActual = productos[productosLlaves[j]];

                    CommandIngDoc.Connection = conexionIngDoc.OpeAbrirConex();
                    CommandIngDoc.CommandText = @"
                            SELECT  IIM.IMFLPF
                            FROM  [BD_SeguimientoPlanta].[BPCS].[IIM]                        
                            Where IPROD  = '" + productosLlaves[j] + "';";
                    DataReaderIngDoc = CommandIngDoc.ExecuteReader();

                    if(DataReaderIngDoc.Read()){
                        produccionActual = (int) Math.Round(produccionActual * float.Parse(DataReaderIngDoc.GetValue(0).ToString()));
                        //diccionario.Add(DataReaderIngDoc.GetValue(0).ToString(),DataReaderIngDoc.GetDecimal(3) * Decimal.Parse(DataReaderBPCS.GetValue(3).ToString()));
                    }else{
                        produccionActual = -1;
                    }
                    CommandIngDoc.Connection = conexionIngDoc.OpeCerrarConex();
                    productos[productosLlaves[j]] = produccionActual;

                }
                produccion[maquinasllaves[i]] = productos;
            }
            return produccion;
        }
        public Dictionary<string, int> ProduccionActualPorMaquinaPorHora(Dictionary<string,Dictionary<string,int>> produccion,int Periodotiempo){
            //Dictionary<string, Dictionary<string, int>> produccion = MaquinaProductosProduccionActual1turno();
            int suma = 0;
            float resul = 0;
            Dictionary<string,int> ProduccionPorHora = new Dictionary<string,int>();
            List<string> maquinasllaves = new List<string>(produccion.Keys);
            Dictionary<string, int> cantidadSegunProductos;
            Dictionary<string, float> tiempo;

            if(Periodotiempo == 1){
                tiempo = gespline.tiempoTrabajadoActual1turno();
            }else if(Periodotiempo == 2){
                tiempo = gespline.tiempoTrabajadoActual2turno(true);
            }else if(Periodotiempo == 3){
                tiempo = gespline.tiempoTrabajadoActual2turno(false);
            }else{
                return null;
            }

            for (int i = 0; i < maquinasllaves.Count(); i++)
            {
                cantidadSegunProductos = produccion[maquinasllaves[i]];
                foreach (var item in cantidadSegunProductos)
                {
                    suma += item.Value;
                }
                ProduccionPorHora.Add(maquinasllaves[i],(int) Math.Round(suma/tiempo[maquinasllaves[i]]));
                suma = 0;
            }
            return ProduccionPorHora;
        }
    }
}