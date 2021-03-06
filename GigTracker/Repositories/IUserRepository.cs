﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Controllers;
using GigTracker.Models;
using GigTracker.Entities;

namespace GigTracker.Repositories {
	public interface IUserRepository {

        Task<IEnumerable<User>> Get();

        Task<User> Get(int id);

        User GetNoTracking(int id);

        Task<User> Add(User gig);

        Task<User> Delete(int id);

        Task<User> Update(User gig);
    }
}
