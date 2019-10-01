using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workforce.Models
{
	public class ResetPasswordModel
	{
		public string Email { get; set; }
		public string NewPasword { get; set; }
		public string ConfirmPassword { get; set; }

	}
}