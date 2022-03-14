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
        [Key]
        public int Id { get; set; }
        public int BlockerUserId { get; set; }
        public int BlockedUserId { get; set; }

    }
}
