using System;

namespace R7.University.Employee
{
	[Serializable]
	public class OccupiedPositionView
	{
		public int ItemID { get; set; }

		public int PositionID { get; set; }

		public int DivisionID { get; set; }

        public string PositionShortTitle { get; set; }

        public string DivisionShortTitle { get; set; }

		public bool IsPrime { get; set; }

		public string TitleSuffix { get; set; }

        public string PositionShortTitleWithSuffix
        {
            get { return PositionShortTitle + " " + TitleSuffix; }
        }

		public OccupiedPositionView ()
		{
			ItemID = ViewNumerator.GetNextItemID ();
		}

		public OccupiedPositionInfo NewOccupiedPositionInfo ()
		{
			var opinfo = new OccupiedPositionInfo ();

			opinfo.PositionID = PositionID;
			opinfo.DivisionID = DivisionID;
			opinfo.IsPrime = IsPrime;
			opinfo.TitleSuffix = TitleSuffix;

			return opinfo;
		}

		public OccupiedPositionView (OccupiedPositionInfoEx opex) : this ()
		{
			PositionID = opex.PositionID;
			DivisionID = opex.DivisionID;
            PositionShortTitle = PositionInfo.FormatShortTitle (opex.PositionTitle, opex.PositionShortTitle);
            DivisionShortTitle = DivisionInfo.FormatShortTitle (opex.DivisionTitle, opex.DivisionShortTitle);
			IsPrime = opex.IsPrime;
			TitleSuffix = opex.TitleSuffix;
		}
	}
}
