namespace GeneralizerDLL
{
    public static class Generalizer_Events
    {
        public static event EventHandler<string>? Warning;
        public static void RaiseWarning(object sender, string message)
        {
            Warning?.Invoke(sender, message);
        }
    }
}
