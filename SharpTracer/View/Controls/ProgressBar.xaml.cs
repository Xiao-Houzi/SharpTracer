using SharpTracer.Base.Messaging;
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

        #region Indicator
        private Double _tickProportion = 0.2;
        private int compass;
        private bool active = false;

        private Timer tick;
        private void TimerTick(object state)
        {
            Application.Current?.Dispatcher.Invoke(
               () =>
               {
                   compass += 1;
                   InvalidateVisual();
               });
        }

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

        public int Boxes
        {
            get; set;
        }
        public double TickProportion
        {
            get
            {
                return _tickProportion;
            }

            set
            {
                if (value > 1) _tickProportion = 1 / value;
                else _tickProportion = value;
            }
        }

        public SolidColorBrush BarColor { get; set; } = null;
        public SolidColorBrush ProgressColor { get; set; } = new SolidColorBrush(Colors.Lime);

        protected override void OnRender(DrawingContext DC)
        {
            base.OnRender(DC);
            if (!active)
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

            FormattedText formattedItem = new FormattedText(Item, culture, FlowDirection.LeftToRight, t, 12, Brushes.White, 1);

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
            Size currentSize = new Size((width) / Max * Current, height - tickSize.Height);
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

        public void Start()
        {
            active = true;
            tick?.Change(0, 1000 / Boxes);
            _fadeOutTimer = null;
        }

        public void Stop()
        {
            if (active)
            {
                Caption = Caption += " Complete";
                Item = "";
                Current = 0;
                Max = 1;
                int fadeInterval = 100;
                _fadeOutTimer = new Timer(FadeTick, null, 0, fadeInterval);
                _fadeCounter = 0;
                _fadeDuration = 2000 / fadeInterval;
                active = false;
                tick?.Change(0, 0);
            }

        }
        #endregion


        #region fields
        static int _id = 0;
        private UInt64 startEventID = 0;
        private string Caption = "";
        private string Item = "";
        private int _max;
        private int _current;
        #endregion

        #region Properties
        public int ID
        {
            get;
        }
        public int Max
        {
            get => _max;
            set
            {
                _max = value; NotifyPropertyChanged();
            }
        }
        public int Current
        {
            get => _current;
            set
            {
                _current = value; NotifyPropertyChanged();
            }
        }
        #endregion

        #region constructors
        public ProgressBar()
        {
            DataContext = this;
            TickProportion = 0.25;
            compass = 0;
            Boxes = 16;

            BarColor = Brushes.DimGray;

            ProgressColor = Brushes.Yellow;

            ID = _id++;
            tick = new Timer(TimerTick, this, 0, 0);
            Messenger.ProgressEvent += Messenger_ProgressEvent;
        }

        public ProgressBar(string caption, string item, int max) : this()
        {
            Max = max;
            Current = 0;
            Item = item;
            Caption = caption;
            InitializeComponent();
            InvalidateVisual();
        }

        ~ProgressBar()
        {
            tick?.Dispose();
            _fadeOutTimer?.Dispose();
        }
        #endregion

        private void Messenger_ProgressEvent(object sender, ProgressArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (args.ProgressID != ID) return;
                if (args.EventID < startEventID) return;
                Item = args.Message;
                switch (args.Status)
                {
                    case Status.Start:
                        Max = args.Max;
                        Current = 0;
                        Caption = args.Caption;
                        startEventID = args.EventID;
                        Start();
                        break;

                    case Status.Stop:
                        Stop();
                        break;

                    case Status.Increment:
                        Current++;
                        goto case Status.Update;

                    case Status.Update:
                        if (Current >= Max) goto case Status.Stop;
                        break;

                    case Status.Clear:
                        Clear();
                        break;
                }
                InvalidateVisual();
            });
        }

        private void Clear()
        {
            Caption = "";
            Item = "";
            Current = 0;
            Max = 0;
        }

        public static ProgressBar Create(string caption, string item, int max)
        {
            return new ProgressBar(caption, item, max);
        }

    }
}
