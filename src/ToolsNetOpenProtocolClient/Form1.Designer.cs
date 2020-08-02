namespace ToolsNetOpenProtocolClient
{
    partial class Form1
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
            this.grpPimSettings = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPimPortNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPimIpAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.grpClientSettings = new System.Windows.Forms.GroupBox();
            this.cmbControllerIpAddress = new System.Windows.Forms.ComboBox();
            this.grpConnectToPim = new System.Windows.Forms.GroupBox();
            this.txtPimCommunication = new System.Windows.Forms.TextBox();
            this.btnPimOpenCloseConnection = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtToolsnetIpAdress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtToolsnetPortNumber = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtToolsnetCommunication = new System.Windows.Forms.TextBox();
            this.btnToolsnetOpenCloseConnection = new System.Windows.Forms.Button();
            this.chkSimulateControllerMessages = new System.Windows.Forms.CheckBox();
            this.grpPimSettings.SuspendLayout();
            this.grpClientSettings.SuspendLayout();
            this.grpConnectToPim.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPimSettings
            // 
            this.grpPimSettings.Controls.Add(this.label2);
            this.grpPimSettings.Controls.Add(this.txtPimPortNumber);
            this.grpPimSettings.Controls.Add(this.label1);
            this.grpPimSettings.Controls.Add(this.txtPimIpAddress);
            this.grpPimSettings.Location = new System.Drawing.Point(12, 12);
            this.grpPimSettings.Name = "grpPimSettings";
            this.grpPimSettings.Size = new System.Drawing.Size(236, 83);
            this.grpPimSettings.TabIndex = 0;
            this.grpPimSettings.TabStop = false;
            this.grpPimSettings.Text = "PIM Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "PIM Port Number:";
            // 
            // txtPimPortNumber
            // 
            this.txtPimPortNumber.Location = new System.Drawing.Point(115, 49);
            this.txtPimPortNumber.Name = "txtPimPortNumber";
            this.txtPimPortNumber.Size = new System.Drawing.Size(95, 20);
            this.txtPimPortNumber.TabIndex = 3;
            this.txtPimPortNumber.Text = "6575";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PIM IP Address:";
            // 
            // txtPimIpAddress
            // 
            this.txtPimIpAddress.Location = new System.Drawing.Point(115, 22);
            this.txtPimIpAddress.Name = "txtPimIpAddress";
            this.txtPimIpAddress.Size = new System.Drawing.Size(95, 20);
            this.txtPimIpAddress.TabIndex = 1;
            this.txtPimIpAddress.Text = "172.18.4.44";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Screwer IP Address:";
            // 
            // grpClientSettings
            // 
            this.grpClientSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpClientSettings.Controls.Add(this.cmbControllerIpAddress);
            this.grpClientSettings.Controls.Add(this.label4);
            this.grpClientSettings.Location = new System.Drawing.Point(502, 12);
            this.grpClientSettings.Name = "grpClientSettings";
            this.grpClientSettings.Size = new System.Drawing.Size(727, 83);
            this.grpClientSettings.TabIndex = 1;
            this.grpClientSettings.TabStop = false;
            this.grpClientSettings.Text = "Screwer Settings (This System)";
            // 
            // cmbControllerIpAddress
            // 
            this.cmbControllerIpAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbControllerIpAddress.FormattingEnabled = true;
            this.cmbControllerIpAddress.Location = new System.Drawing.Point(127, 22);
            this.cmbControllerIpAddress.Name = "cmbControllerIpAddress";
            this.cmbControllerIpAddress.Size = new System.Drawing.Size(121, 21);
            this.cmbControllerIpAddress.TabIndex = 1;
            // 
            // grpConnectToPim
            // 
            this.grpConnectToPim.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConnectToPim.Controls.Add(this.txtPimCommunication);
            this.grpConnectToPim.Controls.Add(this.btnPimOpenCloseConnection);
            this.grpConnectToPim.Location = new System.Drawing.Point(12, 101);
            this.grpConnectToPim.Name = "grpConnectToPim";
            this.grpConnectToPim.Size = new System.Drawing.Size(1217, 191);
            this.grpConnectToPim.TabIndex = 2;
            this.grpConnectToPim.TabStop = false;
            this.grpConnectToPim.Text = "1. Connect to PIM in order to get ToolsNet IP and Port";
            // 
            // txtPimCommunication
            // 
            this.txtPimCommunication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPimCommunication.Location = new System.Drawing.Point(6, 48);
            this.txtPimCommunication.Multiline = true;
            this.txtPimCommunication.Name = "txtPimCommunication";
            this.txtPimCommunication.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPimCommunication.Size = new System.Drawing.Size(1205, 137);
            this.txtPimCommunication.TabIndex = 2;
            this.txtPimCommunication.WordWrap = false;
            // 
            // btnPimOpenCloseConnection
            // 
            this.btnPimOpenCloseConnection.Location = new System.Drawing.Point(6, 19);
            this.btnPimOpenCloseConnection.Name = "btnPimOpenCloseConnection";
            this.btnPimOpenCloseConnection.Size = new System.Drawing.Size(225, 23);
            this.btnPimOpenCloseConnection.TabIndex = 0;
            this.btnPimOpenCloseConnection.Text = "Open / Close Connection to PIM";
            this.btnPimOpenCloseConnection.UseVisualStyleBackColor = true;
            this.btnPimOpenCloseConnection.Click += new System.EventHandler(this.btnPimOpenCloseConnection_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtToolsnetIpAdress);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtToolsnetPortNumber);
            this.groupBox1.Location = new System.Drawing.Point(254, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 83);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ToolsNet Settings";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "ToolsNet IP Address:";
            // 
            // txtToolsnetIpAdress
            // 
            this.txtToolsnetIpAdress.Enabled = false;
            this.txtToolsnetIpAdress.Location = new System.Drawing.Point(137, 22);
            this.txtToolsnetIpAdress.Name = "txtToolsnetIpAdress";
            this.txtToolsnetIpAdress.Size = new System.Drawing.Size(95, 20);
            this.txtToolsnetIpAdress.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "ToolsNet Port Number:";
            // 
            // txtToolsnetPortNumber
            // 
            this.txtToolsnetPortNumber.Enabled = false;
            this.txtToolsnetPortNumber.Location = new System.Drawing.Point(137, 49);
            this.txtToolsnetPortNumber.Name = "txtToolsnetPortNumber";
            this.txtToolsnetPortNumber.Size = new System.Drawing.Size(95, 20);
            this.txtToolsnetPortNumber.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkSimulateControllerMessages);
            this.groupBox2.Controls.Add(this.txtToolsnetCommunication);
            this.groupBox2.Controls.Add(this.btnToolsnetOpenCloseConnection);
            this.groupBox2.Location = new System.Drawing.Point(6, 298);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1217, 242);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. Connect to ToolsNet to send Alarm and Torque";
            // 
            // txtToolsnetCommunication
            // 
            this.txtToolsnetCommunication.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToolsnetCommunication.Location = new System.Drawing.Point(6, 48);
            this.txtToolsnetCommunication.Multiline = true;
            this.txtToolsnetCommunication.Name = "txtToolsnetCommunication";
            this.txtToolsnetCommunication.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtToolsnetCommunication.Size = new System.Drawing.Size(1205, 188);
            this.txtToolsnetCommunication.TabIndex = 2;
            this.txtToolsnetCommunication.WordWrap = false;
            // 
            // btnToolsnetOpenCloseConnection
            // 
            this.btnToolsnetOpenCloseConnection.Location = new System.Drawing.Point(6, 19);
            this.btnToolsnetOpenCloseConnection.Name = "btnToolsnetOpenCloseConnection";
            this.btnToolsnetOpenCloseConnection.Size = new System.Drawing.Size(225, 23);
            this.btnToolsnetOpenCloseConnection.TabIndex = 0;
            this.btnToolsnetOpenCloseConnection.Text = "Open/Close Connection to ToolsNet";
            this.btnToolsnetOpenCloseConnection.UseVisualStyleBackColor = true;
            this.btnToolsnetOpenCloseConnection.Click += new System.EventHandler(this.btnToolsnetOpenCloseConnection_Click);
            // 
            // chkSimulateControllerMessages
            // 
            this.chkSimulateControllerMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSimulateControllerMessages.AutoSize = true;
            this.chkSimulateControllerMessages.Location = new System.Drawing.Point(966, 23);
            this.chkSimulateControllerMessages.Name = "chkSimulateControllerMessages";
            this.chkSimulateControllerMessages.Size = new System.Drawing.Size(245, 17);
            this.chkSimulateControllerMessages.TabIndex = 3;
            this.chkSimulateControllerMessages.Text = "Only simulate controller\'s messages (for debug)";
            this.chkSimulateControllerMessages.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1241, 552);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpConnectToPim);
            this.Controls.Add(this.grpClientSettings);
            this.Controls.Add(this.grpPimSettings);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpPimSettings.ResumeLayout(false);
            this.grpPimSettings.PerformLayout();
            this.grpClientSettings.ResumeLayout(false);
            this.grpClientSettings.PerformLayout();
            this.grpConnectToPim.ResumeLayout(false);
            this.grpConnectToPim.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPimSettings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPimPortNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPimIpAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grpClientSettings;
        private System.Windows.Forms.GroupBox grpConnectToPim;
        private System.Windows.Forms.TextBox txtPimCommunication;
        private System.Windows.Forms.Button btnPimOpenCloseConnection;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtToolsnetPortNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtToolsnetIpAdress;
        private System.Windows.Forms.ComboBox cmbControllerIpAddress;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtToolsnetCommunication;
        private System.Windows.Forms.Button btnToolsnetOpenCloseConnection;
        private System.Windows.Forms.CheckBox chkSimulateControllerMessages;
    }
}

