// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.IO.Compression;

namespace Reaqtor.Remoting.QueryEvaluator
{
    //
    // CONSIDER: We can add some more choices here, including compression level, which can also enable the use of GZIP without
    //           compression in order to gain its CRC capability to detect data corruption.
    //
    //           At this point, it has been shown that compression isn't very effective for checkpoint state, which is rather
    //           fine-grained (in order to make differential checkpoints small). It also adds non-trivial CPU cost in the hot
    //           path of checkpointing. However, we may want to reconsider this in conjunction with other Stream transformers
    //           such as encryption for compliance environments (where we have implemented such a scheme).
    //
    //           Due to the parameterizing of the engine on a store, we don't need to (nor do we want to) push down this
    //           functionality to the core assemblies, but we may still want to add functionality to the framework, so users
    //           can compose it. They payload format with envelopes can be standardized as well, including for transport of
    //           events and/or Bonsai trees. (This file just has the header for compression; see experiments in Pearls for an
    //           implementation of stream transformers with "tags" that allow the "decoder" to automatically recreate the
    //           inverse function, e.g. "decompress using GZIP, then decrypt, then run checksum validation".
    //

    /// <summary>
    /// Compression algorithm.
    /// </summary>
    internal enum Compression
    {
        /// <summary>
        /// No compression used.
        /// </summary>
        None,

        /// <summary>
        /// GZip compression, recommended for stores that don't have CRC checks and/or support inspecting data in the GZIP data format.
        /// </summary>
        GZip,

        /// <summary>
        /// Deflate compression.
        /// </summary>
        Deflate,
    }

    /// <summary>
    /// Utilities for compression of state stored in KVS.
    /// </summary>
    internal static class CompressionUtils
    {
        /// <summary>
        /// Length of compression headers.
        /// </summary>
        private const int HeaderLength = 4;

        /// <summary>
        /// Compresses the buffer using the specified compression algorithm.
        /// </summary>
        /// <param name="buffer">Buffer to compress.</param>
        /// <param name="compression">Compression algorithm to use.</param>
        /// <returns>Compressed buffer.</returns>
        public static byte[] Compress(byte[] buffer, Compression compression)
        {
            if (compression == Compression.None)
            {
                return buffer;
            }

            using var res = new MemoryStream();

            WriteHeader(res, compression);

            using (var zip = GetStream(compression, CompressionMode.Compress, res))
            {
                zip.Write(buffer, 0, buffer.Length);
                zip.Flush();
            }

            return res.ToArray();
        }

        /// <summary>
        /// Decompresses the specified buffer.
        /// </summary>
        /// <param name="buffer">Buffer to decompress.</param>
        /// <returns>Decompressed buffer.</returns>
        public static byte[] Decompress(byte[] buffer)
        {
            if (buffer.Length >= HeaderLength)
            {
                var compression = ReadHeader(buffer);

                if (compression != Compression.None)
                {
                    using var inp = new MemoryStream(buffer, HeaderLength, buffer.Length - HeaderLength);

                    using var res = new MemoryStream();

                    using (var zip = GetStream(compression, CompressionMode.Decompress, inp))
                    {
                        zip.CopyTo(res);
                        zip.Flush();
                    }

                    return res.ToArray();
                }
            }

            return buffer;
        }

        /// <summary>
        /// Writes the header indicating the compression algorithm.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="compression">Compression algorithm to write the header for.</param>
        private static void WriteHeader(Stream stream, Compression compression)
        {
            switch (compression)
            {
                case Compression.GZip:
                    stream.WriteByte((byte)'G');
                    stream.WriteByte((byte)'Z');
                    stream.WriteByte((byte)'I');
                    stream.WriteByte((byte)'P');
                    break;
                case Compression.Deflate:
                    stream.WriteByte((byte)'D');
                    stream.WriteByte((byte)'E');
                    stream.WriteByte((byte)'F');
                    stream.WriteByte((byte)'L');
                    break;
            }
        }

        /// <summary>
        /// Reads the header indicating the compression algorithm.
        /// </summary>
        /// <param name="buffer">Buffer to read the header from.</param>
        /// <returns>Compression algorithm as indicated by the header.</returns>
        private static Compression ReadHeader(byte[] buffer)
        {
            if (buffer.Length >= 4)
            {
                var h1 = (char)buffer[0];
                var h2 = (char)buffer[1];
                var h3 = (char)buffer[2];
                var h4 = (char)buffer[3];

                if (h1 == 'G' && h2 == 'Z' && h3 == 'I' && h4 == 'P')
                {
                    return Compression.GZip;
                }

                if (h1 == 'D' && h2 == 'E' && h3 == 'F' && h4 == 'L')
                {
                    return Compression.Deflate;
                }
            }

            return Compression.None;
        }

        /// <summary>
        /// Gets a compression stream.
        /// </summary>
        /// <param name="compression">The type of compression to use.</param>
        /// <param name="mode">The compression mode.</param>
        /// <param name="stream">The underlying stream to read from or write to.</param>
        /// <returns>A compression stream.</returns>
        private static Stream GetStream(Compression compression, CompressionMode mode, Stream stream) => compression switch
        {
            Compression.GZip => new GZipStream(stream, mode),
            Compression.Deflate => new DeflateStream(stream, mode),
            _ => throw new NotSupportedException("Unknown compression method."),
        };
    }
}
