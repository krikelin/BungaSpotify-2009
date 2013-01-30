﻿using Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BungaSpotify09.Apps
{
    public class whatsnew : Spider.App
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
        public whatsnew(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            InitializeComponent();
        }
        private void whatsnew_Load(object sender, EventArgs e)
        {
            this.Spider.LoadFile("views\\whatsnew.xml");
            
        }
    }
}
