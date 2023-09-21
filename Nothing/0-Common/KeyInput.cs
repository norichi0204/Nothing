using System;
using DxLibDLL;

namespace Nothing
{
    internal class KeyInput
    {
        private byte[] bytes = new byte[256];
        private int[] cntBytes = new int[256];


        public static KeyInput Key;

        static KeyInput() => KeyInput.Key = new KeyInput();

        public bool IsPush(int key) => this.cntBytes[key] == 1;

        public bool IsPushing(int key) => this.cntBytes[key] > 0;

        public void Update()
        {
            if (DX.GetHitKeyStateAll(this.bytes) != 0)
                return;
            for (int index = 0; index < this.bytes.Length; ++index)
            {
                if (this.bytes[index] > 0)
                    ++this.cntBytes[index];
                else
                    this.cntBytes[index] = 0;
            }
        }


    }
}
