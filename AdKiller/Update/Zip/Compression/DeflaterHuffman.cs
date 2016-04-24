

using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{

    public class DeflaterHuffman
    {

        static readonly byte[] bit4Reverse = {
            0,
            8,
            4,
            12,
            2,
            10,
            6,
            14,
            1,
            9,
            5,
            13,
            3,
            11,
            7,
            15
        };

        public static short BitReverse(int toReverse)
        {
            return (short)(bit4Reverse[toReverse & 0xF] << 12 |
                            bit4Reverse[(toReverse >> 4) & 0xF] << 8 |
                            bit4Reverse[(toReverse >> 8) & 0xF] << 4 |
                            bit4Reverse[toReverse >> 12]);
        }

    }
}
