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
using libspotifydotnet;
using System.Net;
using System.Runtime.InteropServices;
namespace BungaSpotify.Service
{
    public class SpotifyService : IMusicService
    {

        #region Spotify Callbacks
        /// <summary>
        /// Occurs at end of track
        /// </summary>
        public event sp_end_of_track TrackEnded;
        public event sp_metadata_updated MetadataUpdated;
        public event sp_connection_error ConnectionError;
        public event sp_message_to_user MessageToUser;
        public event sp_notify_main_thread MainThreadNotified;
        public event sp_music_delivery MusicDelivery;
        public event sp_play_token_lost PlayTokenLost;
        public event sp_log_message LogMessage;
        public event sp_end_of_track EndOfTrack;
        public event sp_streaming_error StreamingError;
        public event sp_userinfo_updated UserInfoUpdated;
        public event sp_start_playback StartPlayback;
        public event sp_end_of_track EndOftrack;
        public event sp_get_audio_buffer_stats AudioBufferStatsRecieved;
        public event sp_offline_status_updated OfflineStatusUpdated;
        public event sp_offline_error OfflineError;
        public event sp_credentials_blob_upated CredentialsBlobUpdated;
        public event sp_connectionstate_updated ConnectionStateUpdated;
        public event sp_scrobble_error ScrobbleError;
        public event sp_private_session_mode_changed PrivateSessionModeChanged;

