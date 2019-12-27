using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using MapAssist;

namespace MapAssist
{
    public partial class MapAssist : Form
    {
        FileRead fileRead_Map = new FileRead();

        bool FileModuleSaveEnable = false;
        public static  int testGvalue = 1;
        List<OBJ> PlayDetailObjs = new List<OBJ>();
        public MapAssist()
        {
            InitializeComponent();
        }
        private void MapAssist_Load(object sender, EventArgs e)
        {
            //===================UI init======================//
            //dataGridView_FileModule.Columns.Add("fileNumber", "Number");
            dataGridView_FileModule.Columns.Add("fileName",   "SourceFile");
            dataGridView_FileModule.Columns.Add("fileModule", "Module");

            //
            listBox_files.SelectionMode = SelectionMode.MultiExtended;

            checkedListBox_Resource.Items.Add(eResourceType.eCODE_FLASH);
            checkedListBox_Resource.Items.Add(eResourceType.eSRAM);
            checkedListBox_Resource.Items.Add(eResourceType.eDRAM);
            //===================Data Init======================//
            fileRead_Map.Init_Load();
        }
        private void button_AddFile_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog_Add = new OpenFileDialog();

            openFileDialog_Add.InitialDirectory = "./";
            openFileDialog_Add.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog_Add.FilterIndex = 2;
            openFileDialog_Add.RestoreDirectory = true;
            openFileDialog_Add.Multiselect = true;

