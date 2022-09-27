using MSIWMIACPI2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsiWmiECDumper
{
    public partial class Edit : Form
    {
        private readonly WmiAcpiLib2 wmi = new WmiAcpiLib2();
        public Edit()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            var data = new byte[1];
            data[0] = (byte)value.Value;
            wmi.InvokeWmiMethod(WmiAcpiLib2.Methods.Set_Data, (byte)addr.Value, data);
        }
    }
}
