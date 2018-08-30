using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VersionManagement.Dtos;
using VersionManagement.Models;

namespace VersionManagement.BusinessLogics
{
    public interface IVersionLogic
    {
        VersionInfo GetversionById(Guid id);

        ICollection<VersionInfo> GetPagedVersionList(Department department, VersionStatus status, out long totalCount, int pageNumber, int pageSize);

        Task<VersionInfo> UpdateVersionAsync(VersionInfo version);

        void DeleteVersion(Guid versionId);

        VersionInfo SubmitVersion(Guid versionId, string releaseNote);

        ICollection<VersionDetail> GetVersionDetails(Guid versionId, out long totalCount, int pageIndex, int pageSize, string applicant);

        VersionDetail GetVersionDetailById(Guid detailId);

        VersionDetail UpdateVersionDetail(VersionDetailDto detail);

        void DeleteVersionDetail(Guid detailId);
    }
}
