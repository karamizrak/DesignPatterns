using Microsoft.AspNetCore.Mvc;

namespace WebApp.CommandPattern.Commands
{
    public class FileCreateInvoker
    {
        private ITableActionCommand _tableActionCommand = null!;

        private readonly List<ITableActionCommand>  _tableActionCommands= new();

        public void SetCommad(ITableActionCommand actionCommand)
        {
            _tableActionCommand = actionCommand;


        }

        public void AddCommand(ITableActionCommand actionCommand)
        {
            _tableActionCommands.Add(actionCommand);
        }


        public Task<IActionResult> CreateFile()
        {
            return _tableActionCommand.ExecuteAsync();
        }

        public List<Task<IActionResult>> CreateFiles()
        {
            
            var ss=_tableActionCommands.Select(x=>x.ExecuteAsync()).ToList();
            return ss;
        }
    }
}
