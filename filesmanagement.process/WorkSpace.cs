using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Logging;

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
                try
                {
                    string[] files = Directory.GetFiles(path);
                    if (files != null && files.Length > 0)
                    {
                        foreach (string file in files)
                        {
                            if (File.Exists(file))
                            {
                                _list.AddLast(new FileInfo(file));
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    // suppress and continue after this exception
                    Logger.Write(String.Format("Cannot get files for the directory: {0} due to exception: {1}", path, ex.ToString()));
                }

                try
                {
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
                catch(Exception ex)
                {
                    // suppress and continue after this exception
                    Logger.Write(String.Format("Cannot get child directories for this parent directory: {0} due to exception: {1}", path, ex.ToString()));
                }
            }
            return _list;
        }

        public static void Create(String DestinationPath)
        {
            if (String.IsNullOrEmpty(DestinationPath))
            {
                throw new Exception("Creating workspace operation aborted as destination workspace not specified or exists");
            }

            try
            {
                if (!Directory.Exists(DestinationPath))
                {
                    // Create initial destination root workspace folder
                    Directory.CreateDirectory(DestinationPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(String.Format("Creating workspace directory {0} failed due to exception: {1}", DestinationPath, ex.ToString()));
                throw ex;
            }

            // creating initial data for workspace after destination path (root path) looks good
            String[] _months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            DateTime dt = DateTime.Now;
            int _year = dt.Year;
            int _month = dt.Month;
            int _backyear = _year - 6;
            int _backmonth = 0;

            // creating year and month folders
            while (_backyear <= _year)
            {
                String year_path = Path.Combine(DestinationPath, _backyear.ToString());
                if (!Directory.Exists(year_path))
                {
                    // creating year folders
                    Directory.CreateDirectory(year_path);
                }

                for (_backmonth=0; _backmonth<_months.Length; _backmonth++)
                {
                    if (!((_backmonth >= _month) && (_backyear == _year)))
                    {
                        String month_path = Path.Combine(year_path, _months[_backmonth]);
                        if (!Directory.Exists(month_path))
                        {
                            // Create month folder
                            Directory.CreateDirectory(month_path);
                        }
                    }
                }

                // keep increment the year+1
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

        public static void Organize(LinkedList<FileInfo> fileInfo, string WorkspacePath)
        {
            if (fileInfo != null && fileInfo.Count > 0 && !String.IsNullOrEmpty(WorkspacePath))
            {
                String[] _months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                foreach (FileInfo _file in fileInfo)
                {
                    int _year = _file.LastWriteTime.Year;
                    int _month = _file.LastWriteTime.Month;
                    String _DestinationPath = Path.Combine(WorkspacePath, _year.ToString(), _months[_month - 1], _file.Name);
                    try
                    {
                        _file.MoveTo(_DestinationPath);
                    }
                    catch(Exception ex)
                    {
                        // continue and suppress the exceptions
                        Logger.Write(String.Format("Copy failed to destination path: {0) due to exception: {1}", _DestinationPath, ex.ToString()));
                    }
                }
            }
            else
            {
                Logger.Write("Operation failed as list of files or destination workspace not specified or exists");
            }
        }

        public static void Organize(LinkedList<string> fileInfo, string WorkspacePath)
        {
            if (fileInfo != null && fileInfo.Count > 0 && !String.IsNullOrEmpty(WorkspacePath))
            {
                String[] _months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                foreach (string _f in fileInfo)
                {
                    FileInfo _file = new FileInfo(_f);
                    int _year = _file.LastWriteTime.Year;
                    int _month = _file.LastWriteTime.Month;
                    String _DestinationPath = Path.Combine(WorkspacePath, _year.ToString(), _months[_month - 1], _file.Name);
                    try
                    {
                        _file.MoveTo(_DestinationPath);
                    }
                    catch (Exception ex)
                    {
                        // continue and suppress the exceptions
                        Logger.Write(String.Format("Copy failed to destination path: {0) due to exception: {1}", _DestinationPath, ex.ToString()));
                    }
                }
            }
            else
            {
                Logger.Write("Operation failed as list of files or destination workspace not specified or exists");
            }
        }

        public static LinkedList<string> Organize(string SourcePath, string WorkspacePath)
        {
            String[] _months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            LinkedList<string> _notprocessed = new LinkedList<string>();
            if (!String.IsNullOrEmpty(SourcePath) && !String.IsNullOrEmpty(WorkspacePath) && Directory.Exists(SourcePath) && Directory.Exists(WorkspacePath))
            {
                try
                {
                    string[] files = Directory.GetFiles(SourcePath);
                    if (files != null && files.Length > 0)
                    {
                        foreach (string file in files)
                        {
                            FileInfo _file = new FileInfo(file);
                            int _year = _file.LastWriteTime.Year;
                            int _month = _file.LastWriteTime.Month;
                            String _DestinationPath = Path.Combine(WorkspacePath, _year.ToString(), _months[_month - 1], _file.Name);
                            try
                            {
                                if (!File.Exists(_DestinationPath))
                                {
                                    // move to the destination
                                    _file.MoveTo(_DestinationPath);
                                }
                                else
                                {
                                    // Rename this file to copy from source and delete it once sucessfully copied
                                    String newName = String.Empty;
                                    if (_file.Name.LastIndexOf('.') > -1)
                                    {
                                        newName = String.Format("{0}-{1}{2}", _file.Name.Substring(0, _file.Name.LastIndexOf('.')), Guid.NewGuid().ToString(), _file.Extension);
                                    }
                                    else
                                    {
                                        newName = String.Format("{0}-{1}{2}", _file.Name, Guid.NewGuid().ToString(), _file.Extension);
                                    }
                                    _DestinationPath = Path.Combine(WorkspacePath, _year.ToString(), _months[_month - 1], newName);
                                    _file.CopyTo(_DestinationPath);
                                    _file.Delete();
                                }
                            }
                            catch (Exception ex)
                            {
                                // suppress and continue after this exception
                                Logger.Write(String.Format("Cannot copy file to the destination {0} due to exception: {1}", file, ex.ToString()));
                                _notprocessed.AddLast(_file.FullName);
                            }
                            finally
                            {
                                _file = null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // suppress and continue after this exception
                    Logger.Write(String.Format("Cannot get files for the directory: {0} due to exception: {1}", SourcePath, ex.ToString()));
                    _notprocessed.AddLast(SourcePath);
                }

                try
                {
                    string[] directories = Directory.GetDirectories(SourcePath);
                    if (directories != null && directories.Length > 0)
                    {
                        foreach (string directory in directories)
                        {
                            LinkedList<string> _deepList = Organize(directory, WorkspacePath);
                            if (_deepList != null && _deepList.Count > 0)
                            {
                                foreach (string file in _deepList)
                                {
                                    _notprocessed.AddLast(file);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // suppress and continue after this exception
                    Logger.Write(String.Format("Cannot get child directories for this parent directory: {0} due to exception: {1}", SourcePath, ex.ToString()));
                }
                finally
                {
                    //_notprocessed.AddLast(SourcePath);
                }

                try
                {
                    Directory.Delete(SourcePath);
                }
                catch (Exception ex)
                {
                    // suppress and continue after this exception
                    Logger.Write(String.Format("Cannot delete directory: {0} due to exception: {1}", SourcePath, ex.ToString()));
                }
            }
            else
            {
                Logger.Write("Operation failed as Source and Destination workspace not specified or exists");
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
