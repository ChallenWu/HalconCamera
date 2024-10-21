using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HalconCamera
{
    public class CameraTop
    {
        HTuple hv_AcqHandle = new HTuple();
        public bool IsGrap = false;
        public bool Open()
        {
            try
            {
                hv_AcqHandle.Dispose();
                string json = File.ReadAllText(@".\ConfigFile\Config.json");
                var DataConfig = json.Length > 0 ? JsonConvert.DeserializeObject<Parameter.Config>(json) : new Parameter.Config();
                if (DataConfig.Camera.CameraTopType == "DirectShow")
                {  
                    HOperatorSet.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "default", -1, "false", "default", DataConfig.Camera.CameraTopName, 0, -1, out hv_AcqHandle);
                }
                else
                {
                    HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive", -1, "default", -1, "false", "default", "CCD2", 0, -1, out hv_AcqHandle);
                }
                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Close()
        {
            try
            {
                IsGrap = false;
                Thread.Sleep(100);
                hv_AcqHandle.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public HObject GetOneFrame()
        {
            try
            {
                HObject ho_Image = null;
                HOperatorSet.GenEmptyObj(out ho_Image);
                ho_Image.Dispose();
                HOperatorSet.GrabImage(out ho_Image, hv_AcqHandle);
                return ho_Image;
            }
            catch
            {
                return null;
            }
        }
        public void StartGrap(HSmartWindowControl hWindow)
        {
            Thread tr = new Thread(() =>
            {
                try
                {
                    IsGrap = true;
                    HObject ho_Image = null;
                    HOperatorSet.GenEmptyObj(out ho_Image);
                    while (true)
                    {
                        try
                        {
                            if (!IsGrap)
                            {
                                break;
                            }
                            else
                            {
                                ho_Image.Dispose();
                                HOperatorSet.GrabImage(out ho_Image, hv_AcqHandle);
                                HOperatorSet.DispObj(ho_Image, hWindow.HalconWindow);
                            }
                        }
                        catch
                        {
                            break;
                        }
                        Thread.Sleep(1);
                    }
                    ho_Image.Dispose();
                }
                catch
                {

                }
            });
            tr.IsBackground = true;
            tr.Start();
        }
        public void StopGrap()
        {
            IsGrap = false;
        }
        public void SetPartWindow(HSmartWindowControl hWindow)
        {
            try
            {
                HObject ho_Image = null;
                HOperatorSet.GenEmptyObj(out ho_Image);
                HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                ho_Image.Dispose();
                HOperatorSet.GrabImage(out ho_Image, hv_AcqHandle);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(hWindow.HalconWindow, 0, 0, hv_Height - 1, hv_Width - 1);
                HOperatorSet.DispObj(ho_Image, hWindow.HalconWindow);
                hv_Width.Dispose();
                hv_Height.Dispose();
                ho_Image.Dispose();
            }
            catch
            {
            }
        }
        public void setparameter(double Value)
        {
            try
            {
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureAuto", "Off");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureTime", Value);
            }
            catch { }

        }
    }
    public class CameraBot
    {
        HTuple hv_AcqHandle = new HTuple();
        public bool IsGrap=false;
        public bool Open()
        {
            try
            {
                hv_AcqHandle.Dispose();
                string json = File.ReadAllText(@".\ConfigFile\Config.json");
                var DataConfig = json.Length > 0 ? JsonConvert.DeserializeObject<Parameter.Config>(json) : new Parameter.Config();
                if (DataConfig.Camera.CameraBotType.Trim() == "DirectShow")
                {
                    HOperatorSet.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "default", -1, "false", "default", DataConfig.Camera.CameraBotName.Trim(), 0, -1, out hv_AcqHandle);
                }
                else
                {
                    HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive", -1, "default", -1, "false", "default", DataConfig.Camera.CameraBotName.Trim(), 0, -1, out hv_AcqHandle);
                }
                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Close()
        {
            try
            {
                IsGrap = false;
                Thread.Sleep(100);
                hv_AcqHandle.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public HObject GetOneFrame()
        {
            try
            {
                HObject ho_Image = null;
                HOperatorSet.GenEmptyObj(out ho_Image);
                ho_Image.Dispose();
                HOperatorSet.GrabImage(out ho_Image, hv_AcqHandle);
                return ho_Image;
            }
            catch
            {
                return null;
            }
        }
        public void StartGrap(HWindowControl hWindow)
        {
            Thread tr = new Thread(() =>
            {
                try
                {
                    IsGrap = true;
                    HObject ho_Image = null;
                    HOperatorSet.GenEmptyObj(out ho_Image);
                    while (true)
                    {
                        try
                        {
                            if (!IsGrap)
                            {
                                break;
                            }
                            else
                            {
                                ho_Image.Dispose();
                                HOperatorSet.GrabImage(out ho_Image, hv_AcqHandle);
                                HOperatorSet.DispObj(ho_Image, hWindow.HalconWindow);
                            }
                        }
                        catch
                        {
                            break;
                        }
                        Thread.Sleep(1);
                    }
                    ho_Image.Dispose();
                }
                catch
                {

                }
            });
            tr.IsBackground = true;
            tr.Start();
        }
        public void StopGrap()
        {
            IsGrap = false;
        }
        public void SetPartWindow(HWindowControl hWindow)
        {
            try
            {
                HObject ho_Image = null;
                HOperatorSet.GenEmptyObj(out ho_Image);
                HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                ho_Image.Dispose();
                HOperatorSet.GrabImage(out ho_Image, hv_AcqHandle);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(hWindow.HalconWindow, 0, 0, hv_Height - 1, hv_Width - 1);
                HOperatorSet.DispObj(ho_Image, hWindow.HalconWindow);
                hv_Width.Dispose();
                hv_Height.Dispose();
                ho_Image.Dispose();
            }
            catch
            {

            }
        }
        public void setparameter(double Value)
        {
            try
            {
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureAuto", "Off");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureTime", Value);
            }
            catch { }

        }
    }
}
