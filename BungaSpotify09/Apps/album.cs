using Spider;
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
        public override object Loading(object arguments)
        {
            IMusicService ms = this.Host.MusicService;
            String[] parameters = (String[])arguments;
            Release release = ms.LoadRelease(parameters[2]);
            release.LoadTracks();
            return new
            {
                Release = release
            };

        }
        private void artist_Load(object sender, EventArgs e)
        {
            
        }
    }
}
