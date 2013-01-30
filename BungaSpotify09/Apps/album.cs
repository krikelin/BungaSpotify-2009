using Spider;
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
            Thread.Sleep(1000);
            var album = new {
                uri = String.Join(":", (String[])arguments),
                artist = new {
                    name = "Armin Van Buuren",
                    uri = "spotify:artist:0SfsnGyD8FpIN4U4WCkBZ5"
                },
                name = "In and Out of Love",
                tracks = new Object[] {
                    new {
                        uri = "spotify:track:1Hm1e7VMs1CmqzhjOLEci1",
                        name = "In and Out of Love - Radio Edit",
                        artists = new Object[] {
                            new {
                                name = "Armin Van Buuren",
                                uri = "spotify:artist:0SfsnGyD8FpIN4U4WCkBZ5"
                            }
                        }
                    }
                }
            };

            return new { album = album };
        }
        private void artist_Load(object sender, EventArgs e)
        {
            
        }
    }
}
