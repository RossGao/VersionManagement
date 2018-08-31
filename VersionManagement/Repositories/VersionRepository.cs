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

        public void DeleteVersion(Guid versionId)
        {
            if (versionId != Guid.Empty)
            {
                //var version = dbContext.Versions.Find(versionId);

                //if (version != null)
                //{
                //    dbContext.Versions.Remove(version);
                //    dbContext.SaveChanges();
                //    return version;
                //}

                var version = new VersionInfo() { Id = versionId };
                dbContext.Versions.Attach(version);
                dbContext.Remove(version);
                dbContext.SaveChanges();
            }
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
            return dbContext.Versions
                .Where(v => departments.Contains(v.Department) && status.Contains(v.Status))
                .OrderByDescending(o => o.ReleaseDate)
                .OrderByDescending(o => o.Status)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public Task<VersionInfo> UpdateVersionAsync(VersionInfo version)
        {
            return Task.Run(() =>
           {
               if (version != null)
               {
                   if (version.Id == Guid.Empty)
                   {
                       version.Id = Guid.NewGuid();
                       version.Status = VersionStatus.unaudited;
                       dbContext.Versions.Add(version);
                   }
                   else
                   {
                       if (dbContext.Versions.Any(v => v.Id == version.Id))
                       {
                           dbContext.Entry(version).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                           dbContext.Entry(version).Property(v => v.Status).IsModified = false; // version status cannot be update in this api.
                       }
                       else
                       {
                           return null;
                       }
                   }

                   dbContext.SaveChanges();
               }

               return version;
           });
        }

        public ICollection<VersionDetail> GetVersionDetails(Guid versionId, int pageIndex = 1, int pageSize = 20, string applicant = "")
        {
            IQueryable<VersionDetail> q = dbContext.Details;

            if (versionId != Guid.Empty)
            {
                q = q.Where(d => d.Version.Id == versionId);
            }

            if (!string.IsNullOrWhiteSpace(applicant))
            {
                q = q.Where(d => string.Equals(d.Applicant, applicant, StringComparison.OrdinalIgnoreCase));
            }

            return q.OrderBy(d => d.Applicant).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public VersionDetail GetVersionDetailById(Guid detailId)
        {
            if (detailId != Guid.Empty)
            {
                //return dbContext.Versions.Find(versionId).Detailes.FirstOrDefault(v => v.Id == detailId);
                return dbContext.Details.Find(detailId);
            }

            return null;
        }

        public VersionDetail UpdateVersionDetail(VersionDetail detail)
        {
            if (detail != null)
            {
                dbContext.Versions.Attach(detail.Version);

                if (detail.Id == Guid.Empty)
                {
                    detail.Id = Guid.NewGuid();
                    dbContext.Details.Add(detail);
                }
                else
                {
                    if (dbContext.Details.Any(d => d.Id == detail.Id))
                    {
                        dbContext.Entry(detail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    else
                    {
                        return null;
                    }
                }

                dbContext.SaveChanges();
                return detail;
            }
            return null;
        }

        public void DeleteVersionDetail(Guid detailId)
        {
            if (detailId != Guid.Empty)
            {
                var detail = new VersionDetail() { Id = detailId };
                dbContext.Details.Attach(detail);
                dbContext.Details.Remove(detail);
                dbContext.SaveChanges();
            }
        }

        public long GetDetailsCount(Guid versionId)
        {
            if (versionId == Guid.Empty)
            {
                return dbContext.Details.Count();
            }

            return dbContext.Details.Where(d => d.Version.Id == versionId).Count();
        }

        public VersionInfo SubmitVersion(Guid versionId, string releaseNote)
        {
            if (versionId != Guid.Empty)
            {
                var version = dbContext.Versions.Find(versionId);

                if (version != null)
                {
                    version.Status = VersionStatus.audited;
                    version.ReleaseNote = releaseNote;
                    dbContext.SaveChanges();
                    return version;
                }
            }

            return null;
        }
    }
}
