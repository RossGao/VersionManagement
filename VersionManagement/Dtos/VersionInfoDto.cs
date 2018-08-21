using System.Collections.Generic;
using VersionManagement.Models;

namespace VersionManagement.Dtos
{
    public class VersionInfoDto
    {
        public ICollection<VersionInfo> VersionList { get; set; }

        public long TotalCount { get; set; }
    }
}
