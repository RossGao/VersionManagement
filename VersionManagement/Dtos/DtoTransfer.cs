using System.Collections.Generic;
using VersionManagement.Models;

namespace VersionManagement.Dtos
{
    public static class DtoTransfer
    {
        public static VersionInfoDto ConvertToVersionsDto(ICollection<VersionInfo> versions, long count)
        {
            return new VersionInfoDto()
            {
                VersionList = versions,
                TotalCount = count,
            };
        }

        public static VersionDto ConvertToVersionDto(VersionInfo version)
        {
            if (version != null)
            {
                return new VersionDto()
                {
                    Id = version.Id,
                    Creator = version.Creator,
                    Department = version.Department.ToString(),
                    ReleaseDate = version.ReleaseDate,
                    ReleaseNote = version.ReleaseNote,
                    Status = version.Status.ToString(),
                    VersionNumber = version.VersionNumber,
                    VersionTitle = version.VersionTitle
                };
            }

            return null;
        }

        public static DetailsDto ConvertToDetailsDto(ICollection<VersionDetail> details, long count)
        {
            var detailDtos = new List<VersionDetailDto>();

            foreach (var detail in details)
            {
                detailDtos.Add(ConvertToDetailDto(detail));
            }

            return new DetailsDto()
            {
                Details = detailDtos,
                TotalCount = count
            };
        }

        public static VersionDetailDto ConvertToDetailDto(VersionDetail detail)
        {
            if (detail != null)
            {
                return new VersionDetailDto()
                {
                    Id = detail.Id,
                    Applicant = detail.Applicant,
                    CommitIds = detail.CommitIds,
                    DetailNote = detail.DetailNote,
                    Iteration = detail.Iteration,
                    TaskTitle = detail.TaskTitle,
                    Type = detail.Type,
                    VersionId = detail.Version.Id
                };
            }

            return null;
        }
    }
}
