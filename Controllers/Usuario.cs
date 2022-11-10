using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ConsultasSQL.Model;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;
using ConsultasSQL.Logic;

namespace ConsultasSQL.Controllers{

    [ApiController]
    [Route("Usuario")]

    public class UsuarioController : ControllerBase
    {
        private UsuarioLogic usuarioLogic = new UsuarioLogic();

        [HttpGet]
        [Route("BuscarUsuarioPorFicha/{ficha}")]
        public dynamic objePorHoraProductoActualEstandar(string ficha){
            string JSONString = string.Empty;
            ficha = ficha.ToUpper();
            Usuario usuario;
            usuario = usuarioLogic.ObtenerUsuarioPorFicha(ficha);
            JSONString = JsonConvert.SerializeObject(usuario);
            return JSONString;
        }
    }
}