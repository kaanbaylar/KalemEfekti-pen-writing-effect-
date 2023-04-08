using System;
using System.Drawing;
using Sony.Vegas;

public class KalemEfekti : ICommand
{
    public void Execute()
    {
        // Aktif proje ve zaman �izelgesi al�n�r
        Project proj = Vegas.Project;
        Timecode projLength = proj.Length;

        // Se�ili klipler al�n�r
        TrackEvent ev = Vegas.Project.GetSelectedEvent();
        VideoEvent vidEvent = ev as VideoEvent;

        // Klipler �zerinde i�lemler yap�l�r
        if (vidEvent != null)
        {
            // Kalem efekti i�in yeni bir video efekti olu�turulur
            VideoFX kalemEfekti = new VideoFX("Kalem Efekti");

            // Kalem efektinin parametreleri ayarlan�r
            kalemEfekti.Parameters["Thickness"].Value = 2;
            kalemEfekti.Parameters["Color"].Value = Color.Red;

            // G�r�nt� boyutlar� al�n�r
            int width = vidEvent.Video.Width;
            int height = vidEvent.Video.Height;

            // �izim i�lemi i�in bir Bitmap nesnesi olu�turulur
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);

            // Bitmap nesnesi Graphics s�n�f�na d�n��t�r�l�r
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

            // �izim rengi ayarlan�r
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 2);

            // Yukar�dan a�a�� do�ru bir �izim i�lemi ger�ekle�tirilir
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y == 0)
                    {
                        graphics.DrawLine(pen, x, y, x, y + 1);
                    }
                    else
                    {
                        System.Drawing.Color pixelColor = bitmap.GetPixel(x, y - 1);
                        graphics.DrawLine(pen, x, y, x, y + 1);
                        bitmap.SetPixel(x, y, pixelColor);
                    }
                }
            }

            // Bitmap nesnesi video stream'e d�n��t�r�l�r
            VideoStream stream = new VideoStream(bitmap);

            // Efekt klibe uygulan�r
            vidEvent.VideoFX.AddEffect(kalemEfekti);
            vidEvent.VideoMedia.SetStream(stream);
        }

        // Script tamamland�
        Vegas.ShowMessage("Kalem efekti ba�ar�yla eklendi.");
    }
}
