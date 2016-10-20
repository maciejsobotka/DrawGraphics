using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string graphicPath = "";
        private bool graphicLoaded = false;
        private bool graphicNew = false;
        private Shapes shape = Shapes.None;
        private Operations operation = Operations.None;
        private Point p;
        private Point p1;
        private Point p2;
        private VectorDraw vd;
        private Brush color;
        
        public MainWindow()
        {
            InitializeComponent();
            vd = new VectorDraw();
            color = Brushes.White;
            p.X = 0.0;
            p.Y = 0.0;
        }

        //=====================================================================
        // Menu
        private void NewFile_Click(object sender, EventArgs e)
        {
            if (graphicLoaded || graphicNew)
            {
                ClearObjects();
                textBoxSource.Text = "New File";
            }
            canvas.Background = Brushes.White;
            graphicNew = true;
            graphicLoaded = false;
            textBoxSource.Text = "";
            textBoxSource.Foreground = Brushes.Black;
            textBoxSource.FontStyle = FontStyles.Normal;
            textBoxSource.Text = "New File";
            EnableButtons();
            saveFile.Foreground = Brushes.White;
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XAML |*.XAML";
            dlg.Multiselect = false;  // default
            dlg.ShowDialog();
            graphicPath = dlg.FileName;
            
            textBoxSource.Text = "";
            textBoxSource.Foreground = Brushes.Black;
            textBoxSource.FontStyle = FontStyles.Normal;
            textBoxSource.Text = dlg.SafeFileName;

            if (graphicNew || graphicLoaded)
            {
                ClearObjects();
            }
            canvas.Background = Brushes.White;
            graphicLoaded = true;
            graphicNew = false;
            EnableButtons();

            FileStream file = new FileStream(graphicPath, FileMode.Open);
            Canvas c = (Canvas) XamlReader.Load(file);
            while (c.Children.Count > 0)
            {
                Shape s = (Shape) c.Children[0];
                s.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                listOfObjects.Items.Add(vd.ParametersToString(s));
                c.Children.RemoveAt(0);
                canvas.Children.Add(s);
            }
            textBoxSource.Text = canvas.Children[0].ToString();

            saveFile.Foreground = Brushes.White;
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("graphic.xaml", FileMode.Create, FileAccess.Write);
            XamlWriter.Save(canvas, file);
            foreach (var shape in canvas.Children)
                XamlWriter.Save(shape);
            file.Close();
        }

        private void menuOption_MouseEnter(object sender, MouseEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            switch (menuItem.Name)
            {
                case "newFile":
                    newFile.Foreground = Brushes.Black;
                    break;
                case "openFile":
                    openFile.Foreground = Brushes.Black;
                    break;
                case "saveFile":
                    if (graphicNew || graphicLoaded)
                        saveFile.Foreground = Brushes.Black;
                    break;
            }
        }

        private void menuOption_MouseLeave(object sender, MouseEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            switch (menuItem.Name)
            {
                case "newFile":
                    newFile.Foreground = Brushes.White;
                    break;
                case "openFile":
                    openFile.Foreground = Brushes.White;
                    break;
                case "saveFile":
                    if(graphicNew || graphicLoaded)
                        saveFile.Foreground = Brushes.White;
                    break;
            }
        }

        private void EnableButtons()
        {
            lineButton.IsEnabled = true;
            ellipseButton.IsEnabled = true;
            rectangleButton.IsEnabled = true;
            triangleButton.IsEnabled = true;
            paintButton.IsEnabled = true;
        }

        private void ClearObjects()
        {
            canvas.Children.Clear();
            listOfObjects.Items.Clear();
        }

        //=====================================================================
        // Options management
        private void shapeButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;

            switch (senderButton.Name)
            {
                case "lineButton":
                    shape = Shapes.Line;
                    break;
                case "ellipseButton":
                    shape = Shapes.Ellipse;
                    break;
                case "rectangleButton":
                    shape = Shapes.Rectangle;
                    break;
                case "triangleButton":
                    shape = Shapes.Triangle;
                    break;
            }
            operation = Operations.None;
            p1 = p;
            p2 = p;
        }

        private void paintButton_Click(object sender, RoutedEventArgs e)
        {
            operation = Operations.Paint;
        }

        //=====================================================================
        // Vector graphics
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (operation == Operations.None)
            {
                if (shape == Shapes.Line)
                    if (p1 == p) p1 = Mouse.GetPosition(canvas);
                    else if (p2 == p)
                    {
                        p2 = Mouse.GetPosition(canvas);
                        Line line = vd.CreateLine(p1, p2, 2, Brushes.Black);
                        line.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                        canvas.Children.Add(line);
                        listOfObjects.Items.Add(vd.ParametersToString(line));
                        p1 = p;
                        p2 = p;
                    }
                if (shape == Shapes.Ellipse)
                    if (p1 == p) p1 = Mouse.GetPosition(canvas);
                    else if (p2 == p)
                    {
                        p2 = Mouse.GetPosition(canvas);
                        Ellipse ellipse = vd.CreateEllipse(p1, p2, 2, Brushes.Black, color);
                        ellipse.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                        canvas.Children.Add(ellipse);
                        listOfObjects.Items.Add(vd.ParametersToString(ellipse));
                        p1 = p;
                        p2 = p;
                    }
                if (shape == Shapes.Rectangle)
                    if (p1 == p) p1 = Mouse.GetPosition(canvas);
                    else if (p2 == p)
                    {
                        p2 = Mouse.GetPosition(canvas);
                        Rectangle rectangle = vd.CreateRectangle(p1, p2, 2, Brushes.Black, color);
                        rectangle.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                        canvas.Children.Add(rectangle);
                        listOfObjects.Items.Add(vd.ParametersToString(rectangle));
                        p1 = p;
                        p2 = p;
                    }
                if (shape == Shapes.Triangle)
                    if (p1 == p) p1 = Mouse.GetPosition(canvas);
                    else if (p2 == p)
                    {
                        p2 = Mouse.GetPosition(canvas);
                        Polygon triangle = vd.CreateTriangle(p1, p2, 2, Brushes.Black, color);
                        triangle.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                        canvas.Children.Add(triangle);
                        listOfObjects.Items.Add(vd.ParametersToString(triangle));
                        p1 = p;
                        p2 = p;
                    }
            }
        }

        private void shape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (operation == Operations.Paint) {
                int index = canvas.Children.IndexOf((Shape)sender);
                ((Shape) canvas.Children[index]).Fill = color;
            }
        }

        private void colorButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;

            switch (senderButton.Name)
            {
                case "whiteButton":
                    color = Brushes.White;
                    break;
                case "grayButton":
                    color = Brushes.Gray;
                    break;
                case "blackButton":
                    color = Brushes.Black;
                    break;
                case "redButton":
                    color = Brushes.Red;
                    break;
                case "greenButton":
                    color = Brushes.Green;
                    break;
                case "blueButton":
                    color = Brushes.Blue;
                    break;
            }
        }
    }
}
