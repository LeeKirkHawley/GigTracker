using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigTracker.Entities {
	public class Gig {
		[Key]
		public int Id { get; set; }

		public User User { get; set; }

		[ForeignKey("User")]
		[Required]
		public int UserId { get; set; }

		[Required]
		public string VenueName { get; set; }

		public string VenueAddress { get; set; }

		public string VenuePhone { get; set; }

		public DateTime Date { get; set;}

		[Required]
		public string ArtistName { get; set; }

	}
}