        public delegate void sp_logged_in(IntPtr sp_sesion);
        public delegate void sp_metadata_updated(IntPtr sp_session);
        public delegate void sp_connection_error(IntPtr sp_session, libspotify.sp_error error);
        public delegate void sp_message_to_user(IntPtr sp_session, IntPtr message);
        public delegate void sp_notify_main_thread(IntPtr sp_session);
        public delegate void sp_music_delivery(IntPtr sp_session, IntPtr sp_audioformat, IntPtr frames, int num_frames);
        public delegate void sp_play_token_lost(IntPtr session);
        public delegate void sp_log_message(IntPtr session, IntPtr data);
        public delegate void sp_end_of_track(IntPtr sp_session);
        public delegate void sp_streaming_error(IntPtr sp_session, libspotifydotnet.libspotify.sp_error error);
        public delegate void sp_userinfo_updated(IntPtr sp_session);
        public delegate void sp_start_playback(IntPtr sp_session);
        public delegate void sp_stop_playback(IntPtr sp_session);
        public delegate void sp_get_audio_buffer_stats(IntPtr sp_session, libspotifydotnet.libspotify.sp_audio_buffer_stats stats);
        public delegate void sp_offline_status_updated(IntPtr sp_session);
        public delegate void sp_offline_error(IntPtr sp_session, libspotifydotnet.libspotify.sp_error error);
        public delegate void sp_credentials_blob_upated(IntPtr sp_session, IntPtr blob);
        public delegate void sp_connectionstate_updated(IntPtr sp_session);
        public delegate void sp_scrobble_error(IntPtr sp_session, libspotifydotnet.libspotify.sp_error error);
        public delegate void sp_private_session_mode_changed(IntPtr sp_session, bool isPrivate);
        #endregion
        public byte[] g_appkey =  new byte[] {
	        0x01, 0x0B, 0x8F, 0x5E, 0x69, 0xE3, 0x6C, 0x3E, 0x08, 0x04, 0x49, 0xCD, 0xF5, 0x65, 0x41, 0x24,
	        0x53, 0xD1, 0x46, 0x93, 0x20, 0x82, 0xA0, 0x37, 0x01, 0x6D, 0x17, 0x99, 0x4D, 0x37, 0x31, 0x6F,
	        0xBB, 0x85, 0x85, 0xFE, 0xE6, 0x7F, 0x4F, 0x88, 0x59, 0x27, 0xE3, 0x64, 0x2A, 0x0C, 0x28, 0xF6,
	        0x95, 0xE7, 0xBC, 0x68, 0xEB, 0x14, 0x2C, 0x5F, 0xF7, 0x32, 0x7F, 0x3B, 0x8A, 0x17, 0xBF, 0xC8,
	        0x84, 0xC4, 0xE7, 0x2B, 0x4B, 0x96, 0xB8, 0x75, 0xF0, 0x60, 0xA8, 0xC3, 0xED, 0x6D, 0x47, 0x82,
	        0x87, 0xC3, 0x51, 0xD4, 0x76, 0xE5, 0xAA, 0xFA, 0x15, 0xED, 0xFB, 0x04, 0x12, 0x7F, 0x40, 0x35,
	        0x10, 0x85, 0x70, 0xA5, 0xF8, 0x62, 0xC3, 0x95, 0x6B, 0x30, 0x5B, 0x3C, 0x0E, 0x27, 0x01, 0x75,
	        0x25, 0xE3, 0xCE, 0xC8, 0xA1, 0x27, 0x0A, 0x46, 0x9B, 0x00, 0x9D, 0xFC, 0xEF, 0x23, 0x56, 0x3C,
	        0x8D, 0x10, 0x39, 0x20, 0x00, 0xBE, 0xAF, 0x25, 0x8F, 0x4F, 0x25, 0xB9, 0x42, 0xD0, 0xD6, 0xFD,
	        0x3A, 0x73, 0x77, 0x22, 0xB7, 0x07, 0x22, 0x6E, 0xE1, 0x84, 0x85, 0x1F, 0xED, 0x04, 0x71, 0x4F,
	        0xF0, 0x6C, 0x32, 0x0E, 0xED, 0x7A, 0xEB, 0x8F, 0x7E, 0x61, 0x22, 0x75, 0xCC, 0xEB, 0xB1, 0xEF,
	        0x6A, 0x22, 0x29, 0x95, 0x68, 0x16, 0x57, 0xE2, 0x9F, 0x51, 0xC3, 0xCE, 0x26, 0xA6, 0x1B, 0xC8,
	        0xFC, 0x7F, 0xF1, 0xDC, 0xF8, 0x65, 0x2B, 0x21, 0x8C, 0xFA, 0x51, 0xD1, 0x80, 0xFF, 0xD9, 0x48,
	        0x26, 0x35, 0x12, 0xDA, 0x0B, 0x07, 0xD6, 0x1B, 0x15, 0xE9, 0x38, 0x05, 0x16, 0xAF, 0x57, 0x2D,
	        0xE0, 0x27, 0x8F, 0xFD, 0x20, 0x33, 0x10, 0x36, 0x06, 0x2D, 0x5A, 0x20, 0xD7, 0xC2, 0x2A, 0x8C,
	        0x94, 0x29, 0xD2, 0xF2, 0xEC, 0x96, 0x7E, 0x95, 0xE5, 0xB8, 0xDA, 0x59, 0x4C, 0x9C, 0xE7, 0xE1,
	        0xA2, 0x23, 0x9F, 0x07, 0x51, 0x3A, 0x0E, 0xC4, 0xEF, 0x5F, 0x0F, 0x27, 0xEC, 0xC9, 0xB9, 0x4C,
	        0x75, 0xF8, 0xA8, 0x84, 0x0C, 0x88, 0x93, 0x26, 0xD5, 0xF3, 0xC6, 0xA7, 0xC5, 0xCE, 0x2D, 0x67,
	        0xDA, 0x09, 0xE4, 0xD8, 0xC9, 0x26, 0x10, 0xBE, 0x6F, 0x18, 0x6E, 0x00, 0xD2, 0x8A, 0x32, 0x81,
	        0xB3, 0x8B, 0x3A, 0x96, 0x65, 0xD1, 0xEF, 0x60, 0xD5, 0x87, 0x01, 0x96, 0x55, 0xAA, 0x60, 0x30,
	        0xCD,
        };
        #region Members
        libspotify.sp_session_callbacks session_callbacks;
        libspotify.sp_session_config session_config;
        
        IntPtr session_config_ptr;
        IntPtr session_ptr;
        #endregion

