using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Workforce.Models
{
	public class ChangepasswordModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string Newpassword { get; set; }
		[Required]
		[Compare ("Newpassword",ErrorMessage = "Pssword isn't matched")]
		public string Confirmpassword { get; set; }
	


	}
}