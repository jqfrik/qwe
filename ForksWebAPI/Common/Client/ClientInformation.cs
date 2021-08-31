
using System;
using System.Linq;

namespace ForksWebAPI.Common.Client
{
  [Serializable]
  public class ClientInformation
  {
    public string ProcessorId { get; set; }

    public int ProcessorCount { get; set; }

    public string MotherBoard { get; set; }

    public string KeyBoard { get; set; }

    public string OperatingSystem { get; set; }

    public string Mouse { get; set; }

    public string ComputerSystemProduct { get; set; }

    public string OsUserName { get; set; }

    public string PcName { get; set; }

    public string HddSerial { get; set; }

  }
}
