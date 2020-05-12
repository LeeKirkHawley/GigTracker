using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Models;
using GigTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace GigTracker.Repositories {
	public class UserRepository : IUserRepository{

        ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<User>> Get() {
            return _context.User;
            //return await Task.Run(() => _context.Set<User>().   .ToList());
        }

        public async Task<User> Get(int id) {
            return await _context.Set<User>().FindAsync(id);
        }

        public User GetNoTracking(int id) {

            return _context.User
                .AsNoTracking()
                .Where(u => u.Id == id)
                .FirstOrDefault();
        }

        public async Task<User> Add(User gig) {
            _context.Set<User>().Add(gig);
            await _context.SaveChangesAsync();
            return gig;
        }

        public async Task<User> Delete(int id) {
            var gig = await _context.Set<User>().FindAsync(id);
            if (gig == null) {
                return gig;
            }

            _context.Set<User>().Remove(gig);
            await _context.SaveChangesAsync();

            return gig;
        }

        public async Task<User> Update(User gig) {
            _context.Entry(gig).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return gig;
        }
    }
}
