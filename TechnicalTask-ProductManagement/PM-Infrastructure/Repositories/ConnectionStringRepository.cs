using Microsoft.Extensions.Configuration;
using PM_Domain.Interfaces;

namespace PM_Infrastructure.Repositories
{
    public class ConnectionStringRepository : IConnectionStringRepository
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }
    }


}
