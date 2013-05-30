using System;
using System.Threading;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.FEZ;


namespace LuxPlugOnPanda
{

public class Program
  {
  
  static TSL2561 LuxPlug = new TSL2561(0x39, 400);

  public static void Main()
  {
    while (true)
    {
      Debug.Print("--------------");
      LuxPlug.ReadSensor();

      //Debug.Print("Lux = " + LuxPlug.mbar.ToString);

      Thread.Sleep(5000);
    }
  }

  }
}
