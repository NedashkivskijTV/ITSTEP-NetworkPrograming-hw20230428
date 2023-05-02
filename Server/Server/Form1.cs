using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public partial class Form1 : Form
    {

        Thread thread;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnServerStart_Click(object sender, EventArgs e)
        {
            if (thread != null)
            {
                return;
            }
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            IPEndPoint endPoint = new IPEndPoint(Dns.GetHostAddresses(Dns.GetHostName())[2], 11000);
            socket.Bind(endPoint);
            thread = new Thread(SendRandomStringToClient);
            thread.IsBackground = true;
            thread.Start(socket);
            Text = "Server was started !";
            tbServerStatistics.Text = "Server was started !\r\n";
        }

        private void SendRandomStringToClient(object? obj)
        {
            Socket socket_ReceiveAndSend = obj as Socket;
            byte[] buffer = new byte[1024];
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 11000);
            try
            {
                do
                {
                    // -------------------------------------------------------------- ��������� ����� - ����� -----------------------------------
                    int len = socket_ReceiveAndSend.ReceiveFrom(buffer, ref endPoint);
                    StringBuilder sb = new StringBuilder(tbServerStatistics.Text);
                    sb.AppendLine($"{len} bytes received from {endPoint} et {DateTime.Now.ToLongTimeString()}"); 
                    sb.AppendLine(Encoding.Default.GetString(buffer, 0, len)); 
                    tbServerStatistics.BeginInvoke(new Action<string>(AddText), sb.ToString());

                    // -------------------------------------------------------------- ²������� ����� -------------------------------------------
                    //string strRandom = "random text";
                    //string strRandom = RandonText();
                    len =  socket_ReceiveAndSend.SendTo(Encoding.Default.GetBytes(RandonText()), endPoint);
                    sb.AppendLine($"{len} bytes sended to {endPoint} et {DateTime.Now.ToLongTimeString()}");
                    tbServerStatistics.BeginInvoke(new Action<string>(AddText), sb.ToString());

                    if (!socket_ReceiveAndSend.Connected)
                    {
                        sb.AppendLine($"Client {endPoint} disconnected et {DateTime.Now.ToLongTimeString()}\r\n");
                        tbServerStatistics.BeginInvoke(new Action<string>(AddText), sb.ToString());
                    }

                } while (true);

            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string RandonText()
        {
            Random random = new Random();
            string[] strings = new string[] {
                "A bove maiore discit arare minor � ������ ��������� �� ������ ���������",
                "Barba crescit, caput nescit � ������ �����, ������ �������",
                "Contra spem spero � ��� ��䳿 ���������",
                "Dura lex sed lex � ����� �������, ��� �� �����",
                "Edimus, ut vivamus, non vivimus, ut edamus � ���, ��� ����, � �� ������, ��� ����",
                "Felix, qui potuit rerum cognoscere causas � �������� ���, ��� ��� ������ ������ �����",
                "Homo homini lupus est � ������ ����� ����",
                "Imperare sibi, maximum imperium est � �������� ��� ����� � �������� � �������",
                "Lupus non mordet lupum � ���� �� ���� �����",
                "Mendaci homini verum quidem dicenti credere non solemus � �������� ����� �� �� �����, ����� ���� ���� �������� ������",
                "Non progredi est regredi � �� ��� ������, ������ ��� �����",
                "Omnia mea mecum porto � ��� ��� ���� � �����",
                "Primus inter pares � ������ ����� �����",
                "Quod licet Iovi non licet bovi � ��, �� ������ ������, �� ������ �����",
                "Repetitio est mater studiorum � ���������� � ���� ��������",
            };  
            int n = random.Next(strings.Length);
            return strings[n];
        }

        private void AddText(string str)
        {
            tbServerStatistics.Text = str;
        }
    }
}