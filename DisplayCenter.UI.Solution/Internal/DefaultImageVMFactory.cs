using DisplayCenter.Core;
using DisplayCenter.Core.Options;
using DisplayCenter.UI.Solution.ViewModels;
using Microsoft.Extensions.Options;
using Suites.Wpf.MaterialDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DisplayCenter.UI.Solution.Internal
{
    public class DefaultImageVMFactory : IImageFactory
    {
        private readonly IClassifyGroupLocator _classifyGroupLocator;
        private readonly bool _showRoiDescription;

        public DefaultImageVMFactory(
            IClassifyGroupLocator classifyGroupLocator,
            IOptions<SolutionCardViewOptions> solutionCardViewOptions)
        {
            _classifyGroupLocator = NamedNullException.Assert(classifyGroupLocator, nameof(classifyGroupLocator));
            _showRoiDescription = NamedNullException.Assert(solutionCardViewOptions?.Value, nameof(solutionCardViewOptions)).ShowRoiDescription;
        }

        public DetailedImageModel ExtractFrom(ImageDto dto, ClassifyGroup group)
        {
            NamedNullException.Assert(dto, nameof(dto));

            var (colorName, type) = extractDecorator(dto, group);
            return new DetailedImageModel(
                dto.GetImage(), dto.Time,
                extractDescription(dto, group),
                colorName, type);
        }

        private string extractDescription(ImageDto dto, ClassifyGroup group)
        {
            switch (dto)
            {
                case FileImageDto fdto:
                    return fdto.Key;
                case MemoryImageDto mdto:
                {
                    var sb = new StringBuilder();
                    sb.Append($"{group.Display};");
                    sb.Append($"{_classifyGroupLocator.Locate(mdto.ErrorId).Display}");

                    if (_showRoiDescription && mdto.Rois != default && mdto.Rois.Any())
                    {
                        sb.Append("(");
                        foreach (var roi in mdto.Rois)
                        {
                            sb.Append($"{_classifyGroupLocator.Locate(roi.ErrorId).Display} ");
                        }
                        sb.Append(")");
                    }
                    return sb.ToString();
                }
                default:
                    throw new NotSupportedException("未知数据类型!");
            }
        }

        private (string ColorName, string type) extractDecorator(ImageDto dto, ClassifyGroup group)
        {
            var errId = 0;
            switch (dto)
            {
                case FileImageDto fdto:
                    errId = group.Id;
                    break;
                case MemoryImageDto mdto:
                    errId = mdto.ErrorId;
                    break;
                default:
                    throw new NotSupportedException("未知数据类型!");
            }

            var error = _classifyGroupLocator.Locate(errId);
            return (error.Color, error.Display);
        }
    }
}
