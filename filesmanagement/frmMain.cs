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

        private void btnOrganize_Click(object sender, EventArgs e)
        {
            btnOrganize.Enabled = false;
            richTxtBoxStatus.Clear();

            LinkedList<string> _failed_finallist = null;
            String source = null;
            String destination = null;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                source = folderBrowserDialog1.SelectedPath;
                richTxtBoxStatus.AppendText(String.Format("Source selected: {0}!!",source));
            }
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                destination = folderBrowserDialog1.SelectedPath;
                richTxtBoxStatus.AppendText(String.Format("\nWorkspace selected: {0}!!", destination));
            }

            Task t_destination = Task.Run(() => WorkSpace.Create(destination));

            // Do some more processing

            try
            {
                t_destination.Wait();
                if (t_destination.IsCompleted)
                {
                    richTxtBoxStatus.AppendText("\nWorkspace created successfully!!");
                }
                Task<LinkedList<string>> t_organize = Task.Run(() => WorkSpace.Organize(source, destination));
                t_organize.Wait();
                if (t_organize.IsCompleted)
                {
                    _failed_finallist = t_organize.Result;
                    if(_failed_finallist != null && _failed_finallist.Count > 0)
                    {
                        richTxtBoxStatus.AppendText("\nFailed to copy the below files and directories:");
                        foreach (string _ff in _failed_finallist)
                        {
                            richTxtBoxStatus.AppendText(String.Format("\n{0}", _ff));
                        }
                    }
                    else
                    {
                        richTxtBoxStatus.AppendText("\nSource is organized successfully!!");
                    }
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
                            richTxtBoxStatus.AppendText("\nThe below directories were cleanedup:");
                            foreach (string _ff in _finallist)
                            {
                                richTxtBoxStatus.AppendText(String.Format("\n{0}", _ff));
                            }
                        }
                        else
                        {
                            richTxtBoxStatus.AppendText("\nNo directory found empty!!");
                        }
                    }
                    richTxtBoxStatus.AppendText("\nCleanup completed successfully!!");
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
