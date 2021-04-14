using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SYSMCLTD
{    
    public class CustomerDTO
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CustomerNumber { get; set; }

        public ICollection<AddressDTO> Addresses { get; set; }
        public ICollection<ContactDTO> Contacts { get; set; }
    }
}
