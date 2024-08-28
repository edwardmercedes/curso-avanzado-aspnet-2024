using _04_Inventory.Api.DTOS;
using _04_Inventory.Api.Infrastruture;
using _04_Inventory.Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetCategtoryByCode/{code}")]

        public async Task<IActionResult> GetCategtoryByCode(int code)
        {
            try
            {
                var item = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == code);

                if (item == null)
                    return NotFound(new { message = $"no exsiste una categoria con el codigo {code}" });


                var category = _mapper.Map<CategoryDTO>(item);

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO category) 
        {
            try
            {
                if (category == null) 
                 return BadRequest(new {message = "la categoria no puede ser nula" });

                var existCode = ExistCategory(category.Code);
                
                if (existCode)
                    return BadRequest("Codigo de categoria ya existe");

                var item = _mapper.Map<CATEGORIA>(category);

                await _context.CATEGORIA.AddAsync(item);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Registro creado exitosamente"});
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDTO category)
        {
            try
            {
                if (category == null)
                    return BadRequest(new { message = "La categoria no puede ser nula" });

                var item = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == category.Code);

                if (item == null)
                    return NotFound(new { message = "No existe una categoria con ese codigo" });

                item.DESCRIPCION = category.Description;
                item.ULT_MODIF_USUARIO = category.UpdateUser;
                item.ULT_MODIF_TSTAMP = DateTime.Now;

                _context.CATEGORIA.Update(item);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteGetCategtory/{code}")]

        public async Task<IActionResult> DeleteGetCategtory(int code)
        {
            try
            {
                var item = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == code);

                if (item == null)
                    return NotFound(new { message = $"no exsiste una categoria con el codigo {code}" });

                 _context.CATEGORIA.Remove(item);
                await _context.SaveChangesAsync();

                return Ok(new { message = "categoria eliminada exitosamente"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private bool ExistCategory(int code)
        {
            var result = _context.CATEGORIA.Any(x => x.CODIGO == code);
            return result;
        }
    }
}
