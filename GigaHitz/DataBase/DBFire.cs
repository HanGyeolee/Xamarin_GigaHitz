using System;


namespace GigaHitz.DataBase
{
    public class DBFire
    {
        private string root;
        public DBFire(string _root)
        {
            root = _root;
        }

        public string GetVersion()
        {
            return root + "/Config/UpdatedVersion";
        }

        public DBFire GetChildByName(string child)
        {
            return new DBFire(root + "/" + child);
        }

        public string GetJSON()
        {
            return root + ".json";
        }
    }
}
