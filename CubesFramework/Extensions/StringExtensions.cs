using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubesFramework.Extensions
{
    public static class StringExtensions
    {
        public static string SplitBoardInfo(this string str,char sperator, int blocksize)
        {
            var serial = new StringBuilder();
            for (int i = 0; i < str.Length; i += blocksize)
            {
                blocksize = i + blocksize > str.Length ? str.Length - i : blocksize;
                serial.Append(str.Substring(i, blocksize) + sperator);
            }
            return serial.ToString().Remove(serial.Length - 1);
        }
    }
}
