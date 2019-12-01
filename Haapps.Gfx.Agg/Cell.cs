namespace Haapps.Gfx.Agg
{
	public struct Cell : ICell<Cell>
	{
		public int X { get; set; }

		public int Y { get; set; }

		public int Cover { get; set; }

		public int Area { get; set; }

		public void Reset()
		{
			X = 0x7FFFFFFF;
			Y = 0x7FFFFFFF;
			Cover = 0;
			Area = 0;
		}

		public readonly bool CheckIfNotEqual(int ex, int ey, in Cell _) => ((ex - X) | (ey - Y)) != 0;

		public void SetStyle(ref Cell cell)
		{
		}
#if DEBUG
		public override string ToString() => $"X: {X}, Y: {Y}, Cover: {Cover}, Area: {Area}";
#endif
	}
}