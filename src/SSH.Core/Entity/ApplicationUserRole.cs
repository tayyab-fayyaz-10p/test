using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using Recipe.Core.Base.Interface;

namespace SSH.Core.Entity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public ApplicationUserRole()
        {
        }

        [Key]
        public int Id { get; set; }
    }
}
