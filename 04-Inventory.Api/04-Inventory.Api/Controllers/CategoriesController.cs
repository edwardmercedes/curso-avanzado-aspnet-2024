using _04_Inventory.Api.DTOS;
using _04_Inventory.Api.Infrastruture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace _04_Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
            
        [HttpGet("GetAllCategtories")]
        public async Task<IActionResult> GetAllCategtories()
        {
            try 
            {
                var items = await _context.CATEGORIA.ToListAsync();

                var categories = _mapper.Map<List<CategoryDTO>>(items); 
                //convirtiendo datos con linq
                //var categories = (from item in items
                //                 select new CategoryDTO
                //                 {
                //                     Code = item.CODIGO,
                //                     Description = item.DESCRIPCION
                //                 }).ToList();

                return Ok(categories);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
