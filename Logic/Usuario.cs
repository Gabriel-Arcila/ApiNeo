using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;

namespace ConsultasSQL.Logic{
    public class UsuarioLogic 
    {
        private DbUsuarioConexion conexionUsuario = new DbUsuarioConexion();

        private Usuario usuario;

        private SqlCommand comandUsuario = new SqlCommand();

        SqlDataReader? DataReaderUsuario;

        public Usuario ObtenerUsuarioPorFicha(string ficha){
            var dataTable = new DataTable();
            comandUsuario.Connection = conexionUsuario.OpeAbrirConex();
            usuario = new Usuario();
            comandUsuario.CommandText = @"
                    SELECT MAESTRO_TRABAJADOR.NOMFI1 AS [Nombre], MAESTRO_TRABAJADOR.APEFI1 AS [Apellido],DEPARTAMENTOS.DESDPT AS [Departamento],CARGOS.DESCGO AS [Cargo]
                    FROM PLST.SPI.MAESTRO_TRABAJADOR 
                    INNER JOIN PLST.SPI.DEPARTAMENTOS ON DEPARTAMENTOS.CODDPT = MAESTRO_TRABAJADOR.DPTFIC
                    INNER JOIN PLST.SPI.CARGOS ON MAESTRO_TRABAJADOR.CGOFIC = CARGOS.CODCGO
                    WHERE CARGOS.CIACGO = '10' AND MAESTRO_TRABAJADOR.CODFIC LIKE '%" + ficha + @"%'
            "; 
            DataReaderUsuario = comandUsuario.ExecuteReader();
            dataTable.Load(DataReaderUsuario);
            foreach (DataRow row in dataTable.Rows)
            {
                usuario.nombre = row["Nombre"].ToString();
                usuario.apellido = row["Apellido"].ToString();
                usuario.cargo = row["Cargo"].ToString();
                usuario.departamento = row["Departamento"].ToString();
                break;
            }
            return usuario;
        }
    }
}