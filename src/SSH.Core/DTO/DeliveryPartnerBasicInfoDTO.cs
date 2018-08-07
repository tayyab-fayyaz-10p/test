using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe.Core.Base.Abstract;
using SSH.Core.Entity;
using SSH.Core.Enum;

namespace SSH.Core.DTO
{
    public class DeliveryPartnerBasicInfoDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string EmailId { get; set; }

        public string PhoneNumber { get; set; }

        public DeliveryPartnerCategories DeliveryPartnerCategoryId { get; set; }

        public string DeliveryPartnerCategoryName { get; set; }

        public string ProfilePicture { get; set; }
    }
}
