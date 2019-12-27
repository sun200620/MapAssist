using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAssist
{

    class OBJ:IComparable<OBJ>
    {

        private string objName;

        public string ObjName
        {
            get { return objName; }
            set { objName = value; }
        }
        /// <summary>
        /// get from map when read one line 
        /// </summary>
        private string secName;

        public string SecName
        {
            get { return secName; }
            set { secName = value; }
        }
        public int SecAddr;
        private int secSize;

        public int SecSize
        {
            get { return secSize; }
            set { secSize = value; }
        }
        
        /// <summary>
        /// get from ld when read
        /// </summary>
        public string MemName;
        public int MemOrg;
        public int MemLen;

        private eResourceType resource;

        public eResourceType Resource
        {
            get { return resource; }
            set { resource = value; }
        }

        //get from ui
        private string allocateModule;

        public string AllocateModule
        {
            get { return allocateModule; }
            set { allocateModule = value; }
        }
        //public string Responsible;

        //file
        public string ObjPath;
        
        public string MapFileName;
        public string LdFileName;


        override public Boolean Equals(object o)
        {
            OBJ obj = (OBJ)o;
            if (obj.objName == this.objName &&
                //obj.ObjPath == this.ObjPath &&
                obj.secName == this.secName &&
                obj.SecAddr == this.SecAddr
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CompareTo(OBJ other)
        {
            return secSize.CompareTo(other.secSize);
            //throw new NotImplementedException();
        }
        public void SetObjMemInfor(MEMORY mem)
        {
            MemName  = mem.Name;
            MemOrg   = mem.Org;
            MemLen   = mem.Len;
            resource = mem.Resource;
        }
        public void SetObjFileInfor(string mapName,string ldName)
        {
            MapFileName = mapName;
            LdFileName = ldName;
        }
        public void SetObjFileModule(string moduleName)
        {
            allocateModule = moduleName;
        }
    };

}
