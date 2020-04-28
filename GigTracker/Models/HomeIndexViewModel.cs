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
	public class HomeIndexViewModel {
		[HiddenInput]
		public int? userId { get; set; }

		[HiddenInput]
		public string userRole { get; set; }

		public PagedResult<Gig> Gigs { get; set; }

		public PagingInfo PagingInfo { get; set; }

		public string ArtistSearch { get; set; }

	}
}
