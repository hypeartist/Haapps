using System;

namespace Haapps.Gfx.Agg.WinForms.Controls.Win32
{
	public sealed class AnimationTimer : IDisposable
	{
		private int _id;
		private readonly NativeMethods.TimerCallback _callback;
		private bool _disposed;
		
		public event EventHandler Timer;

		public AnimationTimer() => _callback = Callback;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					Stop();
				}
			}
			_disposed = true;
		}

		~AnimationTimer() => Dispose(false);

		private void OnTimer(EventArgs e) => Timer?.Invoke(this, e);

		public void Stop()
		{
			lock (this)
			{
				if (_id == 0) return;
				NativeMethods.TimeKillEvent(_id);
				_id = 0;
			}
		}

		public void Start(int ms, bool repeat)
		{
			Stop();
			var f = NativeMethods.fuEvent.TIME_CALLBACK_FUNCTION | (repeat ? NativeMethods.fuEvent.TIME_PERIODIC : NativeMethods.fuEvent.TIME_ONESHOT);

			lock (this)
			{
				_id = NativeMethods.TimeSetEvent(ms, 0, _callback, UIntPtr.Zero, (int)f);
				if (_id == 0)
				{
					throw new Exception("timeSetEvent error");
				}
			}
		}

		private void Callback(int timerId, int msg, UIntPtr user, UIntPtr dw1, UIntPtr dw2) => OnTimer(new EventArgs());
	}

	public sealed class ExecutionTimer
	{
		private long _startTime;
		private long _stopTime;
		private long _freq;

		public ExecutionTimer()
		{
			_startTime = 0;
			_stopTime = 0;
		}

		public void Start()
		{
			NativeMethods.QueryPerformanceFrequency(ref _freq);
			NativeMethods.QueryPerformanceCounter(ref _startTime);
		}

		public void Stop()
		{
			NativeMethods.QueryPerformanceCounter(ref _stopTime);
		}

		public double Milliseconds => ((_stopTime - _startTime) * 1000.0) / _freq;
	}
}