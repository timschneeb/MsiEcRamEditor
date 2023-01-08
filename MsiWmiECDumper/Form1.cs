using Be.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Logging;
using MSIWMIACPI2;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MsiWmiECDumper
{
    public partial class Form1 : Form
    {
        private byte[] buffer = new byte[256];
        private Task dumpTask;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private bool bufferInit = false;
        private ArrayList filteredAddresses = new ArrayList();
        private string lastFilterInput = "";

        public Form1()
        {

            var isAdmin = false;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            // Suppress WmiAcpiLib2 log spam
            Console.SetOut(TextWriter.Null);

            InitializeComponent();
            var byteProvider = new DynamicByteProvider(new byte[256]);
            hexBox.ByteProvider = byteProvider;
            hexBox.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, SystemFonts.MessageBoxFont.Size, SystemFonts.MessageBoxFont.Style);
            hexBox.ReadOnly = true;


            toolStripStatusLabel.Text = "PLEASE WAIT...";
            listView1.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);

            dumpTask = Task.Run(() =>
            {
                DumpTask(); 
            });

            // Check with a simple condition whether you are admin or not
            if (!isAdmin)
            {
                MessageBox.Show("Access to EC RAM denied. Please restart this app with administrator privileges.", "MSI EC RAM Dump", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        void RunOnUI(Action a)
        {
            var form = Application.OpenForms[0];
            if (form.InvokeRequired)
            {
                form.Invoke(a);
            }
        }

        void DumpTask()
        {
            var wmi = new WmiAcpiLib2();
            var bitInfo = new BitInfo((byte)0, 0);
            var initial = true;

            while (!cts.IsCancellationRequested)
            {
                var priv = new byte[256];
                for (int i = 0; i <= 255; i++)
                {
                    var result = wmi.InvokeWmiMethod(WmiAcpiLib2.Methods.Get_Data, (byte)i, new byte[1]);
                    if (result.Flag == 1)
                    {
                        priv[i] = result.Data[0];

                    }
                    else
                    {
                        Debug.WriteLine($"WARNING: EC memory 0x{string.Format("{0:X2}", i)} couldn't be read");
                    }
                }


                for (int i = 0; i <= 255; i++)
                {
                    var x = priv[i];
                    if (x != hexBox.ByteProvider.ReadByte(i))
                    {
                        hexBox.ByteProvider.WriteByte(i, x);

                        if (initial || filteredAddresses.Contains(i))
                            continue;

                        bitInfo.Value = x;


                        listView1.Invoke(new Action(() => {
                            listView1.Items.Add(new ListViewItem(new[] { $"0x{string.Format("{0:X2}", i)}", $"0x{string.Format("{0:X2}", x)} ({bitInfo}b; {x}d)" }));
                            if(listView1.Items.Count > 0)
                                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            if (listView1.Items.Count > 1000)
                                listView1.Items.RemoveAt(0);
                        }));
                     }
                }

                initial = false;
                hexBox.Invalidate();

                lock (buffer)
                {
                    Array.Copy(priv, buffer, 256);
                }


                if (!bufferInit)
                {
                    this.Invoke(new Action(() => {
                        toolStripStatusLabel.Text = "Ready";
                    }));
                 }
                bufferInit = true;

                Thread.Sleep(10);
            }
        }

        void Position_Changed(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = string.Format("Ln {0}; Col {1}",
                hexBox.CurrentLine, hexBox.CurrentPositionInLine);

            string bytePresentation = string.Empty;

            byte? currentByte = hexBox.ByteProvider != null && hexBox.ByteProvider.Length > hexBox.SelectionStart
                ? hexBox.ByteProvider.ReadByte(hexBox.SelectionStart)
                : (byte?)null;

            BitInfo bitInfo = currentByte != null ? new BitInfo((byte)currentByte, hexBox.SelectionStart) : null;

            bytePresentation = $"Value 0x{string.Format("{0:X2}", currentByte)}; ";

            if (bitInfo != null)
            {
                bytePresentation += $"{bitInfo.ToString()}b; ";
            }

            bytePresentation += $"{currentByte}d";

            this.bitValueToolStripStatusLabel.Text = bytePresentation;
            this.bitToolStripStatusLabel.Text = $"Offset 0x{string.Format("{0:X2}", hexBox.SelectionStart)}";
        }


        void hexBox_RequiredWidthChanged(object sender, EventArgs e)
        {
            UpdateFormWidth();
        }

        void UpdateFormWidth()
        {
            this.Width = this.hexBox.RequiredWidth + 100;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hexBox.ByteProvider == null)
                return;

            if(!bufferInit)
            {
                MessageBox.Show("Buffer empty. No data received yet");
                return;
            }

            string input = Interaction.InputBox("Enter file name", "Save", "ec_dump_" + DateTime.Now.ToString("yyyyMMddHHmmssffff"));


            var dynamicByteProvider = hexBox.ByteProvider as DynamicByteProvider;
            using (BinaryWriter binWriter = new BinaryWriter(File.Open(input + ".bin", FileMode.Create)))
            {
                lock (buffer)
                {
                    binWriter.Write(buffer);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
            dumpTask.Wait(200);
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Edit().Show();
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void filterAddressesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Enter comma-separated list of memory addresses (1 byte; HEX) to exclude. Example: 45,1d,ff", "Confirm", lastFilterInput);
            lastFilterInput = input;
            var items = input.Split(',');
            filteredAddresses.Clear();
            foreach(var item in items)
            {
                var clean = item.Trim();
                if (clean.Count() != 1)
                    continue;
                filteredAddresses.Add(Convert.ToByte(clean));
            }
        }
    }
}
