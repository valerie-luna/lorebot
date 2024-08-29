namespace Weather
{
    public enum CloudLevel
    {
        Unspecified = 0,
        SkyClear = 1,
        Trace = 2 | CloudyFlag,
        Scattered = 3 | CloudyFlag,
        Broken = 4 | CloudyFlag,
        Overcast = 5 | CloudyFlag,
        CloudyFlag = 0b10000000
    }
}