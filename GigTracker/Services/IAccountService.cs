using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigTracker.Entities;

namespace GigTracker.Services {
	public interface IAccountService {
		public abstract string HashPwd(string pwd);
		public abstract bool VerifyPwd(string hashedPwd, string password);
	}
}
