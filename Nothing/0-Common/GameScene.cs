
using System.Collections.Generic;

namespace Nothing
{
    public class GameScene
    {
        public List<GameScene> list子GameScene;
        protected bool b初めての進行描画 = true;

        public bool b活性化してる { get; private set; }

        public bool b活性化してない
        {
            get => !this.b活性化してる;
            set => this.b活性化してる = !value;
        }

        public GameScene()
        {
            this.b活性化してない = true;
            this.list子GameScene = new List<GameScene>();
        }

        public virtual void tRecovery()
        {
            if (this.b活性化してる)
                return;
            this.b活性化してる = true;
        }

        public virtual void tDelete()
        {
            if (this.b活性化してない)
                return;
            this.b活性化してない = true;
        }

        public virtual void tDraw()
        {
            if (!this.b活性化してない)
                ;
        }
    }
}
