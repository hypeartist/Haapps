namespace Haapps.Gfx.Agg
{
	public struct CellStyle : ICell<CellStyle>
	{
		public int X { get; set; }

		public int Y { get; set; }

		public int Cover { get; set; }

		public int Area { get; set; }

		public int Left { get; set; }

		public int Right { get; set; }

		public void Reset()
		{
			X = 0x7FFFFFFF;
			Y = 0x7FFFFFFF;
			Cover = 0;
			Area = 0;
			Left = -1;
			Right = -1;
		}

		public readonly bool CheckIfNotEqual(int ex, int ey, in CellStyle c) => ((ex - X) | (ey - Y) | (Left - c.Left) | (Right - c.Right)) != 0;

		public void SetStyle(ref CellStyle styleCell)
		{
			Left = styleCell.Left;
			Right = styleCell.Right;
		}
	}
}