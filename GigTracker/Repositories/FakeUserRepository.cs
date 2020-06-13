using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Controllers;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Services;
using GigTracker.Entities;

namespace GigTracker.Repositories {
	public class FakeUserRepository : UserRepository {

		//IAccountService _accountService;
		//UserService _userService;

		//public FakeUserRepository(IAccountService accountService, UserService userService) {
		//	_accountService = accountService;
		//	_userService = userService;
		//}

		//IQueryable<User> Users => _userService.GetAll().AsQueryable();

		//public IEnumerable<User> Get() { 
		//	//var t = Task.Run(() => Users);
		//	return Users;
		//}

		//public async Task<User> Get(int id) {
		//	var t = await Task.Run(() => (User)null);
		//	return t;
		//}

		List<User> Users => new List<User> {
			new User { Id = 1, UserName = "jimbob", Password = "password", FirstName = "Jim", LastName = "Bob", Email = "jimbob@bobs" , Role = Role.Admin},
			new User { Id = 2, UserName = "marjorie", Password = "pasword", FirstName = "Marge", LastName = "Morningstar", Email = "marge@margesplace", Role = Role.User}
		};


		public async Task<IEnumerable<User>> Get() {
			return Users;
		}

		public Task<User> Get(int id) {
			return Task.FromResult(Users.FirstOrDefault());
		}

		public User GetNoTracking(int id) {
			return Users.FirstOrDefault();
		}

		public async Task<User> Add(User gig) {
			await Task.Run(() => Users.Add(gig));
			return gig;
		}

		public async Task<User> Delete(int id) {
			User gig = Users.Where(g => g.Id == id).FirstOrDefault();
			await Task.Run(() => Users.Remove(gig));
			return gig;
		}

		public async Task<User> Update(User gig) {
			int a = 0;
			await Task.Run(() => a = 1); // some bs so I don't have to implement this at the oment
			return gig;
		}
	}
}
