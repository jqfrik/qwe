using System;

namespace ForksWebAPI.Common.Client
{
  [Flags]
  [Serializable]
  public enum ESubscribeState
  {
    End = 1,
    Active = 2,
    Freeze = 4,
    New = 8,
    TestDrive = 16, // 0x00000010
  }
}
