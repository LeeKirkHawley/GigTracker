using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Data;
using GigTracker.Models;

namespace GigTracker.Data {
	public class UserRepository : IUserRepository{

        ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }
        IQueryable<User> Users { get; }

        public async Task<List<User>> Get() {
            var t = await Task.Run(() => _context.Set<User>().ToList() );
            return t;
        }

        public async Task<User> Get(int id) {
            return await _context.Set<User>().FindAsync(id);
        }
    }
}
