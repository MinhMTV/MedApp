using App1.Helpers;
using App1.Models;
using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    class SwipeViewModel : BaseViewModel
    {
        private PictureDBHelper pictureDBHelper = new PictureDBHelper();

        private ObservableCollection<Pictures> _pictures = new ObservableCollection<Pictures>();
        private uint _threshold;
        private static bool _isDraggingUpSupported;
        private static bool _isDraggingDownSupported;
        private static bool _isSwipeUpSupported;
        private static bool _isSwipeDownSupported;
        private uint _animationLength;

        public static void SetUp()
        {
            _isDraggingUpSupported = true;
            _isSwipeUpSupported = true; ;

            _isDraggingDownSupported = false;
            _isSwipeDownSupported = false;
        }

        public static void SetDown()
        {

            _isDraggingUpSupported = false;
            _isSwipeUpSupported = false; ;

            _isDraggingDownSupported = true;
            _isSwipeDownSupported = true;
        }

        public static void SetAll()
        {

            _isDraggingUpSupported = true;
            _isSwipeUpSupported = true; ;

            _isDraggingDownSupported = true;
            _isSwipeDownSupported = true;
        }


        public bool IsDraggingUpSupported
        {
            get => _isDraggingUpSupported;
            set
            {
                _isDraggingUpSupported = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SupportedDraggingDirections));
            }
        }

        public bool IsDraggingDownSupported
        {
            get => _isDraggingDownSupported;
            set
            {
                _isDraggingDownSupported = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SupportedDraggingDirections));
            }
        }

        public SwipeCardDirection SupportedDraggingDirections => (IsDraggingUpSupported ? SwipeCardDirection.Up : SwipeCardDirection.None)
                                                              | (IsDraggingDownSupported ? SwipeCardDirection.Down : SwipeCardDirection.None);

        public bool IsSwipeUpSupported
        {
            get => _isSwipeUpSupported;
            set
            {
                _isSwipeUpSupported = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SupportedSwipeDirections));
            }
        }

        public bool IsSwipeDownSupported
        {
            get => _isSwipeDownSupported;
            set
            {
                _isSwipeDownSupported = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SupportedSwipeDirections));
            }
        }

        public SwipeCardDirection SupportedSwipeDirections => (IsSwipeUpSupported ? SwipeCardDirection.Up : SwipeCardDirection.None)
                                                              | (IsSwipeDownSupported ? SwipeCardDirection.Down : SwipeCardDirection.None);
        public uint AnimationLength
        {
            get => _animationLength;
            set
            {
                _animationLength = value;
                OnPropertyChanged();
            }
        }

        public uint Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                OnPropertyChanged();
            }
        }

        public SwipeViewModel()
        {
            _pictures = pictureDBHelper.GetAllImagesToCollection();
 //           InitializePictures();

            Threshold = (uint)(App.ScreenWidth / 3);

            SetAll();

            _threshold = 200;
            _animationLength = 400;

            SwipedCommand = new Command<SwipedCardEventArgs>(OnSwipedCommand);
            DraggingCommand = new Command<DraggingCardEventArgs>(OnDraggingCommand);
        }

        public ObservableCollection<Pictures> Pictures
        {
            get => _pictures;
            set
            {
                _pictures = value;
                OnPropertyChanged();
            }
        }

        public ICommand SwipedCommand { get; }

        public ICommand DraggingCommand { get; }


        private void OnSwipedCommand(SwipedCardEventArgs eventArgs)
        {
        }

        private void OnDraggingCommand(DraggingCardEventArgs eventArgs)
        {
            switch (eventArgs.Position)
            {
                case DraggingCardPosition.Start:
                    return;
                case DraggingCardPosition.UnderThreshold:
                    break;
                case DraggingCardPosition.OverThreshold:
                    break;
                case DraggingCardPosition.FinishedUnderThreshold:
                    return;
                case DraggingCardPosition.FinishedOverThreshold:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitializePictures()
        {
            //Hier ist die Reihenfolge der Bilder angegeben
            //Reihenfolge ist fest
            //Timer bei 5 min. , danach automatisch Intent

            //List<Pictures> newList = pictureDBHelper.GetPictures();

            List<int> randomNumbers = new List<int>();
            Random random = new Random();

            for (int i = 1; i <= GlobalVariables.NroOfAvailablePics; i++)
            {
                int number = random.Next(1, GlobalVariables.NroOfAvailablePics + 1);
                while (randomNumbers.Contains(number))
                {
                    number = random.Next(1, GlobalVariables.NroOfAvailablePics + 1);
                }

                randomNumbers.Add(number);

                if (number <= GlobalVariables.NrOfAvailableGoodPics)
                {
                    this.Pictures.Add(new Pictures { TypeId = i, Type = PicType.Good, Photo = "g" + number.ToString("00") + ".jpg" });
                }
                else
                {
                    this.Pictures.Add(new Pictures { TypeId = i, Type = PicType.Bad, Photo = "b" + (number - GlobalVariables.NrOfAvailableGoodPics).ToString("00") + ".jpg" });
                }

            }


            //for (int i = 0; i <= GlobalVariables.NroOfAvailablePics; i++)
            //{

            //    if (i <= GlobalVariables.NrOfAvailableGoodPics)
            //    {
            //        this.Pictures.Add(new Pictures { TypeId = i+1, Type = PicType.Good, Photo = "g" + (i+1).ToString("00") + ".jpg" });
            //    }
            //    else
            //    {
            //        this.Pictures.Add(new Pictures { TypeId = i+1, Type = PicType.Good, Photo = "b" + (i+1).ToString("00") + ".jpg" });
            //    }

            //}



            //this.Pictures = new ObservableCollection<Pictures> (pictureDBHelper.GetPictures());

            //this.Pictures.Add(new Pictures { TypeId = 1, Type = PicType.Good, Photo = "g01.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 2, Type = PicType.Good, Photo = "g02.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 3, Type = PicType.Good, Photo = "g03.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 4, Type = PicType.Good, Photo = "g04.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 5, Type = PicType.Good, Photo = "g05.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 6, Type = PicType.Good, Photo = "g06.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 7, Type = PicType.Good, Photo = "g07.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 8, Type = PicType.Good, Photo = "g08.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 9, Type = PicType.Good, Photo = "g09.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 10, Type = PicType.Good, Photo = "g10.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 11, Type = PicType.Good, Photo = "g11.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 12, Type = PicType.Good, Photo = "g12.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 13, Type = PicType.Good, Photo = "g13.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 14, Type = PicType.Good, Photo = "g14.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 15, Type = PicType.Good, Photo = "g15.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 16, Type = PicType.Good, Photo = "g16.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 17, Type = PicType.Good, Photo = "g17.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 18, Type = PicType.Good, Photo = "g18.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 19, Type = PicType.Good, Photo = "g19.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 20, Type = PicType.Good, Photo = "g20.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 21, Type = PicType.Good, Photo = "g21.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 22, Type = PicType.Good, Photo = "g22.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 23, Type = PicType.Good, Photo = "g23.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 24, Type = PicType.Good, Photo = "g24.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 25, Type = PicType.Good, Photo = "g25.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 26, Type = PicType.Good, Photo = "g26.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 27, Type = PicType.Good, Photo = "g27.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 28, Type = PicType.Good, Photo = "g28.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 29, Type = PicType.Good, Photo = "g29.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 30, Type = PicType.Good, Photo = "g30.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 31, Type = PicType.Good, Photo = "g31.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 32, Type = PicType.Good, Photo = "g32.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 33, Type = PicType.Good, Photo = "g33.jpg" });


            //this.Pictures.Add(new Pictures { TypeId = 34, Type = PicType.Bad, Photo = "b01.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 35, Type = PicType.Bad, Photo = "b02.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 36, Type = PicType.Bad, Photo = "b03.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 37, Type = PicType.Bad, Photo = "b04.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 38, Type = PicType.Bad, Photo = "b05.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 39, Type = PicType.Bad, Photo = "b06.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 40, Type = PicType.Bad, Photo = "b07.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 41, Type = PicType.Bad, Photo = "b08.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 42, Type = PicType.Bad, Photo = "b09.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 43, Type = PicType.Bad, Photo = "b10.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 44, Type = PicType.Bad, Photo = "b11.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 45, Type = PicType.Bad, Photo = "b12.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 46, Type = PicType.Bad, Photo = "b13.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 47, Type = PicType.Bad, Photo = "b14.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 48, Type = PicType.Bad, Photo = "b15.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 49, Type = PicType.Bad, Photo = "b16.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 50, Type = PicType.Bad, Photo = "b17.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 51, Type = PicType.Bad, Photo = "b18.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 52, Type = PicType.Bad, Photo = "b19.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 53, Type = PicType.Bad, Photo = "b20.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 53, Type = PicType.Bad, Photo = "b21.jpg" });
            //this.Pictures.Add(new Pictures { TypeId = 54, Type = PicType.Bad, Photo = "b22.jpg" });


        }
    }
}

