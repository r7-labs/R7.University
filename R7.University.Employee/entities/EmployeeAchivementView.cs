using System;
using System.Runtime.Serialization;

namespace R7.University.Employee
{
	[Serializable]
	public class EmployeeAchivementView : EmployeeAchivementInfo, ISerializable
	{
		public int ItemID { get; set; }
	
		/*
		public int EmployeeAchivementID { get; set; }
		public int EmployeeID { get; set; }
		public string Title { get; set; }
		public string ShortTitle { get; set; }
		public int? YearBegin { get; set; }
		public int? YearEnd { get; set; }
		public AchivementType AchivementType { get; set; }
		*/

		private static int nextItemID = 0;

		public static int GetNextItemID()
		{
			return nextItemID++;
		}

		public EmployeeAchivementView ()
		{
			ItemID = GetNextItemID ();
		}
		
		public EmployeeAchivementView (EmployeeAchivementInfo achivement)
		{
			ItemID = GetNextItemID ();
			
			EmployeeAchivementID = achivement.EmployeeAchivementID;
			EmployeeID = achivement.EmployeeID;
			Title = achivement.Title;
			ShortTitle = achivement.ShortTitle;
			Description = achivement.Description;
			DocumentURL = achivement.DocumentURL;
			IsTitle = achivement.IsTitle;
			YearBegin = achivement.YearBegin;
			YearEnd = achivement.YearEnd;
			AchivementType = achivement.AchivementType;
		}

		// NOTE: if [Serializable] is set, all *fields* serialized by default

		#region ISerializable implementation

		protected EmployeeAchivementView (SerializationInfo info, StreamingContext context)
		{ 
			ItemID = info.GetInt32 ("ItemID");
			EmployeeAchivementID = info.GetInt32 ("EmployeeAchivementID");
			EmployeeID = info.GetInt32 ("EmployeeID");
			Title = info.GetString ("Title");
			ShortTitle = info.GetString ("ShortTitle");
			Description = info.GetString ("Description");
			DocumentURL = info.GetString ("DocumentURL");
			IsTitle = info.GetBoolean ("IsTitle");
			AchivementType = (AchivementType)info.GetChar ("AchivementType");
			
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
			info.AddValue ("EmployeeAchivementID", EmployeeAchivementID);
			info.AddValue ("EmployeeID", EmployeeID);
			info.AddValue ("Title", Title);
			info.AddValue ("ShortTitle", ShortTitle);
			info.AddValue ("Description", Description);
			info.AddValue ("DocumentURL", DocumentURL);
			info.AddValue ("IsTitle", IsTitle);
			info.AddValue ("AchivementType", (char)AchivementType);
			
			if (YearBegin != null)
				info.AddValue ("YearBegin", YearBegin.Value);
			
			if (YearEnd != null)
				info.AddValue ("YearEnd", YearEnd.Value);
		}

		#endregion
		
		/*
		public EmployeeAchivementInfo NewEmployeeAchivementInfo()
		{
			
			var achInfo = new EmployeeAchivementInfo ();

			// achInfo.Description 
			// achInfo.DocumentURL
			// achInfo.IsTitle 
			
			achInfo.EmployeeID = EmployeeID;
			achInfo.EmployeeAchivementID = EmployeeAchivementID;
			achInfo.Title = Title;
			achInfo.ShortTitle = ShortTitle;
			achInfo.AchivementType = AchivementType;
			achInfo.YearBegin = YearBegin;
			achInfo.YearEnd = YearEnd;

			return achInfo;
            
			
			// return this;
		}
        */

	} // class
} // namespace

