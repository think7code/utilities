using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections; 

namespace filesmanagement.process
{

    public class WorkSpaceReport
    {
        private int no_of_files;
        private int no_of_Directories;
        private LinkedList<string> types = new LinkedList<string>();
        private int size;
        private DateTime mindt;
        private DateTime maxdt;
        private LinkedList<FileInfo> _fileList = new LinkedList<FileInfo>();

        public int No_of_files
        {
            get
            {
                return no_of_files;
            }

            set
            {
                no_of_files = value;
            }
        }

        public int No_of_Directories
        {
            get
            {
                return no_of_Directories;
            }

            set
            {
                no_of_Directories = value;
            }
        }

        public LinkedList<string> Types
        {
            get
            {
                return types;
            }

            set
            {
                types = value;
            }
        }

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public DateTime FirstDirectoryOrFileCreationTime
        {
            get
            {
                return mindt;
            }

            set
            {
                mindt = value;
            }
        }

        public DateTime LastDirectoryOrFileCreationTime
        {
            get
            {
                return maxdt;
            }

            set
            {
                maxdt = value;
            }
        }

        public LinkedList<FileInfo> FileList
        {
            get
            {
                return _fileList;
            }

            set
            {
                _fileList = value;
            }
        }
    }

    public static class WorkSpace
    {
        public static LinkedList<string> Analysis(string path)
        {
            LinkedList<string> _list = new LinkedList<string>();
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                if(files!=null && files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        _list.AddLast(file);
                    }
                }
                string[] directories = Directory.GetDirectories(path);
                if(directories != null && directories.Length > 0)
                {
                    foreach(string directory in directories)
                    {
                        LinkedList<string> _deepList = Analysis(directory);
                        foreach(string s in _deepList)
                        {
                            _list.AddLast(s);
                        }
                    }
                }
            }
            return _list;
        }

        public static LinkedList<FileInfo> Analysis2(string path)
        {
            LinkedList<FileInfo> _list = new LinkedList<FileInfo>();
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                if (files != null && files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        if(File.Exists(file))
                        {
                            _list.AddLast(new FileInfo(file));
                        }
                    }
                }
                string[] directories = Directory.GetDirectories(path);
                if (directories != null && directories.Length > 0)
                {
                    foreach (string directory in directories)
                    {
                        LinkedList<FileInfo> _deepList = Analysis2(directory);
                        foreach (FileInfo s in _deepList)
                        {
                            _list.AddLast(s);
                        }
                    }
                }
            }
            return _list;
        }

        public static void Create(String DestinationPath)
        {
            String[] _months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            DateTime dt = DateTime.Now;
            int _year = dt.Year;
            int _month = dt.Month;
            int _backyear = _year - 6;
            int _backmonth = 0;

            if (String.IsNullOrEmpty(DestinationPath))
            {
                throw new Exception("Operation aborted as destination workspace not specified or exists");
            }
            if (!Directory.Exists(DestinationPath))
            {
                // Create initial destination root workspace folder
                Directory.CreateDirectory(DestinationPath);
            }
            while ((_backyear <= _year))
            {
                if (!Directory.Exists(String.Format("{0}\\{1}", DestinationPath, _backyear)))
                {
                    // Create Year Folder
                    Directory.CreateDirectory(String.Format("{0}\\{1}",DestinationPath,_backyear));
                }
                for (_backmonth=0; _backmonth < _months.Length; _backmonth++)
                {
                    if (!((_backmonth >= (_month) && (_backyear == _year))))
                    {
                        if (!Directory.Exists(String.Format("{0}\\{1}\\{2}", DestinationPath, _backyear, _months[_backmonth])))
                        {
                            // Create Month Folder
                            Directory.CreateDirectory(String.Format("{0}\\{1}\\{2}", DestinationPath, _backyear, _months[_backmonth]));
                        }
                    }
                }
                _backyear = _backyear + 1;
            }
        }

        public static void Backup()
        {

        }

        public static void SearchDuplicates()
        {

        }

        public static void CleanDuplicates()
        {

        }

        public static void Delete()
        {
        }

        public static void Copy()
        {
        }

        public static void Cut()
        {

        }

        public static LinkedList<FileInfo> Organize(LinkedList<FileInfo> fileInfo, String WorkspacePath)
        {
            LinkedList<FileInfo> _notprocessed = new LinkedList<FileInfo>();
            if (fileInfo != null && !String.IsNullOrEmpty(WorkspacePath))
            {
                String[] _months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                foreach (FileInfo _file in fileInfo)
                {
                    int _year = _file.LastWriteTime.Year;
                    int _month = _file.LastWriteTime.Month;
                    String _DestinationPath = String.Format("{0}\\{1}\\{2}\\{3}", WorkspacePath,_year,_months[_month-1],_file.Name);
                    try
                    {
                        _file.CopyTo(_DestinationPath);
                    }
                    catch(Exception ex)
                    {
                        // continue and suppress the exceptions
                        _notprocessed.AddLast(_file);
                    }
                }
            }
            return _notprocessed;
        }

        public static void Merge()
        {

        }

        public static void Search()
        {

        }
    }
}
