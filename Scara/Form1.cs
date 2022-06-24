using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;


namespace Scara
{

    public partial class Form1 : Form
    {
        double x = 0;
        double y = 0;
    
        int a = 0;


        static int BoardSize = 300;
        static double l1 = 140f;
        static double l2 = 255f;

        static int SquareNumber = 7;
        double SquareSize = BoardSize / SquareNumber;

        public int row = 0, col = 0;
        public int SquareSelect = 0;
        public string row_str, col_str, z_str;
        char turn = 'o';

        double[] Ox = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] Oy = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        double[] Xx = new double[] { 4, 4, 4, 4 };
        double[] Xy = new double[] { 0, 0, 0, 0 };

        static int[,] Z3 = new int[3, 3] {
                            { -87, -75, -60 },
                            { -90, -83, -65 },
                            { -87, -75, -60}

        };
        static int[,] Z5 = new int[5, 5] {
                            { -87, -81, -75, -67 , -60},
                            { -88, -91, -83, -79 , -66},
                            { -90, -86, -83, -79, -68},
                            { -85, -83, -79, -70, -62},
                            { -87, -81, -75, -67, -60}
        };
        
        static int[,] Z7 = new int[7, 7] {
                            { -92, -87, -81, -75, -69, -64 , -59},
                            { -87, -82, -77, -72, -67, -62 , -57},
                            { -82, -77, -72, -67, -62, -57 , -52},
                            { -77, -72, -67, -62, -57, -52 , -47},
                            { -82, -77, -72, -67, -62, -57 , -52},
                            { -87, -82, -77, -72, -67, -62 , -57},
                            { -92, -87, -81, -75, -69, -64 , -59},
        };
        
        new int[,] Z = Z7;
        double ox = 0, oy = 0;

        double theta1 = 0, theta2 = 0;

        String GCoordinate = "";

        void CenterCoordinate()
        {
            row = SquareSelect / SquareNumber;
            col = SquareSelect % SquareNumber;

            ox = 100 + (col + 0.5f) * SquareSize;
            oy = (row - SquareNumber / 2) * SquareSize;
        }

        void MakeMove()
        {
            if (turn == 'x')
            {
                PlayX();
            }
            else
            {
                PlayO();
            }
        }

        void PlayO()
        {
            for (int i = 0; i < 8; i++)
            {
                x = ox + 5 * Math.Cos(i * Math.PI / 4);
                y = oy + 5 * Math.Sin(i * Math.PI / 4);

                if (row > SquareNumber * 1f / 2f)
                {
                    theta1 = 2 * Math.Atan((2 * y * l1 - Math.Sqrt((-Math.Pow(x, 4) - 2 * Math.Pow(x, 2) * Math.Pow(y, 2) + 2 * Math.Pow(x, 2) * Math.Pow(l1, 2) + 2 * Math.Pow(x, 2) * Math.Pow(l2, 2) - Math.Pow(y, 4) + 2 * Math.Pow(y, 2) * Math.Pow(l1, 2) + 2 * Math.Pow(y, 2) * Math.Pow(l2, 2) - Math.Pow(l1, 4) + 2 * Math.Pow(l1, 2) * Math.Pow(l2, 2) - Math.Pow(l2, 4)))) / (Math.Pow(x, 2) + 2 * x * l1 + Math.Pow(y, 2) + Math.Pow(l1, 2) - Math.Pow(l2, 2)));
                    theta2 = 2 * Math.Atan(Math.Sqrt(((-Math.Pow(x, 2) - Math.Pow(y, 2) + Math.Pow(l1, 2) + 2 * l1 * l2 + Math.Pow(l2, 2)) * (Math.Pow(x, 2) + Math.Pow(y, 2) - Math.Pow(l1, 2) + 2 * l1 * l2 - Math.Pow(l2, 2)))) / (Math.Pow(x, 2) + Math.Pow(y, 2) - Math.Pow(l1, 2) + 2 * l1 * l2 - Math.Pow(l2, 2)));
                }

                else
                {
                    theta1 = 2 * Math.Atan((2 * y * l1 + Math.Sqrt((-Math.Pow(x, 4) - 2 * Math.Pow(x, 2) * Math.Pow(y, 2) + 2 * Math.Pow(x, 2) * Math.Pow(l1, 2) + 2 * Math.Pow(x, 2) * Math.Pow(l2, 2) - Math.Pow(y, 4) + 2 * Math.Pow(y, 2) * Math.Pow(l1, 2) + 2 * Math.Pow(y, 2) * Math.Pow(l2, 2) - Math.Pow(l1, 4) + 2 * Math.Pow(l1, 2) * Math.Pow(l2, 2) - Math.Pow(l2, 4)))) / (Math.Pow(x, 2) + 2 * x * l1 + Math.Pow(y, 2) + Math.Pow(l1, 2) - Math.Pow(l2, 2)));
                    theta2 = -2 * Math.Atan(Math.Sqrt(((-Math.Pow(x, 2) - Math.Pow(y, 2) + Math.Pow(l1, 2) + 2 * l1 * l2 + Math.Pow(l2, 2)) * (Math.Pow(x, 2) + Math.Pow(y, 2) - Math.Pow(l1, 2) + 2 * l1 * l2 - Math.Pow(l2, 2)))) / (Math.Pow(x, 2) + Math.Pow(y, 2) - Math.Pow(l1, 2) + 2 * l1 * l2 - Math.Pow(l2, 2)));
                }

                Ox[i] = ((theta1 * 180 / Math.PI) * 8.8 / 90);
                Oy[i] = ((theta1 + theta2) * 180 / Math.PI) * 8.8 / 90;
            }

            GCoordinate = "X" + Ox[0].ToString("0.0") + ' ' + "Y" + Oy[0].ToString("0.0");
            COM.WriteLine(GCoordinate);
            COM.WriteLine("Z" + Z[row, col].ToString("0.0"));

            for (int i = 1; i < 8; i++)
            {
                GCoordinate = "X" + Ox[i].ToString("0.0") + ' ' + "Y" + Oy[i].ToString("0.0");
                COM.WriteLine(GCoordinate);
            }
            GCoordinate = "X" + Ox[0].ToString("0.0") + ' ' + "Y" + Oy[0].ToString("0.0");
            COM.WriteLine(GCoordinate);

            COM.WriteLine("Z-40");
            COM.WriteLine("X8.8 Y8.8");
        }

