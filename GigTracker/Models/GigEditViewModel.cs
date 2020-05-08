using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Entities;

namespace GigTracker.Models {
	public class GigEditViewModel {
		public Gig Gig { get; set; }
		public string ErrorMsg { get; set; }

	}
}
