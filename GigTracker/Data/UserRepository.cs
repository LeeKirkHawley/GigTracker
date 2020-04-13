using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Data;
using GigTracker.Models;
using GigTracker.Entities;

namespace GigTracker.Data {
	public class UserRepository : IUserRepository{

        ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }
//        IQueryable<User> Users { get; }

        public IEnumerable<GigTrackerUser> Get() {
            //var t = await Task.Run(() => _context.User);
            return _context.User;
        }

        public async Task<GigTrackerUser> Get(int id) {
            return await _context.Set<GigTrackerUser>().FindAsync(id);
        }
    }
}
