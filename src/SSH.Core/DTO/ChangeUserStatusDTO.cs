﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
   public class ChangeUserStatusDTO
    {
       public string Id { get; set; }

       public int Status { get; set; }

       public string Reason { get; set; }
    }
}
