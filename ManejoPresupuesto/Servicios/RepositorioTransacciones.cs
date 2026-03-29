using Dapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Borrar(int id);
        Task Crear(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioTransacciones : RepositorioBase, IRepositorioTransacciones
    {
        public RepositorioTransacciones(IConfiguration configuration) : base(configuration) { }

        public async Task Crear(Transaccion transaccion)
        {
            using var connection = CrearConexion();
            var id = await connection.QuerySingleAsync<int>("Transacciones_Insertar",
                new
                {
                    transaccion.UsuarioId,
                    transaccion.FechaTransaccion,
                    transaccion.Monto,
                    transaccion.CategoriaId,
                    transaccion.CuentaId,
                    transaccion.Nota
                },
                commandType: System.Data.CommandType.StoredProcedure);

            transaccion.Id = id;
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(
            ObtenerTransaccionesPorCuenta modelo)
        {
            using var connection = CrearConexion();
            return await connection.QueryAsync<Transaccion>
                (@"SELECT t.Id, t.Monto, t.FechaTransaccion, c.Nombre as Categoria,
                    cu.Nombre as Cuenta, c.TipoOperacionId
                    FROM Transacciones t
                    INNER JOIN Categorias c
                    ON c.Id = t.CategoriaId
                    INNER JOIN Cuentas cu
                    ON cu.Id = t.CuentaId
                    WHERE t.CuentaId = @CuentaId AND t.UsuarioId = @UsuarioId
                    AND FechaTransaccion BETWEEN @FechaInicio AND @FechaFin", modelo);

        }
        public async Task Actualizar(Transaccion transaccion, decimal montoAnterior,
            int cuentaAnteriorId)
        {
            using var connection = CrearConexion();
            await connection.ExecuteAsync("Transacciones_Actualizar",
            new
            {
                transaccion.Id,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.CategoriaId,
                transaccion.CuentaId,
                transaccion.Nota,
                montoAnterior,
                cuentaAnteriorId

            },
            commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Transaccion> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = CrearConexion();
            return await connection.QueryFirstOrDefaultAsync<Transaccion>(
                @"SELECT Transacciones.*, cat.TipoOperacionId 
                FROM Transacciones
                INNER JOIN Categorias cat
                ON cat.Id = Transacciones.CategoriaId
                WHERE Transacciones.Id = @Id and Transacciones.UsuarioId = @UsuarioId",
                new { id, usuarioId });
        }

        public async Task Borrar(int id)
        {
            using var connection = CrearConexion();
            await connection.ExecuteAsync("Transacciones_Borrar",
                new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
