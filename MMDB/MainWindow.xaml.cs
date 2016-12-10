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
        private Point p1;
        private Point p2;
        private VectorDraw vd;
        private Brush color, color2;
        private Shape clickedShape;
        private bool shapeCreated = false;
        private EditWindow editWindow = new EditWindow();
        private SearchWindow searchWindow;
        public MainWindow()
        {
            InitializeComponent();
            vd = new VectorDraw();
            color = Brushes.White;
            color2 = Brushes.Black;
            fillRectangle.Fill = color;
            strokeRectangle.Fill = color2;
            this.KeyDown += new KeyEventHandler(Window_KeyDown);
            editWindow.MyEvent += new EventHandler(childWindow_MyEvent);
        }

        //=====================================================================
        // Menu
        private void NewFile_Click(object sender, EventArgs e)
        {
            if (graphicLoaded || graphicNew)
            {
                ClearObjects();
                textBoxFileName.Text = "New File";
            }
            canvas.Background = Brushes.White;
            graphicNew = true;
            graphicLoaded = false;
            textBoxFileName.Text = "";
            textBoxFileName.Foreground = Brushes.Black;
            textBoxFileName.FontStyle = FontStyles.Normal;
            textBoxFileName.Text = "New File";
            EnableButtons();
            saveFile.Foreground = Brushes.White;
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XAML |*.xaml";
            dlg.Multiselect = false;  // default
            if (dlg.ShowDialog() == true)
            {
                graphicPath = dlg.FileName;

                textBoxFileName.Text = "";
                textBoxFileName.Foreground = Brushes.Black;
                textBoxFileName.FontStyle = FontStyles.Normal;
                textBoxFileName.Text = dlg.SafeFileName;

                if (graphicNew || graphicLoaded)
                {
                    ClearObjects();
                }
                canvas.Background = Brushes.White;
                graphicLoaded = true;
                graphicNew = false;
                EnableButtons();

                FileStream file = new FileStream(graphicPath, FileMode.Open);
                Canvas c = (Canvas)XamlReader.Load(file);
                while (c.Children.Count > 0)
                {
                    Shape s = (Shape)c.Children[0];
                    s.MouseLeftButtonDown += new MouseButtonEventHandler(shape_MouseLeftButtonDown);
                    s.MouseRightButtonDown += new MouseButtonEventHandler(shape_MouseRightButtonDown);
                    s.MouseMove += new MouseEventHandler(shape_MouseMove);
                    listOfObjects.Items.Add(vd.ParametersToString(s));
                    c.Children.RemoveAt(0);
                    canvas.Children.Add(s);
                }
                saveFile.Foreground = Brushes.White;
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "XAML |*.xaml";
            dlg.RestoreDirectory = true ;

            if (dlg.ShowDialog() == true)
            {
                FileStream file = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write);
                XamlWriter.Save(canvas, file);
                foreach (var shape in canvas.Children)
                    XamlWriter.Save(shape);
                file.Close();
                textBoxFileName.Text = dlg.SafeFileName;
            }
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
                NewFile_Click(sender, e);
            if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
                OpenFile_Click(sender, e);
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
                SaveFile_Click(sender, e);
            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                searchWindow = new SearchWindow();
                searchWindow.Show();
            }
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
            editButton.IsEnabled = true;
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
                case "editButton":
                    operationType = Operations.Resize;
                    break;
            }
            shapeType = Shapes.None;
        }

        //=====================================================================
        // Vector graphics
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            p1 = Mouse.GetPosition(canvas);
            shapeCreated = false;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            p2 = Mouse.GetPosition(canvas);
            textBoxCoords.Text = p2.X.ToString() + " x " + p2.Y.ToString();
            if (Mouse.LeftButton == MouseButtonState.Pressed && operationType == Operations.None)
            {
                if (!shapeCreated)
                {
                    Shape shape;
                    switch (shapeType)
                    {
                        case Shapes.Line:
                            shape = vd.CreateLine(p1, p2, 2, color2);
                            break;
                        case Shapes.Ellipse:
                            shape = vd.CreateEllipse(p1, p2, 2, color2, color);
                            break;
                        case Shapes.Rectangle:
                            shape = vd.CreateRectangle(p1, p2, 2, color2, color);
                            break;
                        case Shapes.Triangle:
                            shape = vd.CreateTriangle(p1, p2, 2, color2, color);
                            break;
                        default:
                            shape = new Line();
                            break;
                    }
                    shape.MouseLeftButtonDown += new MouseButtonEventHandler(shape_MouseLeftButtonDown);
                    shape.MouseRightButtonDown += new MouseButtonEventHandler(shape_MouseRightButtonDown);
                    shape.MouseMove += new MouseEventHandler(shape_MouseMove);
                    canvas.Children.Add(shape);
                    listOfObjects.Items.Add(vd.ParametersToString(shape));
                    clickedShape = shape;
                    shapeCreated = true;
                }
                else
                {
                    int index = canvas.Children.IndexOf(clickedShape);
                    canvas.Children[index] = vd.ResizeShape(clickedShape, p1, p2);
                    listOfObjects.Items[index] = vd.ParametersToString(clickedShape);
                }
            }
        }

        private void shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                case Operations.Resize:
                    if (!editWindow.IsVisible)
                    {
                        editWindow = new EditWindow();
                        editWindow.MyEvent += new EventHandler(childWindow_MyEvent);
                        editWindow.SetShapeData(vd.GetP1(clickedShape), vd.GetP2(clickedShape));
                        editWindow.Show();
                    }
                    else
                        editWindow.SetShapeData(vd.GetP1(clickedShape), vd.GetP2(clickedShape));
                    break;
            }
        }

        private void shape_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickedShape = (Shape)sender;
            switch (operationType)
            {
                case Operations.Paint:
                    int index = canvas.Children.IndexOf((Shape)sender);
                    ((Shape)canvas.Children[index]).Stroke = color2;
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
                if (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
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
            fillRectangle.Fill = color;
        }

        private void colorButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button senderButton = (Button)sender;
            switch (senderButton.Name)
            {
                case "whiteButton":
                    color2 = Brushes.White;
                    break;
                case "grayButton":
                    color2 = Brushes.Gray;
                    break;
                case "blackButton":
                    color2 = Brushes.Black;
                    break;
                case "redButton":
                    color2 = Brushes.Red;
                    break;
                case "greenButton":
                    color2 = Brushes.Green;
                    break;
                case "blueButton":
                    color2 = Brushes.Blue;
                    break;
            }
            strokeRectangle.Fill = color2;
        }

        void childWindow_MyEvent(object sender, EventArgs e)
        {
            try
            {
                Point p1 = new Point(Convert.ToDouble(editWindow.x1.Text), Convert.ToDouble(editWindow.y1.Text));
                Point p2 = new Point(Convert.ToDouble(editWindow.x2.Text), Convert.ToDouble(editWindow.y2.Text));
                int index = canvas.Children.IndexOf(clickedShape);
                canvas.Children[index] = vd.ResizeShape(clickedShape, p1, p2);
                listOfObjects.Items[index] = vd.ParametersToString(clickedShape);
            }
            catch (FormatException) { }
        }

        /// <summary>
        /// Shutdown on MainWindowClose
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            App.Current.Shutdown();
        }
    }
}
