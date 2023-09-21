using DxLibDLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nothing
{
    internal static class Program
    {
        [STAThread]
        internal static void Main()
        {
            try
            {
                Program.App();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "エラーが発生しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!Directory.Exists("Info\\Error"))
                    Directory.CreateDirectory("Info\\Error");
                StreamWriter text = File.CreateText("Info\\Error\\Error_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log");
                text.WriteLine("エラー内容は以下の通りです。\r\n" + ex.ToString());
                text.Close();
                text.Dispose();
            }
        }

        internal static void App()
        {
            DX.SetOutApplicationLogValidFlag(0);//Logファイルを出力するか
            DX.ChangeWindowMode(1);
            DX.SetGraphMode(Program.bIsFullHD ? 1920 : 1280, Program.bIsFullHD ? 1080 : 720, 32); // 初期ウィンドウのサイズ
            DX.SetUseTransColor(0);
            DX.SetAlwaysRunFlag(1);//常にウィンドウを実行するか
            DX.SetDoubleStartValidFlag(0);
            DX.SetWaitVSyncFlag(1);//垂直同期
            DX.SetWindowSizeChangeEnableFlag(1);//ウィンドウサイズの変更をするか

            DX.SetMainWindowText("Hi, this is a new TaikoSimu!!!! ");

            DX.DxLib_Init();
            load.tRecovery();
            while (DX.ProcessMessage() == 0 && DX.ScreenFlip() == 0 && DX.ClearDrawScreen() == 0)
            {
                DX.SetDrawScreen(-2);
                DX.ClearDrawScreen();
                Keyinput.Update();

                load.tMainLoader();

            }
            DX.DxLib_End();
        }
        public static Program.EScene eScene;
        public static Loader load = new Loader();
        public static KeyInput Keyinput = new KeyInput();
        public static bool bIsFullHD;

        public enum EScene
        {
            eTitle,
            eLoading,
            eSongSelect,
            eGame,
            eResult,
        }
    }
}
