using Microsoft.AspNetCore.Mvc;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using api.Data;
using Microsoft.Data.SqlClient;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IRUnit _unit;
        public EmployeeController(
            ApplicationDbContext coreAdminContext,
            IRUnit unit)
        {
            _dbContext = coreAdminContext;
            _unit=unit;
        }

        public IConfigurationRoot Configuration { get; set; }
        [HttpPost("SaveOrEditEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrEditEmployee(Employee objData)
        {
            #region  Save or Update Employee
            using(var _dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if(_unit.isSave(objData.ID))
                    {
                        objData.ID=_unit.GetNewID();
                        _unit.Employee.Add(objData);
                    }else{
                        Employee data = _unit.Employee.GetFirstOrDefault(x=>x.ID==objData.ID);
                        if(data==null) return NotFound(StaticMSG.NotFound(nameof(Employee)));
                        data.ID=objData.ID;
                        data.FirstName=objData.FirstName;
                        data.LastName=objData.LastName;
                        data.MiddleName=objData.MiddleName;
                        _unit.Employee.Update(data);
                    }
                    _unit.SaveChanges();
                    _dbContextTransaction.Commit();
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    if(_dbContextTransaction!=null)
                        _dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                        return BadRequest(ex.InnerException.Message);
                    else
                        return BadRequest(ex.Message);
                }
            }
            #endregion
        }
        [HttpGet("DeleteEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            #region  Delete Individual Employee
            using(var _dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var data = _unit.Employee.GetFirstOrDefault(x=>x.ID==id);
                    if(data==null) return NotFound(StaticMSG.NotFound(nameof(Employee)));
                    _unit.Employee.Remove(data);
                    _unit.SaveChanges();
                    _dbContextTransaction.Commit();
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    if(_dbContextTransaction!=null)
                        _dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                        return BadRequest(ex.InnerException.Message);
                    else
                        return BadRequest(ex.Message);
                }
            }
            #endregion
        }
        [HttpGet("GetAllEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEmployee()
        {
            #region  Get All Employee
            try
            {
                var data=await _unit.Employee.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                        return BadRequest(ex.InnerException.Message);
                else
                    return BadRequest(ex.Message);
            }
            #endregion
        }
        [HttpGet("GetEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmployee(string id)
        {
            #region  Get Individual Employee
            try
            {
                var data =_unit.Employee.GetFirstOrDefault(x=>x.ID==id);
                if(data==null) return NotFound(StaticMSG.NotFound());
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                        return BadRequest(ex.InnerException.Message);
                else
                    return BadRequest(ex.Message);
            }
            #endregion
        }
        
    }
    
}