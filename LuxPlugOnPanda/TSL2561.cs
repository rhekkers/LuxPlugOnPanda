using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;


namespace LuxPlugOnPanda
{

  public class TSL2561
  {
    private static I2CDevice.Configuration I2CConfig;
    private static I2CDevice I2C;

    public TSL2561(byte I2CAddress, int ClockinKHz)
    {
      I2CConfig = new I2CDevice.Configuration(I2CAddress, ClockinKHz);
      I2C = new I2CDevice(I2CConfig);

      // read the ID register.
      var Actions = new I2CDevice.I2CTransaction[2];
      byte[] rx = new byte[1];
      Actions[0] = I2CDevice.CreateWriteTransaction(new byte[] { 0x0a });
      Actions[1] = I2CDevice.CreateReadTransaction(rx);
      if (I2C.Execute(Actions, 1000) == 0)
      {
        Debug.Print("Read ID Register failed");
        // exit or something
      }
      else
      {
        Debug.Print("ID value: " + rx[0].ToString());
      }
      // 4 msb must be 0001 for a TSL2561
      if ((rx[0] & 0xf0) != 0xa0)
      {
        // exit or something
      }

      setGain(0x10);
      Thread.Sleep(5);   // Mandatory after each Write transaction !!!
    }

    private void write(byte reg, byte value)
    {
      Debug.Print("> " + reg.ToString() + " " + value.ToString());
      var Actions = new I2CDevice.I2CTransaction[1];
      Actions[0] = I2CDevice.CreateWriteTransaction(new byte[] { reg, value });
      I2C.Execute(Actions, 1000);
      Thread.Sleep(5);   // Mandatory after each Write transaction !!!
    }

    ushort read16(byte reg)
    {
      Debug.Print("> " + reg.ToString());
      var Actions = new I2CDevice.I2CTransaction[2];
      byte[] rx = new byte[2];

      Actions[0] = I2CDevice.CreateWriteTransaction(new byte[] { reg });
      Actions[1] = I2CDevice.CreateReadTransaction(rx);
      if (I2C.Execute(Actions, 1000) == 0)
      {
        Debug.Print("Failed to perform read16");
      }
      else
      {
        Debug.Print("Result: " + rx[0].ToString() + " " + rx[1].ToString());
      }
      return (ushort)((ushort)(rx[0] * 256) + (ushort)rx[1]);
    }


    private void powerOn()
    {
      write(0x80, 0x03);
    }
    private void powerOff()
    {
      write(0x80, 0x00);
    }

    public void setGain(byte gain)
    {
      powerOn();
      write(0x81, gain);
      powerOff();
    }
    public void setTiming(byte integration)
    {
      powerOn();
      write(0x81, integration);
      powerOff();
    }


    /// <summary>
    /// Reads the specified address.
    /// </summary>
    /// <param name="Address">The address to be read</param>
    /// <returns>One byte from the EEprom</returns>
    public void ReadSensor()
    {
      powerOn();
      Thread.Sleep(14);
      read16(0xAE);
      read16(0xAC);
      powerOff();
    }
  }
}
