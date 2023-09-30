using Microsoft.AspNetCore.Mvc;

namespace WebApp.CommandPattern.Commands
{
    public interface IFileType<T>
    {
        public string FileName { get;}
        public string FileType { get;}
        Task<MemoryStream> Create();

    }

    public interface IPdfFile<T>:IFileType<T>
    {


    }

    public interface IExcelFile<T> : IFileType<T>
    {


    }

}
