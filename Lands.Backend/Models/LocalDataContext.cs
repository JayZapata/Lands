using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lands.Backend.Models
{
    using Domain;

    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<Lands.Domain.User> Users { get; set; }
    }

}