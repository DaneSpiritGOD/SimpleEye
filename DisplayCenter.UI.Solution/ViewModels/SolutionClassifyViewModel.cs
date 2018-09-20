using DisplayCenter.Core;
using DisplayCenter.Core.Options;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dc = DisplayCenter.Core;

namespace DisplayCenter.UI.Solution.ViewModels
{
    public class SolutionClassifyViewModel : BindableBase
    {
        private readonly dc.Solution _sln;
        public ObservableCollection<TabItemViewModel> ClassifiedImageTable { get; }
        private readonly IImageFactory _imageFactory;
        private readonly IClassifyGroupLocator _classifyGroupLocator;

        public int CardHeight { get; }
        public int CardWidth { get; }

        public SolutionClassifyViewModel(
            dc.Solution sln,
            int cardWidth, int cardHeight,
            IImageFactory imageFactory,
            IClassifyGroupLocator classifyGroupLocator)
        {
            _sln = NamedNullException.Assert(sln, nameof(sln));
            CardHeight = cardHeight;
            CardWidth = cardWidth;

            _imageFactory = NamedNullException.Assert(imageFactory, nameof(imageFactory));
            _classifyGroupLocator = NamedNullException.Assert(classifyGroupLocator, nameof(classifyGroupLocator));

            ClassifiedImageTable = new ObservableCollection<TabItemViewModel>();
        }

        public ClassifyGroup Add(ImageDto dto, out DetailedImageModel im)
        {
            var options = getClassifyOptions(dto);
            var key = options.Display;

            var imageModel = _imageFactory.ExtractFrom(dto, options);

            addCachedCard(key, imageModel, options.MaxCacheCount);

            if (options.DisableDisplayInFront)
            {
                im = default;
            }
            else
            {
                im = imageModel;
            }

            return options;
        }

        private TabItemViewModel getTabItem(string header)
        {
            var item = ClassifiedImageTable.Where(x => StringComparer.OrdinalIgnoreCase.Equals(x.Header, header)).SingleOrDefault();
            if (item == default)
            {
                item = new TabItemViewModel()
                {
                    Header = header,
                    Content = new ObservableCollection<DetailedImageModel>()
                };
                ClassifiedImageTable.Add(item);
            }
            return item;
        }

        private ClassifyGroup getClassifyOptions(ImageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Key) || _sln.Classes == default)
            {
                return _classifyGroupLocator.DefaultGroup;
            }

            switch (dto)
            {
                case MemoryImageDto mdto when (int.TryParse(mdto.Key, out var groupId)):
                    return _classifyGroupLocator.Locate(groupId);

                case FileImageDto fdto:
                case MemoryImageDto mdto:
                    return _sln.Classes.SingleOrDefault(x => dto.Key.ToLower().Contains(x.AlterKey.ToLower())) ?? _classifyGroupLocator.DefaultGroup;
                default:
                    throw new NotSupportedException("未知数据类型!");
            }
        }

        private void addCachedCard(string key, DetailedImageModel imageModel, int maxCount)
        {
            if (maxCount <= 0)
            {
                return;
            }

            var collection = getTabItem(key).Content;
            addCachedCardCore(collection, imageModel, maxCount);
        }

        private static void addCachedCardCore(
            ObservableCollection<DetailedImageModel> collection,
            DetailedImageModel imageModel,
            int maxCount)
        {
            if (maxCount <= 0)
            {
                return;
            }

            while (collection.Count >= maxCount)
            {
                //ClassifiedImages.RemoveAt(0);
                collection.RemoveAt(collection.Count - 1);
            }

            //ClassifiedImages.Insert(ClassifiedImages.Count, imageModel);
            collection.Insert(0, imageModel);
        }

        public void ClearImage()
        {
            ClassifiedImageTable.Clear();
        }
    }
}
