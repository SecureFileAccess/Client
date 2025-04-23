using Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Contracts
{
    public abstract class FileOperationBase : IFileOperation
    {
        public abstract string ActionName { get; }
        public abstract Task<FileResponse> ExecuteAsync(FileCommand request);
    }
}
