﻿using System;
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
		public int? UserId { get; set; }

		[HiddenInput]
		public string UserRole { get; set; }

		public string ArtistSearch { get; set; }

		public virtual User User { get; set; }

	}
}