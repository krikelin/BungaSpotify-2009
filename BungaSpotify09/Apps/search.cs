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
    public class search : Spider.App
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
        public search(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            Template = "views\\search.xml";
            InitializeComponent();
            Start();
        }
        public override string GetName()
        {
            return this.query;
        }
        public override SPListItem.ListIcon GetIcon()
        {
            return new SPListItem.ListIcon()
            {
                Normal = Properties.Resources.ic_search_normal,
                Selected = Properties.Resources.ic_search_selected
            };
        }
        private String query;
        public override object Loading(object arguments)
        {
            String[] parameters = (String[])arguments;

            this.query = parameters[3];
            IMusicService service = this.Host.MusicService;
            SearchResult res = service.Find(parameters[2], 12, 1);
            
            return new
            {
                Search = res
            };
            
        }

        private void artist_Load(object sender, EventArgs e)
        {

        }
    }
}


