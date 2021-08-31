namespace ForksWebAPI.Common.Client
{
  public enum EForkHideRule
  {
    ThisForks = 1,
    AllBkAllFork = 2,
    OnlyOneBkAllForks = 3,
    OnlyTwoBkAllForks = 4,
    TwoBkAllForks = 5,
    OnlyOneBkOnlyThisStake = 6,
    OnlyTwoOnlyThisStake = 7,
    TwoBkOnlyThisStake = 8,
    ShowAllHidenForks = 99, // 0x00000063
  }
}
