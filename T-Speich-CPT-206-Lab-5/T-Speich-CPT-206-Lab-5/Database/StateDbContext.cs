using Microsoft.EntityFrameworkCore;
using T_Speich_CPT_206_Lab_5.Models;

namespace T_Speich_CPT_206_Lab_5
{
    public class StateDbContext : DbContext
    {
        public StateDbContext(DbContextOptions<StateDbContext> options) : base(options) { }

        public DbSet<State> State { get; set; }
    }
}
