using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace MapAssist
{

    public struct strUsage
    {
        public int Size;
        public int Used;
        public double UsedPercent;
    };
    enum eResourceType
    {
        eTBD,
        eEEROM,
        eBOOT_FLASH,
        eCODE_FLASH,
        eSRAM,
        eDRAM,

    };
    public enum MapLdSameT
    {
        NoMapExist = 0,
        NotLDExist = 1,
        AllLDExist = 3,
    };
    class FileRead
    {
        /// <summary>
        /// filepath , file-name
        /// </summary>
        private Dictionary<string, string> FileList = new Dictionary<string, string>();

        /// <summary>
        /// Save all Readed Map Files Info
        /// </summary>
        private List<MapInfo> MapInfos = new List<MapInfo>();

        /// <summary>
        /// Save All Readed Ld Files Info
        /// </summary>
        private List<LdInfo> LdInfos = new List<LdInfo>();

        /// <summary>
        /// All Obj list in all Maps
        /// </summary>
        //private List<OBJ> AllObjs = new List<OBJ>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, int> AllSecSize = new Dictionary<string, int>();
        //
        /// <summary>
        /// to save Mcu Resource and memory Used percent
        /// </summary>
        public Dictionary<eResourceType, strUsage> McuResourceSize = new Dictionary<eResourceType, strUsage>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> SourceFileModule = new Dictionary<string, string>();
        public bool SourceFileModuleCfged = false;
        /// <summary>
        /// 
        /// </summary>
        public void Init_Start()
        {
            MapInfos.Clear();
            LdInfos.Clear();
            AllSecSize.Clear();

        }
        public void Init_Load()
        {
            MapInfos.Clear();
            LdInfos.Clear();
            AllSecSize.Clear();

            McuResourceInit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileNames"></param>
        public void UpdateFileList(string[] fileNames)
        {
            FileList.Clear();
            foreach (string fileNamePath in fileNames)
            {
                string fileName = null;

                GetNameFromPath(fileNamePath, ref fileName);
                if (FileList.ContainsKey(fileNamePath))
                {

                }
                else
                {
                    FileList.Add(fileNamePath, fileName);
                }
            }

        }
        public MapLdSameT MapAndLdCheck(ref ArrayList NoLdMapsName, ref ArrayList AllMapsName)
        {
            if (MapInfos.Count <= 0)
            {
                return MapLdSameT.NoMapExist;
            }
            foreach (MapInfo map in MapInfos)
            {
                foreach (LdInfo ldInfo in LdInfos)
                {
                    if (map.LdNameInMap == ldInfo.LdName)
                    {
                        map.LdExist = true;
                        break;
                    }
                }

                if (map.LdExist == false)
                {
                    NoLdMapsName.Add(map.MapFileName);
                }
                AllMapsName.Add(map.MapFileName);

            }

            //return 
            foreach (MapInfo map in MapInfos)
            {
                if (map.LdExist == false)
                {
                    return MapLdSameT.NotLDExist;
                }
            }

            return MapLdSameT.AllLDExist;
        }
        /// <summary>
        /// read map and ld file,and save 
        /// </summary>
        /// <returns></returns>
        public bool filesRead()
        {

            if (FileList.Count <= 0)
            {
                return false;
            }
            else
            {
                
                foreach (string filepath in FileList.Keys)
                {
                    RunfileRead(filepath, FileList[filepath]);
                    Console.WriteLine("Run File Read {0}", filepath);
                }
            }

            return true;
        }

        public void CalculateMcuResourceUsed(ref Dictionary<eResourceType, strUsage> McuResourceUsed)
        {

            SupplymentMapByLd();
            List<OBJ> AllObjs = new List<OBJ>();
            CombineAllObjInMaps(ref AllObjs);//must combine all objs first

            //set mcu resource size according to default value or set manual
            foreach (eResourceType eRes in McuResourceSize.Keys.ToArray())
            {
                McuResourceUsed.Add(eRes, McuResourceSize[eRes]);
            }


            foreach (eResourceType eRes in McuResourceUsed.Keys.ToArray())
            {
                List<OBJ> SameResourceObjs = new List<OBJ>();
                int rUsed = 0;

                Console.WriteLine("Ready to Find in AllObjs Same Resource {0}", eRes);
                foreach (OBJ objTmp in AllObjs)
                {
                    if (eRes == objTmp.Resource)
                    {
                        rUsed += objTmp.SecSize;
                        SameResourceObjs.Add(objTmp);
                        //Console.WriteLine(objTmp.SecAddr.ToString("X"));
                    }
                }
                strUsage ReUsedInfo;
                ReUsedInfo = McuResourceUsed[eRes];//get value resource size saved in dic
                ReUsedInfo.Used = rUsed;
                ReUsedInfo.UsedPercent = Convert.ToDouble(rUsed) / Convert.ToDouble(ReUsedInfo.Size);
                McuResourceUsed[eRes] = ReUsedInfo;

            }

            Console.WriteLine("Resource Used List:");
            foreach (eResourceType eRes in McuResourceUsed.Keys.ToArray())
            {
                Console.WriteLine("Name:{0}" + " Used:{1}" + "Used Percent:{2}",
                                  eRes.ToString().PadRight(20), McuResourceUsed[eRes].Used.ToString().PadRight(20), McuResourceUsed[eRes].UsedPercent.ToString().PadRight(20));
            }



        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateMemoryUsageInEachMapLd(ref Dictionary<string, List<MEMORY>> MapLd_MemUsed)
        {
            SupplymentMapByLd();

            foreach (MapInfo mapinfor in MapInfos)
            {
                foreach (LdInfo ldinfo in LdInfos)
                {
                    if (mapinfor.LdNameInMap == ldinfo.LdName)
                    {
                        string mapName = mapinfor.MapFileName;
                        List<MEMORY> memUsed = new List<MEMORY>();
                        mapinfor.CalSameMemUsed(ldinfo, ref memUsed);

                        MapLd_MemUsed.Add(mapName, memUsed);
                        break;
                    }

                }

            }

        }
        public void CalModuleResourceUsed(string module, ref Dictionary<eResourceType, strUsage> McuResourceUsed,ref List<OBJ> sameModuleObj)
        {
            List<OBJ> AllObjs = new List<OBJ>();

            SupplymentMapByLd();
            CombineAllObjInMaps(ref AllObjs);

            //collect same module file
            foreach (OBJ objTmp in AllObjs)
            {
                if (objTmp.AllocateModule == module)
                {
                    if(sameModuleObj.Contains(objTmp))
                    {
                    }
                    else
                    {
                    sameModuleObj.Add(objTmp);
                    }
                }
            }

            //
            //set mcu resource size according to default value or set manual
            foreach (eResourceType eRes in McuResourceSize.Keys.ToArray())
            {
                McuResourceUsed.Add(eRes, McuResourceSize[eRes]);
            }

            //
            Console.WriteLine("Ready to Find analysis module: ", module);
            foreach (eResourceType eRes in McuResourceUsed.Keys.ToArray())
            {
                List<OBJ> SameResourceObjs = new List<OBJ>();
                int rUsed = 0;


                foreach (OBJ objTmp in sameModuleObj)
                {
                    if (eRes == objTmp.Resource)
                    {
                        rUsed += objTmp.SecSize;
                        SameResourceObjs.Add(objTmp);
                        //Console.WriteLine(objTmp.SecAddr.ToString("X"));
                    }
                }
                strUsage ReUsedInfo;
                ReUsedInfo = McuResourceUsed[eRes];//get value resource size saved in dic
                ReUsedInfo.Used = rUsed;
                ReUsedInfo.UsedPercent = Convert.ToDouble(rUsed) / Convert.ToDouble(ReUsedInfo.Size);
                McuResourceUsed[eRes] = ReUsedInfo;

            }

        }
        public void CalObjResourceUsed(List<OBJ> SourceObj,ref Dictionary<eResourceType, strUsage> McuResourceUsed)
        {

            foreach (eResourceType eRes in McuResourceSize.Keys.ToArray())
            {
                McuResourceUsed.Add(eRes, McuResourceSize[eRes]);
            }
            foreach (eResourceType eRes in McuResourceUsed.Keys.ToArray())
            {
                List<OBJ> SameResourceObjs = new List<OBJ>();
                int rUsed = 0;
                foreach (OBJ objTmp in SourceObj)
                {
                    if (eRes == objTmp.Resource)
                    {
                        rUsed += objTmp.SecSize;
                        SameResourceObjs.Add(objTmp);
                    }
                }
                strUsage ReUsedInfo;
                ReUsedInfo = McuResourceUsed[eRes];//get value resource size saved in dic
                ReUsedInfo.Used = rUsed;
                ReUsedInfo.UsedPercent = Convert.ToDouble(rUsed) / Convert.ToDouble(ReUsedInfo.Size);
                McuResourceUsed[eRes] = ReUsedInfo;
            }
        }
        public void ReadCfgFileMoldule(string fpath)
        {
            StreamReader streamR = new StreamReader(fpath);
            string fline = null;

            while ((fline = streamR.ReadLine()) != null)
            {
                if (fline.Contains(","))//to resure 
                {
                    string[] fileStr = fline.Split(",".ToCharArray());

                    string fileName = fileStr[0].TrimEnd();
                    string fileModule = fileStr[1].TrimEnd();
                    if (SourceFileModule.ContainsKey(fileName))
                    {
                        SourceFileModule[fileName] = fileModule;
                    }
                    else
                    {
                        SourceFileModule.Add(fileName, fileModule);
                    }
                }

            }
            streamR.Close();
        }

        //----------------------------private-------------------------------------------//
        //-----------------------------------------------------------------------//

        private string GetNameFromPath(string fline, ref string fname)
        {
            if (fline.Contains('\\'))
            {
                fline = fline.Replace('\\', '/');
            }
            fname = fline.Substring(fline.LastIndexOf('/')).TrimStart('/');

            return fline;
        }
        private string RunfileRead(string filePath, string fileName)
        {

            filePath = filePath.TrimStart('"').TrimEnd('"');

            if (filePath.EndsWith(".map"))
            {

                //MapInfos
                if (IsMapAlreadyinMapInfos(filePath))
                {
                    Console.WriteLine("{0} File already read", filePath);
                }
                else
                {
                    MapInfo mapinfo = new MapInfo();
                    mapinfo.MapFileName = fileName;
                    mapinfo.ReadMapFile(filePath, ref mapinfo);

                    this.MapInfos.Add(mapinfo);

                }

            }
            else if (filePath.EndsWith(".ld"))
            {

                if (IsLdAlreadyinLdInfos(filePath))
                {
                    Console.WriteLine("{0} File already read", filePath);
                }
                else
                {
                    LdInfo LdInfo = new LdInfo();
                    LdInfo.ReadLdFile(filePath, ref LdInfo);

                    LdInfos.Add(LdInfo);

                }

            }

            return filePath;
        }
        


        /// <summary>
        /// Read all Obj file into file module
        /// </summary>
        public void AddFileNameIntoSourceFileModule()
        {
            foreach (MapInfo map in MapInfos)
            {
                foreach(OBJ objTmp in map.MapFileObjs)
                {
                    string objName = objTmp.ObjName;

                    if (SourceFileModule.Keys.Contains(objName) == false)
                    {
                        SourceFileModule.Add(objName,"TBD");
                    }
                    
                }
                
            }
            
        }
        public void SetMapInfoObjModule()
        {
            foreach (MapInfo map in MapInfos)
            {
                map.SetObjFileModule(SourceFileModule);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// read file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>


        /// <summary>
        /// Supplyment Mapinfos by ld file
        /// </summary>
        public void SupplymentMapByLd()
        {
            foreach (MapInfo mapinfo in MapInfos)
            {

                foreach (LdInfo LdInfo in LdInfos)
                {

                    if (mapinfo.LdNameInMap == LdInfo.LdName)
                    {
                        mapinfo.SupplymentMapObjs(LdInfo);
                        break;
                    }

                }

            }

        }

        /// <summary>
        /// combine all obj in mapinfs
        /// </summary>
        public void CombineAllObjInMaps(ref List<OBJ> AllObjs)
        {

            foreach (MapInfo mapinfo in MapInfos)
            {

                foreach (OBJ objtmp in mapinfo.MapFileObjs)
                {


                    if (AllObjs.Contains(objtmp))
                    {

                    }
                    else
                    {
                        AllObjs.Add(objtmp);
                    }

                }

            }


        }
        /// <summary>
        /// 
        /// </summary>
        public void AllObjsDebugOut()
        {
            List<OBJ> AllObjs = new List<OBJ>();
            CombineAllObjInMaps(ref AllObjs);//must combine all objs first
            Console.WriteLine("Output Combined allObjs in all maps");
            foreach (OBJ obj in AllObjs)
            {
                Console.WriteLine("{0} {1} {2} {3} {4}",
                                 obj.ObjName,
                                 obj.ObjPath,
                                 obj.SecName,
                                 obj.SecAddr,
                                 obj.SecSize);
            }

        }

        public void CombineAllMapSectionSize(List<MapInfo> MapInfos)
        {

            foreach (MapInfo mapinfo in MapInfos)
            {

                foreach (string sectionName in mapinfo.MapFileSectionSize.Keys.ToArray())
                {
                    if (AllSecSize.Keys.Contains(sectionName))
                    {

                    }
                    else
                    {
                        AllSecSize.Add(sectionName, 0);
                    }
                }

            }
        }
        public void CombineAllMapSectionSize(List<OBJ> AllObjs)
        {
            foreach (OBJ objtmp in AllObjs)
            {
                string secName = objtmp.SecName;

                if (AllSecSize.Keys.Contains(secName))
                {

                }
                else
                {
                    AllSecSize.Add(secName, 0);
                }
            }
        }
        public void CombineAndCalculateAllSecSize()
        {
            List<OBJ> AllObjs = new List<OBJ>();
            CombineAllObjInMaps(ref AllObjs);//must combine all objs first

            AllSecSize.Clear();

            foreach (OBJ objtmp in AllObjs)
            {
                string secName = objtmp.SecName;
                int secSize = objtmp.SecSize;

                if (AllSecSize.Keys.Contains(secName))
                {
                    AllSecSize[secName] += secSize;
                }
                else
                {
                    AllSecSize.Add(secName, 0);
                }
            }
        }
        private bool IsMapAlreadyinMapInfos(string filepath)
        {
            foreach (MapInfo map in MapInfos)
            {
                string savedMapPath = map.MapFilePath;
                if (filepath == savedMapPath)
                {
                    return true;
                }
            }
            //not find 
            return false;
        }
        private bool IsLdAlreadyinLdInfos(string filepath)
        {
            foreach (LdInfo Ld in LdInfos)
            {
                string savedMapPath = Ld.LdFilePath;
                if (filepath == savedMapPath)
                {
                    return true;
                }
            }
            //not find 
            return false;
        }


        private void McuResourceInit()
        {
            /// 0x00800000-0x0080FFFF EEROM          :64K
            /// 0x00A00000-0x00FFFFFF BootFlash      :416K
            /// 0x01000000-0x0117FFFF CodeFlash      :1536K
            /// 0x40000000-0x4017FFFF SRAM           :1536K
            /// 0x50800000-0x5082FFFF DMEM           :192K
            strUsage ReUsedInfo;

            if(McuResourceSize.Keys.Contains(eResourceType.eCODE_FLASH) == false)
            {
                ReUsedInfo.Size = 1536 * 1024;
                ReUsedInfo.Used = 0;
                ReUsedInfo.UsedPercent = 0;
                McuResourceSize.Add(eResourceType.eCODE_FLASH, ReUsedInfo);
            }

            if (McuResourceSize.Keys.Contains(eResourceType.eSRAM) == false)
            {
                ReUsedInfo.Size = 1536 * 1024;
                ReUsedInfo.Used = 0;
                ReUsedInfo.UsedPercent = 0;
                McuResourceSize.Add(eResourceType.eSRAM, ReUsedInfo);
            }

            if (McuResourceSize.Keys.Contains(eResourceType.eDRAM) == false)
            {
                ReUsedInfo.Size = 192 * 1024;
                ReUsedInfo.Used = 0;
                ReUsedInfo.UsedPercent = 0;
                McuResourceSize.Add(eResourceType.eDRAM, ReUsedInfo);
            }


        }

        public List<OBJ> LookupSameParObj(List<OBJ> sourceObj,lookUpType lookupType, string[] Pars)
        {
            List<OBJ> tmplist = new List<OBJ>();
            foreach (OBJ ObjTmp in sourceObj)
            {

                switch (lookupType)
                {
                    case lookUpType.T_FILE:
                        break;
                    case lookUpType.T_MODULE:
                        string module = ObjTmp.AllocateModule;
                        if (Pars.Contains(module))
                        {
                            tmplist.Add(ObjTmp);
                        }
                        else 
                        {
                        }
                        break;
                    case lookUpType.T_SEG:
                        break;
                    case lookUpType.T_RESOURCE:
                        string source = ObjTmp.Resource.ToString();
                        if (Pars.Contains(source))
                        {
                            tmplist.Add(ObjTmp);
                        }
                        else 
                        {
                        }
                        break;
                    case lookUpType.T_MEM:
                        break;
                    default:
                        break;
                }
            }
            return tmplist;
        }
    }
}
