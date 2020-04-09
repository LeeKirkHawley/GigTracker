using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Controllers;

namespace GigTracker.Models {
	public class FakeUserRepository : IUserRepository {

		IAccountService _accountService;

		//FakeUserRepository() { }

		public FakeUserRepository(IAccountService accountService) {
			_accountService = accountService;
		}

		public IQueryable<User> Users => new List<User> {
			new User { UserName = "kirkhawley", Password = _accountService.HashPwd("password"), FirstName = "Kirk", LastName = "Hawley", Email = "leekirkhawley@gmail.com"},
			new User { UserName = "leonredbone", Password = _accountService.HashPwd("password"), FirstName = "Leon", LastName = "Redbone", Email = "leekirkhawley@gmail.com"}
		}.AsQueryable<User>();
	}
}
