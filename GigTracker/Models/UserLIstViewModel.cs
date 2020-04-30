using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Entities;

namespace GigTracker.Models {
	public class UserListViewModel {
		public IEnumerable<User> Users { get; set; }

		public string ErrorMsg { get; set; }
	}
}
