using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFiller.DB
{
    public class BCCReclaimContext : DbContext
    {
        public DbSet<Multisig> MultiSigs { get; set; }
    }
}
