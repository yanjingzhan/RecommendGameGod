using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace UtilityLibs
{
    public class ImageHelper
    {
        public static void MakeThumbnail(string imgPath_old, string imgPath_new, int width, int height, string mode, string type)
        {

            System.Drawing.Image img = System.Drawing.Image.FromFile(imgPath_old);
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = img.Width;
            int oh = img.Height;

            switch (mode.ToUpper())
            {
                case "HW":  //指定高宽缩放（可能变形） 
                    break;
                case "W":  //指定宽，高按比例
                    if (width > img.Width)
                    {
                        towidth = img.Width;
                        toheight = img.Height;
                    }
                    else
                    {
                        toheight = img.Height * width / img.Width;
                    }
                    break;
                case "H":  //指定高，宽按比例
                    if (height > img.Height)
                    {
                        toheight = img.Height;
                        towidth = img.Width;
                    }
                    towidth = img.Width * height / img.Height;
                    break;
                case "CUT":   //指定高宽裁减（不变形） 
                    if ((double)img.Width / (double)img.Height > (double)towidth / (double)toheight)
                    {
                        oh = img.Height;
                        ow = img.Height * towidth / toheight;
                        y = 0;
                        x = (img.Width - ow) / 2;
                    }
                    else
                    {
                        ow = img.Width;
                        oh = img.Width * height / towidth;
                        x = 0;
                        y = (img.Height - oh) / 2;
                    }
                    break;
                case "DB":    // 按值较大的进行等比缩放（不变形）
                    if (width > img.Width && height > img.Height)
                    {
                        toheight = img.Height;
                        towidth = img.Width;
                    }
                    else if ((double)img.Width / (double)towidth < (double)img.Height / (double)toheight)
                    {
                        toheight = height;
                        towidth = img.Width * height / img.Height;
                    }
                    else
                    {
                        towidth = width;
                        toheight = img.Height * width / img.Width;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(img, new System.Drawing.Rectangle(0, 0, towidth, toheight),
            new System.Drawing.Rectangle(x, y, ow, oh),
            System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //保存缩略图
                switch (type.ToUpper())
                {
                    case "JPG":
                        bitmap.Save(imgPath_new, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "GIF":
                        bitmap.Save(imgPath_new, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "PNG":
                        bitmap.Save(imgPath_new, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case "BMP":
                        bitmap.Save(imgPath_new, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                img.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth"></param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>

        public static void GetPicThumbnail(string sFile, string dFile, int dWidth, int dHeight, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);

            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放

            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100

            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }

                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
            }
            catch
            {
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }

        }
    }
}
