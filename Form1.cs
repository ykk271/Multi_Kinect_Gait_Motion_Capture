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
using Accord.Video.FFMPEG; //Vidoe save

using System.Diagnostics;

namespace Dual_Kinect
{
    public partial class Form1 : Form
    {
    
        int installCameraNumber;
        Device[] kinect;
        Tracker[] bodyTracker;
        Calibration[] calib;
        Skeleton[] skl;
        bool[] detected;
        Thread[] trackingThread;
        Thread[] RGBThread;
        Frame[] frame;
        Image[] colorImage;
        Bitmap[] colorBitmap;
        double cameraTime = 0;
        VideoFileWriter[] vfw;

        string[] dataFullName;
        string[] videoFullName;

        bool Loop = true;
        
        public Form1()
        {
            InitializeComponent();

            installCameraNumber = Device.GetInstalledCount();

            if (installCameraNumber < 1)
            {
                MessageBox.Show("연결 된 Kinect가 없습니다.", "Error");
                Application.ExitThread();
                Environment.Exit(0);
            }

            //installCameraNumber = 1;

            //MessageBox.Show(installCameraNumber.ToString(), "Error");

            kinect          = new Device[installCameraNumber]; ;
            bodyTracker     = new Tracker[installCameraNumber];
            calib           = new Calibration[installCameraNumber];
            skl             = new Skeleton[installCameraNumber];
            
            detected        = new bool[installCameraNumber];
            frame           = new Frame[installCameraNumber];
            trackingThread  = new Thread[installCameraNumber];
            RGBThread       = new Thread[installCameraNumber];
            colorImage      = new Image[installCameraNumber];
            colorBitmap     = new Bitmap[installCameraNumber];
            vfw             = new VideoFileWriter[installCameraNumber];

            videoFullName   = new string[installCameraNumber];
            dataFullName    = new string[installCameraNumber];


            for (int i = 0; i < installCameraNumber; ++i)
            {

                kinect[i] = Device.Open(i);

                kinect[i].StartCameras(new DeviceConfiguration
                {
                    ColorFormat = ImageFormat.ColorBGRA32,
                    ColorResolution = ColorResolution.R1536p,
                    DepthMode = DepthMode.NFOV_Unbinned,
                    SynchronizedImagesOnly = true,
                    CameraFPS = FPS.FPS30,
                });


                calib[i] = kinect[i].GetCalibration();
                bodyTracker[i] = Tracker.Create(calib[i], new TrackerConfiguration
                {
                    ProcessingMode = TrackerProcessingMode.Gpu,
                    SensorOrientation = SensorOrientation.Default,
                    GpuDeviceId = 1
                });


                trackingThread[i] = new Thread(() => kinectCapture(i));
                trackingThread[i].Start();

                RGBThread[i] = new Thread(() => kinectRGB(i));
                RGBThread[i].Start();


                detected[i] = false;

                Thread.Sleep(200);                
            }

            Task.Run(() => kinectSave(installCameraNumber));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Loop = false;

            for (int i = 0; i < installCameraNumber; ++i)
            {
                kinect[i].StopCameras();
                trackingThread[i].Abort();
                RGBThread[i].Abort();

            }
        }

        private void kinectRGB(int i)
        {
            while (Loop)
            {
                RGB(i);
            }
        }


        private async Task kinectSave(int N)
        {
            double tmp = 0;
            await Task.Run(() =>
            {
                while (Loop)
                {
                    if (tmp != cameraTime)
                    {
                        for (int i = 0; i < N; ++i)
                        {

                            if (Record)
                            {
                                dataSave(i);

                            }

                        }
                        tmp = cameraTime;
                    }
                }                       
               
            });
        }

        private void kinectCapture(int i)
        {      
            while (Loop)
            {          
              

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        label6.Text = cameraTime.ToString();

                        if (i == 1)
                            if (Record)
                            {
                                if (vedioRecord) vfw[i].WriteVideoFrame(colorBitmap[i]);
                            }
                    }));


                }
                else
                {
                    label6.Text = cameraTime.ToString();

                }

                bodyTraking(i);

            }
        }


        private void bodyTraking(int i)
        {
            bodyTracker[i].EnqueueCapture(kinect[i].GetCapture());

            frame[i] = bodyTracker[i].PopResult();

            if (frame != null)
            {
                if (i == 0)
                {
                    cameraTime = frame[0].DeviceTimestamp.TotalSeconds;
                }


                uint bodyNum = frame[i].NumberOfBodies;
                if (bodyNum > 0)
                {
                    detected[i] = true;
                    skl[i] = frame[i].GetBodySkeleton(0);
                }
                else detected[i] = false;
            }
            frame[i].Dispose();



        }


        private void RGB(int i)
        {
            CreateColorImage(kinect[i].GetCapture(), i);
            
            switch (i)
            {
                case 0:
                    pictureBox1.Image = colorBitmap[i];
                    break;
                case 1:
                    pictureBox2.Image = colorBitmap[i];
                    break;
                case 2:
                    pictureBox3.Image = colorBitmap[i];
                    break;
                case 3:
                    pictureBox4.Image = colorBitmap[i];
                    break;
            }        

        }

        private void CreateColorImage(Capture capture, int i)
        {
            unsafe
            {
                colorImage[i] = capture.Color;

               using (MemoryHandle pin = colorImage[i].Memory.Pin())
               {
                colorBitmap[i] = new Bitmap(
                colorImage[i].WidthPixels,
                colorImage[i].HeightPixels,
                colorImage[i].StrideBytes,
                PixelFormat.Format32bppArgb,
                (IntPtr)pin.Pointer);
               }   
            }
        }





    }
}
