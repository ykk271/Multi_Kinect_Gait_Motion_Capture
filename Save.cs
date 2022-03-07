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



namespace Dual_Kinect
{
    public partial class Form1 : Form
    {
        bool Record = false;
        bool vedioRecord = false;
        int dataNum = 1;

        string dataName;
        string dataPath;


        private void Form1_Load(object sender, EventArgs e)
        {
            //dataPath = Application.StartupPath;
            dataPath = "C:" + "\\" + "Users" + "\\" + "ykk27" + "\\" + "OneDrive" + "\\" + "바탕 화면";
            label3.Text = dataPath;
            label5.Text = dataNum.ToString();
            dataName = "Data";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataName = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                if (Record == false)
                {
                    while (true)
                    {
                        fileName();
                        FileInfo fileInfo = new FileInfo(dataFullName[0]);
                        if (fileInfo.Exists)
                        {
                            dataNum++;
                        }
                        else
                            break;
                    }

                    label5.Text = dataNum.ToString();

                    for (int i = 0; i < installCameraNumber; ++i)
                    {
                        dataGen(i);
                    }

                    button1.Image = Properties.Resources.Stop;

                    if (vedioRecord)
                    {
                        for (int i = 0; i < installCameraNumber; ++i)
                        {
                            vfw[i] = new VideoFileWriter();
                            vfw[i].Open(videoFullName[i], colorBitmap[i].Width, colorBitmap[i].Height, 10, VideoCodec.MPEG4, 1572864);
                        }
                    }

                    //Thread.Sleep(100);

                    Record = true;
                }
                else if (Record == true)
                {
                    Record = false;

                    button1.Image = Properties.Resources.Start;

                    if (vedioRecord)
                    {
                     for (int i = 0; i < installCameraNumber; ++i)
                         vfw[i].Close();
                    }
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                label3.Text = folderBrowserDialog.SelectedPath.ToString();
                dataPath = folderBrowserDialog.SelectedPath.ToString();
                dataNum = 1;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!vedioRecord) vedioRecord = true;
            else vedioRecord = false;
        }

        private int cofig2Int(int camNum, int jointNum)
        {
            if (!detected[camNum])
                return 0;
            else if (skl[camNum].GetJoint(jointNum).ConfidenceLevel == JointConfidenceLevel.High)
                return 3;
            else if (skl[camNum].GetJoint(jointNum).ConfidenceLevel == JointConfidenceLevel.Medium)
                return 2;
            else if (skl[camNum].GetJoint(jointNum).ConfidenceLevel == JointConfidenceLevel.Low)
                return 1;
            else if (skl[camNum].GetJoint(jointNum).ConfidenceLevel == JointConfidenceLevel.None)
                return 0;
            else
                return 99;
        }

