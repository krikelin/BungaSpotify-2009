
using Spider;
using Spider.Media;
using Spider.Skinning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BungaSpotify09
{
    public partial class Form1 : Form
    {
        #region Appearance
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        // Define the CS_DROPSHADOW constant
        private const int CS_DROPSHADOW = 0x00020000;

        // Override the CreateParams property
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            System.Drawing.Rectangle rect = Screen.GetWorkingArea(this);
            this.MaximizedBounds = Screen.GetWorkingArea(this);
        }
        #endregion
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
            SpiderHost.RegistredAppTypes.Add("app", typeof(Apps.app));
            SpiderHost.RegistredAppTypes.Add("artist", typeof(Apps.artist));
            SpiderHost.RegistredAppTypes.Add("album", typeof(Apps.album));
            SpiderHost.RegistredAppTypes.Add("search", typeof(Apps.search));
            SpiderHost.RegistredAppTypes.Add("user", typeof(Apps.user));
            tmrReload = new System.Windows.Forms.Timer();
            listView = new SPListView(this.Stylesheet, SpiderHost);
            this.Controls.Add(SpiderHost);
            SpiderHost.Dock = DockStyle.Fill;
            this.Controls.Add(listView);

            listView.ItemInserted += listView_ItemInserted;
            listView.AddItem(new Uri("spotify:internal:home"), true);
            listView.AddItem("-", new Uri("spotify:internal:placeholder"), true);
            listView.ItemSelected += listView_ItemSelected;
            // this.SpiderHost.MusicService.ObjectsDelivered += MusicService_ObjectsDelivered;
            listView.ItemReordered += listView_ItemReordered;
            listView.Dock = DockStyle.Left;
            listView.Width = 270;
            this.SpiderHost.MusicService.RequestUserObjects();
            this.SpiderHost.MusicService.PlaybackStarted += MusicService_PlaybackStarted;
            var panel = new AppHeader((PixelStyle)this.Stylesheet);
            panel.BackgroundImage = Properties.Resources.header;
            
            var panel2 = new AppHeader((PixelStyle)this.Stylesheet);
            panel2.BackgroundImage = Properties.Resources.footer;
            panel2.Dock = DockStyle.Bottom;
            panel.Dock = DockStyle.Top;
            panel2.Height = panel2.BackgroundImage.Height;
            panel2.MouseDown += panel2_MouseDown;
            panel.Height = panel.BackgroundImage.Height;
            infoBar = new infobar(this.Stylesheet);
            infoBar.Hide();
            this.Controls.Add(infoBar);
            infoBar.Dock = DockStyle.Top;
            this.Controls.Add(panel);
            panel.MouseMove += panel_MouseMove;

            this.Controls.Add(panel2);
            searchBox = new SearchBox();
            panel.Controls.Add(searchBox);
            searchBox.Left = 80;
            searchBox.Top = 26;
            searchBox.SearchClicked += searchBox_SearchClicked;
           
            // add some playlists
            
        }

        void MusicService_PlaybackStarted(object sender, EventArgs e)
        {
            (this.SpiderHost.Apps["spotify:internal:playqueue"] as Apps.@internal).Start();
        }

        void listView_ItemInserted(object sender, SPListView.ItemInsertEventArgs e)
        {
            this.SpiderHost.MusicService.insertUserObject(e.Uri.ToString(), e.Position);
        }

        void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 6);
            }
        }

        void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
        infobar infoBar;
        void SpiderHost_Notify(object sender, NotificationEventArgs e)
        {
        //    this.infoBar.ShowMessage(e.Text, e.Type);
        }

        void listView_ItemReordered(object sender, SPListView.ItemReorderEventArgs e)
        {
        }

        void MusicService_ObjectsDelivered(object sender, UserObjectsEventArgs e)
        {
            foreach (String str in e.Objects)
            {
                this.listView.AddItem(new Uri(str), true);
            }
        }

        void searchBox_SearchClicked(object sender, EventArgs e)
        {
            if (!SpiderHost.Navigate(searchBox.ToString()))
            {
                infoBar.ShowMessage("Link could not be found", NotificationType.Warning);
            }
           /* try
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
            }*/
            SpiderHost.Navigate(searchBox.Text);
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
                    this.listView.AddItem(new Uri("" + identifier), false);
                    this.listView.Refresh();
                }
                return;
            }
            if (!SpiderHost.Navigate(e.Item.Uri.ToString()))
            {
                infoBar.ShowMessage("Link could not be found", NotificationType.Warning);
            }
        }

        public void Navigate(Uri uri)
        {

            if (!SpiderHost.Navigate(uri.ToString()))
            {
                infoBar.ShowMessage("Link could not be found", NotificationType.Warning);
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
