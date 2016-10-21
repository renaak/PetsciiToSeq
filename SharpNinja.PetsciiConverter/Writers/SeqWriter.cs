using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpNinja.PetsciiConverter.Converters;

namespace SharpNinja.PetsciiConverter.Writers
{
    public class SeqWriter : BaseWriter
    {
        private byte[] _colors =
        {
            144, // black
            5,  // white
            28,  // red
            159, // cyan
            156, // purple
            30,  // green
            31,  // blue
            158, // yellow
            129, // orange
            149, // brown
            150, // pink
            151, // dark gray
            152, // gray
            153, // light green
            154, // light blue
            155, // light gray
        };

        public SeqWriter(Options options, BaseConverter converter) : base(options, converter)
        {
        }

        public override List<byte[]> WriteBytes()
        {
            var results = new List<byte[]>();

            var data = _converter.Convert();

            data?.ForEach(bytes =>
            {
                var builder = new List<byte>();
                var currentColor = 0;
                var revOn = false;

                for (int i = 0; i < bytes.Length; ++i)
                {
                    var b = bytes[i];
                    if (b == 0)
                    {
                        currentColor = bytes[++i];
                        builder.Add(_colors[currentColor]);
                    }
                    else if (b > 127 && !revOn)
                    {
                        builder.Add(18);
                        builder.Add(ScreenCodeToPetscii(b));
                        revOn = true;
                    }
                    else if (b < 128 && revOn)
                    {
                        builder.Add(146);
                        builder.Add(ScreenCodeToPetscii(b));
                        revOn = false;
                    }
                    else
                    {
                        builder.Add(ScreenCodeToPetscii(b));
                    }
                }

                results.Add(builder.ToArray());
            });

            return results;
        }

        protected byte ScreenCodeToPetscii(byte screenCode)
        {
            var result = (byte)0;

            if (screenCode > 128) screenCode -= 128;

            if (screenCode >= 0x20 && screenCode <= 0x3f)
            {
                result = screenCode;
            }
            else if (screenCode <= 0x1f)
            {
                byte offset = 0x40;
                result = (byte)(screenCode + offset);
            }
            else if (screenCode >= 0x40 && screenCode <= 0x5f)
            {
                byte offset = 0x20;
                result = (byte)(screenCode + offset);
            }
            else if (screenCode >= 0x60 && screenCode <= 0x7f)
            {
                byte offset = 0x40;
                result = (byte)(screenCode + offset);
            }

            return result;
        }
    }
}
