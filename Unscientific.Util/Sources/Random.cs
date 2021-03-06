﻿namespace Unscientific.Util
{
    /// <summary>
    /// Random class, can be used as component (e.g. in Singletons scope).
    /// Uses XorShift128 to generate random numbers. This class is completely deterministic.
    /// </summary>
    public class Random
    {
        private ulong _state0;
        private ulong _state1;

        public Random(ulong seed)
        {
            Seed(seed);
        }

        public int Next()
        {
            return (int) (XorShift128() & 0x7FFFFFFFUL);
        }

        public int Next(int max)
        {
            var next = XorShift128() & 0x7FFFFFFFUL;

            return (int) (next * (ulong) max / 0x7FFFFFFFUL);
        }

        public int Next(int min, int max)
        {
            return min + Next(max - min);
        }

        public void Seed(ulong seed)
        {
            _state0 = MurmurHash3(seed);
            _state1 = MurmurHash3(~seed);
        }

        private static ulong MurmurHash3(ulong h)
        {
            h ^= h >> 33;
            h *= 0xFF51AFD7ED558CCDUL;
            h ^= h >> 33;
            h *= 0xC4CEB9FE1A85EC53UL;
            h ^= h >> 33;
            return h;
        }

        private ulong XorShift128()
        {
            var x = _state0;
            var y = _state1;

            _state0 = y;
            x ^= x << 23;
            _state1 = x ^ y ^ (x >> 17) ^ (y >> 26);
            return _state1 + y;
        }
    }
}