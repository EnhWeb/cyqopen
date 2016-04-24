

using System;
using System.IO;
using System.Text;

namespace ICSharpCode.SharpZipLib.Zip
{
	
	/// <summary>
	/// This class assists with writing/reading from Zip files.
	/// </summary>
	internal class ZipHelperStream : Stream
	{
		#region Constructors
		/// <summary>
		/// Initialise an instance of this class.
		/// </summary>
		/// <param name="name">The name of the file to open.</param>
		public ZipHelperStream(string name)
		{
			stream_ = new FileStream(name, FileMode.Open, FileAccess.ReadWrite);
			isOwner_ = true;
		}

		/// <summary>
		/// Initialise a new instance of <see cref="ZipHelperStream"/>.
		/// </summary>
		/// <param name="stream">The stream to use.</param>
		public ZipHelperStream(Stream stream)
		{
			stream_ = stream;
		}
		#endregion

		/// <summary>
		/// Get / set a value indicating wether the the underlying stream is owned or not.
		/// </summary>
		/// <remarks>If the stream is owned it is closed when this instance is closed.</remarks>
		public bool IsStreamOwner
		{
			get { return isOwner_; }
			set { isOwner_ = value; }
		}

		#region Base Stream Methods
		public override bool CanRead
		{
			get { return stream_.CanRead; }
		}

		public override bool CanSeek
		{
			get { return stream_.CanSeek; }
		}

#if !NET_1_0 && !NET_1_1 && !NETCF_1_0
		public override bool CanTimeout
		{
			get { return stream_.CanTimeout; }
		}
#endif

		public override long Length
		{
			get { return stream_.Length; }
		}

		public override long Position
		{
			get { return stream_.Position; }
			set { stream_.Position = value;	}
		}

		public override bool CanWrite
		{
			get { return stream_.CanWrite; }
		}

		public override void Flush()
		{
			stream_.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return stream_.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			stream_.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return stream_.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			stream_.Write(buffer, offset, count);
		}

		/// <summary>
		/// Close the stream.
		/// </summary>
		/// <remarks>
		/// The underlying stream is closed only if <see cref="IsStreamOwner"/> is true.
		/// </remarks>
		override public void Close()
		{
			Stream toClose = stream_;
			stream_ = null;
			if (isOwner_ && (toClose != null))
			{
				isOwner_ = false;
				toClose.Close();
			}
		}
		#endregion
		
	
		/// <summary>
		/// Locates a block with the desired <paramref name="signature"/>.
		/// </summary>
		/// <param name="signature">The signature to find.</param>
		/// <param name="endLocation">Location, marking the end of block.</param>
		/// <param name="minimumBlockSize">Minimum size of the block.</param>
		/// <param name="maximumVariableData">The maximum variable data.</param>
		/// <returns>Eeturns the offset of the first byte after the signature; -1 if not found</returns>
		public long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
		{
			long pos = endLocation - minimumBlockSize;
			if ( pos < 0 ) {
				return -1;
			}

			long giveUpMarker = Math.Max(pos - maximumVariableData, 0);

			// TODO: This loop could be optimised for speed.
			do {
				if ( pos < giveUpMarker ) {
					return -1;
				}
				Seek(pos--, SeekOrigin.Begin);
			} while ( ReadLEInt() != signature );

			return Position;
		}


		#region LE value reading/writing
		/// <summary>
		/// Read an unsigned short in little endian byte order.
		/// </summary>
		/// <returns>Returns the value read.</returns>
		/// <exception cref="IOException">
		/// An i/o error occurs.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		/// The file ends prematurely
		/// </exception>
		public int ReadLEShort()
		{
			int byteValue1 = stream_.ReadByte();

			if (byteValue1 < 0) {
				throw new EndOfStreamException();
			}

			int byteValue2 = stream_.ReadByte();
			if (byteValue2 < 0) {
				throw new EndOfStreamException();
			}

			return byteValue1 | (byteValue2 << 8);
		}

		/// <summary>
		/// Read an int in little endian byte order.
		/// </summary>
		/// <returns>Returns the value read.</returns>
		/// <exception cref="IOException">
		/// An i/o error occurs.
		/// </exception>
		/// <exception cref="System.IO.EndOfStreamException">
		/// The file ends prematurely
		/// </exception>
		public int ReadLEInt()
		{
			return ReadLEShort() | (ReadLEShort() << 16);
		}

		#endregion


		#region Instance Fields
		bool isOwner_;
		Stream stream_;
		#endregion
	}
}
