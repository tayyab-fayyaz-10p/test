using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Recipe.Core.Base.Abstract;
using SSH.Core.Constant;
using SSH.Core.Entity;
using SSH.Core.Enum;

namespace SSH.Core.DTO
{
    public class UserRoleDTO
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }

        public ApplicationUserRole ConvertToEntity(ApplicationUserRole entity)
        {
            //entity = base.ConvertToEntity(entity);
            entity.RoleId = this.RoleId;
            entity.UserId = this.UserId;
            
            return entity;
        }

        public void ConvertFromEntity(ApplicationUserRole entity)
        {
            //base.ConvertFromEntity(entity);
            this.RoleId = entity.RoleId;
            this.UserId = entity.UserId;
        }
    }
}