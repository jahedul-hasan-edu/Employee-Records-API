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
        private readonly ConnectionHelper _conHelper;
        public EmployeeController(
            ApplicationDbContext coreAdminContext,
            IRUnit unit)
        {
            _dbContext = coreAdminContext;
            _unit=unit;
            _conHelper=new ConnectionHelper(_dbContext);
        }

        public IConfigurationRoot Configuration { get; set; }
        [HttpPost("SaveOrEditEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrEditEmployee(Employee objData)
        {
            #region  SaveOrEditEmployee With Entity and Repository
            // using(var _dbContextTransaction = _dbContext.Database.BeginTransaction())
            // {
            //     try
            //     {
            //         if(_unit.isSave(objData.ID))
            //         {
            //             objData.ID=_unit.GetNewID();
            //             _unit.Employee.Add(objData);
            //         }else{
            //             Employee data = _unit.Employee.GetFirstOrDefault(x=>x.ID==objData.ID);
            //             if(_unit.isSafe(data))
            //             {
            //                 data=_unit.Employee.SetData(objData);
            //                 _unit.Employee.Update(data);
            //             }
            //         }
            //         _unit.SaveChanges();
            //         _dbContextTransaction.Commit();
            //         return Ok(true);
            //     }
            //     catch (Exception ex)
            //     {
            //         if(_dbContextTransaction!=null)
            //             _dbContextTransaction.Rollback();
            //         if (ex.InnerException != null)
            //             return BadRequest(ex.InnerException.Message);
            //         else
            //             return BadRequest(ex.Message);
            //     }
            // }
            #endregion
            #region  SaveOrEditEmployee ADO.NET(Without Entity) and Repository
            try
            {
                _unit.Employee.SaveOrUpdateADO(_conHelper.GetConnectionString,objData);
                return Ok(true);
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
        [HttpGet("DeleteEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            #region  DeleteEmployee With Entity and Repository
            // using(var _dbContextTransaction = _dbContext.Database.BeginTransaction())
            // {
            //     try
            //     {
            //         var data = _unit.Employee.GetFirstOrDefault(x=>x.ID==id);
            //         if(_unit.isSafe(data))
            //         {
            //             _unit.Employee.Remove(data);
            //             _unit.SaveChanges();
            //         }
            //         _dbContextTransaction.Commit();
            //         return Ok(true);
            //     }
            //     catch (Exception ex)
            //     {
            //         if(_dbContextTransaction!=null)
            //             _dbContextTransaction.Rollback();
            //         if (ex.InnerException != null)
            //             return BadRequest(ex.InnerException.Message);
            //         else
            //             return BadRequest(ex.Message);
            //     }
            // }
            #endregion
            #region  DeleteEmployee ADO.NET(Without Entity) and Repository
            try
            {
                _unit.Employee.DeleteADO(_conHelper.GetConnectionString,id);
                return Ok(true);
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
        [HttpGet("GetAllEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEmployee()
        {
            try
            {
                #region  GetAllEmployee With Entity and Repository
                // var data=await _unit.Employee.GetAllAsync();
                #endregion
                #region  GetAllEmployee ADO.NET(Without Entity) and Repository
                var data=_unit.Employee.GetAllADO(_conHelper.GetConnectionString);
                #endregion
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                        return BadRequest(ex.InnerException.Message);
                else
                    return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmployee(string id)
        {
            try
            {
                #region  GetEmployee With Entity and Repository
                // var data =_unit.Employee.GetFirstOrDefault(x=>x.ID==id);
                #endregion
                #region  GetEmployee ADO.NET(Without Entity) and Repository
                Employee data=_unit.Employee.GetADO(_conHelper.GetConnectionString,id);
                #endregion
                if(_unit.isSafe(data))
                {
                    return Ok(data);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                        return BadRequest(ex.InnerException.Message);
                else
                    return BadRequest(ex.Message);
            }
        }
        
    }
    
}