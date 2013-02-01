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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SearchBox_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.X > this.Width - 24)
            if (SearchClicked != null)
                SearchClicked(sender, e);
        }
    }
}
