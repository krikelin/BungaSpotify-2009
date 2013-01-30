using Spider;
using SpiderView.ProtocolBuffer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net; 
using System.Net.Sockets;
using System.Windows.Forms; 
namespace BungaSpotify09.Apps
{
    class app : Spider.App
    {
        public Dictionary<String, List<Object>> Messages { get; set; }
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
            this.Messages = new Dictionary<string, List<object>>();

        }
        public Protocol protocol;
        public void LoadScheme(String file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                protocol = new Protocol(sr.ReadToEnd());
                sr.Close();
                
            }
            
        }

        public app(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            InitializeComponent();
        }
        public override void Navigate(string[] arguments)
        {
            base.Navigate(arguments);
            String folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
           
            this.Spider.LoadFile( folder + "\\Spider\\" + arguments[0] + ".xml");
        }
        private void artist_Load(object sender, EventArgs e)
        {
            
        }
    }
}
