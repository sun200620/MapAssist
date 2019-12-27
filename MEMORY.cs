using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAssist
{

    class MEMORY
    {
        //get from ld
        string name;
        int org;
        int len;

        //for configure
        public eResourceType Resource;

        //for cal
        public strUsage memUsage;
        //public int Used;
       // public double UsedPercent;
        //get from ui mem allocate RAM Flash


        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Org
        {
            get { return org; }
            set { org = value; }
        }
        public int Len
        {
            get { return len; }
            set { len = value; }
        }


        
        /// <summary>
        /// auto allocate mem to resource
        /// </summary>
        /// 0x00800000-0x0080FFFF EEROM          :64K
        /// 0x00A00000-0x00FFFFFF BootFlash      :416K
        /// 0x01000000-0x0117FFFF CodeFlash      :1536K
        /// 0x40000000-0x4017FFFF SRAM           :1536K
        /// 0x50800000-0x5082FFFF DMEM           :192K
        /// 
        public void SetMemResource()
        {
            int Addr = Org;

            if ((Addr >= Convert.ToInt32(0x00800000)) &&
                (Addr <= Convert.ToInt32(0x0080FFFF)))
            {
                Resource = eResourceType.eEEROM;
            }
            else if ((Addr >= Convert.ToInt32(0x00A00000)) &&
                     (Addr <= Convert.ToInt32(0x00FFFFFF)))
            {
                Resource = eResourceType.eBOOT_FLASH;
            }
            else if ((Addr >= Convert.ToInt32(0x01000000)) &&
                     (Addr <= Convert.ToInt32(0x0117FFFF)))
            {
                Resource = eResourceType.eCODE_FLASH;
            }
            else if ((Addr >= Convert.ToInt32(0x40000000)) &&
                     (Addr <= Convert.ToInt32(0x4017FFFF)))
            {
                Resource = eResourceType.eSRAM;
            }
            else if ((Addr >= Convert.ToInt32(0x50800000)) &&
                     (Addr <= Convert.ToInt32(0x5082FFFF)))
            {
                Resource = eResourceType.eDRAM;
            }
            else
            {
                Console.WriteLine("set resource error,Org: {0}exceed MCU Range", Addr);
            }
        }

    }
}
