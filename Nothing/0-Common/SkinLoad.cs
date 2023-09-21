
using DxLibDLL;
using System;
using System.Collections.Generic;
using System.IO;

namespace Nothing
{
    public class SkinLoad
    {
        const string BASE = @"System\";
        const string SOUND = @"Sound\";

        public void tLoadTexture()
        {

        }
        public void tLoadSound()
        {

        }



        #region[ 関数呼び出し ]
        public int t連番画像の枚数を数える(string FilePass)
        {
            int num = 0;
            while (File.Exists(FilePass + num.ToString() + ".png"))
                ++num;
            return num;
        }
        public int t連番フォルダの個数を数える(string FilePass = "")
        {
            int num = 0;
            while (Directory.Exists(FilePass + num))
                num++;

            return num;
        }
        #endregion
    }
}
