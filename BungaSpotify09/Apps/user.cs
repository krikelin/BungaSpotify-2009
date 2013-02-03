using Spider;
using Spider.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BungaSpotify09.Apps
{
    class user : Spider.App
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "search";
            this.Size = new System.Drawing.Size(147, 150);
            this.Load += new System.EventHandler(this.artist_Load);
            this.ResumeLayout(false);

        }
        public Playlist Playlist;
        public user(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            
            Template = "views\\playlist.xml";
           
            InitializeComponent();
            Start();

        }
        public override void DropItem(IDataObject data)
        {
            String d = (String)data.GetData(DataFormats.StringFormat);
            Track track = new Track(this.Host.MusicService, d.Split(':')[2]);
            bool dw = this.Host.MusicService.InsertTrack(this.Playlist, track, 0);
            this.Playlist.Tracks.Insert(0, track);
           
            this.Render();

            
        }
        public override bool AllowsDrop(IDataObject data)
        {
            return true;
        }
        public override void Reorder(int oldPos, int count, int newPos)
        {
            
        }
        public void Render()
        {
            this.ListView.Items.Clear();
            foreach (Track t in Playlist.Tracks)
            {
                var listViewItem = new Spider.CListView.CListViewItem(t.Name);
                listViewItem.Spawn = this.Spider.Sections["overview"].ListView;
                t.Item = listViewItem;
                t.Item.Track = t;
                this.Spider.Sections["overview"].ListView.AddItem(listViewItem);
            }
            te = new System.Windows.Forms.Timer();
        }
        public override void LoadFinished()
        {
            base.LoadFinished();
            this.ListView.Reordered += ListView_Reordered;
            this.ListView.ItemsDeleting += ListView_ItemsDeleting;
            Render();  
        }

        void ListView_ItemsDeleting(object sender, CListView.ReorderEventArgs e)
        {
            this.Playlist.Delete(e.Indexes);
        }
        System.Windows.Forms.Timer te;
        public CListView ListView
        {
            get
            {
                return this.Spider.Sections["overview"].ListView;
            }
        }
        void te_Tick(object sender, EventArgs e)
        {
            
            Playlist.Tracks.RemoveAt(1);
            this.Spider.Sections["overview"].ListView.Items.RemoveAt(1);
            this.spiderView.refresh(new
            {
                Playlist = Playlist
            });
           
            te.Stop();
        }

        void ListView_Reordered(object sender, CListView.ReorderEventArgs e)
        {
            
            
               
            this.Playlist.Reorder(e.Position, e.Indexes, e.NewPosition);
        }
        public override object Loading(object arguments)
        {
            String[] parameters = (String[])arguments;
            Playlist playlist = this.Host.MusicService.LoadPlaylist(parameters[2], parameters[4]);
            this.Playlist = playlist;
            playlist.Tracks = this.Host.MusicService.GetPlaylistTracks(playlist, 0);
            return new
            
            {
                Playlist = playlist
            };
            

        }
        public override string GetName()
        {
            return this.Playlist.Name;
        }
        public override Spider.SPListItem.ListIcon GetIcon()
        {
            return new SPListItem.ListIcon()
            {
                Normal = Properties.Resources.ic_playlist_normal,
                Selected = Properties.Resources.ic_playlist_selected
            };
        }
        private void artist_Load(object sender, EventArgs e)
        {

        }
    }
}
