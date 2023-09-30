using System.IO.Compression;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.CommandPattern.Commands;
using WebApp.CommandPattern.Models;

namespace WebApp.CommandPattern.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public ProductController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.Products.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> CreateFile(int type)
        {
            var products = await _context.Products.ToListAsync();
            FileCreateInvoker fileCreateInvoker = new();

            EFileType fileType = (EFileType)type;

            switch (fileType)
            {
                case EFileType.Excel:
                    ExcelFile<Product> excelFile = new ExcelFile<Product>(products);
                    fileCreateInvoker.SetCommad(new CreateExcelTableActionCommand<Product>(excelFile));
                    break;
                case EFileType.Pdf:
                    PdfFile<Product> pdfFile = new PdfFile<Product>(products, HttpContext);
                    fileCreateInvoker.SetCommad(new CreatePdfTableActionCommand<Product>(pdfFile));
                    break;
                default:
                    break;
            }

            return await fileCreateInvoker.CreateFile();
        }

        [HttpGet]
        public async Task<IActionResult> CreateFiles(int type)
        {
            var products = await _context.Products.ToListAsync();
            FileCreateInvoker fileCreateInvoker = new();
            ExcelFile<Product> excelFile = new ExcelFile<Product>(products);
            PdfFile<Product> pdfFile = new PdfFile<Product>(products, HttpContext);
            fileCreateInvoker.AddCommand(new CreateExcelTableActionCommand<Product>(excelFile));
            fileCreateInvoker.AddCommand(new CreatePdfTableActionCommand<Product>(pdfFile));


            var fileResult = fileCreateInvoker.CreateFiles();
            
            using (var zipMemoStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipMemoStream, ZipArchiveMode.Create))
                {
                    foreach (var item in fileResult)
                    {
                        var fileContent = item.Result as FileContentResult;
                        var zipFile = archive.CreateEntry(fileContent.FileDownloadName);
                        using (var zipEntryStream=zipFile.Open())
                        {
                            await new MemoryStream(fileContent.FileContents).CopyToAsync(zipEntryStream);
                        }
                    }
                }

                return File(zipMemoStream.ToArray(), "application/zip", "all.zip");
            }
        }
    }
}
