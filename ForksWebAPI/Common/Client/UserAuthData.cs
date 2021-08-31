
using System;

namespace ForksWebAPI.Common.Client
{
  [Serializable]
  public class UserAuthData
  {
    public string Login { get; set; }

    public PositiveSubscribeInfo SubscribeInfo { get; set; }
  }
}
