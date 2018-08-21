using System.Collections.Generic;
using VersionManagement.Models;

namespace VersionManagement.Dtos
{
    public static class DtoTransfer
    {
        public static VersionInfoDto ConvertToVersionDto(ICollection<VersionInfo> versions, long count)
        {
            return new VersionInfoDto()
            {
                VersionList = versions,
                TotalCount = count,
            };
        }

        public static DetailsDto ConvertToDetailsDto(ICollection<VersionDetail> details, long count)
        {
            return new DetailsDto()
            {
                Details = details,
                TotalCount = count
            };
        }
    }
}
