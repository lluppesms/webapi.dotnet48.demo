//-----------------------------------------------------------------------
// <copyright file="_BaseController.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Base Controller
// </summary>
//-----------------------------------------------------------------------
using Contoso.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contoso.WebApi.Controllers
{
	/// <summary>
	/// Base Controller
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class _BaseController : Controller
	{
		#region Configuration Functions
		/// <summary>
		/// Gets value from web.config file AppSettings section.
		/// </summary>
		/// <param name="settingName">Setting Name.</param>
		/// <returns>Return value.</returns>
		public string GetConfigKeyValue(string settingName)
		{
			try
			{
				if (System.Configuration.ConfigurationManager.AppSettings[settingName] != null)
				{
					return System.Configuration.ConfigurationManager.AppSettings[settingName];
				}

				return string.Empty;
			}
			catch
			{
				return string.Empty;
			}
		}
		#endregion

		#region Logging Functions
		/// <summary>
		/// Write value to log.
		/// </summary>
		/// <param name="msg">Message Text.</param>
		public void WriteToLog(string msg)
		{
			var logDirectory = GetConfigKeyValue("LogDirectory");
			var logFileName = GetConfigKeyValue("LogFileName");

			StreamWriter myStreamWriter = null;
			try
			{
				if (logDirectory.Length <= 0)
				{
					logDirectory = @"C:\Logs\";
				}

				if (logFileName.Length <= 0)
				{
					logFileName = "YOURPROJECT.log";
				}

				logFileName = DateifyFileName(logDirectory + logFileName);
				if (!Directory.Exists(logDirectory))
				{
					Directory.CreateDirectory(logDirectory);
				}

				if (logFileName == string.Empty)
				{
					return;
				}
				myStreamWriter = System.IO.File.AppendText(logFileName);
				myStreamWriter.WriteLine("{0}\t{1}", DateTime.Now, msg);
				myStreamWriter.Flush();
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch
			{
				////  do nothing with the message ?
			}
			finally
			{
				//// Close the object if it has been created.
				if (myStreamWriter != null)
				{
					myStreamWriter.Close();
				}
			}
		}

		/// <summary>
		/// Write value to log.
		/// </summary>
		/// <param name="msg">Message Text.</param>
		public void WriteSevereError(string msg)
		{
			WriteSevereError(string.Empty, msg, GetRequestUrl(), GetUserName());
		}

		/// <summary>
		/// Write value to log.
		/// </summary>
		/// <param name="msg">Message Text.</param>
		/// <param name="url">URL for the page with an error.</param>
		/// <param name="userName">User Name.</param>
		public void WriteSevereError(string msg, string url, string userName)
		{
			WriteSevereError(string.Empty, msg, url, userName);
		}

		/// <summary>
		/// Write value to log.
		/// </summary>
		/// <param name="subjectText">Subject of email.</param>
		/// <param name="msg">Message Text.</param>
		/// <param name="url">URL for the page with an error.</param>
		/// <param name="userName">User Name.</param>
		public void WriteSevereError(string subjectText, string msg, string url, string userName)
		{
			////var returnMessageText = string.Empty;
			msg = msg.Trim();
			if (msg.StartsWith("Thread was being aborted") || msg.EndsWith("Thread was being aborted") ||
				msg.EndsWith("Thread was being aborted."))
			{
				System.Diagnostics.Trace.TraceError(msg);
				return;
			}

			//// always write to database
			////try
			////{
			////  DatabaseEntities pe = new DatabaseEntities();
			////  LogEntry le = new LogEntry("ERROR", msg, userName);
			////  pe.LogEntry.Add(le);
			////  pe.SaveChanges();
			////}
			////catch (Exception)
			////{
			////  //  do nothing with the message ?
			////}

			if (url == string.Empty)
			{
				url = "Unknown";
			}
			//// write to local text file if on local machine
			if (url.IndexOf("localhost", StringComparison.CurrentCultureIgnoreCase) > 0)
			{
				WriteToLog(msg);
			}
			////else
			////{
			////try
			////{
			//////// send email to admins if it's not local machine
			////string errorEmail = GetConfigKeyValue("EmailSendTo");
			////string siteName = GetConfigKeyValue("ApplicationName");
			////if (siteName == string.Empty)
			////{
			////  siteName = "website";
			////}
			////if (subjectText == string.Empty)
			////{
			////  subjectText = "An error occured in " + siteName + "!";
			////}
			////else
			////{
			////  subjectText = subjectText.Replace("@SiteName", siteName);
			////}
			////var msgPlus = msg + "<br /><br />" + "\n" +
			////  "Sent from " + url + "<br />" + "\n" +
			////  "User: " + userName + "<br />" + "\n" +
			////  "Machine " + Environment.MachineName + "<br />" + "\n";
			////SendMail(errorEmail, "HTML", subjectText, msgPlus, ref returnMessageText);
			////}
			//// ReSharper disable once EmptyGeneralCatchClause
			////catch
			////{
			////  ////  do nothing with the message ?
			////}
			////}
		}

		/// <summary>
		/// Convert file name to format that contains date as numbers.
		/// </summary>
		/// <param name="parmFileName">File Name to be changed.</param>
		/// <returns>File Name.</returns>
		public string DateifyFileName(string parmFileName)
		{
			var newDate = DateTime.Today.ToString("yyyyMMdd");
			var period = parmFileName.IndexOf(".", StringComparison.Ordinal);
			if (period > 0)
			{
				parmFileName = parmFileName.Substring(0, period) + newDate + parmFileName.Substring(period, parmFileName.Length - period);
			}
			else
			{
				parmFileName = parmFileName + newDate;
			}

			return parmFileName;
		}

		/// <summary>
		/// Gets inner exception message.
		/// </summary>
		/// <param name="ex">The Exception.</param>
		/// <returns>The Full Message.</returns>
		public string GetExceptionMessage(Exception ex)
		{
			var message = string.Empty;
			if (ex == null)
			{
				return message;
			}

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (ex.Message != null)
			{
				if (ex.GetType().IsAssignableFrom(typeof(DbEntityValidationException)))
				{
					var exv = (DbEntityValidationException)ex;
					// ReSharper disable once LoopCanBeConvertedToQuery
					foreach (var eve in exv.EntityValidationErrors)
					{
						// ReSharper disable once LoopCanBeConvertedToQuery
						foreach (var ve in eve.ValidationErrors)
						{
							message += " " + ve.PropertyName + ": " + ve.ErrorMessage;
						}
					}
				}
				else
				{
					message += ex.Message;
				}
			}

			if (ex.InnerException == null)
			{
				return message;
			}

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (ex.InnerException.Message != null)
			{
				message += " " + ex.InnerException.Message;
			}

			if (ex.InnerException.InnerException == null)
			{
				return message;
			}

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (ex.InnerException.InnerException.Message != null)
			{
				message += " " + ex.InnerException.InnerException.Message;
			}

			if (ex.InnerException.InnerException.InnerException == null)
			{
				return message;
			}

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (ex.InnerException.InnerException.InnerException.Message != null)
			{
				message += " " + ex.InnerException.InnerException.InnerException.Message;
			}

			return message;
		}
		#endregion

		#region Nullable Object Helpers
		/// <summary>
		/// Returns a string.
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <returns>Return value.</returns>
		public string CStrNull(object o)
		{
			return CStrNull(o, string.Empty);
		}

		/// <summary>
		/// Returns a string.
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <param name="dflt">Default value.</param>
		/// <returns>Return value.</returns>
		public string CStrNull(object o, string dflt)
		{
			string returnValue;
			try
			{
				if (o != null && !Convert.IsDBNull(o))
				{
					returnValue = o.ToString();
				}
				else
				{
					returnValue = dflt;
				}
			}
			catch
			{
				returnValue = null;
			}

			return returnValue;
		}

		/// <summary>
		/// Returns a decimal.
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <returns>Return value.</returns>
		public decimal CDecNull(object o)
		{
			return CDecNull(o, 0);
		}

		/// <summary>
		/// Returns a decimal.
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <param name="dflt">Default value.</param>
		/// <returns>Return value.</returns>
		public decimal CDecNull(object o, decimal dflt)
		{
			decimal returnValue;
			try
			{
				if (o != null && !Convert.IsDBNull(o))
				{
					returnValue = Convert.ToDecimal(o);
				}
				else
				{
					returnValue = dflt;
				}
			}
			catch
			{
				return decimal.MinValue;
			}

			return returnValue;
		}

		/// <summary>
		/// Returns an integer.
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <returns>Return value.</returns>
		public int CIntNull(object o)
		{
			return CIntNull(o, 0);
		}

		/// <summary>
		/// Returns an integer.
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <param name="dflt">Default value.</param>
		/// <returns>Return value.</returns>
		public int CIntNull(object o, int dflt)
		{
			int returnValue;
			try
			{
				if (o != null && !Convert.IsDBNull(o) && Convert.ToString(o) != string.Empty)
				{
					returnValue = Convert.ToInt32(o);
				}
				else
				{
					returnValue = dflt;
				}
			}
			catch
			{
				return int.MinValue;
			}

			return returnValue;
		}

		/// <summary>
		/// Returns a DateTime object.
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <returns>Return value.</returns>
		public DateTime CDateNull(object o)
		{
			return CDateNull(o, DateTime.MinValue);
		}

		/// <summary>
		/// Returns a DateTime object.
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <param name="dflt">Default value.</param>
		/// <returns>Return value.</returns>
		public DateTime CDateNull(object o, DateTime dflt)
		{
			DateTime returnValue;
			try
			{
				if (o != null && !Convert.IsDBNull(o))
				{
					returnValue = Convert.ToDateTime(o);
				}
				else
				{
					returnValue = dflt;
				}
			}
			catch
			{
				return DateTime.MinValue;
			}

			return returnValue;
		}

		/// <summary>
		/// Returns a Global Id
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <returns>Return value.</returns>
		public Guid CGuidNull(object o)
		{
			return CGuidNull(o, Guid.Empty);
		}

		/// <summary>
		/// Returns a Global Id
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <param name="dflt">Default value in a string</param>
		/// <returns>Return value.</returns>
		public Guid CGuidNull(object o, string dflt)
		{
			return CGuidNull(o, Guid.Parse(dflt));
		}

		/// <summary>
		/// Returns a Global Id
		/// </summary>
		/// <param name="o">Input parameter.</param>
		/// <param name="dflt">Default value.</param>
		/// <returns>Return value.</returns>
		public Guid CGuidNull(object o, Guid dflt)
		{
			Guid returnValue;
			try
			{
				if (o != null && !Convert.IsDBNull(o))
				{
					returnValue = Guid.Parse(o.ToString());
				}
				else
				{
					returnValue = dflt;
				}
			}
			catch
			{
				returnValue = Guid.Empty;
			}

			return returnValue;
		}
		#endregion

		#region Request Helpers
		/// <summary>
		/// Returns UserName if logged on, UnitTest if request object is null
		/// </summary>
		/// <returns>User Name</returns>
		public string GetUserName()
		{
			return (User == null || string.IsNullOrEmpty(User.Identity.Name)) ? "Unknown" : RawUserName(User.Identity.Name);
		}

		/// <summary>
		/// Returns UserName without the domain attached.
		/// </summary>
		/// <param name="userName">User Name</param>
		/// <returns>User Name without domain</returns>
		public string RawUserName(string userName)
		{
			var rawUserName = userName;
			try
			{
				userName = userName.Replace("/", "\\");
				var startPos = userName.LastIndexOf("\\", StringComparison.Ordinal);
				if (startPos > 0)
				{
					rawUserName = userName.Substring(startPos + 1, userName.Length - startPos - 1).Replace("\\", string.Empty);
				}
			}
			catch (Exception)
			{
				rawUserName = string.Empty;
			}

			return rawUserName;
		}

		/// <summary>
		/// Returns UserAgent if logged on, UnitTest if request object is null.
		/// </summary>
		/// <returns>The User Agent.</returns>
		public string GetUserAgent()
		{
			if (HttpContext == null)
			{
				return "UnitTest";
			}

			if (HttpContext.Request == null)
			{
				return string.Empty;
			}

			return HttpContext.Request.UserAgent ?? string.Empty;
		}

		/// <summary>
		/// Returns UserAgent and IPAddress if logged on, UnitTest if request object is null.
		/// </summary>
		/// <returns>The Request Details.</returns>
		public string GetRequestDetails()
		{
			try
			{
				if (HttpContext == null)
				{
					return "UnitTest";
				}
				if (HttpContext.Request == null)
				{
					return string.Empty;
				}
				if (HttpContext.Request.UserAgent == null)
				{
					return string.Empty;
				}
				return "UserAgent: " + HttpContext.Request.UserAgent + "; UserHostAddress: " + HttpContext.Request.UserHostAddress;
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Returns URL from request object if it exists.
		/// </summary>
		/// <returns>The Request URL.</returns>
		public string GetRequestUrl()
		{
			try
			{
				if (HttpContext == null)
				{
					return "UnitTest";
				}
				var applicationURL = CStrNull(HttpContext.Request.Url);
				applicationURL = applicationURL.ToLower();
				////applicationURL = FixURLNames(applicationURL);
				return applicationURL;
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Returns a QueryString value, or empty string if request object is null
		/// </summary>
		/// <param name="key">The requested key</param>
		/// <returns>The requested value</returns>
		public string GetRequestQueryString(string key)
		{
			if (HttpContext == null)
			{
				return string.Empty;
			}

			return HttpContext.Request == null ? string.Empty : HttpContext.Request.QueryString[key] ?? string.Empty;
		}

		/// <summary>
		/// Returns a Cookie value, or empty string if request object is null
		/// </summary>
		/// <param name="key">The requested key</param>
		/// <returns>The requested value</returns>
		public string GetRequestCookie(string key)
		{
			if (HttpContext == null)
			{
				return string.Empty;
			}

			if (HttpContext.Request == null)
			{
				return string.Empty;
			}

			return HttpContext.Request.Cookies[key] == null ? string.Empty : HttpContext.Request.Cookies[key].Value;
		}

		/// <summary>
		/// Create Cookie
		/// </summary>
		/// <param name="cookieName">Cookie Name</param>
		/// <param name="cookieValue">Cookie Value</param>
		public void CreateCookie(string cookieName, string cookieValue)
		{
			var newCookie = new HttpCookie(cookieName)
			{
				Value = cookieValue,
				Expires = DateTime.UtcNow.AddDays(360)
			};
			Response.Cookies.Add(newCookie);
		}

		/// <summary>
		/// Returns Base Application URL
		/// </summary>
		/// <param name="thisControllerName">Controller Name</param>
		/// <returns>Url</returns>
		public string GetApplicationURL(string thisControllerName)
		{
			var applicationURL = GetRequestUrl();
			applicationURL = applicationURL.ToLower();
			var startPos = applicationURL.IndexOf(thisControllerName.ToLower(), StringComparison.Ordinal);
			if (startPos > 0)
			{
				applicationURL = applicationURL.Substring(0, startPos);
			}

			applicationURL = applicationURL.ToLower();
			return applicationURL;
		}

		/// <summary>
		/// Checks to see if user is running Internet Explorer 6 or 7
		/// </summary>
		/// <returns>Result</returns>
		public bool IsObsoleteBrowser()
		{
			var ua = GetUserAgent().ToUpper();
			if (ua.Contains("MSIE 6"))
			{
				return true;
			}
			if (ua.Contains("MSIE 8"))
			{
				return true;
			}
			//// Trident new in IE8 and above
			return ua.Contains("MSIE 7") && !ua.Contains("TRIDENT");
		}
		#endregion

		#region Standard Lookup Tables
		/// <summary>
		/// Get Ranged Number List
		/// </summary>
		/// <param name="minValue">Min Value</param>
		/// <param name="maxValue">Max Value</param>
		/// <returns>List</returns>
		public List<StaticNumberTable> GetRangedNumberList(int minValue, int maxValue)
		{
			return (from r in Enumerable.Range(minValue, maxValue)
					select new StaticNumberTable(r, r)
				   ).ToList();
		}

		/// <summary>
		/// Get Sort Order List of Numbers from 1-99
		/// </summary>
		/// <returns>List</returns>
		public List<StaticNumberTable> GetSortOrderList()
		{
			return (from r in Enumerable.Range(1, 99)
					select new StaticNumberTable(r, r)
				   ).ToList();
		}

		/// <summary>
		/// Get List of Yes/No
		/// </summary>
		/// <returns>List</returns>
		public List<StaticCodeTable> GetYesNoList()
		{
			return new List<StaticCodeTable>() { new StaticCodeTable("Y", "Yes"), new StaticCodeTable("N", "No") };
		}
		#endregion

		#region MVC Helpers
		/// <summary>
		/// Gets a list of errors in the model state and combines them into one simple string.
		/// Very useful for debugging if you get an error in your model, but you are not displaying the field on the screen so you don't know what it is.
		/// </summary>
		/// <param name="ms">Model State.</param>
		/// <returns>List of errors in one string.</returns>
		public string ShowAllModelStateErrors(ModelStateDictionary ms)
		{
			//// if you have errors you can't find, set a breakpoint after the ModelState.IsValid and run this command in the Immediate Window:
			////    ? ShowAllModelStateErrors(ModelState)
			//// OR -- if you want to use this more often, you can put it right into your controller & view
			////   put code like this in your Controller
			////      if (ModelState.IsValid)
			////        {... do stuff here...}
			////      else
			////        ViewBag.allErrors = ShowAllModelStateErrors(ModelState);
			////   put this somewhere in your view
			////      @{ if (ViewBag.allErrors != null) { <!-- Errors: @ViewBag.allErrors  --> } }
			////   now when you hit the screen again, those errors will be visible
			var errors = ms
			  .Where(x => x.Value.Errors.Count > 0)
			  .Select(x => new
			  {
				  x.Key,
				  x.Value.Errors
			  })
			  .ToArray();
			var allErrors = string.Empty;
			foreach (var err in errors)
			{
				allErrors += err.Key + ": ";
				// ReSharper disable once LoopCanBeConvertedToQuery
				foreach (var e2 in err.Errors)
				{
					allErrors += e2.ErrorMessage + "; ";
				}
			}

			return allErrors;
		}

		/// <summary>
		/// Removes auto-populated audit fields from validation
		/// </summary>
		public void RemoveAuditFieldsFromValidation()
		{
			RemoveAuditFieldsFromValidation(new[] { "CreateUserName", "CreateDateTime", "ChangeUserName", "ChangeDateTime" });
		}

		/// <summary>
		/// Removes auto-populated audit fields from validation
		/// </summary>
		/// <param name="fieldsToRemove">List of fields to remove from ModelState</param>
		public void RemoveAuditFieldsFromValidation(string[] fieldsToRemove)
		{
			if (fieldsToRemove == null)
			{
				return;
			}

			foreach (var s in fieldsToRemove)
			{
				ModelState.Remove(s);
			}
		}
		#endregion
	}
}