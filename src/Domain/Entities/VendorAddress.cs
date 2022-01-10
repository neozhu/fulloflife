using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Domain.Entities;

public class VendorAddress:AuditableEntity
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string Contact { get; set; }
    public string PhoneNumber { get; set; }
}
