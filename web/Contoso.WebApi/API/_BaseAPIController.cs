//-----------------------------------------------------------------------
// <copyright file="_BaseAPIController.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Base API Controller
// </summary>
//-----------------------------------------------------------------------

using System;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.Http;

namespace Contoso.WebApi.API
{
    /// <summary>
    /// Base API Class
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class _BaseAPIController : ApiController
    {
        #region Variables
        /// <summary>
        /// Unknown User Id
        /// </summary>
        protected const string UnknownUserName = "Unknown";

        /// <summary>
        /// The bogus user name
        /// </summary>
        protected const string BogusUserName = "BOGUS";
        #endregion

        #region Authorization Helpers
        /// <summary>
        /// Returns UserName if logged on, UnitTest if request object is null
        /// </summary>
        /// <returns>UserName</returns>
        protected string GetUserName()
        {
            var userName = UnknownUserName;
            if (HttpContext.Current == null)
            {
                return userName;
            }

            if (HttpContext.Current.User == null)
            {
                return userName;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (HttpContext.Current.User.Identity != null)
            {
                userName = RawUserName(HttpContext.Current.User.Identity.Name);
            }
            return userName;
        }

        /// <summary>
        /// Returns UserName without the domain attached.
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <returns>User Name without domain</returns>
        protected string RawUserName(string userName)
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
        #endregion

        #region Nullable Object Helpers
        /// <summary>
        /// Returns a string.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <returns>Return value.</returns>
        protected string CStrNull(object o)
        {
            return CStrNull(o, string.Empty);
        }

        /// <summary>
        /// Returns a string.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <param name="dflt">Default value.</param>
        /// <returns>Return value.</returns>
        protected string CStrNull(object o, string dflt)
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
        protected decimal CDecNull(object o)
        {
            return CDecNull(o, 0);
        }

        /// <summary>
        /// Returns a decimal.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <param name="dflt">Default value.</param>
        /// <returns>Return value.</returns>
        protected decimal CDecNull(object o, decimal dflt)
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
        protected int CIntNull(object o)
        {
            return CIntNull(o, 0);
        }

        /// <summary>
        /// Returns an integer.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <param name="dflt">Default value.</param>
        /// <returns>Return value.</returns>
        protected int CIntNull(object o, int dflt)
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
        protected DateTime CDateNull(object o)
        {
            return CDateNull(o, DateTime.MinValue);
        }

        /// <summary>
        /// Returns a DateTime object.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <param name="dflt">Default value.</param>
        /// <returns>Return value.</returns>
        protected DateTime CDateNull(object o, DateTime dflt)
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
        #endregion

        #region Other
        /// <summary>
        /// Gets inner exception message.
        /// </summary>
        /// <param name="ex">The Exception.</param>
        /// <returns>The Full Message.</returns>
        protected string GetExceptionMessage(Exception ex)
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
                    try
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
                    catch
                    {
                        //// ignore it and move on...
                        message += ex.Message;
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
    }
}