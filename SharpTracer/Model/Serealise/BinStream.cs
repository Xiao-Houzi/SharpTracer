﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Model
{
	class BinStream
	{
		public static byte[] FromFileStream(FileStream stream)
		{
			long initialLength;
			// If we've been passed an unhelpful initial length, just
			// use 32K.
			if (stream.Length > 0)
			{
				initialLength = stream.Length;
			}
			else return new byte[0];

			byte[] buffer = new byte[initialLength];
			int read = 0;

			int chunk;
			while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
			{
				read += chunk;

				// If we've reached the end of our buffer, check to see if there's
				// any more information
				if (read == buffer.Length)
				{
					int nextByte = stream.ReadByte();

					// End of stream? If so, we're done
					if (nextByte == -1)
					{
						return buffer;
					}

					// Nope. Resize the buffer, put in the byte we've just
					// read, and continue
					byte[] newBuffer = new byte[buffer.Length * 2];
					Array.Copy(buffer, newBuffer, buffer.Length);
					newBuffer[read] = (byte)nextByte;
					buffer = newBuffer;
					read++;
				}
			}
			// Buffer is now too big. Shrink it.
			byte[] ret = new byte[read];
			Array.Copy(buffer, ret, read);
			return ret;
		}
	}
}
