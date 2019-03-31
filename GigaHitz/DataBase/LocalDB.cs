using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace GigaHitz.DataBase
{
    public class LocalDB
    {
        string filename;
        string line;
        string text;
        List<string> tmp;

        public LocalDB()
        {
            var document = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            filename = Path.Combine(document, "Option.txt");
        }

        public bool IsExist()
        {
            return File.Exists(filename);
        }

        public void AddItem<T>(T param)
        {
            if (line != null)
                line += string.Format(",{0}", param);
            else
                line = param.ToString();
        }

        public void Write()
        {
            if (line != null && filename != null)
                File.WriteAllText(filename, line);
        }

        public void GetContent()
        {
            if (filename != null && text == null)
                text = File.ReadAllText(filename);
            if (text != null)
            {
                tmp = new List<string>(text.Split(','));
            }
        }

        public bool Read(out int param)
        {
            if(tmp != null)
            {
                try
                {
                    param = int.Parse(tmp[0]);
                }
                catch(Exception)
                {
                    param = -1;
                    return false;
                }

                tmp.RemoveAt(0);
                return true;
            }

            param = -1;
            return false;
        }

        public bool Read(out float param)
        {
            if (tmp != null)
            {
                try
                {
                    param = float.Parse(tmp[0]);
                }
                catch (Exception)
                {
                    param = float.NaN;
                    return false;
                }

                tmp.RemoveAt(0);
                return true;
            }

            param = float.NaN;
            return false;
        }

        public bool Read(out double param)
        {
            if (tmp != null)
            {
                try
                {
                    param = double.Parse(tmp[0]);
                }
                catch (Exception)
                {
                    param = double.NaN;
                    return false;
                }

                tmp.RemoveAt(0);
                return true;
            }

            param = double.NaN;
            return false;
        }
    }
}
