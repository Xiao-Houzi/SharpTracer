using SharpTracer.Model.Base.Messaging;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SharpTracer.View.Controls
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class ProgressBar : UserControl
    {
        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Application.Current?.Dispatcher?.Invoke(
               () =>
               {
                   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
               });
        }
        #endregion

        #region XamlProperties
        public static readonly DependencyProperty BoxesProperty =
                 DependencyProperty.Register("Boxes", typeof(int), typeof(ProgressBar));
        public static readonly DependencyProperty TickProportionProperty =
                 DependencyProperty.Register("TickProportion", typeof(float), typeof(ProgressBar));
        public static readonly DependencyProperty MaxProperty =
                 DependencyProperty.Register("Max", typeof(float), typeof(ProgressBar));
        public static readonly DependencyProperty CurrentProperty =
                 DependencyProperty.Register("Current", typeof(float), typeof(ProgressBar), new FrameworkPropertyMetadata(OnCurrentPropertyChanged));
        public static readonly DependencyProperty CaptionProperty =
                 DependencyProperty.Register("Caption", typeof(string), typeof(ProgressBar));
        public static readonly DependencyProperty CurrentProcessProperty =
                 DependencyProperty.Register("CurrentProcess", typeof(string), typeof(ProgressBar));
        public static readonly DependencyProperty BarColourProperty =
                 DependencyProperty.Register("BarColor", typeof(SolidColorBrush), typeof(ProgressBar));
        public static readonly DependencyProperty ProgressColourProperty =
                 DependencyProperty.Register("ProgressColor", typeof(SolidColorBrush), typeof(ProgressBar));

        public int Boxes
        {
            get => _boxes; set { _boxes = value; NotifyPropertyChanged(); }
        }
        public float TickProportion
        {
            get
            {
                return _tickProportion;
            }
            set
            {
                if (value > 1) _tickProportion = 1 / value;
                else _tickProportion = value;
                NotifyPropertyChanged();
            }
        }
        public float Max
        {
            get => _max;
            set
            {
                _max = value; NotifyPropertyChanged();
            }
        }
        public float Current
        {
            get { return (float)GetValue(CurrentProperty); }
            set
            {
                SetValue(CurrentProperty, value); NotifyPropertyChanged();
                compass++;
                InvalidateVisual();
            }
        }
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set
            {
                SetValue(CaptionProperty, value); NotifyPropertyChanged();
            }
        }
        public string CurrentProcess
        {
            get => _currentProcess;
            set
            {
                _currentProcess = value;
                NotifyPropertyChanged();
            }
        }
        public SolidColorBrush BarColor { get; set; } = null;
        public SolidColorBrush ProgressColor { get; set; } = new SolidColorBrush(Colors.Lime);
        private static void OnCurrentPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ProgressBar control = source as ProgressBar;
            control.Current = (float)e.NewValue;
        }
    #endregion

    public ProgressBar()
    {
        BarColor = Brushes.DimGray;
        ProgressColor = Brushes.Yellow;

        InitializeComponent();
        InvalidateVisual();
    }
    ~ProgressBar()
    {
        _fadeOutTimer?.Dispose();
    }

    #region Private
    private string _caption = "";
    private string _currentProcess = "";
    private float _max = 100;
    private float _current = 0;
    private int _boxes = 16;
    private float _tickProportion = 0.25f;
    private int compass = 0;
    private bool active = false;

    private int _fadeDuration;
    private int _fadeCounter;
    private Timer _fadeOutTimer;
    private void FadeTick(object state)
    {
        if (_fadeCounter == _fadeDuration)
        {
            _fadeOutTimer?.Change(0, 0);
            _fadeOutTimer = null;

        }
        else
        {
            _fadeCounter++;
            Application.Current?.Dispatcher.Invoke(() =>
            {
                InvalidateVisual();
            });
        }
    }

    protected override void OnRender(DrawingContext DC)
    {
        base.OnRender(DC);
        if (active)
        {
            if (_fadeDuration != 0)
            {
                DC.PushOpacity(1 - ((double)_fadeCounter / _fadeDuration));
            }
            else { DC.PushOpacity(0); }
        }
        else { DC.PushOpacity(1); }

        DC.PushClip(new RectangleGeometry(new Rect(0, 0, (int)ActualWidth, (int)ActualHeight)));
        CultureInfo culture = CultureInfo.CurrentCulture;
        Typeface t = new Typeface("calibri");
        FormattedText formattedCaption = new FormattedText(Caption, culture, FlowDirection.LeftToRight, t, 14, Brushes.White, 1);

        FormattedText formattedItem = new FormattedText(CurrentProcess, culture, FlowDirection.LeftToRight, t, 12, Brushes.White, 1);

        Double width = ActualWidth - 4;
        Double height = 16;

        Size tickSize = new Size(width / Boxes, height * _tickProportion);

        for (int i = 0; i < Boxes; i++)
        {
            double x = 2 + (((Boxes * tickSize.Width) / (Boxes)) * i);
            Rect tickRectangle = new Rect(new Point(x, 19), tickSize);

            if (compass % Boxes == i)
                DC.DrawRectangle(ProgressColor, new Pen(ProgressColor, 1), tickRectangle);
            else
                DC.DrawRectangle(BarColor, new Pen(BarColor, 1), tickRectangle);
        }

        Size progressSize = new Size((width), height - tickSize.Height);
        Size currentSize = new Size(((width) / Max) * Current, height - tickSize.Height);
        Point progressPosition = new Point(2, tickSize.Height + 19);
        Rect progressBar = new Rect(progressPosition, progressSize);
        Rect currentProgress = new Rect(progressPosition, currentSize);
        DC.DrawRectangle(BarColor, new Pen(BarColor, 1), progressBar);
        DC.DrawRectangle(ProgressColor, new Pen(ProgressColor, 1), currentProgress);

        DC.DrawText(formattedCaption, new Point(1, 1));

        DC.DrawText(formattedItem,
            new Point(formattedItem.Width < ActualWidth ? 0 : ActualWidth - formattedItem.Width, 37));
        DC.Pop();
    }

    private void Start()
    {
        active = true;
        _fadeOutTimer = null;
    }

    private void Stop()
    {
        if (active)
        {
            Caption = Caption += " Complete";
            CurrentProcess = "";
            Current = 0;
            Max = 1;
            int fadeInterval = 100;
            _fadeOutTimer = new Timer(FadeTick, null, 0, fadeInterval);
            _fadeCounter = 0;
            _fadeDuration = 2000 / fadeInterval;
            active = false;
        }
    }
    #endregion
}
}
