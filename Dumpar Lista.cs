using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using SKYPE4COMLib;
namespace Tool
{
    public partial class Dumpar_Lista : Form
    {
        public Dumpar_Lista()
        {
            InitializeComponent();
        }
        public List<User> Usuarios = new List<User>();
        private void Dumpar_Lista_Load(object sender, EventArgs e)
        {
            foreach (User usuario in Usuarios)
            {
                listBox1.Items.Add(usuario.Handle);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button2.Enabled = false;
            button1.Enabled = true;
        }
        public int Index = 0;
        void DumparLista()
        {
              try
            {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "";
            sfd.Filter = "txt files (*.txt)|*.txt";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] linhas = new string[listBox2.Items.Count - 1];
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    linhas[i] = listBox2.Items[i].ToString();
                }
                System.IO.File.WriteAllLines(sfd.FileName, linhas);
            }
            }
              catch (Exception ex)
              {

              }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
                if (Index < Usuarios.Count)
                {
                    string IP = IPTools.Skype2IP(Usuarios[Index].Handle);
                    listBox1.Items[Index] = "Tentando...";
                    if (!IP.Contains("No"))
                    {
                        listBox2.Items.Add(Usuarios[Index].FullName + "-" + IP);
                    }
                    Index++;
                }
                else
                {
                    timer1.Stop();

                }
            
         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DumparLista();
        }
    }
  
}
