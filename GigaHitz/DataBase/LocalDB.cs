using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace GigaHitz.DataBase
{
    public class LocalDB
    {
        readonly string filename;
        string line;
        string text;
        List<string> tmp;

        public LocalDB()
        {
            var document = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            filename = Path.Combine(document, "Option.txt");
        }

        private bool GetContent()
        {
            if (filename != null)
                text = File.ReadAllText(filename);
            if (text != null)
            {
                tmp = new List<string>(text.Split(','));
                return true;
            }
            return false;
        }

        public bool IsExist()
        {
            if (File.Exists(filename))
            {
                GetContent();
                if (tmp.Count < 4)
                    return false;

                return true;
            }
            return false;
        }

        public void ClearItem()
        {
            line = null;
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

        public bool Read(out int param, int index = 0)
        {
            if (tmp != null)
            {
                try
                {
                    param = int.Parse(tmp[index]);
                }
                catch (Exception)
                {
                    param = -1;
                    return false;
                }
                return true;
            }

            param = -1;
            return false;
        }

        public bool Read(out float param, int index = 0)
        {
            if (tmp != null)
            {
                try
                {
                    param = float.Parse(tmp[index]);
                }
                catch (Exception)
                {
                    param = float.NaN;
                    return false;
                }
                return true;
            }

            param = float.NaN;
            return false;
        }

        public bool Read(out double param, int index = 0)
        {
            if (tmp != null)
            {
                try
                {
                    param = double.Parse(tmp[index]);
                }
                catch (Exception)
                {
                    param = double.NaN;
                    return false;
                }
                return true;
            }

            param = double.NaN;
            return false;
        }

        public bool Read(out bool param, int index = 0)
        {
            if (tmp != null)
            {
                try
                {
                    param = bool.Parse(tmp[index]);
                }
                catch (Exception)
                {
                    param = false;
                    return false;
                }
                return true;
            }

            param = false;
            return false;
        }

        public bool Read(out string param, int index = 0)
        {
            if (tmp != null)
            {
                try
                {
                    param = tmp[index];
                }
                catch (Exception)
                {
                    param = null;
                    return false;
                }
                return true;
            }

            param = null;
            return false;
        }

        public string ReadIndexOf(int index)
        {
            return tmp[index];
        }
    }
}
