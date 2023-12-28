using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using MyTest.Module.BusinessObjects.CRM;
using MyTest.Module.BusinessObjects.Product;
using MyTest.Module.BusinessObjects.Production;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Text;


namespace MyTest.Win.Controllers
{
    public partial class BarcodeReaderDetailViewController : ViewController<ListView>
    {
        private readonly List<SerialPort> serials = new();
        ElementToProductionTask current = null;
        XPObjectSpaceProvider securedObjectSpaceProvider;
        IObjectSpace unsecuredObjectSpace;
        ProductionTask task;
        public BarcodeReaderDetailViewController()
        { }

        protected override void OnActivated()
        {
            base.OnActivated();
            securedObjectSpaceProvider = new XPObjectSpaceProvider(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString, null);
            unsecuredObjectSpace = securedObjectSpaceProvider.CreateObjectSpace();
            task = View?.CurrentObject as ProductionTask;
            int tempBaudRate = 57600;
            var baudRate = ConfigurationManager.AppSettings["BaudRate"];
            if (!string.IsNullOrWhiteSpace(baudRate))
                int.TryParse(baudRate, out tempBaudRate);

            var comReader = ConfigurationManager.AppSettings["COMPortName"];

            var comNames = SerialPort.GetPortNames()?.OrderByDescending(x => x);
            foreach (var comName in comNames)
            {
                //if (comReader.Equals(comName, StringComparison.OrdinalIgnoreCase))
                //{

                SerialPort serial = new();
                serial.PortName = comName;
                serial.BaudRate = tempBaudRate;
                serial.Handshake = Handshake.None;
                serial.Parity = Parity.None;
                serial.DataBits = 8;
                serial.StopBits = StopBits.One;
                serial.ReadTimeout = 3000;
                //serial.ReadTimeout = 0;
                serial.WriteTimeout = 50;
                try
                {
                    serial.Open();
                    serial.DataReceived += Serial_DataReceived;

                    serials.Add(serial);
                }
                catch (Exception) { }
                serials.Add(serial);
                //}
            }

            if (!serials.Any())
                XtraMessageBox.Show("Nie udało się połączyć z portem", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var received_data = (sender as SerialPort).ReadExisting();
            received_data = received_data.Replace("\r", "");
            if (DeserializeSystemCode(received_data, unsecuredObjectSpace) is ElementToProductionTask etpt)
            {
                etpt.Completed = true;
                current = etpt;
            }
            else if (DeserializeSystemCode(received_data, unsecuredObjectSpace) is Person person && current != null)
            {
                current.Employee = person; current = null;
            }
            unsecuredObjectSpace.CommitChanges();
        }
        public static Object DeserializeSystemCode(string code, IObjectSpace space)
        {
            if (!string.IsNullOrWhiteSpace(code) && code.StartsWith('!'))
            {
                int result = 0;
                int result2 = 0;
                int num = code.IndexOfAny(new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                if (num >= 0)
                {
                    int num2 = code.IndexOf('-');
                    if (num2 > num && int.TryParse(code.Substring(num, num2 - num), out result))
                    {
                        XPObjectType objectByKey = space.GetObjectByKey<XPObjectType>(result);
                        if (int.TryParse(code.Substring(num2 + 1, code.Length - 1 - num2), out result2))
                        {
                            return space.GetObjectByKey(objectByKey.SystemType, result2);
                        }
                    }
                }
            }

            return null;
        }
    }
}
