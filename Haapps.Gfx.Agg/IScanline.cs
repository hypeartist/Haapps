namespace Haapps.Gfx.Agg
{
	public unsafe interface IScanline
	{
		bool IsHit { get; }
		int Y { get; }
		bool IsEmpty { get; }
		int CalcMaxLength(int minX, int maxX);
		void Reset(byte* covers, Span* spans);
		void AddCell(int x, byte cover);
		void AddCells(int x, int length, byte* covers);
		void AddSpan(int x, int length, byte cover);
		void Finalize(int y);
		void ResetSpans();
		Span* GetSpans(out int count);
	}
}