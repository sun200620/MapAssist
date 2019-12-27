using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MapAssist
{
    class LdInfo
    {
        /*********************************************LD File Infomation****************************************************/
        public string LdFilePath;

        public string LdName;
        /// <summary>
        /// memorys : ld file memory info data save
        /// 
        /// </summary>
        public List<MEMORY> Memorys = new List<MEMORY>();

        /// <summary>
        ///  Section allocate in which memory in ld file when read ld file will auto save
        /// </summary>
        public Dictionary<string, string> SectionMemory = new Dictionary<string, string>();

        /// <summary>
        /// memory allocate which resource(ram flash dram...) in mcu,need configure by user
        /// </summary>
        public Dictionary<string, eResourceType> MemoryAllocate = new Dictionary<string, eResourceType>();

        /// <summary>
        /// to save percent of section group allocate in memory,
        /// must read map and ld file 
        /// </summary>
        //public Dictionary<string, double> UsedMemPercent = new Dictionary<string, double>();

        enum LDSPACE
        {
            LD_BLANK = 1,
            LD_MEM = 2,
            LD_SEG = 3,
        };
        public void ReadLdFile(string fpath,ref LdInfo LdInfo)
        {
            StreamReader streamMap = new StreamReader(fpath);

            string fline;
            LdInfo.LdFilePath = fpath;
            GetNameFromPath(fpath, ref LdInfo.LdName);
            LDSPACE ldSpace = LDSPACE.LD_BLANK;

            LdInfo.Memorys.Clear();
            LdInfo.SectionMemory.Clear();

            while ((fline = streamMap.ReadLine()) != null)
            {

                fline = fline.Trim();

                switch (ldSpace)
                {
                    case LDSPACE.LD_BLANK:
                        if (fline.StartsWith("MEMORY"))
                        {
                            ldSpace = LDSPACE.LD_MEM;
                        }
                        break;
                    case LDSPACE.LD_MEM:
                        if (fline.StartsWith("SECTIONS"))
                        {
                            ldSpace = LDSPACE.LD_SEG;
                        }

                        if ((fline.Contains(':') == true) && (fline.StartsWith("/*") == false))//not comment line
                        {

                            MemoryLineRead(fline, ref LdInfo.Memorys);
                        }



                        break;
                    case LDSPACE.LD_SEG:

                        if (fline.StartsWith("."))
                        {
                            string[] secstrs = fline.Split(' ');

                            
                            if (secstrs[0] != null)
                            {
                                if (secstrs[0].EndsWith(":"))// for deal with   .fft_buff_seg: {}
                                {
                                    secstrs[0] = secstrs[0].TrimEnd(':');
                                }
                                if (secstrs[0].EndsWith("(TEXT)"))//(TEXT).acfls_code_rom(TEXT) ALIGN(4) : {KEEP (*(.acfls_code_rom))} 
                                {
                                    secstrs[0] = secstrs[0].TrimEnd("(TEXT)".ToCharArray());
                                }


                                    LdInfo.SectionMemory.Add(secstrs[0], null);
                            }


                        }

                        if (fline.Contains('>'))
                        {
                            string[] secstrs = fline.Split('>');

                            string memName = secstrs[1].Trim();


                            string[] keyArr = LdInfo.SectionMemory.Keys.ToArray();

                            for (int i = 0; i < keyArr.Length; i++)
                            {
                                string secName = keyArr[i];

                                if (LdInfo.SectionMemory[secName] == null)
                                {
                                    LdInfo.SectionMemory[secName] = memName;

                                    //Console.WriteLine("Section:{0} allocate mem:{1}", secName.PadRight(26), memName);
                                }


                            }


                        }


                        break;
                    default:
                        break;

                }
            }

            streamMap.Close();

        }

        /// <summary>
        /// memory line context example
        /// 
        /// </summary>
        /// <param name="fline"></param>
        private void MemoryLineRead(string fline,ref List<MEMORY> Memorys)
        {
            ///example:
            /// flash_rchw          : org = 0x01000000,         len = 0x4
            /// cpu0_reset_vec      : org = 0x01000004,         len = 4
            /// int_flash           : org = 0x01001000,         len = 507K

            MEMORY memItem = new MEMORY();

            string[] memline = fline.Split(':');
            string[] memline_orglen = memline[1].Split(',');

            memItem.Name = memline[0].Trim();


            string[] orgStrs = memline_orglen[0].Split('=');
            memItem.Org = Convert.ToInt32(orgStrs[1].Trim(), 16);

            string[] lenStrs = memline_orglen[1].Split('=');
            string len = lenStrs[1].Trim();
            if (len.EndsWith("K"))
            {
                len = len.TrimEnd('K');
                memItem.Len = Convert.ToInt32(len) * 1024;
            }
            else
            {
                memItem.Len = Convert.ToInt32(len, 16);
            }

            //add for def mem allocate which mcu resource
            memItem.SetMemResource();
            
            Memorys.Add(memItem);


            //collect and save mem name in dictionnary
            if (MemoryAllocate.Keys.Contains(memItem.Name))
            {

            }
            else
            {
                MemoryAllocate.Add(memItem.Name, eResourceType.eTBD);
            }
        }

        private string GetNameFromPath(string fline, ref string fname)//
        {
            if (fline.Contains('\\'))
            {
                fline = fline.Replace('\\', '/');
            }
            fname = fline.Substring(fline.LastIndexOf('/')).TrimStart('/');

            return fline;
        }


    }
}
