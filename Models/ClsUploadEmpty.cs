using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.parser.LocationTextExtractionStrategy;
using Rectangle = iTextSharp.text.Rectangle;

namespace new_SDLC.Models
{
    public class ClsUploadEmpty
    {
        public string ProcessPdf(string uName, List<string> uploadData)
        {
            string resultFunction = "";
            string resultFn = "";

            if (GetNameLatestFolder() != "")
            {
                GetFilePath(uName, GetNameLatestFolder(), ref resultFn);

                if ( !resultFn.Contains("Err") )
                {
                    string sourcePdfPath = resultFn;
                    string destinationPdfPath = sourcePdfPath + ".temp"; // Ubah dengan path ke file tujuan

                    using (PdfReader pdfReader = new PdfReader(sourcePdfPath))
                    {
                        using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(destinationPdfPath, FileMode.Create)))
                        {
                            for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                            {
                                PdfContentByte pdfContentByte = pdfStamper.GetOverContent(i);
                                var strategy = new MyLocationTextExtractionStrategy();
                                PdfTextExtractor.GetTextFromPage(pdfReader, i, strategy);

                                // Set font and size for the added text
                                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                pdfContentByte.SetFontAndSize(bf, 12);

                                foreach (var p in strategy.myPoints)
                                {
                                    //if (p.Text.Contains("13/03/2024") || p.Text.Contains("19/03/2024")) //bagian ini yang looping karena bisa jadi ada banyak tanggal
                                    //{
                                    //    pdfContentByte.BeginText();
                                    //    pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Cuti", (p.Rect.Right + 312), p.Rect.Bottom, 0);
                                    //    pdfContentByte.EndText();
                                    //}

                                    // Looping melalui list
                                    foreach (var item in uploadData)
                                    {
                                        // Pisahkan tanggal dan deskripsi menggunakan separator '_'
                                        string[] parts = item.Split(new[] { '_' }, 2); // Split dengan maksimal 2 bagian: tanggal dan deskripsi
                                        string tanggal = parts[0];
                                        string deskripsi = parts.Length > 1 ? parts[1] : "";

                                        // Di sini Anda bisa melakukan validasi dan penggunaan tanggal dan deskripsi
                                        Console.WriteLine($"Tanggal: {tanggal}, Deskripsi: {deskripsi}");

                                        // Misal validasi p.Text mengandung tanggal
                                        if (p.Text.Contains(tanggal)) // Ganti kondisi sesuai kebutuhan
                                        {
                                            pdfContentByte.BeginText();
                                            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, deskripsi, (p.Rect.Right + 312), p.Rect.Bottom, 0);
                                            pdfContentByte.EndText();
                                        }
                                    }

                                }
                            }

                            pdfStamper.Close();
                        }
                    }

                    // Langkah 2: Menghapus file sumber
                    if (File.Exists(sourcePdfPath))
                    {
                        File.Delete(sourcePdfPath);
                    }

                    // Langkah 3: Mengganti nama file sementara, menghilangkan '.temp'
                    if (File.Exists(destinationPdfPath))
                    {
                        File.Move(destinationPdfPath, sourcePdfPath);
                    }

                }

                //return jika error lain 
                else
                {
                    resultFunction = resultFn;
                }

            }

            return resultFunction;

        }

        public string GetNameLatestFolder()
        {
            string pathToSearch = @"\\kphodeco407\absensi2";

            // Dapatkan semua direktori/subfolder di lokasi tertentu
            var directories = Directory.GetDirectories(pathToSearch);

            // Filter direktori yang sesuai dengan format 'yyyyMM' dan urutkan dari yang terbaru
            var latestFolder = directories
                .Where(dir => System.IO.Path.GetFileName(dir).Length == 6 && int.TryParse(System.IO.Path.GetFileName(dir), out _)) // Pastikan nama folder 6 karakter dan numerik
                .OrderByDescending(dir => dir)
                .FirstOrDefault(); // Ambil yang paling terbaru

            if (latestFolder != null)
            {
                return pathToSearch + "\\" + System.IO.Path.GetFileName(latestFolder);
            }
            else
            {
                return "";
            }
        }

        public void GetFilePath(string uname, string pathToSearch, ref string sentFn)
        {
            string searchPattern = $"*{uname.ToUpper()}*.pdf";

            try
            {
                // Dapatkan semua file PDF yang mengandung 'DEVXX' dalam nama file
                var matchingFiles = Directory.GetFiles(pathToSearch, searchPattern, SearchOption.AllDirectories);

                if (matchingFiles.Any())
                {
                    // Ambil path lengkap dari file pertama yang ditemukan
                    string sourcePath = matchingFiles.First();
                    sentFn = sourcePath;
                }
                else
                {
                    Console.WriteLine("Err No File Found");
                    sentFn = "Err No File Found";
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Err Access Folder : {ex.Message}");
                sentFn = "Err Access Folder";
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Err No Access Folder : {ex.Message}");
                sentFn = "Err No Access Folder";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Err Something Wrong : {ex.Message}");
                sentFn = "Err Something Wrong";
            }
        }

    }

    public class MyLocationTextExtractionStrategy : LocationTextExtractionStrategy
    {
        public List<RectAndText> myPoints = new List<RectAndText>();

        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);
            LineSegment segment = renderInfo.GetBaseline();
            Vector start = segment.GetStartPoint();
            Vector end = segment.GetEndPoint();
            float x = start[Vector.I1];
            float y = start[Vector.I2];
            float width = end[Vector.I1] - start[Vector.I1];
            float height = renderInfo.GetAscentLine().GetEndPoint()[Vector.I2] - y;
            var rect = new iTextSharp.text.Rectangle(x, y, x + width, y + height);
            myPoints.Add(new RectAndText(rect, renderInfo.GetText()));
        }

        public class RectAndText
        {
            public Rectangle Rect;
            public String Text;
            public RectAndText(Rectangle rect, String text)
            {
                this.Rect = rect;
                this.Text = text;
            }
        }
    }
}