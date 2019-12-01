namespace Haapps.Gfx.Agg
{
	public interface ICell<TSelf>
		where TSelf : unmanaged, ICell<TSelf>
	{
		int X { get; set; }
		int Y { get; set; }
		int Cover { get; set; }
		int Area { get; set; }
		void Reset();
		bool CheckIfNotEqual(int ex, int ey, in TSelf c = default);
		void SetStyle(ref TSelf cell);
	}
}