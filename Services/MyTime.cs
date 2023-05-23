public class MyTime : ITime
{
    public string getTime()
    {
        return DateTime.Now.ToString();
    }
}