using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Controllers;
using GigTracker.Models;

namespace GigTracker.Data {
	public interface IUserRepository {

		//IQueryable<User> Users { get; }
		Task<List<User>> Get();

		Task<User> Get(int id);
	}
}
