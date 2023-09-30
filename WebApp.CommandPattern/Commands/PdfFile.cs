using DinkToPdf;
using System.Text;
using DinkToPdf.Contracts;

namespace WebApp.CommandPattern.Commands
{
    public class PdfFile<T>:IPdfFile<T>
    {
        public readonly List<T> _list;
        private readonly HttpContext _context;
        public string FileName => $"{typeof(T).Name}.pdf";
        
        public string FileType => "application/octet-stream";

        public PdfFile(List<T> list, HttpContext context)
        {
            _list = list;
            _context = context;
        }

        public Task<MemoryStream> Create()
        {
            var type = typeof(T);
            var sb = new StringBuilder();
            sb.AppendLine($@"<html>
                                <head></head>
                                <body>
                                    <div class='text-center'> <h1>{type.Name} Tablo</h1></div>
                                        <table class='table table-striped' align='center'>
                            ");
            sb.Append("<tr>");
            type.GetProperties().ToList().ForEach(x =>
            {
                sb.Append($@"<th>{x.Name}</th>");
            });
            sb.Append("</tr>");

            _list.ForEach(x =>
            {
                var values = type.GetProperties().Select(s => s.GetValue(x, null)).ToList();
                sb.Append("<tr>");
                values.ForEach(x =>
                {
                    sb.Append($"<td>{x}</td>");
                });
                sb.Append("</tr>");
            });
            sb.Append("</table></body></html>");

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = { ColorMode = ColorMode.Color, Orientation = Orientation.Portrait, PaperSize = PaperKind.A4 },
                Objects = { new ObjectSettings()
                                {
                                    PagesCount = true,
                                    HtmlContent = sb.ToString(),
                                    WebSettings = {DefaultEncoding = "utf-8",UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/lib/bootstrap/dist/css/bootstrap.css") },
                                    HeaderSettings = {FontSize = 9,Right = "[toPage] nın [page]. sayfası",Line = true,Spacing = 2.812}
                                }
                }

            };

            var converter = _context.RequestServices.GetService<IConverter>();
            MemoryStream pdfMemory = new(converter.Convert(doc));

            return Task.FromResult(pdfMemory);
            

        }
    }
}
