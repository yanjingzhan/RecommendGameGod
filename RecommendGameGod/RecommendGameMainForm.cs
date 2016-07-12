﻿using Models;
using RecommendGameGod.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityLibs;

namespace RecommendGameGod
{
    public partial class RecommendGameMainForm : Form
    {
        List<PictureBox> _screenShotPictureList;
        List<TextBox> _textBoxScreenShotList;
        string _tempDir;
        public RecommendGameMainForm()
        {
            InitializeComponent();

            this.comboBox_GameType.SelectedIndex = 0;
            this.comboBox_PhoneVersion.SelectedIndex = 0;
            this.comboBox_Price.SelectedIndex = 0;

            textBox_GameName.Focus();

            _screenShotPictureList = new List<PictureBox>
            {
                pictureBox_Images1,
                pictureBox_Images2,
                pictureBox_Images3,
                pictureBox_Images4,
                pictureBox_Images5,
                pictureBox_Images6,
                pictureBox_Images7,
                pictureBox_Images8,
            };

            _textBoxScreenShotList = new List<TextBox>
            {
                this.textBox_Images1,
                this.textBox_Images2,
                this.textBox_Images3,
                this.textBox_Images4,
                this.textBox_Images5,
                this.textBox_Images6,
                this.textBox_Images7,
                this.textBox_Images8
            };

            _tempDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
            if (!Directory.Exists(_tempDir))
            {
                Directory.CreateDirectory(_tempDir);
            }
        }


        private string CheckControlState()
        {
            if (string.IsNullOrWhiteSpace(textBox_GameName.Text))
            {
                return "游戏名称不能为空";
            }

            if (string.IsNullOrWhiteSpace(textBox_GameID.Text))
            {
                return "游戏ID不能为空";
            }

            if (string.IsNullOrWhiteSpace(textBox_Version.Text))
            {
                return "游戏版本号不能为空";
            }

            if (string.IsNullOrWhiteSpace(textBox_PusherName.Text))
            {
                return "游戏发布者不能为空";
            }

            if (string.IsNullOrWhiteSpace(textBox_FileSize.Text))
            {
                return "游戏大小不能为空";
            }

            if (string.IsNullOrWhiteSpace(textBox_DownloadCount.Text))
            {
                return "游戏下载量不能为空";
            }

            if (string.IsNullOrWhiteSpace(textBox_GameDetails.Text))
            {
                return "游戏描述不能为空";
            }

            if (checkBox_SourceType.Checked)
            {
                if (this.pictureBox_Header.Image == null)
                {
                    return "该游戏为轮播，应该添加轮播图片";
                }
            }

            if (checkBox_LogoPath.Checked)
            {
                if (this.pictureBox_LogoPath.Image == null)
                {
                    return "请添加Logo";
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(textBox_LogoPath.Text))
                {
                    return "请添加Logo文件的Url";
                }
            }

            if (checkBox_UploadImages.Checked)
            {
                if (this.pictureBox_Images1.Image == null)
                {
                    return "请添加截屏";
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(this.textBox_Images1.Text))
                {
                    return "请添加截屏文件的Url";
                }
            }

            return "200";
        }

        private async void button_OK_Click(object sender, EventArgs e)
        {
            string info = CheckControlState();
            if (info != "200")
            {
                SetInfo(info);
                return;
            }

            SetInfo(string.Format("{0},开始提交", this.textBox_GameName.Text));
            await Task.Delay(TimeSpan.FromSeconds(0.1));

            GameModel gameModel_t = new GameModel
            {
                DownloadCount = this.textBox_DownloadCount.Text,
                FileSize = this.textBox_FileSize.Text,
                GameDetails = this.textBox_GameDetails.Text,
                GameID = this.textBox_GameID.Text,
                GameName = this.textBox_GameName.Text,
                GameType = this.comboBox_GameType.Text,
                PhoneVersion = this.comboBox_PhoneVersion.Text,
                Price = this.comboBox_Price.Text,
                PusherName = this.textBox_PusherName.Text,
                SourceType = this.checkBox_SourceType.Checked ? "header" : "list",
                Version = this.textBox_Version.Text,
                UpdateTime = this.dateTimePicker_UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
            };

            if (!checkBox_LogoPath.Checked)
            {
                gameModel_t.LogoPath = this.textBox_LogoPath.Text;
            }
            else
            {
                ImageHelper.GetPicThumbnail(pictureBox_LogoPath.ImageLocation, Path.Combine(_tempDir, this.textBox_GameName.Text + "_logo.png"),
                    pictureBox_LogoPath.Image.Width, pictureBox_LogoPath.Image.Height, 80);

                HttpDataHelper.UploadImage(Path.Combine(_tempDir, this.textBox_GameName.Text + "_logo.png"));
                gameModel_t.LogoPath = string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_logo.png",
                    this.textBox_GameName.Text);
            }

