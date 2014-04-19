﻿using Spider;
using Spider.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BungaSpotify09.Apps
{
    public class page : Spider.App
    {
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "page";
            this.Size = new System.Drawing.Size(147, 150);
            this.Load += new System.EventHandler(this.page_Load);
            this.ResumeLayout(false);

        }
        public page(SpiderHost host, String[] uri)
            : base(host, uri)
        {
            InitializeComponent();
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

        private void page_Load(object sender, EventArgs e)
        {

        }
    }
}


