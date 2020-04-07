using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigTracker.Models {
	public class FakeUserRepository : IUserRepository {
		public IQueryable<User> Users => new List<User> {
			new User { UserName = "kirkhawley", Password = "password", FirstName = "Kirk", LastName = "Hawley", Email = "leekirkhawley@gmail.com"}
		}.AsQueryable<User>();
	}
}
