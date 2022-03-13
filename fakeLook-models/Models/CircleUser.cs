using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fakeLook_models.Models
{
    public class CircleUser
    {
        [Key]
        public int Id { get; set; }
        public int CircleId { get; set; }
        public int UserId { get; set; }
    }
}
