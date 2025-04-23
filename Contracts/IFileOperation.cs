using Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Contracts
{
    public interface IFileOperation
    {
        Task<FileResponse> ExecuteAsync(FileCommand request);
    }
}
