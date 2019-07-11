using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace OnlineShoesStore.Models
{
    public class UserData
    {
        public IConfiguration Configuration { get; }
        public UserData(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void RegisterUser()
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            Console.WriteLine(connectionString);
        }
    }
}
