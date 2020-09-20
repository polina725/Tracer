﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tracer
{
    class Displayer
    {
        public void Display(Stream outputStream,string result)
        {
            outputStream.Write(Encoding.UTF8.GetBytes(result), 0, result.Length);
            outputStream.Flush();
            outputStream.Close();
        }

        public void Display(TextWriter outputWriter,string result)
        {
            outputWriter.Write(result);
        }
    }
}
