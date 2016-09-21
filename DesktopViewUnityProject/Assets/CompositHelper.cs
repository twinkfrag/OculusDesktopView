using System;
using System.Linq;
using System.IO;
using twinkfrag.DesktopView.Composition;

public static class CompositHelper
{
	private const string ComponentFilename = "DesktopView.Component.dll";

	public static ITakeShot CreateInstance()
	{
#if UNITY_EDITOR
		var path = Path.Combine(new DirectoryInfo(".").FullName, ComponentFilename);
#else
		var path = Path.Combine(UnityEngine.Application.streamingAssetsPath, ComponentFilename);
#endif
		var interfaceName = typeof(ITakeShot).FullName;
		var assembly = System.Reflection.Assembly.LoadFrom(path);

		var types = assembly.GetTypes();
		// ReSharper disable once ReplaceWithSingleCallToFirst
		var typeName = types
			.Where(x => x.GetInterface(interfaceName) != null)
			.Where(x => x.IsClass)
			.Where(x => x.IsPublic)
			.Where(x => !x.IsAbstract)
			.First();
		UnityEngine.Debug.Log(typeName.FullName);

		return assembly.CreateInstance("twinkfrag.DesktopView.Component.DesktopViewModule") as ITakeShot;
	}
}

internal class TakeShotOnUnity : ITakeShot
{
	internal TakeShotOnUnity(int width, int height, Func<byte[]> takeShot)
	{
		this.Width = width;
		this.Height = height;
		this.takeShot = takeShot;
	}

	public byte[] TakeShot()
	{
		if (takeShot != null)
		{
			return takeShot();
		}
		throw new NotImplementedException();
	}

	private readonly Func<byte[]> takeShot;

	public int Width { get; private set; }
	public int Height { get; private set; }
}