using UnityEngine;

namespace WorldBuilder.Debuggers
{
    public class WorldGridDebuggerByte : WorldGridDebugger<byte>
    {
        protected override Color GetColor(byte value)
        {
            return Color.Lerp(Color.black, Color.white, (float)value / 255);
        }
    }
}