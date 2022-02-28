using Placeholder.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Integration
{
    public interface IUploadFiles
    {
        UploadedFile UploadFile(Guid shop_id, byte[] bytes, string filePathAndName);
    }
}
