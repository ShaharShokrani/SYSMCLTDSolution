using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SYSMCLTD
{
    public partial class Customer
    {
        public Customer()
        {
            Addresses = new HashSet<Address>();
            Contacts = new HashSet<Contact>();
        }
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string CustomerNumber { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
