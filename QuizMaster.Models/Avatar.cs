using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class Avatar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }


        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
