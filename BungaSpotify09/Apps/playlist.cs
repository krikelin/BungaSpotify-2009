using Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BungaSpotify09.Apps
{
    class playlist : Spider.App
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
        public playlist(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            Template = "views\\playlist.xml";
            InitializeComponent();
            Start();
        }
        public override object Loading(object arguments)
        {
            String[] parameters = (String[])arguments;
            Thread.Sleep(100);
            return new {
                playlist = new {
                    permissions= new {
                        reorder = "true",
                        add = "true",
                        remove = "true"
                    },
                    uri=String.Join(":", arguments),
                    name="My playlist",
                    user = new {
                        uri = "spotify:user:drsounds",
                        name = "dr. sounds"
                    },
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
                            album = new Object[] {
                                new {
                                    name = "In and out of Love",
                                    uri = "spotify:album:7znjtUTvvmRO54knNZEUx5"
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