        private void dataSave(int i)
        {
            fileName();           

            StreamWriter sw = File.AppendText(dataFullName[i]);

            if (sw != null)
            {
                sw.WriteLine(
                $"{cameraTime}" +
                $",{skl[i].GetJoint(0).Position.X},{skl[i].GetJoint(0).Position.Y},{skl[i].GetJoint(0).Position.Z}" +
                $",{cofig2Int(0, 0)}" +
                $",{skl[i].GetJoint(1).Position.X},{skl[i].GetJoint(1).Position.Y},{skl[i].GetJoint(1).Position.Z}" +
                $",{cofig2Int(0, 1)}" +
                $",{skl[i].GetJoint(2).Position.X},{skl[i].GetJoint(2).Position.Y},{skl[i].GetJoint(2).Position.Z}" +
                $",{cofig2Int(0, 2)}" +
                $",{skl[i].GetJoint(3).Position.X},{skl[i].GetJoint(3).Position.Y},{skl[i].GetJoint(3).Position.Z}" +
                $",{cofig2Int(0, 3)}" +
                $",{skl[i].GetJoint(4).Position.X},{skl[i].GetJoint(4).Position.Y},{skl[i].GetJoint(4).Position.Z}" +
                $",{cofig2Int(0, 4)}" +
                $",{skl[i].GetJoint(5).Position.X},{skl[i].GetJoint(5).Position.Y},{skl[i].GetJoint(5).Position.Z}" +
                $",{cofig2Int(0, 5)}" +
                $",{skl[i].GetJoint(6).Position.X},{skl[i].GetJoint(6).Position.Y},{skl[i].GetJoint(6).Position.Z}" +
                $",{cofig2Int(0, 6)}" +
                $",{skl[i].GetJoint(7).Position.X},{skl[i].GetJoint(7).Position.Y},{skl[i].GetJoint(7).Position.Z}" +
                $",{cofig2Int(0, 7)}" +
                $",{skl[i].GetJoint(8).Position.X},{skl[i].GetJoint(8).Position.Y},{skl[i].GetJoint(8).Position.Z}" +
                $",{cofig2Int(0, 8)}" +
                $",{skl[i].GetJoint(9).Position.X},{skl[i].GetJoint(9).Position.Y},{skl[i].GetJoint(9).Position.Z}" +
                $",{cofig2Int(0, 9)}" +
                $",{skl[i].GetJoint(10).Position.X},{skl[i].GetJoint(10).Position.Y},{skl[i].GetJoint(10).Position.Z}" +
                $",{cofig2Int(0, 10)}" +
                $",{skl[i].GetJoint(11).Position.X},{skl[i].GetJoint(11).Position.Y},{skl[i].GetJoint(11).Position.Z}" +
                $",{cofig2Int(0, 11)}" +
                $",{skl[i].GetJoint(12).Position.X},{skl[i].GetJoint(12).Position.Y},{skl[i].GetJoint(12).Position.Z}" +
                $",{cofig2Int(0, 12)}" +
                $",{skl[i].GetJoint(13).Position.X},{skl[i].GetJoint(13).Position.Y},{skl[i].GetJoint(13).Position.Z}" +
                $",{cofig2Int(0, 13)}" +
                $",{skl[i].GetJoint(14).Position.X},{skl[i].GetJoint(14).Position.Y},{skl[i].GetJoint(14).Position.Z}" +
                $",{cofig2Int(0, 14)}" +
                $",{skl[i].GetJoint(15).Position.X},{skl[i].GetJoint(15).Position.Y},{skl[i].GetJoint(15).Position.Z}" +
                $",{cofig2Int(0, 15)}" +
                $",{skl[i].GetJoint(16).Position.X},{skl[i].GetJoint(16).Position.Y},{skl[i].GetJoint(16).Position.Z}" +
                $",{cofig2Int(0, 16)}" +
                $",{skl[i].GetJoint(17).Position.X},{skl[i].GetJoint(17).Position.Y},{skl[i].GetJoint(17).Position.Z}" +
                $",{cofig2Int(0, 17)}" +
                $",{skl[i].GetJoint(18).Position.X},{skl[i].GetJoint(18).Position.Y},{skl[i].GetJoint(18).Position.Z}" +
                $",{cofig2Int(0, 18)}" +
                $",{skl[i].GetJoint(19).Position.X},{skl[i].GetJoint(19).Position.Y},{skl[i].GetJoint(19).Position.Z}" +
                $",{cofig2Int(0, 19)}" +
                $",{skl[i].GetJoint(20).Position.X},{skl[i].GetJoint(20).Position.Y},{skl[i].GetJoint(20).Position.Z}" +
                $",{cofig2Int(0, 20)}" +
                $",{skl[i].GetJoint(21).Position.X},{skl[i].GetJoint(21).Position.Y},{skl[i].GetJoint(21).Position.Z}" +
                $",{cofig2Int(0, 21)}" +
                $",{skl[i].GetJoint(22).Position.X},{skl[i].GetJoint(22).Position.Y},{skl[i].GetJoint(22).Position.Z}" +
                $",{cofig2Int(0, 22)}" +
                $",{skl[i].GetJoint(23).Position.X},{skl[i].GetJoint(23).Position.Y},{skl[i].GetJoint(23).Position.Z}" +
                $",{cofig2Int(0, 23)}" +
                $",{skl[i].GetJoint(24).Position.X},{skl[i].GetJoint(24).Position.Y},{skl[i].GetJoint(24).Position.Z}" +
                $",{cofig2Int(0, 24)}" +
                $",{skl[i].GetJoint(25).Position.X},{skl[i].GetJoint(25).Position.Y},{skl[i].GetJoint(25).Position.Z}" +
                $",{cofig2Int(0, 25)}" +
                $",{skl[i].GetJoint(26).Position.X},{skl[i].GetJoint(26).Position.Y},{skl[i].GetJoint(26).Position.Z}" +
                $",{cofig2Int(0, 26)}" +
                $",{skl[i].GetJoint(27).Position.X},{skl[i].GetJoint(27).Position.Y},{skl[i].GetJoint(27).Position.Z}" +
                $",{cofig2Int(0, 27)}" +
                $",{skl[i].GetJoint(28).Position.X},{skl[i].GetJoint(28).Position.Y},{skl[i].GetJoint(28).Position.Z}" +
                $",{cofig2Int(0, 28)}" +
                $",{skl[i].GetJoint(29).Position.X},{skl[i].GetJoint(29).Position.Y},{skl[i].GetJoint(29).Position.Z}" +
                $",{cofig2Int(0, 29)}" +
                $",{skl[i].GetJoint(30).Position.X},{skl[i].GetJoint(30).Position.Y},{skl[i].GetJoint(30).Position.Z}" +
                $",{cofig2Int(0, 30)}" +
                $",{skl[i].GetJoint(31).Position.X},{skl[i].GetJoint(31).Position.Y},{skl[i].GetJoint(31).Position.Z}" +
                $",{cofig2Int(0, 31)}");
                sw.Close();
            }

        }

