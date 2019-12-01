namespace Haapps.Gfx.Agg
{
	public readonly unsafe struct RowInfo
	{
		public readonly byte* Pointer;
		public readonly int X1;
		public readonly int X2;

		public RowInfo(byte* ptr, int x1, int x2)
		{
			Pointer = ptr;
			X1 = x1;
			X2 = x2;
		}
	}
}