            if (checkBox_SourceType.Checked)
            {
                ImageHelper.GetPicThumbnail(this.pictureBox_Header.ImageLocation, Path.Combine(_tempDir, this.textBox_GameName.Text + "_header.png"),
                   pictureBox_Header.Image.Width, pictureBox_Header.Image.Height, 80);

                HttpDataHelper.UploadImage(Path.Combine(_tempDir, this.textBox_GameName.Text + "_header.png"));
                gameModel_t.HeadImage = string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_header.png",
                    this.textBox_GameName.Text);
            }

            if (!checkBox_UploadImages.Checked)
            {
                gameModel_t.Images1 = this.textBox_Images1.Text;
                gameModel_t.Images2 = this.textBox_Images2.Text;
                gameModel_t.Images3 = this.textBox_Images3.Text;
                gameModel_t.Images4 = this.textBox_Images4.Text;
                gameModel_t.Images5 = this.textBox_Images5.Text;
                gameModel_t.Images6 = this.textBox_Images6.Text;
                gameModel_t.Images7 = this.textBox_Images7.Text;
                gameModel_t.Images8 = this.textBox_Images8.Text;
            }
            else
            {
                for (int i = 0; i < _screenShotPictureList.Count; i++)
                {
                    if (_screenShotPictureList[i].Image != null)
                    {
                        ImageHelper.GetPicThumbnail(_screenShotPictureList[i].ImageLocation, Path.Combine(_tempDir, this.textBox_GameName.Text + "_image" + (i + 1).ToString() + ".png"),
                            _screenShotPictureList[i].Image.Width, _screenShotPictureList[i].Image.Height, 80);

                        HttpDataHelper.UploadImage(Path.Combine(_tempDir, this.textBox_GameName.Text + "_image" + (i + 1).ToString() + ".png"));
                    }
                }

                gameModel_t.Images1 = string.IsNullOrWhiteSpace(pictureBox_Images1.ImageLocation) ? string.Empty :
                    string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_image1.png", this.textBox_GameName.Text);

                gameModel_t.Images2 = string.IsNullOrWhiteSpace(pictureBox_Images2.ImageLocation) ? string.Empty :
                    string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_image2.png", this.textBox_GameName.Text);

                gameModel_t.Images3 = string.IsNullOrWhiteSpace(pictureBox_Images3.ImageLocation) ? string.Empty :
                    string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_image3.png", this.textBox_GameName.Text);

                gameModel_t.Images4 = string.IsNullOrWhiteSpace(pictureBox_Images4.ImageLocation) ? string.Empty :
                    string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_image4.png", this.textBox_GameName.Text);

                gameModel_t.Images5 = string.IsNullOrWhiteSpace(pictureBox_Images5.ImageLocation) ? string.Empty :
                    string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_image5.png", this.textBox_GameName.Text);

                gameModel_t.Images6 = string.IsNullOrWhiteSpace(pictureBox_Images6.ImageLocation) ? string.Empty :
                    string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_image6.png", this.textBox_GameName.Text);

                gameModel_t.Images7 = string.IsNullOrWhiteSpace(pictureBox_Images7.ImageLocation) ? string.Empty :
                    string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_image7.png", this.textBox_GameName.Text);

                gameModel_t.Images8 = string.IsNullOrWhiteSpace(pictureBox_Images8.ImageLocation) ? string.Empty :
                    string.Format("http://recommendgames.pettostudio.net/resoures/wp/images/{0}_image8.png", this.textBox_GameName.Text);

            }

            HttpDataHelper.AddGame(gameModel_t);

