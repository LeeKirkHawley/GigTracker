using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Entities;

namespace GigTracker.Repositories {
	public interface IGigRepository {

        Task<List<Gig>> Get();

        Task<Gig> Get(int id);

        Task<Gig> Add(Gig gig);

        Task<Gig> Delete(int id);

        Task<Gig> Update(Gig gig);
    }
}
