using System.Data;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;

namespace ConsultasSQL.Model
{
    public class DbBpcsVen
    {
        private SqlConnection ConOpe = new SqlConnection("Data Source = DCTDTDB02; Initial Catalog = DbBpcsVen; TrustServerCertificate=True;Persist Security Info=True;User ID=usrLectura;Password=usrLectura");
        
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