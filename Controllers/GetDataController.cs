using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;

using System.Text;
using System.Collections;

namespace WebApplication5.Controllers
{
    public class GetDataController : Controller
    {
        //
        // GET: /GetData/
        public ActionResult Index()
        {
            if (Session["zbrojeva"] != null) { if (((Dictionary<DateTime, int>)Session["zbrojeva"]).Count > 0)ViewBag.podataka = true; else ViewBag.podataka = false; }
            else ViewBag.podataka = false;
            return View();
        }

        public FileContentResult GetChart()
        {
            
            return File(Chart(), "image/png");
       
        }


        public FileStreamResult GetPdf()
        {
            var doc = new Document();
            //var pdf = Server.MapPath("Chart.pdf");
            //string pdf = @"d:\chart.pdf";
            MemoryStream stream = new MemoryStream();
           PdfWriter pdfWriter= PdfWriter.GetInstance(doc, stream);
           pdfWriter.CloseStream = false;
            doc.Open();

            //doc.Add(new Paragraph("MMR"));
            var image = iTextSharp.text.Image.GetInstance(Chart());
            image.Rotation = (1.57f);
            image.ScalePercent(80f);
            doc.Add(image);
            doc.Close();
            stream.Flush(); //Always catches me out
            stream.Position = 0; //Not sure if this is required
            return File(stream, "application/pdf", "Chart.pdf");
        }

        public FileStreamResult GetTxt()
        {
            //todo: add some data from your database into that string:
            Dictionary<DateTime, int> zbrojeva = (Dictionary<DateTime, int>)Session["zbrojeva"];
            var string_with_your_data = String.Join(System.Environment.NewLine, zbrojeva.Select(x => x.Key + "\t" + x.Value).ToArray());

            var byteArray = Encoding.ASCII.GetBytes(string_with_your_data);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "Chart.txt");
        }


        private Byte[] Chart()
        {

        

            //var db = new NorthwindDataContext();
            //var query = from o in db.Orders
            //            group o by o.Employee
            //                into g
            //                select new
            //                {
            //                    Employee = g.Key,
            //                    NoOfOrders = g.Count()
            //                };
            Dictionary<DateTime, int> zbrojeva = (Dictionary<DateTime, int>)Session["zbrojeva"];

            var chart = new Chart
            {
                Width = 900,
                Height = 600,
                RenderType = RenderType.ImageTag,
                AntiAliasing = AntiAliasingStyles.All,
                TextAntiAliasingQuality = TextAntiAliasingQuality.High
            };

            chart.Titles.Add("MMR GRAPH");
            chart.Titles[0].Font = new Font("Arial", 16f);

            chart.ChartAreas.Add("");
            chart.ChartAreas[0].AxisX.Title = zbrojeva.First().Key.ToString("dd/MM/yyyy") + "-" + zbrojeva.Last().Key.ToString("dd/MM/yyyy");
            chart.ChartAreas[0].AxisY.Title = "MMR";
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12f);
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12f);
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 10f);
            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            chart.ChartAreas[0].BackColor = Color.White;


            chart.ChartAreas[0].BackColor = Color.Transparent;
            chart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            chart.ChartAreas[0].AxisY.IsLabelAutoFit = false;
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chart.ChartAreas[0].AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chart.ChartAreas[0].AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);




            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;


            chart.Legends.Add(CreateLegend());

            chart.Series.Add("Solo MMR");
            chart.Series[0].ChartType = SeriesChartType.Line;

            //Dictionary<DateTime, int> zbrojeva = new Dictionary<DateTime, int> { { DateTime.Now, 5688 }, { DateTime.Now +TimeSpan.FromHours(1),5664} };

            //string[] lines = System.IO.File.ReadAllLines(@"D:\VS2013\PortableSteam\PortableSteam\bin\Debug\76561198026710641.txt");
            //var dict = lines.Select(l => l.Split('\t')).ToDictionary(a => a[0], a => Convert.ToInt32(a[1]));



            chart.ChartAreas[0].AxisX.Interval = Math.Max(1, Math.Floor(((double)zbrojeva.Count / 30)));
            chart.ChartAreas[0].AxisY.Minimum = (Math.Round((((double)zbrojeva.Values.Min()) - 100) / 100, 0) * 100);
            chart.ChartAreas[0].AxisY.Maximum = (Math.Round((((double)zbrojeva.Values.Max()) + 100) / 100, 0) * 100); ;


            List<string> lista1 = new List<string>();
            List<int> lista2 = new List<int>();
            int brojac = 0;
            int broj2 = zbrojeva.Count - 1;
            foreach (var q in zbrojeva)
            {
               
                lista1.Add((broj2-brojac) + " (" + q.Key.ToString("dd/MM/yyyy") + ")");
                lista2.Add (q.Value);
                brojac++;
            }
            for (int i = lista1.Count - 1; i >= 0; i--)
            {
                chart.Series[0].Points.AddXY(lista1[i], lista2[i]);
            }

            using (var chartimage = new MemoryStream())
            {
                chart.SaveImage(chartimage, ChartImageFormat.Png);
                return chartimage.GetBuffer();
            }
        }

        [NonAction]
        public Legend CreateLegend()
        {
            Legend legend = new Legend();
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 9);
            legend.LegendStyle = LegendStyle.Row;

            return legend;
        }

	}
}