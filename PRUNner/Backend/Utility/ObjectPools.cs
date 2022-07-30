using System.Text;

namespace PRUNner.Backend.Utility
{
    public static class ObjectPools
    {
        public static readonly SimpleObjectPool<StringBuilder> StringBuilderPool = new(sb => sb.Clear());
    }
}