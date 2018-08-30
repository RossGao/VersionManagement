using System.Collections.Generic;
using VersionManagement.Models;

namespace VersionManagement.Dtos
{
    public class DetailsDto
    {
        public long TotalCount { get; set; }

        public ICollection<VersionDetailDto> Details { get; set; }
    }
}
