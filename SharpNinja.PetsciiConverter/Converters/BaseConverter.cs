using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SharpNinja.PetsciiConverter.Converters
{
    public abstract class BaseConverter
    {
        protected Options _options = null;

        public BaseConverter(Options options)
        {
            _options = options;
        }

        public abstract List<byte[]> Convert();
    }
}
