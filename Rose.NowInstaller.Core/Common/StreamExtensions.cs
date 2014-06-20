using System.IO;

namespace Rose.NowInstaller.Core.Common
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream input)
        {
            if (input is MemoryStream)
            {
                return ((MemoryStream)input).ToArray();
            }

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
