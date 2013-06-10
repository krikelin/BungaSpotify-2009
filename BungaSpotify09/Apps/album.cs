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
    class album : Spider.App
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // artist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "album";
            this.Template = "views\\album.xml";
            this.Load += new System.EventHandler(this.artist_Load);
            this.ResumeLayout(false);

        }
        public album(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            InitializeComponent();
            Start();
        }
        Release release;
        public override object Loading(object arguments)
        {
            IMusicService ms = this.Host.MusicService;
            String[] parameters = (String[])arguments;
            release = ms.LoadRelease(parameters[2]);
            release.LoadTracks();
            return new
            {
                Release = release
            };

        }
        public override string GetName()
        {
            if (release != null)
                return release.Name != null ? release.Name : "";
            else
                return "";
        }
        private void artist_Load(object sender, EventArgs e)
        {
            
        }
        public override string GetSubName()
        {
            if (release != null)
                return "by " + release.Artist.Name;
            else
                return "";
        }
        public override SPListItem.ListIcon GetIcon()
        {
            return new SPListItem.ListIcon()
            {
                Normal = Properties.Resources.ic_album_normal,
                Selected = Properties.Resources.ic_album_selected
            };
        }
    }
}
