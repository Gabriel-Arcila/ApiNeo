using System.Data;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;

namespace ConsultasSQL.Model
{
    public class DbIngDoc
    {
        private SqlConnection ConOpe = new SqlConnection("Data Source=10.20.1.60\\DBVEN01; Initial Catalog =DOC_IngI; TrustServerCertificate=True;Persist Security Info=True;User ID=usrLectura;Password= ");
        //private SqlConnection ConOpe = new SqlConnection("Data Source=10.20.1.60\\DBVEN01; Initial Catalog = SIPDATABASE;User ID=portaluser;Password=PORT34erySADF ");
        // // Conexion para los centros y ordenes de producción
        public SqlConnection OpeAbrirConex()
        {
            if (ConOpe.State == ConnectionState.Closed)
                ConOpe.Open();
            return ConOpe;
        }

        public SqlConnection OpeCerrarConex()
        {
            if (ConOpe.State == ConnectionState.Open)
                ConOpe.Close();
            return ConOpe;
        }
    }
}