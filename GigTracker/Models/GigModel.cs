using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigTracker.Models {
	public class GigModel {
		public string VenueName { get; set; }
		public string VenueAddress { get; set; }
		public string VenuePhone { get; set; }
		public DateTime Date { get; set; }
		public string ArtistName { get; set; }
	}
}
