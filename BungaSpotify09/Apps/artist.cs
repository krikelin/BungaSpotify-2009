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
            Form1 form1 = (Form1)Tag;
            IMusicService service = form1.MusicService;
            Artist artist = form1.MusicService.LoadArtist(parameters[2]);

            // Load all releases
            artist.LoadReleases();
            
            // Load tracklist for all releases
            foreach(Release release in artist.Albums) 
            {
                release.LoadTracks();
            }
            // Load tracklist for all releases
            foreach (Release release in artist.Singles)
            {
                release.LoadTracks();
            }
            return new
            {
                Artist = artist
            };
                
#if(False)
            Thread.Sleep(100);
            return new {
                artist = new {
                    name = "Armin Van Buuren",
                    image = "http://o.scdn.co/300/e7cd6c28d836bf81f8a0623615e759f6c3c8fbd2",
                    
                    biography = "A progressive trance DJ like Klaus",
                    uri = String.Join(":", parameters),
                    albums = new Object[] {
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
                    },
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
#endif


        }

        private void artist_Load(object sender, EventArgs e)
        {

        }
    }
}
