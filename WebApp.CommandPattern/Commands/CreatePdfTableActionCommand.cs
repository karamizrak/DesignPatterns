using Microsoft.AspNetCore.Mvc;

namespace WebApp.CommandPattern.Commands;

public class CreatePdfTableActionCommand<T> : ITableActionCommand
{
    private readonly IPdfFile<T> _pdfFile;

    public CreatePdfTableActionCommand(PdfFile<T> pdfFile)
    {
        _pdfFile = pdfFile;
    }

    public async Task<IActionResult> ExecuteAsync()
    {
        var excelMemoryStream = await _pdfFile.Create();
        return new FileContentResult(excelMemoryStream.ToArray(), _pdfFile.FileType)
            { FileDownloadName = _pdfFile.FileName };
    }
}