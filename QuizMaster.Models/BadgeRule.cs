using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class BadgeRule
    {
        public string BadgeName { get; set; }
        public string Type { get; set; }  
        public int Threshold { get; set; }
    }
}
