

using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	public interface ITaggedData
	{
		/// <summary>
		/// Get the ID for this tagged data value.
		/// </summary>
		short TagID { get; }

		/// <summary>
		/// Set the contents of this instance from the data passed.
		/// </summary>
		/// <param name="data">The data to extract contents from.</param>
		/// <param name="offset">The offset to begin extracting data from.</param>
		/// <param name="count">The number of bytes to extract.</param>
		void SetData(byte[] data, int offset, int count);

		/// <summary>
		/// Get the data representing this instance.
		/// </summary>
		/// <returns>Returns the data for this instance.</returns>
		byte[] GetData();
	}
	
  
	sealed public class ZipExtraData : IDisposable
	{
		#region Constructors

		/// <summary>
		/// Initialise with known extra data.
		/// </summary>
		/// <param name="data">The extra data.</param>
		public ZipExtraData(byte[] data)
		{
			if ( data == null )
			{
				_data = new byte[0];
			}
			else
			{
				_data = data;
			}
		}
		#endregion


		/// <summary>
		/// Clear the stored data.
		/// </summary>
		public void Clear()
		{
			if ( (_data == null) || (_data.Length != 0) ) {
				_data = new byte[0];
			}
		}
		
		/// <summary>
		/// Get the length of the last value found by <see cref="Find"/>
		/// </summary>
		/// <remarks>This is only valid if <see cref="Find"/> has previously returned true.</remarks>
		public int ValueLength
		{
			get { return _readValueLength; }
		}

		/// <summary>
		/// Get the index for the current read value.
		/// </summary>
		/// <remarks>This is only valid if <see cref="Find"/> has previously returned true.
		/// Initially the result will be the index of the first byte of actual data.  The value is updated after calls to
		/// <see cref="ReadInt"/>, <see cref="ReadShort"/> and <see cref="ReadLong"/>. </remarks>
		public int CurrentReadIndex
		{
			get { return _index; }
		}

		/// <summary>
		/// Get the number of bytes remaining to be read for the current value;
		/// </summary>
		public int UnreadCount
		{
			get 
			{
                //if ((_readValueStart > _data.Length) ||
                //    (_readValueStart < 4) ) {
                //    throw new ZipException("Find must be called before calling a Read method");
                //}

				return _readValueStart + _readValueLength - _index; 
			}
		}

		/// <summary>
		/// Find an extra data value
		/// </summary>
		/// <param name="headerID">The identifier for the value to find.</param>
		/// <returns>Returns true if the value was found; false otherwise.</returns>
		public bool Find(int headerID)
		{
			_readValueStart = _data.Length;
			_readValueLength = 0;
			_index = 0;

			int localLength = _readValueStart;
			int localTag = headerID - 1;

			// Trailing bytes that cant make up an entry (as there arent enough
			// bytes for a tag and length) are ignored!
			while ( (localTag != headerID) && (_index < _data.Length - 3) ) {
				localTag = ReadShortInternal();
				localLength = ReadShortInternal();
				if ( localTag != headerID ) {
					_index += localLength;
				}
			}

			bool result = (localTag == headerID) && ((_index + localLength) <= _data.Length);

			if ( result ) {
				_readValueStart = _index;
				_readValueLength = localLength;
			}

			return result;
		}

		#region Reading Support
		/// <summary>
		/// Read a long in little endian form from the last <see cref="Find">found</see> data value
		/// </summary>
		/// <returns>Returns the long value read.</returns>
		public long ReadLong()
		{
			ReadCheck(8);
			return (ReadInt() & 0xffffffff) | ((( long )ReadInt()) << 32);
		}

		/// <summary>
		/// Read an integer in little endian form from the last <see cref="Find">found</see> data value.
		/// </summary>
		/// <returns>Returns the integer read.</returns>
		public int ReadInt()
		{
			ReadCheck(4);

			int result = _data[_index] + (_data[_index + 1] << 8) + 
				(_data[_index + 2] << 16) + (_data[_index + 3] << 24);
			_index += 4;
			return result;
		}

		/// <summary>
		/// Read a short value in little endian form from the last <see cref="Find">found</see> data value.
		/// </summary>
		/// <returns>Returns the short value read.</returns>
		public int ReadShort()
		{
			ReadCheck(2);
			int result = _data[_index] + (_data[_index + 1] << 8);
			_index += 2;
			return result;
		}

		/// <summary>
		/// Read a byte from an extra data
		/// </summary>
		/// <returns>The byte value read or -1 if the end of data has been reached.</returns>
		public int ReadByte()
		{
			int result = -1;
			if ( (_index < _data.Length) && (_readValueStart + _readValueLength > _index) ) {
				result = _data[_index];
				_index += 1;
			}
			return result;
		}

		/// <summary>
		/// Skip data during reading.
		/// </summary>
		/// <param name="amount">The number of bytes to skip.</param>
		public void Skip(int amount)
		{
			ReadCheck(amount);
			_index += amount;
		}

		void ReadCheck(int length)
		{
            //if ((_readValueStart > _data.Length) ||
            //    (_readValueStart < 4) ) {
            //    throw new ZipException("Find must be called before calling a Read method");
            //}

            //if (_index > _readValueStart + _readValueLength - length ) {
            //    throw new ZipException("End of extra data");
            //}

            //if ( _index + length < 4 ) {
            //    throw new ZipException("Cannot read before start of tag");
            //}
		}

		/// <summary>
		/// Internal form of <see cref="ReadShort"/> that reads data at any location.
		/// </summary>
		/// <returns>Returns the short value read.</returns>
		int ReadShortInternal()
		{
            //if ( _index > _data.Length - 2) {
            //    throw new ZipException("End of extra data");
            //}

			int result = _data[_index] + (_data[_index + 1] << 8);
			_index += 2;
			return result;
		}

		void SetShort(ref int index, int source)
		{
			_data[index] = (byte)source;
			_data[index + 1] = (byte)(source >> 8);
			index += 2;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Dispose of this instance.
		/// </summary>
		public void Dispose()
		{
            //if ( _newEntry != null ) {
            //    _newEntry.Close();
            //}
		}

		#endregion

		#region Instance Fields
		int _index;
		int _readValueStart;
		int _readValueLength;

		//MemoryStream _newEntry;
		byte[] _data;
		#endregion
	}
}
