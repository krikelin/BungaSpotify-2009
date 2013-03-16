using BungaSpotify.Service;
using Spider;
using Spider.Media;
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

       

        SPListView listView;
        public Style Stylesheet = new PixelStyle();
        private void Form1_Load(object sender, EventArgs e)
        {

            SpiderHost = new Spider.SpiderHost(new DummyService());
            SpiderHost.Notify += SpiderHost_Notify;
            SpiderHost.RegistredAppTypes.Add("group", typeof(Apps.group));
            SpiderHost.RegistredAppTypes.Add("internal", typeof(Apps.@internal));
            SpiderHost.RegistredAppTypes.Add("playqueue", typeof(Apps.playqueue));
            SpiderHost.RegistredAppTypes.Add("artist", typeof(Apps.artist));
            SpiderHost.RegistredAppTypes.Add("album", typeof(Apps.album));
            SpiderHost.RegistredAppTypes.Add("search", typeof(Apps.search));
            SpiderHost.RegistredAppTypes.Add("user", typeof(Apps.user));
            tmrReload = new System.Windows.Forms.Timer();
            listView = new SPListView(this.Stylesheet, SpiderHost);
            this.Controls.Add(SpiderHost);
            SpiderHost.Dock = DockStyle.Fill;
            this.Controls.Add(listView);
            listView.AddItem(new Uri("spotify:internal:whatsnew"));
            listView.AddItem(new Uri("spotify:internal:toplist"));
            listView.AddItem(new Uri("spotify:internal:playqueue"));
            listView.AddItem(new Uri("spotify:internal:own"));
            listView.AddItem("-", new Uri("spotify:internal:toplist"));
            listView.AddItem(new Uri("spotify:internal:add_playlist"));
            listView.ItemSelected += listView_ItemSelected;
            this.SpiderHost.MusicService.ObjectsDelivered += MusicService_ObjectsDelivered;
            listView.ItemReordered += listView_ItemReordered;
            listView.Dock = DockStyle.Left;
            listView.Width = 270;
            this.SpiderHost.MusicService.RequestUserObjects();
            var panel = new AppHeader((PixelStyle)this.Stylesheet);
            panel.BackgroundImage = Properties.Resources.header;
            
            var panel2 = new AppHeader((PixelStyle)this.Stylesheet);
            panel2.BackgroundImage = Properties.Resources.footer;
            panel2.Dock = DockStyle.Bottom;
            panel.Dock = DockStyle.Top;
            panel2.Height = panel2.BackgroundImage.Height;
            panel.Height = panel.BackgroundImage.Height;
            infoBar = new infobar(this.Stylesheet);
            infoBar.Hide();
            this.Controls.Add(infoBar);
            infoBar.Dock = DockStyle.Top;
            this.Controls.Add(panel);

            this.Controls.Add(panel2);
            searchBox = new SearchBox();
            panel.Controls.Add(searchBox);
            searchBox.Left = 80;
            searchBox.Top = 26;
            searchBox.SearchClicked += searchBox_SearchClicked;
           
            // add some playlists
            
        }
        infobar infoBar;
        void SpiderHost_Notify(object sender, NotificationEventArgs e)
        {
            this.infoBar.ShowMessage(e.Text, e.Type);
        }

        void listView_ItemReordered(object sender, SPListView.ItemReorderEventArgs e)
        {
        }

        void MusicService_ObjectsDelivered(object sender, UserObjectsEventArgs e)
        {
            foreach (String str in e.Objects)
            {
                this.listView.AddItem(new Uri(str));
            }
        }

        void searchBox_SearchClicked(object sender, EventArgs e)
        {
            try
            {
                if (searchBox.Text.StartsWith("spotify:"))
                {
                    Navigate(new Uri(searchBox.Text));
                }
                else
                {
                    SpiderHost.Navigate("spotify:search:" + searchBox.Text.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        SearchBox searchBox;
        void listView_ItemSelected(object Sender, SPListView.SelectedItemEventArgs e)
        {
            if (e.Item.Uri.ToString().StartsWith("spotify:internal:add_playlist"))
            {
                e.Cancel = true;
                NewPlaylist np = new NewPlaylist();
                if (np.ShowDialog() == DialogResult.OK)
                {
                    String identifier = this.SpiderHost.MusicService.NewPlaylist(np.PlaylistName);
                    this.listView.AddItem(new Uri("" + identifier));
                    this.listView.Refresh();
                }
                return;
            }
            Navigate(e.Item.Uri);
        }

        public void Navigate(Uri uri)
        {
            if (uri.ToString().StartsWith("spotify:"))
            {
                SpiderHost.Navigate(uri.ToString());
            }
            
            foreach (SPListItem item in this.listView.Items)
            {

                if (item.Uri.ToString() == uri.ToString())
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
            listView.Draw(listView.CreateGraphics());
        }

    }
}
