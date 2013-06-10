using LuaInterface;
using Newtonsoft.Json.Linq;
using Spider;
using SpiderView.ProtocolBuffer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms; 
namespace BungaSpotify09.Apps
{
    class app : Spider.App
    {
        System.Net.WebSockets.ClientWebSocket webSocket;
        public Dictionary<String, List<Object>> Messages { get; set; }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // artist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "app";
            this.Load += new System.EventHandler(this.artist_Load);
            this.ResumeLayout(false);
            this.Messages = new Dictionary<string, List<object>>();

        }
        public Protocol Protocol;
        private System.Net.WebSockets.ClientWebSocket streamWebSocket;
        public void LoadScheme(String file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                Protocol = new Protocol(sr.ReadToEnd());
                sr.Close();
            }
            
        }
        String folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        public delegate void lua_send_request(object data);
        public delegate void lua_set_entity(object entity);
        public delegate void lua_alax(string method, string url, string onreadystatechange);
        public void setEntity(object entity)
        {
            this.spiderView.Refresh(entity);
        }
        public class AlaxArgs
        {
            public String url;
            public string method;
            public String callback;
            public JObject data;
        }
        public void alax(String method, String url, String onreadystatechange)
        {
            
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork +=bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync(new AlaxArgs() { callback = onreadystatechange, method = method, url = url });
    
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AlaxArgs data = (AlaxArgs)e.Result;

            this.spiderView.Scripting.InvokeFunction(data.callback, data.data);
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            AlaxArgs args = (AlaxArgs)e.Argument;
            WebClient wc = new WebClient();
            try
            {
                String data = wc.DownloadString(args.url);
                JObject obj = JObject.Parse(data);
                args.data = obj;
                e.Result = args;
            }
            catch (Exception ex)
            {
                e.Result = "error";
            }
        }
        public object ask(object data)
        {
           
            LuaTable table = (LuaTable)data;
            foreach (LuaTable field in table.Values)
            {
                List<InputField> inputFields = new List<InputField>();
                foreach (LuaTable t in table.Values)
                {
                    int i = 0;
                    InputField inputField = new InputField();
                    foreach (string tt in t.Keys)
                    {
                        if (tt == "title")
                        {
                            inputField.Title = (String)t.Values.OfType<string>().ElementAt(i);
                        }
                        if (tt == "key")
                        {
                            inputField.Key = (String)t.Values.OfType<string>().ElementAt(i);
                        }

                    }
                    inputFields.Add(inputField);

                }
                InputForm inputForm = new InputForm(inputFields);
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return inputForm.Data;
                }

            }
            return null;
           
        }
        private string[] arguments;
        public app(SpiderHost host, String[] arguments)
            : base(host, arguments)
            
        {
            this.arguments = arguments;
            InitializeComponent();
            this.Template = (folder + "\\Spider\\" + arguments[2] + "\\view.xml");
          //  LoadScheme(folder + "\\spider\\" + arguments[0] + "\\view.proto");


          
            // Register the send object
            this.spiderView.Scripting.RegisterFunction("sendRequest", new lua_send_request(luaSendRequest), "");
            this.spiderView.Scripting.RegisterFunction("alax", new lua_alax(alax), "");
            this.spiderView.Scripting.RegisterFunction("setEntity", (object)this, typeof(app).GetMethod("setEntity"));
            this.spiderView.Scripting.RegisterFunction("ask", (object)this, typeof(app).GetMethod("ask"));

            Start();
            // Load Manifest
            using (StreamReader sr = new StreamReader(folder + "\\Spider\\" + arguments[2] + "\\manifest.json"))
            {
                String data = sr.ReadToEnd();


                Manifest = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(data);
            }
        }
        JObject Manifest;
#if(false)
        private async void connect()
        {
            
                webSocket = new System.Net.WebSockets.ClientWebSocket();
                // Have we connected yet?
                await webSocket.ConnectAsync(new Uri((String)Manifest["Endpoint"]), CancellationToken.None);
                await Task.WhenAll(receive(webSocket), send(webSocket));

            }
        }
        private async Task receive(ClientWebSocket socket)
        {
            byte[] buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {

                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                }
                else
                {
                    String data = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                }

            }
        }
        private async Task send(ClientWebSocket socket)
        {
            while (webSocket.State == WebSocketState.Open)
            {

            }
        }
#endif
        private void luaSendRequest(object data)
        {
        }
        public override void Navigate(string[] arguments)
        {
            base.Navigate(arguments);
            
        }
        private void artist_Load(object sender, EventArgs e)
        {
            
        }
    }
}
