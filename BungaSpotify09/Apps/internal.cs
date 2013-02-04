using Spider;
using Spider.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BungaSpotify09.Apps
{
    class @internal : Spider.App
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // whatsnew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "whatsnew";
            this.Load += new System.EventHandler(this.whatsnew_Load);
            this.ResumeLayout(false);
            
        }
        public @internal(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            InitializeComponent();
            this.Arguments = arguments;
            Template = "views\\" + arguments[2] + ".xml";
            Start();
        }
        public override void LoadFinished()
        {
            switch ((string)((string[])Arguments)[2])
            {
                case "whatsnew":
                    {
                    }
                    break;
                case "toplist":
                    {
                        /*foreach(Track t in ((TopList)currentResource).TopTracks) {
                            Spider.CListView.CListViewItem item = new Spider.CListView.CListViewItem("");
                            item.Track = t;
                            this.Spider.Sections["toplist"].ListView.Items.Add(item);
                        }*/
                        
                    }
                    break;
                case "Play Queue":
                    {
                        
                    }
                   break;
            }
        }
        Resource currentResource;
        public override object Loading(object arguments)
        {
            switch ((string)((string[])arguments)[2])
            {
                case "whatsnew":
                    return base.Loading(arguments);
                case "toplist":
                    {
                        currentResource = this.Host.MusicService.LoadTopListForResource(this.Host.MusicService.GetCurrentCountry());
                        return new {
                            TopList = currentResource
                        };

                    }
                case "Play Queue":
                    {
                        break; 
                    }
            }
            return base.Loading(arguments);
        }
        public override SPListItem.ListIcon GetIcon()
        {
            switch (Arguments[2])
            {
                case "add_playlist":
                    return new SPListItem.ListIcon()
                     {
                         Normal = Properties.Resources.ic_new_playlist,
                         Selected = Properties.Resources.ic_new_playlist
                     };
                case "whatsnew":
                    return new SPListItem.ListIcon()
                    {
                        Normal = Properties.Resources.ic_home_normal,
                        Selected = Properties.Resources.ic_home_selected
                    };
                    break;
                case "playqueue":
                    return new SPListItem.ListIcon()
                    {
                        Normal = Properties.Resources.ic_playqueue_normal,
                        Selected = Properties.Resources.ic_playqueue_selected
                    };
                case "toplist":
                    return new SPListItem.ListIcon()
                    {
                        Normal = Properties.Resources.ic_toplist_normal,
                        Selected = Properties.Resources.ic_toplist_selected
                    };
                    break;
            }
            return null;
        }
        public override string GetName()
        {
            switch(Arguments[2]) {
                case "add_playlist":
                    return "New Playlist";
                case "whatsnew":
                    return "What's New";
                case "toplist":
                    return "Top List";
                case "playqueue":
                    return "Play Queue";

            
            }
            return "Internal";
        }
        private void whatsnew_Load(object sender, EventArgs e)
        {
            
            
        }
    }
}
