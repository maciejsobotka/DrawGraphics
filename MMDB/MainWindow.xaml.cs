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
        private Shapes shapeType = Shapes.None;
        private Operations operationType = Operations.None;
        private Point p;
        private Point p1;
        private Point p2;
        private VectorDraw vd;
        private Brush color;
        private Shape clickedShape;
        
        public MainWindow()
        {
            InitializeComponent();
            vd = new VectorDraw();
            color = Brushes.White;
            colorRectangle.Fill = color;
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
                s.MouseMove += new MouseEventHandler(shape_MouseMove);
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
            grabButton.IsEnabled = true;
            removeButton.IsEnabled = true;
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
                    shapeType = Shapes.Line;
                    break;
                case "ellipseButton":
                    shapeType = Shapes.Ellipse;
                    break;
                case "rectangleButton":
                    shapeType = Shapes.Rectangle;
                    break;
                case "triangleButton":
                    shapeType = Shapes.Triangle;
                    break;
            }
            operationType = Operations.None;
            p1 = p;
            p2 = p;
        }

        private void operationButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;

            switch (senderButton.Name)
            {
                case "paintButton":
                    operationType = Operations.Paint;
                    break;
                case "grabButton":
                    operationType = Operations.Grab;
                    break;
                case "removeButton":
                    operationType = Operations.Remove;
                    break;
            }
            shapeType = Shapes.None;
        }

        //=====================================================================
        // Vector graphics
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (operationType == Operations.None && shapeType != Shapes.None)
            {
                Shape shape;
                if (p1 == p) p1 = Mouse.GetPosition(canvas);
                else if (p2 == p)
                {
                    p2 = Mouse.GetPosition(canvas);
                    switch (shapeType)
                    {
                        case Shapes.Line:
                            shape = vd.CreateLine(p1, p2, 2, Brushes.Black);
                            break;
                        case Shapes.Ellipse:
                            shape = vd.CreateEllipse(p1, p2, 2, Brushes.Black, color);
                            break;
                        case Shapes.Rectangle:
                            shape = vd.CreateRectangle(p1, p2, 2, Brushes.Black, color);
                            break;
                        case Shapes.Triangle:
                            shape = vd.CreateTriangle(p1, p2, 2, Brushes.Black, color);
                            break;
                        default:
                            shape = new Line();
                            break;
                    }
                    shape.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                    shape.MouseMove += new MouseEventHandler(shape_MouseMove);
                    canvas.Children.Add(shape);
                    listOfObjects.Items.Add(vd.ParametersToString(shape));
                    p1 = p;
                    p2 = p;
                }
            }
        }

        private void shape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            clickedShape = (Shape)sender;
            switch(operationType)
            {
                case Operations.Paint:
                    int index = canvas.Children.IndexOf((Shape)sender);
                    ((Shape) canvas.Children[index]).Fill = color;
                    break;
                case Operations.Grab:
                    p1 = Mouse.GetPosition(canvas);
                    break;
                case Operations.Remove:
                    index = canvas.Children.IndexOf((Shape)sender);
                    canvas.Children.RemoveAt(index);
                    listOfObjects.Items.RemoveAt(index);
                    break;
            }
        }

        private void shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (operationType == Operations.Grab)
            {
                p2 = Mouse.GetPosition(canvas);
                textBoxSource.Text = p2.X.ToString() + " x " + p2.Y.ToString();
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    int index = canvas.Children.IndexOf(clickedShape);
                    Thickness t = ((Shape)canvas.Children[index]).Margin;
                    double leftMargin = t.Left - (p1.X - p2.X);
                    double topMargin = t.Top - (p1.Y - p2.Y);
                    ((Shape)canvas.Children[index]).Margin = new Thickness(leftMargin, topMargin, 0, 0);
                    p1 = p2;
                    listOfObjects.Items[index] = vd.ParametersToString(clickedShape);
                }
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
            colorRectangle.Fill = color;
        }
    }
}
