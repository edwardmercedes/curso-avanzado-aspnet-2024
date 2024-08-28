using _04_Inventory.Api.DTOS;
using _04_Inventory.Api.Infrastruture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _04_Inventory.Api.Controllers
{
    public class ItemsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public ItemsController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var categories = await (from item in _context.ARTICULOS
                                 join category in _context.CATEGORIA
                                 on item.CATEGORIA equals category.CODIGO
                                 select new ItemDTO 
                                 {
                                     Code = item.CODIGO,
                                     Description = item.DESCRIPCION,
                                     CategoryId = item.CATEGORIA,
                                     CategoryName = category.DESCRIPCION
                                 }).ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
