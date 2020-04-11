using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Models;
using GigTracker.Data;

namespace GigTracker.Data {
	public class FakeGigRepository : IGigRepository{
		IQueryable<Gig> Gigs => new List<Gig> {
			new Gig { VenueName = "Ripps", VenueAddress = "666 W. 66 St.", VenuePhone = "66-666-6666", Date = new DateTime(2020, 6, 8) , ArtistName = "The Effects"},
			new Gig { VenueName = "Rhythm Room", VenueAddress = "777 W. 7 St.", VenuePhone = "777-777-7777", Date = new DateTime(2020, 6, 8) , ArtistName = "The Effects"}
		}.AsQueryable<Gig>();

		public IEnumerable<Gig> Get() {
			return Gigs;
		}

		public Gig Get(int id) {
			Gig gig = null;
			return gig;
		}

	}
}
