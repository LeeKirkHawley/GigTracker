using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Controllers;
using GigTracker.Models;
using GigTracker.Data;
using GigTracker.Services;
using GigTracker.Entities;

namespace GigTracker.Data {
	public class FakeUserRepository : IUserRepository {

		IAccountService _accountService;
		UserService _userService;

		//FakeUserRepository() { }

		public FakeUserRepository(IAccountService accountService, UserService userService) {
			_accountService = accountService;
			_userService = userService;
		}

		IQueryable<User> Users => _userService.GetAll().AsQueryable();

		public IEnumerable<User> Get() { 
			//var t = Task.Run(() => Users);
			return Users;
		}

		public async Task<User> Get(int id) {
			var t = await Task.Run(() => (User)null);
			return t;
		}

	}
}
