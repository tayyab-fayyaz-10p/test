using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSH.Core.Enum;

namespace SSH.Core.DTO
{
    public class ShipmentFeedbackRatingDTO
    {
        public int Id { get; set; }

        public string AWB { get; set; }

        public Rating FBRatings { get; set; }
    }
}
