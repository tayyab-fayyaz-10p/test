using System.ComponentModel.DataAnnotations;
using Recipe.Core.Base.Abstract;

namespace SSH.Core.Entity
{
    public class LOV : EntityBase<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Group { get; set; }

        public bool IsActive { get; set; }

        public int ImageResourceId { get; set; }

        public string Meta { get; set; }
    }
}
