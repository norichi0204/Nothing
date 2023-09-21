
using DxLibDLL;
using System;
using System.IO;

namespace Nothing
{
    public class Sound : IDisposable
    {
        /// <summary>
        /// サウンドを生成します。
        /// </summary>
        public Sound(string fileName, Async sync = Async.OFF)
        {
            Sync = sync;
            if (sync == Async.ON)
            {
                DX.SetCreateSoundDataType(DX.DX_SOUNDDATATYPE_FILE);
                DX.SetUseASyncLoadFlag(DX.TRUE);
                ID = DX.LoadSoundMem(fileName);
                DX.SetUseASyncLoadFlag(DX.FALSE);
                DX.SetCreateSoundDataType(DX.DX_SOUNDDATATYPE_MEMNOPRESS_PLUS);
            }
            else
                ID = DX.LoadSoundMem(fileName);

            if (ID != -1)
            {
                IsEnable = true;
            }
            FileName = fileName;

            Volume(255);
        }

        ~Sound()
        {
            if (IsEnable)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (Sync == Async.ON ? DX.CheckHandleASyncLoad(ID) != 0 : false) return;
            if (DX.DeleteSoundMem(ID) != -1)
            {
                IsEnable = false;
            }
        }

        /// <summary>
        /// サウンドを再生します。
        /// </summary>
        /// <param name="playFromBegin">はじめから</param>

        public void Play(long nPlayTime = 0L)
        {
            DX.PlaySoundMem(this.ID, 1);
            DX.SetSoundCurrentTime(nPlayTime, this.ID);
        }
        /// <summary>
        /// サウンドを停止します。
        /// </summary>
        public void Stop()
        {
            if (IsEnable)
            {
                DX.StopSoundMem(ID);
            }
        }

        /// <summary>
        /// 有効かどうか。
        /// </summary>
        public bool IsEnable { get; private set; }

        /// <summary>
        /// ファイル名。
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// ID。
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 再生中かどうか。
        /// </summary>
       	public bool CheackSoundPlay()
        {
            return DX.CheckSoundMem(this.ID) == 1;
        }

        /// <summary>
        /// パン。
        /// </summary>
        public int Pan
        {
            get
            {
                return _pan;
            }
            set
            {
                _pan = value;
                DX.ChangePanSoundMem(value, ID);
            }
        }

        /// <summary>
        /// 音量。
        /// </summary>
        public void Volume(int nVolume)
        {
            DX.ChangeVolumeSoundMem(nVolume, this.ID);
        }

        /// <summary>
        /// 再生位置。秒が単位。
        /// </summary>
        public double Time
        {
            get
            {
                var freq = DX.GetFrequencySoundMem(ID);
                var pos = DX.GetCurrentPositionSoundMem(ID);
                // サンプル数で割ると秒数が出るが出る
                return 1.0 * pos / freq;
            }
            set
            {
                var freq = DX.GetFrequencySoundMem(ID);
                var pos = value;
                DX.SetCurrentPositionSoundMem((int)(1.0 * pos * freq), ID);
            }
        }

        /// <summary>
        /// 再生速度を倍率で変更する。
        /// </summary>
        public double PlaySpeed
        {
            get
            {
                return _ratio;
            }
            set
            {
                _ratio = value;
                DX.ResetFrequencySoundMem(ID);
                var freq = DX.GetFrequencySoundMem(ID);
                // 倍率変更
                var speed = value * freq;
                // 1秒間に再生すべきサンプル数を上げ下げすると速度が変化する。
                DX.SetFrequencySoundMem((int)speed, ID);
            }
        }

        private int _pan;
        private double _ratio;

        public Async Sync;
        public enum Async
        {
            ON,
            OFF,
        }
    }
}
