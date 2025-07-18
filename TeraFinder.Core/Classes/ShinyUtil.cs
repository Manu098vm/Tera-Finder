// TeraFinder.Core/Classes/ShinyUtil.cs

namespace TeraFinder.Core
{
    public static class ShinyUtil
    {
        /// <summary>
        /// Returns true if the PID is shiny for the given 32-bit Trainer ID.
        /// </summary>
        public static bool GetIsShiny(uint id32, uint pid)
        {
            // Standard Gen 8/9 shiny check
            uint tid = id32 & 0xFFFF;
            uint sid = (id32 >> 16) & 0xFFFF;
            uint xor = ((pid >> 16) ^ (pid & 0xFFFF)) ^ tid ^ sid;
            return xor < 16;
        }

        /// <summary>
        /// Computes the shiny XOR value for a PID and a TID/SID pair.
        /// </summary>
        public static uint GetShinyXor(uint pid, uint id32)
        {
            uint tid = id32 & 0xFFFF;
            uint sid = (id32 >> 16) & 0xFFFF;
            return ((pid >> 16) ^ (pid & 0xFFFF)) ^ tid ^ sid;
        }

        public static bool GetIsShiny6(uint id32, uint pid)
        {
            // If Gen 6/7 shiny logic is required, use this; otherwise, it is identical to Gen 8/9.
            // Standard Gen 6+ shiny check: xor of TID/SID and PID lower/upper 16 bits
            uint tid = id32 & 0xFFFF;
            uint sid = (id32 >> 16) & 0xFFFF;
            uint xor = ((pid >> 16) ^ (pid & 0xFFFF)) ^ tid ^ sid;
            return xor < 16;
        }

        /// <summary>
        /// Forces the PID to be shiny or not shiny for the given id32 and shiny Xor (0 = Square, 1 = Star).
        /// </summary>
        public static void ForceShinyState(bool shiny, ref uint pid, uint id32, uint xor)
        {
            uint tid = id32 & 0xFFFF;
            uint sid = (id32 >> 16) & 0xFFFF;
            uint low = pid & 0xFFFF;
            uint pidXor = shiny ? xor : 0x10; // 0 for Square, 1 for Star, >=16 for not shiny

            uint high = (low ^ tid ^ sid ^ pidXor) & 0xFFFF;
            pid = (high << 16) | low;

            // If not shiny, ensure xor >= 16
            if (!shiny && GetShinyXor(pid, id32) < 16)
            {
                // Flip a bit to break shiny status
                pid ^= 0x10000000;
            }
        }

        /// <summary>
        /// Generates a shiny PID with the given TID/SID and shiny type.
        /// shinyType: 0 = Square (xor==0), 1 = Star (xor==1)
        /// </summary>
        public static uint GetShinyPID(ushort tid, ushort sid, uint pid, uint shinyType)
        {
            uint low = pid & 0xFFFF;
            uint xor = (shinyType == 0) ? 0u : 1u;
            uint high = (low ^ tid ^ sid ^ xor) & 0xFFFF;
            return (high << 16) | low;
        }
    }
}
