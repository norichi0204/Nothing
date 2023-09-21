
using DxLibDLL;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Nothing
{
    public class Texture : IDisposable
    {
        public int nHandle;
        public float n透明度;
        public PointF Scale;
        private bool bDispose完了済み;

        public void Dispose()
        {
            if (this.bDispose完了済み)
                return;
            if (this.nHandle != -1)
            {
                DX.DeleteGraph(this.nHandle);
                this.nHandle = -1;
            }
            this.bDispose完了済み = true;
        }

        public Texture(string FilePass)
        {
            this.nHandle = DX.LoadGraph(FilePass);
            this.n透明度 = 255;
            this.Scale.X = 1f;
            this.Scale.Y = 1f;

        }

        public Texture(Bitmap bitmap) : this()
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                var buf = ms.ToArray();

                unsafe
                {
                    fixed (byte* p = buf)
                    {
                        DX.SetDrawValidGraphCreateFlag(DX.TRUE);
                        DX.SetDrawValidAlphaChannelGraphCreateFlag(DX.TRUE);

                        nHandle = DX.CreateGraphFromMem((IntPtr)p, buf.Length);

                        DX.SetDrawValidGraphCreateFlag(DX.FALSE);
                        DX.SetDrawValidAlphaChannelGraphCreateFlag(DX.FALSE);
                    }
                }
            }
        }

        public Texture()
        {
            Rotation = 0.0f;
            this.Scale.X = 1f;
            this.Scale.Y = 1f;
            this.n透明度 = 255;
            ReferencePoint = EReferencePoint.TopLeft;

        }

        public void Draw(float x, float y, Rectangle? rectangle = null)
        {
            #region[ Draw ]
            PointF pointF = new PointF();
            int SizeXBuf;
            int SizeYBuf;
            DX.GetGraphSize(this.nHandle, out SizeXBuf, out SizeYBuf);
            if (!rectangle.HasValue)
                rectangle = new Rectangle?(new Rectangle(0, 0, SizeXBuf, SizeYBuf));
            Rectangle rectangle1;
            switch (this.ReferencePoint)
            {
                case Texture.EReferencePoint.TopLeft:
                    pointF.X = 0.0f;
                    pointF.Y = 0.0f;
                    break;
                case Texture.EReferencePoint.TopCenter:
                    pointF.X = rectangle.Value.Width / 2f;
                    pointF.Y = 0.0f;
                    break;
                case Texture.EReferencePoint.TopRight:
                    pointF.X = rectangle.Value.Width;
                    pointF.Y = 0.0f;
                    break;
                case Texture.EReferencePoint.CenterLeft:
                    pointF.X = 0.0f;
                    ref PointF local1 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double num1 = rectangle1.Height / 2.0;
                    local1.Y = (float)num1;
                    break;
                case Texture.EReferencePoint.Center:
                    pointF.X = rectangle.Value.Width / 2f;
                    ref PointF local2 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double num2 = rectangle1.Height / 2.0;
                    local2.Y = (float)num2;
                    break;
                case Texture.EReferencePoint.CenterRight:
                    pointF.X = rectangle.Value.Width;
                    ref PointF local3 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double num3 = rectangle1.Height / 2.0;
                    local3.Y = (float)num3;
                    break;
                case Texture.EReferencePoint.BottomLeft:
                    pointF.X = 0.0f;
                    ref PointF local4 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double height1 = rectangle1.Height;
                    local4.Y = (float)height1;
                    break;
                case Texture.EReferencePoint.BottomCenter:
                    pointF.X = rectangle.Value.Width / 2f;
                    ref PointF local5 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double height2 = rectangle1.Height;
                    local5.Y = (float)height2;
                    break;
                case Texture.EReferencePoint.BottomRight:
                    pointF.X = rectangle.Value.Width;
                    ref PointF local6 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double height3 = rectangle1.Height;
                    local6.Y = (float)height3;
                    break;
                default:
                    pointF.X = 0.0f;
                    pointF.Y = 0.0f;
                    break;
            }
            int n透明度 = (int)this.n透明度;
            if (this.BlendMode == Texture.EBlendMode.None)
                DX.SetDrawBlendMode(1, n透明度);
            else if (this.BlendMode == Texture.EBlendMode.Add)
                DX.SetDrawBlendMode(2, n透明度);
            else if (this.BlendMode == Texture.EBlendMode.Subtract)
                DX.SetDrawBlendMode(3, n透明度);
            else if (this.BlendMode == Texture.EBlendMode.Multiply)
                DX.SetDrawBlendMode(11, n透明度);
            DX.SetDrawMode(1);
            DX.SetDrawBright(this.Color.R, this.Color.G, this.Color.B);
            double num4 = x / (Program.bIsFullHD ? 1.0 : 1.5);
            double num5 = y / (Program.bIsFullHD ? 1.0 : 1.5);
            rectangle1 = rectangle.Value;
            int x1 = rectangle1.X;
            rectangle1 = rectangle.Value;
            int y1 = rectangle1.Y;
            rectangle1 = rectangle.Value;
            int width = rectangle1.Width;
            rectangle1 = rectangle.Value;
            int height4 = rectangle1.Height;
            double x2 = pointF.X;
            double y2 = pointF.Y;
            double ExtRateX = this.Scale.X / (Program.bIsFullHD ? 1.0 : 1.5);
            double ExtRateY = this.Scale.Y / (Program.bIsFullHD ? 1.0 : 1.5);
            double rotation = this.Rotation;
            int nHandle = this.nHandle;
            DX.DrawRectRotaGraph3F((float)num4, (float)num5, x1, y1, width, height4, (float)x2, (float)y2, ExtRateX, ExtRateY, rotation, nHandle, 1);
            DX.SetDrawBlendMode(0, 0);
            #endregion
        }
        public void DrawFlip(float x, float y, int FlipType, Rectangle? rectangle = null)
        {
            #region[ DrawFlip ]
            PointF pointF = new PointF();
            int SizeXBuf;
            int SizeYBuf;
            DX.GetGraphSize(this.nHandle, out SizeXBuf, out SizeYBuf);
            if (!rectangle.HasValue)
                rectangle = new Rectangle?(new Rectangle(0, 0, SizeXBuf, SizeYBuf));
            Rectangle rectangle1;
            switch (this.ReferencePoint)
            {
                case Texture.EReferencePoint.TopLeft:
                    pointF.X = 0.0f;
                    pointF.Y = 0.0f;
                    break;
                case Texture.EReferencePoint.TopCenter:
                    pointF.X = (float)rectangle.Value.Width / 2f;
                    pointF.Y = 0.0f;
                    break;
                case Texture.EReferencePoint.TopRight:
                    pointF.X = (float)rectangle.Value.Width;
                    pointF.Y = 0.0f;
                    break;
                case Texture.EReferencePoint.CenterLeft:
                    pointF.X = 0.0f;
                    ref PointF local1 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double num1 = (double)rectangle1.Height / 2.0;
                    local1.Y = (float)num1;
                    break;
                case Texture.EReferencePoint.Center:
                    pointF.X = (float)rectangle.Value.Width / 2f;
                    ref PointF local2 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double num2 = (double)rectangle1.Height / 2.0;
                    local2.Y = (float)num2;
                    break;
                case Texture.EReferencePoint.CenterRight:
                    pointF.X = (float)rectangle.Value.Width;
                    ref PointF local3 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double num3 = (double)rectangle1.Height / 2.0;
                    local3.Y = (float)num3;
                    break;
                case Texture.EReferencePoint.BottomLeft:
                    pointF.X = 0.0f;
                    ref PointF local4 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double height1 = (double)rectangle1.Height;
                    local4.Y = (float)height1;
                    break;
                case Texture.EReferencePoint.BottomCenter:
                    pointF.X = (float)rectangle.Value.Width / 2f;
                    ref PointF local5 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double height2 = (double)rectangle1.Height;
                    local5.Y = (float)height2;
                    break;
                case Texture.EReferencePoint.BottomRight:
                    pointF.X = (float)rectangle.Value.Width;
                    ref PointF local6 = ref pointF;
                    rectangle1 = rectangle.Value;
                    double height3 = (double)rectangle1.Height;
                    local6.Y = (float)height3;
                    break;
                default:
                    pointF.X = 0.0f;
                    pointF.Y = 0.0f;
                    break;
            }
            int n透明度 = (int)this.n透明度;
            if (this.BlendMode == Texture.EBlendMode.None)
                DX.SetDrawBlendMode(1, n透明度);
            else if (this.BlendMode == Texture.EBlendMode.Add)
                DX.SetDrawBlendMode(2, n透明度);
            else if (this.BlendMode == Texture.EBlendMode.Subtract)
                DX.SetDrawBlendMode(3, n透明度);
            else if (this.BlendMode == Texture.EBlendMode.Multiply)
                DX.SetDrawBlendMode(11, n透明度);
            DX.SetDrawMode(1);
            DX.SetDrawBright((int)this.Color.R, (int)this.Color.G, (int)this.Color.B);
            double num4 = (double)x / (Program.bIsFullHD ? 1.0 : 1.5);
            double num5 = (double)y / (Program.bIsFullHD ? 1.0 : 1.5);
            rectangle1 = rectangle.Value;
            int x1 = rectangle1.X;
            rectangle1 = rectangle.Value;
            int y1 = rectangle1.Y;
            rectangle1 = rectangle.Value;
            int width = rectangle1.Width;
            rectangle1 = rectangle.Value;
            int height4 = rectangle1.Height;
            double x2 = (double)pointF.X;
            double y2 = (double)pointF.Y;
            double ExtRateX = (double)this.Scale.X / (Program.bIsFullHD ? 1.0 : 1.5);
            double ExtRateY = (double)this.Scale.Y / (Program.bIsFullHD ? 1.0 : 1.5);
            double rotation = (double)this.Rotation;
            int nHandle = this.nHandle;
            DX.DrawRectRotaGraphFast3((int)num4, (int)num5, x1, y1, width, height4, (int)x2, (int)y2, (float)ExtRateX, (float)ExtRateY, (float)rotation, nHandle, 1, FlipType == 1 || FlipType == 2 ? 1 : 0, FlipType == 0 || FlipType == 2 ? 1 : 0);
            DX.SetDrawBlendMode(0, 0);
            #endregion
        }
        public void Draw(PointF point)
        {
            Point point1 = new Point();
            int SizeXBuf;
            int SizeYBuf;
            DX.GetGraphSize(this.nHandle, out SizeXBuf, out SizeYBuf);
            Rectangle rectangle = new Rectangle(0, 0, SizeXBuf, SizeYBuf);
            int n透明度 = (int)this.n透明度;
            if (this.BlendMode == Texture.EBlendMode.None)
                DX.SetDrawBlendMode(1, n透明度);
            else if (this.BlendMode == Texture.EBlendMode.Add)
                DX.SetDrawBlendMode(2, n透明度);
            else if (this.BlendMode == Texture.EBlendMode.Subtract)
                DX.SetDrawBlendMode(3, n透明度);
            else if (this.BlendMode == Texture.EBlendMode.Multiply)
                DX.SetDrawBlendMode(11, n透明度);
            point1.X = rectangle.Width / 2;
            point1.Y = rectangle.Height / 2;
            DX.SetDrawMode(1);
            int r = this.Color.R;
            Color color = this.Color;
            int g = color.G;
            color = this.Color;
            int b = color.B;
            DX.SetDrawBright(r, g, b);
            DX.DrawRectRotaGraph3F(point.X / (Program.bIsFullHD ? 1f : 1.5f), point.Y / (Program.bIsFullHD ? 1f : 1.5f), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, point1.X, point1.Y, this.Scale.X / (Program.bIsFullHD ? 1.0 : 1.5), this.Scale.Y / (Program.bIsFullHD ? 1.0 : 1.5), this.Rotation, this.nHandle, 1);
            DX.SetDrawBlendMode(0, 0);
        }

        public void DrawWhite(float x, float y, Rectangle? rectangle = null)
        {
            #region[ DrawWhite ]
            Point point = new Point();
            int SizeXBuf;
            int SizeYBuf;
            DX.GetGraphSize(this.nHandle, out SizeXBuf, out SizeYBuf);
            if (!rectangle.HasValue)
                rectangle = new Rectangle?(new Rectangle(0, 0, SizeXBuf, SizeYBuf));
            Rectangle rectangle1;
            switch (this.ReferencePoint)
            {
                case Texture.EReferencePoint.TopLeft:
                    point.X = 0;
                    point.Y = 0;
                    break;
                case Texture.EReferencePoint.TopCenter:
                    point.X = rectangle.Value.Width / 2;
                    point.Y = 0;
                    break;
                case Texture.EReferencePoint.TopRight:
                    point.X = rectangle.Value.Width;
                    point.Y = 0;
                    break;
                case Texture.EReferencePoint.CenterLeft:
                    point.X = 0;
                    ref Point local1 = ref point;
                    rectangle1 = rectangle.Value;
                    int num1 = rectangle1.Height / 2;
                    local1.Y = num1;
                    break;
                case Texture.EReferencePoint.Center:
                    point.X = rectangle.Value.Width / 2;
                    point.Y = rectangle.Value.Height / 2;
                    break;
                case Texture.EReferencePoint.CenterRight:
                    point.X = rectangle.Value.Width;
                    point.Y = rectangle.Value.Height / 2;
                    break;
                case Texture.EReferencePoint.BottomLeft:
                    point.X = 0;
                    ref Point local2 = ref point;
                    rectangle1 = rectangle.Value;
                    int height1 = rectangle1.Height;
                    local2.Y = height1;
                    break;
                case Texture.EReferencePoint.BottomCenter:
                    point.X = rectangle.Value.Width / 2;
                    point.Y = rectangle.Value.Height;
                    break;
                case Texture.EReferencePoint.BottomRight:
                    point.X = rectangle.Value.Width;
                    point.Y = rectangle.Value.Height;
                    break;
                default:
                    point.X = 0;
                    point.Y = 0;
                    break;
            }
            int n透明度 = (int)this.n透明度;
            DX.SetDrawBright(this.Color.R, this.Color.G, this.Color.B);
            DX.SetDrawBlendMode(10, n透明度);
            double num2 = x / (Program.bIsFullHD ? 1.0 : 1.5);
            double num3 = y / (Program.bIsFullHD ? 1.0 : 1.5);
            rectangle1 = rectangle.Value;
            int x1 = rectangle1.X;
            rectangle1 = rectangle.Value;
            int y1 = rectangle1.Y;
            rectangle1 = rectangle.Value;
            int width1 = rectangle1.Width;
            rectangle1 = rectangle.Value;
            int height2 = rectangle1.Height;
            double x2 = point.X;
            double y2 = point.Y;
            double ExtRateX1 = this.Scale.X / (Program.bIsFullHD ? 1.0 : 1.5);
            double ExtRateY1 = this.Scale.Y / (Program.bIsFullHD ? 1.0 : 1.5);
            double rotation1 = this.Rotation;
            int nHandle1 = this.nHandle;
            DX.DrawRectRotaGraph3F((float)num2, (float)num3, x1, y1, width1, height2, (float)x2, (float)y2, ExtRateX1, ExtRateY1, rotation1, nHandle1, 1);
            DX.SetDrawBlendMode(2, n透明度);
            double num4 = x / (Program.bIsFullHD ? 1.0 : 1.5);
            double num5 = y / (Program.bIsFullHD ? 1.0 : 1.5);
            rectangle1 = rectangle.Value;
            int x3 = rectangle1.X;
            rectangle1 = rectangle.Value;
            int y3 = rectangle1.Y;
            rectangle1 = rectangle.Value;
            int width2 = rectangle1.Width;
            rectangle1 = rectangle.Value;
            int height3 = rectangle1.Height;
            double x4 = point.X;
            double y4 = point.Y;
            double ExtRateX2 = this.Scale.X / (Program.bIsFullHD ? 1.0 : 1.5);
            double ExtRateY2 = this.Scale.Y / (Program.bIsFullHD ? 1.0 : 1.5);
            double rotation2 = this.Rotation;
            int nHandle2 = this.nHandle;
            DX.DrawRectRotaGraph3F((float)num4, (float)num5, x3, y3, width2, height3, (float)x4, (float)y4, ExtRateX2, ExtRateY2, rotation2, nHandle2, 1);
            DX.SetDrawBlendMode(0, n透明度);
            #endregion
        }

        public void DrawFlip(float x, float y, int FlipType)
        {
            int SizeXBuf;
            int SizeYBuf;
            DX.GetGraphSize(this.nHandle, out SizeXBuf, out SizeYBuf);
            int n透明度 = (int)this.n透明度;
            Color color = this.Color;
            int r = color.R;
            color = this.Color;
            int g = color.G;
            color = this.Color;
            int b = color.B;
            DX.SetDrawBright(r, g, b);
            DX.SetDrawBlendMode(1, n透明度);
            DX.DrawRotaGraph((int)(x / (Program.bIsFullHD ? 1.0 : 1.5)) + (int)(SizeXBuf / 2.0 / (Program.bIsFullHD ? 1.0 : 1.5)), (int)(y / (Program.bIsFullHD ? 1.0 : 1.5)) + (int)(SizeYBuf / 2.0 / (Program.bIsFullHD ? 1.0 : 1.5)), (this.Scale.X + this.Scale.Y) / 2.0 / (Program.bIsFullHD ? 1.0 : 1.5), 0.0, this.nHandle, 1, FlipType == 1 || FlipType == 2 ? 1 : 0, FlipType == 0 || FlipType == 2 ? 1 : 0);
        }

        public void SaveAsPNG(string path) => DX.SaveDrawValidGraphToPNG(this.nHandle, 0, 0, this.Size.X, this.Size.Y, path, 0);

        public Color Color { get; set; } = Color.White;

        public bool IsEnable { get; private set; }

        public Texture.EBlendMode BlendMode { get; set; }

        public string FileName { get; private set; }

        public float Rotation { get; set; }

        public Texture.EReferencePoint ReferencePoint { get; set; }

        public (int X, int Y) Size
        {
            get
            {
                int SizeXBuf;
                int SizeYBuf;
                DX.GetGraphSize(this.nHandle, out SizeXBuf, out SizeYBuf);
                return ((int)(float)SizeXBuf, (int)(float)SizeYBuf);
            }
        }

        private unsafe Bitmap ResizeBitmap(
          Bitmap original,
          int width,
          int height,
          InterpolationMode interpolationMode)
        {
            Graphics graphics = (Graphics)null;
            Bitmap bitmap1;
            try
            {
                PixelFormat format = original.PixelFormat;
                if (original.PixelFormat == PixelFormat.Format8bppIndexed)
                    format = PixelFormat.Format24bppRgb;
                bitmap1 = new Bitmap(width, height, format);
                RectangleF destRect = new RectangleF(0.0f, 0.0f, width, height);
                RectangleF srcRect = new RectangleF(-0.5f, -0.5f, original.Width, original.Height);
                graphics = Graphics.FromImage((Image)bitmap1);
                graphics.Clear(Color.Transparent);
                graphics.InterpolationMode = interpolationMode;
                graphics.DrawImage((Image)original, destRect, srcRect, GraphicsUnit.Pixel);
            }
            finally
            {
                graphics?.Dispose();
            }
            Bitmap bitmap2;
            if (original.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height, PixelFormat.Format8bppIndexed);
                ColorPalette palette = bitmap2.Palette;
                for (int index = 0; index < original.Palette.Entries.Length; ++index)
                    palette.Entries[index] = original.Palette.Entries[index];
                bitmap2.Palette = palette;
                BitmapData bitmapdata1 = bitmap1.LockBits(new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), ImageLockMode.ReadWrite, bitmap1.PixelFormat);
                BitmapData bitmapdata2 = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadWrite, bitmap2.PixelFormat);
                int stride1 = bitmapdata1.Stride;
                int stride2 = bitmapdata2.Stride;
                byte* scan0_1 = (byte*)(void*)bitmapdata1.Scan0;
                byte* scan0_2 = (byte*)(void*)bitmapdata2.Scan0;
                for (int index1 = 0; index1 < bitmapdata1.Height; ++index1)
                {
                    for (int index2 = 0; index2 < bitmapdata1.Width; ++index2)
                        scan0_2[index2 + index1 * stride2] = scan0_1[index2 * 3 + index1 * stride1];
                }
                bitmap2.UnlockBits(bitmapdata2);
                bitmap1.UnlockBits(bitmapdata1);
                bitmap1.Dispose();
            }
            else
                bitmap2 = bitmap1;
            return bitmap2;
        }

        public enum EBlendMode
        {
            None,
            Add,
            Subtract,
            Multiply,
        }

        public enum EReferencePoint
        {
            TopLeft,
            TopCenter,
            TopRight,
            CenterLeft,
            Center,
            CenterRight,
            BottomLeft,
            BottomCenter,
            BottomRight,
        }
    }
}
