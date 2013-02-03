using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BungaSpotify09
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            // BungaSpotify.Service.SpotifyService spotify = new BungaSpotify.Service.SpotifyService();
            // spotify.LoadTrack("6APMhJwtqQMf7jDzqkNYgf");
        }
    }
}
