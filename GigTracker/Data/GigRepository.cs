using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Models;

namespace GigTracker.Data {
	public class GigRepository : IGigRepository {

		ApplicationDbContext _context;

		public GigRepository(ApplicationDbContext context) {
			_context = context;
		}

        public IEnumerable<Gig> Get() {
            var t = _context.Set<Gig>();
            return t;
        }

        public Gig Get(int id) {
            //return await _context.Set<Gig>().FindAsync(id);
            return _context.Gig.Where(g => g.Id == id).FirstOrDefault();
        }

    }
}
