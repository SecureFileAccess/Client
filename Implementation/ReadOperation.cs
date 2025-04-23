using Broker;
using Client.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Implementation
{
    class ReadOperation : FileOperationBase
    {
        public override string ActionName => "Read";

        public override async Task<FileResponse> ExecuteAsync(FileCommand request)
        {
            try
            {
                if (!File.Exists(request.FilePath))
                {
                    return new FileResponse
                    {
                        Success = false,
                        Message = "File not found."
                    };
                }

                var content = await File.ReadAllTextAsync(request.FilePath);
                return new FileResponse
                {
                    Success = true,
                    Message = "Read successful.",
                    Content = Google.Protobuf.ByteString.CopyFromUtf8(content),
                };
            }
            catch (Exception ex)
            {
                return new FileResponse
                {
                    Success = false,
                    Message = $"Read failed: {ex.Message}"
                };
            }
        }
    }
}
