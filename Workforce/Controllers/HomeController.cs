using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Workforce.Models;

namespace Workforce.Controllers
{

	public class HomeController : Controller
	{

		WorkforceEntities db = new WorkforceEntities();

		public object WebSecurity { get; private set; }

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contactus()
		{
			ViewBag.Message = "Your Contact Page.";
			return View();
		}
		[HttpGet]
		[UserAuthentication("MD")]
		public ActionResult Addemployee()
		{
			ViewBag.Success = TempData["Success"];
			var citylist = new List<SelectListItem> {
				new SelectListItem { Text = "Ahmedabad",Value = "Ahmedabad"},
				new SelectListItem { Text = "Mumbai",Value = "Mumbai"},
				new SelectListItem { Text ="Delhi", Value="Delhi"},
				new SelectListItem { Text ="Chennai", Value="Chennai"},
				new SelectListItem { Text ="Hyderabad", Value="Hyderabad"},
				new SelectListItem { Text ="Banglore", Value="Banglore"},
				new SelectListItem { Text ="Pune", Value="Pune"},
			};
			var positionlist = new List<SelectListItem> {
				new SelectListItem { Text = "MD",Value = "MD"},
				new SelectListItem { Text = "CEO",Value = "CEO"},
				new SelectListItem { Text ="GM", Value="GM"},
				new SelectListItem { Text ="Manager", Value="Manager"},
				new SelectListItem { Text ="Employee", Value="Employee"},
			};

			var experience = new List<SelectListItem> {
				new SelectListItem { Text ="0-1 Year", Value="0-1"},
				new SelectListItem { Text ="1-3 Year", Value="1-3"},
				new SelectListItem { Text ="3-5 Year", Value="3-5"},
				new SelectListItem { Text ="5+ Year", Value="5+"}
			};
			ViewBag.Postionlist = positionlist;
			ViewBag.Citylist = citylist;
			ViewBag.Experience = experience;

			AddemployeeModel aempm = new AddemployeeModel();
			return View(aempm);
		}
		[UserAuthentication("MD")]
		[HttpPost]
		public ActionResult Addemployee(AddemployeeModel model)
		{
			string ResumeName = Path.GetFileNameWithoutExtension(model.ResumeFile.FileName);
			string ResumeExtension = Path.GetExtension(model.ResumeFile.FileName);
			//Add Current Date and time To Attached File Name  
			ResumeName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + ResumeName.Trim() + ResumeExtension;
			string Resumepath = Path.Combine(Server.MapPath("~/Resume"), ResumeName);
			model.ResumeFile.SaveAs(Resumepath);

			string PhotoName = Path.GetFileNameWithoutExtension(model.PhotoFile.FileName);
			string PhotoExtension = Path.GetExtension(model.PhotoFile.FileName);
			//Add Current Date and time To Attached File Name  
			PhotoName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + PhotoName.Trim() + PhotoExtension;
			string PhotoPath = Path.Combine(Server.MapPath("~/Image"), PhotoName);
			model.PhotoFile.SaveAs(PhotoPath);

			wf_emp wfemp = new wf_emp
			{
				Photo = PhotoName,
				Resume = ResumeName,
				Id = model.id,
				Employeeid = model.Eemployeeid,
				Name = model.Name,
				Dob = model.Dob,
				Gender = model.Gender,
				Address = model.Address,
				City = model.City,
				Email = model.Email,
				Mobile = model.Mobile,
				Position = model.Positon,
				HireDate = model.HireDate,
				Experience = model.Experience,
				Password = model.Password
			};
			db.wf_emp.Add(wfemp);
			db.SaveChanges();

			//using (MailMessage mm = new MailMessage(new MailAddress("Dotnet2784@gmail.com", "Highfive Rewards"), new MailAddress(model.Email)))
			//	//customise name here
			//{
			//	try
			//	{
			//		//customise subject here
			//		mm.Subject = "Welcome to Highfive Reward!";
			//		//customise mail body here
			//		mm.Body = "<a href='http://xyz.com/account/verify/code'>Hello, Thanks for Register with us!!</a>";
			//		mm.IsBodyHtml = true;
			//		SmtpClient smtp = new SmtpClient();
			//		smtp.Host = /*"smtp-mail.outlook.com";*/"smtp.gmail.com";
			//		smtp.EnableSsl = true;                                   
			//		NetworkCredential NetworkCred = new NetworkCredential("Dotnet2784@gmail.com", "dotnetmvc");
			//		smtp.UseDefaultCredentials = false;                   //email & password
			//		smtp.Credentials = NetworkCred;
			//		smtp.Port = 587;
			//		smtp.Send(mm);
			//	}
			//	catch (Exception ex)
			//	{
			//		throw ex;
			//	}
			//}

			TempData["Success"] = "Employee data added successfully.";
			return RedirectToAction("Addemployee", "Home");
		}
		public JsonResult IsEmailAvailable(string Email)
		{
			return Json(!db.wf_emp.Any(i => i.Email == Email), JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		[UserAuthentication("MD,Manager")]
		public ActionResult Employeelist()
		{
			List<AddemployeeModel> employeelist = db.wf_emp.Select(i => new AddemployeeModel
			{
				Photo = i.Photo,
				Resume = i.Resume,
				id = i.Id,
				Eemployeeid = i.Employeeid,
				Name = i.Name,
				Dob = i.Dob,
				Gender = i.Gender,
				Address = i.Address,
				City = i.City,
				Email = i.Email,
				Mobile = i.Mobile,
				Positon = i.Position,
				HireDate=i.HireDate,
				Experience = i.Experience,
				Password = i.Password,

			}).ToList();

			return View(employeelist);
		}
		public ActionResult Details(int? id)
		{
			var employee = db.wf_emp.Where(i => i.Id == id).FirstOrDefault();
			if (employee == null)
			{
				ViewBag.Massage = "Not Found";
				return View();
			}
			else
			{
				return View(employee);
			}

		}

		[HttpGet]
		public ActionResult UpdateEmployee(int id)
		{
			ViewBag.Success = TempData["Success"];
			var citylist = new List<SelectListItem> {
				new SelectListItem { Text = "Ahmedabad",Value = "Ahmedabad"},
				new SelectListItem { Text = "Mumbai",Value = "Mumbai"},
				new SelectListItem { Text ="Delhi", Value="Delhi"},
				new SelectListItem { Text ="Chennai", Value="Chennai"},
				new SelectListItem { Text ="Hyderabad", Value="Hyderabad"},
				new SelectListItem { Text ="Banglore", Value="Banglore"},
				new SelectListItem { Text ="Pune", Value="Pune"},
			};
			ViewBag.Success = TempData["Success"];
			var positionlist = new List<SelectListItem> {
				new SelectListItem { Text = "MD",Value = "MD"},
				new SelectListItem { Text = "CEO",Value = "CEO"},
				new SelectListItem { Text ="GM", Value="GM"},
				new SelectListItem { Text ="Manager", Value="Manager"},
				new SelectListItem { Text ="Employee", Value="Employee"},
			};

			var experience = new List<SelectListItem> {
				new SelectListItem { Text ="0-1 Year", Value="0-1"},
				new SelectListItem { Text ="1-3 Year", Value="1-3"},
				new SelectListItem { Text ="3-5 Year", Value="3-5"},
				new SelectListItem { Text ="5+ Year", Value="5+"}
			};
			ViewBag.Postionlist = positionlist;
			ViewBag.Citylist = citylist;
			ViewBag.Experience = experience;

			var wfemp = db.wf_emp.Where(i => i.Id == id).FirstOrDefault();
			AddemployeeModel model = new AddemployeeModel();
			if (wfemp != null)
			{
				model.id = wfemp.Id;
				model.Photo = wfemp.Photo;
				model.Resume = wfemp.Resume;
				model.Eemployeeid = wfemp.Employeeid;
				model.Name = wfemp.Name;
				model.Dob = wfemp.Dob;
				model.Gender = wfemp.Gender;
				model.Address = wfemp.Address;
				model.City = wfemp.City;
				model.Email = wfemp.Email;
				model.Mobile = wfemp.Mobile;
				model.Positon = wfemp.Position;
				model.HireDate = wfemp.HireDate;
				model.Experience = wfemp.Experience;
				model.Password = wfemp.Password;
			}
			else
			{
				ViewBag.Error = "Record not Found";
			}
			return View(model);
		}
		[HttpPost]
		public ActionResult UpdateEmployee(AddemployeeModel model)
		{
			string ResumeName = Path.GetFileNameWithoutExtension(model.ResumeFile.FileName);
			string ResumeExtension = Path.GetExtension(model.ResumeFile.FileName);
			//Add Current Date and time To Attached File Name  
			ResumeName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + ResumeName.Trim() + ResumeExtension;
			string Resumepath = Path.Combine(Server.MapPath("~/Resume"), ResumeName);
			model.ResumeFile.SaveAs(Resumepath);

			string PhotoName = Path.GetFileNameWithoutExtension(model.PhotoFile.FileName);
			string PhotoExtension = Path.GetExtension(model.PhotoFile.FileName);
			//Add Current Date and time To Attached File Name  
			PhotoName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + PhotoName.Trim() + PhotoExtension;
			string PhotoPath = Path.Combine(Server.MapPath("~/Image"), PhotoName);
			model.PhotoFile.SaveAs(PhotoPath);

			wf_emp wfemp = db.wf_emp.Where(i => i.Id == model.id).FirstOrDefault();
			wfemp.Id = model.id;
			wfemp.Photo = PhotoName;
			wfemp.Resume = ResumeName;
			wfemp.Employeeid = model.Eemployeeid;
			wfemp.Name = model.Name;
			wfemp.Dob = model.Dob;
			wfemp.Gender = model.Gender;
			wfemp.Address = model.Address;
			wfemp.City = model.City;
			wfemp.Email = model.Email;
			wfemp.Mobile = model.Mobile;
			wfemp.Position = model.Positon;
			wfemp.HireDate = model.HireDate;
			wfemp.Experience = model.Experience;
			wfemp.Password = model.Password;
			db.SaveChanges();

			return RedirectToAction("Employeelist", "Home");
		}
		[HttpGet]
		public ActionResult RemoveEmployee(int id)
		{
			wf_emp wfemp = db.wf_emp.Where(i => i.Id == id).FirstOrDefault();
			db.wf_emp.Remove(wfemp);
			db.SaveChanges();
			return RedirectToAction("Employeelist", "Home");
		}
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Emploggin()
		{
			ViewBag.Success = TempData["Success"];
			return View();
		}
		[HttpPost]
		[AllowAnonymous]
		public ActionResult Emploggin(EmplogginModel model)
		{
			if (ModelState.IsValid)
			{
				var account = db.wf_emp.Where(i => i.Email == model.Email && i.Password == model.Password).FirstOrDefault();
				if (account != null)
				{
					FormsAuthentication.SetAuthCookie(account.Id.ToString(), false);
					var authTicket = new FormsAuthenticationTicket(1, account.Id.ToString(), DateTime.Now, DateTime.Now.AddMinutes(10), false, account.Position);
					string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
					var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
					HttpContext.Response.Cookies.Add(authCookie);

					HttpCookie cookies = new HttpCookie("user_photo", account.Photo);
					Response.Cookies.Add(cookies);

					HttpCookie cookie = new HttpCookie("user_name", account.Name);
					Response.Cookies.Add(cookie);

					//Session["user_id"] = "1";

					return RedirectToAction("Employeelist", "Home");
				}
				else
				{
					TempData["Success"] = "Invalid User!";
					return RedirectToAction("Emploggin", "Home");
				}

			}
			else
			{
				TempData["Success"] = "All fields is required!";
				return RedirectToAction("Emploggin", "Home");
			}

		}
		[AllowAnonymous]
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("Emploggin", "Home");
		}

		[HttpGet]
		public ActionResult Changepassword()
		{
			ViewBag.Success = TempData["Success"];
			return View();
		}
		[HttpPost]
		public ActionResult Changepassword(ChangepasswordModel model)
		{
			if (ModelState.IsValid)
			{
				var account = db.wf_emp.Where(i => i.Email == model.Email && i.Password == model.Password).FirstOrDefault();
				if (account != null)
				{
					wf_emp wfemp = db.wf_emp.Where(i => i.Email == model.Email).FirstOrDefault();
					wfemp.Password = model.Newpassword;
					db.SaveChanges();


					TempData["Success"] = "You have change Password Successfully!!";
					return RedirectToAction("Emploggin", "Home");
				}

				else
				{
					TempData["Success"] = "Invalid User!!";
					return RedirectToAction("Changepassword", "Home");
				}
			}
			else
			{
				TempData["Success"] = "All field is require!!";
				return RedirectToAction("Changepassword", "Home");
			}
		}

		[HttpGet]
		public ActionResult ForgotPassword()
		{
			return View();
		}
		[HttpPost]
		public ActionResult ForgotPassword(ForgotPassword model)
		{
			if (ModelState.IsValid)
			{
				var userexist = db.wf_emp.Where(i => i.Email == model.Email).FirstOrDefault();
				if (userexist != null)
				{
					Guid g = Guid.NewGuid();
					string GuidString = Convert.ToBase64String(g.ToByteArray());
					GuidString = GuidString.Replace("=", "");
					GuidString = GuidString.Replace("+", "");
					userexist.Tokan = GuidString;
					db.SaveChanges();

					using (MailMessage mm = new MailMessage(new MailAddress("Dotnet2784@gmail.com", "Wokforce Admin"), new MailAddress(model.Email)))
					//customise name here
					{
						try
						{
							//customise subject here
							mm.Subject = "Worforce/Forgot Password do not reply!";
							//customise mail body here
							mm.Body = "<a href='http://localhost:64391/home/ResetPassword?Email=" + Url.Encode(userexist.Email) + "&Token=" + GuidString + "'>Click on Reset Password Link!!</a>";
							mm.IsBodyHtml = true;
							SmtpClient smtp = new SmtpClient();
							smtp.Host = /*"smtp-mail.outlook.com";*/"smtp.gmail.com";
							smtp.EnableSsl = true;
							NetworkCredential NetworkCred = new NetworkCredential("Dotnet2784@gmail.com", "dotnetmvc");
							smtp.UseDefaultCredentials = false;                   //email & password
							smtp.Credentials = NetworkCred;
							smtp.Port = 587;
							smtp.Send(mm);
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}

				}

			}
			return View("ForgotPasswordMassage");
		}
		public ActionResult ForgotPasswordMassage()
		{
			return View();
		}
		[HttpGet]
		public ActionResult ResetPassword(string Email, string Token)
		{
			var userexist = db.wf_emp.Where(i => i.Email == Email && i.Tokan == Token).FirstOrDefault();
			if (userexist != null)
			{
				ResetPasswordModel model = new ResetPasswordModel();
				model.Email = Email;

				ViewBag.Success = TempData["Success"];
				return View("ResetPassword", model);
			}
			else
			{
				return View("AccessDenied");
			}
		}
		[HttpPost]
		public ActionResult ResetPassword(ResetPasswordModel model)
		{

			var userexist = db.wf_emp.Where(i => i.Email == model.Email).FirstOrDefault();
			if (userexist != null)
			{
				userexist.Password = model.NewPasword;
				db.SaveChanges();

				TempData["Success"] = "You have reset your Password Successfully!!";

				return View("Emploggin");

			}
			else
			{
				return View();
			}

		}
		[HttpGet]
		public ActionResult AccessDenied()
		{
			return View();

		}

	}
}
