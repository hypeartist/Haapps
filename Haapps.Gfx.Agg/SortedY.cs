namespace Haapps.Gfx.Agg
{
	public struct SortedY
	{
		public int Start;
		public int Count;
#if DEBUG
		public override string ToString() => $"Start: {Start}, Count: {Count}";
#endif
	}
}