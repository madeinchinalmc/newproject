using Model.Dto;
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

namespace WinFormSpider
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            class1 cc = new class1();
            cc.Name = "qweqwe qweq dsdsa";
        }
        private void fun1(string str)
        {
            //for(int i = 0; i < 5; i++)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        this.label1.Text = i.ToString();
            //    });
            //}



            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Action<string> act1 = t =>
            {
                Action<string> act = s =>
                {
                    this.label1.Text = s;
                };
                label1.Invoke(act, t);
            };
            //Task.Factory.StartNew(() =>
            //{
            //    for(int i = 0; i < 1000; i++)
            //    {
            //        act1(i.ToString());
            //        //fun1(i.ToString());
            //    }
            //});
            Task.Factory.StartNew(() =>
            {
                Parallel.For(0, 100, t =>
                {
                    Thread.Sleep(10);
                    act1(t.ToString());
                });
            });
            
            MessageBox.Show("OK");
        }
    }
    public class MyLengthAttribute : Attribute
    {
        private int DefindLength;
        public MyLengthAttribute(int _Length)
        {
            this.DefindLength = _Length;
            IsMaxLength();
        }
        public bool IsMaxLength()
        {
            if (this.DefindLength > 0 && this.DefindLength < 10)
            {
                return true;
            }
            else
            {
                throw (new Exception("xxxx"));
            }
        }
    }
    public class class1
    {
        [MyLength(10)]
        public string Name { get; set; }
    }
}
