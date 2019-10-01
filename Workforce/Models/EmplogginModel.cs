using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Workforce.Models
{
	public class EmplogginModel
	{
		[Required (ErrorMessage ="Eamil is Manadatory")]
		[EmailAddress]
		public string Email { get; set; }

		[Required (ErrorMessage ="Password is Mandatory")]
		public string Password { get; set; }
	}
}