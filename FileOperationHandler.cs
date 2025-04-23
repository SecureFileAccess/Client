using Broker;
using Client.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class FileOperationHandler
    {
        private readonly Dictionary<string, FileOperationBase> _commands;

        public FileOperationHandler(IEnumerable<IFileOperation> commands)
        {
            _commands = commands.OfType<FileOperationBase>().ToDictionary(cmd => cmd.ActionName, StringComparer.OrdinalIgnoreCase);
        }

        public async Task<FileResponse> DispatchAsync(FileCommand request)
        {
            if (_commands.TryGetValue(request.Action, out var handler))
            {
                return await handler.ExecuteAsync(request);
            }

            throw new InvalidOperationException("Unsupported command");
        }
    }
}
