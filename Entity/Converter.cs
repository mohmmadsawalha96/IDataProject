using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public static class Converter
    {
        public static double ConvertBytesToKilobytes(long bytes)
        {
            return (bytes / 1024f);
        }

       


    }
}
