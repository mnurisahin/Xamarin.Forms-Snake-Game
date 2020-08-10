using System;
using System.Collections.Generic;
using System.ComponentModel;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SnakeApp
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private int score;
        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Init();
        }

        int positionX, positionY;
        int appleX, appleY;
        int tailSize;
        List<Trail> trail;
        int gridSize, tileCount = 20;
        int velocityX, velocityY;
        bool continueTimer;

        private void Init()
        {
            Score = 0;
            positionX = 10;
            positionY = 10;
            appleX = 5;
            appleY = 5;
            tailSize = 5;
            trail = new List<Trail>();
            gridSize = 20;
            tileCount = 20;
            velocityX = 0;
            velocityY = 0;
            continueTimer = true;

            Device.StartTimer(TimeSpan.FromMilliseconds(1000), TimerElapsed);
        }

        private bool TimerElapsed()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //put here your code which updates the view
                Update();
                Draw();
            });
            //return true to keep timer reccurring
            //return false to stop timer
            return continueTimer;
        }

        private void Update()
        {
            positionX += velocityX;
            positionY += velocityY;

            // duvara carpti mi
            if (positionX < 0)
                positionX = (tileCount - 1);
            if (positionY < 0)
                positionY = (tileCount - 1);
            if (positionX > (tileCount - 1))
                positionX = 0;
            if (positionY > (tileCount - 1))
                positionY = 0;

            //kendi uzerine basti mi
            foreach (var t in trail)
            {
                if (positionX == t.PositionX && positionY == t.PositionY)
                    Reset();
            }

            // hareket ettirme
            trail.Add(new Trail(positionX, positionY));

            while(trail.Count > tailSize)
            {
                trail.RemoveAt(0);
            }

            // elma uzerine basti mi kontrolu
            if(appleX ==positionX && appleY == positionY)
            {
                tailSize++;
                appleX = RandomNumber(0, tileCount);
                appleY = RandomNumber(0, tileCount);
                Score++;
            }
        }

        private void Draw()
        {
            CanvasView.InvalidateSurface();
        }

        private void Reset()
        {
            continueTimer = false;
            Init();
        }

        void CanvasView_PaintSurface(System.Object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            var paintYellow = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Yellow.ToSKColor(),
            };

            foreach (var t in trail)
            {
                canvas.DrawRect(t.PositionX * gridSize, t.PositionY * gridSize, gridSize - 5, gridSize - 5, paintYellow);
            }

            var paintPink = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Pink.ToSKColor(),
            };

            canvas.DrawRect(appleX * gridSize, appleY * gridSize, gridSize - 5, gridSize - 5, paintYellow);
            
        }

        void leftSwipe_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            Left();
        }

        void rightSwipe_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            Right();
        }

        void upSwipte_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            Up();
        }

        void downSwipe_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            Down();
        }

        // Instantiate random number generator.  
        private readonly Random _random = new Random();

        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        void buttonUp_Clicked(System.Object sender, System.EventArgs e)
        {
            Up();
        }

        void buttonDown_Clicked(System.Object sender, System.EventArgs e)
        {
            Down();
        }

        void buttonLeft_Clicked(System.Object sender, System.EventArgs e)
        {
            Left();
        }

        void buttonRight_Clicked(System.Object sender, System.EventArgs e)
        {
            Right();
        }

        private void Left()
        {
            if (velocityX != 1)
            {
                this.velocityX = -1;
                this.velocityY = 0;
            }
        }

        private void Right()
        {
            if (velocityX != -1)
            {
                this.velocityX = 1;
                this.velocityY = 0;
            }
        }

        private void Up()
        {
            if (velocityY != 1)
            {
                this.velocityX = 0;
                this.velocityY = -1;
            }
        }

        private void Down()
        {
            if (velocityX != -1)
            {
                this.velocityX = 0;
                this.velocityY = 1;
            }
        }
    }

    public class Trail
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public Trail(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }
    }
}