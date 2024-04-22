using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTTR_Language_Tool
{
    public class LeStringPairData
    {

        public UInt64 number;
        public UInt64 offset;

        public List<LeString> leStrings = new List<LeString>();


        public LeStringPairData(ulong number, ulong offset)
        {
            this.number = number;
            this.offset = offset;
        }
    }
}
