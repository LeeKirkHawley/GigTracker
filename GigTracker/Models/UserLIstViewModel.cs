using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Entities;
using GigTracker.LinqExtensions;

namespace GigTracker.Models {
	public class UserListViewModel {

		public PagedResult<User> Users { get; set; }

		public PagingInfo PagingInfo { get; set; }

		public string ErrorMsg { get; set; }

		public User CurrentUser { get; set; }


		//public IEnumerable<User> Users { get; set; }

		//public string ErrorMsg { get; set; }
	}
}