        void PlayX()
        {
            for (int i = 0; i < 4; i++)
            {
                x = ox + 5 * Math.Cos(i * Math.PI / 2);
                y = oy + 5 * Math.Sin(i * Math.PI / 2);

                if (row > SquareNumber * 1f / 2f)
                {
                    theta1 = 2 * Math.Atan((2 * y * l1 - Math.Sqrt((-Math.Pow(x, 4) - 2 * Math.Pow(x, 2) * Math.Pow(y, 2) + 2 * Math.Pow(x, 2) * Math.Pow(l1, 2) + 2 * Math.Pow(x, 2) * Math.Pow(l2, 2) - Math.Pow(y, 4) + 2 * Math.Pow(y, 2) * Math.Pow(l1, 2) + 2 * Math.Pow(y, 2) * Math.Pow(l2, 2) - Math.Pow(l1, 4) + 2 * Math.Pow(l1, 2) * Math.Pow(l2, 2) - Math.Pow(l2, 4)))) / (Math.Pow(x, 2) + 2 * x * l1 + Math.Pow(y, 2) + Math.Pow(l1, 2) - Math.Pow(l2, 2)));
                    theta2 = 2 * Math.Atan(Math.Sqrt(((-Math.Pow(x, 2) - Math.Pow(y, 2) + Math.Pow(l1, 2) + 2 * l1 * l2 + Math.Pow(l2, 2)) * (Math.Pow(x, 2) + Math.Pow(y, 2) - Math.Pow(l1, 2) + 2 * l1 * l2 - Math.Pow(l2, 2)))) / (Math.Pow(x, 2) + Math.Pow(y, 2) - Math.Pow(l1, 2) + 2 * l1 * l2 - Math.Pow(l2, 2)));
                }

                else
                {
                    theta1 = 2 * Math.Atan((2 * y * l1 + Math.Sqrt((-Math.Pow(x, 4) - 2 * Math.Pow(x, 2) * Math.Pow(y, 2) + 2 * Math.Pow(x, 2) * Math.Pow(l1, 2) + 2 * Math.Pow(x, 2) * Math.Pow(l2, 2) - Math.Pow(y, 4) + 2 * Math.Pow(y, 2) * Math.Pow(l1, 2) + 2 * Math.Pow(y, 2) * Math.Pow(l2, 2) - Math.Pow(l1, 4) + 2 * Math.Pow(l1, 2) * Math.Pow(l2, 2) - Math.Pow(l2, 4)))) / (Math.Pow(x, 2) + 2 * x * l1 + Math.Pow(y, 2) + Math.Pow(l1, 2) - Math.Pow(l2, 2)));
                    theta2 = -2 * Math.Atan(Math.Sqrt(((-Math.Pow(x, 2) - Math.Pow(y, 2) + Math.Pow(l1, 2) + 2 * l1 * l2 + Math.Pow(l2, 2)) * (Math.Pow(x, 2) + Math.Pow(y, 2) - Math.Pow(l1, 2) + 2 * l1 * l2 - Math.Pow(l2, 2)))) / (Math.Pow(x, 2) + Math.Pow(y, 2) - Math.Pow(l1, 2) + 2 * l1 * l2 - Math.Pow(l2, 2)));
                }

                Xx[i] = ((theta1 * 180 / Math.PI) * 8.8 / 90);
                Xy[i] = ((theta1 + theta2) * 180 / Math.PI) * 8.8 / 90;
            }

            GCoordinate = "X" + Xx[0].ToString("0.0") + ' ' + "Y" + Xy[0].ToString("0.0");
            COM.WriteLine(GCoordinate);
            COM.WriteLine("Z" + Z[row, col].ToString("0.0"));

            GCoordinate = "X" + Xx[2].ToString("0.0") + ' ' + "Y" + Xy[2].ToString("0.0");
            COM.WriteLine(GCoordinate);
            COM.WriteLine("Z" + (Z[row, col] + 20).ToString("0.0"));

            GCoordinate = "X" + Xx[1].ToString("0.0") + ' ' + "Y" + Xy[1].ToString("0.0");
            COM.WriteLine(GCoordinate);
            COM.WriteLine("Z" + Z[row, col].ToString("0.0"));

            GCoordinate = "X" + Xx[3].ToString("0.0") + ' ' + "Y" + Xy[3].ToString("0.0");
            COM.WriteLine(GCoordinate);
            COM.WriteLine("Z" + (Z[row, col] + 20).ToString("0.0"));

            COM.WriteLine("Z-40");
            COM.WriteLine("X8.8 Y8.8");
        }

