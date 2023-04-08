using System;
using System.Drawing;
using Sony.Vegas;

public class KalemEfekti : ICommand
{
    public void Execute()
    {
        // Aktif proje ve zaman çizelgesi alınır
        Project proj = Vegas.Project;
        Timecode projLength = proj.Length;

        // Seçili klipler alınır
        TrackEvent ev = Vegas.Project.GetSelectedEvent();
        VideoEvent vidEvent = ev as VideoEvent;

        // Klipler üzerinde işlemler yapılır
        if (vidEvent != null)
        {
            // Kalem efekti için yeni bir video efekti oluşturulur
            VideoFX kalemEfekti = new VideoFX("Kalem Efekti");

            // Kalem efektinin parametreleri ayarlanır
            kalemEfekti.Parameters["Thickness"].Value = 2;
            kalemEfekti.Parameters["Color"].Value = Color.Red;

            // Görüntü boyutları alınır
            int width = vidEvent.Video.Width;
            int height = vidEvent.Video.Height;

            // Çizim işlemi için bir Bitmap nesnesi oluşturulur
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);

            // Bitmap nesnesi Graphics sınıfına dönüştürülür
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

            // Çizim rengi ayarlanır
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 2);

            // Yukarıdan aşağı doğru bir çizim işlemi gerçekleştirilir
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

            // Bitmap nesnesi video stream'e dönüştürülür
            VideoStream stream = new VideoStream(bitmap);

            // Efekt klibe uygulanır
            vidEvent.VideoFX.AddEffect(kalemEfekti);
            vidEvent.VideoMedia.SetStream(stream);
        }

        // Script tamamlandı
        Vegas.ShowMessage("Kalem efekti başarıyla eklendi.");
    }
}
