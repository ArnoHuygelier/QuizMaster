using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaster.Models;

namespace QuizMaster.Ui.Mvc.ViewModels.Categories
{
    public class CategoriesViewModel
    {
        public int Id { get; set; }
        public List<Category> Categories { get; set; } 
        public required string Name { get; set; }

    }

}
