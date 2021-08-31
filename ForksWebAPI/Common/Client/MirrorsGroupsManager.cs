
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForksWebAPI.Common.Client
{
  public class MirrorsGroupsManager
  {
    private static readonly Dictionary<int, List<int>> _groups = new Dictionary<int, List<int>>()
    {
      {
        1,
        new List<int>() { 1, 306 }
      },
      {
        2,
        new List<int>() { 2, 324 }
      },
      {
        3,
        new List<int>() { 3, 339 }
      },
      {
        4,
        new List<int>() { 4, 6, 34, 337, 346, 315, 359 }
      },
      {
        5,
        new List<int>() { 5, 348 }
      },
      {
        7,
        new List<int>() { 7 }
      },
      {
        8,
        new List<int>() { 8, 30, 300, 321, 334, 338 }
      },
      {
        9,
        new List<int>() { 9, 314 }
      },
      {
        11,
        new List<int>()
        {
          11,
          301,
          302,
          312,
          327,
          341,
          309,
          316,
          326
        }
      },
      {
        12,
        new List<int>() { 12 }
      },
      {
        15,
        new List<int>() { 15 }
      },
      {
        16,
        new List<int>() { 16, 313 }
      },
      {
        17,
        new List<int>() { 17 }
      },
      {
        18,
        new List<int>() { 18, 304 }
      },
      {
        19,
        new List<int>() { 19, 332 }
      },
      {
        20,
        new List<int>() { 20, 349 }
      },
      {
        21,
        new List<int>() { 21, 322 }
      },
      {
        22,
        new List<int>() { 22, 38, 298, 299 }
      },
      {
        23,
        new List<int>() { 23, 351 }
      },
      {
        24,
        new List<int>()
        {
          24,
          35,
          311,
          318,
          342,
          347,
          308,
          329,
          333,
          340
        }
      },
      {
        25,
        new List<int>() { 25, 319, 320, 330 }
      },
      {
        26,
        new List<int>() { 26, 344 }
      },
      {
        27,
        new List<int>() { 27 }
      },
      {
        29,
        new List<int>() { 29, 307, 317 }
      },
      {
        31,
        new List<int>() { 31 }
      },
      {
        32,
        new List<int>() { 32 }
      },
      {
        33,
        new List<int>() { 33, 310, 296 }
      },
      {
        39,
        new List<int>() { 39 }
      },
      {
        40,
        new List<int>() { 40, 44 }
      },
      {
        41,
        new List<int>() { 41, 14, 336 }
      },
      {
        42,
        new List<int>() { 42, 10 }
      },
      {
        43,
        new List<int>() { 43 }
      },
      {
        45,
        new List<int>() { 45 }
      }
    };
    private List<EBookmakers> _notSupportLinksBookmakers = new List<EBookmakers>()
    {
      EBookmakers._comeOn,
      EBookmakers._jenningsBet,
      EBookmakers._mobilbet,
      EBookmakers._giocoDigitale,
      EBookmakers._12bet,
      EBookmakers._betstar,
      EBookmakers._ladbrokesAu,
      EBookmakers._bizonBet,
      EBookmakers._noxWin,
      EBookmakers._olybet,
      EBookmakers._royalPanda,
      EBookmakers._williamHillMobile
    };

    public List<EBookmakers> AllBookmakers { get; }

    public Dictionary<EBookmakers, EBookmakers> RuMirrors { get; } = new Dictionary<EBookmakers, EBookmakers>()
    {
      {
        EBookmakers._zenit,
        EBookmakers._zenitWin
      },
      {
        EBookmakers._olimp,
        EBookmakers._olimpBet
      },
      {
        EBookmakers._marathon,
        EBookmakers._marathonRu
      },
      {
        EBookmakers._parimatch,
        EBookmakers._parimatchRu
      },
      {
        EBookmakers._1xbet,
        EBookmakers._1xstavkaRu
      },
      {
        EBookmakers._fonbet,
        EBookmakers._fonbetRu
      },
      {
        EBookmakers._winlinebet,
        EBookmakers._winlineRu
      },
      {
        EBookmakers._betcity,
        EBookmakers._betcityRu
      },
      {
        EBookmakers._baltbet,
        EBookmakers._baltbetRu
      },
      {
        EBookmakers._liga_stavok,
        EBookmakers._ligastavokRu
      },
      {
        EBookmakers._bwin,
        EBookmakers._bwinRu
      },
      {
        EBookmakers._tennisi,
        EBookmakers._tennisiRu
      },
      {
        EBookmakers._mostbet,
        EBookmakers._mostbetRu
      }
    };

    public Dictionary<EBookmakers, EBookmakers> OffshoreMirrors { get; }

    public Dictionary<EBookmakers, List<EBookmakers>> BookmakersGroups { get; } = new Dictionary<EBookmakers, List<EBookmakers>>();

    public MirrorsGroupsManager()
    {
      HashSet<EBookmakers> source = new HashSet<EBookmakers>();
      foreach (KeyValuePair<int, List<int>> group in MirrorsGroupsManager._groups)
      {
        EBookmakers key = (EBookmakers) group.Key;
        if (!Enum.IsDefined(typeof (EBookmakers), (object) key))
          throw new ArgumentException(string.Format("Группа {0}({1}) не принадлежит Bookmakers", (object) key.ToNormalStringUpper(), (object) group.Key));
        this.BookmakersGroups.Add(key, new List<EBookmakers>());
        foreach (int num in group.Value)
        {
          EBookmakers book = (EBookmakers) num;
          if (!Enum.IsDefined(typeof (EBookmakers), (object) book))
            throw new ArgumentException(string.Format("Букмекер {0}({1}) не принадлежит Bookmakers", (object) book.ToNormalStringUpper(), (object) num));
          if (this.BookmakersGroups[key].Contains(book))
            throw new ArgumentException(string.Format("Бк {0} уже есть в главной группе {1}", (object) book.ToNormalStringUpper(), (object) num));
          source.Add(book);
          this.BookmakersGroups[key].Add(book);
        }
      }
      List<EBookmakers> list = Enum.GetValues(typeof (EBookmakers)).Cast<EBookmakers>().ToList<EBookmakers>();
      if (source.Count != list.Count - 1)
      {
        foreach (EBookmakers book in list)
        {
          if (!source.Contains(book))
            throw new ArgumentException(book.ToNormalStringUpper() + " не было использовано!");
        }
      }
      this.AllBookmakers = source.ToList<EBookmakers>();
      this.OffshoreMirrors = this.RuMirrors.ToDictionary<KeyValuePair<EBookmakers, EBookmakers>, EBookmakers, EBookmakers>((Func<KeyValuePair<EBookmakers, EBookmakers>, EBookmakers>) (x => x.Value), (Func<KeyValuePair<EBookmakers, EBookmakers>, EBookmakers>) (x => x.Key));
    }

    public bool IsSupportLink(EBookmakers bookmaker)
    {
      return this._notSupportLinksBookmakers.Contains(bookmaker);
    }

    public bool IsPositiveBetRuMirror(EBookmakers bookmaker)
    {
      return this.OffshoreMirrors.ContainsKey(bookmaker);
    }

    public EBookmakers GetBookmakerGroup(EBookmakers group)
    {
      foreach (KeyValuePair<EBookmakers, List<EBookmakers>> bookmakersGroup in this.BookmakersGroups)
      {
        foreach (EBookmakers ebookmakers in bookmakersGroup.Value)
        {
          if (group == ebookmakers)
            return bookmakersGroup.Key;
        }
      }
      return EBookmakers._None;
    }
  }
}
