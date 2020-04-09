using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace GigTracker.Models {
	public class Gig {
		[Key]
		public int Id { get; set; }
		public string VenueName { get; set; }
		public string VenueAddress { get; set; }
		public string VenuePhone { get; set; }
		public DateTime Date { get; set; }
		public string ArtistName { get; set; }
	}
}
