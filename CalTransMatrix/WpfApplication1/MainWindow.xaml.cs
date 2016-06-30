using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectSensor sensor;
        private MultiSourceFrameReader multiFrameSourceReader;
        
        private int counter = 100;

        private WriteableBitmap bitmap;

        private CoordinateMapper coordinateMapper;

        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7)/8;
        private FrameDescription colorFrameDescription;
        private ushort[] _depthData;
        private byte[] _colorFrameData;

        public ImageSource ImageSource => bitmap;

        int colorSpaceWidth;
        int colorSpaceHeight;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            sensor = KinectSensor.GetDefault();

            multiFrameSourceReader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth);
            multiFrameSourceReader.MultiSourceFrameArrived += multi_FrameArrived;

            coordinateMapper = sensor.CoordinateMapper;

            colorFrameDescription = sensor.ColorFrameSource.FrameDescription;

            colorSpaceWidth = colorFrameDescription.Width;
            colorSpaceHeight = colorFrameDescription.Height;

            // allocate space to put the pixels being received and converted
            //depthPixels = new byte[depthFrameDescription.Width*depthFrameDescription.Height];

            // create the bitmap to display
            bitmap = new WriteableBitmap(colorSpaceWidth, colorSpaceHeight, 96.0, 96.0, PixelFormats.Bgra32, null);

            DataContext = this;
            sensor.Open();

        }

        private void multi_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            DepthFrame depthFrame = null;
            ColorFrame colorFrame = null;

            MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();
            if (multiSourceFrame == null)
            {
                return;
            }

            try
            {
                depthFrame = multiSourceFrame.DepthFrameReference.AcquireFrame();
                colorFrame = multiSourceFrame.ColorFrameReference.AcquireFrame();

                // If any frame has expired by the time we process this event, return.
                // The "finally" statement will Dispose any that are not null.
                if ((depthFrame == null) || (colorFrame == null))
                {
                    return;
                }

                if (counter > -1)
                {
                    counter--;
                }
                    
                if (counter == 0)
                {
                    if (_colorFrameData == null)
                    {
                        int size = colorSpaceWidth * colorSpaceHeight;
                        _colorFrameData = new byte[size * bytesPerPixel];
                    }
                    colorFrame.CopyConvertedFrameDataToArray(_colorFrameData, ColorImageFormat.Bgra);
                    //////////////////////////////////////////////////////
                    var depthDesc = depthFrame.FrameDescription;
                    uint pixelNum = depthDesc.LengthInPixels;
                    if (_depthData == null)
                    {
                        _depthData = new ushort[pixelNum];
                    }
                    depthFrame.CopyFrameDataToArray(_depthData);

                    CameraSpacePoint[] cameraSpacePoints = new CameraSpacePoint[pixelNum];
                    ColorSpacePoint[] colorSpacePoints = new ColorSpacePoint[pixelNum];

                    coordinateMapper.MapDepthFrameToCameraSpace(_depthData, cameraSpacePoints);
                    coordinateMapper.MapDepthFrameToColorSpace(_depthData, colorSpacePoints);

                    double[,] spaceAndRGB = new double[pixelNum, 6];

                    ushort maxDepth = 2000;
                    ushort minDepth = 500;

                    List<int> rList = new List<int>();

                    Debug.Print(pixelNum.ToString());
                    // we need a starting point, let's pick 0 for now
                    for (int index = 0; index < pixelNum; index++)
                    {
                        ushort depth = _depthData[index];
                        if (depth < minDepth || depth > maxDepth)
                        {
                            continue;
                        }
                        ColorSpacePoint point = colorSpacePoints[index];
                        CameraSpacePoint spacePoint = cameraSpacePoints[index];
                        
                        if (spacePoint.X == 0)
                        {
                            continue;
                        }

                        // round down to the nearest pixel
                        int colorX = (int)Math.Floor(point.X + 0.5);
                        int colorY = (int)Math.Floor(point.Y + 0.5);

                        // make sure the pixel is part of the image
                        if ((colorX >= 0 && (colorX < colorSpaceWidth) && (colorY >= 0) && (colorY < colorSpaceHeight)))
                        {
                            int colorImageIndex = ((colorSpaceWidth * colorY) + colorX) * bytesPerPixel;

                            byte b = _colorFrameData[colorImageIndex];
                            byte g = _colorFrameData[colorImageIndex + 1];
                            byte r = _colorFrameData[colorImageIndex + 2];
                            byte a = _colorFrameData[colorImageIndex + 3];


                            spaceAndRGB[index, 0] = spacePoint.X;
                            spaceAndRGB[index, 1] = spacePoint.Y;
                            spaceAndRGB[index, 2] = spacePoint.Z;
                            spaceAndRGB[index, 3] = r;
                            spaceAndRGB[index, 4] = g;
                            spaceAndRGB[index, 5] = b;

                            if (r < 140 || g > r - 80 || b > r - 80)
                            {
                                continue;
                            }
                            rList.Add(index);
                        }
                    }

                    StreamWriter file = new StreamWriter(@"C:\Users\Leo\Dropbox\Matlab\pointcloud.txt");
                    
                    foreach (int index in rList)
                    {
                        file.WriteLine("{0} {1} {2} {3} {4} {5}",
                            spaceAndRGB[index, 0], spaceAndRGB[index, 1], spaceAndRGB[index, 2],
                            spaceAndRGB[index, 3], spaceAndRGB[index, 4], spaceAndRGB[index, 5]);
                    }

                    file.Close();
                    Debug.Print("Write File End");
                    // Process Color
                    bitmap.WritePixels(
                        new Int32Rect(0, 0, colorSpaceWidth, colorSpaceHeight),
                        _colorFrameData,
                        colorSpaceWidth * bytesPerPixel,
                        0);
                }


            }
            finally
            {
                if (depthFrame != null)
                {
                    depthFrame.Dispose();
                }

                if (colorFrame != null)
                {
                    colorFrame.Dispose();
                }
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (multiFrameSourceReader != null)
            {
                // MultiSourceFrameReder is IDisposable
                multiFrameSourceReader.Dispose();
                multiFrameSourceReader = null;
            }

            if (sensor != null)
            {
                sensor.Close();
                sensor = null;
            }
        }
    }
}

