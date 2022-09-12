using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyStructureController : ControllerBase
    {
        private CompanyStructureContext _dbContext;
        public CompanyStructureController(CompanyStructureContext dbContext)
        {
            //Установка контеста данных
            _dbContext = dbContext;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyStructure>>> Get()
        {
            //Получам все данные, если запрос не содержал указание родительского отдела
            var queryResult = _dbContext.CompanyStructures.ToArray();
            if (queryResult.Length == 0)
                return NotFound();
            return Ok(queryResult);
        }

        [HttpGet("{parent}")]
        public async Task<ActionResult<IEnumerable<CompanyStructure>>> Get(string parent)
        {
            //Выполняем сортировку по имени родительского отдела
            var queryResult = await _dbContext.CompanyStructures.Where(x => x.ParentDepartment == parent).ToArrayAsync();
            if (queryResult.Length == 0)
                return NotFound();
            return Ok(queryResult);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyStructure>> Post(CompanyStructure newRow)
        { 
            //Проводим проверку входных данных
            if (newRow == null)
                return BadRequest();
            _dbContext.CompanyStructures.Add(newRow);
            await _dbContext.SaveChangesAsync();
            return Ok(newRow);
        }

        [HttpPut]
        public async Task<ActionResult<CompanyStructure>> Put(CompanyStructure modifiedRow)
        { 
            //Проверка входных данных
            if (modifiedRow == null)
                return BadRequest();
            //Если в контесте нет записи с таким айди, вернуть notfound
            if (!_dbContext.CompanyStructures.Any(x => x.Id == modifiedRow.Id))
                return NotFound();
            _dbContext.Update(modifiedRow);
            await _dbContext.SaveChangesAsync();
            return Ok(modifiedRow);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CompanyStructure>> Delete(int id)
        { 
            //Получаем строку для удаления
            CompanyStructure row = await _dbContext.CompanyStructures.FirstOrDefaultAsync(x => x.Id == id);
            if (row == null)
                return NotFound();
            _dbContext.CompanyStructures.Remove(row);
            await _dbContext.SaveChangesAsync();
            return Ok(row);
        }
    }
}
