using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Controllers;
using GigTracker.Models;
using GigTracker.Entities;

namespace GigTracker.Repositories {
	public interface IUserRepository {

		//IQueryable<User> Users { get; }
		IEnumerable<User> Get();

		Task<User> Get(int id);
	}
}
