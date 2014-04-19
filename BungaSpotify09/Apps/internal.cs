﻿using Spider;
using Spider.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public override bool AcceptsUri(string uri)
        {
            Regex regex = new Regex(uri);
            return regex.IsMatch("spotify:internal:home");
        }
        public @internal(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            InitializeComponent();
           
        }
        public override void Navigate(string[] arguments)
        {
            this.Arguments = arguments;
            this.Template = "views\\whatsnew.xml";
            Start();
        }
        public override void LoadFinished()
        {
            switch ((string)((string[])Arguments)[0])
            {
                case "home":
                    {
                    }
                    break;
                case "toplist":
                    {
                        try
                        {
                            foreach (Track t in ((TopList)currentResource).TopTracks)
                            {
                                Spider.CListView.CListViewItem item = new Spider.CListView.CListViewItem("");
                                item.Track = t;
                                this.Spider.Sections["toplist"].ListView.Items.Add(item);
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        
                    }
                    break;
                case "playqueue":
                    {
                        if (currentResource != null)
                        {
                            this.Spider.Sections["playqueue"].ListView.Items.Clear();
                            foreach (Track t in (currentResource as PlayQueue).Queue)
                            {

                                Spider.CListView.CListViewItem listViewItem = new Spider.CListView.CListViewItem(t.Name);
                                listViewItem.Spawn = this.Spider.Sections["playqueue"].ListView;
                                t.Item = listViewItem;
                                t.Item.Track = t;
                                
                                this.Spider.Sections["playqueue"].ListView.Items.Add(listViewItem);
                            }
                            this.Spider.Sections["history"].ListView.Items.Clear();
                            foreach (Track t in (currentResource as PlayQueue).History)
                            {

                                Spider.CListView.CListViewItem listViewItem = new Spider.CListView.CListViewItem(t.Name);
                                listViewItem.Spawn = this.Spider.Sections["history"].ListView;
                                t.Item = listViewItem;
                                t.Item.Track = t;

                                this.Spider.Sections["history"].ListView.Items.Add(listViewItem);
                            }
                        }
                    }
                    break;
                case "history":
                   {
                       if (currentResource != null)
                       {
                           this.Spider.Sections["history"].ListView.Items.Clear();
                           foreach (Track t in (currentResource as TrackCollection))
                           {

                               Spider.CListView.CListViewItem listViewItem = new Spider.CListView.CListViewItem(t.Name);
                               listViewItem.Spawn = this.Spider.Sections["history"].ListView;
                               t.Item = listViewItem;
                               t.Item.Track = t;

                               this.Spider.Sections["history"].ListView.Items.Add(listViewItem);
                           }
                       }
                   }

                   break;
                case "own":
                   {
                       foreach (Track t in ((TrackCollection)currentResource).Items)
                       {
                           Spider.CListView.CListViewItem item = new Spider.CListView.CListViewItem("");
                           item.Track = t;
                           t.Item = item;
                           try
                           {
                               item.Spawn = this.Spider.Sections["own"].ListView;
                               t.LoadAsync(null);
                               this.Spider.Sections["own"].ListView.Items.Add(item);
                           }
                           catch (Exception e)
                           {
                           }
                           
                       }
                   }
                   break;
            }
        }
        Resource currentResource;
        public override object Loading(object arguments)
        {
            switch ((string)((string[])arguments)[2])
            {
                case "toplist":
                    {
                        currentResource = this.Host.MusicService.LoadTopListForResource(this.Host.MusicService.GetCurrentCountry());
                        return new {
                            TopList = currentResource
                        };

                    }
                case "playqueue":
                    {
                        currentResource = new PlayQueue(this.Host.MusicService);

                        foreach (Track t in (this.Host.PlayQueue))
                        {
                            Track track = t;
                            (currentResource as PlayQueue).Queue.Enqueue(track);
                        }
                        foreach (Track t in (this.Host.PlayHistory).Reverse())
                        {
                            Track track = t;
                            (currentResource as PlayQueue).History.Push(track);
                        }
                        return currentResource;
                      
                    
                    }
             
                case "own":
                    {
                        currentResource = this.Host.MusicService.GetCollection("own", "");
                        return new
                        {
                            Own = currentResource
                        };
                    }
                case "home":
                    currentResource = this.Host.MusicService.GetCollection("whatsnew", "");
                    return new
                    {
                        Home = currentResource
                    };
            }
            return base.Loading(arguments);
        }
        public override SPListItem.ListIcon GetIcon()
        {
            switch (Arguments[0])
            {
                case "add_playlist":
                    return new SPListItem.ListIcon()
                     {
                         Normal = Properties.Resources.ic_new_playlist,
                         Selected = Properties.Resources.ic_new_playlist
                     };
                case "home":
                    return new SPListItem.ListIcon()
                    {
                        Normal = Properties.Resources.ic_home_normal,
                        Selected = Properties.Resources.ic_home_selected
                    };
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
                case "own":
                    return new SPListItem.ListIcon()
                    {
                        Normal = Properties.Resources.ic_own_normal,
                        Selected = Properties.Resources.ic_own_selected
                    };
            }
            return null;
        }
        public override string GetName()
        {
            String argument = Arguments.Length > 1 ? Arguments[1] : Arguments[0];
            switch(argument) {
                case "add_playlist":
                    return "New Playlist";
                case "home":
                    return "Home";
                case "toplist":
                    return "Top List";
                case "playqueue":
                    return "Play Queue";
                case "own":
                    return "Own music";

            
            }
            return "Internal";
        }
        private void whatsnew_Load(object sender, EventArgs e)
        {
            
            
        }
    }
}
