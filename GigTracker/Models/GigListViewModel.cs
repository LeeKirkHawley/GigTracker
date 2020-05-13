using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using GigTracker.LinqExtensions;


namespace GigTracker.Models {
	public class GigListViewModel {
		public PagedResult<Gig> Gigs { get; set; }

		public PagingInfo PagingInfo { get; set; }

		public string ErrorMsg { get; set; }

		public NavbarModel NavbarModel { get; set; }

	}
}
