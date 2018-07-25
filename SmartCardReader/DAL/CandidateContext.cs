using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SmartCardReader.DAL
{
    public class CandidateContext: DbContext
    {


        public CandidateContext(): base("DefaultConnection")
        {
            
        }

        public DbSet<CandidateGrid> Candidates { get; set; }
    }
}