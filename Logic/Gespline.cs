using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;

namespace ConsultasSQL.Logic{
    public class Gespline 
    {
        private DbIngDoc conexionIngDoc = new DbIngDoc();
        private SqlCommand CommandIngDoc = new SqlCommand();
        private SqlDataReader? DataReaderIngDoc;

        private DbSIPDATABASE conexionSIPDATABASE = new DbSIPDATABASE();
        private SqlCommand comandSIPDATABASE = new SqlCommand();
        SqlDataReader? DataReaderSIPDATABASE;

        public List<string> MaquinasGesplineActivos1turno(){
            List<string> maquina = new List<string>();
            var dataTable = new DataTable();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
                comandSIPDATABASE.CommandText = @"
                        SELECT CUADROPNFINAL.CODIGOPROCESO
                        FROM SIPDATABASE.dbo.CUADROPNFINAL 
                        WHERE CUADROPNFINAL.[FECHAENTRADA] >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '05:50:00' AND CUADROPNFINAL.[FECHAENTRADA] < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '18:00:00'
                        GROUP BY CUADROPNFINAL.CODIGOPROCESO
                        ORDER BY CUADROPNFINAL.CODIGOPROCESO;
                "; 
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);

            foreach (DataRow row in dataTable.Rows)
            {
                maquina.Add(row["CODIGOPROCESO"].ToString());
            }
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            return maquina;
        }
        
