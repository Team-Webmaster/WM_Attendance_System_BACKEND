using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WM_Attendance_System.Services
{
    public interface IFaceService
    {
        IFaceClient Authenticate();
        Task<DetectedFace> DetectFaceExtract(IFaceClient client,Stream image);
        Task<PersistedFace> AddFaceToFaceList(IFaceClient client, string imgPath);
        Task CreateFaceList(IFaceClient client, string imgName);
        Task<IList<SimilarFace>> IdentifyFaceList(IFaceClient client, MemoryStream image);
    }
}
