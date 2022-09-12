using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyStructureTotalController : ControllerBase
    {
        private CompanyStructureContext _dbContext;
        public CompanyStructureTotalController(CompanyStructureContext dbContext)
        { 
            //Добавялем контекст данных
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<CompanyStructureTotal[]>> Get()
        {
            //Преобразуем данные из базы в локальный массив
            var data = await _dbContext.CompanyStructures.ToArrayAsync();
            //Производим группировку по отделу
            var departments = data.GroupBy(x => x.Department);
            var result = new CompanyStructureTotal[departments.Count()];
            int i = 0;
            foreach (var item in departments)
            {
                //Получаем количество уникальных пользователей в группе
                int userTot = item.Select(m => new { m.Department, m.UserName}).Distinct().Count();
                //Получаем количество уникальных типов работ в группе
                int jobTot = item.Select(m => new { m.Department, m.JobName }).Distinct().Count();
                var newRow = new CompanyStructureTotal() { Department = item.Key, UsersTotal = userTot, JobsTotal = jobTot };
                //Заполняем массив
                result[i] = newRow;
                i++;
            }

            return Ok(result);
        }
    }
}
