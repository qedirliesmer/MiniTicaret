using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Domain.Entities;
public class BaseEntity
{
    public Guid Id { get; set; }
    public Guid? CreateUser { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? UpdateUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
