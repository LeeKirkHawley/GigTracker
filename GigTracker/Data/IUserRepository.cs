using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Controllers;
using GigTracker.Models;
using GigTracker.Entities;

namespace GigTracker.Data {
	public interface IUserRepository {

		//IQueryable<User> Users { get; }
		IEnumerable<GigTrackerUser> Get();

		Task<GigTrackerUser> Get(int id);
	}
}
