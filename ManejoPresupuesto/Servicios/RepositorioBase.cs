using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public class RepositorioBase
    {
        private readonly string _connectionString;
        protected RepositorioBase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        protected SqlConnection CrearConexion()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