        public List<string> MaquinasGesplineActivos2turnoDespues0am(){
            List<string> maquina = new List<string>();
            var dataTable = new DataTable();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
                comandSIPDATABASE.CommandText = @"
                        SELECT CUADROPNFINAL.CODIGOPROCESO
                        FROM SIPDATABASE.dbo.CUADROPNFINAL 
                        WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),-1) + '17:55:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '6:00:00'
                        GROUP BY CUADROPNFINAL.CODIGOPROCESO
                        ORDER BY CUADROPNFINAL.CODIGOPROCESO;
                ";  
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);

            foreach (DataRow row in dataTable.Rows)
            {
                maquina.Add(row["CODIGOPROCESO"].ToString());
            }
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            return maquina;
        }

        public List<string> MaquinasGesplineActivos2turnoAntes0am(){
            List<string> maquina = new List<string>();
            var dataTable = new DataTable();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
                comandSIPDATABASE.CommandText = @"
                        SELECT CUADROPNFINAL.CODIGOPROCESO
                        FROM SIPDATABASE.dbo.CUADROPNFINAL
                        WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '17:55:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),1) + '6:00:00'
                        GROUP BY CUADROPNFINAL.CODIGOPROCESO
                        ORDER BY CUADROPNFINAL.CODIGOPROCESO;
                ";  
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();

            foreach (DataRow row in dataTable.Rows)
            {
                maquina.Add(row["CODIGOPROCESO"].ToString());
            }
            return maquina;
        }

        public Dictionary<string,float> tiempoPerdidoActual1turno(){
            var dataTable = new DataTable();
            Dictionary<string,float> tiempo = new Dictionary<string,float>();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                    SELECT CUADROPNFINAL.CODIGOPROCESO, ISNULL((SUM((CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float)))* 24),0) AS [Tiempo Perdido]
                    FROM SIPDATABASE.dbo.PARADASEJECUTADAS 
                    INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
                    INNER JOIN SIPDATABASE.dbo.PARADAS ON PARADASEJECUTADAS.CODIGOPARADA  = PARADAS.CODIGOPARADA
                    WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '05:50:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '18:00:00'
                    GROUP BY CUADROPNFINAL.CODIGOPROCESO
                    ORDER BY  CUADROPNFINAL.CODIGOPROCESO;
                ";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows)
            {
                tiempo.Add(row["CODIGOPROCESO"].ToString(),float.Parse(row["Tiempo Perdido"].ToString()));
            }
            return tiempo;
        }
        public Dictionary<string,float> tiempoPerdidoActual2turnoAntes0am(){
            var dataTable = new DataTable();
            Dictionary<string,float> tiempo = new Dictionary<string,float>();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                    SELECT CUADROPNFINAL.CODIGOPROCESO, ISNULL((SUM((CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float)))* 24),0) AS [Tiempo Perdido]
                    FROM SIPDATABASE.dbo.PARADASEJECUTADAS 
                    INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
                    INNER JOIN SIPDATABASE.dbo.PARADAS ON PARADASEJECUTADAS.CODIGOPARADA  = PARADAS.CODIGOPARADA
                    WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '17:55:00' AND CUADROPNFINAL.FECHAENTRADA <= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),1) + '23:59:59'
                    GROUP BY CUADROPNFINAL.CODIGOPROCESO
                    ORDER BY  CUADROPNFINAL.CODIGOPROCESO;
                ";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows)
            {
                tiempo.Add(row["CODIGOPROCESO"].ToString(),float.Parse(row["Tiempo Perdido"].ToString()));
            }
            return tiempo;
        }

        public Dictionary<string,float> tiempoPerdidoActual2turnoDespues0am(){
            var dataTable = new DataTable();
            Dictionary<string,float> tiempo = new Dictionary<string,float>();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                    SELECT CUADROPNFINAL.CODIGOPROCESO, ISNULL((SUM((CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float)))* 24),0) AS [Tiempo Perdido]
                    FROM SIPDATABASE.dbo.PARADASEJECUTADAS 
                    INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
                    INNER JOIN SIPDATABASE.dbo.PARADAS ON PARADASEJECUTADAS.CODIGOPARADA  = PARADAS.CODIGOPARADA
                    WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),-1) + '17:55:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),1) + '6:00:00'
                    GROUP BY CUADROPNFINAL.CODIGOPROCESO
                    ORDER BY  CUADROPNFINAL.CODIGOPROCESO;
                ";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows)
            {
                tiempo.Add(row["CODIGOPROCESO"].ToString(),float.Parse(row["Tiempo Perdido"].ToString()));
            }
            return tiempo;
        }
        public Dictionary<string,float> tiempoEjecutadoActual1(){
            var dataTable = new DataTable();
            Dictionary<string,float> tiempo = new Dictionary<string,float>();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                    SELECT CUADROPNFINAL.CODIGOPROCESO, SUM(CUADROPNFINAL.HORASEJECUTADAS) AS [Tiempo Ejecutado]
                    FROM SIPDATABASE.dbo.CUADROPNFINAL
                    WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '05:50:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '18:00:00'
                    GROUP BY CUADROPNFINAL.CODIGOPROCESO
                    ORDER BY  CUADROPNFINAL.CODIGOPROCESO;
                ";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows)
            {
                tiempo.Add(row["CODIGOPROCESO"].ToString(),float.Parse(row["Tiempo Ejecutado"].ToString()));
            }
            return tiempo;
        }
        public Dictionary<string,float> tiempoEjecutadoActual2turnoAntes0am(){
            var dataTable = new DataTable();
            Dictionary<string,float> tiempo = new Dictionary<string,float>();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                    SELECT CUADROPNFINAL.CODIGOPROCESO, SUM(CUADROPNFINAL.HORASEJECUTADAS) AS [Tiempo Ejecutado]
                    FROM SIPDATABASE.dbo.CUADROPNFINAL
                    WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '17:55:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '23:59:59'
                    GROUP BY CUADROPNFINAL.CODIGOPROCESO
                    ORDER BY  CUADROPNFINAL.CODIGOPROCESO;
                ";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows)
            {
                tiempo.Add(row["CODIGOPROCESO"].ToString(),float.Parse(row["Tiempo Ejecutado"].ToString()));
            }
            return tiempo;
        }
        //Todo: continuar
        public Dictionary<string,float> tiempoEjecutadoActual2turnoDespues0am(){
            var dataTable = new DataTable();
            Dictionary<string,float> tiempo = new Dictionary<string,float>();
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                    SELECT CUADROPNFINAL.CODIGOPROCESO, SUM(CUADROPNFINAL.HORASEJECUTADAS) AS [Tiempo Ejecutado]
                    FROM SIPDATABASE.dbo.CUADROPNFINAL
                    WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),-1) + '17:50:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '05:50:00'
                    GROUP BY CUADROPNFINAL.CODIGOPROCESO
                    ORDER BY  CUADROPNFINAL.CODIGOPROCESO;
                ";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows)
            {
                tiempo.Add(row["CODIGOPROCESO"].ToString(),float.Parse(row["Tiempo Ejecutado"].ToString()));
            }
            return tiempo;
        }
        
        public Dictionary<string,float> tiempoTrabajadoActual1turno(){
            Dictionary<string,float> tiempoEjecutado = tiempoEjecutadoActual1();
            Dictionary<string,float> tiempoPerdido = tiempoPerdidoActual1turno();
            List<string> llaves = new List<string>(tiempoEjecutado.Keys);
            
            for (int i = 0; i < llaves.Count(); i++)
            {
                if(tiempoPerdido.ContainsKey(llaves[i])){
                    tiempoEjecutado[llaves[i]] = tiempoEjecutado[llaves[i]] - tiempoPerdido[llaves[i]];
                }else{
                    tiempoEjecutado[llaves[i]] = tiempoEjecutado[llaves[i]];
                }

            }
            return tiempoEjecutado;
        }
        public Dictionary<string,float> tiempoTrabajadoActual2turno(bool band){
            Dictionary<string,float> tiempoEjecutado;
            Dictionary<string,float> tiempoPerdido;
            if(band){ //* si es true antes de 0 am 
                tiempoEjecutado = tiempoEjecutadoActual2turnoAntes0am();
                tiempoPerdido = tiempoPerdidoActual2turnoAntes0am();
            }else{ //* si es false despues de 0 am 
                tiempoEjecutado = tiempoEjecutadoActual2turnoDespues0am();
                tiempoPerdido = tiempoPerdidoActual2turnoDespues0am();
            }
            List<string> llaves = new List<string>(tiempoEjecutado.Keys);
            
            for (int i = 0; i < llaves.Count(); i++)
            {
                tiempoEjecutado[llaves[i]] = tiempoEjecutado[llaves[i]] - tiempoPerdido[llaves[i]];
            }
            return tiempoEjecutado;
        }

        public List<List<string>> obtenerParadasActuales1turno(string centroCosto){
            var dataTable = new DataTable();
            List<string> codigos = new List<string>();
            List<string> parada = new List<string>();
            List<string> tiempo = new List<string>();
            List<string> idRegistro = new List<string>();
            List<string> idArea = new List<string>();   
            List<string> Area = new List<string>();
            List<List<string>> datos = new List<List<string>>(4);


            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                SELECT PARADASEJECUTADAS.CODIGOREGISTRSO,GRUPOSDEPARADAS.CODIGOGRUPOPARADA, PARADAS.NOMBREPARADA ,((CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float)))* 1440 AS [Tiempo Perdido],Partes.parteNombre,Partes.Codigo
                FROM SIPDATABASE.dbo.PARADASEJECUTADAS 
                INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
                INNER JOIN SIPDATABASE.dbo.PARADAS ON PARADASEJECUTADAS.CODIGOPARADA  = PARADAS.CODIGOPARADA
                INNER JOIN dbo.GRUPOSDEPARADAS ON dbo.GRUPOSDEPARADAS.CODIGOGRUPOPARADA = PARADAS.CODIGOGRUPOPARADA
                LEFT JOIN dbo.Partes ON dbo.Partes.Codigo = UPPER(SUBSTRING ([dbo].[PARADASEJECUTADAS].[CODIGOPARADA],0,3))
                WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '05:50:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '18:00:00' AND DATENAME(HOUR, CUADROPNFINAL.FECHAENTRADA) < 17 AND REVERSE(SUBSTRING(REVERSE([PARADAS].[CODIGOPARADA]),1,4)) <> '0114' AND CUADROPNFINAL.CODIGOPROCESO = " + centroCosto + @"
                ORDER BY  [Tiempo Perdido] DESC;";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows){
                idRegistro.Add(row["CODIGOREGISTRSO"].ToString());
                codigos.Add(row["CODIGOGRUPOPARADA"].ToString());
                parada.Add(row["NOMBREPARADA"].ToString());
                tiempo.Add(row["Tiempo Perdido"].ToString());
                idArea.Add(row["parteNombre"].ToString());
                Area.Add(row["Codigo"].ToString());
            }
            datos.Add(idRegistro);
            datos.Add(codigos);
            datos.Add(parada);
            datos.Add(tiempo);
            datos.Add(idArea);
            datos.Add(Area);
            return datos;
        }
        
        public List<List<string>> obtenerParadasActuales1turnoAgrupados(string centroCosto){
            var dataTable = new DataTable();
            List<string> CODIGOPARADA = new List<string>();
            List<string> CodigoParte = new List<string>();
            List<string> NOMBREPARADA = new List<string>();
            List<string> parteNombre = new List<string>();
            List<string> TiempoPerdido = new List<string>();
            List<string> codigoGrupoParada = new List<string>();
            List<List<string>> datos = new List<List<string>>(5);


            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                SELECT PARADAS.CODIGOPARADA,GRUPOSDEPARADAS.CODIGOGRUPOPARADA,Partes.Codigo,PARADAS.NOMBREPARADA,Partes.parteNombre ,sum((CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float)))* 1440 AS [Tiempo Perdido]
                FROM SIPDATABASE.dbo.PARADASEJECUTADAS 
                INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
                INNER JOIN SIPDATABASE.dbo.PARADAS ON PARADASEJECUTADAS.CODIGOPARADA  = PARADAS.CODIGOPARADA
                INNER JOIN dbo.GRUPOSDEPARADAS ON dbo.GRUPOSDEPARADAS.CODIGOGRUPOPARADA = PARADAS.CODIGOGRUPOPARADA
                LEFT JOIN dbo.Partes ON dbo.Partes.Codigo = UPPER(SUBSTRING ([dbo].[PARADASEJECUTADAS].[CODIGOPARADA],0,3))
                WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '05:50:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '18:00:00' AND DATENAME(HOUR, CUADROPNFINAL.FECHAENTRADA) < 17 AND REVERSE(SUBSTRING(REVERSE([PARADAS].[CODIGOPARADA]),1,4)) <> '0114' AND CUADROPNFINAL.CODIGOPROCESO = "+ centroCosto +@"
                GROUP BY PARADAS.CODIGOPARADA,GRUPOSDEPARADAS.CODIGOGRUPOPARADA,Partes.Codigo,PARADAS.NOMBREPARADA,Partes.parteNombre
                ORDER BY  [Tiempo Perdido] DESC;";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows){
                CODIGOPARADA.Add(row["CODIGOPARADA"].ToString());
                codigoGrupoParada.Add(row["CODIGOGRUPOPARADA"].ToString());
                CodigoParte.Add(row["Codigo"].ToString());
                NOMBREPARADA.Add(row["NOMBREPARADA"].ToString());
                parteNombre.Add(row["parteNombre"].ToString());
                TiempoPerdido.Add(row["Tiempo Perdido"].ToString());
            }
            datos.Add(CODIGOPARADA);
            datos.Add(codigoGrupoParada);
            datos.Add(CodigoParte);
            datos.Add(NOMBREPARADA);
            datos.Add(parteNombre);
            datos.Add(TiempoPerdido);
            return datos;
        }

            public List<List<string>> obtenerParadasActuales2turnoAntesDeLas0am(string centroCosto){
            var dataTable = new DataTable();
            List<string> codigos = new List<string>();
            List<string> parada = new List<string>();
            List<string> tiempo = new List<string>();
            List<string> idRegistro = new List<string>();
            List<string> idArea = new List<string>();
            List<string> Area = new List<string>();
            List<List<string>> datos = new List<List<string>>(4);


            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                SELECT PARADASEJECUTADAS.CODIGOREGISTRSO,GRUPOSDEPARADAS.CODIGOGRUPOPARADA, PARADAS.NOMBREPARADA ,((CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float)))* 1440 AS [Tiempo Perdido],Partes.parteNombre,Partes.Codigo
                FROM SIPDATABASE.dbo.PARADASEJECUTADAS 
                INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
                INNER JOIN SIPDATABASE.dbo.PARADAS ON PARADASEJECUTADAS.CODIGOPARADA  = PARADAS.CODIGOPARADA
                INNER JOIN dbo.GRUPOSDEPARADAS ON dbo.GRUPOSDEPARADAS.CODIGOGRUPOPARADA = PARADAS.CODIGOGRUPOPARADA
                INNER JOIN dbo.Partes ON dbo.Partes.Codigo = UPPER(SUBSTRING ([dbo].[PARADASEJECUTADAS].[CODIGOPARADA],0,3))
                WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '18:00:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),1) + '06:00:00' AND DATENAME(HOUR, CUADROPNFINAL.FECHAENTRADA) >= 17 AND REVERSE(SUBSTRING(REVERSE([PARADAS].[CODIGOPARADA]),1,4)) <> '0114' AND CUADROPNFINAL.CODIGOPROCESO = "+ centroCosto + @"
                ORDER BY  [Tiempo Perdido] DESC;";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows){
                idRegistro.Add(row["CODIGOREGISTRSO"].ToString());
                codigos.Add(row["CODIGOGRUPOPARADA"].ToString());
                parada.Add(row["NOMBREPARADA"].ToString());
                tiempo.Add(row["Tiempo Perdido"].ToString());
                idArea.Add(row["parteNombre"].ToString());
                Area.Add(row["Codigo"].ToString());
            }
            datos.Add(idRegistro);
            datos.Add(codigos);
            datos.Add(parada);
            datos.Add(tiempo);
            datos.Add(idArea);
            datos.Add(Area);
            return datos;
        }

        public List<List<string>> obtenerParadasActuales2turnoAntesDeLas0amAgupados(string centroCosto){
            var dataTable = new DataTable();
            List<string> CODIGOPARADA = new List<string>();
            List<string> CodigoParte = new List<string>();
            List<string> NOMBREPARADA = new List<string>();
            List<string> parteNombre = new List<string>();
            List<string> TiempoPerdido = new List<string>();
            List<string> codigoGrupoParada = new List<string>();
            List<List<string>> datos = new List<List<string>>(4);

    
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                SELECT PARADAS.CODIGOPARADA,GRUPOSDEPARADAS.CODIGOGRUPOPARADA,Area.ACodGes,PARADAS.NOMBREPARADA,Area.AParte ,sum((CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float)))* 1440 AS [Tiempo Perdido]
                FROM SIPDATABASE.dbo.ENTRADAEJECUCION
                INNER JOIN SIPDATABASE.dbo.TUPLAEJECUCION ON TUPLAEJECUCION.CODIGOTUPLA = ENTRADAEJECUCION.CODIGOTUPLA
                INNER JOIN SIPDATABASE.dbo.PROCESO ON PROCESO.CODIGOPROCESO = TUPLAEJECUCION.CODIGOPROCESO
                INNER JOIN SIPDATABASE.dbo.PARADASEJECUTADAS ON PARADASEJECUTADAS.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION
                INNER JOIN SIPDATABASE.dbo.PARADAS ON PARADAS.CODIGOPARADA = PARADASEJECUTADAS.CODIGOPARADA 
                INNER JOIN SIPDATABASE.dbo.GRUPOSDEPARADAS ON GRUPOSDEPARADAS.CODIGOGRUPOPARADA  = PARADAS.CODIGOGRUPOPARADA
                LEFT JOIN SIPDATABASE.dbo.Area ON Area.ACodGes = UPPER(SUBSTRING (PARADAS.CODIGOPARADA,0,5))
                WHERE ENTRADAEJECUCION.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '17:55:00' AND ENTRADAEJECUCION.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),1) + '06:00:00' AND REVERSE(SUBSTRING(REVERSE([PARADAS].[CODIGOPARADA]),1,4)) <> '0114' AND PROCESO.CODIGOPROCESO = "+ centroCosto + @"
                GROUP BY  PARADAS.CODIGOPARADA,GRUPOSDEPARADAS.CODIGOGRUPOPARADA,Area.ACodGes,PARADAS.NOMBREPARADA,Area.AParte
                ORDER BY  [Tiempo Perdido] DESC;";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows){
                CODIGOPARADA.Add(row["CODIGOPARADA"].ToString());
                codigoGrupoParada.Add(row["CODIGOGRUPOPARADA"].ToString());
                CodigoParte.Add(row["ACodGes"].ToString());
                NOMBREPARADA.Add(row["NOMBREPARADA"].ToString());
                parteNombre.Add(row["AParte"].ToString());
                TiempoPerdido.Add(row["Tiempo Perdido"].ToString());
            }
            datos.Add(CODIGOPARADA);
            datos.Add(codigoGrupoParada);
            datos.Add(CodigoParte);
            datos.Add(NOMBREPARADA);
            datos.Add(parteNombre);
            datos.Add(TiempoPerdido);
            return datos;
        }

        public List<List<string>> obtenerParadasActuales2turnoDespuesDeLas0amAgrupados(string centroCosto){
            var dataTable = new DataTable();
            List<string> CODIGOPARADA = new List<string>();
            List<string> CodigoParte = new List<string>();
            List<string> NOMBREPARADA = new List<string>();
            List<string> parteNombre = new List<string>();
            List<string> TiempoPerdido = new List<string>();
            List<string> codigoGrupoParada = new List<string>();
            List<List<string>> datos = new List<List<string>>(4);


            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                SELECT PARADAS.CODIGOPARADA,GRUPOSDEPARADAS.CODIGOGRUPOPARADA,Area.ACodGes,PARADAS.NOMBREPARADA,Area.AParte ,sum((CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float)))* 1440 AS [Tiempo Perdido]
                FROM SIPDATABASE.dbo.ENTRADAEJECUCION 
                INNER JOIN SIPDATABASE.dbo.TUPLAEJECUCION ON TUPLAEJECUCION.CODIGOTUPLA = ENTRADAEJECUCION.CODIGOTUPLA
                INNER JOIN SIPDATABASE.dbo.PROCESO ON PROCESO.CODIGOPROCESO = TUPLAEJECUCION.CODIGOPROCESO
                INNER JOIN SIPDATABASE.dbo.PARADASEJECUTADAS ON PARADASEJECUTADAS.CODIGOENTRADAEJECUCION = ENTRADAEJECUCION.CODIGOENTRADAEJECUCION
                INNER JOIN SIPDATABASE.dbo.PARADAS ON PARADAS.CODIGOPARADA = PARADASEJECUTADAS.CODIGOPARADA 
                INNER JOIN SIPDATABASE.dbo.GRUPOSDEPARADAS ON GRUPOSDEPARADAS.CODIGOGRUPOPARADA  = PARADAS.CODIGOGRUPOPARADA
                LEFT JOIN SIPDATABASE.dbo.Area ON Area.ACodGes = UPPER(SUBSTRING (PARADAS.CODIGOPARADA,0,5))
                WHERE ENTRADAEJECUCION.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),-1) + '17:55:00' AND ENTRADAEJECUCION.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '06:00:00' AND REVERSE(SUBSTRING(REVERSE([PARADAS].[CODIGOPARADA]),1,4)) <> '0114' AND PROCESO.CODIGOPROCESO = "+ centroCosto + @"
                GROUP BY PARADAS.CODIGOPARADA,GRUPOSDEPARADAS.CODIGOGRUPOPARADA,Area.ACodGes,PARADAS.NOMBREPARADA,Area.AParte
                ORDER BY  [Tiempo Perdido] DESC;";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows){
                CODIGOPARADA.Add(row["CODIGOPARADA"].ToString());
                codigoGrupoParada.Add(row["CODIGOGRUPOPARADA"].ToString());
                CodigoParte.Add(row["ACodGes"].ToString());
                NOMBREPARADA.Add(row["NOMBREPARADA"].ToString());
                parteNombre.Add(row["AParte"].ToString());
                TiempoPerdido.Add(row["Tiempo Perdido"].ToString());
            }
            datos.Add(CODIGOPARADA);
            datos.Add(codigoGrupoParada);
            datos.Add(CodigoParte);
            datos.Add(NOMBREPARADA);
            datos.Add(parteNombre);
            datos.Add(TiempoPerdido);
            return datos;
        }

        public List<List<string>> obtenerParadasActuales2turnoDespuesDeLas0am(string centroCosto){
            var dataTable = new DataTable();
            List<string> codigos = new List<string>();
            List<string> parada = new List<string>();
            List<string> tiempo = new List<string>();
            List<string> idRegistro = new List<string>();
            List<string> idArea = new List<string>();
            List<string> Area = new List<string>();
            List<List<string>> datos = new List<List<string>>(4);


            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeAbrirConex();
            comandSIPDATABASE.CommandText = @"
                SELECT PARADASEJECUTADAS.CODIGOREGISTRSO,GRUPOSDEPARADAS.CODIGOGRUPOPARADA, PARADAS.NOMBREPARADA ,((CAST(PARADASEJECUTADAS.TIMESPAN as float) - CAST(PARADASEJECUTADAS.FECHAYHORAPARADA as float)))* 1440 AS [Tiempo Perdido],Partes.parteNombre,Partes.Codigo
                FROM SIPDATABASE.dbo.PARADASEJECUTADAS 
                INNER JOIN SIPDATABASE.dbo.CUADROPNFINAL ON CUADROPNFINAL.CODENTRADAEJECUCION = PARADASEJECUTADAS.CODIGOENTRADAEJECUCION 
                INNER JOIN SIPDATABASE.dbo.PARADAS ON PARADASEJECUTADAS.CODIGOPARADA  = PARADAS.CODIGOPARADA
                INNER JOIN dbo.GRUPOSDEPARADAS ON dbo.GRUPOSDEPARADAS.CODIGOGRUPOPARADA = PARADAS.CODIGOGRUPOPARADA
                INNER JOIN dbo.Partes ON dbo.Partes.Codigo = UPPER(SUBSTRING ([dbo].[PARADASEJECUTADAS].[CODIGOPARADA],0,3))
				WHERE CUADROPNFINAL.FECHAENTRADA >= DATEADD(dd,DATEDIFF(dd,0,GETDATE()),-1) + '18:00:00' AND CUADROPNFINAL.FECHAENTRADA < DATEADD(dd,DATEDIFF(dd,0,GETDATE()),0) + '06:00:00' AND DATENAME(HOUR, CUADROPNFINAL.FECHAENTRADA) >= 17 AND REVERSE(SUBSTRING(REVERSE([PARADAS].[CODIGOPARADA]),1,4)) <> '0114' AND CUADROPNFINAL.CODIGOPROCESO = "+ centroCosto +@"
                ORDER BY  [Tiempo Perdido] DESC;";
            DataReaderSIPDATABASE = comandSIPDATABASE.ExecuteReader();
            dataTable.Load(DataReaderSIPDATABASE);
            comandSIPDATABASE.Connection = conexionSIPDATABASE.OpeCerrarConex();
            foreach (DataRow row in dataTable.Rows){
                idRegistro.Add(row["CODIGOREGISTRSO"].ToString());
                codigos.Add(row["CODIGOGRUPOPARADA"].ToString());
                parada.Add(row["NOMBREPARADA"].ToString());
                tiempo.Add(row["Tiempo Perdido"].ToString());
                idArea.Add(row["parteNombre"].ToString());
                Area.Add(row["Codigo"].ToString());
            }
            datos.Add(idRegistro);
            datos.Add(codigos);
            datos.Add(parada);
            datos.Add(tiempo);
            datos.Add(idArea);
            datos.Add(Area);
            return datos;
        }

        public List<List<string>> obtenerParadasActuales2turno(string centroCosto){
            DateTime hoy = DateTime.Now;
            if(! (hoy.Hour >= 18 || hoy.Hour < 24)){
                return this.obtenerParadasActuales2turnoAntesDeLas0am(centroCosto);
            }else{
                return this.obtenerParadasActuales2turnoDespuesDeLas0am(centroCosto);
            }
        }
        public List<List<string>> obtenerParadasActualesConFiltro(List<List<string>> datos,string cadenaIdRegistros){
            //List<List<string>> datos = this.obtenerParadasActuales1turno(centroCosto);
            string[] cadenas = cadenaIdRegistros.Replace("[","").Replace("]","").Split(",");
            int remover;

            for (int i = 0; i < cadenas.Length; i++)
            {
                remover = datos[0].FindIndex(d => d.Contains(cadenas[i]));
                datos[0].RemoveAt(remover);
                datos[1].RemoveAt(remover);
                datos[2].RemoveAt(remover);
                datos[3].RemoveAt(remover);
                datos[4].RemoveAt(remover);
                datos[5].RemoveAt(remover);
            }
            return datos;
        }
        


        // public Dictionary<string,float> ttiempoPerdidoActual2turnoDespues0am(){

        // }
    }
}