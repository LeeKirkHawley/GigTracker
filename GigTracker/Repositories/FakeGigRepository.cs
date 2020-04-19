using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Entities;

namespace GigTracker.Repositories {
	public class FakeGigRepository : IGigRepository{
		List<Gig> Gigs => new List<Gig> {
			new Gig { Id = 1, VenueName = "Ripps", VenueAddress = "666 W. 66 St.", VenuePhone = "66-666-6666", Date = new DateTime(2020, 6, 8) , ArtistName = "The Effects"},
			new Gig { Id = 2, VenueName = "Rhythm Room", VenueAddress = "777 W. 7 St.", VenuePhone = "777-777-7777", Date = new DateTime(2020, 6, 8) , ArtistName = "The Effects"}
		};

		public async Task<List<Gig>> Get() {
			return Gigs;
		}

		public Task<Gig> Get(int id) {
			return Task.FromResult(Gigs.FirstOrDefault());
		}

		public async Task<Gig> Add(Gig gig) {
			await Task.Run(() => Gigs.Add(gig));
			return gig;
		}

		public async Task<Gig> Delete(int id) {
			Gig gig = Gigs.Where(g => g.Id == id).FirstOrDefault();
			await Task.Run(() => Gigs.Remove(gig));
			return gig;
		}

		public async Task<Gig> Update(Gig gig) {
			int a = 0;
			await Task.Run(() => a = 1); // some bs so I don't have to implement this at the oment
			return gig;
		}
	}
}
