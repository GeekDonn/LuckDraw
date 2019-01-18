using CefSharp;
using CefSharp.WinForms;
using LuckDraw.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LuckDraw
{
    public partial class Form1 : Form
    {
        JsonUtility jsons = new JsonUtility();
        public Form1()
        {
            InitializeComponent();
            ReadFile();
            GetWebPage(Application.StartupPath + "\\lottery\\index.html");
        }
        ChromiumWebBrowser webbrowser = null;
        public void GetWebPage(String url)
        {
            CefSettings settings = new CefSettings();

            Cef.Initialize(settings);
            webbrowser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill
            };
            //webbrowser.AllowDrop = false;
            webbrowser.MenuHandler = new MenuHandler();
            tabPage1.Controls.Add(webbrowser);
            webbrowser.Dock = DockStyle.Fill;

        }

        public void ReadFile()
        {
            StreamReader reder = File.OpenText(Application.StartupPath + "\\lottery\\js\\member.js");
       
            String json = reder.ReadToEnd().Replace("var member = ", "").Replace("\r\n", "").TrimEnd(';');
            reder.Close();
            //
            JsonUtility jsons = new JsonUtility();

            IList<Item> list = jsons.JsonToObject<IList<Item>>(json);
            this.dataGridView1.Rows.Clear();
            foreach (Item item in list)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(this.dataGridView1, item.name);
                row.Tag = item;
                this.dataGridView1.Rows.Add(row);
            }
            
        }


        public void SaveFile()
        {
            string json = "var member = " + GetJson() + ";";
            //StreamWriter sw = new StreamWriter(Application.StartupPath + "\\lottery\\js\\member.js");
            //sw.Write(json);
            //sw.Flush();
            //sw.Close();
            File.WriteAllText(Application.StartupPath + "\\lottery\\js\\member.js", json);
            //String json = reder.ReadToEnd().Replace("", "").TrimEnd(';').Replace("\r\n", "");
            ////

            //IList<Item> list = jsons.JsonToObject<IList<Item>>(json);
            //this.dataGridView1.Rows.Clear();
            //foreach (Item item in list)
            //{
            //    DataGridViewRow row = new DataGridViewRow();
            //    row.CreateCells(this.dataGridView1, item.name);
            //    row.Tag = item;
            //    this.dataGridView1.Rows.Add(row);
            //}

        }

        public String GetJson()
        {
            IList<Item> list = new List<Item>();
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                Item ite = new Item() {name= item.Cells[0].FormattedValue?.ToString(),phone=""};
                if (!String.IsNullOrEmpty(ite.name))
                {
                    list.Add(ite);
                }
            }
            return jsons.ObjectToJson(list);
        }

        /// <summary>
        /// 窗体大小发生变化，浏览器刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            webbrowser.Reload();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            webbrowser.Reload();
        }
    }
    public class Item
    {
        public String phone { get; set; }
        public String name { get; set; }
    }
    internal class MenuHandler : IContextMenuHandler
    {
        void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Clear();
        }

        bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }

        void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            //throw new NotImplementedException();
        }

        bool IContextMenuHandler.RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }

}
