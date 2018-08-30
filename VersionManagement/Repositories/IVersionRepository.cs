using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VersionManagement.Models;

namespace VersionManagement.Repositories
{
    public interface IVersionRepository
    {
        VersionInfo GetVersionById(Guid id);

        ICollection<VersionInfo> GetPagedVersions(ICollection<Department> departments, ICollection<VersionStatus> status, int pageNumber, int pageSize);

        Task<VersionInfo> UpdateVersionAsync(VersionInfo version);

        void DeleteVersion(Guid versionId);

        VersionInfo SubmitVersion(Guid versionId, string releaseNote);

        long GetVersionsCount(ICollection<Department> departments, ICollection<VersionStatus> status);

        ICollection<VersionDetail> GetVersionDetails(Guid versionId, int pageIndex, int pageSize, string applicant);

        VersionDetail UpdateVersionDetail(VersionDetail detail);

        VersionDetail GetVersionDetailById(Guid detailId);

        long GetDetailsCount(Guid versionId);

        void DeleteVersionDetail(Guid detailId);
    }
}