            SetInfo(string.Format("{0},提交成功", this.textBox_GameName.Text));
            ClearAll();
        }

        private void ClearAll()
        {
            this.textBox_DownloadCount.Clear();
            this.textBox_FileSize.Clear();
            this.textBox_GameDetails.Clear();
            this.textBox_GameID.Clear();
            this.textBox_GameName.Clear();
            this.textBox_Images1.Clear();
            this.textBox_Images2.Clear();
            this.textBox_Images3.Clear();
            this.textBox_Images4.Clear();
            this.textBox_Images5.Clear();
            this.textBox_Images6.Clear();
            this.textBox_Images7.Clear();
            this.textBox_Images8.Clear();
            this.textBox_LogoPath.Clear();
            this.textBox_PusherName.Clear();
            this.textBox_Version.Clear();
            this.pictureBox_Header.Image = null;
            this.pictureBox_Header.ImageLocation = null;
            this.pictureBox_Images1.Image = null;
            this.pictureBox_Images1.ImageLocation = null;
            this.pictureBox_Images2.Image = null;
            this.pictureBox_Images2.ImageLocation = null;
            this.pictureBox_Images3.Image = null;
            this.pictureBox_Images3.ImageLocation = null;
            this.pictureBox_Images4.Image = null;
            this.pictureBox_Images4.ImageLocation = null;
            this.pictureBox_Images5.Image = null;
            this.pictureBox_Images5.ImageLocation = null;
            this.pictureBox_Images6.Image = null;
            this.pictureBox_Images6.ImageLocation = null;
            this.pictureBox_Images7.Image = null;
            this.pictureBox_Images7.ImageLocation = null;
            this.pictureBox_Images8.Image = null;
            this.pictureBox_Images8.ImageLocation = null;
            this.pictureBox_LogoPath.Image = null;
            this.pictureBox_LogoPath.ImageLocation = null;

        }
        private void SetInfo(string msg)
        {
            this.textBox_Info.AppendText(DateTime.Now.ToString() + "," + msg);
            this.textBox_Info.AppendText(Environment.NewLine);
        }

        private void checkBox_SourceType_CheckedChanged(object sender, EventArgs e)
        {
            this.pictureBox_Header.Visible = checkBox_SourceType.Checked;
        }

        private void checkBox_LogoPath_CheckedChanged(object sender, EventArgs e)
        {
            this.pictureBox_LogoPath.Visible = checkBox_LogoPath.Checked;
        }

        private void checkBox_UploadImages_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox_Images1.Visible = pictureBox_Images2.Visible = pictureBox_Images3.Visible = pictureBox_Images4.Visible
                = pictureBox_Images5.Visible = pictureBox_Images6.Visible = pictureBox_Images7.Visible = pictureBox_Images8.Visible = checkBox_UploadImages.Checked;
        }

        private void pictureBox_LogoPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "png|*.png";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (Path.GetExtension(ofd.FileName).ToLower() == ".png")
                    {
                        using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                            if (image.Width == 300 && image.Height == 300)
                            {
                                (sender as PictureBox).ImageLocation = ofd.FileName;
                            }
                            else
                            {
                                SetInfo("Logo必须为300*300");
                            }
                        }
                    }
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if ((sender as PictureBox).Image != null)
                {
                    (sender as PictureBox).Image = null;
                    (sender as PictureBox).ImageLocation = null;
                }
            }
        }

        private void pictureBox_Images_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "png|*.png";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (Path.GetExtension(ofd.FileName).ToLower() == ".png")
                    {
                        (sender as PictureBox).ImageLocation = ofd.FileName;
                    }
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if ((sender as PictureBox).Image != null)
                {
                    (sender as PictureBox).Image = null;
                    (sender as PictureBox).ImageLocation = null;
                }
            }
        }

        private void button_ImportAllImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "png|*.png";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var item in ofd.FileNames)
                {
                    using (FileStream fs = new FileStream(item, FileMode.Open, FileAccess.Read))
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                        if (image.Width == 300 && image.Height == 300)
                        {
                            this.pictureBox_LogoPath.ImageLocation = item;
                        }
                        else
                        {
                            foreach (var p in _screenShotPictureList)
                            {
                                if (string.IsNullOrWhiteSpace(p.ImageLocation))
                                {
                                    p.ImageLocation = item;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.textBox_DownloadCount.Text = "100";
            //this.textBox_FileSize.Text = "51";
            //this.textBox_GameDetails.Text = "textBox_GameDetails";
            //this.textBox_GameID.Text = "textBox_GameID";
            //this.textBox_GameName.Text = "textBox_GameName";
            //this.textBox_PusherName.Text = "textBox_PusherName";
            //this.textBox_Version.Text = "textBox_Version";

            if (string.IsNullOrWhiteSpace(this.textBox_GameID.Text))
            {
                SetInfo("请填写游戏ID才可以自动获取呦！");
                return;
            }

            GameInfoInStore gameInfo_t = XMLHelper.GetGameInfoInStore(HttpDataHelper.GetGameXMLInfo(this.textBox_GameID.Text));
            this.dateTimePicker_UpdateTime.Value = DateTime.Parse(gameInfo_t.Updated.Replace("T", " ").Replace("Z", ""));
            this.textBox_DownloadCount.Text = new Random().Next(1500, 8000).ToString();
            this.textBox_FileSize.Text = gameInfo_t.PackageSize;
            this.textBox_GameDetails.Text = gameInfo_t.Content;
            this.textBox_GameName.Text = gameInfo_t.Title;
            this.textBox_LogoPath.Text = gameInfo_t.Image;
            this.textBox_PusherName.Text = gameInfo_t.Publisher;
            this.textBox_Version.Text = gameInfo_t.Version;

            for (int i = 0; i < gameInfo_t.screenshots.Count; i++)
            {
                _textBoxScreenShotList[i].Text = gameInfo_t.screenshots[i];
            }

            if (gameInfo_t.ClientTypes.Contains("WindowsPhone71"))
            {
                this.comboBox_PhoneVersion.Text = "WP7";
            }

            for (int i = 0; i < this.comboBox_GameType.Items.Count; i++)
            {
                if(this.comboBox_GameType.GetItemText(this.comboBox_GameType.Items[i]).ToLower() == gameInfo_t.Category.ToLower())
                {
                    this.comboBox_GameType.SelectedIndex = i;
                }
            }

            SetInfo("自动填写完毕，请提交！");
        }

    }
}