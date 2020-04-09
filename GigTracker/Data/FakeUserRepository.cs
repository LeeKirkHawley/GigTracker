using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Controllers;
using GigTracker.Models;
using GigTracker.Data;


namespace GigTracker.Data {
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

		public async Task<List<User>> Get() { 
			var t = await Task.Run(() => new List<User>());
			return t;
		}

		public async Task<User> Get(int id) {
			var t = await Task.Run(() => (User)null);
			return t;
		}

	}
}
