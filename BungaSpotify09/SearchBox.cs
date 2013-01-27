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
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
        }
        public event EventHandler SearchClicked;
        private void SearchBox_Click(object sender, EventArgs e)
        {
            if (SearchClicked != null)
                SearchClicked(sender, e);
        }
        public String Text
        {
            get
            {
                return this.textBox1.Text;
            }
            set
            {
                this.textBox1.Text = value;
            }
        }
        private void SearchBox_Load(object sender, EventArgs e)
        {

        }
    }
}
