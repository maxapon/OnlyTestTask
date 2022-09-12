using OfficeOpenXml;
namespace WebApplication1.Models
{
    public class CompanyStructure
    {
        public int Id { get; set; }
        public string? Department { get ; set; }
        public string? ParentDepartment { get; set; }
        public string? JobName { get; set; }
        public string? UserName { get; set; }

        //Словарь, отображающий требуемое название столбцов в excel
        private static Dictionary<string, string> _rusColumnName
            = new Dictionary<string, string> { { "A", "Отдел" }, 
                                               { "B", "Родительский отдел" }, 
                                               { "C", "Должность" }, 
                                               { "D", "Пользователь" } };

        /// <summary>
        /// Проверяет лист excel и генерирует на его основе массив CompanyStructure
        /// </summary>
        /// <param name="ws">Лист excel</param>
        /// <returns>массив CompanyStructure</returns>
        public static CompanyStructure[] MapFromExcel(ExcelWorksheet ws)
        {
            List<CompanyStructure> result = new List<CompanyStructure>();
            if (IsRightExcelFormat(ws))
            {
                int endRowIndex = ws.Dimension.End.Row;
                for (int i = 2; i < endRowIndex; i++)
                {
                    CompanyStructure newRow = new CompanyStructure();
                    newRow.Department = ws.Cells["A" + i].Value?.ToString();
                    newRow.ParentDepartment = ws.Cells["B" + i].Value?.ToString();
                    newRow.JobName = ws.Cells["C" + i].Value?.ToString();
                    newRow.UserName = ws.Cells["D" + i].Value?.ToString();
                    result.Add(newRow);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Проверяет корректность листа excel
        /// </summary>
        /// <param name="ws">Excel лист</param>
        /// <returns>true - если лист в правильном формате, иначе - false</returns>
        private static bool IsRightExcelFormat(ExcelWorksheet ws)
        {
            bool result = true;
            foreach (KeyValuePair<string, string> item in _rusColumnName)
            {
                string colName = ws.Cells[item.Key + "1"].Value.ToString();
                if (colName != item.Value)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

    }
}
