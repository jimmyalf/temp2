﻿using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace Spinit.Wpc.Synologen.ServiceLibrary {
	public class UserValidation : UserNamePasswordValidator {
		public override void Validate ( string userName, string password ) {
			var exceptionMessage = "Validation Failed!";
			var lowerCaseUserName = userName.ToLower();
			var lowercaseClientCredential = ConfigurationSettings.Common.ClientCredentialUserName.ToLower();

			if (!lowerCaseUserName.Equals(lowercaseClientCredential))
				throw new SecurityTokenException( exceptionMessage );

			if(!password.Equals(ConfigurationSettings.Common.ClientCredentialPassword))
				throw new SecurityTokenException( exceptionMessage );
		}
	}
}
