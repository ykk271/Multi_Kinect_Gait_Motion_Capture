using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Azure.Kinect.Sensor;
using Microsoft.Azure.Kinect.BodyTracking;
using System.IO; //file IO
using System.Threading; // multi threading
using Image = Microsoft.Azure.Kinect.Sensor.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using System.Buffers;


namespace Dual_Kinect
{
    public partial class Form1 : Form
    {



        private void DrawLine(Graphics g, float[] X, float[] Y, int index1, int index2)
        {
            Pen P = new Pen(Color.FromArgb(100, 0, 100, 200), 25);

            if (X[index1] != 0 && X[index2] != 0)
            {
                g.DrawLine(P, X[index1], Y[index1], X[index2], Y[index2]);
            }
        }


        private void DrawHuman(Graphics g, float[] X, float[] Y)
        {
            DrawLine(g, X, Y, 0, 1);
            DrawLine(g, X, Y, 1, 2);
            DrawLine(g, X, Y, 2, 3);
            DrawLine(g, X, Y, 3, 26);
            //Left Arm
            DrawLine(g, X, Y, 2, 4);
            DrawLine(g, X, Y, 4, 5);
            DrawLine(g, X, Y, 5, 6);
            DrawLine(g, X, Y, 6, 7);
            //Left Leg
            DrawLine(g, X, Y, 0, 18);
            DrawLine(g, X, Y, 18, 19);
            DrawLine(g, X, Y, 19, 20);
            DrawLine(g, X, Y, 20, 21);
            //Right Arm
            DrawLine(g, X, Y, 2, 11);
            DrawLine(g, X, Y, 11, 12);
            DrawLine(g, X, Y, 12, 13);
            DrawLine(g, X, Y, 13, 14);
            //Right Leg
            DrawLine(g, X, Y, 0, 22);
            DrawLine(g, X, Y, 22, 23);
            DrawLine(g, X, Y, 23, 24);
            DrawLine(g, X, Y, 24, 25);
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int N = 0;

            if (!detected[N])
                return;

            float[] X = new float[31];
            float[] Y = new float[31];

            Graphics g = e.Graphics;
            g.FillEllipse(Brushes.Red, 30, 30, 40, 40);

            for (int i = 0; i < 31; ++i)
            {
                var point = calib[N].TransformTo2D(skl[N].
                GetJoint(i).Position, CalibrationDeviceType.Depth, CalibrationDeviceType.Color);

                if (point != null)
                {
                    X[i] = point.Value.X / 2000 * 600;
                    Y[i] = point.Value.Y / 1500 * 450;


                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 ||
                        i == 7 || i == 11 || i == 12 || i == 13 || i == 14 || i == 18 || i == 19 ||
                        i == 19 || i == 20 || i == 21 || i == 22 || i == 23 || i == 24 || i == 25)
                    {

                        if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.Low)
                        {
                            g.FillEllipse(Brushes.Red, X[i], Y[i], 20, 20);
                        }
                        else if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.Medium)
                        {
                            g.FillEllipse(Brushes.Green, X[i], Y[i], 20, 20);
                        }
                        else if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.High)
                        {
                            g.FillEllipse(Brushes.Blue, X[i], Y[i], 20, 20);
                        }
                    }
                }
            }

            DrawHuman(g, X, Y);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            int N = 1;

            if (installCameraNumber < N + 1)
                return;

            if (!detected[N])
                return;

            float[] X = new float[31];
            float[] Y = new float[31];

            Graphics g = e.Graphics;
            g.FillEllipse(Brushes.Red, 30, 30, 40, 40);

            for (int i = 0; i < 31; ++i)
            {
                var point = calib[N].TransformTo2D(skl[N].
                GetJoint(i).Position, CalibrationDeviceType.Depth, CalibrationDeviceType.Color);

                if (point != null)
                {
                    X[i] = point.Value.X / 2000 * 600;
                    Y[i] = point.Value.Y / 1500 * 450;


                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 ||
                        i == 7 || i == 11 || i == 12 || i == 13 || i == 14 || i == 18 || i == 19 ||
                        i == 19 || i == 20 || i == 21 || i == 22 || i == 23 || i == 24 || i == 25)
                    {

                        if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.Low)
                        {
                            g.FillEllipse(Brushes.Red, X[i], Y[i], 20, 20);
                        }
                        else if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.Medium)
                        {
                            g.FillEllipse(Brushes.Green, X[i], Y[i], 20, 20);
                        }
                        else if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.High)
                        {
                            g.FillEllipse(Brushes.Blue, X[i], Y[i], 20, 20);
                        }
                    }
                }
            }

            DrawHuman(g, X, Y);
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            int N = 2;

            if (installCameraNumber < N + 1)
                return;


            if (!detected[N])
                return;

            float[] X = new float[31];
            float[] Y = new float[31];

            Graphics g = e.Graphics;
            g.FillEllipse(Brushes.Red, 30, 30, 40, 40);

            for (int i = 0; i < 31; ++i)
            {
                var point = calib[N].TransformTo2D(skl[N].
                GetJoint(i).Position, CalibrationDeviceType.Depth, CalibrationDeviceType.Color);

                if (point != null)
                {
                    X[i] = point.Value.X / 2000 * 600;
                    Y[i] = point.Value.Y / 1500 * 450;


                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 ||
                        i == 7 || i == 11 || i == 12 || i == 13 || i == 14 || i == 18 || i == 19 ||
                        i == 19 || i == 20 || i == 21 || i == 22 || i == 23 || i == 24 || i == 25)
                    {

                        if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.Low)
                        {
                            g.FillEllipse(Brushes.Red, X[i], Y[i], 20, 20);
                        }
                        else if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.Medium)
                        {
                            g.FillEllipse(Brushes.Green, X[i], Y[i], 20, 20);
                        }
                        else if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.High)
                        {
                            g.FillEllipse(Brushes.Blue, X[i], Y[i], 20, 20);
                        }
                    }
                }
            }

            DrawHuman(g, X, Y);
        }


        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            int N = 3;

            if (installCameraNumber < N + 1)
                return;


            if (!detected[N])
                return;

            float[] X = new float[31];
            float[] Y = new float[31];

            Graphics g = e.Graphics;
            g.FillEllipse(Brushes.Red, 30, 30, 40, 40);

            for (int i = 0; i < 31; ++i)
            {
                var point = calib[N].TransformTo2D(skl[N].
                GetJoint(i).Position, CalibrationDeviceType.Depth, CalibrationDeviceType.Color);

                if (point != null)
                {
                    X[i] = point.Value.X / 2000 * 600;
                    Y[i] = point.Value.Y / 1500 * 450;


                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 ||
                        i == 7 || i == 11 || i == 12 || i == 13 || i == 14 || i == 18 || i == 19 ||
                        i == 19 || i == 20 || i == 21 || i == 22 || i == 23 || i == 24 || i == 25)
                    {

                        if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.Low)
                        {
                            g.FillEllipse(Brushes.Red, X[i], Y[i], 20, 20);
                        }
                        else if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.Medium)
                        {
                            g.FillEllipse(Brushes.Green, X[i], Y[i], 20, 20);
                        }
                        else if (skl[N].GetJoint(i).ConfidenceLevel == JointConfidenceLevel.High)
                        {
                            g.FillEllipse(Brushes.Blue, X[i], Y[i], 20, 20);
                        }
                    }
                }
            }

            DrawHuman(g, X, Y);
        }





    }
}
