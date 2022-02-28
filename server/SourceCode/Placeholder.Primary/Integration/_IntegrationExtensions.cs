using Placeholder.Primary.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary
{
    public static class _IntegrationExtensions
    {
        public static string GenerateFilePathForTestReport(this IUploadFiles uploadFiles, Guid shop_id, Guid test_report_id, string fileName)
        {
            if (uploadFiles == null)
            {
                return null;
            }
            string fakeFolder = Guid.NewGuid().ToString("N");
            return $"files/{shop_id}/testreports/{test_report_id}/{fakeFolder}/{fileName}";
        }
    }
}
