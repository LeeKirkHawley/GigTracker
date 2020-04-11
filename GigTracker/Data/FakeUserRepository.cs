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

		IQueryable<GigTrackerUser> Users => _userService.GetAll().AsQueryable();

		public async Task<List<GigTrackerUser>> Get() { 
			var t = await Task.Run(() => Users.ToList());
			return t;
		}

		public async Task<GigTrackerUser> Get(int id) {
			var t = await Task.Run(() => (GigTrackerUser)null);
			return t;
		}

	}
}