        public void LetGo()
        {
            CenterCoordinate();
            MakeMove();
        }

        public Form1()
        {

            InitializeComponent();
        }

        int so_cong_com = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] port = SerialPort.GetPortNames();
            if (so_cong_com != port.Length)
            {
                so_cong_com = port.Length;
                cbb.Items.Clear();
                for (int j = 0; j < so_cong_com; j++) cbb.Items.Add(port[j]);
            }
        }




        private void Connect_Click(object sender, EventArgs e)
        {
            if (STT.Text == "Not Connected")
            {
                COM.PortName = cbb.Text;
                COM.Open();
                STT.Text = "Connected";
                Connect.Text = "Disconnected";
                COM.WriteLine("G91");
            }
            else
            {
                COM.Close();
                STT.Text = "Not Connected";
                Connect.Text = "Connect";
            }
        }










        private void button1_Click(object sender, EventArgs e)
        {
            COM.WriteLine("X8.8 Y8.8 Z-40");
        }





        private void button4_Click(object sender, EventArgs e)
        {
            SquareSelect = 0;
            LetGo();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SquareSelect = 1;
            LetGo();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SquareSelect = 2;
            LetGo();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SquareSelect = 3;

            CenterCoordinate();
            ox = ox + 10;
            MakeMove();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SquareSelect = 4;
            LetGo();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SquareSelect = 7;
            LetGo();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SquareSelect = 5;
            LetGo();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            SquareSelect = 6;
            LetGo();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            row = Int32.Parse(row_str) - 1;
            col = Int32.Parse(col_str) - 1;


            ox = 100 + (col + 0.5f) * SquareSize;
            oy = (row - SquareNumber / 2) * SquareSize;
            /*
            if (col == 5)
            {
                ox = ox + 10;
            }    
            */
            oy = oy - 5;
            MakeMove();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            row_str = textBox1.Text;

        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            col_str = textBox2.Text;


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void STT_Click(object sender, EventArgs e)
        {

        }

        private void cbb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            string Message = "Z" + z_str;
            COM.WriteLine(Message);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            z_str = textBox3.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            SquareSelect = 8;
            LetGo();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            turn = 'x';
        }

        private void button17_Click(object sender, EventArgs e)
        {
            turn = 'o';
        }
    }
}

