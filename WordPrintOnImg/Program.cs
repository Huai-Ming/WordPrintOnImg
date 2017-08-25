using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPrintOnImg
{
    class Program
    {
        static void Main(string[] args)
        {
            //WriteTextOnImg();
            //CutPic();

            //Point[] myArray =
            // {
            //     new Point(363, 259),
            //     new Point(365, 257),
            //     new Point(368, 257),
            //     new Point(370, 257),
            //     new Point(373, 259),
            //     new Point(373, 262),
            //     new Point(373, 262),
            // };

            //// Create the path and add the curves.
            //GraphicsPath myPath = new GraphicsPath();
            //myPath.AddBeziers(myArray);

            //// Draw the path to the screen.
            //Pen myPen = new Pen(Color.Red, 2);
            //Bitmap bmp = new Bitmap(800, 800); // Determining Width and Height of Source Image
            //Graphics graphics = Graphics.FromImage(bmp);
            //graphics.DrawPath(myPen, myPath);
            //bmp.Save("test.jpg", ImageFormat.Jpeg);
            //graphics.Dispose();
            //bmp.Dispose();

            //Bitmap bmp = new Bitmap(500, 500); // Determining Width and Height of Source Image
            //Graphics g = Graphics.FromImage(bmp);
            //// Create a graphics path
            //CaptchaDrawer drawer = new CaptchaDrawer(500, 500);

            //// Draw path
            //Pen redPen = new Pen(Color.Red, 2);
            //g.DrawPath(redPen, drawer.GetCaptchaPath());
            //bmp.Save("test.jpg", ImageFormat.Jpeg);
            //// Dispose of objects
            //redPen.Dispose();
            //g.Dispose();

            SetClipPathCombine();
            Console.Read();
        }

        public static void SetClipPathCombine()
        {
            
            string oriPicPath = "iceberg_cut.jpg";
            System.Drawing.Image bitmap = (System.Drawing.Image)Bitmap.FromFile(oriPicPath); // set image     
            //draw the image object using a Graphics object    
            Bitmap bmp = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics graphicsImage = Graphics.FromImage(bmp);
            CaptchaDrawer drawer = new CaptchaDrawer(bitmap.Width, bitmap.Height);
            var path = drawer.GetCaptchaPath();
            path.CloseAllFigures();
            //graphicsImage.SetClip(path);
            graphicsImage.SetClip(path);
            //graphicsImage.FillPath(new SolidBrush(Color.Red), path);
            graphicsImage.DrawImage(bitmap, new Point(0,0));

           
            bmp.Save("test1.jpg", ImageFormat.Jpeg);
            System.Drawing.Image test1 = (System.Drawing.Image)Bitmap.FromFile("test1.jpg");
            var temp = new Bitmap(test1, new Size(50, 50));
            temp.Save("test2.jpg", ImageFormat.Jpeg);
            graphicsImage.Dispose();
            bitmap.Dispose();
        }

        public static void CutPic()
        {
            string oriPicPath ="iceberg_cut.jpg";//原始图片全路径
            string m_CutPicPath = System.IO.Path.GetExtension(oriPicPath);
            m_CutPicPath = oriPicPath.Substring(0, oriPicPath.Length - m_CutPicPath.Length) + "_cut" + m_CutPicPath;//裁剪图片全路径

            int bIsSave = cutPicAndSave(oriPicPath, m_CutPicPath, 10, 10, 150, 150);
        }

        public static int cutPicAndSave(string oldPath, string newPath, int x, int y, int width, int height)
        {

            Rectangle cropArea = new System.Drawing.Rectangle(x, y, width, height); //要截取的区域大小
            Image img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(System.IO.File.ReadAllBytes(oldPath)));//加载图片

            //判断超出的位置否
            if ((img.Width < x + width) || img.Height < y + height)
            {
                //logger.Warn("截取的区域超过了图片本身的高度、宽度.");
                img.Dispose();
                return 0;
            }
            Bitmap bmpImage = new System.Drawing.Bitmap(img);
           
          

            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            bmpCrop.Save(newPath);//保存成新文件
            ChangeOpacity(img);
            //释放对象
            img.Dispose();
            bmpCrop.Dispose();
            return 1;

        }


      

        public static void ChangeOpacity(Image img)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = 10;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            bmp.Save("iceberg_transpareny.jpg", ImageFormat.Jpeg);
            bmp.Dispose();
        }


        private static void WriteTextOnImg()
        {
//creating a image object    
            System.Drawing.Image bitmap = (System.Drawing.Image) Bitmap.FromFile("iceberg.jpg"); // set image     
            //draw the image object using a Graphics object    
            Graphics graphicsImage = Graphics.FromImage(bitmap);

            //Set the alignment based on the coordinates       
            StringFormat stringformat = new StringFormat();
            stringformat.Alignment = StringAlignment.Far;
            stringformat.LineAlignment = StringAlignment.Far;

            StringFormat stringformat2 = new StringFormat();
            stringformat2.Alignment = StringAlignment.Center;
            stringformat2.LineAlignment = StringAlignment.Center;

            //Set the font color/format/size etc..      
            Color StringColor = System.Drawing.ColorTranslator.FromHtml("#933eea"); //direct color adding    
            Color StringColor2 = System.Drawing.ColorTranslator.FromHtml("#e80c88"); //customise color adding    
            string Str_TextOnImage = "学"; //Your Text On Image    
            string Str_TextOnImage2 = "宇"; //Your Text On Image    

            graphicsImage.DrawString(Str_TextOnImage, new Font("arial", 40,
                FontStyle.Regular), new SolidBrush(StringColor), new Point(130, 245),
                stringformat);


            graphicsImage.DrawString(Str_TextOnImage2, new Font("Edwardian Script ITC", 111,
                FontStyle.Bold), new SolidBrush(StringColor2), new Point(345, 255),
                stringformat2);

           
            bitmap.Save("iceberg1.jpg", ImageFormat.Jpeg);
        }
    }
}
