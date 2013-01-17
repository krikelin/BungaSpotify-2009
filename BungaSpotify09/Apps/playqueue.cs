using Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BungaSpotify09.Apps
{
    public class playqueue : Spider.App
    {

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // whatsnew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "whatsnew";
            this.Load += new System.EventHandler(this.playqueue_Load);
            this.ResumeLayout(false);

        }
        public playqueue (SpiderHost host) : base(host) {
            InitializeComponent();
        }
        private void playqueue_Load(object sender, EventArgs e)
        {
            this.Spider.LoadFile("views\\playqueue.xml");
        }
    }
}
