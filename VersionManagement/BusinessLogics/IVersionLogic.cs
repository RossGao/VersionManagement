using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VersionManagement.Models;

namespace VersionManagement.BusinessLogics
{
    public interface IVersionLogic
    {
        VersionInfo GetversionById(Guid id);

        ICollection<VersionInfo> GetPagedVersionList(Department department, VersionStatus status, out long totalCount, int pageNumber, int pageSize);

        Task<VersionInfo> UpdateVersionAsync(VersionInfo version);

        VersionInfo DeleteVersion(Guid versionId);

        VersionInfo SubmitVersion(Guid versionId);

        ICollection<VersionDetail> GetVersionDetails(Guid versionId, out long totalCount, int pageIndex, int pageSize, string applicant);

        VersionDetail GetVersionDetailById(Guid versionId, Guid detailId);

        VersionDetail UpdateVersionDetail(Guid versionId, VersionDetail detail);

        VersionDetail DeleteVersionDetail(Guid versionId, Guid detailId);
    }
}
