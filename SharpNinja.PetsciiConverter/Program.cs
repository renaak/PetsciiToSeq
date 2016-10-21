using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using SharpNinja.PetsciiConverter.Converters;
using SharpNinja.PetsciiConverter.Writers;

namespace SharpNinja.PetsciiConverter
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    Execute,
                    errors =>
                    {
                        errors.ToList().ForEach(Console.WriteLine);
                        return -1;
                    });
        }

        private static int Execute(Options options)
        {
            var result = 1;

            switch (options.InputType)
            {
                case InputTypes.JSON:
                    switch (options.OutputType)
                    {
                        case OutputTypes.SEQ:
                            var output = new SeqWriter(options, new JsonConverter(options)).WriteBytes();
                            if (output != null && output.Count > 1)
                            {
                                var counter = 0;
                                output.ForEach(bytes =>
                                {
                                    var filename = $"{options.OutputFile}{++counter:000}.seq";
                                    var file = new System.IO.FileStream(filename, FileMode.Create, FileAccess.Write);
                                    file.Write(bytes, 0, bytes.Length);
                                    file.Flush();
                                    file.Dispose();
                                });
                            }
                            else if (output != null && output.Count == 1)
                            {
                                string outFileName = options.OutputFile +
                                                     (options.OutputFile.ToLower().EndsWith(".seq") ? "" : ".seq");
                                var outfile = new System.IO.FileStream(outFileName, FileMode.Create, FileAccess.Write);
                                outfile.Write(output[0], 0, output[0].Length);
                                outfile.Flush();
                                outfile.Dispose();
                            }
                            else
                            {
                                result = 1;
                            }
                            break;
                    }
                    break;
            }

            return result;
        }
    }

    public class Options
    {
        [Option(HelpText = "Type of input file.  Available: JSON", Required = true)]
        public InputTypes InputType { get; set; }

        [Option(HelpText = "Type of file to write.", Required = true)]
        public OutputTypes OutputType { get; set; }

        [Option(HelpText = "Combine the output into a single SEQ file.", Required = false)]
        public bool Combined { get; set; }

        [Option(HelpText = "Source files to convert.", Required = true)]
        public IEnumerable<string> InputFiles { get; set; }

        [Option(HelpText = "Base output filename.  Yields a numbered list if multiple source files are selected and combined is not.", Required = true)]
        public string OutputFile { get; set; }
    }

    public enum InputTypes
    {
        JSON
    }

    public enum OutputTypes
    {
        SEQ
    }
}
