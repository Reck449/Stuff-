using System;
using System.IO;
using System.Threading;

namespace Nomad.Archive.SevenZip
{
	public class InStreamTimedWrapper : StreamWrapper, ISequentialInStream, IInStream
	{
		private const int KeepAliveInterval = 10000;

		private string BaseStreamFileName;

		private long BaseStreamLastPosition;

		private Timer CloseTimer;

		public InStreamTimedWrapper(Stream baseStream) : base(baseStream)
		{
			if (this.BaseStream is FileStream && !this.BaseStream.CanWrite && this.BaseStream.CanSeek)
			{
				this.BaseStreamFileName = ((FileStream)this.BaseStream).Name;
				this.CloseTimer = new Timer(new TimerCallback(this.CloseStream), null, 10000, -1);
			}
		}

		private void CloseStream(object state)
		{
			if (this.CloseTimer != null)
			{
				this.CloseTimer.Dispose();
				this.CloseTimer = null;
			}
			if (this.BaseStream != null)
			{
				if (this.BaseStream.CanSeek)
				{
					this.BaseStreamLastPosition = this.BaseStream.Position;
				}
				this.BaseStream.Close();
				this.BaseStream = null;
			}
		}

		public uint Read(byte[] data, uint size)
		{
			this.ReopenStream();
			return (uint)this.BaseStream.Read(data, 0, (int)size);
		}

		protected void ReopenStream()
		{
			if (this.BaseStream != null)
			{
				if (this.CloseTimer != null)
				{
					this.CloseTimer.Change(10000, -1);
				}
				return;
			}
			if (this.BaseStreamFileName == null)
			{
				throw new ObjectDisposedException("StreamWrapper");
			}
			this.BaseStream = new FileStream(this.BaseStreamFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
			{
				Position = this.BaseStreamLastPosition
			};
			this.CloseTimer = new Timer(new TimerCallback(this.CloseStream), null, 10000, -1);
		}

		public override void Seek(long offset, uint seekOrigin, IntPtr newPosition)
		{
			this.ReopenStream();
			base.Seek(offset, seekOrigin, newPosition);
		}
	}
}