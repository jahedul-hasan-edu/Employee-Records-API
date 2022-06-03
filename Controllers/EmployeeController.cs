using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.Transactions;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Globalization;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Extensions; 
using OtpNet;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using api.Data;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly coreadminContext _coreAdminContext;
        private readonly UrlEncoder _urlEncoder;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(
            ILogger<EmployeeController> logger,
            IConfiguration config,
            coreadminContext coreAdminContext,
            IWebHostEnvironment env,
            UrlEncoder urlEncoder)
        {
            _logger = logger;
            _config = config;
            _coreAdminContext = coreAdminContext;
            _env = env;
            _urlEncoder = urlEncoder;
        }

        public IConfigurationRoot Configuration { get; set; }
        [HttpPost("SaveOrEditEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrEditEmployee(Employee objData)
        {
            using(var _coreAdminContextTransaction = _coreAdminContext.Database.BeginTransaction())
            {
                try
                {
                    if(string.IsNullOrEmpty(objData.ID))
                    {
                        objData.ID=Guid.NewGuid().ToString();
                        _coreAdminContext.Employee.Add(objData);
                    }else{
                        Employee data = _coreAdminContext.Employee.Where(x => x.ID == objData.ID).FirstOrDefault();
                        if(data==null)
                        {
                            return BadRequest("Data Not Found!"); 
                        }
                        data.ID=objData.ID;
                        data.FirstName=objData.FirstName;
                        data.LastName=objData.LastName;
                        data.MiddleName=objData.MiddleName;
                        _coreAdminContext.Employee.Update(data);
                    }
                    _coreAdminContext.SaveChanges();
                    _coreAdminContextTransaction.Commit();
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    if(_coreAdminContextTransaction!=null)
                    {
                        _coreAdminContextTransaction.Rollback();
                    }
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        [HttpGet("DeleteEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            using(var _coreAdminContextTransaction = _coreAdminContext.Database.BeginTransaction())
            {
                try
                {
                    var data = _coreAdminContext.Employee.Where(x => x.ID == id).FirstOrDefault();
                    if(data==null)
                    {
                        return BadRequest("Data Not Found!"); 
                    }
                    _coreAdminContext.Employee.Remove(data);
                    _coreAdminContext.SaveChanges();
                    _coreAdminContextTransaction.Commit();
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    if(_coreAdminContextTransaction!=null)
                    {
                        _coreAdminContextTransaction.Rollback();
                    }
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        [HttpGet("GetAllEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEmployee()
        {
            try
            {
                var data=await _coreAdminContext.Employee.ToListAsync();
                return Ok(new {res=data});
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return BadRequest(ex.InnerException.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpGet("GetEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmployee(string id)
        {
            try
            {
                var data = _coreAdminContext.Employee.Where(x => x.ID == id).FirstOrDefault();
                if(data==null)
                {
                    return BadRequest("Data Not Found!"); 
                }
                return Ok(new {res=data});
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return BadRequest(ex.InnerException.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        
    }
    
}