using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PM_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Get the connection string from the appsettings.json or environment variables
            return _configuration.GetConnectionString("DefaultConnection");
        }
    }


}
