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
		[Display(Name = "Venue Name")]
		public string VenueName { get; set; }

		[Required]
		[Display(Name = "Venue Address")] 
		public string VenueAddress { get; set; }

		[Display(Name = "Venue Phone")]
		public string VenuePhone { get; set; }

		[Display(Name = "Gig Date")]
		public DateTime Date { get; set;}

		[Display(Name = "Artist's Name")]
		[Required]
		public string ArtistName { get; set; }

	}
}