        Spotify.Session session;
        public static String CONNECTION_PATH = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=myspotify.mdb;Jet OLEDB:Database Password=;";
        public OleDbConnection MakeConnection()
        {
            return new OleDbConnection(CONNECTION_PATH);
        }
        public SpotifyService()
        {
            session = Spotify.Session.CreateInstance(g_appkey, "Data", "Settings", "LinSpot");
            this.LogIn("drsounds", "carin123");
#if(False)
            session_callbacks = new libspotify.sp_session_callbacks();
            session_config = new libspotify.sp_session_config();
            this.ConnectionError += SpotifyService_ConnectionError;
            this.ConnectionStateUpdated += SpotifyService_ConnectionStateUpdated;
            this.CredentialsBlobUpdated += SpotifyService_CredentialsBlobUpdated;
            this.EndOftrack += SpotifyService_EndOftrack;
            this.AudioBufferStatsRecieved += SpotifyService_AudioBufferStatsRecieved;
            session_callbacks.connection_error = Marshal.GetFunctionPointerForDelegate(ConnectionError);
            session_callbacks.connectionstate_updated = Marshal.GetFunctionPointerForDelegate(ConnectionStateUpdated);
            session_callbacks.credentials_blob_updated = Marshal.GetFunctionPointerForDelegate(CredentialsBlobUpdated);
            session_callbacks.end_of_track = Marshal.GetFunctionPointerForDelegate(EndOftrack);
            session_callbacks.get_audio_buffer_stats = Marshal.GetFunctionPointerForDelegate(AudioBufferStatsRecieved);
            session_config.api_version = 12;
            session_config.user_agent = "Spotify Ultra";
            session_config.cache_location = "Data";
            IntPtr g_appkey_ptr =  Marshal.AllocHGlobal(g_appkey.Length);
            Marshal.Copy(g_appkey, 0, g_appkey_ptr, g_appkey.Length);
            session_config.application_key = g_appkey_ptr;
            session_config.application_key_size = g_appkey.Length;
            IntPtr sp_callbacks_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(session_callbacks));
            Marshal.StructureToPtr(session_callbacks,  sp_callbacks_ptr, true);
            session_config.callbacks = sp_callbacks_ptr;
            session_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(8));
            libspotify.sp_session_create(ref session_config, out session_ptr);
            this.LogIn("drsounds", "carin123");
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
#endif
        }

        void SpotifyService_AudioBufferStatsRecieved(IntPtr sp_session, libspotify.sp_audio_buffer_stats stats)
        {
            
        }

        void SpotifyService_EndOftrack(IntPtr sp_session)
        {
            
        }

        void SpotifyService_CredentialsBlobUpdated(IntPtr sp_session, IntPtr blob)
        {
            
        }

        void SpotifyService_ConnectionStateUpdated(IntPtr sp_session)
        {
            
        }

        void SpotifyService_ConnectionError(IntPtr sp_session, libspotify.sp_error error)
        {
            
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
        public Mutex artistMutex = new Mutex();
        public Mutex releaseMutex = new Mutex();
        public Dictionary<String, Artist> ArtistCache = new Dictionary<string, Artist>();
        public Artist LoadArtist(IntPtr ptrArtist)
        {
            lock (artistMutex)
            {
                Artist artist = new Artist(this);
               
                IntPtr ptrName = libspotify.sp_artist_name(ptrArtist);
                artist.Name = Marshal.PtrToStringAnsi(ptrName);
                libspotify.sp_artist_release(ptrArtist);
                return artist;
            }
        }
        public Artist LoadArtist(string identifier)
        {
            
            
           
                IntPtr ptrLink = libspotify.sp_link_create_from_string("spotify:artist:" + identifier);
                IntPtr ptrArtist = libspotify.sp_link_as_artist(ptrLink);

                return LoadArtist(ptrArtist);
           
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
        public Release LoadRelease(IntPtr ptrRelease)
        {
            Release release = new Release(this);
            // Wait until the release is loaded
            while (!libspotify.sp_album_is_loaded(ptrRelease)) { }

            IntPtr ptrName = libspotify.sp_album_name(ptrRelease);
            release.Name = Marshal.PtrToStringAnsi(ptrName);

            release.ReleaseDate = new DateTime(libspotify.sp_album_year(ptrRelease), 1, 1);
            IntPtr artist = libspotify.sp_album_artist(ptrRelease);
            release.Artist = LoadArtist(artist);



            libspotify.sp_album_release(ptrRelease);
            return release;
        }
        public Release LoadRelease(string identifier) 
        { 
            lock (releaseMutex)
            {
                IntPtr ptrLink = libspotify.sp_link_create_from_string("spotify:album:" + identifier);
                IntPtr ptrRelease = libspotify.sp_link_as_album(ptrLink);

                Release release = LoadRelease(ptrRelease);
                
                return release;
            }
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
        /// <summary>
        /// from https://github.com/jonasl/libspotify-sharp/blob/master/libspotify-sharp/src/libspotify.cs
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal static string GetString(IntPtr ptr, string defaultValue)
        {
            if (ptr == IntPtr.Zero)
                return defaultValue;

            System.Collections.Generic.List<byte> l = new System.Collections.Generic.List<byte>();
            byte read = 0;
            do
            {
                read = Marshal.ReadByte(ptr, l.Count);
                l.Add(read);
            }
            while (read != 0);

            if (l.Count > 0)
                return System.Text.Encoding.UTF8.GetString(l.ToArray(), 0, l.Count - 1);
            else
                return string.Empty;
        }

        Mutex mutex = new Mutex();
       
        public Track LoadTrack(string identifier)
        {
            if (Cache.ContainsKey(identifier))
            {
                return Cache[identifier];
            }
           
            // byte[] iBytes = Encoding.ASCII.GetBytes(identifier);
            Spotify.Track track = Spotify.Track.CreateFromLink(Spotify.Link.Create("spotify:track:" + identifier));
            while (!track.IsLoaded) ;
            Track t = new Track(this);
            t.Name = track.Name;



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
        libspotify.sp_session_callbacks callbacks;
        public LogInResult LogIn(string userName, string passWord)
        {
            session.OnLoginComplete += session_OnLoginComplete;
           Spotify.sp_error error= session.LogInSync(userName, passWord, new System.TimeSpan( 10000));
        
                return LogInResult.Failure;
        }

        void session_OnLoginComplete(Spotify.Session sender, Spotify.SessionEventArgs e)
        {
            Spotify.Track track = Spotify.Track.CreateFromLink(Spotify.Link.Create("spotify:track:6APMhJwtqQMf7jDzqkNYgf"));
            while (!track.IsLoaded) ;
            Console.Write(track.Name);
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
        public Track MakeUserTrackFromString(String row)
        {
            String[] parts = row.Split(':');
            Track track = new Track(this,  parts[4]);
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
                Track pt = MakeUserTrackFromString(strtrack);
                tc.Add((Track)pt);
            }
            return tc;
        }

        public event TrackChangedEventHandler TrackDeleted;

        public event TrackChangedEventHandler TrackAdded;

        public event TrackChangedEventHandler TrackReordered;


        public bool ReorderTracks(Playlist playlist, int startPos, Track[] tracks, int newPos)
        {
            throw new NotImplementedException();
        }


        public bool DeleteTracks(Playlist playlist, int[] indexes)
        {
            throw new NotImplementedException();
        }


        public bool ReorderTracks(Playlist playlist, int startPos, int[] tracks, int newPos)
        {
            throw new NotImplementedException();
        }


        public string NewPlaylist(string name)
        {
            throw new NotImplementedException();
        }


        public event UserObjectsventHandler ObjectsDelivered;

        public void RequestUserObjects()
        {
            throw new NotImplementedException();
        }


        public void MoveUserObject(int oldPos, int newPos)
        {
            throw new NotImplementedException();
        }

        public void insertUserObject(string uri, int pos)
        {
            throw new NotImplementedException();
        }
    }
}
