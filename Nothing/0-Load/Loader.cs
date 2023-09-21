using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DxLibDLL;

namespace Nothing
{
    public class Loader
    {
        private Texture Back,
            Panel;
        private Counter ctFlush;

        private int nSelect;

        private bool b決定;
        public void tRecovery()
        {
            Back = new Texture(@"System\Texture\Background.png");
            Panel = new Texture(@"System\Texture\Panel.png");

            ctFlush = new Counter(0,100,1000,false);
            b決定 = false;
        }
        public void tMainLoader()
        {
            ctFlush.tTick();

            Back.Draw(0,0);

            Panel.Draw(105 +(nSelect * 256), 600, new Rectangle(0, 0, 300, 400));
            Panel.Draw(150,620,new Rectangle(320,0,1370,400));
            //  Panel.Draw(1050, 50, new Rectangle(0, 420, 830, 300));
            if(ctFlush.nValue >= 10 && ctFlush.nValue <= 90)
               Panel.Draw(105 + (nSelect * 256), 600, new Rectangle(0, 0, 300, 400));

            #region[ キー ]
            if (!b決定)
            {
                if (Program.Keyinput.IsPush(33) || Program.Keyinput.IsPush(36))
                {
                    ctFlush.Start();
                   
                    tSelected(nSelect);
                }
                else if (Program.Keyinput.IsPush(DX.KEY_INPUT_D))
                {
                    if (nSelect != 0)
                        nSelect--;
                }
                else if (Program.Keyinput.IsPush(DX.KEY_INPUT_K))
                {
                    if (nSelect < 3)
                        nSelect++;
                }
            }
            if (Program.Keyinput.IsPush(DX.KEY_INPUT_ESCAPE))
            {
                b決定 = false;
            }
            #endregion

        }
        private void tSelected(int n)
        {
            switch (n)
            {
                case 0:

                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:
                    DX.DxLib_End();
                    break;

            }
            b決定 = true;
        }
    }
}
