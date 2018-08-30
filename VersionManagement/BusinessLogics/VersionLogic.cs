using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VersionManagement.Dtos;
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

        public void DeleteVersion(Guid versionId)
        {
            if (versionId != Guid.Empty)
            {
                repo.DeleteVersion(versionId);
            }
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

            totalCount = repo.GetDetailsCount(versionId);
            return repo.GetVersionDetails(versionId, pageIndex, pageSize, applicant);
        }

        public VersionDetail UpdateVersionDetail(VersionDetailDto detail)
        {
            if (detail != null)
            {
                return repo.UpdateVersionDetail(new VersionDetail()
                {
                    Id = detail.Id,
                    Applicant = detail.Applicant,
                    CommitIds = detail.CommitIds,
                    DetailNote = detail.DetailNote,
                    Iteration = detail.Iteration,
                    TaskTitle = detail.TaskTitle,
                    Type = detail.Type,
                    Version = new VersionInfo() { Id = detail.VersionId }
                });
            }

            return null;
        }

        public VersionDetail GetVersionDetailById(Guid detailId)
        {
            if (detailId != Guid.Empty)
            {
                return repo.GetVersionDetailById(detailId);
            }

            return null;
        }

        public void DeleteVersionDetail(Guid detailId)
        {
            if (detailId != Guid.Empty)
            {
                repo.DeleteVersionDetail(detailId);
            }
        }

        public VersionInfo SubmitVersion(Guid versionId, string releaseNote)
        {
            if (versionId != Guid.Empty)
            {
                return repo.SubmitVersion(versionId, releaseNote);
            }

            return null;
        }
    }
}
