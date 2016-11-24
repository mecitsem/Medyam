using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision.Contract;

namespace Medyam.Services.Interfaces
{
    public interface IVisionService
    {
        Task<AnalysisResult> UploadAndDescripteImageAsync(string imageFilePath);
        Task<AnalysisResult> DescripteUrlAsync(string imageUrl);
    }
}
