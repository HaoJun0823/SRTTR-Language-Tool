using MiniExcelLibs;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace SRTTR_Language_Tool
{
    internal class Program
    {


        static DataTable dt = new DataTable();

        static bool IsOtherCode = false;

        static void Main(string[] args)
        {
            //if(args.Length != 0)
            //{
            //    IsOtherCode = true;
            //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //}


            Directory.CreateDirectory(".\\Le_Strings");

            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Index", typeof(int));
            //dt.Columns.Add("Total", typeof(int));
            dt.Columns.Add("Hash", typeof(uint));
            dt.Columns.Add("Data", typeof(string));

            var files = Directory.GetFiles(".\\Le_Strings","*.le_strings", SearchOption.TopDirectoryOnly);

            List<LeStringsDecoder> list = new List<LeStringsDecoder>();

            foreach(var file in files)
            {
                list.Add(new LeStringsDecoder(file));
            }

            SaveDataTable(list);
            


        }


        static void SaveDataTable(List<LeStringsDecoder> list)
        {
            
            
            foreach(var file in list)
            {
                int id = 0;
                int total = 0;

                foreach (var filedata in file.StringRefrenceList)
                {
                    int index = 0;
                    foreach(var filestring in filedata.leStrings)
                    {

                        DataRow dr = dt.NewRow();

                        dr["Name"] = file.OriginFileInfo.Name;
                        dr["Id"] = id;
                        dr["Index"] = index;
                        dr["Hash"] = filestring.Hash;

                        if (IsOtherCode)
                        {
                            dr["Data"] = Encoding.GetEncoding("GB2312").GetString(filestring.BinaryUnicodeString);
                        }
                        else
                        {
                            dr["Data"] = filestring.ToString();
                        }

                        dt.Rows.Add(dr);

                        index++;
                        total++;
                    }

                    id++;
                }



                


            }

            if (File.Exists(".\\le_strings.xlsx"))
            {
                File.Delete(".\\le_strings.xlsx");
            }

            MiniExcel.SaveAs(".\\le_strings.xlsx",dt);


        }

    }
}
