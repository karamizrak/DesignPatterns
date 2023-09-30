using Microsoft.AspNetCore.Mvc;

namespace WebApp.CommandPattern.Commands
{
    public class CreateExcelTableActionCommand<T>: ITableActionCommand
    {
        private readonly ExcelFile<T> _excelFile;

        public CreateExcelTableActionCommand(ExcelFile<T> excelFile)
        {
            _excelFile = excelFile;
        }

        public async Task<IActionResult> ExecuteAsync()
        {
            var excelMemoryStream = await _excelFile.Create();
            return new FileContentResult(excelMemoryStream.ToArray(), _excelFile.FileType)
                { FileDownloadName = _excelFile.FileName };


        }
    }
}
