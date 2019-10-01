using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workforce.CustomValidation;


namespace Workforce.Models
{
	public class AddemployeeModel
	{
		public int id { get; set; }

		[Required(ErrorMessage = "Please select file.")]
		[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
		public HttpPostedFileBase PhotoFile { get; set; }

		public string Photo { get; set; }
		[Required(ErrorMessage = "Please select file.")]
		[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.doc|.docx|.pdf)$", ErrorMessage = "Only doc files allowed.")]
		public HttpPostedFileBase ResumeFile { get; set; }

		public string Resume { get; set; }
		[Required]
		public int? Eemployeeid { get; set; }
		[Required]
		[RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Only Alphabets are allowed")]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
		public DateTime? Dob { get; set; }
		[Required]
		public string Gender { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		[EmailAddress]
		[Remote("IsEmailAvailable", "Home", ErrorMessage = ("This User allready Added"))]
		public string Email { get; set; }
		[Required]
		[StringLength(10, ErrorMessage = "Should be 10 digits!", MinimumLength = 10)]
		public string Mobile { get; set; }
		[Required]
		public string Positon { get; set; }
		[DateRange("01/07/2010", ErrorMessage = "Hire Date must be between 01/07/2010 to Current date")]
		[Required]
		[DataType(DataType.DateTime)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
		//[Range(typeof(DateTime), "01/07/2010", "31/12/2017")]
		[CustomHireDate (ErrorMessage ="Hiredate must me less than or equal to Today's Date")]
		public DateTime? HireDate { get; set; }
		[Required]
		public string Experience { get; set; }
		[Required]
		//[RegularExpression(@"^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", 
		//ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
		[RegularExpression(@"^(?=^.{8,15}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "Password must be minimum 8 and maximum 15 characters long, Upper case (A-Z), Lower case (a-z), Numbers (0-9) and special character (e.g. !@#$%^&*)")]
		public string Password { get; set; }


	}
}