            if (openFileDialog_Add.ShowDialog() == DialogResult.OK)
            {
                
                try
                {
                    string[] fileNames = openFileDialog_Add.FileNames;
                    
                    //append file to list box
                    foreach (string fileName in fileNames)
                    {
                       
                        if (listBox_files.Items.Contains(fileName))
                        {
                            
                        }
                        else
                        {
                            listBox_files.Items.Add(fileName);
                        }
                    }

                    //update data filelist from ui list box
                    ArrayList flist = new ArrayList();
                    foreach(string fName in listBox_files.Items)
                    {
                        flist.Add(fName);
                    }

                    fileNames = (string []) flist.ToArray(typeof(string));
                    fileRead_Map.UpdateFileList(fileNames);



                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }
        private void button_Delfile_Click(object sender, EventArgs e)
        {
            //delete list_box select file 
            for (int i = listBox_files.SelectedItems.Count - 1; i >= 0; i--)
            {
                object objTmp = listBox_files.SelectedItems[i];
                listBox_files.Items.Remove(objTmp);
                
            }
            
            //to update adta filelist 
            ArrayList fileNameArray = new ArrayList();
            foreach(string fileName in listBox_files.Items)
            {
                fileNameArray.Add(fileName);
            }
            string[] fileNames = (string[])fileNameArray.ToArray(typeof(string));


            fileRead_Map.UpdateFileList(fileNames);
        }
        private void button_Start_Click(object sender, EventArgs e)
        {
            fileRead_Map.Init_Start();
            RichTxtBox_Log.Clear();
            string logout = null;

            if (fileRead_Map.filesRead() == false)//to avoid case :no add file 
            {
                MessageBox.Show("Please add file");
                return;
            }

            //check ld and map
            ArrayList mapNameArray_Nold = new ArrayList();
            ArrayList mapName_All = new ArrayList();
            MapLdSameT checkResult = fileRead_Map.MapAndLdCheck(ref mapNameArray_Nold, ref mapName_All);

            //get mapName from Arraylist
            string strNoldmapNames = null;
            foreach (string mapName in mapNameArray_Nold)
            {
                strNoldmapNames += mapName + " \r\n";
            }
            string strAllmapNames = null;
            foreach (string mapName in mapNameArray_Nold)
            {
                strAllmapNames += mapName + " ";
            }

            //tips
            if (checkResult == MapLdSameT.NotLDExist)
            {
                logout = "Not LD Find to supplyment Mapfile:\r\n " + strNoldmapNames;
                Console.WriteLine(logout);
                MessageBox.Show(logout);
            }

            //when all mapfile in fileRead_Map.mapinfos find its ld file
            if (checkResult == MapLdSameT.AllLDExist)
            {
                logout = "Ready to Analysis :\r\n" + strNoldmapNames;
                RichTxtBox_Log.AppendText(logout);


                Dictionary<eResourceType, strUsage> McuResourceUsed = new Dictionary<eResourceType, strUsage>();
                fileRead_Map.CalculateMcuResourceUsed(ref McuResourceUsed);

                //----------Display McuResourceUsed-----------------//
                int ResourceALign = 20;
                string Logout = "\r\n==========Mcu Resource Used:===========\r\n";
                Logout += "Resource".PadRight(ResourceALign) + "Used".PadRight(ResourceALign) + "UsedPercent".PadRight(ResourceALign) + "\r\n";
                foreach (eResourceType resource in McuResourceUsed.Keys.ToArray())
                {
                    Logout += resource.ToString().PadRight(ResourceALign) +
                             McuResourceUsed[resource].Used.ToString().PadRight(ResourceALign) +
                             McuResourceUsed[resource].UsedPercent.ToString("0.00%").PadRight(ResourceALign) +
                             "\r\n";


                }
                RichTxtBox_Log.AppendText(Logout);
                //----------Display McuResourceUsed End-----------------//


                Dictionary<string, List<MEMORY>> MapLd_MemUsed = new Dictionary<string, List<MEMORY>>();
                fileRead_Map.CalculateMemoryUsageInEachMapLd(ref MapLd_MemUsed);

                int memUsedALign = 30;
                Logout = "\r\n=================Each Map LD Memory Used:=================\r\n";

                foreach (string mapld in MapLd_MemUsed.Keys.ToArray())
                {
                    Logout += "\r\nMap LD:" + mapld + "\r\n";
                    Logout += "Memory".PadRight(memUsedALign) + "Used".PadRight(memUsedALign) + "UsedPercent".PadRight(memUsedALign) + "\r\n";
                    foreach (MEMORY memTmp in MapLd_MemUsed[mapld])
                    {

                        Logout += memTmp.Name.ToString().PadRight(memUsedALign) +
                                 memTmp.memUsage.Used.ToString().PadRight(memUsedALign) +
                                 memTmp.memUsage.UsedPercent.ToString("0.00%").PadRight(memUsedALign) +
                                 "\r\n";
                    }


                }
                RichTxtBox_Log.AppendText(Logout);
                //----------Display McuResourceUsed End-----------------//


                //save map file Name to fileModule and Update UI DataGridView_
                string fpath = "./fileModule.cfg";
                if (File.Exists(fpath))
                {
                    fileRead_Map.ReadCfgFileMoldule(fpath);
                }
                else 
                {
                    fileRead_Map.AddFileNameIntoSourceFileModule();
                }

                //dataGridView_FileModule_DataSet(fileRead_Map.SourceFileModule);
                dataGridView_FileModule.Rows.Clear();
                //int number = 1;
                foreach (string fileName in fileRead_Map.SourceFileModule.Keys.ToArray())
                {
                    //    DataGridViewRow dataRow = new DataGridViewRow();
                    //    dataRow.Cells.AddRange();
                    //    dataRow.Cells[0].Value = fileName;
                    //    dataRow.Cells[1].Value = fileModule[fileName];
                    //    dataGridView_FileModule.Rows.Add(dataRow);
                    //dataGridView_FileModule.Rows.Add(number,fileName, fileRead_Map.SourceFileModule[fileName]);
                    dataGridView_FileModule.Rows.Add(fileName, fileRead_Map.SourceFileModule[fileName]);
                    //number++;
                }

                foreach (string module in fileRead_Map.SourceFileModule.Values.ToArray())
                {

                    if (checkedListBox_module.Items.Contains(module))
                    {
                    }
                    else
                    {
                        checkedListBox_module.Items.Add(module);
                    }
                }
                FileModuleSaveEnable = true;

            }



        }
        private void button_ReadModuleCfg_Click(object sender, EventArgs e)
        {
            string fpath = "./fileModule.cfg";
            if (File.Exists(fpath))
            {
                //ReadCfgFromFiletoDic(fpath, ref fileRead_Map.SourceFileModule);


                fileRead_Map.ReadCfgFileMoldule(fpath);


                //dataGridView_FileModule_DataSet(fileRead_Map.SourceFileModule);
                dataGridView_FileModule.Rows.Clear();
                //int number = 1;
                foreach (string fileName in fileRead_Map.SourceFileModule.Keys.ToArray())
                {
                    //dataGridView_FileModule.Rows.Add(number,fileName, fileRead_Map.SourceFileModule[fileName]);
                    dataGridView_FileModule.Rows.Add(fileName, fileRead_Map.SourceFileModule[fileName]);
                    //number++;
                }


                //
                fileRead_Map.SetMapInfoObjModule();

                //
                //checklist_Module_Update(fileRead_Map.SourceFileModule);
                foreach (string module in fileRead_Map.SourceFileModule.Values.ToArray())
                {

                    if (checkedListBox_module.Items.Contains(module))
                    {

                    }
                    else
                    {
                        checkedListBox_module.Items.Add(module);
                    }

                }
                //enable save
                FileModuleSaveEnable = true;
                fileRead_Map.SourceFileModuleCfged = true;

            }
            else
            {
                MessageBox.Show("Configure file not exist");
            }
        }
        private void button_SaveModuleCfg_Click(object sender, EventArgs e)
        {

            if (FileModuleSaveEnable == false)
            {
                return;
            }

            string fpath = "./fileModule.cfg";
            if (File.Exists(fpath) == false)
            {
                File.Create(fpath);
            }
            else
            {
                
            }
            
            //save dataGridView into dictionary SourceFileModule and than write int cfg file
            //GetCfgFrom_dataGridView_FileModule(ref fileRead_Map.SourceFileModule);
            foreach (DataGridViewRow dataRow in dataGridView_FileModule.Rows)
            {
                string fName = null;
                string fModule = null;

                if (dataRow.Cells[0].Value != null)
                {
                    fName = dataRow.Cells[0].Value.ToString();

                    if (dataRow.Cells[1].Value != null)
                    {
                        fModule = dataRow.Cells[1].Value.ToString();
                    }
                    else
                    {
                        fModule = "Module_TBD";
                    }

                    //add file module
                    if (fileRead_Map.SourceFileModule.ContainsKey(fName))
                    {
                        fileRead_Map.SourceFileModule[fName] = fModule;
                    }
                    else
                    {
                        fileRead_Map.SourceFileModule.Add(fName, fModule);
                    }
                }
            }

            //WriteCfgFromDictoFile(fpath, fileRead_Map.SourceFileModule);
            StreamWriter streamW = new StreamWriter(fpath);
            string fline = null;

            foreach (string fName in fileRead_Map.SourceFileModule.Keys.ToArray())
            {
                fline = fName.PadRight(30) + "," + fileRead_Map.SourceFileModule[fName].PadRight(20);
                streamW.WriteLine(fline);
            }
            streamW.Close();


            //checklist_Module_Update(fileRead_Map.SourceFileModule);
            foreach (string module in fileRead_Map.SourceFileModule.Values.ToArray())
            {

                if (checkedListBox_module.Items.Contains(module))
                {

                }
                else
                {
                    checkedListBox_module.Items.Add(module);
                }

            }
            fileRead_Map.SourceFileModuleCfged = true;

            //call module set to supplyment map obj info
            fileRead_Map.SetMapInfoObjModule();

            
        }
        private void button_AnalysisFilterModule_Click(object sender, EventArgs e)
        {
            RichTxtBox_Log.Clear();
            fileRead_Map.SupplymentMapByLd();

            List<OBJ> AnalysisObjs = new List<OBJ>();
            fileRead_Map.CombineAllObjInMaps(ref AnalysisObjs);

            //not configed
            if (fileRead_Map.SourceFileModuleCfged == false)
            {
                MessageBox.Show("Configure FileModule");
                return;
            }

            //if configed ,then get select item to module
            ArrayList selectedModule = new ArrayList();
            if (checkedListBox_module.CheckedItems.Count > 0)
            {
            foreach (string module in checkedListBox_module.CheckedItems)
            {
                selectedModule.Add(module);
                }
            }
            else 
            {

            }
            string[] selmodule = (string[])selectedModule.ToArray(typeof(string));
            AnalysisObjs = fileRead_Map.LookupSameParObj(AnalysisObjs, lookUpType.T_MODULE, selmodule);
            //cal module used resource // ---//display resource
            ArrayList selectedSource = new ArrayList();
            if (checkedListBox_Resource.CheckedItems.Count > 0)
            {
                foreach (eResourceType source in checkedListBox_Resource.CheckedItems)
                {
                    selectedSource.Add(source.ToString());
                }
            }
            else
            {

            }
            string[] selsource = (string[])selectedSource.ToArray(typeof(string));
            AnalysisObjs = fileRead_Map.LookupSameParObj(AnalysisObjs, lookUpType.T_RESOURCE, selsource);
            foreach (string module in selectedModule)
            {
                Dictionary<eResourceType, strUsage> ModuleResourceUsed = new Dictionary<eResourceType, strUsage>();
                fileRead_Map.CalObjResourceUsed(AnalysisObjs, ref ModuleResourceUsed);
                int ResourceALign = 20;
                string Logout = "\r\n==========" + module.ToString() +" Resource Used:===========\r\n";
                Logout += "Resource".PadRight(ResourceALign) + "Used".PadRight(ResourceALign) + "UsedPercent".PadRight(ResourceALign) + "\r\n";
                foreach (eResourceType resource in ModuleResourceUsed.Keys.ToArray())
                {
                    Logout += resource.ToString().PadRight(ResourceALign) +
                             ModuleResourceUsed[resource].Used.ToString().PadRight(ResourceALign) +
                             ModuleResourceUsed[resource].UsedPercent.ToString("0.00%").PadRight(ResourceALign) +
                             "\r\n";
                }
                RichTxtBox_Log.AppendText(Logout);
            }
            PlayDetailObjs = AnalysisObjs;
            if (PlayDetailObjs.Count > 0)
            {
                dataGridView_detail.DataSource = PlayDetailObjs;
            }
            else
            {
            }
           

        }
        private void checkBox_ModuleAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ModuleAll.CheckState == CheckState.Checked)
            {
                if (checkedListBox_module.Items.Count > 0)
                {
                    for (int i = 0; i < checkedListBox_module.Items.Count; i++)
                    {
                        checkedListBox_module.SetItemCheckState(i, CheckState.Checked);
                    }

                }

            }
            else if (checkBox_ModuleAll.CheckState == CheckState.Unchecked)
            {
                if (checkedListBox_module.Items.Count > 0)
                {
                    for (int i = 0; i < checkedListBox_module.Items.Count; i++)
                    {
                        checkedListBox_module.SetItemCheckState(i, CheckState.Unchecked);
                    }

                }
            }
            else
            {
                //
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("HASCO SoftWare tools");
        }
        private void button_Detail_Click(object sender, EventArgs e)
        {
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                