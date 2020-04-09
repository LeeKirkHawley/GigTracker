﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigTracker.Models {
	public interface IAccountService {
		public abstract string HashPwd(string pwd);
		public abstract bool VerifyPwd(string hashedPwd, string password);
	}
}
