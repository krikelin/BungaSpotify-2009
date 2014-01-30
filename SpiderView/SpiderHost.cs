﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spider.Media;

namespace Spider
{
    public partial class SpiderHost : UserControl
    {
        public event NotificationEventHandler Notify;
        Dictionary<String, IMusicService> Services { get; set; }
        public IMusicService MusicService { get; set; }
        public Dictionary<String, App> Apps = new System.Collections.Generic.Dictionary<string, App>();
        public Dictionary<String, Type> RegistredAppTypes = new Dictionary<string, Type>();
        public Stack<String> History = new Stack<string>();
        public Stack<String> Future = new Stack<string>();
        public Form Form;
        public String CurrentURI = "";

        public PlayQueue PlayQueue { get; set; }
        public PlayQueue PlayHistory { get; set; }
        public Board PlayContext; // The context of playback
        public void Play(track track)
        {
            track.Track.Play();
        }
        public void PlayNext()
        {
            Track lastTrack = null;
            if(PlayContext != null)
            for (int i = 0; i <  PlayContext.Tracks.Count; i++)
            {
                track track = PlayContext.Tracks[i];
                if (lastTrack != null && lastTrack.Status == Track.State.Available)
                {
                    MusicService.Stop();
                    track.Track.Play();
                    return;
                }
                if (track.Track.Playing)
                {
                    lastTrack = track.Track;
                   
                }
                   
            }
            try
            {
                foreach (Spider.CListView.CListViewItem t in this.PlayContext.Section.ListView.Items)
                {

                    Track track = t.Track;
                    if (lastTrack != null && lastTrack.Status == Track.State.Available)
                    {
                        track.Play();
                        return;
                    }
                    if (track.Playing)
                    {
                        lastTrack = track;

                    }
                }
            }
            catch (Exception e)
            {
            }
        }

#if(false)
        public App LoadApp(String uri)
        {
            String[] segments = uri.Split(':');
            var ns = segments[1];
            String appId = uri;
            String[] arguments = new String[segments.Length - 2];
            System.Array.Copy(segments, 2, arguments, 0, segments.Length - 2);
            // If app is already loaded bring it to front
            if (Apps.ContainsKey(appId))
            {

                App app = Apps[appId];
               
                app.Navigate(arguments);
                if (this.Navigated != null)
                    this.Navigated(this, new SpiderNavigationEventArgs() { Arguments = arguments });
                Future.Clear();


                return app;
            }
            Type type = RegistredAppTypes[ns];
            App appClass = (App)type.GetConstructor(new Type[] { typeof(SpiderHost), typeof(String[]) }).Invoke(new Object[] { this, uri.Split(':') });
            appClass.Tag = Form;
            Apps.Add(appId, appClass);
            appClass.Navigate(segments);
            this.Controls.Add(appClass);

            appClass.Dock = DockStyle.Fill;
           
            Future.Clear();
            return appClass;
        }
        public List<App> RegistredAppTypes
        {
            get
            {
                return registredAppTypes;
            }
        }
        public void Navigate(String uri)
        {
            try
            {
                String[] segments = uri.Split(':');
                var ns = segments[1];
                String appId = uri ;
                String[] arguments = new String[segments.Length - 2];
                System.Array.Copy(segments, 2, arguments, 0, segments.Length - 2);
                // If app is already loaded bring it to front
                if (Apps.ContainsKey(appId))
                {
                    
                    App app = Apps[appId];
                    app.BringToFront();
                    app.Navigate(arguments);
                    if (this.Navigated != null)
                        this.Navigated(this, new SpiderNavigationEventArgs() { Arguments = arguments });
                    Future.Clear();
                    
                    app.Show();
                    foreach (Control a in this.Controls)
                    {
                        if (a != app)
                            a.Hide();
                    }
                    return;
                }
                Type type = RegistredAppTypes[ns];
                App appClass = (App)type.GetConstructor(new Type[] { typeof(SpiderHost), typeof(String[]) }).Invoke(new Object[] { this, uri.Split(':') });
                appClass.Tag = Form;
                Apps.Add(appId, appClass);
                appClass.Navigate(segments);
                this.Controls.Add(appClass);
                
                appClass.Dock = DockStyle.Fill;
                appClass.BringToFront();
                appClass.Show();
                foreach (Control a in this.Controls)
                {
                    if (a != appClass)
                        a.Hide();
                }
                if (this.Navigated != null)
                    this.Navigated(this, new SpiderNavigationEventArgs() { Arguments = arguments });
                Future.Clear();

            }
            catch (Exception e)
            {
                if (this.Notify != null)
                {
                    this.Notify(this, new NotificationEventArgs() { Text = "An error occured loading the app",  Type = NotificationType.Error });

                }
            }


        }
#endif
        public void OnNotify(object sender, NotificationEventArgs e)
        {
            if (this.Notify != null)
            {
                this.Notify(sender, e);

            }
        }

        /// <summary>
        /// Event arguments for navigation events
        /// </summary>
        public class SpiderNavigationEventArgs
        {
            public String[] Arguments { get; set; }
        }

        /// <summary>
        /// Event argumetns for app events
        /// </summary>
        public class SpiderAppEventArgs {
            public App App;
        }
        /// <summary>
        /// Delegate for navigation events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SpiderNavigationEventHandler(object sender, SpiderNavigationEventArgs e);
        public event SpiderNavigationEventHandler Navigated;
        public delegate void SpiderAppEvent(object sender, SpiderAppEventArgs e);
       
        
        public event SpiderAppEvent AppStarted;

        public bool Navigate(string uri)
        {
            foreach (App app in registredAppTypes)
            {
                if (app.AcceptsUri(uri))
                {
                    app.BringToFront();
                    app.Navigate(uri.Split(':'));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Loads an spider app into the host
        /// </summary>
        /// <param name="app">An instance of an app implementation</param>
        /// <param name="arguments">arguments to provide to the app</param>
        public void LoadApp(App app, String[] arguments)
        {
            this.Controls.Add(app);
            app.Dock = DockStyle.Fill;
        }
        public SpiderHost()
        {
            InitializeComponent();
        }
        public SpiderHost(IMusicService defaultService)
        {
            InitializeComponent();
            this.MusicService = defaultService;
            this.MusicService.PlaybackFinished += MusicService_PlaybackFinished;
            registredAppTypes = new List<App>();
        }
        public void RegisterAppType(App app)
        {
            this.registredAppTypes.Add(app);
            this.Controls.Add(app);
            app.Dock = DockStyle.Fill;
        }
        void MusicService_PlaybackFinished(object sender, EventArgs e)
        {
            PlayNext();
        }

        private void SpiderHost_Load(object sender, EventArgs e)
        {

        }

        public List<App> registredAppTypes { get; set; }
    }
}
