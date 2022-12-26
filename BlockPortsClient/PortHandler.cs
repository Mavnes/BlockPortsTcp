using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPortsClient
{
    internal class PortHandler
    {
        bool USB = false;
        bool COM = false;
        bool LPT = false;
        bool CD_DVD = false;
        bool Floppy = false;

        public PortHandler()
        {
            getFromRegistry();
        }

        public string getPorts()
        {
            string data = "";

            data += Convert.ToInt32(USB);
            data += Convert.ToInt32(COM);
            data += Convert.ToInt32(LPT);
            data += Convert.ToInt32(CD_DVD);
            data += Convert.ToInt32(Floppy);

            return data;
        }

        public void setPorts(string data)
        {
            if (data[0] == '1')
                USB = true;
            else
                USB = false;

            if (data[1] == '1')
                COM = true;
            else
                COM = false;

            if (data[2] == '1')
                LPT = true;
            else
                LPT = false;

            if (data[3] == '1')
                CD_DVD = true;
            else
                CD_DVD = false;

            if (data[4] == '1')
                Floppy = true;
            else
                Floppy = false;

            setInRegistry();
        }

        public void getFromRegistry()
        {
            RegistryKey keyUSB = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\USBSTOR", true);
            if (keyUSB != null)
            {
                int value = 4;
                try {
                    value = (int)keyUSB.GetValue("Start");
                }
                catch {
                    keyUSB.SetValue("Start", 4);
                }
                keyUSB.Close();
                if (value == 3)
                    USB = true;
                else
                    USB = false;
            }
            else
            {
                USB = false;
            }

            RegistryKey keyCOM = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM", true);
            if (keyCOM != null)
            {
                int value = 4;
                try {
                    value = (int)keyCOM.GetValue("Start");
                }
                catch {
                    keyCOM.SetValue("Start", 4);
                }
                keyCOM.Close();
                if (value == 3)
                    COM = true;
                else
                    COM = false;
            }
            else
            {
                COM = false;
            }

            RegistryKey keyLPT = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\PARALLEL PORTS", true);
            if (keyLPT != null)
            {
                int value = 4;
                try {
                    value = (int)keyLPT.GetValue("Start");
                }
                catch {
                    keyLPT.SetValue("Start", 4);
                }
                keyLPT.Close();
                if (value == 3)
                    LPT = true;
                else
                    LPT = false;
            }
            else
            {
                LPT = false;
            }

            RegistryKey keyCD_DVD = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\cdrom", true);
            if (keyCD_DVD != null)
            {
                int value = 4;
                try {
                    value = (int)keyCD_DVD.GetValue("Start");
                }
                catch {
                    keyCD_DVD.SetValue("Start", 4);
                }
                keyCD_DVD.Close();
                if (value == 3)
                    CD_DVD = true;
                else
                    CD_DVD = false;
            }
            else
            {
                CD_DVD = false;
            }

            RegistryKey keyFloppy = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\flpydisk", true);
            if (keyFloppy != null)
            {
                int value = 4;
                try {
                    value = (int)keyFloppy.GetValue("Start");
                }
                catch {
                    keyFloppy.SetValue("Start", 4);
                }
                keyFloppy.Close();
                if (value == 3)
                    Floppy = true;
                else
                    Floppy = false;
            }
            else
            {
                Floppy = false;
            }
        }

        public void setInRegistry()
        {
            RegistryKey keyUSB = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\USBSTOR", true);
            if (keyUSB != null)
            {
                if (USB)
                    keyUSB.SetValue("Start", 3);
                else
                    keyUSB.SetValue("Start", 4);

                keyUSB.Close();
            }

            RegistryKey keyCOM = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM", true);
            if (keyCOM != null)
            {
                if (COM)
                    keyCOM.SetValue("Start", 3);
                else
                    keyCOM.SetValue("Start", 4);

                keyCOM.Close();
            }

            RegistryKey keyLPT = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\PARALLEL PORTS", true);
            if (keyLPT != null)
            {
                if (LPT)
                    keyLPT.SetValue("Start", 3);
                else
                    keyLPT.SetValue("Start", 4);

                keyLPT.Close();
            }

            RegistryKey keyCD_DVD = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\cdrom", true);
            if (keyCD_DVD != null)
            {
                if (CD_DVD)
                    keyCD_DVD.SetValue("Start", 3);
                else
                    keyCD_DVD.SetValue("Start", 4);

                keyCD_DVD.Close();
            }

            RegistryKey keyFloppy = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\flpydisk", true);
            if (keyFloppy != null)
            {
                if (Floppy)
                    keyFloppy.SetValue("Start", 3);
                else
                    keyFloppy.SetValue("Start", 4);

                keyFloppy.Close();
            }
        }

        public string PortsToString()
        {
            string data = "+----------------+\n";
            data += "Port settings: ";

            data += "\n1. USB: ";
            if (USB)
                data += "ON";
            else
                data += "OFF";

            data += "\n2. COM: ";
            if (COM)
                data += "ON";
            else
                data += "OFF";

            data += "\n3. LPT: ";
            if (LPT)
                data += "ON";
            else
                data += "OFF";

            data += "\n4. CD/DVD: ";
            if (CD_DVD)
                data += "ON";
            else
                data += "OFF";

            data += "\n5. Floppy: ";
            if (Floppy)
                data += "ON";
            else
                data += "OFF";

            data += "\n+----------------+";
            return data;
        }
    }
}
