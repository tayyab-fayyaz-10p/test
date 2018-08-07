using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
    public class FileDTO
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string SourceFileName { get; set; }

        public string ReferenceType { get; set; }

        public string Data { get; set; }

        public DateTimeOffset? LastModified { get; set; }
    }
}
