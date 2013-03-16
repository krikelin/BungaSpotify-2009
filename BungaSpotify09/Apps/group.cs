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
    class group : Spider.App
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // artist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "album";
            this.Template = "views\\group.xml";
            this.Load += new System.EventHandler(this.group_Load);
            this.ResumeLayout(false);

        }
        public group(SpiderHost host, String[] arguments)
            : base(host, arguments)
        {
            InitializeComponent();
            Start();
        }
        Release release;
        public override object Loading(object arguments)
        {
            
            return new
            {
                
            };

        }
        public override string GetName()
        {
            return "My Group";
        }
        private void group_Load(object sender, EventArgs e)
        {
            
        }
        public override string GetSubName()
        {
            return "";
        }
        public override SPListItem.ListIcon GetIcon()
        {
            return new SPListItem.ListIcon()
            {
                Normal = Properties.Resources.ic_album_normal,
                Selected = Properties.Resources.ic_album_selected
            };
        }
    }
}
