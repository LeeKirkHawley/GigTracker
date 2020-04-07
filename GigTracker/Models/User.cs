using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigTracker.Models {
	public class User {
		string UserName { get; set; }
		string Password { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string Email { get; set; }
	}
}
