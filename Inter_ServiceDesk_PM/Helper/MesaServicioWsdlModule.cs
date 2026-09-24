using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Inter_ServiceDesk_PM
{
    /// <summary>
    /// Removes the unused text-matching WSDL namespace emitted by ASMX for MesaServicio.
    /// </summary>
    public sealed class MesaServicioWsdlModule : IHttpModule
    {
        private const string TargetServicePath = "/Intermediario/MesaServicio.asmx";

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        public void Dispose()
        {
        }

        private static void OnBeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            if (!IsMesaServicioWsdl(context.Request))
            {
                return;
            }

            context.Response.BufferOutput = true;
            context.Response.Headers.Remove("Content-Length");
            context.Response.Filter = new WsdlNamespaceFilter(
                context.Response.Filter,
                context.Response.ContentEncoding ?? Encoding.UTF8);
        }

        private static bool IsMesaServicioWsdl(HttpRequest request)
        {
            if (!request.Path.EndsWith(
                    TargetServicePath,
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string query = request.Url == null ? String.Empty : request.Url.Query;

            return String.Equals(query, "?WSDL", StringComparison.OrdinalIgnoreCase)
                || query.StartsWith("?WSDL=", StringComparison.OrdinalIgnoreCase);
        }

        private sealed class WsdlNamespaceFilter : Stream
        {
            private static readonly Regex TextMatchingNamespace = new Regex(
                @"\s+xmlns:tm\s*=\s*""http://microsoft\.com/wsdl/mime/textMatching/""",
                RegexOptions.Compiled | RegexOptions.CultureInvariant);

            private readonly Stream inner;
            private readonly MemoryStream buffer = new MemoryStream();
            private readonly Encoding encoding;
            private bool responseWritten;

            public WsdlNamespaceFilter(Stream inner, Encoding encoding)
            {
                this.inner = inner;
                this.encoding = encoding;
            }

            public override bool CanRead => false;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length => buffer.Length;

            public override long Position
            {
                get => buffer.Position;
                set => throw new NotSupportedException();
            }

            public override void Flush()
            {
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

            public override void Write(byte[] source, int offset, int count)
            {
                buffer.Write(source, offset, count);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && !responseWritten)
                {
                    responseWritten = true;
                    string wsdl = encoding.GetString(buffer.ToArray());
                    string filteredWsdl = TextMatchingNamespace.Replace(wsdl, String.Empty);
                    byte[] response = encoding.GetBytes(filteredWsdl);

                    inner.Write(response, 0, response.Length);
                    inner.Flush();
                    buffer.Dispose();
                }

                base.Dispose(disposing);
            }
        }
    }
}
