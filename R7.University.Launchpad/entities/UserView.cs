using System;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common.Utilities;

namespace R7.University.Launchpad
{
	/* was used in OO-implementation of users combobox
	public class UserView : UserInfo
	{
		public UserView (string overrideValue)
		{
			UsernameAndEmail = overrideValue;
		}

		public UserView (UserInfo user)
		{
			UserID = user.UserID;
			Email = user.Email;
			Username = user.Username;
		}

		private string overrideValue;

		public string UsernameAndEmail
		{
			get 
			{ 
				if (overrideValue == null)
					return Username + " / " + Email;
				else
					return overrideValue;
			}
			set	
			{ 
				overrideValue = value; 
				UserID = Null.NullInteger;
			}
		}
	}*/
}

