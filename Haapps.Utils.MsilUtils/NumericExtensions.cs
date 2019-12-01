namespace Kaybbo.Utils
{
	public static class NumericExtensions
	{
		public static Numeric<T, C> ToNumeric<T, C>(this byte v)  where T:unmanaged where C:unmanaged
		{
			return v;
		}
	}
}