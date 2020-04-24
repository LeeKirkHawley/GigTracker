using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Entities;

namespace GigTracker.Models {
	public class GigListViewModel {
		public IEnumerable<Gig> Gigs { get; set; }

		public User User { get; set; }

	}
}
