using Spider.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Net;
namespace BungaSpotify.Service
{
    public class SpotifyService : IMusicService
    {
        public static String CONNECTION_PATH = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=myspotify.mdb;Jet OLEDB:Database Password=;";
        public OleDbConnection MakeConnection()
        {
            return new OleDbConnection(CONNECTION_PATH);
        }
        public SpotifyService()
        {

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
        }
        private Track nowPlayingTrack;
        public Track NowPlayingTrack
        {
            get
            {
                return nowPlayingTrack;
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (NowPlayingTrack == null)
                return;

            position++;
            if (position >= nowPlayingTrack.Duration)
            {
                if (PlaybackFinished != null)
                {
                    timer.Stop();
                    position = 0; // Reset the position
                    PlaybackFinished(this, new EventArgs());
                    //     nowPlayingTrack = null;
                }

            }
        }
        public event PlayStateChangedEventHandler PlaybackFinished;
        public string Namespace
        {
            get { return "spotify"; }
        }
        /// <summary>
        /// Simulate the track playback by a timer
        /// </summary>
        System.Windows.Forms.Timer timer;
        public string Name
        {
            get { return "Spotify"; }
        }
        int position = 0;
        private Track CurrentTrack;
        public void Play(Track track)
        {
            this.nowPlayingTrack = track;

            timer.Start();

        }

        public void Stop()
        {
            if (nowPlayingTrack == null)
                return;
            timer.Stop(); // Stop "playback"
            position = 0; // Reset position
            nowPlayingTrack = null;
        }

        public void Pause()
        {
            if (nowPlayingTrack == null)
                return;
            timer.Stop();
        }

        public void Seek(int pos)
        {
            if (CurrentTrack == null)
                return;
            position = pos;
            if (position >= CurrentTrack.Duration)
            {
                if (PlaybackFinished != null)
                {
                    timer.Stop();
                    position = 0; // Reset the position
                    PlaybackFinished(this, new EventArgs());
                    nowPlayingTrack = null;
                }

            }
        }
        public Dictionary<String, Artist> ArtistCache = new Dictionary<string, Artist>();
        public Artist LoadArtist(string identifier)
        {
            WebClient wc = new WebClient();
            String data = wc.DownloadString("http://ws.spotify.com/lookup/1/.json?uri=spotify:artist:" + identifier);
            JObject jdata = JObject.Parse(data);
            return ArtistFromJObject((JObject)jdata["artist"]);
        }
        public Artist ArtistFromJObject(JObject jobject)
        {
            Artist artist = new Artist(this);
            artist.Name = (String)jobject["name"];
            try
            {
                artist.Identifier = ((String)jobject["href"]).Replace("spotify:artist:", "");
            }
            catch (Exception e)
            {
                artist.Identifier = ((String)jobject["artist-id"]).Replace("spotify:artist:", "");

            }
            artist.Status = Resource.State.Available;
            return artist;

        }
        public Release ReleaseFromJObject(JObject jobject)
        {
            Release r = new Release(this);
            r.Name = (String)jobject["name"];
            try
            {
                r.Identifier = ((string)jobject["href"]).Replace("spotify:album:", "");
            }
            catch (Exception e)
            {
                r.Identifier = ((string)jobject["album-id"]).Replace("spotify:album:", "");
            }
            r.Status = ((string)jobject["avaible"] == "true") ? Resource.State.Available : Resource.State.NotAvailable;
            
            try
            {
                r.Artist = LoadArtist(((String)jobject["artist-id"]).Replace("spotify:artist:", ""));
            }
            catch (Exception e)
            {
            }
            r.ReleaseDate = new DateTime(int.Parse((String)jobject["released"]), 1, 1);
           
            return r;
        }
        public Track TrackFromJObject(JObject jobject)
        {
            Track t = new Track(this);
            t.Name = (string)jobject["name"];
            try
            {
                t.Identifier = ((string)jobject["href"]).Replace("spotify:track:", "");
            }
            catch (Exception e)
            {
                t.Identifier = ((string)jobject["track-id"]).Replace("spotify:track:", "");
            }
            t.Artists = new Artist[] { };
            JToken album;
            if (jobject.TryGetValue("album", out album))
            {
                t.Album = ReleaseFromJObject((JObject)album);
            }
            return t;
        }

        public Playlist PlaylistFromRow(DataRow row)
        {
            Playlist playlist = new Playlist(this);
            playlist.Name = (String)row["title"];
            playlist.Description = (String)row["playlist.description"];
            playlist.Image = (String)row["playlist.image"];
            playlist.Status = Resource.State.Available;
            playlist.User = new User(this)
            {
                Name = (String)row["users.identifier"],
                Identifier = (String)row["users.identifier"]
            };
            playlist.Identifier =(String)row["playlist.identifier"];
            return playlist;
        }
        public Playlist LoadPlaylist(string username, string identifier)
        {
            DataSet dr = MakeDataSet("SELECT * FROM users, playlist WHERE playlist.user = users.id AND users.identifier = '" + username + "' AND playlist.identifier = '" + identifier + "'");
            Playlist pl = PlaylistFromRow(dr.Tables[0].Rows[0]);
            return pl;
        }
        public Dictionary<String, Track> Cache = new Dictionary<string, Track>();
        public SearchResult Find(string query, int maxResults, int page)
        {
            // Find songs
            WebClient wc = new WebClient();
            String strTracksResult = wc.DownloadString("http://ws.spotify.com/search/1/track.json?q=" + query);
            JObject jTracksResult = JObject.Parse(strTracksResult);
            SearchResult sr = new SearchResult(this);
            sr.Tracks = new TrackCollection(this, sr, new List<Track>());
            foreach (JObject dr in (JArray)jTracksResult["tracks"])
            {
                
                sr.Tracks.Add(TrackFromJObject(dr));
            }

            // Find artists
            String strArtistsResult = wc.DownloadString("http://ws.spotify.com/search/1/artist.json?q=" + query);
            JObject jArtistsResult = JObject.Parse(strArtistsResult);
            ArtistCollection ac = new ArtistCollection(this, new List<Artist>());
            foreach (JObject dr in (JArray)jArtistsResult["artists"])
            {
                ac.Add(ArtistFromJObject(dr));
            }
            sr.Artists = ac;

            // Find albums
            String strAlbumsResult = wc.DownloadString("http://ws.spotify.com/search/1/album.json?q=" + query);
            JObject jAlbumsResult = JObject.Parse(strAlbumsResult);
            ReleaseCollection rc = new ReleaseCollection(this, new List<Release>());
            foreach (JObject dr in (JArray)jAlbumsResult["albums"])
            {
                rc.Add(ReleaseFromJObject(dr));
            }
            sr.Albums = rc;
            return sr;
        }


        public ReleaseCollection LoadReleasesForGivenArtist(Artist artist, ReleaseType type, int page)
        {
            WebClient wc = new WebClient();
            String d = wc.DownloadString("http://ws.spotify.com/lookup/1/.json?uri=spotify:artist:" + artist.Identifier + "&extras=albumdetail");
            JObject art = JObject.Parse(d);
            ReleaseCollection rc = new ReleaseCollection(this, new List<Release>());
            foreach (JObject releaseRow in (JArray)art["artist"]["albums"])
            {
                Release r = ReleaseFromJObject((JObject)releaseRow["album"]);
                if(type == ReleaseType.Album)
                rc.Add(r);
                
            }
            rc.Items.Sort(delegate(Release r1, Release r2)
            {
                return r2.Year - r1.Year;
            });
            return rc;
        }
        public List<String> BadKarmaReleases = new List<string>() {
            "spotify:album:6tyAtZM8hahW48Nc4NFSYZ",
            "spotify:album:1fD1PBEVHg5dxwzkKM0qrp",
            "spotify:album:2jqEl3r8MUAg9GKBfDHkBO",
            "spotify:album:2mR8L2Y3JGI4Q2GAOu09Az",
            "spotify:album:7sRQhF3XtF3ihzjNEZgw9s",
            "spotify:album:3rjCKY4dlUrb8ERvEXhqYk",
            "spotify:album:6SgUhH8v3UtoTG33JyAFCF",
            "spotify:album:6VuU7RpBYQdjhWo1ympHdu",
            "spotify:album:2NMe4mNXstHtfAd4KfwMaU",
            "spotify:album:0Pfn6g44Ni7Prg5TBWsuFW",
            "spotify:album:39vXIDw9SlwYtwLFlaS1hc",
            "spotify:album:14Kuc9J9G5Hr8rq5K0cguy",
            "spotify:album:73acbjSikjUA6JkpokTS2j"
       
        };
        public TrackCollection LoadTracksForPlaylist(Playlist playlist)
        {
            Thread.Sleep(1000);
            TrackCollection tc = new TrackCollection(this, playlist, new List<Track>());
            for (var i = 0; i < 3; i++)
            {
                Track track = new Track(this)
                {
                    Identifier = "5124525ffs12",
                    Name = "Test",
                    Artists = new Artist[] { new Artist(this) { Name = "TestArtist", Identifier = "2FOU" } }
                };
                tc.Add(track);
            }
            return tc;
        }

        public Release LoadRelease(string identifier)
        {
            WebClient wc = new WebClient();
            String d = wc.DownloadString("http://ws.spotify.com/lookup/1/.json?uri=spotify:album:" +identifier + "");
            JObject art = JObject.Parse(d);
            
            return ReleaseFromJObject((JObject)art["album"]);
        }


        public DataSet MakeDataSet(String sql)
        {
            OleDbConnection conn = MakeConnection();
            conn.Open();
            OleDbDataAdapter oda = new OleDbDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            conn.Close();
            return ds;
        }
        public Dictionary<String, Track> TrackCache = new Dictionary<String, Track>();
        public TrackCollection LoadTracksForGivenRelease(Release release)
        {
            bool badKarma = this.BadKarmaReleases.Contains("spotify:album:" + release.Identifier);
            WebClient wc = new WebClient();
            String d = wc.DownloadString("http://ws.spotify.com/lookup/1/.json?uri=spotify:album:" + release.Identifier + "&extras=trackdetail");

            JObject jobject = JObject.Parse(d);
            TrackCollection tc = new TrackCollection(this, release, new List<Track>());
            foreach (JObject row in (JArray)jobject["album"]["tracks"])
            {
                Track t = TrackFromJObject(row);
                if(badKarma) 
                    t.Status = Resource.State.BadKarma;

                if (!Cache.ContainsKey(t.Identifier))
                {
                    Cache.Add(t.Identifier, t);
                }
                tc.Add(t);
            }

            return tc;

        }


        public Track LoadTrack(string identifier)
        {
            if (Cache.ContainsKey(identifier))
            {
                return Cache[identifier];
            }
            WebClient wc = new WebClient();
            String d = wc.DownloadString("http://ws.spotify.com/lookup/1/.json?uri=spotify:track:" + identifier + "");
            JObject jtrack = JObject.Parse(d);
            var t = TrackFromJObject((JObject)jtrack["track"]);


            try
            {
                Cache.Add(identifier, t);
            }
            catch (Exception e)
            {
                return Cache[identifier];
            }
            return t;
        }


        public TopList LoadTopListForResource(Resource res)
        {
            throw new NotImplementedException();
        }

        public User LoadUser(string identifier)
        {
            return new User(this)
            {
                Name = identifier,
                CanoncialName = identifier
            };
        }


        public SessionState SessionState
        {
            get { return SessionState.LoggedIn; }
        }

        public LogInResult LogIn(string userName, string passWord)
        {
            return LogInResult.Success;
        }
        public Country GetCurrentCountry()
        {
            return new Country(this, "Sweden");
        }
        public User GetCurrentUser()
        {
            return new User(this)
            {
                Name = "Test"
            };
        }


        public bool InsertTrack(Playlist playlist, Track track, int pos)
        {
            return true;   
        }

        public bool ReorderTracks(Playlist playlist, int startPos, int count, int newPos)
        {
            return true;
        }

        public bool DeleteTrack(Playlist playlist, Track track)
        {
            return true;
        }
        public PlaylistTrack MakeUserTrackFromString(String row)
        {
            String[] parts = row.Split(':');
            PlaylistTrack track = new PlaylistTrack(this, parts[2], parts[4]);
            return track;
        }
        public TrackCollection GetPlaylistTracks(Playlist playlist, int revision)
        {
            DataSet ds = MakeDataSet("SELECT data FROM [playlist], [users] WHERE [users].[id] = [playlist].[user] AND [playlist].[identifier] = '" + playlist.Identifier + "' AND [users].[identifier] = '" + playlist.User.Identifier + "'");
            String d = (String)ds.Tables[0].Rows[0]["data"];
            String[] tracks = d.Split('&');
            TrackCollection tc = new TrackCollection(this, playlist, new List<Track>());
            foreach (String strtrack in tracks)
            {
                PlaylistTrack pt = MakeUserTrackFromString(strtrack);
                tc.Add((Track)pt);
            }
            return tc;
        }
    }
}
