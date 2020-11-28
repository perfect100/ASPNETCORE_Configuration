using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Configuration.Models
{
    public class ValidateItem
    {
        [MaxLength(5)]
        public string Name { get; set; }

        [Range(0,100)]
        public int Age { get; set; }
    }
}