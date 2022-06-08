using Microsoft.AspNetCore.Mvc;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
namespace api.Data{
    public class REmployee:Repository<Employee>,IREmployee
    {
        public REmployee(ApplicationDbContext db): base(db) 
        { 
        }
        public async Task<IEnumerable<Employee>> GetAllAsync() 
        { 
            return await Task.Run(()=>GetAll());
        }
    }
}