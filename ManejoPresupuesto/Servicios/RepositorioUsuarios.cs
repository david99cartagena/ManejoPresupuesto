using Dapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
    }
    public class RepositorioUsuarios : RepositorioBase, IRepositorioUsuarios
    {
        public RepositorioUsuarios(IConfiguration configuration) : base(configuration) { }

        public async Task<int> CrearUsuario(Usuario usuario)
        {
            //usuario.EmailNormalizado = usuario.Email.ToUpper();
            using var connection = CrearConexion();
            var usuarioId = await connection.QuerySingleAsync<int>(@"
                INSERT INTO Usuarios (Email, EmailNormalizado, PasswordHash)
                VALUES (@Email, @EmailNormalizado, @PasswordHash);
                SELECT SCOPE_IDENTITY();
                ", usuario);
            
            await connection.ExecuteAsync("CrearDatosUsuariosNuevo", new { usuarioId },
                commandType: System.Data.CommandType.StoredProcedure);

            return usuarioId;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
        {
            using var connection = CrearConexion();
            return await connection.QuerySingleOrDefaultAsync<Usuario>
                (@"SELECT * FROM Usuarios WHERE EmailNormalizado = @emailNormalizado", 
                new { emailNormalizado });
        }

    }
}
