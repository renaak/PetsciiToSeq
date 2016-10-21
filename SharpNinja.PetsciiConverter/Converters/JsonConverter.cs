using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SharpNinja.PetsciiConverter.Converters
{
    public class JsonConverter : BaseConverter
    {
        public JsonConverter(Options options) : base(options)
        {
        }

        public override List<byte[]> Convert()
        {
            var results = new List<byte[]>();
            foreach (var inputFile in _options.InputFiles)
            {
                var file = System.IO.File.ReadAllText(inputFile);
                dynamic json = JsonConvert.DeserializeObject(file);

                dynamic screenBytes = json.charData;
                var sbytes = new byte[0];

                if (screenBytes != null)
                {
                    sbytes = new byte[screenBytes.Count];
                    for(int i = 0; i < sbytes.Length; ++i)
                    {
                        sbytes[i] = screenBytes[i];
                    }
                }

                dynamic colorBytes = json.colorData;
                var cbytes = new byte[0];

                if (colorBytes != null)
                {
                    cbytes = new byte[colorBytes.Count];
                    for (int i = 0; i < cbytes.Length; ++i)
                    {
                        cbytes[i] = colorBytes[i];
                    }
                }

                if (cbytes.Length != sbytes.Length)
                    throw new Exception("Color and Char bytes arrays are not the same size.");

                var output = new List<byte>();
                var lastc = 0;
                for (int i = 0; i < sbytes.Length; ++i)
                {
                    var s = sbytes[i];
                    var c = cbytes[i];

                    if (c != lastc)
                    {
                        output.Add(0);
                        output.Add(c);
                        lastc = c;
                    }

                    output.Add(s);
                }

                results.Add(output.ToArray());
            }

            return results;
        }
    }
}
