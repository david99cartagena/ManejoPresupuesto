using Dapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task<int> Contar(int usuarioId);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, PaginacionViewModel paginacion);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioCategorias : RepositorioBase, IRepositorioCategorias
    {
        public RepositorioCategorias(IConfiguration configuration) : base(configuration) { }

        public async Task Crear(Categoria categoria)
        {
            using var connection = CrearConexion();
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId) 
                Values (@Nombre, @TipoOperacionId, @UsuarioId);
                SELECT SCOPE_IDENTITY();", categoria);
            categoria.Id = id;
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, PaginacionViewModel paginacion)
        {
            using var connection = CrearConexion();
            return await connection.QueryAsync<Categoria>(
                @$"SELECT * 
                FROM Categorias 
                WHERE UsuarioId = @usuarioId
                ORDER BY Nombre
                OFFSET {paginacion.RecordsASaltar}
                ROWS FETCH NEXT {paginacion.RecordsPorPagina} ROWS ONLY",
                new { usuarioId });
        }

        public async Task<int> Contar(int usuarioId)
        {
            using var connection = CrearConexion();
            return await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*) FROM Categorias WHERE UsuarioId = @usuarioId",
                new { usuarioId });
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connection = CrearConexion();
            return await connection.QueryAsync<Categoria>(
                @"SELECT * FROM Categorias WHERE 
                UsuarioId = @usuarioId AND 
                TipoOperacionId = @TipoOperacionId",
                new { usuarioId, tipoOperacionId });
        }
        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = CrearConexion();
            return await connection.QueryFirstOrDefaultAsync<Categoria>(
                @"SELECT * FROM Categorias WHERE Id = @Id AND UsuarioId = @UsuarioId",
                new { id, usuarioId });
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var connection = CrearConexion();
            await connection.ExecuteAsync(
                @"UPDATE Categorias SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionID 
                WHERE Id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connection = CrearConexion();
            await connection.ExecuteAsync(@"DELETE Categorias WHERE Id = @Id", new { id });
        }

    }
}
