using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Table(nameof(Avatar))]
    public class Avatar
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AvatarUrl { get; set; }


        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