        private void fileName()
        {
            for (int i = 0; i < installCameraNumber; ++i)
            {
                dataFullName[i] = dataPath + "\\" + "[" + dataNum + "]" + dataName + "_" + (i + 1).ToString() + ".csv";
                videoFullName[i] = dataPath + "\\" + "[" + dataNum + "]" + dataName + "_" + (i + 1).ToString() + ".mp4";
            }

        }
        private void dataGen(int i)
        {
            fileName();

            StreamWriter sw = File.AppendText(dataFullName[i]);

            sw.WriteLine($"{"LEFT_TIME"}" +
         /*0*/ $",{"PELVIS.X"},{"PELVIS.Y"},{"PELVIS.Z"}" +
               $",{"PELVIS.CONFIDENCE"}" +
         /*1*/ $",{"SPINE_NAVAL.X"},{"SPINE_NAVAL.Y"},{"SPINE_NAVAL.Z"}" +
               $",{"SPINE_NAVAL.CONFIDENCE"}" +
         /*2*/ $",{"SPINE_CHEST.X"},{"SPINE_CHEST.Y"},{"SPINE_CHEST.Z"}" +
               $",{"SPINE_CHEST.CONFIDENCE"}" +
         /*3*/ $",{"NECK.X"},{"NECK.Y"},{"NECK.Z"}" +
               $",{"NECK.CONFIDENCE"}" +
         /*4*/ $",{"CLAVICLE_LEFT.X"},{"CLAVICLE_LEFT.Y"},{"CLAVICLE_LEFT.Z"}" +
               $",{"CLAVICLE_LEFT.CONFIDENCE"}" +
         /*5*/ $",{"SHOULDER_LEFT.X"},{"SHOULDER_LEFT.Y"},{"SHOULDER_LEFT.Z"}" +
               $",{"SHOULDER_LEFT.CONFIDENCE"}" +
         /*6*/ $",{"ELBOW_LEFT.X"},{"ELBOW_LEFT.Y"},{"ELBOW_LEFT.Z"}" +
               $",{"ELBOW_LEFT.CONFIDENCE"}" +
         /*7*/ $",{"WRIST_LEFT.X"},{"WRIST_LEFT.Y"},{"WRIST_LEFT.Z"}" +
               $",{"WRIST_LEFT.CONFIDENCE"}" +
         /*8*/ $",{"HAND_LEFT.X"},{"HAND_LEFT.Y"},{"HAND_LEFT.Z"}" +
               $",{"HAND_LEFT.CONFIDENCE"}" +
         /*9*/ $",{"HANDTIP_LEFT.X"},{"HANDTIP_LEFT.Y"},{"HANDTIP_LEFT.Z"}" +
               $",{"HANDTIP_LEFT.CONFIDENCE"}" +
         /*10*/ $",{"THUMB_LEFT.X"},{"THUMB_LEFT.Y"},{"THUMB_LEFT.Z"}" +
                $",{"THUMB_LEFT.CONFIDENCE"}" +
         /*11*/ $",{"CLAVICLE_RIGHT.X"},{"CLAVICLE_RIGHT.Y"},{"CLAVICLE_RIGHT.Z"}" +
                $",{"CLAVICLE_RIGHT.CONFIDENCE"}" +
         /*12*/ $",{"SHOULDER_RIGHT.X"},{"SHOULDER_RIGHT.Y"},{"SHOULDER_RIGHT.Z"}" +
                $",{"SHOULDER_RIGHT.CONFIDENCE"}" +
         /*13*/ $",{"ELBOW_RIGHT.X"},{"ELBOW_RIGHT.Y"},{"ELBOW_RIGHT.Z"}" +
                $",{"ELBOW_RIGHT.CONFIDENCE"}" +
         /*14*/ $",{"WRIST_RIGHT.X"},{"WRIST_RIGHT.Y"},{"WRIST_RIGHT.Z"}" +
                $",{"WRIST_RIGHT.CONFIDENCE"}" +
         /*15*/ $",{"HAND_RIGHT.X"},{"HAND_RIGHT.Y"},{"HAND_RIGHT.Z"}" +
                $",{"HAND_RIGHT.CONFIDENCE"}" +
         /*16*/ $",{"HANDTIP_RIGHT.X"},{"HANDTIP_RIGHT.Y"},{"HANDTIP_RIGHT.Z"}" +
                $",{"HANDTIP_RIGHT.CONFIDENCE"}" +
         /*17*/ $",{"THUMB_RIGHT.X"},{"THUMB_RIGHT.Y"},{"THUMB_RIGHT.Z"}" +
                $",{"THUMB_RIGHT.CONFIDENCE"}" +
         /*18*/ $",{"HIP_LEFT.X"},{"HIP_LEFT.Y"},{"HIP_LEFT.Z"}" +
                $",{"HIP_LEFT.CONFIDENCE"}" +
         /*19*/ $",{"KNEE_LEFT.X"},{"KNEE_LEFT.Y"},{"KNEE_LEFT.Z"}" +
                $",{"KNEE_LEFT.CONFIDENCE"}" +
         /*20*/ $",{"ANKLE_LEFT.X"},{"ANKLE_LEFT.Y"},{"ANKLE_LEFT.Z"}" +
                $",{"ANKLE_LEFT.CONFIDENCE"}" +
         /*21*/ $",{"FOOT_LEFT.X"},{"FOOT_LEFT.Y"},{"FOOT_LEFT.Z"}" +
                $",{"FOOT_LEFT.CONFIDENCE"}" +
         /*22*/ $",{"HIP_RIGHT.X"},{"HIP_RIGHT.Y"},{"HIP_RIGHT.Z"}" +
                $",{"HIP_RIGHT.CONFIDENCE"}" +
         /*23*/ $",{"KNEE_RIGHT.X"},{"KNEE_RIGHT.Y"},{"KNEE_RIGHT.Z"}" +
                $",{"KNEE_RIGHT.CONFIDENCE"}" +
         /*24*/ $",{"ANKLE_RIGHT.X"},{"ANKLE_RIGHT.Y"},{"ANKLE_RIGHT.Z"}" +
                $",{"ANKLE_RIGHT.CONFIDENCE"}" +
         /*25*/ $",{"FOOT_RIGHT.X"},{"FOOT_RIGHT.Y"},{"FOOT_RIGHT.Z"}" +
                $",{"FOOT_RIGHT.CONFIDENCE"}" +
         /*26*/ $",{"HEAD.X"},{"HEAD.Y"},{"HEAD.Z"}" +
                $",{"HEAD.CONFIDENCE"}" +
         /*27*/ $",{"NOSE.X"},{"NOSE.Y"},{"NOSE.Z"}" +
                $",{"NOSE.CONFIDENCE"}" +
         /*28*/ $",{"EYE_LEFT.X"},{"EYE_LEFT.Y"},{"EYE_LEFT.Z"}" +
                $",{"EYE_LEFT.CONFIDENCE"}" +
         /*29*/ $",{"EAR_LEFT.X"},{"EAR_LEFT.Y"},{"EAR_LEFT.Z"}" +
                $",{"EAR_LEFT.CONFIDENCE"}" +
         /*30*/ $",{"EYE_RIGHT.X"},{"EYE_RIGHT.Y"},{"EYE_RIGHT.Z"}" +
                $",{"EYE_RIGHT.CONFIDENCE"}" +
         /*31*/ $",{"EAR_RIGHT.X"},{"EAR_RIGHT.Y"},{"EAR_RIGHT.Z"}" +
                $",{"EAR_RIGHT.CONFIDENCE"}");
            sw.Close();
        }



    }
}
