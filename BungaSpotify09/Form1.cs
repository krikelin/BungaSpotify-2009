using Spider;
using Spider.Skinning;
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
    public partial class Form1 : Form
    {
        private Spider.SpiderHost SpiderHost;
        private Timer tmrReload;
        public Form1()
        {
            InitializeComponent();
        }

        public Style Stylesheet = new PixelStyle();
        private void Form1_Load(object sender, EventArgs e)
        {
            SpiderHost = new Spider.SpiderHost();
            SpiderHost.RegistredAppTypes.Add("whatsnew", typeof(Apps.whatsnew));
            SpiderHost.RegistredAppTypes.Add("playqueue", typeof(Apps.playqueue));
            tmrReload = new System.Windows.Forms.Timer();

            this.Controls.Add(SpiderHost);
            SpiderHost.Dock = DockStyle.Fill;
            SPListView listView = new SPListView(this.Stylesheet);
            this.Controls.Add(listView);
            listView.AddItem("What's new", new Uri("spotify:whatsnew"));
            listView.AddItem("Play Queue", new Uri("spotify:playqueue"));
            listView.AddItem("-", new Uri("spotify:playqueue"));
            listView.AddItem("Searches", new Uri("spotify:searches"));
            listView.AddItem("-", new Uri("spotify:playqueue"));
            listView.AddItem("+ New Playlist", new Uri("spotify:searches"));
            listView.ItemSelected += listView_ItemSelected;
            listView.Dock = DockStyle.Left;
            listView.Width = 270;
            SpiderHost.Navigate("spotify:whatsnew:a");

            var panel = new Panel();
            panel.BackgroundImage = Properties.Resources.header;
            var panel2 = new Panel();
            panel2.BackgroundImage = Properties.Resources.footer;
            panel2.Dock = DockStyle.Bottom;
            panel.Dock = DockStyle.Top;
            panel2.Height = panel2.BackgroundImage.Height;
            panel.Height = panel.BackgroundImage.Height;
            this.Controls.Add(panel);

            this.Controls.Add(panel2);

            // add some playlists
            
        }

        void listView_ItemSelected(object Sender, SPListView.SPListItemEventArgs e)
        {
            Navigate(e.Item.Uri);
        }

        public void Navigate(Uri uri)
        {
            if (uri.ToString().StartsWith("spotify:"))
            {
                SpiderHost.Navigate(uri.ToString());
            }
        }

    }
}
