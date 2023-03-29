// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using Mercury.Language.Exceptions;
using Mercury.Language.Log;
using Mercury.Language.Extensions;

namespace System.IO
{
    /// <summary>
    /// PrintStream Description
    /// https://www.c-sharpcorner.com/UploadFile/87ad51/printing-from-a-windows-service/
    /// https://stackoverflow.com/questions/44979794/how-to-print-a-long-string-into-multiple-pages-in-c-sharp
    /// </summary>
    public class PrintStream : Stream
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        private String _printerName;
        private Font _printFont;
        private Margins _margins;
        private RectangleF _rectangleF;
        private SizeF _layoutSize;
        private Brush _brush;
        private PageSettings _pageSettings;
        private PrintDocument _printDoc;
        private StringBuilder remainingText = new StringBuilder();

        //private Stream IOStream;

        #region Unused Implemented Properties

        public override bool CanRead => throw new NotSupportedException();

        public override bool CanSeek => throw new NotSupportedException();

        public override bool CanWrite => throw new NotSupportedException();

        public override long Length => throw new NotSupportedException();

        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
#endregion

        public String PrinterName
        {
            get { return _printerName; }
            set {
                SetDefaultPrinter(value);
                _printerName = value;
            }
        }

        public Font PrintFont
        {
            get { return _printFont; }
            set { _printFont = value; }
        }

        public Margins PrintMargins
        {
            get { return _margins; }
            set { _margins = value; }
        }

        public PageSettings PrintPageSettings
        {
            get { return _pageSettings; }
            set { _pageSettings = value; }
        }

        public Brush PrintBrush
        {
            get { return _brush; }
            set { _brush = value; }
        }

        public PrintStream():this (new PrinterSettings().PrinterName, new Font("Arial", 10), new PrintDocument().DefaultPageSettings.Margins, new System.Drawing.SolidBrush(System.Drawing.Color.Black), new PrintDocument().DefaultPageSettings)
        {
        }

        public PrintStream(String printerName, Font printFont, Margins printMargins, Brush printBrush, PageSettings pageSettings)
        {
            PrinterName = printerName;
            PrintFont = printFont;
            PrintMargins = printMargins;
            PrintBrush = printBrush;
            PrintPageSettings = pageSettings;

            _printDoc = new PrintDocument();

            SetLayoutArea();

            _printDoc.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
            {
                int charsFitted, linesFilled;

                // measure how many characters will fit of the remaining text
                var realsize = e1.Graphics.MeasureString(
                    remainingText.ToString(),
                    _printFont,
                    _layoutSize,
                    StringFormat.GenericDefault,
                    out charsFitted,  // this will return what we need
                    out linesFilled);

                // take from the remainingText what we're going to print on this page
                var fitsOnPage = remainingText.ToString().Substring(0, charsFitted);
                // keep what is not printed on this page 
                string buf = remainingText.ToString().Substring(charsFitted).Trim();
                remainingText.Clear();
                remainingText.Append(buf);

                // print what fits on the page
                e1.Graphics.DrawString(
                    fitsOnPage,
                    _printFont,
                    _brush,
                    _rectangleF);

                // if there is still text left, tell the PrintDocument it needs to call 
                // PrintPage again.
                e1.HasMorePages = remainingText.Length > 0;
            };
        }

        public void Printline(String data)
        {
            remainingText.AppendLine(data);
        }

        public void Print(String data)
        {
            remainingText.Append(data);
        }

        private void SetLayoutArea()
        {
            _rectangleF = new RectangleF(
                _margins.Left,
                _margins.Top,
                _printDoc.DefaultPageSettings.PrintableArea.Width - (_margins.Left + _margins.Right),
                _printDoc.DefaultPageSettings.PrintableArea.Height - (_margins.Top + _margins.Bottom));

            _layoutSize = _rectangleF.Size;
            _layoutSize.Height = _layoutSize.Height - _printFont.GetHeight(); // keep lastline visible
        }

        public void Printing()
        {
            try
            {
                TaskScheduler Sta = new StaTaskScheduler(1);
                Task.Factory.StartNew(() => _printDoc.Print(), CancellationToken.None, TaskCreationOptions.None, Sta).Wait();
            }
            catch (System.Exception ex)
            {
                EventLog e = new EventLog(LocalizedResources.Instance().PRINT_ERROR);
                e.WriteEntry(String.Format(LocalizedResources.Instance().PRINT_FAILED_IN_PRINTING, ex.Message));
            }
        }

        public override void Flush()
        {
            Printing();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            remainingText.Append(System.Text.Encoding.Default.GetString(buffer, offset, count));
        }
    }
}
