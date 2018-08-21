using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VersionManagement.Models;
using VersionManagement.Repositories;

namespace VersionManagement.BusinessLogics
{
    public class VersionLogic : IVersionLogic
    {
        private IVersionRepository repo;

        public VersionLogic(IVersionRepository theRepo)
        {
            repo = theRepo;
        }

        public VersionInfo DeleteVersion(Guid versionId)
        {
            if (versionId != Guid.Empty)
            {
                return repo.DeleteVersion(versionId);
            }

            return null;
        }

        public VersionInfo GetversionById(Guid id)
        {
            if (id != Guid.Empty)
            {
                return repo.GetVersionById(id);
            }

            return null;
        }

        public ICollection<VersionInfo> GetPagedVersionList(Department department, VersionStatus status, out long totalCount, int pageNumber = 1, int pageSize = 20)
        {
            ICollection<Department> departments = new List<Department>();

            if (department != Department.All)
            {
                departments.Add(department);
            }
            else
            {
                departments = new List<Department>() { Department.BPO, Department.ESB, Department.FanYou, Department.FYU, Department.HaoTian, Department.SDB };
            }

            ICollection<VersionStatus> statusList = new List<VersionStatus>();

            if (status != VersionStatus.Undefined)
            {
                statusList.Add(status);
            }
            else
            {
                statusList = new List<VersionStatus>() { VersionStatus.unaudited, VersionStatus.audited };
            }

            totalCount = repo.GetVersionsCount(departments, statusList);
            return repo.GetPagedVersions(departments, statusList, pageNumber, pageSize);
        }

        public async Task<VersionInfo> UpdateVersionAsync(VersionInfo version)
        {
            if (version != null)
            {
                return await repo.UpdateVersionAsync(version);
            }

            return null;
        }

        public ICollection<VersionDetail> GetVersionDetails(Guid versionId, out long totalCount, int pageIndex = 1, int pageSize = 20, string applicant = "")
        {
            totalCount = 0;

            if (versionId != Guid.Empty)
            {
                totalCount = repo.GetDetailsCount(versionId);
                return repo.GetVersionDetails(versionId, pageIndex, pageSize, applicant);
            }

            return null;
        }

        public VersionDetail UpdateVersionDetail(Guid versionId, VersionDetail detail)
        {
            if (versionId != Guid.Empty && detail != null)
            {
                return repo.UpdateVersionDetail(versionId, detail);
            }

            return null;
        }

        public VersionDetail GetVersionDetailById(Guid versionId, Guid detailId)
        {
            if (versionId != Guid.Empty && detailId != Guid.Empty)
            {
                return repo.GetVersionDetailById(versionId, detailId);
            }

            return null;
        }

        public VersionDetail DeleteVersionDetail(Guid versionId, Guid detailId)
        {
            if (versionId != Guid.Empty && detailId != Guid.Empty)
            {
                return repo.DeleteVersionDetail(versionId, detailId);
            }

            return null;
        }

        public VersionInfo SubmitVersion(Guid versionId)
        {
            if (versionId != Guid.Empty)
            {
                return repo.SubmitVersion(versionId);
            }

            return null;
        }
    }
}
