using _03_Routing_Verbs_Web_Api.DTO;
using Microsoft.AspNetCore.Http;

﻿using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

namespace _03_Routing_Verbs_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        List<EmployeeDTO> employees = new List<EmployeeDTO>();
        public EmployeesController()
        {
            employees.Add(new EmployeeDTO { Id = 1, Name = "Edgar", Position = "Programador" });
            employees.Add(new EmployeeDTO { Id = 2, Name = "Javier", Position = "Mecanico" });
            employees.Add(new EmployeeDTO { Id = 3, Name = "Goku", Position = "Sayayin" });
        }


        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees()
        {
            return Ok(employees);

        }
        //[HttpGet("GetEmployee")] query params
        //[HttpGet("GetEmployee/{id}/{name}")] parametros separados por /
        [HttpGet("GetEmployees/{id}")]
        public IActionResult GetEmployees(int id)
        {
            var employee = employees.Find(x => x.Id == id);

            return Ok(employee);

        }

        [HttpPost("CreateEmployees")]
        public IActionResult CreateEmployees([FromBody] EmployeeDTO employee)
        {
            employees.Add(employee);

            return Ok(employee);

            [HttpGet("GetEmployees")]
            public IActionResult GetEmployees()
            {
                return Ok(new { message = "Employees List" });

            }

            [HttpGet("GetEmployee")] 
            public IActionResult GetEmployee(string id)
            {
                return Ok(new { message = id });


            }
        }
    }

}
