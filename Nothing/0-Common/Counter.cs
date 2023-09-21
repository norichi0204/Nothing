using DxLibDLL;

namespace Nothing
{
    public class Counter
    {
        public double dbTick;
        public int nStart;
        public int nEnd;
        public double nNowTime;
        public bool bIsLoop;
        public bool IsStarted;
        public int nValue;

        public Counter(int begin, int end, double interval, bool isLoop)
        {
            this.nNowTime = DX.GetNowHiPerformanceCount();
            this.nStart = begin;
            this.nEnd = end;
            this.dbTick = interval;
            this.nValue = begin;
            this.bIsLoop = isLoop;
            this.IsStarted = false;
        }

        public void tSpeedChange(double db間隔)
        {
            this.tTick();
            this.dbTick = db間隔;
            this.tTick();
        }

        public void Stop()
        {
            if (!this.IsStarted)
                return;
            this.IsStarted = false;
        }

        public void Start(int nTime = 0)
        {
            this.IsStarted = false;
            this.nValue = nTime;
            if (this.IsStarted)
                return;
            this.tTick();
            this.IsStarted = true;
            this.nValue = nTime;
        }

        public void Reset(long nInitValue = 0)
        {
            this.IsStarted = false;
            this.Stop();
            this.nValue = (int)nInitValue;
        }

        public long tTick()
        {
            int num1 = 0;
            double performanceCount = DX.GetNowHiPerformanceCount();
            if (!this.IsStarted)
            {
                this.nNowTime = performanceCount;
                return 0;
            }
            double num2;
            for (num2 = performanceCount - this.nNowTime; num2 >= this.dbTick; num2 -= this.dbTick)
            {
                ++this.nValue;
                ++num1;
                if (this.nValue >= this.nEnd)
                    this.nValue = !this.bIsLoop ? this.nEnd : this.nStart;
            }
            this.nNowTime = performanceCount - num2;
            return num1;
        }

        public bool bProGress => this.IsStarted;

        public bool bEnded => this.nValue >= this.nEnd;
    }
}
