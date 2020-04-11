using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GigTracker.Models {
	public class CreateRoleViewModel {
		[Required]
		public string RoleName { get; set; }
	}
}
