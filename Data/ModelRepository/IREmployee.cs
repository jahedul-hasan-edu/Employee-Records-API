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
using Microsoft.Data.SqlClient;

namespace api.Data{
    public interface IREmployee:IRepository<Employee>{
        Task<IEnumerable<Employee>> GetAllAsync();
    }
}