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
            LinkedList<FileInfo> _finallist = null;
            LinkedList<FileInfo> _failed_finallist = null;
            String source = null;
            String destination = null;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Source selected!!");
                source = folderBrowserDialog1.SelectedPath;
            }
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                destination = folderBrowserDialog1.SelectedPath;
                MessageBox.Show("Workspace selected!!");
            }

            Task<LinkedList<FileInfo>> t_source = Task<LinkedList<FileInfo>>.Run(() => WorkSpace.Analysis2(source));
            Task t_destination = Task.Run(() => WorkSpace.Create(destination));

            // Do some processing

            try
            {
                t_destination.Wait();
                if (t_destination.IsCompleted)
                {
                    MessageBox.Show("Workspace created successfully!!");
                }
                t_source.Wait();
                if (t_source.IsCompleted)
                {
                    _finallist = t_source.Result;
                    MessageBox.Show("Files to be organized are ready!!");
                }
                Task<LinkedList<FileInfo>> t_organize = Task<LinkedList<FileInfo>>.Run(() => WorkSpace.Organize(_finallist,destination));
                t_organize.Wait();
                if (t_organize.IsCompleted)
                {
                    _failed_finallist = t_organize.Result;
                    MessageBox.Show("Files were organized are successfully!!");
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
}
