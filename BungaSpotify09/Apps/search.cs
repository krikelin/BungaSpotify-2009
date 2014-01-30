using Spider;
using Spider.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BungaSpotify09.Apps
{
    public class search : Spider.App
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
        public search(SpiderHost host)
            : base(host)
        {
            InitializeComponent();
        }
        public override bool AcceptsUri(string uri)
        {
            Regex regex = new Regex("spotify:search:(.*)");
            return regex.IsMatch(uri);
        }
        public override void Navigate(string[] arguments)
        {
            Template = "views\\search.xml";
            Arguments = arguments;
            Start();
        }
        public override string GetName()
        {
            return this.query;
        }
        public override SPListItem.ListIcon GetIcon()
        {
            return new SPListItem.ListIcon()
            {
                Normal = Properties.Resources.ic_search_normal,
                Selected = Properties.Resources.ic_search_selected
            };
        }
        private String query;
        public override void LoadFinished()
        {
            base.LoadFinished();
            
            var listView = this.Spider.Sections["search"].ListView;
            listView.AllowsReoreder = false;
            listView.AllowDrop = false;
            foreach (Track t in this.res.Tracks)
            {
                Spider.CListView.CListViewItem item = new CListView.CListViewItem(t.Name);
                item.Track = t;
                item.Spawn = listView;
                t.Item = item;
                listView.Items.Add(t.Item);
            }
        }
        SearchResult res;
        public override object Loading(object arguments)
        {
            this.Spider.IsPlaylist = true;
            
            String[] parameters = (String[])arguments;

            this.query = parameters[2];
            IMusicService service = this.Host.MusicService;
            res = service.Find(parameters[2], 12, 1);
            
            return new
            {
                Search = res
            };
            
        }

        private void artist_Load(object sender, EventArgs e)
        {

        }
    }
}


