using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Domain.Entities;

public class Announcement : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime DateDue { get; set; }
}
