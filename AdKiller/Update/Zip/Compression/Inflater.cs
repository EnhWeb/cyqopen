

using System;

//using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    public class Inflater
    {
        #region Constants/Readonly
        /// <summary>
        /// Copy lengths for literal codes 257..285
        /// </summary>
        static readonly int[] CPLENS = {
								  3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 15, 17, 19, 23, 27, 31,
								  35, 43, 51, 59, 67, 83, 99, 115, 131, 163, 195, 227, 258
							  };

        /// <summary>
        /// Extra bits for literal codes 257..285
        /// </summary>
        static readonly int[] CPLEXT = {
								  0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2,
								  3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 0
							  };

        /// <summary>
        /// Copy offsets for distance codes 0..29
        /// </summary>
        static readonly int[] CPDIST = {
								1, 2, 3, 4, 5, 7, 9, 13, 17, 25, 33, 49, 65, 97, 129, 193,
								257, 385, 513, 769, 1025, 1537, 2049, 3073, 4097, 6145,
								8193, 12289, 16385, 24577
							  };

        /// <summary>
        /// Extra bits for distance codes
        /// </summary>
        static readonly int[] CPDEXT = {
								0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6,
								7, 7, 8, 8, 9, 9, 10, 10, 11, 11,
								12, 12, 13, 13
							  };

        /// <summary>
        /// These are the possible states for an inflater
        /// </summary>
        const int DECODE_HEADER = 0;
        const int DECODE_DICT = 1;
        const int DECODE_BLOCKS = 2;
        const int DECODE_STORED_LEN1 = 3;
        const int DECODE_STORED_LEN2 = 4;
        const int DECODE_STORED = 5;
        const int DECODE_DYN_HEADER = 6;
        const int DECODE_HUFFMAN = 7;
        const int DECODE_HUFFMAN_LENBITS = 8;
        const int DECODE_HUFFMAN_DIST = 9;
        const int DECODE_HUFFMAN_DISTBITS = 10;
        const int DECODE_CHKSUM = 11;
        const int FINISHED = 12;
        #endregion

        #region Instance Fields
        /// <summary>
        /// This variable contains the current state.
        /// </summary>
        int mode;

        /// <summary>
        /// The adler checksum of the dictionary or of the decompressed
        /// stream, as it is written in the header resp. footer of the
        /// compressed stream. 
        /// Only valid if mode is DECODE_DICT or DECODE_CHKSUM.
        /// </summary>
        int readAdler;

        /// <summary>
        /// The number of bits needed to complete the current state.  This
        /// is valid, if mode is DECODE_DICT, DECODE_CHKSUM,
        /// DECODE_HUFFMAN_LENBITS or DECODE_HUFFMAN_DISTBITS.
        /// </summary>
        int neededBits;
        int repLength;
        int repDist;
        int uncomprLen;

        /// <summary>
        /// True, if the last block flag was set in the last block of the
        /// inflated stream.  This means that the stream ends after the
        /// current block.
        /// </summary>
        bool isLastBlock;

        /// <summary>
        /// The total number of inflated bytes.
        /// </summary>
        long totalOut;

        /// <summary>
        /// The total number of bytes set with setInput().  This is not the
        /// value returned by the TotalIn property, since this also includes the
        /// unprocessed input.
        /// </summary>
        long totalIn;

        /// <summary>
        /// This variable stores the noHeader flag that was given to the constructor.
        /// True means, that the inflated stream doesn't contain a Zlib header or 
        /// footer.
        /// </summary>
        bool noHeader;

        StreamManipulator input;
        OutputWindow outputWindow;
        InflaterDynHeader dynHeader;
        InflaterHuffmanTree litlenTree, distTree;
        //Adler32 adler;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new inflater or RFC1951 decompressor
        /// RFC1950/Zlib headers and footers will be expected in the input data
        /// </summary>
        public Inflater()
            : this(false)
        {
        }

        /// <summary>
        /// Creates a new inflater.
        /// </summary>
        /// <param name="noHeader">
        /// True if no RFC1950/Zlib header and footer fields are expected in the input data
        /// 
        /// This is used for GZIPed/Zipped input.
        /// 
        /// For compatibility with
        /// Sun JDK you should provide one byte of input more than needed in
        /// this case.
        /// </param>
        public Inflater(bool noHeader)
        {
            this.noHeader = noHeader;
           // this.adler = new Adler32();
            input = new StreamManipulator();
            outputWindow = new OutputWindow();
            mode = noHeader ? DECODE_BLOCKS : DECODE_HEADER;
        }
        #endregion

        /// <summary>
        /// Resets the inflater so that a new stream can be decompressed.  All
        /// pending input and output will be discarded.
        /// </summary>
        public void Reset()
        {
            mode = noHeader ? DECODE_BLOCKS : DECODE_HEADER;
            totalIn = 0;
            totalOut = 0;
            input.Reset();
            outputWindow.Reset();
            dynHeader = null;
            litlenTree = null;
            distTree = null;
            isLastBlock = false;
           // adler.Reset();
        }

        /// <summary>
        /// Decodes a zlib/RFC1950 header.
        /// </summary>
        /// <returns>
        /// False if more input is needed.
        /// </returns>
        /// <exception cref="SharpZipBaseException">
        /// The header is invalid.
        /// </exception>
        private bool DecodeHeader()
        {
            int header = input.PeekBits(16);
            if (header < 0)
            {
                return false;
            }
            input.DropBits(16);

            // The header is written in "wrong" byte order
            header = ((header << 8) | (header >> 8)) & 0xffff;
            //if (header % 31 != 0)
            //{
            //    throw new SharpZipBaseException("Header checksum illegal");
            //}

            //if ((header & 0x0f00) != (Deflater.DEFLATED << 8))
            //{
            //    throw new SharpZipBaseException("Compression Method unknown");
            //}
            if ((header & 0x0020) == 0)
            { // Dictionary flag?
                mode = DECODE_BLOCKS;
            }
            else
            {
                mode = DECODE_DICT;
                neededBits = 32;
            }
            return true;
        }

        /// <summary>
        /// Decodes the dictionary checksum after the deflate header.
        /// </summary>
        /// <returns>
        /// False if more input is needed.
        /// </returns>
        private bool DecodeDict()
        {
            while (neededBits > 0)
            {
                int dictByte = input.PeekBits(8);
                if (dictByte < 0)
                {
                    return false;
                }
                input.DropBits(8);
                readAdler = (readAdler << 8) | dictByte;
                neededBits -= 8;
            }
            return false;
        }

        /// <summary>
        /// Decodes the huffman encoded symbols in the input stream.
        /// </summary>
        /// <returns>
        /// false if more input is needed, true if output window is
        /// full or the current block ends.
        /// </returns>
        /// <exception cref="SharpZipBaseException">
        /// if deflated stream is invalid.
        /// </exception>
        private bool DecodeHuffman()
        {
            int free = outputWindow.GetFreeSpace();
            while (free >= 258)
            {
                int symbol;
                switch (mode)
                {
                    case DECODE_HUFFMAN:
                        // This is the inner loop so it is optimized a bit
                        while (((symbol = litlenTree.GetSymbol(input)) & ~0xff) == 0)
                        {
                            outputWindow.Write(symbol);
                            if (--free < 258)
                            {
                                return true;
                            }
                        }

                        if (symbol < 257)
                        {
                            if (symbol < 0)
                            {
                                return false;
                            }
                            else
                            {
                                // symbol == 256: end of block
                                distTree = null;
                                litlenTree = null;
                                mode = DECODE_BLOCKS;
                                return true;
                            }
                        }

                        try
                        {
                            repLength = CPLENS[symbol - 257];
                            neededBits = CPLEXT[symbol - 257];
                        }
                        catch (Exception)
                        {
                            throw new SharpZipBaseException("Illegal rep length code");
                        }
                        goto case DECODE_HUFFMAN_LENBITS; // fall through

                    case DECODE_HUFFMAN_LENBITS:
                        if (neededBits > 0)
                        {
                            mode = DECODE_HUFFMAN_LENBITS;
                            int i = input.PeekBits(neededBits);
                            if (i < 0)
                            {
                                return false;
                            }
                            input.DropBits(neededBits);
                            repLength += i;
                        }
                        mode = DECODE_HUFFMAN_DIST;
                        goto case DECODE_HUFFMAN_DIST; // fall through

                    case DECODE_HUFFMAN_DIST:
                        symbol = distTree.GetSymbol(input);
                        if (symbol < 0)
                        {
                            return false;
                        }

                        try
                        {
                            repDist = CPDIST[symbol];
                            neededBits = CPDEXT[symbol];
                        }
                        catch (Exception)
                        {
                            throw new SharpZipBaseException("Illegal rep dist code");
                        }

                        goto case DECODE_HUFFMAN_DISTBITS; // fall through

                    case DECODE_HUFFMAN_DISTBITS:
                        if (neededBits > 0)
                        {
                            mode = DECODE_HUFFMAN_DISTBITS;
                            int i = input.PeekBits(neededBits);
                            if (i < 0)
                            {
                                return false;
                            }
                            input.DropBits(neededBits);
                            repDist += i;
                        }

                        outputWindow.Repeat(repLength, repDist);
                        free -= repLength;
                        mode = DECODE_HUFFMAN;
                        break;

                    default:
                        throw new SharpZipBaseException("Inflater unknown mode");
                }
            }
            return true;
        }

        /// <summary>
        /// Decodes the adler checksum after the deflate stream.
        /// </summary>
        /// <returns>
        /// false if more input is needed.
        /// </returns>
        /// <exception cref="SharpZipBaseException">
        /// If checksum doesn't match.
        /// </exception>
        private bool DecodeChksum()
        {
            while (neededBits > 0)
            {
                int chkByte = input.PeekBits(8);
                if (chkByte < 0)
                {
                    return false;
                }
                input.DropBits(8);
                readAdler = (readAdler << 8) | chkByte;
                neededBits -= 8;
            }

            //if ((int)adler.Value != readAdler)
            //{
            //    throw new SharpZipBaseException("Adler chksum doesn't match: " + (int)adler.Value + " vs. " + readAdler);
            //}

            mode = FINISHED;
            return false;
        }

        /// <summary>
        /// Decodes the deflated stream.
        /// </summary>
        /// <returns>
        /// false if more input is needed, or if finished.
        /// </returns>
        /// <exception cref="SharpZipBaseException">
        /// if deflated stream is invalid.
        /// </exception>
        private bool Decode()
        {
            switch (mode)
            {
                case DECODE_HEADER:
                    return DecodeHeader();

                case DECODE_DICT:
                    return DecodeDict();

                case DECODE_CHKSUM:
                    return DecodeChksum();

                case DECODE_BLOCKS:
                    if (isLastBlock)
                    {
                        if (noHeader)
                        {
                            mode = FINISHED;
                            return false;
                        }
                        else
                        {
                            input.SkipToByteBoundary();
                            neededBits = 32;
                            mode = DECODE_CHKSUM;
                            return true;
                        }
                    }

                    int type = input.PeekBits(3);
                    if (type < 0)
                    {
                        return false;
                    }
                    input.DropBits(3);

                    if ((type & 1) != 0)
                    {
                        isLastBlock = true;
                    }
                    switch (type >> 1)
                    {
                        case DeflaterConstants.STORED_BLOCK:
                            input.SkipToByteBoundary();
                            mode = DECODE_STORED_LEN1;
                            break;
                        case DeflaterConstants.STATIC_TREES:
                            litlenTree = InflaterHuffmanTree.defLitLenTree;
                            distTree = InflaterHuffmanTree.defDistTree;
                            mode = DECODE_HUFFMAN;
                            break;
                        case DeflaterConstants.DYN_TREES:
                            dynHeader = new InflaterDynHeader();
                            mode = DECODE_DYN_HEADER;
                            break;
                        default:
                            throw new SharpZipBaseException("Unknown block type " + type);
                    }
                    return true;

                case DECODE_STORED_LEN1:
                    {
                        if ((uncomprLen = input.PeekBits(16)) < 0)
                        {
                            return false;
                        }
                        input.DropBits(16);
                        mode = DECODE_STORED_LEN2;
                    }
                    goto case DECODE_STORED_LEN2; // fall through

                case DECODE_STORED_LEN2:
                    {
                        int nlen = input.PeekBits(16);
                        if (nlen < 0)
                        {
                            return false;
                        }
                        input.DropBits(16);
                        mode = DECODE_STORED;
                    }
                    goto case DECODE_STORED; // fall through

                case DECODE_STORED:
                    {
                        int more = outputWindow.CopyStored(input, uncomprLen);
                        uncomprLen -= more;
                        if (uncomprLen == 0)
                        {
                            mode = DECODE_BLOCKS;
                            return true;
                        }
                        return !input.IsNeedingInput;
                    }

                case DECODE_DYN_HEADER:
                    if (!dynHeader.Decode(input))
                    {
                        return false;
                    }

                    litlenTree = dynHeader.BuildLitLenTree();
                    distTree = dynHeader.BuildDistTree();
                    mode = DECODE_HUFFMAN;
                    goto case DECODE_HUFFMAN; // fall through

                case DECODE_HUFFMAN:
                case DECODE_HUFFMAN_LENBITS:
                case DECODE_HUFFMAN_DIST:
                case DECODE_HUFFMAN_DISTBITS:
                    return DecodeHuffman();

                case FINISHED:
                    return false;

                default:
                    throw new SharpZipBaseException("Inflater.Decode unknown mode");
            }
        }

        public void SetInput(byte[] buffer, int index, int count)
        {
            input.SetInput(buffer, index, count);
            totalIn += (long)count;
        }

        //ok
        public int Inflate(byte[] buffer, int offset, int count)
        {
            // Special case: count may be zero
            if (count == 0)
            {
                if (!IsFinished)
                { // -jr- 08-Nov-2003 INFLATE_BUG fix..
                    Decode();
                }
                return 0;
            }

            int bytesCopied = 0;

            do
            {
                if (mode != DECODE_CHKSUM)
                {
                    int more = outputWindow.CopyOutput(buffer, offset, count);
                    if (more > 0)
                    {
                       // adler.Update(buffer, offset, more);
                        offset += more;
                        bytesCopied += more;
                        totalOut += (long)more;
                        count -= more;
                        if (count == 0)
                        {
                            return bytesCopied;
                        }
                    }
                }
            } while (Decode() || ((outputWindow.GetAvailable() > 0) && (mode != DECODE_CHKSUM)));
            return bytesCopied;
        }

        /// <summary>
        /// Returns true, if the input buffer is empty.
        /// You should then call setInput(). 
        /// NOTE: This method also returns true when the stream is finished.
        /// </summary>
        public bool IsNeedingInput
        {
            get
            {
                return input.IsNeedingInput;
            }
        }

        /// <summary>
        /// Returns true, if a preset dictionary is needed to inflate the input.
        /// </summary>
        public bool IsNeedingDictionary
        {
            get
            {
                return mode == DECODE_DICT && neededBits == 0;
            }
        }

        /// <summary>
        /// Returns true, if the inflater has finished.  This means, that no
        /// input is needed and no output can be produced.
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return mode == FINISHED && outputWindow.GetAvailable() == 0;
            }
        }

    }
}
