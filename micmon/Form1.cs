using NAudio.Wave;
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

namespace micmon
{

    public partial class Form1 : Form
    {
        private static double audioValueMax = 1;
        private static double audioValueLast = 0;
        private static int audioCount = 0;
        private readonly static int RATE = 44100;
        private readonly static int BUFFER_SAMPLES = 1024;
        private readonly double fracLevel = .1; //% of stored maximun level
        private readonly double lowOpacity = .1;
        private readonly int delayTime = 1000; //time at 100% Opacity in ms

        public Form1()
        {
            InitializeComponent();
            this.Opacity = lowOpacity;
            var waveIn = new WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new NAudio.Wave.WaveFormat(RATE, 1);
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.BufferMilliseconds = (int)((double)BUFFER_SAMPLES / (double)RATE * 1000.0);
            waveIn.StartRecording();
        }

        private void OnDataAvailable(object sender, WaveInEventArgs args)
        {

            float max = 0;

            // interpret as 16 bit audio
            for (int index = 0; index < args.BytesRecorded; index += 2)
            {
                short sample = (short)((args.Buffer[index + 1] << 8) |
                                        args.Buffer[index + 0]);
                var sample32 = sample / 32768f; // to floating point
                if (sample32 < 0) sample32 = -sample32; // absolute value 
                if (sample32 > max) max = sample32; // is this the max value?
            }

            // calculate what fraction this peak is of previous peaks
            if (max > audioValueMax)
            {
                audioValueMax = (double)max;
            }
            audioValueLast = max;
            audioCount += 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.IndianRed;
            this.TransparencyKey = Color.IndianRed;
            this.Location = new Point(10, 20);
            this.Opacity = lowOpacity;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double frac = audioValueLast / audioValueMax;
            if (frac < fracLevel)
            {

                this.Opacity = lowOpacity;
            }
            else
            {
                this.Opacity = 1;
                int milliseconds = delayTime;
                Thread.Sleep(milliseconds);
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
