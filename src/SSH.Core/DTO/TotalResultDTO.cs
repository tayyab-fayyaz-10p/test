using System.Collections.Generic;

namespace SSH.Core.DTO
{
    public class TotalResultDTO<TList>
    {
        public IList<TList> Result { get; set; }

        public int TotalRecords { get; set; }
    }
}
