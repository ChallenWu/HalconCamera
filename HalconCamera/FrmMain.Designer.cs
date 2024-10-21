namespace HalconCamera
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bntOpen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnTrigger = new System.Windows.Forms.Button();
            this.btnContinous = new System.Windows.Forms.Button();
            this.btnOpenImage = new System.Windows.Forms.Button();
            this.btnDrawRect = new System.Windows.Forms.Button();
            this.btnCreateROI = new System.Windows.Forms.Button();
            this.btnCreateModel = new System.Windows.Forms.Button();
            this.btnTestCheck = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnROICountours = new System.Windows.Forms.Button();
            this.btnCreateModelCountours = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnDrawROI = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.hWindowControl1 = new HalconDotNet.HSmartWindowControl();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bntOpen
            // 
            this.bntOpen.Location = new System.Drawing.Point(27, 20);
            this.bntOpen.Name = "bntOpen";
            this.bntOpen.Size = new System.Drawing.Size(110, 45);
            this.bntOpen.TabIndex = 1;
            this.bntOpen.Text = "Open Camera";
            this.bntOpen.UseVisualStyleBackColor = true;
            this.bntOpen.Click += new System.EventHandler(this.bntOpen_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(211, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 45);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close Camera";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnTrigger
            // 
            this.btnTrigger.Location = new System.Drawing.Point(27, 97);
            this.btnTrigger.Name = "btnTrigger";
            this.btnTrigger.Size = new System.Drawing.Size(110, 45);
            this.btnTrigger.TabIndex = 3;
            this.btnTrigger.Text = "Trigger";
            this.btnTrigger.UseVisualStyleBackColor = true;
            this.btnTrigger.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // btnContinous
            // 
            this.btnContinous.Location = new System.Drawing.Point(27, 171);
            this.btnContinous.Name = "btnContinous";
            this.btnContinous.Size = new System.Drawing.Size(110, 45);
            this.btnContinous.TabIndex = 4;
            this.btnContinous.Text = "Continous";
            this.btnContinous.UseVisualStyleBackColor = true;
            this.btnContinous.Click += new System.EventHandler(this.btnContinous_Click);
            // 
            // btnOpenImage
            // 
            this.btnOpenImage.Location = new System.Drawing.Point(15, 15);
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.Size = new System.Drawing.Size(110, 45);
            this.btnOpenImage.TabIndex = 5;
            this.btnOpenImage.Text = "OpenImage";
            this.btnOpenImage.UseVisualStyleBackColor = true;
            this.btnOpenImage.Click += new System.EventHandler(this.btnOpenImage_Click);
            // 
            // btnDrawRect
            // 
            this.btnDrawRect.Location = new System.Drawing.Point(247, 15);
            this.btnDrawRect.Name = "btnDrawRect";
            this.btnDrawRect.Size = new System.Drawing.Size(110, 45);
            this.btnDrawRect.TabIndex = 6;
            this.btnDrawRect.Text = "Draw Rect";
            this.btnDrawRect.UseVisualStyleBackColor = true;
            this.btnDrawRect.Click += new System.EventHandler(this.btnDrawRect_Click);
            // 
            // btnCreateROI
            // 
            this.btnCreateROI.Location = new System.Drawing.Point(15, 85);
            this.btnCreateROI.Name = "btnCreateROI";
            this.btnCreateROI.Size = new System.Drawing.Size(110, 45);
            this.btnCreateROI.TabIndex = 7;
            this.btnCreateROI.Text = "Creat ROI";
            this.btnCreateROI.UseVisualStyleBackColor = true;
            this.btnCreateROI.Click += new System.EventHandler(this.btnCreateROI_Click);
            // 
            // btnCreateModel
            // 
            this.btnCreateModel.Location = new System.Drawing.Point(131, 85);
            this.btnCreateModel.Name = "btnCreateModel";
            this.btnCreateModel.Size = new System.Drawing.Size(110, 45);
            this.btnCreateModel.TabIndex = 8;
            this.btnCreateModel.Text = "Create Model ROI";
            this.btnCreateModel.UseVisualStyleBackColor = true;
            this.btnCreateModel.Click += new System.EventHandler(this.btnCreateModel_Click);
            // 
            // btnTestCheck
            // 
            this.btnTestCheck.Location = new System.Drawing.Point(15, 234);
            this.btnTestCheck.Name = "btnTestCheck";
            this.btnTestCheck.Size = new System.Drawing.Size(110, 45);
            this.btnTestCheck.TabIndex = 9;
            this.btnTestCheck.Text = "Test Check ROI";
            this.btnTestCheck.UseVisualStyleBackColor = true;
            this.btnTestCheck.Click += new System.EventHandler(this.btnTestCheck_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 633);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Thời gian xử lý";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(110, 634);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "0 ms";
            // 
            // btnROICountours
            // 
            this.btnROICountours.Location = new System.Drawing.Point(15, 160);
            this.btnROICountours.Name = "btnROICountours";
            this.btnROICountours.Size = new System.Drawing.Size(110, 45);
            this.btnROICountours.TabIndex = 12;
            this.btnROICountours.Text = "Creat ROI Label";
            this.btnROICountours.UseVisualStyleBackColor = true;
            this.btnROICountours.Click += new System.EventHandler(this.btnROICountours_Click);
            // 
            // btnCreateModelCountours
            // 
            this.btnCreateModelCountours.Location = new System.Drawing.Point(131, 160);
            this.btnCreateModelCountours.Name = "btnCreateModelCountours";
            this.btnCreateModelCountours.Size = new System.Drawing.Size(110, 45);
            this.btnCreateModelCountours.TabIndex = 13;
            this.btnCreateModelCountours.Text = "Create Model Label";
            this.btnCreateModelCountours.UseVisualStyleBackColor = true;
            this.btnCreateModelCountours.Click += new System.EventHandler(this.btnCreateModelCountours_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(819, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(380, 612);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnDrawROI);
            this.tabPage2.Controls.Add(this.btnOpenImage);
            this.tabPage2.Controls.Add(this.btnCreateModelCountours);
            this.tabPage2.Controls.Add(this.btnDrawRect);
            this.tabPage2.Controls.Add(this.btnROICountours);
            this.tabPage2.Controls.Add(this.btnCreateROI);
            this.tabPage2.Controls.Add(this.btnCreateModel);
            this.tabPage2.Controls.Add(this.btnTestCheck);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(372, 586);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Image Processing";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnDrawROI
            // 
            this.btnDrawROI.Location = new System.Drawing.Point(131, 15);
            this.btnDrawROI.Name = "btnDrawROI";
            this.btnDrawROI.Size = new System.Drawing.Size(110, 45);
            this.btnDrawROI.TabIndex = 14;
            this.btnDrawROI.Text = "Draw FindArea";
            this.btnDrawROI.UseVisualStyleBackColor = true;
            this.btnDrawROI.Click += new System.EventHandler(this.btnDrawROI_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bntOpen);
            this.tabPage1.Controls.Add(this.btnClose);
            this.tabPage1.Controls.Add(this.btnTrigger);
            this.tabPage1.Controls.Add(this.btnContinous);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(372, 586);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Camera Stream";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hWindowControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hWindowControl1.HDoubleClickToFitContent = true;
            this.hWindowControl1.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hWindowControl1.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.HKeepAspectRatio = true;
            this.hWindowControl1.HMoveContent = true;
            this.hWindowControl1.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hWindowControl1.Location = new System.Drawing.Point(9, 12);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(807, 608);
            this.hWindowControl1.TabIndex = 15;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(807, 608);
           
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 661);
            this.Controls.Add(this.hWindowControl1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void FrmMain_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }



        #endregion
        private System.Windows.Forms.Button bntOpen;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnTrigger;
        private System.Windows.Forms.Button btnContinous;
        private System.Windows.Forms.Button btnOpenImage;
        private System.Windows.Forms.Button btnDrawRect;
        private System.Windows.Forms.Button btnCreateROI;
        private System.Windows.Forms.Button btnCreateModel;
        private System.Windows.Forms.Button btnTestCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnROICountours;
        private System.Windows.Forms.Button btnCreateModelCountours;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnDrawROI;
        private HalconDotNet.HSmartWindowControl hWindowControl1;
    }
}

