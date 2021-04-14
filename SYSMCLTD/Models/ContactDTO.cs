using System;
using System.ComponentModel.DataAnnotations;

namespace SYSMCLTD
{
    public class ContactDTO
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string FullName { get; set; }
        public string OfficeNumber { get; set; }
        public string Email { get; set; }
    }
}
