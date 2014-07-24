using System;
using System.Runtime.Serialization;

namespace R7.University.Employee
{
	[Serializable]
	public class EmployeeAchievementView : EmployeeAchievementInfo, ISerializable
	{
		public int ItemID { get; set; }
	
		private static int nextItemID = 0;

		public static int GetNextItemID()
		{
			return nextItemID++;
		}

		public EmployeeAchievementView ()
		{
			ItemID = GetNextItemID ();
		}
		
		public EmployeeAchievementView (EmployeeAchievementInfo achievement)
		{
			ItemID = GetNextItemID ();
			
			EmployeeAchievementID = achievement.EmployeeAchievementID;
			EmployeeID = achievement.EmployeeID;
			Title = achievement.Title;
			ShortTitle = achievement.ShortTitle;
			Description = achievement.Description;
			DocumentURL = achievement.DocumentURL;
			IsTitle = achievement.IsTitle;
			YearBegin = achievement.YearBegin;
			YearEnd = achievement.YearEnd;
			AchievementType = achievement.AchievementType;
		}

		public EmployeeAchievementInfo NewEmployeeAchievementInfo()
		{
			// REVIEW: Use decorator pattern?

			var achInfo = new EmployeeAchievementInfo ();

			achInfo.EmployeeAchievementID = EmployeeAchievementID;
			achInfo.EmployeeID = EmployeeID;
			achInfo.Title = Title;
			achInfo.ShortTitle = ShortTitle;
			achInfo.Description = Description;
			achInfo.DocumentURL = DocumentURL;
			achInfo.IsTitle = IsTitle;
			achInfo.AchievementType = AchievementType;
			achInfo.YearBegin = YearBegin;
			achInfo.YearEnd = YearEnd;

			return achInfo;
        }

		// NOTE: if [Serializable] is set, all *fields* serialized by default

		#region ISerializable implementation

		protected EmployeeAchievementView (SerializationInfo info, StreamingContext context)
		{ 
			ItemID = info.GetInt32 ("ItemID");
			EmployeeAchievementID = info.GetInt32 ("EmployeeAchievementID");
			EmployeeID = info.GetInt32 ("EmployeeID");
			Title = info.GetString ("Title");
			ShortTitle = info.GetString ("ShortTitle");
			Description = info.GetString ("Description");
			DocumentURL = info.GetString ("DocumentURL");
			IsTitle = info.GetBoolean ("IsTitle");
			AchievementType = (AchievementType)info.GetChar ("AchievementType");
			
			try
			{
				YearBegin = info.GetInt32 ("YearBegin");
			}
			catch
			{
				YearBegin = null;
			}
			
			try
			{
				YearEnd = info.GetInt32 ("YearEnd");
			}
			catch
			{
				YearEnd = null;
			}
		}

		public void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			info.AddValue ("ItemID", ItemID);
			info.AddValue ("EmployeeAchievementID", EmployeeAchievementID);
			info.AddValue ("EmployeeID", EmployeeID);
			info.AddValue ("Title", Title);
			info.AddValue ("ShortTitle", ShortTitle);
			info.AddValue ("Description", Description);
			info.AddValue ("DocumentURL", DocumentURL);
			info.AddValue ("IsTitle", IsTitle);
			info.AddValue ("AchievementType", (char)AchievementType);
			
			if (YearBegin != null)
				info.AddValue ("YearBegin", YearBegin.Value);
			
			if (YearEnd != null)
				info.AddValue ("YearEnd", YearEnd.Value);
		}

		#endregion
		
	
		
       

	} // class
} // namespace

