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
            uint tid = id32 & 0xFFFF;
            uint sid = (id32 >> 16) & 0xFFFF;
            uint xor = ((pid >> 16) ^ (pid & 0xFFFF)) ^ tid ^ sid;
            return xor < 16;
        }

        // Placeholder implementations for other referenced methods:
        public static uint GetShinyXor(uint pid, uint tid)
        {
            return ((pid >> 16) ^ (pid & 0xFFFF)) ^ ((tid >> 16) ^ (tid & 0xFFFF));
        }

        public static void ForceShinyState(bool shiny, ref uint pid, uint id32, uint xor)
        {
            // Implementation depends on your shiny enforcement logic.
        }

        public static uint GetShinyPID(ushort tid, ushort sid, uint pid, uint shinyType)
        {
            // Implementation depends on your PID generation logic.
            return pid;
        }
    }
}
