using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    partial class PaymentForm
    {
        private System.ComponentModel.IContainer components = null;
        private Guna2ControlBox btnClose;
        private Guna2DragControl dragControl;
        private Label lblTitle;
        private FlowLayoutPanel flowPlans; // Chứa 3 gói cước

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(20, 20, 20);

            this.dragControl = new Guna2DragControl(this.components);
            this.dragControl.TargetControl = this;

            this.btnClose = new Guna2ControlBox { Anchor = AnchorStyles.Top | AnchorStyles.Right, FillColor = Color.Transparent, Location = new Point(855, 0) };

            this.lblTitle = new Label();
            this.lblTitle.Text = "Chọn Gói Cước Phù Hợp Với Bạn";
            this.lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(250, 30);

            // Container chứa 3 gói
            this.flowPlans = new FlowLayoutPanel();
            this.flowPlans.Location = new Point(50, 100);
            this.flowPlans.Size = new Size(800, 400);
            this.flowPlans.FlowDirection = FlowDirection.LeftToRight;
            this.flowPlans.WrapContents = false;

            this.Controls.Add(btnClose);
            this.Controls.Add(lblTitle);
            this.Controls.Add(flowPlans);
        }
    }
}