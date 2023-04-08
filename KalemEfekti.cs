using System;
using System.Drawing;
using Sony.Vegas;

public class KalemEfekti : ICommand
{
    public void Execute()
    {
        // Aktif proje ve zaman çizelgesi alýnýr
        Project proj = Vegas.Project;
        Timecode projLength = proj.Length;

        // Seçili klipler alýnýr
        TrackEvent ev = Vegas.Project.GetSelectedEvent();
        VideoEvent vidEvent = ev as VideoEvent;

        // Klipler üzerinde iþlemler yapýlýr
        if (vidEvent != null)
        {
            // Kalem efekti için yeni bir video efekti oluþturulur
            VideoFX kalemEfekti = new VideoFX("Kalem Efekti");

            // Kalem efektinin parametreleri ayarlanýr
            kalemEfekti.Parameters["Thickness"].Value = 2;
            kalemEfekti.Parameters["Color"].Value = Color.Red;

            // Görüntü boyutlarý alýnýr
            int width = vidEvent.Video.Width;
            int height = vidEvent.Video.Height;

            // Çizim iþlemi için bir Bitmap nesnesi oluþturulur
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);

            // Bitmap nesnesi Graphics sýnýfýna dönüþtürülür
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

            // Çizim rengi ayarlanýr
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 2);

            // Yukarýdan aþaðý doðru bir çizim iþlemi gerçekleþtirilir
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

            // Bitmap nesnesi video stream'e dönüþtürülür
            VideoStream stream = new VideoStream(bitmap);

            // Efekt klibe uygulanýr
            vidEvent.VideoFX.AddEffect(kalemEfekti);
            vidEvent.VideoMedia.SetStream(stream);
        }

        // Script tamamlandý
        Vegas.ShowMessage("Kalem efekti baþarýyla eklendi.");
    }
}
