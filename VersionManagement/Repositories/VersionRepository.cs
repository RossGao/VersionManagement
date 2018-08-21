using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VersionManagement.Models;

namespace VersionManagement.Repositories
{
    public class VersionRepository : IVersionRepository
    {
        private readonly VersionContext dbContext;

        public VersionRepository(VersionContext theDbCotext)
        {
            dbContext = theDbCotext;
        }

        public VersionInfo DeleteVersion(Guid versionId)
        {
            if (versionId != Guid.Empty)
            {
                var version = dbContext.Versions.Find(versionId);

                if (version != null)
                {
                    dbContext.Versions.Remove(version);
                    dbContext.SaveChanges();
                    return version;
                }
            }

            return null;
        }

        public VersionInfo GetVersionById(Guid id)
        {
            if (id != Guid.Empty)
            {
                return dbContext.Versions.Find(id);
            }

            return null;
        }

        public long GetVersionsCount(ICollection<Department> departments, ICollection<VersionStatus> status)
        {
            return dbContext.Versions.Where(v => departments.Contains(v.Department) && status.Contains(v.Status)).Count();
        }

        /// <summary>
        /// Get version list by department and audit status.
        /// The lastest unaudited items are ordered first.
        /// </summary>
        /// <param name="departments">Fairhr product departments.</param>
        /// <param name="status">Unaudited or audited.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">How many rows in one page.</param>
        /// <returns>List of version infos.</returns>
        public ICollection<VersionInfo> GetPagedVersions(ICollection<Department> departments, ICollection<VersionStatus> status, int pageNumber, int pageSize)
        {
            return dbContext.Versions.Where(v => departments.Contains(v.Department) && status.Contains(v.Status))
                .OrderByDescending(v => v.ReleaseDate).ThenBy(v => v.Status).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public Task<VersionInfo> UpdateVersionAsync(VersionInfo version)
        {
            return Task.Run(() =>
           {
               if (version != null)
               {
                   if (dbContext.Versions.Find(version.Id) == null)
                   {
                       if (version.Id == Guid.Empty)
                       {
                           version.Id = Guid.NewGuid();
                       }

                       version.Status = VersionStatus.unaudited;
                       dbContext.Versions.Add(version);
                   }
                   else
                   {
                       dbContext.Versions.Attach(version);
                   }

                   dbContext.SaveChanges();
               }

               return version;
           });
        }

        public ICollection<VersionDetail> GetVersionDetails(Guid versionId, int pageIndex = 1, int pageSize = 20, string applicant = "")
        {
            var details = dbContext.Versions.Find(versionId).Detailes;

            if (!string.IsNullOrWhiteSpace(applicant))
            {
                details = details.Where(d => string.Equals(d.Applicant, applicant, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return details.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public VersionDetail GetVersionDetailById(Guid versionId, Guid detailId)
        {
            if (versionId != Guid.Empty && detailId != Guid.Empty)
            {
                return dbContext.Versions.Find(versionId).Detailes.FirstOrDefault(v => v.Id == detailId);
            }

            return null;
        }

        public VersionDetail UpdateVersionDetail(Guid versionId, VersionDetail detail)
        {
            if (versionId != Guid.Empty && detail != null)
            {
                var version = dbContext.Versions.Find(versionId);
                List<VersionDetail> detailList;

                if (version != null)
                {
                    detailList = version.Detailes.ToList();

                    if (detail.Id == Guid.Empty)
                    {
                        detail.Id = Guid.NewGuid();
                        detailList.Add(detail);
                    }
                    else
                    {
                        var index = detailList.IndexOf(detail);

                        if (index != -1)
                        {
                            detailList[index] = detail;
                        }
                        else
                        {
                            detailList.Add(detail);
                        }
                    }

                    version.Detailes = detailList;
                    dbContext.SaveChanges();
                    return detail;
                }
            }

            return null;
        }

        public long GetDetailsCount(Guid versionId)
        {
            return dbContext.Versions.Find(versionId).Detailes.Count;
        }

        public VersionDetail DeleteVersionDetail(Guid versionId, Guid detailId)
        {
            if (versionId != Guid.Empty && detailId != Guid.Empty)
            {
                var details = dbContext.Versions.Find(versionId).Detailes;
                var detail = details.FirstOrDefault(d => d.Id == detailId);
                details.Remove(detail);
                dbContext.SaveChanges();
                return detail;
            }

            return null;
        }

        public VersionInfo SubmitVersion(Guid versionId)
        {
            if (versionId != Guid.Empty)
            {
                var version = dbContext.Versions.Find(versionId);

                if (version != null)
                {
                    version.Status = VersionStatus.audited;
                    dbContext.SaveChanges();
                    return version;
                }
            }

            return null;
        }
    }
}
