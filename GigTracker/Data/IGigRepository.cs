using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Models;

namespace GigTracker.Data {
	public interface IGigRepository {
		IEnumerable<Gig> Get();

		Gig Get(int id);
	}
}
