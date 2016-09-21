using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace twinkfrag.DesktopView.Component
{
	public static class DesktopViewStaticModule
	{
		public static int Width { get; }

		public static int Height { get; }

		private readonly static Point pointZero = default(Point);

		static DesktopViewStaticModule()
		{
			Width = Screen.PrimaryScreen.WorkingArea.Width;
			Height = Screen.PrimaryScreen.WorkingArea.Height;
#if DEBUG
			Width = 640;
			Height = 480;
#endif
		}

		public static byte[] TakeShot()
		{
			using (var bitmap = new Bitmap(Width, Height))
			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.CopyFromScreen(pointZero, pointZero, bitmap.Size, CopyPixelOperation.SourceCopy);
				Cursor.Current?.Draw(graphics, new Rectangle(Cursor.Position, Cursor.Current.Size));

				var data = bitmap.LockBits(
					new Rectangle(pointZero, bitmap.Size),
					ImageLockMode.ReadOnly,
					PixelFormat.Format24bppRgb);

				var bytes = new byte[data.Height * data.Stride];
				Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

				bitmap.UnlockBits(data);
				return bytes;
			}
		}
	}
}