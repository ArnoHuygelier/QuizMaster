using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using QuizMaster.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Services.Interfaces
{
    public class CategoryService : ICategoryService
    {
        private readonly QuizMasterDbContext _context;

        public CategoryService(QuizMasterDbContext context)
        {
            _context = context;
        }

        // Alles opvragen
        public async Task<ICollection<Category>> Find()
        {
            return await _context.Categories.Include(c => c.Quizzes).ToListAsync();
        }

        // Eén item opvragen via ID
        public async Task<Category?> Get(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        // Nieuw item aanmaken
        public async Task<Category> Create(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Bestaand item bijwerken
        public async Task<Category?> Update(int id, Category updated)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return null;

            entity.Name = updated.Name;

            await _context.SaveChangesAsync();

            return entity;
        }

        // Verwijderen op ID
        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return false;

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}