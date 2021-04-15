using System;

namespace EntityG.Common.Helpers
{
    public static class Require
    {
        public static void IsNotNull(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
        }
    }
}
