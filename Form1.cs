using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SKYPE4COMLib;
using System.Net;
namespace Tool
{
    public partial class Form1 : Form
    {
        Skype skype = new Skype();
        List<User> Usuarios = new List<User>();
        public Form1()
        {
            InitializeComponent();
        }
        void CarregarUsuarios()
        {
            listBox1.Items.Clear();
            Usuarios.Clear();
            foreach (User usuario in skype.Friends)
            {
                if(usuario.FullName != "")
                {
                    if(!ListaNegra.Contains(usuario.Handle))
                    {
                        listBox1.Items.Add(usuario.FullName);
                        Usuarios.Add(usuario);
                    }
                   
                
                }
            }
            listBox1.SelectedIndex = 0;

        }
        string[] ListaNegra = { "bielzaao", "dwordhd", "ialvejadoi", "maikyroger-", "live:renan.gomes_2", "live:srdocau", "higor.barbosa8" };
        void UsandoThreads()
        {
            int d = listBox1.SelectedIndex;
            Thread t = new Thread(() => Apis(d));
            t.Start();  
        }
          public void AppendTextBox1(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox1), new object[] { value });
                return;
            }
            textBox1.Text = value;
        }
        public void AppendTextBox2(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox2), new object[] { value });
                return;
            }
            textBox2.Text = value;
        }

        void Apis(int id)
        {
            int i = id;
            try
            {
                string IP = IPTools.Skype2IP(Usuarios[i].Handle);
                if (IP.Contains("No") == true)
                {
                    AppendTextBox1("Não foi possivel pegar");
                }
                else
                {
                    string Loc = IPTools.IP2Loc(IP);
                    AppendTextBox1(IP);
                    AppendTextBox2(Loc);
                }
            }
            catch (Exception ex)
            {

            }
           
         
         
        
        }
        void CarregarPefil(int id)
        {
            pictureBox1.ImageLocation = "https://api.skype.com/users/" + Usuarios[id].Handle + "/profile/avatar";
            lblUser.Text = "Usuario: " + Usuarios[id].Handle;
            lblEmail.Text = "Online: " + Usuarios[id].OnlineStatus.ToString().Replace("ols","");
            lblCity.Text = "Cidade: " + Usuarios[id].Country;
            lblSex.Text = "Sexo: " + Usuarios[id].Sex.ToString().Replace("usex", "");
            lblNascido.Text = "Nascido: " + Usuarios[id].Birthday.Day + "/" + Usuarios[id].Birthday.Month + "/" + Usuarios[id].Birthday.Year;
            lblComment.Text = Usuarios[id].RichMoodText;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer2.Start();
            skype.Attach();
            CarregarUsuarios();
        }
        private void EnviarMsg(string msg, string dest)
        {
            skype.SendMessage(dest, msg);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarPefil(listBox1.SelectedIndex);
            textBox1.Text = "Tentando pegar...";
            textBox2.Text = "";
            UsandoThreads();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            Dumpar_Lista frm = new Dumpar_Lista();
            frm.Usuarios = Usuarios;
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "";
                sfd.Filter = "txt files (*.txt)|*.txt";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string[] linhas = new string[listBox1.Items.Count - 1];
                    for (int i = 0; i < listBox1.Items.Count - 1; i++)
                    {
                        linhas[i] = listBox1.Items[i].ToString();
                    }
                    System.IO.File.WriteAllLines(sfd.FileName, linhas);
                }
            }
            catch (Exception ex)
            {

            }
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                EnviarMsg(Usuarios[listBox1.SelectedIndex].Handle, textBox3.Text);
            }
            else
            {
                foreach (User usuario in Usuarios)
            {
                EnviarMsg(textBox3.Text, usuario.Handle);
            }
            }
        }

        int NumEnv = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {

            EnviarMsg(textBox4.Text, Usuarios[listBox1.SelectedIndex].Handle);
            NumEnv++;
            label4.Text = "Env: " + NumEnv;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Start();
            button4.Enabled = false;
            button5.Enabled = true;
         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button5.Enabled = false;
            button4.Enabled = true;
        }
        int corindex = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (corindex == 0)
            {
                label6.ForeColor = Color.Blue;
                label6.BackColor = Color.Red;
                corindex = 1;
            }
            else if (corindex == 1)
            {
                label6.ForeColor = Color.Red;
                label6.BackColor = Color.Green;
                corindex = 2;
            }
            else if (corindex == 2)
            {
                label6.ForeColor = Color.Green;
                label6.BackColor = Color.Blue;
                corindex = 0;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                skype.ClearChatHistory();
                skype.ClearCallHistory();
                skype.ClearVoicemailHistory();
            }
            catch (Exception ex) { }
         
        }
   

    
    }
    public class IPTools
    {
        static WebClient wc = new WebClient();
        public static string Skype2IP(string nome)
        {
            return wc.DownloadString("http://api.predator.wtf/resolver/?arguments=" + nome);
        }
        public static string NMAP(string IP)
        {
            return wc.DownloadString("http://api.hackertarget.com/nmap/?q=" + IP);
        }
        public static string Skype2Usuario(string IP)
        {
            return wc.DownloadString("http://api.predator.wtf/lookup/?arguments=" + IP);
        }
        public static string IP2Loc(string IP)
        {
            return wc.DownloadString("http://api.predator.wtf/geoip/?arguments=" + IP).Replace("<br>", Environment.NewLine);
        }
        public static bool PortChecker(string IP, string Port)
        {
            if (wc.DownloadString("http://api.predator.wtf/pcheck/?arguments=" + IP + "&port=" + Port).Contains("not open") == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
