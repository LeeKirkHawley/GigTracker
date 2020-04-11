using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GigTracker.Entities {
	public class GigTrackerUser {
		[Key]
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public string Token { get; set; }

	}
}
