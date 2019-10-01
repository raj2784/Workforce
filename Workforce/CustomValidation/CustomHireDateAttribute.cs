using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Workforce.CustomValidation
{
	public class CustomHireDateAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			DateTime dateTime = Convert.ToDateTime(value);
			return dateTime <= DateTime.Now;
		}
	}
}
