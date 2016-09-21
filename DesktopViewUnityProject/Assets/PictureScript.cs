using System;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PictureScript : MonoBehaviour
{
	// ReSharper disable once UnusedMember.Local
	void Awake()
	{
		Debug.Log(new System.IO.DirectoryInfo(".").FullName);
		Debug.Log(Application.streamingAssetsPath);
	}

	// Use this for initialization
	// ReSharper disable once UnusedMember.Local
	void Start()
	{
		var module = CompositHelper.CreateInstance();
		var texture = new Texture2D(module.Width, module.Height, TextureFormat.RGB24, false);

		transform.localScale = new Vector3((float)module.Width / module.Height, 1f, 1f);
		GetComponent<Renderer>().material.mainTexture = texture;

		Observable.Create<IObservable<byte[]>>(observer =>
		{
			while (this)
			{
				observer.OnNext(Observable.Return(module.TakeShot(), Scheduler.ThreadPool));
				Thread.Sleep(16);
			}
			observer.OnCompleted();
			return null;
		})
			.SubscribeOn(Scheduler.ThreadPool)
			.ObserveOn(Scheduler.ThreadPool)
			.Concat()
			.SampleFrame(1)
			.DistinctUntilChanged()
			.ObserveOnMainThread()
			.Subscribe(bytes =>
			{
				texture.LoadRawTextureData(bytes);
				texture.Apply();
				//texture.LoadImage(bytes);
			})
			.AddTo(this);
	}
}
