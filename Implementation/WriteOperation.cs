using Broker;
using Client.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Client.Implementation
{
    class WriteOperation : FileOperationBase
    {
        public override string ActionName => "Write";

        public override async Task<FileResponse> ExecuteAsync(FileCommand request)
        {
            try
            {
                var contentString = request.Content.ToStringUtf8();

                await File.WriteAllTextAsync(request.FilePath, contentString);

                return new FileResponse
                {
                    Success = true,
                    Message = "Write successful.",
                    Content = Google.Protobuf.ByteString.CopyFromUtf8(contentString),
                };
            }
            catch (Exception ex)
            {
                return new FileResponse
                {
                    Success = false,
                    Message = $"Write failed: {ex.Message}"
                };
            }
        }
    }
}
