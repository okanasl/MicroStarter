using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MicroStarter.Api.Data
{
    public class MicroStarterApiContext : DbContext
    {
        /// <summary>
        /// Constructor without params
        /// </summary>
        public MicroStarterApiContext()
            : base() 
        {

        }
        /// <summary>
        /// Constructor with options
        /// </summary>
        /// <param name="options"></param>
        public MicroStarterApiContext(DbContextOptions<MicroStarterApiContext> options)
            : base(options)
        {
        }
        /// <summary>
        /// Override Model Create
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}