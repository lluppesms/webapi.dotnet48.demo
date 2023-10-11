//-----------------------------------------------------------------------
// <copyright file="SampleDataManager.cs" company="Luppes Consulting, Inc.">
//   Copyright © 2023  Luppes Consulting, Inc.
// </copyright>
// <summary>
//   Sample Data Manager
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.Data;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Contoso.WebApi.SampleData
{
    [ExcludeFromCodeCoverage]
    public static partial class SampleDataManager
    {
        #region Sample Data Table Arrays

        /// <summary>
        /// Tbl_FactEvent Test Data
        /// </summary>
        public static List<Tbl_FactEvent> Test_Tbl_FactEvent = new List<Tbl_FactEvent>();

        /// <summary>
        /// Tbl_DimOffice Test Data
        /// </summary>
        public static List<Tbl_DimOffice> Test_Tbl_DimOffice = new List<Tbl_DimOffice>();

		/// <summary>
		/// Tbl_FactEvent Test Data
		/// </summary>
		public static List<Tbl_DimRoom> Test_Tbl_DimRoom = new List<Tbl_DimRoom>();

		///// <summary>
		///// AspNetUser test data
		///// </summary>
		//public static List<AspNetUser> Test_AspNetUser = new List<AspNetUser>();

		///// <summary>
		///// AspNetRole test data
		///// </summary>
		//public static List<AspNetRole> Test_AspNetRole = new List<AspNetRole>();

		///// <summary>
		///// AspNetUserRole test data
		///// </summary>
		//public static List<AspNetUserRole> Test_AspNetUserRole = new List<AspNetUserRole>();
		#endregion

		#region Data Variables
		///// <summary>
		///// The user name
		///// </summary>
		//private static string UserName = "UNITTEST";
        #endregion

        /// <summary>
        /// Creates the test data.
        /// </summary>
        public static void CreateSampleData()
        {
            Create_Tbl_DimOffice_Data();
			Create_Tbl_DimRoom_Data();
			Create_Tbl_FactEvent_Data();
			Create_UserProfile_Data();
		}

		/// <summary>
		/// Creates the User Profile test data.
		/// </summary>
		private static void Create_UserProfile_Data()
		{
			//if (Test_AspNetUser.Count <= 0)
			//{
			//	var thisUserId = Guid.NewGuid().ToString();
			//	Test_AspNetUser.Add(new AspNetUser()
			//	{
			//		Id = thisUserId,
			//		UserName = UserName,
			//		FirstName = "Unit",
			//		LastName = "Test",
			//		FullName = "Unit Test",
			//		AccessFailedCount = 0,
			//		SecurityStamp = Guid.NewGuid().ToString(),
			//		Email = "unittest@contoso.com",
			//		CreateUserName = UserName,
			//		CreateDateTime = DateTime.Now,
			//		ChangeUserName = UserName,
			//		ChangeDateTime = DateTime.Now
			//	});

			//	var thisAdminRoleId = Guid.NewGuid().ToString();
			//	var thisUserRoleId = Guid.NewGuid().ToString();
			//	var thisManagerRoleId = Guid.NewGuid().ToString();
			//	var thisSuperAdminRoleId = Guid.NewGuid().ToString();

			//	Test_AspNetRole.Add(new AspNetRole() { Id = thisAdminRoleId, Name = Constants.Role.AdminRole });
			//	Test_AspNetRole.Add(new AspNetRole() { Id = thisUserRoleId, Name = Constants.Role.UserRole });
			//	Test_AspNetRole.Add(new AspNetRole() { Id = thisManagerRoleId, Name = Constants.Role.ManagerRole });
			//	Test_AspNetRole.Add(new AspNetRole() { Id = thisSuperAdminRoleId, Name = Constants.Role.SuperAdminRole });

			//	Test_AspNetUserRole.Add(new AspNetUserRole() { RoleId = thisAdminRoleId, UserId = thisUserId });
			//	Test_AspNetUserRole.Add(new AspNetUserRole() { RoleId = thisUserRoleId, UserId = thisUserId });
			//	Test_AspNetUserRole.Add(new AspNetUserRole() { RoleId = thisManagerRoleId, UserId = thisUserId });
			//	Test_AspNetUserRole.Add(new AspNetUserRole() { RoleId = thisSuperAdminRoleId, UserId = thisUserId });
			//}
		}
	}
}
