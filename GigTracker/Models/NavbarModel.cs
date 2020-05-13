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
	public class NavbarModel {

		[HiddenInput]
		public int? CurrentUserId { get; set; }

		public virtual User CurrentUser { get; set; }

		public string ArtistSearch { get; set; }

	}
}
