using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Helpers
{
    public static class BitwiseHelper
    {
        public static bool HasFlag(int flags, int flagToCheck)
        {
            return (flags & flagToCheck) == flagToCheck;
        }

        public static int AddFlags(int flags, params int[] flagsToAdd)
        {
            if (flagsToAdd == null)
            {
                return flags;
            }

            foreach (int num in flagsToAdd)
            {
                flags |= num;
            }

            return flags;
        }

        public static int DeleteFlags(int flags, params int[] flagsToDelete)
        {
            if (flagsToDelete == null)
            {
                return flags;
            }

            foreach (int num in flagsToDelete)
            {
                flags &= ~num;
            }

            return flags;
        }

        public static int ToggleFlag(int flags, int flagToToggle)
        {
            flags ^= flagToToggle;
            return flags;
        }

        public static int MinFlag(int value)
        {
            int num = 1;
            while ((num & value) == 0 && num != 0)
            {
                num <<= 1;
            }

            return num;
        }

        public static int MaxFlag(int value)
        {
            int num = 1073741824;
            while ((num & value) == 0 && num != 0)
            {
                num >>= 1;
            }

            return num;
        }

        public static long MinFlag(long value)
        {
            int num = 1;
            while ((num & value) == 0L && num != 0)
            {
                num <<= 1;
            }

            return num;
        }

        public static long MaxFlag(long value)
        {
            int num = 1073741824;
            while ((num & value) == 0L && num != 0)
            {
                num >>= 1;
            }

            return num;
        }
    }
}
