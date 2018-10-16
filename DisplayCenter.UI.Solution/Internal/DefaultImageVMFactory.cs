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
        private readonly bool _showRoiDescription;

        public DefaultImageVMFactory(IOptions<SolutionCardViewOptions> solutionCardViewOptions)
        {
            _showRoiDescription = NamedNullException.Assert(solutionCardViewOptions?.Value, nameof(solutionCardViewOptions)).ShowRoiDescription;
        }

        public DetailedImageModel ExtractFrom(ImageDto dto, ClassifyGroup group)
        {
            NamedNullException.Assert(dto, nameof(dto));

            return new DetailedImageModel(
                dto.GetImage(), dto.Time,
                extractDescription(dto, group),
                group.Color, group.Display);
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

                    if (_showRoiDescription && mdto.Rois != default && mdto.Rois.Any())
                    {
                        sb.Append("(");
                        foreach (var roi in mdto.Rois)
                        {
                        }
                        sb.Append(")");
                    }
                    return sb.ToString();
                }
                default:
                    throw new NotSupportedException("未知数据类型!");
            }
        }
    }
}
