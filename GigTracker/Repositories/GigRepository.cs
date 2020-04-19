using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace GigTracker.Repositories {
	public class GigRepository : IGigRepository {

		ApplicationDbContext _context;

		public GigRepository(ApplicationDbContext context) {
			_context = context;
		}

        public async Task<List<Gig>> Get() {
            return await _context.Set<Gig>().ToListAsync();
        }

        public async Task<Gig> Get(int id) {
            return await _context.Set<Gig>().FindAsync(id);
        }

        public async Task<Gig> Add(Gig gig) {
            _context.Set<Gig>().Add(gig);
            await _context.SaveChangesAsync();
            return gig;
        }

        public async Task<Gig> Delete(int id) {
            var gig = await _context.Set<Gig>().FindAsync(id);
            if (gig == null) {
                return gig;
            }

            _context.Set<Gig>().Remove(gig);
            await _context.SaveChangesAsync();

            return gig;
        }


        public async Task<Gig> Update(Gig gig) {
            _context.Entry(gig).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return gig;
        }
    }
}
