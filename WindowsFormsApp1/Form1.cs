using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool chartdisplay = false;
        
        public Form1()
        {
            InitializeComponent();
            My_init();
        }
        public void My_init()
        {
            /*
            comboBox1.Items.Add("COM1");
            comboBox1.Items.Add("COM2");
            comboBox1.Items.Add("COM3");
            comboBox1.Items.Add("COM4");
            */
            //string[] comname = new string[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10" };
            string[] freq = new string[] { "1200", "2400", "4800", "9600", "14400", "19200", "38400"
                , "43000", "57600", "76800", "115200", "128000", "230400","256000"};
            string[] databit = new string[] { "8", "7", "6", "5" };
            string[] checkbit = new string[] { "无", "奇校验", "偶校验" };
            string[] stopbit = new string[] { "1", "1.5", "2" };
            //comboBox1.DataSource = comname;
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            comboBox1.SelectedIndex = 0;
            comboBox2.DataSource = freq;
            comboBox3.DataSource = databit;
            comboBox4.DataSource = checkbit;
            comboBox5.DataSource = stopbit;
            button2.Enabled = false;
            button5.Enabled = false;
            button7.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)//打开串口
        {
            serialPort1.PortName = comboBox1.SelectedItem.ToString();
            switch (comboBox2.SelectedItem.ToString())//波特率
            {
                case "1200": serialPort1.BaudRate = 1200; break;
                case "2400": serialPort1.BaudRate = 2400; break;
                case "4800": serialPort1.BaudRate = 4800; break;
                case "9600": serialPort1.BaudRate = 9600; break;
                case "14400": serialPort1.BaudRate = 14400; break;
                case "19200": serialPort1.BaudRate = 19200; break;
                case "38400": serialPort1.BaudRate = 38400;break;
                case "57600": serialPort1.BaudRate = 43000;break;
                case "76800": serialPort1.BaudRate = 76800; break;
                case "115200": serialPort1.BaudRate = 115200; break;
                case "128000": serialPort1.BaudRate = 128000; break;
                case "230400": serialPort1.BaudRate = 230400; break;
                case "256000": serialPort1.BaudRate = 256000; break;
            }
            switch (comboBox3.SelectedItem.ToString())//数据位
            {
                case "5": serialPort1.DataBits = 5; break;
                case "6": serialPort1.DataBits = 6; break;
                case "7": serialPort1.DataBits = 7; break;
                case "8": serialPort1.DataBits = 8; break;
            }
            switch(comboBox4.SelectedItem.ToString())//校验位
            {
                case "无": serialPort1.Parity = Parity.None; break;
                case "奇校验": serialPort1.Parity = Parity.Odd; break;
                case "偶校验": serialPort1.Parity = Parity.Even; break;
            }
            switch (comboBox5.SelectedItem.ToString())//停止位
            {
                case "1": serialPort1.StopBits=StopBits.One; break;
                case "1.5": serialPort1.StopBits = StopBits.OnePointFive; break;
                case "2": serialPort1.StopBits = StopBits.Two; break;
            }
            serialPort1.Handshake = Handshake.None;//握手协议
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            serialPort1.Open();

            button1.Enabled = false;
            button2.Enabled = true;
            button7.Enabled = true;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
        }
        private void button2_Click(object sender, EventArgs e)//关闭串口
        {
            serialPort1.DataReceived-= new SerialDataReceivedEventHandler(DataReceivedHandler);//删除数据接收处理
            serialPort1.Close();
            button1.Enabled = true;
            button2.Enabled = false;
            button7.Enabled = false;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;
        }
        private void button3_Click(object sender, EventArgs e)//清空接收
        {
            textBox1.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)//开始显示图表
        {
            chartdisplay = true;
            button4.Enabled = false;
            button5.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)//停止显示图表
        {
            chartdisplay = false;
            button4.Enabled = true;
            button5.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)//初始化清空图表
        {
            chart1.Series[0].Points.Clear();
        }

        //public event System.IO.Ports.SerialDataReceivedEventHandler DataReceived;
        private void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)//串口数据接收并打印在文本框
        {
                SerialPort sp = (SerialPort)sender;
                int indata = sp.ReadByte();
                string strHex = indata.ToString("x2");
                Action action = () =>
                {
                    if(checkBox1.Checked)//16进制显示
                        textBox1.Text = textBox1.Text + strHex.ToUpper() + " ";
                    else//10进制显示
                        textBox1.Text = textBox1.Text + indata + " ";
                    if(chartdisplay)
                    {
                        if (chart1.Series[0].Points.Count == 32)//图表已写满
                            chart1.Series[0].Points.Clear();
                        int xnum = chart1.Series[0].Points.Count + 1;
                        chart1.Series[0].Points.AddXY(xnum,indata);
                    }
                };
                Invoke(action);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string data = textBox2.Text;
            if (checkBox2.Checked)//16进制发送
            {
                if (data.Length != 2)
                {
                    MessageBox.Show("请输入0~9，a~f，A~F之间的两位合法16进制数");
                    return;
                }
                for (int i = 0; i < data.Length; i++)
                {
                    if ((data[i] >= '0' && data[i] <= '9') || (data[i] >= 'a' && data[i] <= 'f') || (data[i] >= 'A' && data[i] <= 'F'))
                    { }
                    else
                    {
                        MessageBox.Show("请输入0~9，a~f，A~F之间的两位合法16进制数");
                        return;
                    }
                }
                byte[] databyte = new byte[2];
                databyte[0] = Convert.ToByte(data, 16);
                serialPort1.Write(databyte, 0, 1);
            }
            else//10进制发送
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] < '0' || data[i] > '9')
                    {
                        MessageBox.Show("请输入0~256之间的合法十进制数");
                        return;
                    }
                }
                int temp = Convert.ToInt32(data, 10);
                if (temp < 0 || temp > 255)
                {
                    MessageBox.Show("请输入0~256之间的合法十进制数");
                    return;
                }
                byte[] databyte = new byte[2];
                databyte[0] = Convert.ToByte(data, 10);
                serialPort1.Write(databyte, 0, 1);
            }
        }
    }
}//串口https://blog.csdn.net/cy757/article/details/4474930
 //串口微软官方文档https://docs.microsoft.com/zh-cn/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-3.1
 //checkbox控件https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.forms.checkbox?view=netframework-4.7.1
 //chart控件https://blog.csdn.net/qq_27825451/article/details/81305387
 //https://www.cnblogs.com/mazhenyu/p/9498439.html
 //串口关闭错误https://www.codeproject.com/questions/873337/when-i-close-serial-comport-my-csharp-form-is-hang