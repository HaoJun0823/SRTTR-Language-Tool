using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTTR_Language_Tool
{
    public class LeString
    {


        public UInt32 Hash;

        public byte[] BinaryUnicodeString;



        
        public override String ToString()
        {
            
            return Encoding.Unicode.GetString(BinaryUnicodeString);

        }

    }
}
