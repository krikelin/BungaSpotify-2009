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
    class playlist : Spider.App
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
        public playlist(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            Template = "views\\playlist.xml";
           
            InitializeComponent();
            Start();
        }
        public override void Reorder(int oldPos, int count, int newPos)
        {
            
        }
        public override void LoadFinished()
        {
            base.LoadFinished();
            foreach (Track t in Playlist.Tracks)
            {
                var listViewItem = new Spider.CListView.CListViewItem(t.Name);
                listViewItem.Spawn = this.Spider.Sections["overview"].ListView;
                t.Item = listViewItem;
                t.Item.Track = t;
                this.Spider.Sections["overview"].ListView.AddItem(listViewItem);
            }

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
