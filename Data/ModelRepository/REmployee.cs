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
    public class REmployee:Repository<Employee>,IREmployee{

        private readonly ApplicationDbContext _db;
        public REmployee(ApplicationDbContext db) 
        : base(db) 
        { 
            _db=db;
        }
        public async Task<IEnumerable<Employee>> GetAllAsync() 
        { 
            return await Task.Run(()=>GetAll());
        }
        public Employee SetData(Employee objData)
        {
            Employee data=new Employee();
            data.ID=objData.ID;
            data.FirstName=objData.FirstName;
            data.LastName=objData.LastName;
            data.MiddleName=objData.MiddleName;
            return data;
        }
        public Employee SaveOrUpdateADO(string connectionString,Employee objData)
        {
            Employee ret=new Employee();
            SqlTransaction transaction=null;
            using(SqlConnection sqlCon=new SqlConnection(connectionString))
            {
                try{
                    sqlCon.Open();
                    transaction=sqlCon.BeginTransaction();
                    if(!string.IsNullOrEmpty(objData.ID))
                        DeleteADO(connectionString,objData.ID);
                    string sql="INSERT INTO Employee(ID,FirstName,MiddleName,LastName) VALUES(@ID,@FirstName,@MiddleName,@LastName)";
                    SqlCommand sqlCmd=new SqlCommand(sql,sqlCon,transaction);
                    sqlCmd.Parameters.AddWithValue("@ID",Guid.NewGuid().ToString().ToUpper());
                    sqlCmd.Parameters.AddWithValue("@FirstName",objData.FirstName);
                    sqlCmd.Parameters.AddWithValue("@MiddleName",objData.MiddleName);
                    sqlCmd.Parameters.AddWithValue("@LastName",objData.LastName);
                    sqlCmd.ExecuteNonQuery();
                    transaction.Commit();
                    ret=objData;
                }catch(Exception exp){
                    if(transaction!=null)
                        transaction.Rollback();
                    throw exp;
                }
            }
            return ret;
        }
        
        public void DeleteADO(string connectionString,string ID)
        {
            SqlTransaction transaction=null;
            using(SqlConnection sqlCon=new SqlConnection(connectionString))
            {
                try{
                    sqlCon.Open();
                    transaction=sqlCon.BeginTransaction();
                    Employee data=this.GetADO(connectionString,ID);
                    if(data==null)
                    {
                        throw new Exception("Data not found!");
                    }
                    string sql="delete Employee where ID=@ID";
                    SqlCommand sqlCmd=new SqlCommand(sql,sqlCon,transaction);
                    sqlCmd.Parameters.AddWithValue("@ID",ID);
                    sqlCmd.ExecuteNonQuery();
                    transaction.Commit();
                }catch(Exception exp){
                    if(transaction!=null)
                        transaction.Rollback();
                    throw exp;
                }
            }
        }
        public IEnumerable<Employee> GetAllADO(string connectionString)
        {
            List<Employee> listEmployee=new List<Employee>();
            try{
                using(SqlConnection sqlCon=new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string sql="SELECT ID,FirstName,MiddleName,LastName FROM Employee";
                    SqlCommand sqlCmd=new SqlCommand(sql,sqlCon);
                    SqlDataReader sqlData=sqlCmd.ExecuteReader();
                    while(sqlData.Read())
                    {
                        listEmployee.Add(new Employee(){
                            ID=Convert.ToString(sqlData["ID"]),
                            FirstName=Convert.ToString(sqlData["FirstName"]),
                            MiddleName=Convert.ToString(sqlData["MiddleName"]),
                            LastName=Convert.ToString(sqlData["LastName"])
                        });
                    }
                }
            }catch(Exception exp){
                throw exp;
            }
            return listEmployee;
        }
        public Employee GetADO(string connectionString,string ID)
        {
            Employee employee=new Employee();
            try{
                using(SqlConnection sqlCon=new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string sql="SELECT ID,FirstName,MiddleName,LastName FROM Employee WHERE ID=@ID";
                    SqlCommand sqlCmd=new SqlCommand(sql,sqlCon);
                    sqlCmd.Parameters.AddWithValue("@ID",ID);
                    SqlDataReader sqlData=sqlCmd.ExecuteReader();
                    while(sqlData.Read())
                    {
                        employee.ID=Convert.ToString(sqlData["ID"]);
                        employee.FirstName=Convert.ToString(sqlData["FirstName"]);
                        employee.MiddleName=Convert.ToString(sqlData["MiddleName"]);
                        employee.LastName=Convert.ToString(sqlData["LastName"]);
                    }
                }
            }catch(Exception exp){
                throw exp;
            }
            return employee;
        }

        void SaveChangesAsync()
        {
            _db.SaveChangesAsync();
        }
    }
}