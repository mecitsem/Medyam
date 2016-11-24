using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medyam.Core.Common;
using Medyam.Core.Helpers;
using Medyam.Services.Interfaces;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace Medyam.Services
{
    public class VisionService : IVisionService
    {
        private readonly VisionServiceClient _visionServiceClient;
        public VisionService()
        {
            var subKey = CloudConfigurationManager.GetSetting(Constants.AppSettings.SubscriptionKey);
            if (string.IsNullOrWhiteSpace(subKey))
                throw new ArgumentNullException(nameof(subKey));

            _visionServiceClient = new VisionServiceClient(subKey);
        }

        public async Task<AnalysisResult> UploadAndDescripteImageAsync(string imageFilePath)
        {
            using (Stream imageFileStriStream = File.OpenRead(imageFilePath))
            {
                // Upload and image and request three descriptions
                var analysisResult = await _visionServiceClient.DescribeAsync(imageFileStriStream, 3);
                return analysisResult;
            }
        }

        public async Task<AnalysisResult> DescripteUrlAsync(string imageUrl)
        {
            var analysisResult = await _visionServiceClient.DescribeAsync(imageUrl, 3);
            return analysisResult;
        }

        
    }
}
