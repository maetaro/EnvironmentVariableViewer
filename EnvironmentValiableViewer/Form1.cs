using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Search();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            listView1.Items.Clear();

            EnvironmentVariableTarget target = EnvironmentVariableTarget.Machine;
            if (radioButton1.Checked)
            {
                target = EnvironmentVariableTarget.Process;
            }
            else if (radioButton2.Checked)
            {
                target = EnvironmentVariableTarget.User;
            }
            else if (radioButton3.Checked)
            {
                target = EnvironmentVariableTarget.Machine;
            }
            System.Environment.GetEnvironmentVariables(target)
                .OfType<System.Collections.DictionaryEntry>()
                .Where(item =>
                {
                    if (!string.IsNullOrWhiteSpace(txtKey.Text)
                       && !item.Key.ToString().ToUpper().Contains(txtKey.Text.Trim().ToUpper()))
                    {
                        return false;
                    }
                    if (!string.IsNullOrWhiteSpace(txtValue.Text)
                        && !item.Value.ToString().ToUpper().Contains(txtValue.Text.Trim().ToUpper()))
                    {
                        return false;
                    }
                    return true;
                })
                .ToList()
                .ForEach(item =>
                {
                    ListViewItem subitem = new ListViewItem();
                    subitem.Text = item.Key.ToString();
                    subitem.SubItems.Add(item.Value.ToString());
                    listView1.Items.Add(subitem);
                });
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            Form f = new Form();
            ListView lv = new ListView();
            lv.Columns.Add("Value", 500);
            var items = listView1.SelectedItems[0].SubItems[1].Text.Split(';')
                .Select(item => new ListViewItem() { Text = item })
                .ToArray();
            lv.Items.AddRange(items);
            lv.DoubleClick += (sender2, e2) =>
                {
                    ListView lv2 = (ListView)sender2;
                    string path = lv2.SelectedItems[0].Text;
                    if (!System.IO.Directory.Exists(path))
                    {
                        return;
                    }
                    System.Diagnostics.Process.Start("explorer.exe", path);
                };
            lv.View = View.Details;
            lv.Dock = DockStyle.Fill;
            f.Controls.Add(lv);
            f.Text = listView1.SelectedItems[0].Text;
            f.ShowDialog();
        }

    }
}
