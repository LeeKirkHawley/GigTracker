using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Models;

namespace GigTracker.Data {
	public interface IGigRepository {
		IQueryable<Gig> Gigs { get; }
	}
}
