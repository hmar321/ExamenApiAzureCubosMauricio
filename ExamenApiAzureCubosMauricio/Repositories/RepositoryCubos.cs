using ExamenApiAzureCubosMauricio.Data;
using ExamenApiAzureCubosMauricio.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenApiAzureCubosMauricio.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;

        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }
        public async Task<List<Cubo>> GetCubosAsync()
        {
            List<Cubo>cubos= await this.context.Cubos.ToListAsync();
            foreach (Cubo cubo in cubos)
            {
                cubo.Imagen = "https://storageexamenmauricio.blob.core.windows.net/cubos/" + cubo.Imagen;
            }
            return cubos;
        }
        public async Task<List<Cubo>> GetCuboByMarcaAsync(string marca)
        {
            List<Cubo> cubos = await this.context.Cubos.Where(x => x.Marca == marca).ToListAsync();
            foreach (Cubo cubo in cubos)
            {
                cubo.Imagen = "https://storageexamenmauricio.blob.core.windows.net/cubos/" + cubo.Imagen;
            }
            return cubos;
        }
        public async Task<Usuario> LoginUsuarioAsync(string email,string password)
        {
            Usuario usuario = await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            if (usuario!=null)
            {
                usuario.Imagen = "https://storageexamenmauricio.blob.core.windows.net/usuarios/" + usuario.Imagen;
            }
            return usuario;
        }
        public async Task<List<Pedido>> GetUsuarioPedidosAsync(int idusuario) {
            return await this.context.Pedidos.Where(x => x.IdUsuario == idusuario).ToListAsync();
        }
    }
}
