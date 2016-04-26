using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using filesmanagement.process;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace filesmanagement
{
    public partial class frmMain : Form
    {
        delegate void UpdateStatusCallback(String status);

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnCreateWS_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Task t = Task.Run(() => WorkSpace.Create(folderBrowserDialog1.SelectedPath));
                try
                {
                    t.Wait();
                    if (t.IsCompleted)
                    {
                        MessageBox.Show("Workspace created successfully!!");
                    }
                }
                catch (AggregateException aex)
                {
                    // Assume we know what's going on with this particular exception.
                    // Rethrow anything else. AggregateException.Handle provides
                    // another way to express this. See later example.
                    foreach (Exception ex in aex.InnerExceptions)
                    {
                        // log the exceptions
                        throw ex;
                    }
                }
            }
        }

        private void btnAnalysisWS_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Task<LinkedList<FileInfo>> t = Task<LinkedList<FileInfo>>.Run(() => WorkSpace.Analysis2(folderBrowserDialog1.SelectedPath));
                try
                {
                    t.Wait();
                    if (t.IsCompleted)
                    {
                        LinkedList<FileInfo> _finallist = t.Result;
                        MessageBox.Show("Analysis completed successfully!!");
                    }
                }
                catch (AggregateException aex)
                {
                    // Assume we know what's going on with this particular exception.
                    // Rethrow anything else. AggregateException.Handle provides
                    // another way to express this. See later example.
                    foreach (Exception ex in aex.InnerExceptions)
                    {
                        // log the exceptions
                        throw ex;
                    }
                }
            }
        }

        private void UpdateStatus(String status)
        {
            SetText(status);
        }

        private void SetText(String status)
        {
            if (richTxtBoxStatus.InvokeRequired)
            {
                UpdateStatusCallback d = new UpdateStatusCallback(UpdateStatus);
                this.Invoke(d, new object[] { status });
            }
            else
            {
                richTxtBoxStatus.AppendText(status);
            }
        }

        private void Process(LinkedList<string> final_list)
        {
            try
            {
                if (final_list != null && final_list.Count > 0)
                {
                    SetText("\nFailed to copy the below files and directories:");
                    foreach (string _ff in final_list)
                    {
                        SetText(String.Format("\n{0}", _ff));
                    }
                    SetText("\nOrganizing workspace completed with above unorganized files. please review or re-run the process again!!");
                }
                else
                {
                    SetText("\nSource is organized successfully!!");
                }
            }
            catch (Exception ex)
            {
                // log the exceptions
                Logger.Write("Log exception caused due to : {0}", ex.ToString());
            }
        }

        private void btnOrganize_Click(object sender, EventArgs e)
        {
            btnOrganize.Enabled = false;
            richTxtBoxStatus.Clear();

            String source = null;
            String destination = null;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                source = folderBrowserDialog1.SelectedPath;
                SetText(String.Format("Source selected: {0}!!",source));
            }
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                destination = folderBrowserDialog1.SelectedPath;
                SetText(String.Format("\nWorkspace selected: {0}!!", destination));
            }

            Task t_destination = Task.Run(() => WorkSpace.Create(destination));

            // Do some more processing

            try
            {
                t_destination.Wait();
                if (t_destination.IsCompleted)
                {
                    SetText("\nWorkspace created successfully!!");
                }
                Task<LinkedList<string>> t_organize = Task.Factory.StartNew(() => WorkSpace.Organize(source, destination));
                t_organize.ContinueWith(t => Process(t.Result));
                SetText("\nOrganizing workspace started....");
            }
            catch (AggregateException aex)
            {
                // Assume we know what's going on with this particular exception.
                // Rethrow anything else. AggregateException.Handle provides
                // another way to express this. See later example.
                foreach (Exception ex in aex.InnerExceptions)
                {
                    // log the exceptions
                    Logger.Write("Log exception caused due to : {0}", ex.ToString());
                }
            }
            btnOrganize.Enabled = true;
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Task<LinkedList<string>> t = Task<LinkedList<string>>.Run(() => WorkSpace.Clean(folderBrowserDialog1.SelectedPath));
                try
                {
                    t.Wait();
                    if (t.IsCompleted)
                    {
                        LinkedList<string> _finallist = t.Result;
                        if (_finallist != null && _finallist.Count > 0)
                        {
                            SetText("\nThe below directories were cleanedup:");
                            foreach (string _ff in _finallist)
                            {
                                SetText(String.Format("\n{0}", _ff));
                            }
                        }
                        else
                        {
                            SetText("\nNo directory found empty!!");
                        }
                    }
                    SetText("\nCleanup completed successfully!!");
                }
                catch (AggregateException aex)
                {
                    // Assume we know what's going on with this particular exception.
                    // Rethrow anything else. AggregateException.Handle provides
                    // another way to express this. See later example.
                    foreach (Exception ex in aex.InnerExceptions)
                    {
                        // log the exceptions
                        Logger.Write("Log exception caused due to : {0}", ex.ToString());
                    }
                }
            }
        }
    }
}
