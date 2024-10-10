using _04_Inventory.Api.DTOS;
using _04_Inventory.Api.Infrastruture;
using _04_Inventory.Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace _04_Inventory.Api.Controllers
{
    public class ItemsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemsController> _logger;
        public ItemsController(DBContext context, IMapper mapper, ILogger<ItemsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            _logger.LogError("Prueba de error");
                       
            //var products = _context.ARTICULOS.Include(x=> x.CATEGORIA).ToList();
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


        [HttpGet("GetItemById{codigo}")]
        public async Task<IActionResult> GetItemByCode(int codigo)
        {
            var item = await (from articulo in _context.ARTICULOS
                              join categoria in _context.CATEGORIA
                               on articulo.CATEGORIA equals categoria.CODIGO
                               where articulo.CODIGO == codigo
                                 select new ItemDTO
                                 {
                                      Code = articulo.CODIGO,
                                      Description = articulo.DESCRIPCION,
                                      CategoryId = articulo.CATEGORIA,
                                      CategoryName = categoria.DESCRIPCION
                                 }).FirstOrDefaultAsync();
            if (item == null){
                    return NotFound($"Artículo con código {codigo} no encontrado.");
            }

            return Ok(item);
        }

        [HttpPost("CreateItem")]
        public async Task<IActionResult> CreateItem([FromBody] ItemDTO item)
        {
            if (item is null){
                return BadRequest(new { message = "El artículo es nulo." });
            }

            var articulo = _mapper.Map<ARTICULOS>(item);
            await _context.ARTICULOS.AddAsync(articulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateItem), new { codigo = articulo.CODIGO }, item);  // Retorna la URL del nuevo artículo creado
        }

        [HttpPut("UpdateItem{codigo}")]
        public async Task<IActionResult> UpdateItem(int codigo, [FromBody] ItemDTO updatedItem)
        {
            if (updatedItem == null){
                return BadRequest(new { message = "El artículo es nulo." });
            }

            if (codigo != updatedItem.Code){
                return BadRequest(new { message = "El código del artículo no coincide." });
            }

            var existingItem = await _context.ARTICULOS.FindAsync(codigo);

            if (existingItem == null){
                return NotFound(new { message = "Artículo no encontrado." });
            }

            _mapper.Map(updatedItem, existingItem);

            _context.ARTICULOS.Update(existingItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("DeleteItem/{code}")]
        public async Task<IActionResult> DeleteItem(int code)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

           // MessageeResponseDTO

            string sql = "BEGIN CAPBorrarArticulos (:pArticulo, :pResult); END;";

            OracleParameter pCode = new OracleParameter(":pArticulo", code);
            OracleParameter pResult = new OracleParameter("pResutl", OracleDbType.Varchar2, System.Data.ParameterDirection.InputOutput) {Size = 4000};
           
            await _context.Database.ExecuteSqlRawAsync(sql, pCode, pResult);

            var item = await _context.ARTICULOS.FindAsync(code);

            if (pResult.Value.ToString() == "null")
            {
              // Response
                await transaction.CommitAsync();
            }

             _context.ARTICULOS.Remove(item);
             await _context.SaveChangesAsync();
                // Retorna un 204 No Content para indicar que fue eliminado correctamente
                return NoContent();
        }

    }
}
