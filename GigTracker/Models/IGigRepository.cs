using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigTracker.Models {
	interface IGigRepository {
		IQueryable<Gig> Gigs { get; }
	}
}
