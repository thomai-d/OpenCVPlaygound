﻿using ActionsModule.Events;
using OpenCvSharp;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp.Extensions;
using MenuModule.Events;
using Prism.Mvvm;

namespace ImageModule.ViewModels
{
    public class ImageViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        public ImageViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<OperateOnImageEvent>().Subscribe((actions) =>
            {
                OperatedImage = CurrentImage.Clone();
                foreach (var act in actions)
                {
                    OperatedImage = act.Action(OperatedImage);
                    act.HasChanged = false;
                }
                Image = OperatedImage.ToBitmapSource();
            });

            _ea.GetEvent<OpenImageEvent>().Subscribe((filename) =>
            {
                CurrentImage = Cv2.ImRead(filename);
                OperatedImage = CurrentImage.Clone();
                Image = OperatedImage.ToBitmapSource();
            });
        }

        public Mat CurrentImage { get; set; }
        public Mat OperatedImage { get; set; }

        private BitmapSource image = null;
        public BitmapSource Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }
    }
}
