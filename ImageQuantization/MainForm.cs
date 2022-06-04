using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        Stopwatch stopwatch = new Stopwatch();
        public static RGBPixel[] distincitColors;
        public RGBPixel[] pallette;
        ProjectOperations.Edge[] mst;
        public MainForm()
        {
            InitializeComponent();
        }

        public static RGBPixel[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                stopwatch.Reset();
                stopwatch.Start();
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            stopwatch.Stop();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            try
            {
                stopwatch.Start();
                int K = int.Parse(textBox1.Text.ToString());
                pallette = new RGBPixel[K];
                ProjectOperations.ExtractKClustersFromMST(mst, distincitColors, K);
                pallette = ProjectOperations.calculatingAverage(pallette, distincitColors);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Enter K as A number \n " + exception.ToString());
            }
            
            ProjectOperations.replaceImage(pallette,distincitColors);
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            stopwatch.Stop();
            textBox2.Text = stopwatch.Elapsed.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            stopwatch.Start();
            ProjectOperations.GlobalDataForProjectOperationsClass.setValues();
            distincitColors = ProjectOperations.getDistincitColorsInOriginalImage().ToArray();
            mst = new ProjectOperations.Edge[distincitColors.Length - 1];
            ProjectOperations.constructGraph(distincitColors);
            mst = ProjectOperations.calculateMST(mst , distincitColors.Length);
            distincitColorText.Text = ProjectOperations.GlobalDataForProjectOperationsClass.distincitColorsCounter.ToString();
            mstCostText.Text = ProjectOperations.GlobalDataForProjectOperationsClass.mstCost.ToString();
            stopwatch.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stopwatch.Start();
            ProjectOperations.GlobalDataForProjectOperationsClass.resetKClusterValues();
            textBox1.Text = ProjectOperations.autoDetectionForKClusters(mst).ToString();
            stopwatch.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                stopwatch.Start();
                int K = int.Parse(textBox1.Text.ToString());
                pallette = new RGBPixel[K];
                ProjectOperations.ExtractKClustersFromMST(mst, distincitColors, K);
                pallette = ProjectOperations.calculatingAverage(pallette, distincitColors);
            }
            catch (Exception)
            {
                MessageBox.Show("Enter K as A number \n ");
            }
            ProjectOperations.replaceImage(pallette, distincitColors);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            stopwatch.Stop();
            textBox2.Text = stopwatch.Elapsed.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                stopwatch.Start();
                int K = int.Parse(textBox1.Text.ToString());
                pallette = new RGBPixel[K];

                /*////2nd Bonus////*/
                List<ProjectOperations.Edge> newMST = new List<ProjectOperations.Edge>(mst);
                pallette  = ProjectOperations.determineKClustersInBetterWay(newMST, distincitColors, K).ToArray();

            }
            catch (Exception ee)
            {
                MessageBox.Show("Enter K as A number \n " + ee.ToString());
            }
            ProjectOperations.replaceImage(pallette, distincitColors);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            stopwatch.Stop();
            textBox2.Text = stopwatch.Elapsed.ToString();
        }
    }
}