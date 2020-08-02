using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolsNetOpenProtocolClient
{
    public partial class Form1 : Form
    {
        System.Net.Sockets.TcpClient pimClientSocket;
        System.Net.Sockets.TcpClient toolsnetClientSocket;
        int identifierSent = 0;
        string identifierPim = "00000";
        int resultSequenceNumber;
        int eventSequenceNumber;
        string toolsnetServerIp;
        string toolsnetServerPort;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getIpAdresses();
        }

        void getIpAdresses()
        {
            // Get host name
            String strHostName = Dns.GetHostName();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostEntry(Dns.GetHostName());

            cmbControllerIpAddress.Items.Clear();
            cmbControllerIpAddress.Items.Add("Select IP...");
            // Enumerate IP addresses
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
                    cmbControllerIpAddress.Items.Add(ipaddress.ToString());
            }
            cmbControllerIpAddress.SelectedIndex = 0;
        }

        private void btnPimOpenCloseConnection_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] inStream12 = new byte[65537];
                byte[] inStream08 = new byte[65537];
                string data;
                pimClientSocket = new System.Net.Sockets.TcpClient();

                //1. Controller connects to Server
                pimClientSocket.Connect(txtPimIpAddress.Text, int.Parse(txtPimPortNumber.Text));
                NetworkStream serverStream = pimClientSocket.GetStream();
                txtPimCommunication.Text = "Client Socket Program - Server Connected ..." + Environment.NewLine;
                Application.DoEvents();

                //2. get Telegram 12 (PIM Verification):
                serverStream.Read(inStream12, 0, (int)pimClientSocket.ReceiveBufferSize);
                data = System.Text.Encoding.ASCII.GetString(inStream12);
                DecodeTelegram12_PimVerification(data);
                Application.DoEvents();

                //3. send Telegam 05 (Ack):
                data = EncodeTelegramPim05_Ack();
                byte[] outStream05 = System.Text.Encoding.ASCII.GetBytes(data);
                serverStream.Write(outStream05, 0, outStream05.Length);
                serverStream.Flush();
                Application.DoEvents();

                //4. Send Telegram 07 (PIM Info Request):
                data = EncodeTelegram07_PimInfoRequest();
                byte[] outStream07 = System.Text.Encoding.ASCII.GetBytes(data);
                serverStream.Write(outStream07, 0, outStream07.Length);
                serverStream.Flush();
                Application.DoEvents();

                //5. get Telegram 08 (PIM Info Telegram):
                serverStream.Read(inStream08, 0, (int)pimClientSocket.ReceiveBufferSize);
                data = System.Text.Encoding.ASCII.GetString(inStream08);

                DecodeTelegram08_PimInfoTelegram(data);
                Application.DoEvents();

                //6. Controller Closes Connection to Server:
                serverStream.Close();
                pimClientSocket.Close();
                txtPimCommunication.Text += "Client Socket Program - Connection Closed...";
            }
            catch (Exception exc)
            {
                txtPimCommunication.Text += Environment.NewLine + Environment.NewLine + "ERROR: " + exc.Message;
            }
        }

        string EncodeTelegram01_SystemDescription()
        {
            var length = "0057";
            var command = "01"; //System description telegram
            identifierSent++;
            var tmpIdentifier = identifierSent.ToString().PadLeft(5, '0');
            var systemType = "0003"; // Open Protocol - Undefined
            var systemNumber = "1234"; //arbitraty;
            var SystemIpAddress = cmbControllerIpAddress.Text.PadLeft(15, ' ');
            var SystemName = "SYSTEM TEST ICT".PadLeft(25, ' ');
            //Fixed: "  ", 2 blank spaces

            var telegram = length + command + tmpIdentifier + systemType + systemNumber + SystemIpAddress + SystemName + "  ";

            txtToolsnetCommunication.Text += "Controller >> Server [01]: length: " + length + ", Command: " + command + ", Identifier: " + tmpIdentifier +
                                        ", System Type: " + systemType + ", System Number: " + systemNumber + ", System IP Address: " + SystemIpAddress +
                                        ", System Name: " + SystemName + " /// Raw Message: " + telegram;
            txtToolsnetCommunication.Text += Environment.NewLine;
            return telegram;
        }

        string EncodeTelegram02_StationDescription()
        {
            var length = "0125";
            var command = "02"; //System description telegram
            identifierSent++;
            var tmpIdentifier = identifierSent.ToString().PadLeft(5, '0');
            var systemType = "0003"; // Open Protocol - Undefined
            var systemNumber = "1234"; //arbitraty;
            var StationNumber = "5678";
            var StationIpAddress = cmbControllerIpAddress.Text.PadLeft(15, ' ');
            var StationName = "STATION TEST ICT".PadLeft(25, ' ');
            var NumberOfSpindles = "0001";
            var SpindleNumber = "0001";
            var SpindleName = "SPINDLE TEST ICT".PadLeft(25, ' ');
            var NumberOfPrograms = "0001";
            var ProgramNumber = "0001";
            var ProgramName = "PROGRAM TEST ICT".PadLeft(25, ' ');

            var telegram = length + command + tmpIdentifier + systemType + systemNumber + StationNumber + StationIpAddress + StationName + NumberOfSpindles +
                           SpindleNumber + SpindleName + NumberOfPrograms + ProgramNumber + ProgramName;

            txtToolsnetCommunication.Text += "Controller >> Server [02]: length: " + length + ", Command: " + command + ", Identifier: " + tmpIdentifier +
                                        ", System Type: " + systemType + ", System Number: " + systemNumber + ", Station Number: " + StationNumber +
                                        ", Controller IP Address: " + StationIpAddress + ", Station Name: " + StationName +
                                        ", N° of Spindles: " + NumberOfSpindles + ", Spindle N°: " + SpindleNumber + ", Spindle Name: " + SpindleName +
                                        ", N° of Programs: " + NumberOfPrograms + ", Program N°: " + ProgramNumber + ", Program Name: " + ProgramName +
                                        " /// Raw Message: " + telegram;
            txtToolsnetCommunication.Text += Environment.NewLine;
            return telegram;
        }

        string EncodeTelegram04_Result()
        {
            var length = "0278"; //80 + 70 * spindle rundown + 28 * Additional Vin + 72 * Additional Parameter
            var command = "04"; //System description telegram
            identifierSent++;
            var tmpIdentifier = identifierSent.ToString().PadLeft(5, '0');
            var lengthGeneralInfo = "80";
            var systemType = "0003"; // Open Protocol - Undefined
            var systemNumber = "1234"; //arbitraty;
            var StationNumber = "5678";
            var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultSequenceNumber++;
            var tmpResultSequenceNumber = resultSequenceNumber.ToString().PadLeft(5, '0');
            var vin = "8AP359MU123456".PadLeft(25, ' ');

            var NumberOfSpindles = "0001";
            var lengthOfSpindleInfo = "70";             //Fixed
            var NumberOfAdditionalVin = "02";
            var lengthOfAdditionalVin = "29";           //Fixed
            var NumberOfAdditionalParameter = "001";
            var lengthOfAdditionalParameter = "72";     //Fixed

            //Spindle
            var spindleNumber = "0001";
            var spindleSerialNumber = "SPINDLEICT".PadLeft(10, ' ');
            var programNumber = "0001";
            var overallStatus = "0"; //0: OK - 1: NOK
            var torqueLowLimit = "00004.00";
            var finalTorque = "00004.95";
            var torqueStatus = "0"; //0: OK - 1: NOK - 2: Low - 3: High
            var torqueHighLimit = "00006.00";
            var angleLowLimit = "000040.0";
            var finalAngle = "000049.5";
            var angleStatus = "0"; //0: OK - 1: NOK - 2: Low - 3: High
            var angleHighLimit = "000060.0";
            var timeStatus = "0"; //0: OK - 1: NOK - 2: Low - 3: High

            //Additional VIN:
            var vinIdentifier1 = "0001";
            var vinNumber1 = "8AP359MU000001".PadLeft(25, ' ');
            var vinIdentifier2 = "0002";
            var vinNumber2 = "8AP359MU000002".PadLeft(25, ' ');

            //Parameter Info
            var parameterSpindleNumber = "0000";
            var parameterProgramNumber = "0000";
            var parameterId = "00001";
            var parameterName = "PARAMETER ICT - TIME".PadLeft(25, ' ');
            var parameterValue = "12.754".PadLeft(25, ' ');
            var parameterType = "2"; //0: String - 1: Integer - 2: Real
            var parameterUnit = "seg".PadLeft(6, ' ');
            var parameterStep = "00";

            var telegram = length + command + tmpIdentifier + lengthGeneralInfo + systemType + systemNumber + StationNumber + currentDateTime +
                           tmpResultSequenceNumber + vin + NumberOfSpindles + lengthOfSpindleInfo + NumberOfAdditionalVin + lengthOfAdditionalVin +
                           NumberOfAdditionalParameter + lengthOfAdditionalParameter +
                           spindleNumber + spindleSerialNumber + programNumber + overallStatus +
                           torqueLowLimit + finalTorque + torqueStatus + torqueHighLimit +
                           angleLowLimit + finalAngle + angleStatus + angleHighLimit + timeStatus +
                           vinIdentifier1 + vinNumber1 + vinIdentifier2 + vinNumber2 +
                           parameterSpindleNumber + parameterProgramNumber + parameterId + parameterName + parameterValue + parameterType + parameterUnit + parameterStep;

            txtToolsnetCommunication.Text += "Controller >> Server [02]: length: " + length + ", Command: " + command + ", Identifier: " + tmpIdentifier +
                                        ", System Type: " + systemType + ", System Number: " + systemNumber + ", Station Number: " + StationNumber +
                                        " [TOO MUCH INFO] /// Raw Message: " + telegram;

            txtToolsnetCommunication.Text += Environment.NewLine;
            return telegram;
        }


        string EncodeTelegramPim05_Ack()
        {
            var length = "0024";
            var command = "05"; //ACK
            //identifierPim
            var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var errorCode = "000"; //No error;

            var telegram = length + command + identifierPim + currentDateTime + errorCode;

            txtPimCommunication.Text += "Controller >> Server [05]: length: " + length + ", Command: " + command + ", Identifier: " + identifierPim +
                                        ", Datetime: " + currentDateTime + ", Error Code: " + errorCode + " /// Raw Message: " + telegram;
            txtPimCommunication.Text += Environment.NewLine;

            return telegram;
        }

        string DecodeTelegramToolsnet05_Ack(string telegram)
        {
            var length = telegram.Substring(0, 4);
            var command = telegram.Substring(4, 2);
            identifierPim = telegram.Substring(6, 5);
            var currentDateTime = telegram.Substring(11, 14);
            var errorCode = telegram.Substring(25, 3);


            txtToolsnetCommunication.Text += "Server >> Controller [05]: length: " + length + ", Command: " + command + ", Identifier: " + identifierPim +
                                        ", Datetime: " + currentDateTime + ", Error Code: " + errorCode + " /// Raw Message: " + telegram;
            txtToolsnetCommunication.Text += Environment.NewLine;

            return telegram;
        }

        string EncodeTelegram07_PimInfoRequest()
        {
            var length = "0019";
            var command = "07"; //Pim Info Request
            identifierSent++;
            var tmpIdentifier = identifierSent.ToString().PadLeft(5, '0');
            var systemType = "0003"; // Open Protocol - Undefined
            var systemNumber = "1234"; //arbitraty;
            var stationNumber = "5678"; //arbitraty;

            var telegram = length + command + tmpIdentifier + systemType + systemNumber + stationNumber;
            txtPimCommunication.Text += "Controller >> Server [07]: length: " + length + ", Command: " + command + ", Identifier: " + tmpIdentifier +
                                        ", System Type: " + systemType + ", System Number: " + systemNumber + ", Station Number: " + stationNumber +
                                        " /// Raw Message: " + telegram + Environment.NewLine;
            return telegram;
        }
        void DecodeTelegram08_PimInfoTelegram(string telegram)
        {
            var length = telegram.Substring(0, 4);
            var command = telegram.Substring(4, 2);
            identifierPim = telegram.Substring(6, 5);
            var tmpResultSequenceNumber = telegram.Substring(11, 5);
            resultSequenceNumber = int.Parse(tmpResultSequenceNumber);
            var tmpEventSequenceNumber = telegram.Substring(16, 5);
            eventSequenceNumber = int.Parse(tmpEventSequenceNumber);
            toolsnetServerIp = telegram.Substring(21, 15);
            txtToolsnetIpAdress.Text = toolsnetServerIp;
            toolsnetServerPort = telegram.Substring(36, 5);
            txtToolsnetPortNumber.Text = int.Parse(toolsnetServerPort).ToString();

            txtPimCommunication.Text += "Server >> Controller [08]: length: " + length + ", Command: " + command + ", Identifier: " + identifierPim +
                                        ", result Seq. Number: " + resultSequenceNumber + ", Event Seq. NUmber: " + eventSequenceNumber +
                                        ", ToolsNet IP:Port: " + toolsnetServerIp + ":" + toolsnetServerPort + " /// Raw Message: " + telegram;
            txtPimCommunication.Text += Environment.NewLine;
        }


        string EncodeTelegram09_ErrorEvent()
        {
            var length = "0096"; //48 + 24 * N° of Event Parameters
            var command = "09"; //System description telegram
            identifierSent++;
            var tmpIdentifier = identifierSent.ToString().PadLeft(5, '0');
            var systemType = "0003"; // Open Protocol - Undefined
            var systemNumber = "1234"; //arbitraty;
            var StationNumber = "5678"; //arbitraty;
            var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            eventSequenceNumber++;
            var tmpEventSequenceNumber = eventSequenceNumber.ToString().PadLeft(5, '0');
            var errorCode = "00139"; //0-99999 ?
            var eventLevel = "010"; //0-999 ?
            var numberOfEventParameters = "01"; //1-99

            var eventParameterId1 = "123";
            var eventParameterValueType1 = "5"; //1-9 ?
            var eventParameterValue1 = "EVENT PARAM ICT 1".PadLeft(20, ' ');
            
            var eventParameterId2 = "321";
            var eventParameterValueType2 = "1"; //1-9 ?
            var eventParameterValue2 = "EVENT PARAM ICT 2".PadLeft(20, ' ');

            var telegram = length + command + tmpIdentifier + systemType + systemNumber + StationNumber + currentDateTime +
                           tmpEventSequenceNumber + errorCode + eventLevel + numberOfEventParameters +
                           eventParameterId1 + eventParameterValueType1 + eventParameterValue1 +
                           eventParameterId2 + eventParameterValueType2 + eventParameterValue2;

            txtToolsnetCommunication.Text += "Controller >> Server [02]: length: " + length + ", Command: " + command + ", Identifier: " + tmpIdentifier +
                                        ", System Type: " + systemType + ", System Number: " + systemNumber + ", Station Number: " + StationNumber +
                                        " [TOO MUCH INFO] /// Raw Message: " + telegram;

            txtToolsnetCommunication.Text += Environment.NewLine;
            return telegram;
        }


        void DecodeTelegram12_PimVerification(string telegram)
        {
            var length = telegram.Substring(0, 4);
            var command = telegram.Substring(4, 2);
            identifierPim = telegram.Substring(6, 5);

            txtPimCommunication.Text += "Server >> Controller [12]: length: " + length + ", Command: " + command + ", Identifier: " + identifierPim +
                                        " /// Raw Message: " + telegram;
            txtPimCommunication.Text += Environment.NewLine;
        }

        private void btnToolsnetOpenCloseConnection_Click(object sender, EventArgs e)
        {
            if (cmbControllerIpAddress.SelectedIndex == 0)
            {
                MessageBox.Show("Set Controller IP address");
                cmbControllerIpAddress.Focus();
                return;
            }

            string data;
            txtToolsnetCommunication.Text = "";
            if (chkSimulateControllerMessages.Checked)
            {
                data = EncodeTelegram01_SystemDescription();
                data = EncodeTelegram02_StationDescription();
                data = EncodeTelegram04_Result();
                data = EncodeTelegram09_ErrorEvent();
            }
            else
            {
                try
                {
                    toolsnetClientSocket = new System.Net.Sockets.TcpClient();

                    //1. Controller connects to ToolsNet
                    toolsnetClientSocket.Connect(txtToolsnetIpAdress.Text.Trim(), int.Parse(txtToolsnetPortNumber.Text));
                    NetworkStream serverStream = toolsnetClientSocket.GetStream();
                    txtToolsnetCommunication.Text = "Client Socket Program - Server Connected ..." + Environment.NewLine;
                    Application.DoEvents();

                    //2. send Telegam 01 (System Description):
                    data = EncodeTelegram01_SystemDescription();
                    byte[] outStream01 = System.Text.Encoding.ASCII.GetBytes(data);
                    serverStream.Write(outStream01, 0, outStream01.Length);
                    serverStream.Flush();
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    //3. get Telegram 05 (Ack):
                    byte[] inStream05_1 = new byte[65537];
                    serverStream.Read(inStream05_1, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                    data = System.Text.Encoding.ASCII.GetString(inStream05_1);
                    DecodeTelegramToolsnet05_Ack(data);
                    Application.DoEvents();

                    //4. Send Telegram 02 (Station Description):
                    data = EncodeTelegram02_StationDescription();
                    byte[] outStream02 = System.Text.Encoding.ASCII.GetBytes(data);
                    serverStream.Write(outStream02, 0, outStream02.Length);
                    serverStream.Flush();
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    //5. get Telegram 05 (Ack):
                    byte[] inStream05_2 = new byte[65537];
                    serverStream.Read(inStream05_2, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                    data = System.Text.Encoding.ASCII.GetString(inStream05_2);
                    DecodeTelegramToolsnet05_Ack(data);
                    Application.DoEvents();

                    /*
                    //6. Send Telegram 04 (Result):
                    data = EncodeTelegram04_Result();
                    byte[] outStream04 = System.Text.Encoding.ASCII.GetBytes(data);
                    serverStream.Write(outStream04, 0, outStream04.Length);
                    serverStream.Flush();
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    //7. get Telegram 05 (Ack):
                    byte[] inStream05_3 = new byte[65537];
                    serverStream.Read(inStream05_3, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                    data = System.Text.Encoding.ASCII.GetString(inStream05_3);
                    DecodeTelegramToolsnet05_Ack(data);
                    Application.DoEvents();
                    */

                    //8. Send Telegram 09 (Error Event):
                    data = EncodeTelegram09_ErrorEvent();
                    byte[] outStream08 = System.Text.Encoding.ASCII.GetBytes(data);
                    serverStream.Write(outStream08, 0, outStream08.Length);
                    serverStream.Flush();
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    //9. get Telegram 05 (Ack):
                    byte[] inStream05_4 = new byte[65537];
                    serverStream.Read(inStream05_4, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                    data = System.Text.Encoding.ASCII.GetString(inStream05_4);
                    DecodeTelegramToolsnet05_Ack(data);
                    Application.DoEvents();

                    //10. Controller Closes Connection to Server:
                    serverStream.Close();
                    toolsnetClientSocket.Close();
                    txtToolsnetCommunication.Text += "Client Socket Program - Connection Closed...";
                }
                catch (Exception exc)
                {
                    txtToolsnetCommunication.Text += Environment.NewLine + Environment.NewLine + "ERROR: " + exc.Message;
                }
            }
        }
    }
}