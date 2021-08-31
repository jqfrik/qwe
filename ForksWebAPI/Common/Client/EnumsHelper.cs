
using System;
using System.Collections.Generic;

namespace ForksWebAPI.Common.Client
{
  public static class EnumsHelper
  {
    private static readonly Dictionary<ESport, Dictionary<EBetType, int>> _correctSportValue = new Dictionary<ESport, Dictionary<EBetType, int>>()
    {
      {
        ESport.Football,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            1
          },
          {
            EBetType.Total,
            3
          },
          {
            EBetType.IndTotal,
            5
          },
          {
            EBetType.Fora,
            7
          },
          {
            EBetType.Corner,
            9
          }
        }
      },
      {
        ESport.Tennis,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            2
          },
          {
            EBetType.Total,
            4
          },
          {
            EBetType.IndTotal,
            6
          },
          {
            EBetType.Fora,
            8
          },
          {
            EBetType.WinInGame,
            22
          },
          {
            EBetType.WinInSet,
            24
          },
          {
            EBetType.WinInCurrentGame,
            26
          }
        }
      },
      {
        ESport.Hockey,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            10
          },
          {
            EBetType.Total,
            11
          },
          {
            EBetType.IndTotal,
            12
          },
          {
            EBetType.Fora,
            13
          }
        }
      },
      {
        ESport.Basketball,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            14
          },
          {
            EBetType.Total,
            15
          },
          {
            EBetType.IndTotal,
            16
          },
          {
            EBetType.Fora,
            17
          }
        }
      },
      {
        ESport.VoleyBall,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            18
          },
          {
            EBetType.Total,
            19
          },
          {
            EBetType.IndTotal,
            20
          },
          {
            EBetType.Fora,
            21
          },
          {
            EBetType.WinPoints,
            23
          },
          {
            EBetType.WinInSet,
            25
          }
        }
      },
      {
        ESport.Handball,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            27
          },
          {
            EBetType.Total,
            28
          },
          {
            EBetType.IndTotal,
            29
          },
          {
            EBetType.Fora,
            30
          },
          {
            EBetType.Goals,
            31
          }
        }
      },
      {
        ESport.TableTennis,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            32
          },
          {
            EBetType.Total,
            33
          },
          {
            EBetType.IndTotal,
            34
          },
          {
            EBetType.Fora,
            35
          },
          {
            EBetType.WinPoints,
            36
          }
        }
      },
      {
        ESport.Futsal,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            37
          },
          {
            EBetType.Total,
            38
          },
          {
            EBetType.IndTotal,
            39
          },
          {
            EBetType.Fora,
            40
          }
        }
      },
      {
        ESport.Badminton,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            41
          },
          {
            EBetType.Total,
            42
          },
          {
            EBetType.IndTotal,
            43
          },
          {
            EBetType.Fora,
            44
          },
          {
            EBetType.WinPoints,
            45
          }
        }
      },
      {
        ESport.Baseball,
        new Dictionary<EBetType, int>()
        {
          {
            EBetType.Win,
            46
          },
          {
            EBetType.Total,
            47
          },
          {
            EBetType.IndTotal,
            48
          },
          {
            EBetType.Fora,
            49
          }
        }
      }
    };

    public static ESport SportParse(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException(string.Format("{0}", (object) false));
      switch (value.ToLower())
      {
        case "бадминтон":
          return ESport.Badminton;
        case "баскетбол":
          return ESport.Basketball;
        case "бейсбол":
          return ESport.Baseball;
        case "волейбол":
          return ESport.VoleyBall;
        case "гандбол":
          return ESport.Handball;
        case "настольный теннис":
          return ESport.TableTennis;
        case "теннис":
          return ESport.Tennis;
        case "футбол":
          return ESport.Football;
        case "футзал":
          return ESport.Futsal;
        case "хоккей":
          return ESport.Hockey;
        default:
          throw new ArgumentException("ESport is not have " + value);
      }
    }

    public static string ToNormalString(this EBookmakers bookmaker)
    {
      return bookmaker.ToString().Replace("_", "");
    }

    public static string ToNormalStringUpper(this EBookmakers book)
    {
      string normalString = book.ToNormalString();
      return normalString.Substring(0, 1).ToUpper() + normalString.Substring(1, normalString.Length - 1);
    }

    public static string ToNormalString(this ESport sport)
    {
      switch (sport)
      {
        case ESport.Football:
          return "Футбол";
        case ESport.Tennis:
          return "Теннис";
        case ESport.Hockey:
          return "Хоккей";
        case ESport.Basketball:
          return "Баскетбол";
        case ESport.VoleyBall:
          return "Волейбол";
        case ESport.Handball:
          return "Гандбол";
        case ESport.TableTennis:
          return "Настольный теннис";
        case ESport.Futsal:
          return "Футзал";
        case ESport.Badminton:
          return "Бадминтон";
        case ESport.Baseball:
          return "Бейсбол";
        default:
          throw new ArgumentOutOfRangeException(nameof (sport), (object) sport, (string) null);
      }
    }

    public static string ToNormalString(this EBetType betType)
    {
      switch (betType)
      {
        case EBetType.Win:
          return "Победы";
        case EBetType.Total:
          return "Тоталы";
        case EBetType.IndTotal:
          return "Инд.Тоталы";
        case EBetType.Fora:
          return "Форы";
        case EBetType.Corner:
          return "Угловые";
        case EBetType.WinInSet:
          return "Победа в Сете";
        case EBetType.WinInGame:
          return "Победа в Гейме";
        case EBetType.WinPoints:
          return "Выигрыши очка";
        case EBetType.WinInCurrentGame:
          return "Победа в текущем гейме";
        case EBetType.Goals:
          return "Голы";
        default:
          throw new ArgumentOutOfRangeException(nameof (betType), (object) betType, (string) null);
      }
    }

    public static int ToServerSportId(this ESport sport)
    {
      return (int) (sport - 100);
    }

    public static Dictionary<ESport, Dictionary<EBetType, int>> GetAllSports()
    {
      return EnumsHelper._correctSportValue;
    }

    public static int ConvertToServerBetId(ESport sport, EBetType betType)
    {
      return EnumsHelper._correctSportValue[sport][betType];
    }

    public static string GetSvgImagePath(this ESport sport)
    {
      switch (sport)
      {
        case ESport.Football:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/soccer.svg";
        case ESport.Tennis:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/tennis2.svg";
        case ESport.Hockey:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/hockey.svg";
        case ESport.Basketball:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/basketball.svg";
        case ESport.VoleyBall:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/volleyball.svg";
        case ESport.Handball:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/handball.svg";
        case ESport.TableTennis:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/tableTennis.svg";
        case ESport.Futsal:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/futsal.svg";
        case ESport.Badminton:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/badminton.svg";
        case ESport.Baseball:
          return "/LiveForks.Client;component/Images/SportsIcon/svg/baseball.svg";
        default:
          throw new ArgumentOutOfRangeException(nameof (sport), (object) sport, (string) null);
      }
    }
  }
}
