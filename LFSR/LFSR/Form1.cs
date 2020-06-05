using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace LFSR
{
   
    public partial class Form1 : Form
    {
        public FileStream FileIn;
        public FileStream FileEx;
        public int by = 0;
        public string s1;
        public string s2;
        public string s3;
        public int outByte;
        public long id =0;
        public int counter = 0;
        public Boolean flag = false;
        public Form1()
        {
            InitializeComponent();
        }

        public string from10to2(int b)
        {
            string s="";
            int ko=128;
            for (int i = 0; i < 7; i++)
            {
                if ((b & ko) == 0) s += "0";
                else s += "1";
                ko = ko / 2;
            }
            s += " ";
            return s;
        }

        public int from2to10(string s)
        {
            int n = 0;
            int p = 1;
            for(int i = 1; i <= s.Length; i++)
            {
                n += ((int)Char.GetNumericValue(s[s.Length - i])) * p;
                p *= 2;
            }
            return n;
        }

        public int getNextByte()
        {
            by = FileIn.ReadByte();
            if (by != -1)
            {
                
                s2 += from10to2(by);
                
                if ((counter % 32767 == 0)&&(counter!=0))
                {
                    id++;
                    flag = true;
                }
                counter++;
            }
            return by;
        }

        public void workWithByte(Int64 key)
        {
            long b = key >> 25;
            b = b & 255;
            int a = getNextByte();
            if (a != -1)
            {
                s1 += from10to2((byte)b);
                outByte = a ^ (int)b;
                s3 += from10to2(outByte);
                FileEx.WriteByte((byte)outByte);

                if (flag)
                {
                    key = id * 32768;
                    flag = false;
                    s2 += "\n\n\n\n";
                    s3 += "\n\n\n\n";

                }
            }
        }

        public void cipher(string s)
        {
            int f1, f2;
            Int64 a = 1 << 2;
            Int64 b = 1 << 24;
            Int64 key = from2to10(s);
            while (by != -1)
            {
                for(int i = 0; i < 8; i++)
                {
                    if ((a & key) == 0) f1 = 0; else f1 = 1;
                    if ((b & key) == 0) f2 = 0; else f2 = 1;
                    key = key << 1;
                    key += f1 ^ f2;
                }
                workWithByte(key);
            }

        }


            

        private void button1_Click(object sender, EventArgs e) {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = openFile.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = "->";
            FileIn = new FileStream(richTextBox1.Text, FileMode.Open, FileAccess.Read);
            FileEx = new FileStream(richTextBox1.Text.Replace(Path.GetExtension(richTextBox1.Text), "(ciphered)"+ Path.GetExtension(richTextBox1.Text)), FileMode.OpenOrCreate, FileAccess.Write);
            s1 = "";
            s2 = "";
            s3 = "";
            by = 0;
            id = 0;
            counter = 0;
            flag = false;
            cipher("0000000000000000000000001");
            richTextBox2.Text =s2;
            richTextBox3.Text =s3;
           // richTextBox4.Text = s1;
            FileIn.Close();
            FileEx.Close();
            }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = "<-";
            FileIn = new FileStream(richTextBox1.Text, FileMode.Open, FileAccess.Read);
            FileEx = new FileStream(richTextBox1.Text.Replace(Path.GetExtension(richTextBox1.Text), "(deciphered)" + Path.GetExtension(richTextBox1.Text)), FileMode.OpenOrCreate, FileAccess.Write);
            s1 = "";
            s2 = "";
            s3 = "";
            by = 0;
            id = 0;
            counter = 0;
            flag = false;
            cipher("0000000000000000000000001");
            richTextBox2.Text = s2;
            richTextBox3.Text = s3;
           // richTextBox4.Text = s1;
            FileIn.Close();
            FileEx.Close();
        }
    }
}
