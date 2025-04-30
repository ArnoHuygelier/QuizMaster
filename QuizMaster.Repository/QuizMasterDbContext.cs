using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Repository
{
	public class QuizMasterDbContext(DbContextOptions<QuizMasterDbContext> options) : DbContext(options)
	{

	}
}
