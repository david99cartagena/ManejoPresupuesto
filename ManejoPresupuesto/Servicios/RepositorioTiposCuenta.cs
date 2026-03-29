using Dapper;
using ManejoPresupuesto.Models;
//using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuenta
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
        Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenadas);
    }
    public class RepositorioTiposCuenta : RepositorioBase, IRepositorioTiposCuenta
    {
        //private readonly string connectionString;
        //public RepositorioTiposCuenta(IConfiguration configuration)
        //{
        //    connectionString = configuration.GetConnectionString("DefaultConnection");
        //}
        //private SqlConnection CrearConexion()
        //{
        //    return new SqlConnection(connectionString);
        //}
        public RepositorioTiposCuenta(IConfiguration configuration) : base(configuration) { }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            //using var connection = new SqlConnection(connectionString);
            using var connection = CrearConexion();
            var id = await connection.QuerySingleAsync<int>
                //(@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden)
                //VALUES (@Nombre, @UsuarioId, 0); SELECT SCOPE_IDENTITY();", tipoCuenta);
                ("TiposCuentas_Insertar",
                new
                {
                    usuarioId = tipoCuenta.UsuarioId,
                    nombre = tipoCuenta.Nombre
                }, commandType: System.Data.CommandType.StoredProcedure);
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            //using var connection = new SqlConnection(connectionString);
            using var connection = CrearConexion();
            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 FROM TiposCuentas WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
                new { nombre, usuarioId });

            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            //using var connection = new SqlConnection(connectionString);
            using var connection = CrearConexion();
            return await connection.QueryAsync<TipoCuenta>(
                @"SELECT Id, Nombre, Orden FROM TiposCuentas WHERE UsuarioId = @UsuarioId ORDER BY Orden",
                new { usuarioId });
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            //using var connection = new SqlConnection(connectionString);
            using var connection = CrearConexion();
            await connection.ExecuteAsync(@"UPDATE TiposCuentas SET Nombre = @Nombre 
                WHERE Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            //using var connection = new SqlConnection(connectionString);
            using var connection = CrearConexion();
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden FROM TiposCuentas
                    WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId });
            // Objeto Anonimo
        }

        public async Task Borrar(int id)
        {
            //using var connection = new SqlConnection(connectionString);
            using var connection = CrearConexion();
            await connection.ExecuteAsync(@"DELETE TiposCuentas WHERE Id = @Id", new { id });
        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenadas)
        {
            var query = "UPDATE TiposCuentas SET Orden = @Orden WHERE Id = @Id;";
            //using var connection = new SqlConnection(connectionString);
            using var connection = CrearConexion();
            await connection.ExecuteAsync(query, tipoCuentasOrdenadas);
        }
    }
}
