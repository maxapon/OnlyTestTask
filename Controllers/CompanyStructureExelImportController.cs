using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using OfficeOpenXml;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyStructureExelImportController : ControllerBase
    {
        private CompanyStructureContext _dbContext;
        public CompanyStructureExelImportController(CompanyStructureContext dbContext)
        {
            //Установка контекста данных
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(byte[] data, int workSheetIndex = 0)
        {
            //Проверяем входные данные
            if (data == null || data.Length == 0)
                return BadRequest("Incorrect input data");
            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    //Получаем из MemoryStream excel
                    ExcelPackage excel = new ExcelPackage(ms);
                    //Получаем из excel нужный лист
                    ExcelWorksheet ws = excel.Workbook.Worksheets[workSheetIndex];
                    //Мапим данные из листа и добавляем результат в базу
                    _dbContext.CompanyStructures.AddRange(CompanyStructure.MapFromExcel(ws));
                    await _dbContext.SaveChangesAsync();
                    return Ok("ok");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
