using Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BungaSpotify09.Apps
{
    class artist : Spider.App
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
        public artist(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            Template = "views\\artist.xml";
            InitializeComponent();
            Start();
        }
        public override object Loading(object arguments)
        {
            String[] parameters = (String[])arguments;
            Thread.Sleep(100);
            return new {
                artist = new {
                    name = "Halex",
                    uri = String.Join(":", parameters),
                    albums = new Object[] {},
                    singles = new Object[] {
                        new {
                            title = "In and Out of Love",
                            uri = "spotify:album:7znjtUTvvmRO54knNZEUx5",
                            image = "http://o.scdn.co/300/3d05a8c867cf10deaa92d5dd13359c575d5e744d",
                            tracks = new Object[] {
                                new {
                                    no = 1,
                                    name = "In and Out of Love",
                                    uri = "spotify:track:1Hm1e7VMs1CmqzhjOLEci1",
                                    artists = new Object[] {
                                        new {
                                            name = "Armin Van Buuren",
                                            uri = "spotify:artist:0SfsnGyD8FpIN4U4WCkBZ5"
                                        }
                                    },
                                    album = new Object[]  {
                                        new {
                                            name = "In and out of Love",
                                            uri = "spotify:album:7znjtUTvvmRO54knNZEUx5"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

        }

        private void artist_Load(object sender, EventArgs e)
        {

        }
    }
}
