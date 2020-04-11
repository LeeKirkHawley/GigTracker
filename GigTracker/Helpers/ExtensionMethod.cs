using System.Collections.Generic;
using System.Linq;
using GigTracker.Entities;

namespace GigTracker.Helpers {
    public static class ExtensionMethods {
        public static IEnumerable<GigTrackerUser> WithoutPasswords(this IEnumerable<GigTrackerUser> users) {
            if (users == null) return null;

            return users.Select(x => x.WithoutPassword());
        }

        public static GigTrackerUser WithoutPassword(this GigTrackerUser user) {
            if (user == null) return null;

            user.Password = null;
            return user;
        }
    }
}