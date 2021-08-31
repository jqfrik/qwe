

using System;
using System.Collections.Generic;

namespace ForksWebAPI.Common.Client
{
  [Serializable]
  public class PositiveSubscribeInfo
  {
    public List<Subscribe> Subscribes { get; set; }

    public DateTimeOffset SubscribeEndTime { get; set; }

    public bool IsFreeze { get; set; }

    public bool IsActive { get; set; }

    public TimeSpan EndFreezeTime { get; set; }

    public static PositiveSubscribeInfo Default
    {
      get
      {
        return new PositiveSubscribeInfo()
        {
          Subscribes = new List<Subscribe>(),
          EndFreezeTime = TimeSpan.Zero
        };
      }
    }
  }
}
