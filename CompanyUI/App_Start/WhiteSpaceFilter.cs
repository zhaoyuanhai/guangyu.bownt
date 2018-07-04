using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace CompanyUI.App_Start
{
    public class WhiteSpaceFilter : Stream
    {
        private Stream _shrink;
        private Func<string, string> _filter;
        private System.Text.Encoding _encoding;

        public WhiteSpaceFilter(HttpResponse response, Func<string, string> filter)
        {
            _encoding = response.ContentEncoding;
            _shrink = response.Filter;
            _filter = filter;
        }

        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return true; } }
        public override void Flush() { _shrink.Flush(); }
        public override long Length { get { return _shrink.Length; } }
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _shrink.Read(buffer, offset, count);
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _shrink.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            _shrink.SetLength(value);
        }
        public override void Close()
        {
            _shrink.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // capture the data and convert to string

            
            byte[] data = new byte[count];
            Buffer.BlockCopy(buffer, 0, data, 0, count);

            string s = _encoding.GetString(data);
            // filter the string
            s = _filter(s);

            // write the data to stream
            byte[] outdata = _encoding.GetBytes(s);
            _shrink.Write(outdata, offset, outdata.Length);
        }
    }
}