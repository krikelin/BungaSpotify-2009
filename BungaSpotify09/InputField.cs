using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BungaSpotify09
{
    public partial class InputField : UserControl
    {
        public InputField()
        {
            InitializeComponent();
        }
        public enum InputFieldType
        {
            String, Integer
        }
        public InputFieldType InputType { get; set; }
        public Object Value
        {
            get
            {
                return this.textBox1.Text;
            }
        }
        public String Title
        {
            get
            {
                return this.label1.Text;
            }
            set
            {
                this.label1.Text = value;
            }
        }
        public String Key { get; set; }
        private void InputField_Load(object sender, EventArgs e)
        {

        }
    }
}
