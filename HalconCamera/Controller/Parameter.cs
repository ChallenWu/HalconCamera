using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HalconCamera
{
    public class Parameter
    {
        public static string LoggedInUser { get; set; }
        public static DateTime LastActiveTime { get; set; }
        public static bool Screen_Status { get; set; }
        public static Point Screen_Position { get; set; }
        public static Size Screen_Size { get; set; }
        public static List<ModelMatching> ModelsMatching { get; set; }
        public class Config
        {
            public Roi RoiDUT { get; set; }
            public Roi RoiLabel { get; set; }           
            public Camera Camera { get; set; }
            public List<Position> Position { get; set; }
            public Setup Setup { get; set; }
            public Ats Ats { get; set; }
        }
        public class Roi
        {
            public string component { get; set; }
            public double row1 { get; set; }
            public double col1 { get; set; }
            public double row2 { get; set; }
            public double col2 { get; set; }
            public double row1stand { get; set; } 
            public double col1stand { get; set; }
            public double row2stand { get; set; }
            public double col2stand { get; set; }
            public double radius { get; set; }
            public double markx { get; set; }
            public double marky { get; set; }
            public double centerx { get; set; }
            public double centery { get; set; }
            public double labelx { get; set; }
            public double labely { get; set; }
        }
        public class Camera
        {
            public string CameraTopType { get; set; }
            public string CameraTopName { get; set; }
            public string CameraBotType { get; set; }
            public string CameraBotName { get; set; }
        }
        public class Position
        {
            public string name { get; set; }
            public int zpoint { get; set; }
        }
        public class SpeedJog
        {
            public int value { get; set; }
        }
        public class ModelMatching
        {
            public string name { get; set; }
            public string path { get; set; }
            public HTuple models { get; set; }
        }
        public class ResultMatching
        {
            public double markx { get; set; }
            public double marky { get; set; }
            public double angle { get; set; }
            public double score { get; set; }
        }
        public class ResultCali
        {
            public double dx { get; set; }
            public double dy { get; set; }
            public double angle { get; set; }
            public int pulseX { get; set; }
            public int pulseY { get; set; }
            public int pulseR { get; set; }
            public string type { get; set; }
        }
        public class Setup
        {
            public string language { get; set; }
            public int typerun { get; set; }
            public int moderun { get; set; }
            public int  delaypick { get; set; }
            public int delaystick { get; set; }
            public int delaycali { get; set; }
            public string comscan { get; set; }
            public string comats { get; set; }
            public int deltaX { get; set; }
            public int deltaY { get; set; }
            public int deltaR { get; set; }
            public bool passDUT { get; set; }
            public bool passWindows { get; set; }
        }
        public class Ats
        {
            public string ip { get; set; }
            public int port { get; set; }
        }
        public static Config Loadconfig()
        {
            try
            {
                string json = File.ReadAllText(@".\ConfigFile\Config.json");
                Config DataConfig = json.Length > 0 ? JsonConvert.DeserializeObject<Config>(json) : new Config();
                return DataConfig;
            }
            catch
            {
                return null;
            }
        }
        public static bool Saveconfig(Config DataConfig)
        {
            try
            {
                if (!Directory.Exists(".\\ConfigFile"))
                {
                    Directory.CreateDirectory(".\\ConfigFile");
                }
                string Json = JsonConvert.SerializeObject(DataConfig, Formatting.Indented);
                File.WriteAllText(@".\ConfigFile\Config.json", Json);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool LoadModels()
        {
            try
            {
                if (ModelsMatching != null)
                {
                    ModelsMatching.ForEach(a => a.models.Dispose());
                    ModelsMatching.Clear();
                }
                else
                    ModelsMatching = new List<ModelMatching>();
                HOperatorSet.ReadShapeModel(".\\ModelMatching\\DUTModel", out HTuple DUT);
                ModelsMatching.Add(new ModelMatching
                {
                    name = "DUT",
                    models = DUT
                });
                HOperatorSet.ReadNccModel(".\\ModelMatching\\LabelModel", out HTuple Label);
                ModelsMatching.Add(new ModelMatching
                {
                    name = "LABEL",
                    models = Label
                });
                HOperatorSet.ReadShapeModel(".\\ModelMatching\\AfterModel", out HTuple AFTER);
                ModelsMatching.Add(new ModelMatching
                {
                    name = "AFTER",
                    models = AFTER
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
