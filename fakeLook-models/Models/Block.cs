using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fakeLook_models.Models
{
    public class Block
    {
        public int Id { get; set; }
        [Key]
        public int BlockerUserId { get; set; }
        public int BlockedUserId { get; set; }

    }
}
