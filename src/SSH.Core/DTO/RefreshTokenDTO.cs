using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
    public class RefreshTokenDTO
    {
        [Required]
        public string Refresh_token { get; set; }
    }
}
