
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HalconCamera
{
    /// <summary>
    ///Kết nối camera bằng dll Halcon
    /// </summary>
    public partial class FrmMain : Form
    {
        CameraTop CameraTop = new CameraTop();
        bool CameraTop_;
        bool Graber;
        H_Function H_Function = new H_Function();
        HObject Image = new HObject();
        HDrawingObject hDrawRect;
        HDrawingObject hDrawArea;
        Parameter.Roi Roi;
        Parameter.Roi AreaROI;
        Parameter.Config DataConfig = new Parameter.Config();

        public FrmMain()
        {
            InitializeComponent();
            DataConfig = Parameter.Loadconfig();
            Parameter.LoadModels();
            hWindowControl1.MouseWheel += new MouseEventHandler(HWindowControl1_MouseWheel);           
            //this.hWindowControl1.MouseWheel += HWindowControl1_MouseWheel;
        }

        /// <summary>
        /// Open Camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntOpen_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {

                if (CameraTop.Open())
                {
                    CameraTop.setparameter(3000);
                    CameraTop_ = true;
                    CameraTop.SetPartWindow(hWindowControl1);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// Get 1 image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTrigger_Click(object sender, EventArgs e)
        {
            if(CameraTop_)
            {
                try
                {
                    Image.Dispose();
                    Image = CameraTop.GetOneFrame();
                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.DispObj(Image, hWindowControl1.HalconWindow);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }            }    
            
        }

        /// <summary>
        /// Grabing continous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContinous_Click(object sender, EventArgs e)
        {
            if (Graber == false)
            {
                Graber = true;
                btnContinous.Text = "StopGrabing";
                btnContinous.BackColor = Color.Red;

                CameraTop.StopGrap();
                CameraTop.SetPartWindow(hWindowControl1);
                CameraTop.StartGrap(hWindowControl1);
            }
            else
            {
                Graber = false;
                btnContinous.Text = "Continous";
                btnContinous.BackColor = Color.Green;
                CameraTop.StopGrap();
            }    
        }
        /// <summary>
        /// Stop Camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            CameraTop.Close();
        }

        /// <summary>
        /// Load Image from folder and display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    Image = H_Function.Load_Display_Image(OFD.FileName, hWindowControl1.HalconWindow);
                    HOperatorSet.DispObj(Image, hWindowControl1.HalconWindow);
                }
            }
            catch (HalconException a)
            {
                MessageBox.Show(a.ToString());
            }
        }
        private HWindow hv_handle;
        private double row1;
        private double column1;
        private double row2;
        private double column2;
        private void btnDrawRect_Click(object sender, EventArgs e)
        {
            try
            {
                if (hDrawRect == null)
                {
                    //Tạo đối tượng vẽ 
                    hDrawRect = new HDrawingObject(100, 100, 200, 200);                    
                    //Thiết lập tham số vẽ
                    hDrawRect.SetDrawingObjectParams(new HTuple("line_width"), new HTuple(1));
                    //Gán đối tượng vào cửa sổ Halcon
                    hWindowControl1.HalconWindow.AttachDrawingObjectToWindow(hDrawRect);
                }
            }
            catch (HalconException a)
            {
                MessageBox.Show(a.ToString());
            }
        }

        private void CallbackOnSelect(HDrawingObject drawid, HWindow window, string type)
        {
            
            hDrawRect.SetDrawingObjectParams("color","green");
        }

        private void CallbackDrwingObjParm(HDrawingObject drawid, HWindow window, string type)
        {

            HTuple hv_Parms = "";
            string[] parms;
            string text = "";
            try
            {
                parms = new string[] { "row1", "column1", "row2", "column2" };
                hv_Parms = hDrawRect.GetDrawingObjectParams(parms);
                text = hv_Parms[0] + "\r\n" + hv_Parms[1] + "\r\n" + hv_Parms[2]
                    +"\r\n" + hv_Parms[3] + "\r\n";
                Console.WriteLine(text);
            }

            catch (Exception)
            {
            }
        }

        private void btnCreateROI_Click(object sender, EventArgs e)
        {
            try
            {
                if (hDrawRect == null || hDrawArea == null)
                    return;
                //Lấy dữ liệu Rect được vẽ trên ảnh
                AreaROI = H_Function.GenROI_FromhDrawRoi(hDrawArea);
                Roi = H_Function.GenROI_FromhDrawRoi(hDrawRect);

                //Tâm của Vật thể tìm kiếm
                double col = (Roi.col2 - Roi.col1) / 2 + Roi.col1;
                double row = (Roi.row2 - Roi.row1) / 2 + Roi.row1;
                var radius = (Roi.col2 - Roi.col1) / 2;
                //Bao quanh Vật thể tìm kiếm
                DataConfig.RoiDUT.centery = row;
                DataConfig.RoiDUT.centerx = col;
                DataConfig.RoiDUT.radius = radius;
                //Vùng tìm kiếm
                DataConfig.RoiDUT.row1 = AreaROI.row1;
                DataConfig.RoiDUT.col1 = AreaROI.col1;
                DataConfig.RoiDUT.row2 = AreaROI.row2;
                DataConfig.RoiDUT.col2 = AreaROI.col2;
                //Vật thể cần tìm kiếm
                DataConfig.RoiDUT.row1stand = Roi.row1;
                DataConfig.RoiDUT.col1stand = Roi.col1;
                DataConfig.RoiDUT.row2stand = Roi.row2;
                DataConfig.RoiDUT.col2stand = Roi.col2;
                //Save config to Data
                Parameter.Saveconfig(DataConfig);

                //hDrawRect.ClearDrawingObject();
                //hDrawRect = new HDrawingObject(Roi.row1, Roi.col1,Roi.row2,Roi.col2);
                //hDrawRect = new HDrawingObject();                
                //hWindowControl1.HalconWindow.AttachDrawingObjectToWindow(hDrawRect);
               //MessageBox.Show("Create ROI Success!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                
            }
        }

        private void btnCreateModel_Click(object sender, EventArgs e)
        {
            try
            {
                
                var roistand = H_Function.GenROI_FromhDrawRoi(hDrawRect);
                //Tạo vùng cắt vật thể tìm kiếm
                HObject roi  = H_Function.GenRectangle1_FromhDrawRoi(hDrawRect);
                //Giới hạn vùng điểm ảnh bằng đúng khoảng ROI được tạo ra
                HOperatorSet.ReduceDomain(Image.CopyObj(1, -1), roi, out HObject img_reduce);
                //Cắt hình ảnh ra
                HOperatorSet.CropDomain(img_reduce, out HObject imgCrop);

                //If folder not existed
                if (!Directory.Exists(".\\ModelMatching"))
                {
                    Directory.CreateDirectory(".\\ModelMatching");
                }
                //Tạo model DUT
                H_Function.CreateModelDUT(imgCrop, ".\\ModelMatching\\DUTModel");

                MessageBox.Show("Create Model Success!");
                // Xóa đi Rect trên hình ảnh                
                hDrawRect.ClearDrawingObject();
                hDrawArea.ClearDrawingObject();
                //hDrawRect = new HDrawingObject(Roi.row1, Roi.col1, Roi.row2, Roi.col2);
                //hWindowControl1.HalconWindow.AttachDrawingObjectToWindow(hDrawRect);
                Parameter.LoadModels();
            }
            catch (HalconException a)
            {
                MessageBox.Show(a.ToString());
            }
        }

        private void btnTestCheck_Click(object sender, EventArgs e)
        {
            try
            {   
                Stopwatch sw =new Stopwatch();
                sw.Start();
                //Tìm kiếm dữ liệu
                H_Function.FindPatternDUT(Image, Parameter.ModelsMatching.Where(a => a.name == "DUT").ToList()[0].models, DataConfig.RoiDUT, hWindowControl1);
                sw.Stop();
                //MessageBox.Show("Test Check DUT Success!");
                label2.Text = $"{sw.ElapsedMilliseconds} ms";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //Addlogtext(ex.ToString(), false);
            }
        }

        private void btnROICountours_Click(object sender, EventArgs e)
        {
            try
            {
                var roi = H_Function.GenROI_FromhDrawRoi(hDrawRect);
                double col = (roi.col2 - roi.col1) / 2 + roi.col1;
                double row = (roi.row2 - roi.row1) / 2 + roi.row1;
                //Điểm đánh dấu gốc
                DataConfig.RoiDUT.markx = col;
                DataConfig.RoiDUT.marky = row;
                Parameter.Saveconfig(DataConfig);
                MessageBox.Show($"Tọa độ tâm của vật thể {DataConfig.RoiDUT.markx} { DataConfig.RoiDUT.marky}");
                //ShowAlert("Success");
                //hDrawRect.ClearDrawingObject();
                //hDrawRect = new HDrawingObject(roi.row1 + 50, (roi.col2 - roi.col1) / 2 + roi.col1 - 130, roi.row1 + 150, (roi.col2 - roi.col1) / 2 + roi.col1 + 130);
                //hWindowControl1.HalconWindow.AttachDrawingObjectToWindow(hDrawRect);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //Addlogtext(ex.ToString(), false);
            }
        }

        private void btnCreateModelCountours_Click(object sender, EventArgs e)
        {
            try
            {
                // 
                var roi = H_Function.GenROI_FromhDrawRoi(hDrawRect);

                double col = (roi.col2 - roi.col1) / 2 + roi.col1;
                double row = (roi.row2 - roi.row1) / 2 + roi.row1;
                DataConfig.RoiDUT.labelx = col;
                DataConfig.RoiDUT.labely = row;


                Parameter.Saveconfig(DataConfig);
                MessageBox.Show($"Tọa độ tâm của vật thể {col} {row}");
                HObject roi1 = H_Function.GenRectangle1_FromhDrawRoi(hDrawRect);
                HOperatorSet.ReduceDomain(Image.CopyObj(1, -1), roi1, out HObject img_reduce);
                if (!Directory.Exists(".\\ModelMatching"))
                    Directory.CreateDirectory(".\\ModelMatching");
                H_Function.CreateModelDUT(img_reduce, ".\\ModelMatching\\AfterModel");
                hDrawRect.ClearDrawingObject();
                //Addlogtext("Create Model Label Success!");
                Parameter.LoadModels();
                //ShowAlert("Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //Addlogtext(ex.ToString(), false);
            }
        }

        private void btnDrawROI_Click(object sender, EventArgs e)
        {
            try
            {
                if (hDrawArea == null)
                {
                    //Tạo đối tượng vẽ 
                    hDrawArea = new HDrawingObject(100, 100, 200, 200);
                    //Thiết lập tham số vẽ
                    hDrawArea.SetDrawingObjectParams(new HTuple("line_width"), new HTuple(1));
                    
                    //Gán đối tượng vào cửa sổ Halcon
                    hWindowControl1.HalconWindow.AttachDrawingObjectToWindow(hDrawArea);
                }
            }
            catch (HalconException a)
            {
                MessageBox.Show(a.ToString());
            }
        }
        private void HWindowControl1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //hWindowControl1.MouseWheel += new MouseEventHandler(this.my_MoveWheel);
            Point pt = base.Location;
            int leftBorder = hWindowControl1.Location.X;
            int rightBorder = hWindowControl1.Location.X + hWindowControl1.Size.Width;
            int topBorder = hWindowControl1.Location.Y;
            int botBorder = hWindowControl1.Location.Y + hWindowControl1.Size.Height;
            if (e.X > leftBorder && e.X < rightBorder && e.Y > topBorder && e.Y < botBorder)
            {
                MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, (int)e.X - pt.X, (int)e.Y - pt.Y, e.Delta);
                hWindowControl1.HSmartWindowControl_MouseWheel(sender, newE);
            }
        }

        private void my_MoveWheel(object sender, MouseEventArgs e)
        {
           
        }
    }
}
