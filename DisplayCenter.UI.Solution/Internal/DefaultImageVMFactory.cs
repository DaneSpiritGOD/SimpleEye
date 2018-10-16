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

#warning 目前对于网络流式数据，还没有很好地完善逻辑
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
#warning 针对流式数据，其包含的缺陷ROI信息需要在界面上显示出来
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
