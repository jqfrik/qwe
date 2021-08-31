using System;
using System.Collections.Generic;

namespace ForksWebAPI.Common.Client
{
  [Serializable]
  public class DublicateList : List<KeyValuePair<string, string>>
  {
    public void Add(string key, string value)
    {
      this.Add(new KeyValuePair<string, string>(key, value));
    }

    public string this[string data]
    {
      set
      {
        this.Add(new KeyValuePair<string, string>(data, value));
      }
    }
  }
}
