using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace MapAssist
{
    public enum lookUpType
    {
        T_SEG = 1,
        T_MEM = 2,
        T_MODULE = 3,
        T_FILE = 4,
        T_RESOURCE = 5,

    };

    class MapInfo
    {
        /*********************************************Map File information****************************************************/
        /// <summary>
        /// 
        /// </summary>
        public string MapFilePath;

        public string MapFileName;

        /// <summary>
        /// ld file name in map file
        /// </summary>
        public string LdNameInMap;
        public bool LdExist = false;

        /// <summary>
        /// read map file obj info data and save,obj may be not sigle in the list
        /// </summary>
        public List<OBJ> MapFileObjs = new List<OBJ>();

        /// <summary>
        /// here define var to save section name group
        /// </summary>
        //public ArrayList MapFileSections = new ArrayList();

        /// <summary>
        /// save section name as key and calculate same section sum size result as key value 
        /// 
        /// </summary>
        public Dictionary<string, int> MapFileSectionSize = new Dictionary<string, int>();

        public Dictionary<string, int> MapFileMemSize = new Dictionary<string, int>();

        //public Dictionary<string, MEMORY> MapFileMemUsedInfo = new Dictionary<string, MEMORY>();

        /// <summary>
        /// create section name group from objCollection
        /// </summary>
        private void SaveMapFileSections()
        {

            foreach (OBJ obj in MapFileObjs)
            {
                //foreach (SEC_MEM secmem in obj.SecMem)
                {
                    //save section in array type
                    if (MapFileSectionSize.Keys.Contains(obj.SecName))
                    {

                    }
                    else
                    {
                        MapFileSectionSize.Add(obj.SecName,0);
                    }
                }

            }

        }

        private void CalculateMapFileSectionSize()
        {
            
            foreach (string sectionName in MapFileSectionSize.Keys.ToArray())
            {

                int sumTmp = 0;
                foreach (OBJ obj in MapFileObjs)
                {

                    if (sectionName.Equals(obj.SecName))
                    {
                        sumTmp += obj.SecSize;
                    }

                }

                MapFileSectionSize[sectionName] = sumTmp;

                //Console.WriteLine("SectionName:{0} UsedSize:{1}", sectionName.PadRight(26), sumTmp);

            }

         }

        private void MapInfoPrint()
        {
            Console.WriteLine("Read Map File,Recorded Information as follows:");
            Console.WriteLine(MapFilePath);
            Console.WriteLine(MapFileName);
            Console.WriteLine(LdNameInMap);

            foreach (OBJ obj in MapFileObjs)
            {
                Console.WriteLine("{0} {1} {2} {3} {4}",
                                    obj.ObjName,
                                    obj.ObjPath, 
                                    obj.SecName, 
                                    obj.SecAddr, 
                                    obj.SecSize);
            }

        }

        private void MapSecSizePrint()
        {
            foreach (string secN in MapFileSectionSize.Keys)
            {
                Console.WriteLine("sec:{0},size:{1}",secN + MapFileSectionSize[secN]);
            }
        }

        /// <summary>
        /// Read .map file info into obj collection
        /// </summary>
        /// <param name="fpath"></param>
        public void ReadMapFile(string fpath, ref MapInfo mapInfo)
        {
            
            StreamReader streamMap = new StreamReader(fpath);

            mapInfo.MapFilePath = fpath;
            GetNameFromPath(fpath, ref mapInfo.MapFileName);

            string fline;

            while ((fline = streamMap.ReadLine()) != null)
            {

                fline = fline.Trim();
                if (fline.Contains(".debug_line") ||
                   fline.Contains(".debug_frame") ||
                   fline.Contains(".debug_abbrev") ||
                   fline.Contains(".debug_info") ||
                   fline.Contains(".debug_loc"))
                {
                    continue;
                }
                //search start with '.' and end with ".o"//.bss		50806b78	00006bdc [COMMON] not start with "."
                if ((fline.EndsWith(".o") || fline.EndsWith(".o]"))) //if (fline.StartsWith(".") && (fline.EndsWith(".o") || fline.EndsWith(".o]")))
                {
                    fline = ReadObjLineInMap(fline, ref mapInfo.MapFileObjs);
                }

                //.PPC.EMB.apuinfo 00000000	00000020 ../../Project/build/config/core_0/s32r274_flash_core_0.ld
                if (fline.StartsWith(".") && (fline.EndsWith(".ld")))
                {
                    GetNameFromPath(fline, ref mapInfo.LdNameInMap);
                    foreach(OBJ objTmp in MapFileObjs)
                    {
                        objTmp.SetObjFileInfor(mapInfo.MapFileName,mapInfo.LdNameInMap);
                    }
                    
                }

            }
            //close filestream
            streamMap.Close();
            mapInfo.SaveMapFileSections();
            mapInfo.CalculateMapFileSectionSize();


        }

        /// <summary>
        /// read map line information include obj, save in objCollection
        /// </summary>
        /// <param name="fline"></param>
        /// <returns></returns>
        /// 
        private string ReadObjLineInMap(string fline, ref List<OBJ> objlist)
        {
            OBJ obj_tmp = new OBJ();

            fline = fline.Trim();
            string tmpStr = "";

            //Console.WriteLine("mapline:{0}", fline);

            //remove blank chacter
            fline = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(fline, " ");

            string[] seplines = fline.Split(" ".ToCharArray());

            obj_tmp.SecName = seplines[0].Trim();

            //to deal with .bss big section,not start with .bss but start with fileName 
            //examplle : stDfa_EEP 50806b78	00000468 ../../Project/build/output/core_0/obj/Dfa_EEP_Int.c.o
            if(obj_tmp.SecName.StartsWith(".") == false)
            {
                obj_tmp.SecName = ".bss";//force 
                //Console.WriteLine("line force assign .bss: {0}", fline);
            }

            obj_tmp.SecAddr = Convert.ToInt32(seplines[1].Trim(), 16);
            obj_tmp.SecSize = Convert.ToInt32(seplines[2].Trim(), 16);

            //tmpStr = seplines[3].Trim();
            fline = fline.Replace('\\', '/');
            tmpStr = fline.Substring(fline.LastIndexOf('/')).TrimStart('/');
            obj_tmp.ObjName = tmpStr.TrimEnd(".o".ToArray());

            objlist.Add(obj_tmp);

            return fline;
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

        public void SupplymentMapObjs(LdInfo LdInfo)
        {
            //get each obj section name
            foreach (OBJ objTmp in MapFileObjs)
            {
                string secName = objTmp.SecName;

                //lookup secName in which memory
                if (LdInfo.SectionMemory.Keys.Contains(secName))
                {
                    string memNameInSecMem = LdInfo.SectionMemory[secName];
                    foreach (MEMORY mem in LdInfo.Memorys)
                    {
                        //when find sec name in memorys set obj info
                        if (memNameInSecMem == mem.Name)
                        {
                            objTmp.SetObjMemInfor(mem);
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Not Find SecMem Key Name:{0} in Ldfile:{1},this secName from ObjName:{2} in Map:{3} ",
                                        secName, LdInfo.LdName, objTmp.ObjName, this.MapFileName );
                }


            }
        }

        public void SetObjFileModule(Dictionary<string, string> FileModule)
        {
             foreach(OBJ objTmp in MapFileObjs)
            {
                string objName = objTmp.ObjName;
                if (FileModule.ContainsKey(objName))
                {
                    objTmp.SetObjFileModule(FileModule[objName]);
                }
                else
                {
                    Console.WriteLine("Not Find Obj:{0} In FileModule", objName);
                }
            }
        }
        private List<OBJ> LookupAndCalSameParMemUsed(lookUpType lookupType, string strPar,ref int memUse)
        {
            List<OBJ> tmplist = new List<OBJ>();

            //lookup all list items to search matched item
            foreach (OBJ ObjTmp in MapFileObjs)
            {

                switch (lookupType)
                {
                    case lookUpType.T_FILE:
                        //if (segTmp.ObjFileName == strName)
                        //{
                        //    tmplist.Add(segTmp);
                        //}
                        break;

                    case lookUpType.T_MODULE:

                        break;

                    case lookUpType.T_SEG:

                        break;
                    case lookUpType.T_MEM:
                        if (ObjTmp.MemName == strPar)
                        {
                            tmplist.Add(ObjTmp);
                            memUse += ObjTmp.SecSize;

                            //for debug
                            if(strPar == "int_sram")
                            {
                                //Console.WriteLine(strPar+" "+ObjTmp.SecName + " " +ObjTmp.ObjName);
                                if (ObjTmp.SecName == ".bss")
                                {
                                    //Console.WriteLine(".bss " + ObjTmp.SecSize +"   "+ ObjTmp.ObjName);
                                }
                            }
                            
                        }
                        break;

                    default:
                        break;
                }

            }

            return tmplist;
        }
        public void CalSameMemUsed(LdInfo LdInfo,ref List< MEMORY> MemUsedInfo)
        {

            //MapFileMemUsedInfo.Clear();
            foreach (MEMORY mem in LdInfo.Memorys)
            {
                int useSize = 0;

                LookupAndCalSameParMemUsed(lookUpType.T_MEM, mem.Name, ref useSize);

                mem.memUsage.Used = useSize;
                mem.memUsage.UsedPercent = Convert.ToDouble(useSize) / Convert.ToDouble(mem.Len);
                MemUsedInfo.Add(mem);


            }

        }
        
        private int CalSameParUsedInSameObj(string parName, int memUse, List<OBJ> sameParObj)
        {
            foreach (OBJ objTmp in MapFileObjs)
            {
                if (parName == objTmp.MemName)
                {
                    sameParObj.Add(objTmp);
                    memUse += objTmp.SecSize;
                }
            }

            return memUse;
        }
                

}
    
}