//            using (KinectBuffer depthBuffer = depthFrame.LockImageBuffer())
//                {
//                    // verify data and write the color data to the display bitmap
//                    if (((depthFrameDescription.Width * depthFrameDescription.Height) == (depthBuffer.Size / depthFrameDescription.BytesPerPixel)) &&
//                        (depthFrameDescription.Width == depthBitmap.PixelWidth) && (depthFrameDescription.Height == depthBitmap.PixelHeight))
//                    {
//                        // If you wish to filter by reliable depth distance, uncomment the following line:
//                        ushort maxDepth = depthFrame.DepthMaxReliableDistance;
//
//                        //Debug.Print("{0}, {1}", depthFrame.DepthMinReliableDistance, maxDepth);
//
//                        
//                        ProcessDepthFrameData(depthBuffer.UnderlyingBuffer, depthBuffer.Size, depthFrame.DepthMinReliableDistance, 2500);
//                        depthFrameProcessed = true;
//                    }
//                }
//
//            }
//            if (depthFrameProcessed)
//            {
//                RenderDepthPixels();
//            }
//        }
//
//        private unsafe void ProcessDepthFrameData(IntPtr depthFrameData, uint depthFrameDataSize, ushort minDepth, ushort maxDepth)
//        {
//            // depth frame data is a 16 bit value
//            ushort* frameData = (ushort*)depthFrameData;
//
//            //Debug.Print("counter: {0}", counter);
//            if (counter > -1)
//            {
//                counter--;
//            }
//
//            if (counter == 0)
//            {
//                int pixelNum = (int)(depthFrameDataSize / depthFrameDescription.BytesPerPixel);
//                CameraSpacePoint[] points = new CameraSpacePoint[pixelNum];
//                ushort[] pixels = new ushort[pixelNum];
//
//                int validPixel = 0;
//                for (int i = 0; i < pixelNum; ++i)
//                {
//                    if (frameData[i] >= minDepth && frameData[i] <= maxDepth)
//                    {
//                        pixels[validPixel++] = frameData[i];
//                    }
//                }
//
//                CoordinateMapper mapper = sensor.CoordinateMapper;
//                mapper.MapDepthFrameToCameraSpace(pixels, points);
//                
//                
//                using (StreamWriter file =
//                    new StreamWriter(@"C:\Users\Leo\Desktop\pytest\pointcloud.txt"))
//                {
//                    Debug.Print("Write File");
//                    Debug.Print("Valid Num: {0}", validPixel);
//                    for (int i = 0; i < validPixel; i++)
//                    {
//                        file.WriteLine("{0} {1} {2}", points[i].X, points[i].Y, points[i].Z);
//                    }
//                }
//
//                using (StreamWriter file =
//                    new StreamWriter(@"C:\Users\Leo\Desktop\pytest\depth.txt"))
//                {
//                    Debug.Print("Write File");
//                    file.WriteLine("Valid Num: {0}", validPixel);
//
//                    for (int i = 0; i < validPixel; i++)
//                    {
//                        file.WriteLine("Depth {0}", pixels[i]);
//                    }
//                }
//                Debug.Print("Write File End");
//            }
//            
//
//            // convert depth to a visual representation
//            for (int i = 0; i < (int)(depthFrameDataSize / depthFrameDescription.BytesPerPixel); ++i)
//            {
//                // Get the depth for this pixel
//                ushort depth = frameData[i];
//                //Debug.Print("{0}", depth);
//
//                // To convert to a byte, we're mapping the depth value to the byte range.
//                // Values outside the reliable depth range are mapped to 0 (black).
//                depthPixels[i] = (byte)(depth >= minDepth && depth <= maxDepth ? (depth / MapDepthToByte) : 0);
//            }
//        }
//        
//        private void RenderDepthPixels()
//        {
//            depthBitmap.WritePixels(
//                new Int32Rect(0, 0, depthBitmap.PixelWidth, depthBitmap.PixelHeight),
//                depthPixels,
//                depthBitmap.PixelWidth,
//                0);
//
//        }


//
//        
//    }
//}
