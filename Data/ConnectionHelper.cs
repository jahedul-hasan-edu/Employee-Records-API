using api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace  api.Data
{
    public class ConnectionHelper{
        private readonly ApplicationDbContext _dbContext;
        public ConnectionHelper(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
            this.GetConnectionString=_dbContext.Database.GetConnectionString();
        }
        public string GetConnectionString { get;private set; }
    }
}