using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BungaSpotify09
{
    public partial class NewPlaylist : Form
    {
        /// <summary>
        /// The Playlist name
        /// </summary>
        public String PlaylistName
        {
            get
            {
                return this.textBox1.Text;
            }
        }
        public NewPlaylist()
        {
            InitializeComponent();
        }

        private void NewPlaylist_Load(object sender, EventArgs e)
        {

        }
    }
}
