using Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this.Name = "artist";
            this.Load += new System.EventHandler(this.artist_Load);
            this.ResumeLayout(false);

        }
        public album(SpiderHost host) : base(host) {
            InitializeComponent();
        }
        private void artist_Load(object sender, EventArgs e)
        {
            this.Spider.LoadFile("views\\album.xml");
        }
    }
}
