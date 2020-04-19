using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace GigTracker.Models {
	public class HomeIndexViewModel {
		[HiddenInput]
		public int? userId { get; set; }

		[HiddenInput]
		public string userRole { get; set; }
	}
}
