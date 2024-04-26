using Azure.Security.KeyVault.Secrets;
using ExamenApiAzureCubosMauricio.Models;
using ExamenApiAzureCubosMauricio.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExamenApiAzureCubosMauricio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private RepositoryCubos repo;

        public CubosController(RepositoryCubos repo, SecretClient secretClient1)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cubo>>> GetCubos()
        {
            return await this.repo.GetCubosAsync();
        }
        [HttpGet]
        [Route("[action]/{marca}")]
        public async Task<ActionResult<List<Cubo>>> CubosMarca(string marca)
        {
            return await this.repo.GetCuboByMarcaAsync(marca);
        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Usuario>> UsuarioLoged()
        {
            string json = HttpContext.User.FindFirst("UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(json);
            return usuario;

        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Pedido>>> GetPedidosUsuario()
        {
            string json = HttpContext.User.FindFirst("UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(json);
            return await this.repo.GetUsuarioPedidosAsync(usuario.IdUsuario);
        }
    }
}
