using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;

namespace ConsultasSQL.Logic{
    public class BPCSVen 
    {
        private DbBpcsVen conexionBPCSVen = new DbBpcsVen();
        private SqlCommand CommandBPCSVen = new SqlCommand();
        private SqlDataReader? DataReaderBPCSVen;

        public Dictionary<string,string> ObtenerProductosActualesSegunCentroDeCosto(string centroCosto){
            Dictionary<string,string> ProductosActuales = new Dictionary<string,string>();
            DataTable dataTable = new DataTable();
            CommandBPCSVen.Connection = conexionBPCSVen.OpeAbrirConex();
            CommandBPCSVen.CommandText = @"
                SELECT TOP (1000) [Vista_Orden_Fabricacion].SPROD AS [IdProducto],Vista_Producto.IDESC AS [Descripcion]
                FROM [DbBpcsVen].[dbo].[Vista_Orden_Fabricacion]
                INNER JOIN [dbo].Vista_Producto ON Vista_Producto.IPROD = [Vista_Orden_Fabricacion].SPROD
                WHERE [ESTADO] = 'ABIERTA' AND SWRKC = " + centroCosto + @"
            ";
            DataReaderBPCSVen = CommandBPCSVen.ExecuteReader();
            dataTable.Load(DataReaderBPCSVen);

            foreach (DataRow row in dataTable.Rows)
            {
                if(! ProductosActuales.ContainsKey(row["IdProducto"].ToString())){
                    ProductosActuales.Add(row["IdProducto"].ToString(),row["Descripcion"].ToString());
                }
            }

            return ProductosActuales;
        }
    }
}