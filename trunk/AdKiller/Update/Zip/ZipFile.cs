
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Globalization;

#if !NETCF_1_0
using System.Security.Cryptography;
#endif

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace ICSharpCode.SharpZipLib.Zip 
{
		
	#region ZipFile Class
	
	public class ZipFile : IEnumerable, IDisposable
	{
	
		#region Constructors
		/// <summary>
		/// Opens a Zip file with the given name for reading.
		/// </summary>
		/// <param name="name">The name of the file to open.</param>
		/// <exception cref="ArgumentNullException">The argument supplied is null.</exception>
		/// <exception cref="IOException">
		/// An i/o error occurs
		/// </exception>
		/// <exception cref="ZipException">
		/// The file doesn't contain a valid zip archive.
		/// </exception>
		public ZipFile(string name)
		{
			if ( name == null ) {
				throw new ArgumentNullException("name");
			}
			
			name_ = name;

			baseStream_ = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read);
			isStreamOwner = true;
			
			try {
				ReadEntries();
			}
			catch {
				DisposeInternal(true);
				throw;
			}
		}
		
		/// <summary>
		/// Opens a Zip file reading the given <see cref="FileStream"/>.
		/// </summary>
		/// <param name="file">The <see cref="FileStream"/> to read archive data from.</param>
		/// <exception cref="ArgumentNullException">The supplied argument is null.</exception>
		/// <exception cref="IOException">
		/// An i/o error occurs.
		/// </exception>
		/// <exception cref="ZipException">
		/// The file doesn't contain a valid zip archive.
		/// </exception>
		public ZipFile(FileStream file)
		{
			if ( file == null ) {
				throw new ArgumentNullException("file");
			}

            //if ( !file.CanSeek ) {
            //    throw new ArgumentException("Stream is not seekable", "file");
            //}

			baseStream_  = file;
			name_ = file.Name;
			isStreamOwner = true;
			
			try {
				ReadEntries();
			}
			catch {
				DisposeInternal(true);
				throw;
			}
		}
		
		/// <summary>
		/// Opens a Zip file reading the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read archive data from.</param>
		/// <exception cref="IOException">
		/// An i/o error occurs
		/// </exception>
		/// <exception cref="ZipException">
		/// The stream doesn't contain a valid zip archive.<br/>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The <see cref="Stream">stream</see> doesnt support seeking.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The <see cref="Stream">stream</see> argument is null.
		/// </exception>
		public ZipFile(Stream stream)
		{
			if ( stream == null ) {
				throw new ArgumentNullException("stream");
			}

			if ( !stream.CanSeek ) {
				throw new ArgumentException("Stream is not seekable", "stream");
			}

			baseStream_  = stream;
			isStreamOwner = true;
		
			if ( baseStream_.Length > 0 ) {
				try {
					ReadEntries();
				}
				catch {
					DisposeInternal(true);
					throw;
				}
			} else {
				entries_ = new ZipEntry[0];
				isNewArchive_ = true;
			}
		}

		/// <summary>
		/// Initialises a default <see cref="ZipFile"/> instance with no entries and no file storage.
		/// </summary>
		internal ZipFile()
		{
			entries_ = new ZipEntry[0];
			isNewArchive_ = true;
		}
		
		#endregion
		
		#region Destructors and Closing
		/// <summary>
		/// Finalize this instance.
		/// </summary>
		~ZipFile()
		{
			Dispose(false);
		}
		
		/// <summary>
		/// Closes the ZipFile.  If the stream is <see cref="IsStreamOwner">owned</see> then this also closes the underlying input stream.
		/// Once closed, no further instance methods should be called.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// An i/o error occurs.
		/// </exception>
		public void Close()
		{
			DisposeInternal(true);
			GC.SuppressFinalize(this);
		}
		
		#endregion
		

		
		#region Properties
		/// <summary>
		/// Get/set a flag indicating if the underlying stream is owned by the ZipFile instance.
		/// If the flag is true then the stream will be closed when <see cref="Close">Close</see> is called.
		/// </summary>
		/// <remarks>
		/// The default value is true in all cases.
		/// </remarks>
		public bool IsStreamOwner
		{
			get { return isStreamOwner; }
			set { isStreamOwner = value; }
		}
		
		/// <summary>
		/// Get a value indicating wether
		/// this archive is embedded in another file or not.
		/// </summary>
		public bool IsEmbeddedArchive
		{
			// Not strictly correct in all circumstances currently
			get { return offsetOfFirstEntry > 0; }
		}

		/// <summary>
		/// Get a value indicating that this archive is a new one.
		/// </summary>
		public bool IsNewArchive
		{
			get { return isNewArchive_; }
		}

		/// <summary>
		/// Gets the comment for the zip file.
		/// </summary>
		public string ZipFileComment 
		{
			get { return comment_; }
		}
		
		/// <summary>
		/// Gets the name of this zip file.
		/// </summary>
		public string Name 
		{
			get { return name_; }
		}
		
		/// <summary>
		/// Gets the number of entries in this zip file.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// The Zip file has been closed.
		/// </exception>
		[Obsolete("Use the Count property instead")]
		public int Size 
		{
			get 
			{
				return entries_.Length;
			}
		}
		
		/// <summary>
		/// Get the number of entries contained in this <see cref="ZipFile"/>.
		/// </summary>
		public long Count 
		{
			get 
			{
				return entries_.Length;
			}
		}
		
		/// <summary>
		/// Indexer property for ZipEntries
		/// </summary>
		[System.Runtime.CompilerServices.IndexerNameAttribute("EntryByIndex")]
		public ZipEntry this[int index] 
		{
			get {
				return (ZipEntry) entries_[index].Clone();	
			}
		}
		
		#endregion
		
		#region Input Handling
		/// <summary>
		/// Gets an enumerator for the Zip entries in this Zip file.
		/// </summary>
		/// <returns>Returns an <see cref="IEnumerator"/> for this archive.</returns>
		/// <exception cref="ObjectDisposedException">
		/// The Zip file has been closed.
		/// </exception>
		public IEnumerator GetEnumerator()
		{
			if (isDisposed_) {
				throw new ObjectDisposedException("ZipFile");
			}

			return new ZipEntryEnumerator(entries_);
		}
		
		/// <summary>
		/// Return the index of the entry with a matching name
		/// </summary>
		/// <param name="name">Entry name to find</param>
		/// <param name="ignoreCase">If true the comparison is case insensitive</param>
		/// <returns>The index position of the matching entry or -1 if not found</returns>
		/// <exception cref="ObjectDisposedException">
		/// The Zip file has been closed.
		/// </exception>
		public int FindEntry(string name, bool ignoreCase)
		{
			if (isDisposed_) {
				throw new ObjectDisposedException("ZipFile");
			}			
			
			// TODO: This will be slow as the next ice age for huge archives!
			for (int i = 0; i < entries_.Length; i++) {
				if (string.Compare(name, entries_[i].Name, ignoreCase, CultureInfo.InvariantCulture) == 0) {
					return i;
				}
			}
			return -1;
		}
		
		/// <summary>
		/// Searches for a zip entry in this archive with the given name.
		/// String comparisons are case insensitive
		/// </summary>
		/// <param name="name">
		/// The name to find. May contain directory components separated by slashes ('/').
		/// </param>
		/// <returns>
		/// A clone of the zip entry, or null if no entry with that name exists.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// The Zip file has been closed.
		/// </exception>
        //public ZipEntry GetEntry(string name)
        //{
        //    if (isDisposed_) {
        //        throw new ObjectDisposedException("ZipFile");
        //    }			
						
        //    int index = FindEntry(name, true);
        //    return (index >= 0) ? (ZipEntry) entries_[index].Clone() : null;
        //}

        ///// <summary>
        ///// Gets an input stream for reading the given zip entry data in an uncompressed form.
        ///// Normally the <see cref="ZipEntry"/> should be an entry returned by GetEntry().
        ///// </summary>
        ///// <param name="entry">The <see cref="ZipEntry"/> to obtain a data <see cref="Stream"/> for</param>
        ///// <returns>An input <see cref="Stream"/> containing data for this <see cref="ZipEntry"/></returns>
        ///// <exception cref="ObjectDisposedException">
        ///// The ZipFile has already been closed
        ///// </exception>
        ///// <exception cref="ICSharpCode.SharpZipLib.Zip.ZipException">
        ///// The compression method for the entry is unknown
        ///// </exception>
        ///// <exception cref="IndexOutOfRangeException">
        ///// The entry is not found in the ZipFile
        ///// </exception>
        public Stream GetInputStream(ZipEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            if (isDisposed_)
            {
                throw new ObjectDisposedException("ZipFile");
            }

            long index = entry.ZipFileIndex;
            if ((index < 0) || (index >= entries_.Length) || (entries_[index].Name != entry.Name))
            {
                index = FindEntry(entry.Name, true);
                //if (index < 0)
                //{
                //    throw new ZipException("Entry cannot be found");
                //}
            }
            return GetInputStream(index);
        }
		
		/// <summary>
		/// Creates an input stream reading a zip entry
		/// </summary>
		/// <param name="entryIndex">The index of the entry to obtain an input stream for.</param>
		/// <returns>
		/// An input <see cref="Stream"/> containing data for this <paramref name="entryIndex"/>
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// The ZipFile has already been closed
		/// </exception>
		/// <exception cref="ICSharpCode.SharpZipLib.Zip.ZipException">
		/// The compression method for the entry is unknown
		/// </exception>
		/// <exception cref="IndexOutOfRangeException">
		/// The entry is not found in the ZipFile
		/// </exception>
        public Stream GetInputStream(long entryIndex)
        {
            if (isDisposed_)
            {
                throw new ObjectDisposedException("ZipFile");
            }

            long start = LocateEntry(entries_[entryIndex]);
            CompressionMethod method = entries_[entryIndex].CompressionMethod;
            Stream result = new PartialInputStream(this, start, entries_[entryIndex].CompressedSize);

            switch (method)
            {
                case CompressionMethod.Stored:
                    // read as is.
                    break;

                case CompressionMethod.Deflated:
                    // No need to worry about ownership and closing as underlying stream close does nothing.
                    result = new InflaterInputStream(result, new Inflater(true));
                    break;

                //default:
                //    throw new ZipException("Unsupported compression method " + method);
            }

            return result;
        }

		#endregion
		
    
        enum HeaderTest
        {
            Extract = 0x01,     // Check that this header represents an entry whose data can be extracted
            Header = 0x02,     // Check that this header contents are valid
        }
	
        ///// <summary>
        ///// Test a local header against that provided from the central directory
        ///// </summary>
        ///// <param name="entry">
        ///// The entry to test against
        ///// </param>
        ///// <param name="tests">The type of <see cref="HeaderTest">tests</see> to carry out.</param>
        ///// <returns>The offset of the entries data in the file</returns>
        long TestLocalHeader(ZipEntry entry, HeaderTest tests)
        {
            lock (baseStream_)
            {
                bool testHeader = (tests & HeaderTest.Header) != 0;
                bool testData = (tests & HeaderTest.Extract) != 0;

                baseStream_.Seek(offsetOfFirstEntry + entry.Offset, SeekOrigin.Begin);
                if ((int)ReadLEUint() != ZipConstants.LocalHeaderSignature)
                {
                   // throw new ZipException(string.Format("Wrong local header signature @{0:X}", offsetOfFirstEntry + entry.Offset));
                }

                short extractVersion = (short)ReadLEUshort();
                short localFlags = (short)ReadLEUshort();
                short compressionMethod = (short)ReadLEUshort();
                short fileTime = (short)ReadLEUshort();
                short fileDate = (short)ReadLEUshort();
                uint crcValue = ReadLEUint();
                long compressedSize = ReadLEUint();
                long size = ReadLEUint();
                int storedNameLength = ReadLEUshort();
                int extraDataLength = ReadLEUshort();

                byte[] nameData = new byte[storedNameLength];
                StreamUtils.ReadFully(baseStream_, nameData);

                byte[] extraData = new byte[extraDataLength];
                StreamUtils.ReadFully(baseStream_, extraData);

                ZipExtraData localExtraData = new ZipExtraData(extraData);

                // Extra data / zip64 checks
                if (localExtraData.Find(1))
                {
                    size = localExtraData.ReadLong();
                    compressedSize = localExtraData.ReadLong();
                }
               
                int extraLength = storedNameLength + extraDataLength;
                return offsetOfFirstEntry + entry.Offset + ZipConstants.LocalHeaderBaseSize + extraLength;
            }
        }
		
        //#endregion
		
		#region Updating

		const int DefaultBufferSize = 4096;

		#endregion
		
		#region Disposing

		#region IDisposable Members
		void IDisposable.Dispose()
		{
			Close();
		}
		#endregion

		void DisposeInternal(bool disposing)
		{
			if ( !isDisposed_ ) {
				isDisposed_ = true;
				entries_ = new ZipEntry[0];
						
				if ( IsStreamOwner && (baseStream_ != null) ) {
					lock(baseStream_) {
						baseStream_.Close();
					}
				}
				
				//PostUpdateCleanup();
			}
		}

		/// <summary>
		/// Releases the unmanaged resources used by the this instance and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources;
		/// false to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			DisposeInternal(disposing);
		}

		#endregion
		
		#region Internal routines
		#region Reading
		/// <summary>
		/// Read an unsigned short in little endian byte order.
		/// </summary>
		/// <returns>Returns the value read.</returns>
		/// <exception cref="EndOfStreamException">
		/// The stream ends prematurely
		/// </exception>
		ushort ReadLEUshort()
		{
			int data1 = baseStream_.ReadByte();

			if ( data1 < 0 ) {
				throw new EndOfStreamException("End of stream");
			}

			int data2 = baseStream_.ReadByte();

			if ( data2 < 0 ) {
				throw new EndOfStreamException("End of stream");
			}


			return unchecked((ushort)((ushort)data1 | (ushort)(data2 << 8)));
		}

		/// <summary>
		/// Read a uint in little endian byte order.
		/// </summary>
		/// <returns>Returns the value read.</returns>
		/// <exception cref="IOException">
		/// An i/o error occurs.
		/// </exception>
		/// <exception cref="System.IO.EndOfStreamException">
		/// The file ends prematurely
		/// </exception>
		uint ReadLEUint()
		{
			return (uint)(ReadLEUshort() | (ReadLEUshort() << 16));
		}

		ulong ReadLEUlong()
		{
			return ReadLEUint() | ((ulong)ReadLEUint() << 32);
		}

		#endregion
		// NOTE this returns the offset of the first byte after the signature.
		long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
		{
			using ( ZipHelperStream les = new ZipHelperStream(baseStream_) ) {
				return les.LocateBlockWithSignature(signature, endLocation, minimumBlockSize, maximumVariableData);
			}
		}
		
		/// <summary>
		/// Search for and read the central directory of a zip file filling the entries array.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// An i/o error occurs.
		/// </exception>
		/// <exception cref="ICSharpCode.SharpZipLib.Zip.ZipException">
		/// The central directory is malformed or cannot be found
		/// </exception>
		void ReadEntries()
		{
			// Search for the End Of Central Directory.  When a zip comment is
			// present the directory will start earlier
			// 
			// The search is limited to 64K which is the maximum size of a trailing comment field to aid speed.
			// This should be compatible with both SFX and ZIP files but has only been tested for Zip files
			// If a SFX file has the Zip data attached as a resource and there are other resources occuring later then
			// this could be invalid.
			// Could also speed this up by reading memory in larger blocks.			

            //if (baseStream_.CanSeek == false) {
            //    throw new ZipException("ZipFile stream must be seekable");
            //}
			
			long locatedEndOfCentralDir = LocateBlockWithSignature(ZipConstants.EndOfCentralDirectorySignature,
				baseStream_.Length, ZipConstants.EndOfCentralRecordBaseSize, 0xffff);
			
            //if (locatedEndOfCentralDir < 0) {
            //    throw new ZipException("Cannot find central directory");
            //}

			// Read end of central directory record
			ushort thisDiskNumber           = ReadLEUshort();
			ushort startCentralDirDisk      = ReadLEUshort();
			ulong entriesForThisDisk        = ReadLEUshort();
			ulong entriesForWholeCentralDir = ReadLEUshort();
			ulong centralDirSize            = ReadLEUint();
			long offsetOfCentralDir         = ReadLEUint();
			uint commentSize                = ReadLEUshort();
			
			if ( commentSize > 0 ) {
				byte[] comment = new byte[commentSize]; 

				StreamUtils.ReadFully(baseStream_, comment);
				comment_ = ZipConstants.ConvertToString(comment); 
			}
			else {
				comment_ = string.Empty;
			}
			
			bool isZip64 = false;

			// Check if zip64 header information is required.
			if ( (thisDiskNumber == 0xffff) ||
				(startCentralDirDisk == 0xffff) ||
				(entriesForThisDisk == 0xffff) ||
				(entriesForWholeCentralDir == 0xffff) ||
				(centralDirSize == 0xffffffff) ||
				(offsetOfCentralDir == 0xffffffff) ) {
				isZip64 = true;

				long offset = LocateBlockWithSignature(ZipConstants.Zip64CentralDirLocatorSignature, locatedEndOfCentralDir, 0, 0x1000);
                //if ( offset < 0 ) {
                //    throw new ZipException("Cannot find Zip64 locator");
                //}

				// number of the disk with the start of the zip64 end of central directory 4 bytes 
				// relative offset of the zip64 end of central directory record 8 bytes 
				// total number of disks 4 bytes 
				ReadLEUint(); // startDisk64 is not currently used
				ulong offset64 = ReadLEUlong();
				uint totalDisks = ReadLEUint();

				baseStream_.Position = (long)offset64;
				long sig64 = ReadLEUint();

				if ( sig64 != ZipConstants.Zip64CentralFileHeaderSignature ) {
					//throw new ZipException(string.Format("Invalid Zip64 Central directory signature at {0:X}", offset64));
				}

				// NOTE: Record size = SizeOfFixedFields + SizeOfVariableData - 12.
				ulong recordSize = ReadLEUlong();
				int versionMadeBy = ReadLEUshort();
				int versionToExtract = ReadLEUshort();
				uint thisDisk = ReadLEUint();
				uint centralDirDisk = ReadLEUint();
				entriesForThisDisk = ReadLEUlong();
				entriesForWholeCentralDir = ReadLEUlong();
				centralDirSize = ReadLEUlong();
				offsetOfCentralDir = (long)ReadLEUlong();

				// NOTE: zip64 extensible data sector (variable size) is ignored.
			}
			
			entries_ = new ZipEntry[entriesForThisDisk];
			
			if ( !isZip64 && (offsetOfCentralDir < locatedEndOfCentralDir - (4 + (long)centralDirSize)) ) {
				offsetOfFirstEntry = locatedEndOfCentralDir - (4 + (long)centralDirSize + offsetOfCentralDir);
                //if (offsetOfFirstEntry <= 0) {
                //    throw new ZipException("Invalid embedded zip archive");
                //}
			}

			baseStream_.Seek(offsetOfFirstEntry + offsetOfCentralDir, SeekOrigin.Begin);
			
			for (ulong i = 0; i < entriesForThisDisk; i++) {
				if (ReadLEUint() != ZipConstants.CentralHeaderSignature) {
					//throw new ZipException("Wrong Central Directory signature");
				}
				
				int versionMadeBy      = ReadLEUshort();
				int versionToExtract   = ReadLEUshort();
				int bitFlags           = ReadLEUshort();
				int method             = ReadLEUshort();
				uint dostime           = ReadLEUint();
				uint crc               = ReadLEUint();
				long csize             = (long)ReadLEUint();
				long size              = (long)ReadLEUint();
				int nameLen            = ReadLEUshort();
				int extraLen           = ReadLEUshort();
				int commentLen         = ReadLEUshort();
				
				int diskStartNo        = ReadLEUshort();  // Not currently used
				int internalAttributes = ReadLEUshort();  // Not currently used

				uint externalAttributes = ReadLEUint();
				long offset             = ReadLEUint();
				
				byte[] buffer = new byte[Math.Max(nameLen, commentLen)];
				
				StreamUtils.ReadFully(baseStream_, buffer, 0, nameLen);
				string name = ZipConstants.ConvertToStringExt(bitFlags, buffer, nameLen);
				
				ZipEntry entry = new ZipEntry(name, versionToExtract, versionMadeBy, (CompressionMethod)method);
				entry.Crc = crc & 0xffffffffL;
				entry.Size = size & 0xffffffffL;
				entry.CompressedSize = csize & 0xffffffffL;
				entry.Flags = bitFlags;
				entry.DosTime = (uint)dostime;
				entry.ZipFileIndex = (long)i;
				entry.Offset = offset;
				entry.ExternalFileAttributes = (int)externalAttributes;

				if ((bitFlags & 8) == 0) {
					entry.CryptoCheckValue = (byte)(crc >> 24);
				}
				else {
					entry.CryptoCheckValue = (byte)((dostime >> 8) & 0xff);
				}

				if (extraLen > 0) {
					byte[] extra = new byte[extraLen];
					StreamUtils.ReadFully(baseStream_, extra);
					//entry.ExtraData = extra;
				}

				entry.ProcessExtraData(false);
				
				if (commentLen > 0) {
					StreamUtils.ReadFully(baseStream_, buffer, 0, commentLen);
					//entry.Comment = ZipConstants.ConvertToStringExt(bitFlags, buffer, commentLen);
				}
				
				entries_[i] = entry;
			}
		}

		/// <summary>
		/// Locate the data for a given entry.
		/// </summary>
		/// <returns>
		/// The start offset of the data.
		/// </returns>
		/// <exception cref="System.IO.EndOfStreamException">
		/// The stream ends prematurely
		/// </exception>
		/// <exception cref="ICSharpCode.SharpZipLib.Zip.ZipException">
		/// The local header signature is invalid, the entry and central header file name lengths are different
		/// or the local and entry compression methods dont match
		/// </exception>
        long LocateEntry(ZipEntry entry)
        {
            return TestLocalHeader(entry, HeaderTest.Extract);
        }
		
	

		#endregion
		
		#region Instance Fields
		bool       isDisposed_;
		string     name_;
		string     comment_;
		Stream     baseStream_;
		bool       isStreamOwner;
		long       offsetOfFirstEntry;
		ZipEntry[] entries_;
		//byte[] key;
		bool isNewArchive_;
		
		#endregion
		
		#region Support Classes

		
		/// <summary>
		/// An <see cref="IEnumerator">enumerator</see> for <see cref="ZipEntry">Zip entries</see>
		/// </summary>
		class ZipEntryEnumerator : IEnumerator
		{
			#region Constructors
			public ZipEntryEnumerator(ZipEntry[] entries)
			{
				array = entries;
			}
			
			#endregion
			#region IEnumerator Members
			public object Current 
			{
				get {
					return array[index];
				}
			}
			
			public void Reset()
			{
				index = -1;
			}
			
			public bool MoveNext() 
			{
				return (++index < array.Length);
			}
			#endregion
			#region Instance Fields
			ZipEntry[] array;
			int index = -1;
			#endregion
		}
		
		/// <summary>
		/// A <see cref="PartialInputStream"/> is an <see cref="InflaterInputStream"/>
		/// whose data is only a part or subsection of a file.
		/// </summary>
		class PartialInputStream : Stream
		{
			#region Constructors
			/// <summary>
			/// Initialise a new instance of the <see cref="PartialInputStream"/> class.
			/// </summary>
			/// <param name="zipFile">The <see cref="ZipFile"/> containing the underlying stream to use for IO.</param>
			/// <param name="start">The start of the partial data.</param>
			/// <param name="length">The length of the partial data.</param>
			public PartialInputStream(ZipFile zipFile, long start, long length)
			{
				start_ = start;
				length_ = length;

				// Although this is the only time the zipfile is used
				// keeping a reference here prevents premature closure of
				// this zip file and thus the baseStream_.

				// Code like this will cause apparently random failures depending
				// on the size of the files and when garbage is collected.
				//
				// ZipFile z = new ZipFile (stream);
				// Stream reader = z.GetInputStream(0);
				// uses reader here....
				zipFile_ = zipFile;
				baseStream_ = zipFile_.baseStream_;
				readPos_ = start;
				end_ = start + length;
			}
			#endregion

			/// <summary>
			/// Read a byte from this stream.
			/// </summary>
			/// <returns>Returns the byte read or -1 on end of stream.</returns>
			public override int ReadByte()
			{
				if (readPos_ >= end_) {
					 // -1 is the correct value at end of stream.
					return -1;
				}
				
				lock( baseStream_ ) {
					baseStream_.Seek(readPos_++, SeekOrigin.Begin);
					return baseStream_.ReadByte();
				}
			}
			
			/// <summary>
			/// Close this <see cref="PartialInputStream">partial input stream</see>.
			/// </summary>
			/// <remarks>
			/// The underlying stream is not closed.  Close the parent ZipFile class to do that.
			/// </remarks>
			public override void Close()
			{
				// Do nothing at all!
			}

			/// <summary>
			/// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
			/// </summary>
			/// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
			/// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
			/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
			/// <returns>
			/// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
			/// </returns>
			/// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
			/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
			/// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
			/// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
			/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
			public override int Read(byte[] buffer, int offset, int count)
			{
				lock(baseStream_) {
					if (count > end_ - readPos_) {
						count = (int) (end_ - readPos_);
						if (count == 0) {
							return 0;
						}
					}
					
					baseStream_.Seek(readPos_, SeekOrigin.Begin);
					int readCount = baseStream_.Read(buffer, offset, count);
					if (readCount > 0) {
						readPos_ += readCount;
					}
					return readCount;
				}
			}

			/// <summary>
			/// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
			/// </summary>
			/// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
			/// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
			/// <param name="count">The number of bytes to be written to the current stream.</param>
			/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
			/// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
			/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
			/// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
			/// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}

			/// <summary>
			/// When overridden in a derived class, sets the length of the current stream.
			/// </summary>
			/// <param name="value">The desired length of the current stream in bytes.</param>
			/// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
			/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
			/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			/// <summary>
			/// When overridden in a derived class, sets the position within the current stream.
			/// </summary>
			/// <param name="offset">A byte offset relative to the origin parameter.</param>
			/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference point used to obtain the new position.</param>
			/// <returns>
			/// The new position within the current stream.
			/// </returns>
			/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
			/// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
			/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
			public override long Seek(long offset, SeekOrigin origin)
			{
				long newPos = readPos_;
				
				switch ( origin )
				{
					case SeekOrigin.Begin:
						newPos = start_ + offset;
						break;
						
					case SeekOrigin.Current:
						newPos = readPos_ + offset;
						break;
						
					case SeekOrigin.End:
						newPos = end_ + offset;
						break;
				}
				
				if ( newPos < start_ ) {
					throw new ArgumentException("Negative position is invalid");
				}
				
				if ( newPos >= end_ ) {
					throw new IOException("Cannot seek past end");
				}
				readPos_ = newPos;
				return readPos_;
			}

			/// <summary>
			/// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
			/// </summary>
			/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
			public override void Flush()
			{
				// Nothing to do.
			}

			/// <summary>
			/// Gets or sets the position within the current stream.
			/// </summary>
			/// <value></value>
			/// <returns>The current position within the stream.</returns>
			/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
			/// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
			/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
			public override long Position {
				get { return readPos_ - start_; }
				set { 
					long newPos = start_ + value;
					
					if ( newPos < start_ ) {
						throw new ArgumentException("Negative position is invalid");
					}
					
					if ( newPos >= end_ ) {
						throw new InvalidOperationException("Cannot seek past end");
					}
					readPos_ = newPos;
				}
			}

			/// <summary>
			/// Gets the length in bytes of the stream.
			/// </summary>
			/// <value></value>
			/// <returns>A long value representing the length of the stream in bytes.</returns>
			/// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
			/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
			public override long Length {
				get { return length_; }
			}

			/// <summary>
			/// Gets a value indicating whether the current stream supports writing.
			/// </summary>
			/// <value>false</value>
			/// <returns>true if the stream supports writing; otherwise, false.</returns>
			public override bool CanWrite {
				get { return false; }
			}

			/// <summary>
			/// Gets a value indicating whether the current stream supports seeking.
			/// </summary>
			/// <value>true</value>
			/// <returns>true if the stream supports seeking; otherwise, false.</returns>
			public override bool CanSeek {
				get { return true; }
			}

			/// <summary>
			/// Gets a value indicating whether the current stream supports reading.
			/// </summary>
			/// <value>true.</value>
			/// <returns>true if the stream supports reading; otherwise, false.</returns>
			public override bool CanRead {
				get { return true; }
			}
			
#if !NET_1_0 && !NET_1_1 && !NETCF_1_0
			/// <summary>
			/// Gets a value that determines whether the current stream can time out.
			/// </summary>
			/// <value></value>
			/// <returns>A value that determines whether the current stream can time out.</returns>
			public override bool CanTimeout {
				get { return baseStream_.CanTimeout; }
			}
#endif			
			#region Instance Fields
			ZipFile zipFile_;
			Stream baseStream_;
			long start_;
			long length_;
			long readPos_;
			long end_;
			#endregion	
		}
		#endregion
	}

	#endregion
	
}
