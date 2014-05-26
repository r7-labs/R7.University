using System;
using System.Runtime.Serialization;

namespace R7.University.Employee
{
	[Serializable]
	public class OccupiedPositionView : ISerializable
	{
		public int ItemID { get; set; }

		public int PositionID { get; set; }
		public int DivisionID { get; set; }
		public string PositionShortTitle { get; set; }
		public string DivisionShortTitle { get; set; }
		public bool IsPrime { get; set; }

		private static int nextItemID = 0;

		public static int GetNextItemID()
		{
			return nextItemID++;
		}

		public OccupiedPositionView ()
		{
			ItemID = GetNextItemID ();
		}

		public OccupiedPositionInfo NewOccupiedPositionInfo()
		{
			var opinfo = new OccupiedPositionInfo ();

			opinfo.PositionID = PositionID;
			opinfo.DivisionID = DivisionID;
			opinfo.IsPrime = IsPrime;

			return opinfo;
		}

		public OccupiedPositionView (OccupiedPositionInfoEx opex) : this()
		{
			PositionID = opex.PositionID;
			DivisionID = opex.DivisionID;
			PositionShortTitle = opex.PositionShortTitle;
			DivisionShortTitle = opex.DivisionShortTitle;
			IsPrime = opex.IsPrime;
		}

		public OccupiedPositionView  (int positionID, string positionShortTitle, 
			int divisionID, string divisionShortTitle, bool isPrime) : this()
		{
			PositionID = positionID;
			DivisionID = divisionID;
			PositionShortTitle = positionShortTitle;
			DivisionShortTitle = divisionShortTitle;
			IsPrime = isPrime;
		}

		// NOTE: if [Serializable] is set, all *fields* serialized by default

		#region ISerializable implementation

		protected OccupiedPositionView (SerializationInfo info, StreamingContext context)
		{ 
			ItemID = info.GetInt32("ItemID");
			PositionID = info.GetInt32("PositionID");
			DivisionID = info.GetInt32("DivisionID");
			PositionShortTitle = info.GetString("PositionShortTitle");
			DivisionShortTitle = info.GetString("DivisionShortTitle");
			IsPrime = info.GetBoolean ("IsPrime");
		}

		public void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			info.AddValue ("ItemID", ItemID);
			info.AddValue ("PositionID", PositionID);
			info.AddValue ("DivisionID", DivisionID);
			info.AddValue ("PositionShortTitle", PositionShortTitle);
			info.AddValue ("DivisionShortTitle", DivisionShortTitle);
			info.AddValue ("IsPrime", IsPrime);
		}

		#endregion

	} // class
} // namespace

