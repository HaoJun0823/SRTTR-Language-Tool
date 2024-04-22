using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace SRTTR_Language_Tool
{
    public class LeStringsDecoder
    {

        public static readonly UInt32 LE_STRINGS_HEADER = 0xa84c7f73;

        public UInt16 version;

        public List<LeStringPairData> StringRefrenceList;

        



        private byte[] OriginFile;

        public FileInfo OriginFileInfo;

        private UInt16 StringRefrenceNumber;
        private UInt32 LeStringNumber;


        private LeStringsDecoder() { }


        public LeStringsDecoder(string path) : this(new FileInfo(path))
        {
            
        }

        public LeStringsDecoder(FileInfo path)
        {
            this.OriginFileInfo = path;

            StringRefrenceList = new List<LeStringPairData>();

            Console.WriteLine($"Read File:{path.FullName}");


            OriginFile = File.ReadAllBytes(path.FullName);
            

            using(MemoryStream ms = new MemoryStream(OriginFile)) { 
            
                using(BinaryReader br = new BinaryReader(ms))
                {

                    UInt32 header = br.ReadUInt32();

                    if(header!=LE_STRINGS_HEADER)
                    {
                        throw new FileLoadException($"{path.FullName} is not a le_stings file!");
                    }

                    version = br.ReadUInt16();

                    StringRefrenceNumber = br.ReadUInt16();
                    Console.WriteLine($"Read StringRefrenceNumber:{StringRefrenceNumber}");
                    LeStringNumber = br.ReadUInt32();
                    Console.WriteLine($"Read LeStringNumber:{LeStringNumber}");


                    for (int i=0;i<StringRefrenceNumber;i++)
                    {
                        UInt64 number =  br.ReadUInt64();
                        UInt64 offset = br.ReadUInt64();
                        Console.WriteLine($"[{i+1}/{StringRefrenceNumber}]:Number:{number.ToString("X8")},Offset:{offset.ToString("X8")}");

                        if (number == 0)
                        {
                            Console.WriteLine("PASS WRONG DATA.");
                        }
                        else
                        {
                            StringRefrenceList.Add(new LeStringPairData(number, offset));
                        }

                        

                    }

                    int count = 0;
                    foreach(var item in StringRefrenceList)
                    {

                        Console.WriteLine($"Seek {count} To:{item.offset.ToString("X8")},Read Number:{item.number}");
                        br.BaseStream.Seek((long)item.offset, SeekOrigin.Begin);

                        for(ulong i = 0; i < item.number; i++)
                        {
                            LeString le = ReadLeString(br);
                            //Console.WriteLine(le.ToString());
                            item.leStrings.Add(le);
                            count++;
                        }


                        
                        
                        

                    }

                    Console.WriteLine($"Read:{count},Total:{this.LeStringNumber}");
                }
            }




        }

        private static LeString ReadLeString(BinaryReader reader)
        {
            ulong string_offset = reader.ReadUInt64();
            long current_offset = reader.BaseStream.Position;


            reader.BaseStream.Seek((long)string_offset,SeekOrigin.Begin);

            LeString le = new LeString();

            le.Hash = reader.ReadUInt32();
            
            le.BinaryUnicodeString = readWideDataterminated(reader);

            reader.BaseStream.Position = current_offset;
            Console.WriteLine($"Read Hash:{le.Hash.ToString("X4")},{le.ToString()}");
            return le;

        }

        private static byte[] readWideDataterminated(BinaryReader reader)
        {
            var char_array = new List<byte>();
            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                byte[] char_bytes2 = char_array.ToArray();
                return char_bytes2;
            }
            byte b = reader.ReadByte();
            byte b2 = reader.ReadByte();
            while ((b != 0x00 || b2 != 0x00) && (reader.BaseStream.Position != reader.BaseStream.Length))
            {
                char_array.Add(b);
                char_array.Add(b2);
                b = reader.ReadByte();
                b2 = reader.ReadByte();
            }
            byte[] char_bytes = char_array.ToArray();

            return char_bytes;



        }

        


    }
}
