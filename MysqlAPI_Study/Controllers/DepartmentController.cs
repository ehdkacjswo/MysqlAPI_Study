using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MysqlAPI_Study.Models;
using System.Data;
#nullable disable
namespace MysqlAPI_Study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
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

        
    }
}
