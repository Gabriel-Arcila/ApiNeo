using System.Data;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;

namespace ConsultasSQL.Model
{
    public class DbUsuarioConexion
    {
        private SqlConnection conexion = new SqlConnection("Data Source=DCTDTDB02;Initial Catalog=PLST;TrustServerCertificate=True;Persist Security Info=True;User ID=usrConSpi;Password=Spi2017**");

        public SqlConnection OpeAbrirConex()
        {
            if (conexion.State == ConnectionState.Closed)
                conexion.Open();
            return conexion;
        }
        public SqlConnection OpeCerrarConex()
        {
            if (conexion.State == ConnectionState.Open)
                conexion.Close();
            return conexion;
        }
    }
}