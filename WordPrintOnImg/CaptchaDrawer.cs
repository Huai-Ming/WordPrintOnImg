using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPrintOnImg
{
    public class CaptchaDrawer
    {
        //验证码的左上角(起点)的x y
        private int mCaptchaWidth = 50;
        private int mCaptchaHeight = 50;

        //验证码滑块的宽高
        private int mCaptchaX;
        private int mCaptchaY;

        //控件的宽高
        protected int mWidth;
        protected int mHeight;

        //验证码 阴影、抠图的Path
        private GraphicsPath mCaptchaPath;

        private Random random = new Random();

        public CaptchaDrawer(int width, int height)
        {
            this.mWidth = width;
            this.mHeight = height;
        }

        public GraphicsPath GetCaptchaPath()
        {
            mCaptchaPath = new GraphicsPath();
            CreateCaptchaPath();
            return mCaptchaPath;
        }

        public int CaptchaX
        {
            get { return mCaptchaX; }
        }

        public int CaptchaY
        {
            get { return mCaptchaY; }
        }

        public void CreateCaptchaPath()
        {
            //原本打算随机生成gap，后来发现 宽度/3 效果比较好，
            int gap = random.Next(mCaptchaWidth / 2);
            gap = mCaptchaWidth / 3;

            //随机生成验证码阴影左上角 x y 点，
            mCaptchaX = random.Next(mWidth - mCaptchaWidth - gap);
            mCaptchaY = random.Next(mHeight - mCaptchaHeight - gap);

            mCaptchaPath.Reset();


            //从左上角开始 绘制一个不规则的阴影
            mCaptchaPath.AddLine(mCaptchaX, mCaptchaY, mCaptchaX + gap, mCaptchaY);

            //左上角
            DrawPartCircle(new PointF(mCaptchaX + gap, mCaptchaY), new PointF(mCaptchaX + gap*2, mCaptchaY),
                mCaptchaPath, random.Next(2) > 0);
            mCaptchaPath.AddLine(mCaptchaX + gap * 2, mCaptchaY, mCaptchaX + mCaptchaWidth, mCaptchaY);

            mCaptchaPath.AddLine(mCaptchaX + mCaptchaWidth, mCaptchaY, mCaptchaX + mCaptchaWidth, mCaptchaY + gap);
            //右上角
            DrawPartCircle(new PointF(mCaptchaX + mCaptchaWidth, mCaptchaY + gap),
                new PointF(mCaptchaX + mCaptchaWidth, mCaptchaY + gap * 2),
                mCaptchaPath, random.Next(2) > 0);
            mCaptchaPath.AddLine(mCaptchaX + mCaptchaWidth, mCaptchaY + 2 * gap, mCaptchaX + mCaptchaWidth, mCaptchaY + mCaptchaHeight);

            mCaptchaPath.AddLine(mCaptchaX + mCaptchaWidth, mCaptchaY + mCaptchaHeight, mCaptchaX + 2* gap, mCaptchaY + mCaptchaHeight);
            //右下角
            DrawPartCircle(new PointF(mCaptchaX + mCaptchaWidth - gap, mCaptchaY + mCaptchaHeight),
                new PointF(mCaptchaX + mCaptchaWidth - gap * 2, mCaptchaY + mCaptchaHeight),
                mCaptchaPath, random.Next(2) > 0);
            mCaptchaPath.AddLine(mCaptchaX + gap, mCaptchaY + mCaptchaHeight, mCaptchaX, mCaptchaY + mCaptchaHeight);
            mCaptchaPath.AddLine(mCaptchaX, mCaptchaY + mCaptchaHeight, mCaptchaX, mCaptchaY + 2* gap);
            //左下角
            DrawPartCircle(new PointF(mCaptchaX, mCaptchaY + mCaptchaHeight - gap),
                new PointF(mCaptchaX, mCaptchaY + mCaptchaHeight - gap * 2),
                mCaptchaPath, random.Next(2) > 0);
            mCaptchaPath.AddLine(mCaptchaX, mCaptchaY + gap, mCaptchaX, mCaptchaY);

        }

        public void DrawPartCircle(PointF start, PointF end, GraphicsPath path, bool outer)
        {
            float c = 0.551915024494f;
            PointF middle = new PointF(start.X + (end.X - start.X) / 2, start.Y + (end.Y - start.Y) / 2);
            float r1 = (float)Math.Sqrt(Math.Pow((middle.X - start.X), 2) + Math.Pow((middle.Y - start.Y), 2));
            float gap1 = r1 * c;

            if (Math.Abs(start.X - end.X) < 0.1)
            {
                bool topToBottom = end.Y - start.Y > 0 ? true : false;

                int flag; //旋转系数

                if (topToBottom)
                {
                    flag = 1;
                }
                else
                {
                    flag = -1;
                }
                if (outer)
                {
                    PointF[] array = new PointF[7];
                    var point1 = new PointF((int)(start.X + gap1 * flag), (int)start.Y);
                    var point2 = new PointF((int)(middle.X + r1 * flag), (int)(middle.Y - gap1 * flag));
                    var point3 = new PointF((int)(middle.X + r1 * flag), (int)middle.Y);
                    var point4 = new PointF((int)(middle.X + r1 * flag), (int)(middle.Y + gap1 * flag));
                    var point5 = new PointF((int)(end.X + gap1 * flag), (int)end.Y);
                    var point6 = new PointF((int)(end.X), (int)end.Y);
                    array[0] = point1;
                    array[1] = point2;
                    array[2] = point3;
                    array[3] = point4;
                    array[4] = point5;
                    array[5] = point6;
                    array[6] = point6;
                    path.AddBeziers(array);
                    //凸的 两个半圆
                    //path

                    //path.(start.X + gap1 * flag, start.y,
                    //        middle.X + r1 * flag, middle.y - gap1 * flag,
                    //        middle.x + r1 * flag, middle.y);
                    //path.cubicTo(middle.x + r1 * flag, middle.y + gap1 * flag,
                    //        end.x + gap1 * flag, end.y,
                    //        end.x, end.y);
                }
                else
                {
                    PointF[] array = new PointF[7];
                    var point1 = new PointF((int)(start.X - gap1 * flag), (int)start.Y);
                    var point2 = new PointF((int)(middle.X - r1 * flag), (int)(middle.Y - gap1 * flag));
                    var point3 = new PointF((int)(middle.X - r1 * flag), (int)middle.Y);
                    var point4 = new PointF((int)(middle.X - r1 * flag), (int)(middle.Y + gap1 * flag));
                    var point5 = new PointF((int)(end.X - gap1 * flag), (int)end.Y);
                    var point6 = new PointF((int)(end.X), (int)end.Y);
                    array[0] = point1;
                    array[1] = point2;
                    array[2] = point3;
                    array[3] = point4;
                    array[4] = point5;
                    array[5] = point6;
                    array[6] = point6;
                    path.AddBeziers(array);
                    ////凹的 两个半圆
                    //path.cubicTo(start.x - gap1 * flag, start.y,
                    //        middle.x - r1 * flag, middle.y - gap1 * flag,
                    //        middle.x - r1 * flag, middle.y);
                    //path.cubicTo(middle.x - r1 * flag, middle.y + gap1 * flag,
                    //        end.x - gap1 * flag, end.y,
                    //        end.x, end.y);
                }
            }
            else
            {
                bool leftToRight = end.X - start.X > 0 ? true : false;
                //以下是我写出了所有的计算公式后推的，不要问我过程，只可意会。
                int flag;//旋转系数
                if (leftToRight)
                {
                    flag = 1;
                }
                else
                {
                    flag = -1;
                }
                if (outer)
                {
                    PointF[] array = new PointF[7];
                    var point1 = new PointF((int)(start.X), (int)(start.Y - gap1 * flag));
                    var point2 = new PointF((int)(middle.X - gap1 * flag), (int)(middle.Y - r1 * flag));
                    var point3 = new PointF((int)(middle.X), (int)(middle.Y - r1 * flag));
                    var point4 = new PointF((int)(middle.X + gap1 * flag), (int)(middle.Y - r1 * flag));
                    var point5 = new PointF((int)(end.X), (int)(end.Y - gap1 * flag));
                    var point6 = new PointF((int)(end.X), (int)end.Y);
                    array[0] = point1;
                    array[1] = point2;
                    array[2] = point3;
                    array[3] = point4;
                    array[4] = point5;
                    array[5] = point6;
                    array[6] = point6;
                    path.AddBeziers(array);

                    ////凸 两个半圆
                    //path.cubicTo(start.x, start.y - gap1 * flag,
                    //        middle.x - gap1 * flag, middle.y - r1 * flag,
                    //        middle.x, middle.y - r1 * flag);
                    //path.cubicTo(middle.x + gap1 * flag, middle.y - r1 * flag,
                    //        end.x, end.y - gap1 * flag,
                    //        end.x, end.y);
                }
                else
                {
                    PointF[] array = new PointF[7];
                    var point1 = new PointF((int)(start.X), (int)(start.Y + gap1 * flag));
                    var point2 = new PointF((int)(middle.X - gap1 * flag), (int)(middle.Y + r1 * flag));
                    var point3 = new PointF((int)(middle.X), (int)(middle.Y + r1 * flag));
                    var point4 = new PointF((int)(middle.X + gap1 * flag), (int)(middle.Y + r1 * flag));
                    var point5 = new PointF((int)(end.X), (int)(end.Y + gap1 * flag));
                    var point6 = new PointF((int)(end.X), (int)end.Y);
                    array[0] = point1;
                    array[1] = point2;
                    array[2] = point3;
                    array[3] = point4;
                    array[4] = point5;
                    array[5] = point6;
                    array[6] = point6;
                    path.AddBeziers(array);
                    ////凹 两个半圆
                    //path.cubicTo(start.x, start.y + gap1 * flag,
                    //        middle.x - gap1 * flag, middle.y + r1 * flag,
                    //        middle.x, middle.y + r1 * flag);
                    //path.cubicTo(middle.x + gap1 * flag, middle.y + r1 * flag,
                    //        end.x, end.y + gap1 * flag,
                    //        end.x, end.y);
                }
            }
        }
    }
}
