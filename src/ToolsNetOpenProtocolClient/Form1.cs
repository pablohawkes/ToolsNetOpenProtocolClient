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

        private void btnToolsnetOpenCloseConnection_Click(object sender, EventArgs e)
        {
            if (cmbControllerIpAddress.SelectedIndex == 0)
            {
                MessageBox.Show("Set Controller IP address");
                cmbControllerIpAddress.Focus();
                return;
            }

            string data = "";
            txtToolsnetCommunication.Text = "";
            if (chkSimulateControllerMessages.Checked)
            {
                data = EncodeTelegram01_SystemDescription();
                data = EncodeTelegram02_StationDescription();
                data = EncodeTelegram04_Result();
                data = EncodeTelegram09_ErrorEvent();
                data = EncodeTelegram13_Graph();
                data = EncodeTelegram06_KeepAlive();
            }
            else
            {
                try
                {
                    toolsnetClientSocket = new System.Net.Sockets.TcpClient();
                    byte[] inStream = new byte[65537];

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
                    serverStream.Read(inStream, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                    data = System.Text.Encoding.ASCII.GetString(inStream);
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
                    serverStream.Read(inStream, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                    data = System.Text.Encoding.ASCII.GetString(inStream);
                    DecodeTelegramToolsnet05_Ack(data);
                    Application.DoEvents();

                    //6. Send Telegram 04 (Result):
                    data = EncodeTelegram04_Result();
                    byte[] outStream04 = System.Text.Encoding.ASCII.GetBytes(data);
                    serverStream.Write(outStream04, 0, outStream04.Length);
                    serverStream.Flush();
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    //7. get Telegram 05 (Ack):
                    serverStream.Read(inStream, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                    data = System.Text.Encoding.ASCII.GetString(inStream);
                    DecodeTelegramToolsnet05_Ack(data);
                    Application.DoEvents();

                    /*
                    //ToolsNet Closes connection when you send 09 after 04
                    //8. Send Telegram 09 (Error Event):
                    data = EncodeTelegram09_ErrorEvent();
                    byte[] outStream08 = System.Text.Encoding.ASCII.GetBytes(data);
                    serverStream.Write(outStream08, 0, outStream08.Length);
                    serverStream.Flush();
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    //9. get Telegram 05 (Ack):
                    serverStream.Read(inStream, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                    data = System.Text.Encoding.ASCII.GetString(inStream);
                    DecodeTelegramToolsnet05_Ack(data);
                    Application.DoEvents();
                    */
                    //test KeepAlive 3 times, every 5 seconds:
                    for (int i = 0; i < 3; i++)
                    {
                        //10. Send Telegram 06 (KeepAlive):
                        data = EncodeTelegram06_KeepAlive();
                        byte[] outStream06 = System.Text.Encoding.ASCII.GetBytes(data);
                        serverStream.Write(outStream06, 0, outStream06.Length);
                        serverStream.Flush();
                        Application.DoEvents();
                        Thread.Sleep(1000);

                        //11. get Telegram 05 (Ack):
                        serverStream.Read(inStream, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                        data = System.Text.Encoding.ASCII.GetString(inStream);
                        DecodeTelegramToolsnet05_Ack(data);
                        Application.DoEvents();

                        Thread.Sleep(5000);
                    }

                    /* NOT WORKS:
                    //12. Send Telegram 13 (Graph):
                    data = EncodeTelegram13_Graph();
                    byte[] outStream13 = System.Text.Encoding.ASCII.GetBytes(data);
                    serverStream.Write(outStream13, 0, outStream13.Length);
                    serverStream.Flush();
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    //13. get Telegram 05 (Ack):
                    serverStream.Read(inStream, 0, (int)toolsnetClientSocket.ReceiveBufferSize);
                    data = System.Text.Encoding.ASCII.GetString(inStream);
                    DecodeTelegramToolsnet05_Ack(data);
                    Application.DoEvents();
                    */

                    //14. Controller Closes Connection to Server:
                    serverStream.Close();
                    toolsnetClientSocket.Close();
                    txtToolsnetCommunication.Text += "Client Socket Program - Connection Closed...";
                }
                catch (Exception exc)
                {
                    txtToolsnetCommunication.Text += Environment.NewLine + "ERROR - Telegram: " + data + " - Message: " + exc.Message;
                }
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
            var overallStatus = "1"; //0: OK - 1: NOK
            var torqueLowLimit = "00009.00";
            var finalTorque = "00008.00";
            var torqueStatus = "3"; //0: OK - 1: NOK - 2: Low - 3: High
            var torqueHighLimit = "00010.00";
            var angleLowLimit = "000050.0";
            var finalAngle = "000049.5";
            var angleStatus = "2"; //0: OK - 1: NOK - 2: Low - 3: High
            var angleHighLimit = "000070.0";
            var timeStatus = "1"; //0: OK - 1: NOK - 2: Low - 3: High

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
            var parameterValue = "12.754".PadLeft(25, ' '); //ToolsNet seems to show only 2 decimals on Real >> Sent 12.754, Shown 12.75
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
                                        " [OTHER INFO] /// Raw Message: " + telegram;

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

        string EncodeTelegram06_KeepAlive()
        {
            var length = "0007";
            var command = "06"; //Pim Info Request
            identifierSent++;
            var tmpIdentifier = identifierSent.ToString().PadLeft(5, '0');

            var telegram = length + command + tmpIdentifier;
            txtToolsnetCommunication.Text += "Controller >> Server [06]: length: " + length + ", Command: " + command + ", Identifier: " + tmpIdentifier +
                                        " /// Raw Message: " + telegram;
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
                                        ", result Seq. Number: " + resultSequenceNumber + ", Event Seq. Number: " + eventSequenceNumber +
                                        ", ToolsNet IP:Port: " + toolsnetServerIp + ":" + toolsnetServerPort + " /// Raw Message: " + telegram;
            txtPimCommunication.Text += Environment.NewLine;
        }

        string EncodeTelegram09_ErrorEvent()
        {
            var length = "0048"; //48 + 24 * N° of Event Parameters
            var command = "09"; //System description telegram
            identifierSent++;
            var tmpIdentifier = identifierSent.ToString().PadLeft(5, '0');
            var systemType = "0003"; // Open Protocol - Undefined
            var systemNumber = "1234"; //arbitraty;
            var StationNumber = "5678"; //arbitraty;
            var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            eventSequenceNumber++;
            var tmpEventSequenceNumber = eventSequenceNumber.ToString().PadLeft(5, '0');
            var errorCode = "00139"; //0-99999 >>>> codes defined on toolsnet database?
            var eventLevel = "001"; //0-999 >>>> codes defined on toolsnet database? >>>> Error / Info / Warning / others?
            var numberOfEventParameters = "00"; //1-99
            /*
            var eventParameterId1 = "002"; //001 and 003 aren't saved on database, why?
            var eventParameterValueType1 = "2"; //1-9 ?
            var eventParameterValue1 = "EVENT PARAM ICT 2".PadLeft(20, ' ');
            var eventParameterId2 = "004";
            var eventParameterValueType2 = "4"; //1-9 ?
            var eventParameterValue2 = "EVENT PARAM ICT 4".PadLeft(20, ' ');
            */
            var telegram = length + command + tmpIdentifier + systemType + systemNumber + StationNumber + currentDateTime +
                           tmpEventSequenceNumber + errorCode + eventLevel + numberOfEventParameters; // +
                           //eventParameterId1 + eventParameterValueType1 + eventParameterValue1 +
                           //eventParameterId2 + eventParameterValueType2 + eventParameterValue2;

            txtToolsnetCommunication.Text += "Controller >> Server [02]: length: " + length + ", Command: " + command + ", Identifier: " + tmpIdentifier +
                                        ", System Type: " + systemType + ", System Number: " + systemNumber + ", Station Number: " + StationNumber +
                                        " [OTHER INFO] /// Raw Message: " + telegram;

            txtToolsnetCommunication.Text += Environment.NewLine;
            return telegram;
        }

        //TODO: Error Event Acknowledge telegram - '10'

        //TODO: PIM FSH Info Request telegram - '11'

        void DecodeTelegram12_PimVerification(string telegram)
        {
            var length = telegram.Substring(0, 4);
            var command = telegram.Substring(4, 2);
            identifierPim = telegram.Substring(6, 5);

            txtPimCommunication.Text += "Server >> Controller [12]: length: " + length + ", Command: " + command + ", Identifier: " + identifierPim +
                                        " /// Raw Message: " + telegram;
            txtPimCommunication.Text += Environment.NewLine;
        }

        //TELEGRAM 13 - doesn't work: 
        //When you send message after telegam 04 (result), server closes connection.
        //When you send message without telegam 04 (result), server accepts message but graph is not stored or shown in ToolsNet.
        string EncodeTelegram13_Graph()
        {
            var length = "0126"; //116 + 2 * [dataQty(16bits)]
            var command = "13"; //System description telegram
            identifierSent++;
            var tmpIdentifier = identifierSent.ToString().PadLeft(5, '0');
            var lengthOfGeneralInfo = "116"; //48 + 24 * N° of Event Parameters
            var systemType = "0003"; // Open Protocol - Undefined
            var systemNumber = "1234"; //arbitraty;
            var stationNumber = "5678"; //arbitraty;
            var spindleNumber = "0001";
            var programNumber = "0001";
            var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultSequenceNumber++;
            var tmpEventSequenceNumber = eventSequenceNumber.ToString().PadLeft(5, '0');
            var graphType = "0"; //0: Torque - 1: Angle
            var bitShift = "0000000001"; // BitShift for trace as signed integer ???
            var scaleFactorDom = "0000000001"; // Scale Factor Dom for trace as signed integer ???
            var scaleFactorNom = "0000000001"; // Scale Factor Nom for trace as signed integer ???
            var minLimit = "000001.0"; // Units depends on GraphType
            var maxLimit = "000100.0"; // Units depends on GraphType
            var angleOffset = "000000.0"; //degree
            var sampleTime = "001000.0"; //Time in ms between samples in Trace Data part
            var lengthOfTraceData = "0010"; // 0-8000, Length in bytes
            //Trace Data is a packed Array of signed 16bit integers in network order.
            // [Scale Factor] * [16 bit value] = actual float value
            //Scale factor Calculation
            //Scale factor = 2^[bitshift]* [Scale Factor Dom] / [Scale factor Nom]
            var traceData = "0001020408"; //1,2,3,4,5

            var telegram = length + command + tmpIdentifier + lengthOfGeneralInfo + systemType + systemNumber + stationNumber + spindleNumber +
                            programNumber + currentDateTime + tmpEventSequenceNumber + graphType + bitShift + scaleFactorDom + scaleFactorNom +
                            minLimit + maxLimit + angleOffset + sampleTime + lengthOfTraceData + traceData;

            txtToolsnetCommunication.Text += "Controller >> Server [02]: length: " + length + ", Command: " + command + ", Identifier: " + tmpIdentifier +
                                        ", System Type: " + systemType + ", System Number: " + systemNumber + ", Station Number: " + stationNumber +
                                        " [OTHER INFO] /// Raw Message: " + telegram;

            txtToolsnetCommunication.Text += Environment.NewLine;
            return telegram;
        }
    }
}