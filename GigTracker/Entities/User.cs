using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace GigTracker.Entities {
	public class User {
		[Key]
		public int Id { get; set; }

		[Required]
		[Display(Name = "User Name")]
		public string UserName { get; set; }

		[Required]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		public string FullName {
			get { return FirstName + " " + LastName; }
		}

		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }

		public string Role { get; set; }

		public string Token { get; set; }

		[Timestamp]
		[HiddenInput]
		public byte[] RowVersion { get; set; }

	}
}
