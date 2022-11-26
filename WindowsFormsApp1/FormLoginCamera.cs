﻿using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace IPCameraManager
{
    public partial class FormLoginCamera : Form
    {
        public FormLoginCamera(CameraManager_Type MainCam, CameraManager_Type Cam2)
        {
            InitializeComponent();
            Init_Button();
            LoginInfo_MainCAM.LoginStatus = MainCam.LoginInfo.LoginStatus;
            Live_Status_MainCAM = MainCam.Live_Status;
            LoginInfo_CAM2.LoginStatus = Cam2.LoginInfo.LoginStatus;
            Live_Status_CAM2 = Cam2.Live_Status;
            _Current_tabID = MAINTAB;
        }
        public LoginCameraInfo_Type LoginInfo_MainCAM;
        public LoginCameraInfo_Type LoginInfo_CAM2;
        private int Live_Status_MainCAM;
        private int Live_Status_CAM2;

        private const int MAINTAB = 1;
        private const int SECONDTAB = 2;
        private int _Current_tabID = 0;

        private uint Err_Return;
        private const int ERR_OK = 0;
        private const int ERR_NOT_OK = 1;
        private bool is_ButtonLogin_Pressed = false;
        private void Init_Button()
        {
            btLogin.GotFocus += (s, a) =>
            {
                btLogin.BorderColor = Color.RoyalBlue;
                btLogin.BorderThickness = 2;
            };
            btLogin.LostFocus += (s, a) =>
            {
                btLogin.BorderThickness = 1;
                btLogin.BorderColor = SystemColors.ControlDark;
            };
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            is_ButtonLogin_Pressed = true;
            if (textBoxIP.Text == "" || textBoxPort.Text == "" ||
                textBoxUserName.Text == "" || textBoxPassword.Text == "")
            {
                MessageBox.Show("Hãy nhập đầy đủ thông tin!", "WARNING",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_Current_tabID == MAINTAB)
            {
                // Luu thong tin tu textbox vao Login_Info de xu ly Login_Camera
                LoginInfo_MainCAM.IP_Address = textBoxIP.Text;
                LoginInfo_MainCAM.Port = textBoxPort.Text;
                LoginInfo_MainCAM.Username = textBoxUserName.Text;
                LoginInfo_MainCAM.Password = textBoxPassword.Text;
                if (LoginInfo_MainCAM.LoginStatus < 0)
                {
                    if (ERR_OK == Login_Main_Camera(LoginInfo_MainCAM))
                    {
                        btLogin.Text = "Đăng xuất";
                        // Dang nhap thanh cong thi khong cho ghi thong tin vao textbox
                        textBoxIP.ReadOnly = true;
                        textBoxPassword.ReadOnly = true;
                        textBoxPort.ReadOnly = true;
                        textBoxUserName.ReadOnly = true;
                        MessageBox.Show("Kết nối Camera thành công!");
                    }
                }
                else
                {
                    if (ERR_OK == Logout_Main_Camera(LoginInfo_MainCAM))
                    {
                        btLogin.Text = "Đăng nhập";
                        // Dang xuat thanh cong thi cho ghi thong tin vao textbox
                        textBoxIP.ReadOnly = false;
                        textBoxPassword.ReadOnly = false;
                        textBoxPort.ReadOnly = false;
                        textBoxUserName.ReadOnly = false;
                    }
                }
            }
            else if (_Current_tabID == SECONDTAB)
            {
                // Luu thong tin tu textbox vao Login_Info de xu ly Login_Camera
                LoginInfo_CAM2.IP_Address = textBoxIP.Text;
                LoginInfo_CAM2.Port = textBoxPort.Text;
                LoginInfo_CAM2.Username = textBoxUserName.Text;
                LoginInfo_CAM2.Password = textBoxPassword.Text;
                if (LoginInfo_CAM2.LoginStatus < 0)
                {
                    if (ERR_OK == Login_Second_Camera(LoginInfo_CAM2))
                    {
                        btLogin.Text = "Đăng xuất";
                        // Dang nhap thanh cong thi khong cho ghi thong tin vao textbox
                        textBoxIP.ReadOnly = true;
                        textBoxPassword.ReadOnly = true;
                        textBoxPort.ReadOnly = true;
                        textBoxUserName.ReadOnly = true;
                        MessageBox.Show("Kết nối Camera thành công!");
                    }
                }
                else
                {
                    if (ERR_OK == Logout_Second_Camera(LoginInfo_CAM2))
                    {
                        btLogin.Text = "Đăng nhập";
                        // Dang xuat thanh cong thi cho ghi thong tin vao textbox
                        textBoxIP.ReadOnly = false;
                        textBoxPassword.ReadOnly = false;
                        textBoxPort.ReadOnly = false;
                        textBoxUserName.ReadOnly = false;
                    }
                }
            }


        }
        public int Login_Main_Camera(LoginCameraInfo_Type info)
        {
            if (info.LoginStatus < 0)
            {
                string DVRIPAddress = info.IP_Address;
                Int16 DVRPortNumber;
                try
                {
                    DVRPortNumber = Int16.Parse(info.Port);
                }
                catch
                {
                    return ERR_NOT_OK;
                }
                string DVRUserName = info.Username;
                string DVRPassword = info.Password;

                try
                {
                    IPAddress address = IPAddress.Parse(DVRIPAddress);
                }
                catch
                {
                    MessageBox.Show("Địa chỉ IP không xác định!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ERR_NOT_OK;
                }

                CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

                //Login the device
                LoginInfo_MainCAM.LoginStatus = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
                if (LoginInfo_MainCAM.LoginStatus < 0)
                {
                    Err_Return = CHCNetSDK.NET_DVR_GetLastError();
                    string str = "Camera chính: Đăng nhập thất bại, error code = " + Err_Return; // Print Error Name through Message Box
                    MessageBox.Show(str, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ERR_NOT_OK;
                }
                else
                {
                    //Login Success
                    return ERR_OK;
                }

            }
            else
            {
                // Already Login
                return ERR_NOT_OK;
            }
        }
        public int Login_Second_Camera(LoginCameraInfo_Type info)
        {
            if (info.LoginStatus < 0)
            {
                string DVRIPAddress = info.IP_Address;
                Int16 DVRPortNumber;
                try
                {
                    DVRPortNumber = Int16.Parse(info.Port);
                }
                catch
                {
                    return ERR_NOT_OK;
                }
                string DVRUserName = info.Username;
                string DVRPassword = info.Password;

                try
                {
                    IPAddress address = IPAddress.Parse(DVRIPAddress);
                }
                catch
                {
                    MessageBox.Show("Địa chỉ IP không xác định!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ERR_NOT_OK;
                }

                CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

                //Login the device
                LoginInfo_CAM2.LoginStatus = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
                if (LoginInfo_CAM2.LoginStatus < 0)
                {
                    Err_Return = CHCNetSDK.NET_DVR_GetLastError();
                    string str = "Camera phụ: Đăng nhập thất bại, error code = " + Err_Return; // Print Error Name through Message Box
                    MessageBox.Show(str, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ERR_NOT_OK;
                }
                else
                {
                    //Login Success
                    return ERR_OK;
                }

            }
            else
            {
                // Already Login
                return ERR_NOT_OK;
            }
        }
        public int Logout_Main_Camera(LoginCameraInfo_Type info)
        {
            if (info.LoginStatus < 0)
            {
                return ERR_NOT_OK;
            }
            else
            {
                // Logout the device
                if (Live_Status_MainCAM >= 0)
                {
                    MessageBox.Show("Hãy tắt Camera trước khi ngắt kết nối!", "WARNING",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ERR_NOT_OK;
                }

                if (!CHCNetSDK.NET_DVR_Logout(info.LoginStatus))
                {
                    Err_Return = CHCNetSDK.NET_DVR_GetLastError();
                    string str = "Camera chính: Đăng xuất thất bại, error code = " + Err_Return;
                    MessageBox.Show(str, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ERR_NOT_OK;
                }
                else
                {
                    //Logout Success
                    LoginInfo_MainCAM.LoginStatus = -1;
                    return ERR_OK;
                }
            }
        }
        public int Logout_Second_Camera(LoginCameraInfo_Type info)
        {
            if (info.LoginStatus < 0)
            {
                return ERR_NOT_OK;
            }
            else
            {
                // Logout the device
                if (Live_Status_CAM2 >= 0)
                {
                    MessageBox.Show("Hãy tắt Camera trước khi ngắt kết nối!", "WARNING",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ERR_NOT_OK;
                }

                if (!CHCNetSDK.NET_DVR_Logout(info.LoginStatus))
                {
                    Err_Return = CHCNetSDK.NET_DVR_GetLastError();
                    string str = "Camera phụ: Đăng xuất thất bại, error code = " + Err_Return;
                    MessageBox.Show(str, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ERR_NOT_OK;
                }
                else
                {
                    //Logout Success
                    LoginInfo_CAM2.LoginStatus = -1;
                    return ERR_OK;
                }
            }
        }
        public void Load_MainCAM_Database_Info(LoginCameraInfo_Type info)
        {
            LoginInfo_MainCAM = info;
        }
        public void Load_SecondCAM_Database_Info(LoginCameraInfo_Type info)
        {
            LoginInfo_CAM2 = info;
        }
        public void Get_LoginStatus_MainCam(ref LoginCameraInfo_Type info)
        {
            info = LoginInfo_MainCAM;
        }
        public void Get_LoginStatus_Cam2(ref LoginCameraInfo_Type info)
        {
            info = LoginInfo_CAM2;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btTabMainCam_Click(object sender, EventArgs e)
        {
            _Current_tabID = MAINTAB;
            textBoxIP.Text = LoginInfo_MainCAM.IP_Address;
            textBoxPort.Text = LoginInfo_MainCAM.Port;
            textBoxUserName.Text = LoginInfo_MainCAM.Username;
            textBoxPassword.Text = LoginInfo_MainCAM.Password;
            if (LoginInfo_MainCAM.LoginStatus >= 0)
            {
                btLogin.Text = "Đăng xuất";
                textBoxIP.ReadOnly = true;
                textBoxPassword.ReadOnly = true;
                textBoxPort.ReadOnly = true;
                textBoxUserName.ReadOnly = true;
            }
            else
            {
                btLogin.Text = "Đăng nhập";
                textBoxIP.ReadOnly = false;
                textBoxPassword.ReadOnly = false;
                textBoxPort.ReadOnly = false;
                textBoxUserName.ReadOnly = false;
            }
        }
        private void btTabCam2_Click(object sender, EventArgs e)
        {
            _Current_tabID = SECONDTAB;
            textBoxIP.Text = LoginInfo_CAM2.IP_Address;
            textBoxPort.Text = LoginInfo_CAM2.Port;
            textBoxUserName.Text = LoginInfo_CAM2.Username;
            textBoxPassword.Text = LoginInfo_CAM2.Password;
            if (LoginInfo_CAM2.LoginStatus >= 0)
            {
                btLogin.Text = "Đăng xuất";
                textBoxIP.ReadOnly = true;
                textBoxPassword.ReadOnly = true;
                textBoxPort.ReadOnly = true;
                textBoxUserName.ReadOnly = true;
            }
            else
            {
                btLogin.Text = "Đăng nhập";
                textBoxIP.ReadOnly = false;
                textBoxPassword.ReadOnly = false;
                textBoxPort.ReadOnly = false;
                textBoxUserName.ReadOnly = false;
            }
        }

        private void FormLoginCamera_Load(object sender, EventArgs e)
        {
            is_ButtonLogin_Pressed = false;
            if (_Current_tabID == MAINTAB)
            {
                btTabMainCam_Click(sender, e);
            }
            else if (_Current_tabID == SECONDTAB)
            {
                btTabCam2_Click(sender, e);
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            if(is_ButtonLogin_Pressed == true)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            if (_Current_tabID == MAINTAB)
            {
                LoginInfo_MainCAM.IP_Address = textBoxIP.Text;
            }
            else if (_Current_tabID == SECONDTAB)
            {
                LoginInfo_CAM2.IP_Address = textBoxIP.Text;
            }
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            if (_Current_tabID == MAINTAB)
            {
                LoginInfo_MainCAM.Port = textBoxPort.Text;
            }
            else if (_Current_tabID == SECONDTAB)
            {
                LoginInfo_CAM2.Port = textBoxPort.Text;
            }
        }

        private void textBoxUserName_TextChanged(object sender, EventArgs e)
        {
            if (_Current_tabID == MAINTAB)
            {
                LoginInfo_MainCAM.Username = textBoxUserName.Text;
            }
            else if (_Current_tabID == SECONDTAB)
            {
                LoginInfo_CAM2.Username = textBoxUserName.Text;
            }
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            if (_Current_tabID == MAINTAB)
            {
                LoginInfo_MainCAM.Password = textBoxPassword.Text;
            }
            else if (_Current_tabID == SECONDTAB)
            {
                LoginInfo_CAM2.Password = textBoxPassword.Text;
            }
        }
    }
    public struct LoginCameraInfo_Type
    {
        private string _IP_Address;
        private string _Port;
        private string _Username;
        private string _Password;
        private int? _LoginStatus;

        public int LoginStatus { get { return _LoginStatus ?? -1; } set => _LoginStatus = value; }
        public string IP_Address { get { return _IP_Address ?? ""; } set => _IP_Address = value; }
        public string Port { get { return _Port ?? ""; } set => _Port = value; }
        public string Username { get { return _Username ?? "admin"; } set => _Username = value; }
        public string Password { get { return _Password ?? "abcd1234"; } set => _Password = value; }
    }
}
