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

namespace imageTranslate
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Bitmap> bitmapDict;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "이미지 파일 (*.jpg; *.png; *.bmp; *.gif)|*.jpg; *.png; *.bmp; *.gif";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bitmapDict = new Dictionary<string, Bitmap>();
                    // Panel 생성 (PictureBox들을 포함할 컨테이너)
                    imagePanel.AutoScroll = true; // 스크롤바 활성화 (이미지가 너무 많을 때 스크롤 가능)

                    // Panel을 MainForm에 추가
                    Controls.Add(imagePanel);

                    int x = 10; // PictureBox의 초기 x 좌표
                    int y = 10; // PictureBox의 초기 y 좌표

                    // 선택한 이미지 파일들을 처리
                    foreach (string imagePath in openFileDialog.FileNames)
                    {
                        // PictureBox 생성 및 설정
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        Bitmap loadedBitmap = new Bitmap(imagePath);
                        pictureBox.Image = loadedBitmap;
                        pictureBox.Location = new Point(x, y);

                        pictureBox.Width = loadedBitmap.Width;
                        pictureBox.Width = loadedBitmap.Height;

                        // Panel에 PictureBox 추가
                        imagePanel.Controls.Add(pictureBox);
                        bitmapDict.Add(imagePath, loadedBitmap);

                        // 다음 PictureBox의 위치 계산 (나란히 나열)
                        x += pictureBox.Image.Width +
                             Convert.ToInt32(pictureBox.Image.Width * 0.1); // 이미지 간 간격을 조절하려면 10을 다른 값으로 변경
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bitmapDict.Count > 0)
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    // 다운로드 폴더를 디폴트로 설정
                    folderBrowserDialog.RootFolder = Environment.SpecialFolder.UserProfile;
                    folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Document";
                    
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var r in bitmapDict)
                        {
                            string key = r.Key.ToString();
                            string filename = "";
                            if (key.Contains("."))
                            {
                                filename = Path.GetFileNameWithoutExtension(key);
                            }
                            string savePath = folderBrowserDialog.SelectedPath + "\\" + filename + ".bmp" ;
                            r.Value.Save(savePath, System.Drawing.Imaging.ImageFormat.Bmp);
                        }

                        MessageBox.Show("이미지가 성공적으로 저장되었습니다.");
                    }
                }
            }
            else
            {
                MessageBox.Show("먼저 이미지를 불러와야 합니다.");
            }
        }
    }
}