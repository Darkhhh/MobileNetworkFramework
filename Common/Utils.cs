namespace MobileNetworkFramework.Common
{
    public static class Utils
    {
        public static int NextIndex(int currentIndex, int numberOfElements)
        {
            return (currentIndex + 1) % numberOfElements;
        }

        public static int PreviousIndex(int currentIndex, int numberOfElements)
        {
            if (currentIndex - 1 < 0) return numberOfElements - 1;
            return currentIndex - 1;
        }
    }
}

