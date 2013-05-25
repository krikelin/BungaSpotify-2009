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
   
    public partial class InputForm : Form
    {
        
        public InputForm()
        {
            InitializeComponent();
        }
        public InputForm(List<InputField> inputFields)
        {
            InitializeComponent();
            this.InputFields = inputFields;
            foreach (InputField ifield in this.InputFields)
            {
                ifield.Dock = DockStyle.Top;
                ifield.Height = 128;
                this.Controls.Add(ifield);
            }
        }
        public Dictionary<String, Object> Data {
            get {
                Dictionary<String, Object> fields = new Dictionary<string, object>();
                foreach(InputField inputField in InputFields) {
                    
                    fields.Add(inputField.Key, inputField.Value);
                }
                return fields;
            }
        }
        public List<InputField> InputFields { get; set; }
        private void InputForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
        }
    }
    
}
