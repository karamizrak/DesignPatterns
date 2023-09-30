using Microsoft.AspNetCore.Mvc;

namespace WebApp.CommandPattern.Commands
{
    public interface ITableActionCommand
    {
        Task<IActionResult> ExecuteAsync();
    }
}