using System.Collections.Generic;

namespace VersionManagement.Dtos
{
    public class DetailsDto
    {
        public long TotalCount { get; set; }

        public ICollection<VersionDetailDto> Details { get; set; }
    }
}
