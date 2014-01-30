using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BungaSpotify09
{
    #region LibSpotify
    public class Libspotify
    {
        public struct sp_offline_sync_status
        {
            int queued_tracks;
            int done_tracks;
            int copied_tracks;
            int willnotcopy_tracks;
            int error_tracks;
            bool syncing;

        }
        public struct sp_subscribers
        {
            uint count;
            IntPtr subscribers;
        }
        public struct sp_audio_buffer_stats
        {
            int samples;
            int stutter;
        }
        public enum sp_error {
            SP_ERROR_OK,
            SP_ERROR_BAD_API_VERSION,
            SP_ERROR_API_INITIALIZATION_FAILED,
            SP_ERROR_TRACK_NOT_PLAYABLE,
            SP_ERROR_BAD_APPLICATION_KEY,
            SP_ERROR_BAD_USERNAME_OR_PASSWORD,
            SP_ERROR_USER_BANNED,
            SP_ERROR_UNABLE_TO_CONTACT_SERVER,
            SP_ERROR_CLIENT_TOO_OLD,
            SP_ERROR_OTHER_PERMANENT,
            SP_ERROR_BAD_USER_AGENT,
            SP_ERROR_MISSING_CALLBACK,
            SP_ERROR_INVALID_INDATA,
            SP_ERROR_INDEX_OUT_OF_RANGE,
            SP_ERROR_USER_NEEDS_PREMIUM,
            SP_ERROR_OTHER_TRANSIENT,
            SP_ERROR_IS_LOADING,
            SP_ERROR_NO_STREAM_AVAILABLE,
            SP_ERROR_PERMISSION_DENIED,
            SP_ERROR_INBOX_IS_FULL,
            SP_ERROR_NO_CACHE,
            SP_ERROR_NO_SUCH_USER,
            SP_ERROR_NO_CREDENTIALS,
            SP_ERROR_NETWORK_DISABLED,
            SP_ERROR_INVALID_DEVICE_ID,
            SP_ERROR_CANT_OPEN_TRACE_FILE,
            SP_ERROR_APPLICATION_BANNED,
            SP_ERROR_OFFLINE_TOO_MANY_TRACKS,
            SP_ERROR_OFFLINE_DISK_CACHE,
            SP_ERROR_OFFLINE_EXPIRED,
            SP_ERROR_OFFLINE_NOT_ALLOWED,
            SP_ERROR_OFFLINE_LICENSE_LOST,
            SP_ERROR_OFFLINE_LICENSE_ERROR,
            SP_ERROR_LASTFM_AUTH_ERROR,
            SP_ERROR_INVALID_ARGUMENT,
            SP_ERROR_SYSTEM_FAILURE
        }
        public enum sp_sampletype 
        { 
            SP_SAMPLETYPE_INT16_NATIVE_ENDIAN = 0 
        }
        public enum sp_bitrate
        {
            SP_BITRATE_160k = 0,
            SP_BITRATE_320k = 1,
            SP_BITRATE_96k = 2
        }
        public enum sp_playlist_type { 
          SP_PLAYLIST_TYPE_PLAYLIST = 0, 
          SP_PLAYLIST_TYPE_START_FOLDER = 1, 
          SP_PLAYLIST_TYPE_END_FOLDER = 2, 
          SP_PLAYLIST_TYPE_PLACEHOLDER = 3 
        }
        public enum sp_availability
        {
            SP_TRACK_AVAILABILITY_UNAVAILABLE = 0,
            SP_TRACK_AVAILABILITY_AVAILABLE = 1,
            SP_TRACK_AVAILABILITY_NOT_STREAMABLE = 2,
            SP_TRACK_AVAILABILITY_BANNED_BY_ARTIST = 3
        }
        public enum sp_track_offline_status
        {
            SP_TRACK_OFFLINE_NO = 0,
            SP_TRACK_OFFLINE_WAITING = 1,
            SP_TRACK_OFFLINE_DOWNLOADING = 2,
            SP_TRACK_OFFLINE_DONE = 3,
            SP_TRACK_OFFLINE_ERROR = 4,
            SP_TRACK_OFFLINE_DONE_EXPIRED = 5,
            SP_TRACK_OFFLINE_LIMIT_EXCEEDED = 6,
            SP_TRACK_OFFLINE_DONE_RESYNC = 7
        }

        public enum sp_connection_type
        {
            SP_CONNECTION_TYPE_UNKNOWN = 0,
            SP_CONNECTION_TYPE_NONE = 1,
            SP_CONNECTION_TYPE_MOBILE = 2,
            SP_CONNECTION_TYPE_MOBILE_ROAMING = 3,
            SP_CONNECTION_TYPE_WIFI = 4,
            SP_CONNECTION_TYPE_WIRED = 5
        }
        public enum sp_connection_rules
        {
            SP_CONNECTION_RULE_NETWORK = 0x1,
            SP_CONNECTION_RULE_NETWORK_IF_ROAMING = 0x2,
            SP_CONNECTION_RULE_ALLOW_SYNC_OVER_MOBILE = 0x4,
            SP_CONNECTION_RULE_ALLOW_SYNC_OVER_WIFI = 0x8
        }
        public enum sp_connectionstate {
            SP_CONNECTION_STATE_LOGGED_OUT,
            SP_CONNECTION_STATE_LOGGED_IN,
            SP_CONNECTION_STATE_DISCONNECTED,
            SP_CONNECTION_STATE_UNDEFINED,
            SP_CONNECTION_STATE_OFFLINE
        }
        public enum sp_artistbrowse_type
        {
            SP_ARTISTBROWSE_FULL,
            SP_ARTISTBROWSE_NO_TRACKS,
            SP_ARTISTBROWSE_NO_ALBUMS
        }

        public enum sp_linktype
        {
            SP_LINKTYPE_INVALID = 0,
            SP_LINKTYPE_TRACK = 1,
            SP_LINKTYPE_ALBUM = 2,
            SP_LINKTYPE_ARTIST = 3,
            SP_LINKTYPE_SEARCH = 4,
            SP_LINKTYPE_PLAYLIST = 5,
            SP_LINKTYPE_PROFILE = 6,
            SP_LINKTYPE_STARRED = 7,
            SP_LINKTYPE_LOCALTRACK = 8,
            SP_LINKTYPE_IMAGE = 9
        }
        public enum sp_imageformat
        {
            SP_IMAGE_FORMAT_UNKNOWN = -1,
            SP_IMAGE_FORMAT_JPEG = 0
        }

        public delegate void image_loaded_cb(IntPtr image, IntPtr userdata);

        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_error_message(sp_error error);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_create(IntPtr sp_sesion_config, ref IntPtr session);
        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_release(IntPtr sess);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_login(IntPtr session, IntPtr username, IntPtr password, bool remember_me, IntPtr blob);
        [DllImport("libspotify.dll")]
        public static extern int sp_session_remembered_user(IntPtr session, IntPtr buffer, int buffer_size);

        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_session_user_name(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_forgot_me(IntPtr session);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_session_user(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_logout(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_flush_caches(IntPtr session);
        [DllImport("libspotify.dll")]
        public static extern sp_connectionstate sp_session_connectionstate(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_session_userdata(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_sesion_set_cache_size(IntPtr session, int size);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_process_events(IntPtr session, IntPtr next_timeout);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_player_load(IntPtr session, IntPtr track);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_player_seek(IntPtr session, int offset);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_player_play(IntPtr session, bool play);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_player_unload(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_session_playlistcontainer(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_sesion_inbox_create(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_session_starred_create(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_session_publishedcontainer_for_user_create(IntPtr session, IntPtr canoncial_username);

        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_session_preferred_bitrate(IntPtr session, sp_bitrate bitrate);

        [DllImport("libspotify_dll")]
        public static extern IntPtr sp_session_preferred_offline_bitrate(IntPtr session, sp_bitrate bitrate, bool allow_resync);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_session_get_volume_normalization(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern sp_error sp_sesion_set_connection_type(IntPtr session, sp_connection_type type);
        [DllImport("libspotify.dll")]
        public static extern sp_error sp_sesion_set_connection_rules(IntPtr session, sp_connection_rules rules);

        [DllImport("libspotify.dll")]
        public static extern int sp_offline_tracks_to_sync(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern int sp_offline_num_playlists(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern int sp_session_user_country(IntPtr session);

        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_string(IntPtr link);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_track(IntPtr track, int offset);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_album(IntPtr album);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_album_cover(IntPtr album, sp_image_size size);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_artist(IntPtr artist);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_artist_portrait(IntPtr artist, sp_image_size size);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_artistbrowse_portrait(IntPtr arb, int index);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_search(IntPtr search);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_playlist(IntPtr playlist);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_user(IntPtr user);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_create_from_image(IntPtr image);
        [DllImport("libspotify.dll")]
        public static extern int sp_link_as_string(IntPtr link, IntPtr buffer, int buffer_size);
        [DllImport("libspotify.dll")]
        public static extern sp_linktype sp_link_type(IntPtr link);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_as_track(IntPtr link);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_as_track_and_offset(IntPtr link, int* offset);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_as_album(IntPtr link);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_as_artist(IntPtr link);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_link_as_user(IntPtr link);
        [DllImport("libspotify.dll")]
        public static extern sp_error sp_link_add_ref(IntPtr link);
        [DllImport("libspotify.dll")]
        public static extern sp_error sp_link_release(IntPtr link);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_image_create (IntPtr session, byte[] image_id);
        [DllImport("libspotify.dll")]
        public static extern IntPtr sp_image_create_from_link(IntPtr session, IntPtr l);

        public static int SPOTIFY_API_VERSION = 12;
       
        public delegate void sp_logged_in(IntPtr sp_session, sp_error error);
        public delegate void sp_logged_out(IntPtr sp_session);
        public delegate void sp_metadata_updated(IntPtr session);
        public delegate void sp_connection_error(IntPtr session, sp_error error);
        public delegate void sp_message_to_user(IntPtr session, IntPtr message);
        public delegate void sp_notify_main_thread(IntPtr session);
        public delegate void sp_music_delivery(IntPtr session, IntPtr format, IntPtr frames, int num_frames);
        public delegate void sp_play_token_lost(IntPtr session);
        public delegate void sp_log_message(IntPtr session);
        public delegate void sp_end_of_track(IntPtr session);
        public delegate void sp_streaming_error(IntPtr session);
        public delegate void sp_userinfo_updated(IntPtr session);
        public delegate void sp_start_playback(IntPtr session);
        public delegate void sp_stop_playback(IntPtr session);
        public delegate void sp_get_audio_buffer_stats(IntPtr session, IntPtr stats);
        public delegate void sp_offline_status_updated(IntPtr session);
        public delegate void sp_offline_error(IntPtr session);
        public delegate void sp_credentials_blob_updated(IntPtr session, IntPtr blob);
        public delegate void sp_connectionstate_updated(IntPtr session);
        public delegate void sp_scrobble_error(IntPtr session, sp_error error);
        public delegate void sp_private_session_mode_changed(IntPtr session, bool is_private);
        

        public struct sp_session_callbacks
        {
            sp_logged_in logged_in;
            sp_logged_out logged_out;
            sp_metadata_updated metadata_updated;
            sp_connection_error connection_error;
            sp_message_to_user message_to_user;
            sp_notify_main_thread notify_main_thread;
            sp_music_delivery music_delivery;
            sp_play_token_lost play_token_lost;
            sp_log_message log_message;
            sp_end_of_track end_of_track;
            sp_streaming_error streaming_error;
            sp_userinfo_updated userinfo_updated;
            sp_start_playback start_playback;
            sp_stop_playback stop_playback;
            sp_get_audio_buffer_stats get_audio_buffer_stats;
            sp_offline_status_updated offline_status_updated;
            sp_offline_error offline_error;
            sp_credentials_blob_updated credentials_blob_updated;
            sp_connectionstate_updated connectionstate_updated;
            sp_scrobble_error scrobble_error;
            sp_private_session_mode_changed private_session_mode_changed;

        }

        public struct sp_session_config
        {
            int api_version;
            IntPtr cache_location;
            IntPtr settings_location;
            IntPtr application_key;
            int application_key_size;
            IntPtr application_key_size;
            sp_session_callbacks callbacks;
            IntPtr userdata;
            bool compress_playlists;
            bool dont_save_metadata_for_playlists;
            bool initially_unload_playlists;
            IntPtr device_id;
            IntPtr proxy;
            IntPtr proxy_username;
            IntPtr proxy_password;
            IntPtr ca_certs_filename;
            IntPtr tracefile;
        }
        
    }
    #endregion
    class SpotifyService : Spider.Media.IMusicService
    {
       
        
        public event Spider.Media.TrackChangedEventHandler TrackDeleted;

        public event Spider.Media.TrackChangedEventHandler TrackAdded;

        public event Spider.Media.TrackChangedEventHandler TrackReordered;

        public event Spider.Media.PlayStateChangedEventHandler PlaybackFinished;

        public event Spider.Media.UserObjectsventHandler ObjectsDelivered;

        public event Spider.Media.PlayStateChangedEventHandler PlaybackStarted;

        public string Namespace
        {
            get { return "spotify"; }
        }

        public string Name
        {
            get { return "Spotify"; }
        }

        public void Play(Spider.Media.Track track)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Seek(int pos)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.Artist LoadArtist(string identifier)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.Track LoadTrack(string identifier)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.ReleaseCollection LoadReleasesForGivenArtist(Spider.Media.Artist artist, Spider.Media.ReleaseType type, int page)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.Release LoadRelease(string identifier)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.TrackCollection LoadTracksForGivenRelease(Spider.Media.Release release)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.Playlist LoadPlaylist(string username, string identifier)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.SearchResult Find(string query, int maxResults, int page)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.Track NowPlayingTrack
        {
            get { throw new NotImplementedException(); }
        }

        public Spider.Media.TopList LoadTopListForResource(Spider.Media.Resource res)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.User LoadUser(string identifier)
        {
            throw new NotImplementedException();
        }
       
        public Spider.Media.SessionState SessionState
        {
            get { throw new NotImplementedException(); }
        }



        public Spider.Media.LogInResult LogIn(string userName, string passWord)
        {
            return Spider.Media.LogInResult.Failure;   
        }

        public Spider.Media.User GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        public bool InsertTrack(Spider.Media.Playlist playlist, Spider.Media.Track track, int pos)
        {
            throw new NotImplementedException();
        }

        public bool ReorderTracks(Spider.Media.Playlist playlist, int startPos, int[] tracks, int newPos)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTracks(Spider.Media.Playlist playlist, int[] indexes)
        {
            throw new NotImplementedException();
        }

        public string NewPlaylist(string name)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.TrackCollection GetPlaylistTracks(Spider.Media.Playlist playlist, int revision)
        {
            throw new NotImplementedException();
        }

        public Spider.Media.Country GetCurrentCountry()
        {
            throw new NotImplementedException();
        }

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

        public Spider.Media.TrackCollection GetCollection(string type, string identifier)
        {
            throw new NotImplementedException();
        }
    }
}
