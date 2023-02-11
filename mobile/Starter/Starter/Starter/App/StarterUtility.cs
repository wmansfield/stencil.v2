
namespace Starter.App
{
    public static class StarterUtility
    {
        public static string TrimSafe(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return value.Trim();
        }
    }
}
