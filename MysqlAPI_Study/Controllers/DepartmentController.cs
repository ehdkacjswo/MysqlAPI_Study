using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MysqlAPI_Study.Models;

// Mysql namespace
using MySql.Data.MySqlClient;

// Fluentd namespace
using Serilog;
using Serilog.Sinks.Fluentd;



#nullable disable
namespace MysqlAPI_Study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        Serilog.Core.Logger log;

        public DepartmentController(IConfiguration configuration)
        {
            log = new LoggerConfiguration().WriteTo.Fluentd().CreateLogger();
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Department> Get()
        {
            string query = @"select DepartmentId, DepartmentName from mytestdb.Department";
            
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                log.Information("Get Succeed");
                using(MySqlCommand myCommand=new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    return table.AsEnumerable().Select(row =>
                    new Department
                    {
                        DepartmentID = row.Field<int>("DepartmentId"),
                        DepartmentName = row.Field<string>("DepartmentName")
                    }) ;
                }
            }
        }

        [HttpPost]
        public JsonResult Put(Department dep)
        {
            string query = $"insert into mytestdb.Department (DepartmentName) values (\"{dep.DepartmentName}\");";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();

                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    if (myCommand.ExecuteNonQuery() == 1)
                    {
                        log.Information($"Put Succeed");
                        return new JsonResult("Addition Succeed");
                    }

                    else
                    {
                        log.Information($"Put Failed");
                        return new JsonResult("Addition Failed");
                    }
                }
            }
        }

        [HttpPatch]
        public JsonResult Patch(Department dep)
        {
            string query = $"update mytestdb.Department set DepartmentName = \"{dep.DepartmentName}\"" +
                $"where DepartmentId = {dep.DepartmentID}";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();

                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    if (myCommand.ExecuteNonQuery() == 1)
                    {
                        log.Information($"Put Succeed");
                        return new JsonResult("Addition Succeed");
                    }

                    else
                    {
                        log.Information($"Put Failed");
                        return new JsonResult("Addition Failed");
                    }
                }
            }
        }
    }
}
