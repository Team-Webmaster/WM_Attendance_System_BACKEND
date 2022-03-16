using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WM_Attendance_System.Settings;

namespace WM_Attendance_System.Services
{
    public class FaceService:IFaceService
    {
        private readonly FaceAPI _faceAPI;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly string faceListId = "user_face_list";
        public FaceService(IOptions<FaceAPI> faceAPI,IWebHostEnvironment hostEnvironment)
        {
            _faceAPI = faceAPI.Value;
            this.hostEnvironment = hostEnvironment;
        }
        public IFaceClient Authenticate()
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(_faceAPI.APIKey)) { Endpoint = _faceAPI.Endpoint };
        }
        public async Task<DetectedFace> DetectFaceExtract(IFaceClient client,Stream image)
        {
            IList<DetectedFace> detectedFaces;
            image.Position = ;
            detectedFaces = await client.Face.DetectWithStreamAsync(image,detectionModel:DetectionModel.Detection03,recognitionModel:RecognitionModel.Recognition04,returnFaceAttributes:new List<FaceAttributeType> { FaceAttributeType.QualityForRecognition},faceIdTimeToLive:300);
            return detectedFaces[0];
        }
        
        public async Task<PersistedFace> AddFaceToFaceList(IFaceClient client,string imgName)
        {
            bool sufficientQuality = true;
            string imgPath = Path.Combine(hostEnvironment.ContentRootPath, "Images", imgName);
            FileStream image = File.OpenRead(imgPath);
            DetectedFace face = await DetectFaceExtract(client, image);
            var faceQualityRecognition = face.FaceAttributes.QualityForRecognition;
            if(faceQualityRecognition.HasValue && (faceQualityRecognition.Value == QualityForRecognition.Low))
            {
                sufficientQuality = false;
            }
            if (!sufficientQuality)
            {
                return null;
            }
            FileStream faceImage = File.OpenRead(imgPath);
            var result = await client.FaceList.AddFaceFromStreamAsync(faceListId:faceListId,faceImage,detectionModel:DetectionModel.Detection03);
            return result;
        }
        
        public async Task<IList<SimilarFace>> IdentifyFaceList(IFaceClient client,MemoryStream image)
        {
            DetectedFace detectedFace = await DetectFaceExtract(client, image);
            var result = await client.Face.FindSimilarAsync(detectedFace.FaceId.Value, faceListId: faceListId);
            return result;
        }
        public async Task CreateFaceList(IFaceClient client,string imgName)
        {
            await client.FaceList.CreateAsync(faceListId:faceListId,"userFaces",recognitionModel:RecognitionModel.Recognition04);
        }
    }
}
