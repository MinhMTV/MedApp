using App1.Helpers;
using App1.Models;
using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using App1.Extensions;
using App1.Methods;

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
            var tempPicsList = pictureDBHelper.GetAllImagesToList();
            tempPicsList.Shuffle();
            _pictures = pictureDBHelper.GetAllImagesByListToCollection(tempPicsList);

            DeviceMetricHelper dv = new DeviceMetricHelper();

            Threshold = (uint)(dv.getWidth() / 3);

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
    }
}

