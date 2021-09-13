
namespace TabletFriend.Data
{
	public enum StretchData
	{
		/// <summary>
		/// The content preserves its original size.
		/// </summary>
		None = 0,

		/// <summary> 
		/// The content is resized to fill the destination dimensions. 
		/// The aspect ratio is not preserved.
		/// </summary>
		Fill = 1,

		/// <summary>
		/// The content is resized to fit in the destination dimensions while it preserves its native aspect ratio.
		/// </summary>
		Fit = 2,
	}
}
