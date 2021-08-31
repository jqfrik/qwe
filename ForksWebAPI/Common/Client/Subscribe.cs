

using System;
using System.Diagnostics;

namespace ForksWebAPI.Common.Client
{
  [DebuggerDisplay("N:{Name} - {State} {IsCurrent} F:{FreezeCount} L:{LeftTime} P:{PayDateTime}")]
  [Serializable]
  public class Subscribe
  {
    private DateTime _createdTime;

    public Subscribe()
    {
      this._createdTime = DateTime.UtcNow;
    }

    public string Name { get; set; }

    public bool IsCurrent { get; set; }

    public ESubscribeState State { get; set; }

    public DateTime PayDateTime { get; set; }

    public DateTime EndTime { get; set; }

    public TimeSpan LeftTime { get; set; }

    public int FreezeCount { get; set; }

    public TimeSpan LocalCalcTimeLeft()
    {
      if (this.State != ESubscribeState.Active)
        return TimeSpan.Zero;
      return this._createdTime.Add(this.LeftTime) - DateTime.UtcNow;
    }
  }
}
