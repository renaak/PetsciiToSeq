using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpNinja.PetsciiConverter.Converters;

namespace SharpNinja.PetsciiConverter.Writers
{
    public abstract class BaseWriter
    {
        protected Options _options;
        protected BaseConverter _converter;

        protected BaseWriter(Options options, BaseConverter converter)
        {
            _options = options;
            _converter = converter;
        }

        public abstract List<byte[]> WriteBytes();
    }
}
