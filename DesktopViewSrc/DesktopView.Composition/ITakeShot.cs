namespace twinkfrag.DesktopView.Composition
{
	public interface ITakeShot
    {
	    byte[] TakeShot();

		int Width { get; }
		int Height { get; }
    }
}
