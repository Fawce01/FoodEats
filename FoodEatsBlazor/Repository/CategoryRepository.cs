using FoodEatsBlazor.Data;
using FoodEatsBlazor.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace FoodEatsBlazor.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Category> CreateAsync(Category obj)
        {
            await _db.Category.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var obj = await _db.Category.FirstOrDefaultAsync(u => u.Id == id);
            if(obj != null)
            {
                _db.Category.Remove(obj);
                // Return if more than 0 changes were successfully made
                return await _db.SaveChangesAsync() > 0;
            }
            // No categories were deleted
            return false;
        }

        public async Task<Category> GetAsync(int id)
        {
            var obj = await _db.Category.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                // no category with given id was found
                return obj;
            }
            return new Category();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Category.ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category obj)
        {
            var objFromDb = await _db.Category.FirstOrDefaultAsync(u => u.Id == obj.Id);
            if(objFromDb is not null)
            {
                // update
                objFromDb.Name = obj.Name;
                _db.Category.Update(objFromDb);
                await _db.SaveChangesAsync();
                return objFromDb;
            }
            // unsuccessful change - return old obj
            return obj;
        }
    }
}
