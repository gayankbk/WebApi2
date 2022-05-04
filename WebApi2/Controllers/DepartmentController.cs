using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApi2.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration,IWebHostEnvironment _env)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"Select DepartmentID,DepartmentName from dbo.Department";
            DataTable dataTable = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeConnection");
            //SqlDataReader myReader;
            //using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            //{
            //    myCon.Open();
            //    using (SqlCommand myCommand = new SqlCommand(query, myCon))
            //    {
            //        myReader = myCommand.ExecuteReader();
            //        dataTable.Load(myReader);
            //        myReader.Close();
            //        myCon.Close();
            //    }
            //}

            //sql connection object
            using (SqlConnection conn = new SqlConnection(sqlDatasource))
            {

                //set stored procedure name
                string spName = @"dbo.sp_getDepartment";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);

                //Set SqlParameter - the employee id parameter value will be set from the command line
                    //SqlParameter param1 = new SqlParameter();
                    //param1.ParameterName = "@employeeID";
                    // param1.SqlDbType = SqlDbType.Int;
                    //param1.Value = 1;

                    //add the parameter to the SqlCommand object
                    //cmd.Parameters.Add(param1);

                //open connection
                conn.Open();

                //set the SqlCommand type to stored procedure and execute
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                dataTable.Load(dr);

                dr.Close();
                conn.Close();

                return new JsonResult(dataTable);
            }
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"Insert into dbo.Department values ('"+ department.DepartmentName+ "')";
            DataTable dataTable = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    dataTable.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(dataTable);
        }

        [HttpPut]
        public JsonResult Put(Department dep)
        {
            string query = @"
                            Update dbo.Department set DepartmentName=
                            '" + dep.DepartmentName + @"'
                            where DepartmentId="+dep.DepartmentID+@"
                            ";
            DataTable dataTable = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    dataTable.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Department updeted");
        }
    }
}
