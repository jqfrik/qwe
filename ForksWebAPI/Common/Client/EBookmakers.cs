
using System;

namespace ForksWebAPI.Common.Client
{
  [Flags]
  public enum EBookmakers
  {
    _betcity = 1,
    _marathon = 2,
    _188bet = _marathon | _betcity, // 0x00000003
    _bwin = 4,
    _willhill = _bwin | _betcity, // 0x00000005
    _sportingbet = _bwin | _marathon, // 0x00000006
    _sbobet = _sportingbet | _betcity, // 0x00000007
    _unibet = 8,
    _fonbet = _unibet | _betcity, // 0x00000009
    _bet365Mobile = _unibet | _marathon, // 0x0000000A
    _10bet = _bet365Mobile | _betcity, // 0x0000000B
    _pinnacle = _unibet | _bwin, // 0x0000000C
    _parimatchClassic = _pinnacle | _marathon, // 0x0000000E
    _bet_at_home = _parimatchClassic | _betcity, // 0x0000000F
    _favbet = 16, // 0x00000010
    _betfair = _favbet | _betcity, // 0x00000011
    _baltbet = _favbet | _marathon, // 0x00000012
    _olimp = _baltbet | _betcity, // 0x00000013
    _winlinebet = _favbet | _bwin, // 0x00000014
    _liga_stavok = _winlinebet | _betcity, // 0x00000015
    _1xbet = _winlinebet | _marathon, // 0x00000016
    _zenit = _1xbet | _betcity, // 0x00000017
    _vbet = _favbet | _unibet, // 0x00000018
    _leon = _vbet | _betcity, // 0x00000019
    _tennisi = _vbet | _marathon, // 0x0000001A
    _paddypower = _tennisi | _betcity, // 0x0000001B
    _ladbrokes = _leon | _bwin, // 0x0000001D
    _888sport = _tennisi | _bwin, // 0x0000001E
    _betrally = _888sport | _betcity, // 0x0000001F
    _betfair_sport = 32, // 0x00000020
    _dafabet = _betfair_sport | _betcity, // 0x00000021
    _gamebookers = _betfair_sport | _marathon, // 0x00000022
    _zirkabet = _gamebookers | _betcity, // 0x00000023
    _melbet = _gamebookers | _bwin, // 0x00000026
    _sportingbull = _melbet | _betcity, // 0x00000027
    _mostbet = _betfair_sport | _unibet, // 0x00000028
    _parimatch = _mostbet | _betcity, // 0x00000029
    _bet365 = _mostbet | _marathon, // 0x0000002A
    _betStarts = _bet365 | _betcity, // 0x0000002B
    _1win = _parimatch | _bwin, // 0x0000002D
    _12bet = 296, // 0x00000128
    _1xbit = _12bet | _marathon, // 0x0000012A
    _1xstavkaRu = _1xbit | _betcity, // 0x0000012B
    _32Red = _12bet | _bwin, // 0x0000012C
    _377bet = _32Red | _betcity, // 0x0000012D
    _adjarabet = _32Red | _marathon, // 0x0000012E
    _baltbetRu = 304, // 0x00000130
    _betcityRu = _baltbetRu | _marathon, // 0x00000132
    _betstar = _betcityRu | _betcity, // 0x00000133
    _bizonBet = _baltbetRu | _bwin, // 0x00000134
    _comeOn = _bizonBet | _betcity, // 0x00000135
    _dafabetMobile = _bizonBet | _marathon, // 0x00000136
    _dlvbet = _dafabetMobile | _betcity, // 0x00000137
    _etoto = _baltbetRu | _unibet, // 0x00000138
    _favtoto = _etoto | _betcity, // 0x00000139
    _fonbetRu = _etoto | _marathon, // 0x0000013A
    _giocoDigitale = _fonbetRu | _betcity, // 0x0000013B
    _jenningsBet = _etoto | _bwin, // 0x0000013C
    _ladbrokesAu = _jenningsBet | _betcity, // 0x0000013D
    _lemanbet = _jenningsBet | _marathon, // 0x0000013E
    _leonRu = _lemanbet | _betcity, // 0x0000013F
    _leonRuMobile = 320, // 0x00000140
    _leoVegas = _leonRuMobile | _betcity, // 0x00000141
    _ligastavokRu = _leonRuMobile | _marathon, // 0x00000142
    _marathonRu = _leonRuMobile | _bwin, // 0x00000144
    _mobilbet = _marathonRu | _marathon, // 0x00000146
    _netBet = _mobilbet | _betcity, // 0x00000147
    _noxWin = _leoVegas | _unibet, // 0x00000149
    _oddsRing = _ligastavokRu | _unibet, // 0x0000014A
    _olimpBet = _marathonRu | _unibet, // 0x0000014C
    _olybet = _olimpBet | _betcity, // 0x0000014D
    _paf = _olimpBet | _marathon, // 0x0000014E
    _parimatchRu = _leonRuMobile | _favbet, // 0x00000150
    _partypoker = _parimatchRu | _betcity, // 0x00000151
    _redBet = _parimatchRu | _marathon, // 0x00000152
    _roadBet = _redBet | _betcity, // 0x00000153
    _royalPanda = _parimatchRu | _bwin, // 0x00000154
    _rubet = _royalPanda | _betcity, // 0x00000155
    _tempobet = _royalPanda | _marathon, // 0x00000156
    _tennisiRu = _parimatchRu | _unibet, // 0x00000158
    _vistabet = _tennisiRu | _marathon, // 0x0000015A
    _vivarobet = _vistabet | _betcity, // 0x0000015B
    _williamHillMobile = _tennisiRu | _bwin, // 0x0000015C
    _winlineRu = _williamHillMobile | _betcity, // 0x0000015D
    _zenitWin = _winlineRu | _marathon, // 0x0000015F
    _mostbetRu = _mostbet | _bwin, // 0x0000002C
    _bwinRu = _netBet | _betfair_sport, // 0x00000167
    _None = 99, // 0x00000063
  }
}
