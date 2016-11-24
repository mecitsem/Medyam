using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Medyam.Core.Entities;
using Medyam.Data.Repositories;
using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medyam.Services.Tests
{
    [TestClass]
    public class PhotoServiceTest
    {
        private const string Rowkey = "photo";
        private const string Owner = "medyam";

        [TestMethod]
        public void Create_And_Get_PhotoEntity_TableRepository_ByPartitionKey()
        {
            var tableRepository = new TableRepository();
            var partitionKey = Guid.NewGuid();

            tableRepository.CreateEntity(new PhotoEntity(partitionKey.ToString(), Rowkey)
            {
                ID = partitionKey,
                Title = "Test",
                Owner = Owner
            });

            var photoEntity = tableRepository.GetEntity(partitionKey.ToString(), Rowkey);

            Assert.AreEqual("Test", photoEntity.Title);

        }

        [TestMethod]
        public void Upload_And_Download_Photo()
        {
            var blobRepository = new BlobRepository();
            const string fileName = @"Koala.jpg";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Images\\{fileName}");

            if (!File.Exists(filePath))
                throw new ArgumentNullException(nameof(filePath));
            var partitionKey = Guid.NewGuid();
            var entity = new PhotoEntity(partitionKey.ToString(), Rowkey)
            {
                ID = partitionKey,
                Title = "Koala",
                Owner = Owner,
            };

            string photoUrl;

            using (Stream file = File.OpenRead(filePath))
            {
                photoUrl = blobRepository.UploadBlob(file, entity);
            }

            Assert.IsNotNull(photoUrl);
            Assert.AreEqual($"https://medya.blob.core.windows.net:443/images/photo-{entity.ID}.jpg", photoUrl);
        }

        [TestMethod]
        public void Create_PhotoEntity_And_Upload_Photo()
        {
            var tableRepository = new TableRepository();
            var blobRepository = new BlobRepository();

            //Photo
            const string fileName = @"Penguins.jpg";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Images\\{fileName}");

            if (!File.Exists(filePath))
                throw new ArgumentNullException(nameof(filePath));

            //Photo Entity
            var partitionKey = Guid.NewGuid();
            var entity = new PhotoEntity(partitionKey.ToString(), Rowkey)
            {
                ID = partitionKey,
                Title = "Penguins",
                Owner = Owner,
            };

            string photoUrl;

            using (Stream file = File.OpenRead(filePath))
            {
                photoUrl = blobRepository.UploadBlob(file, entity);
            }

            Assert.IsNotNull(photoUrl);
            Assert.AreEqual($"https://medya.blob.core.windows.net:443/images/photo-{entity.ID}.jpg", photoUrl);

            entity.Url = photoUrl;
            tableRepository.CreateEntity(entity);

            var photoEntity = tableRepository.GetEntity(partitionKey.ToString(), Rowkey);

            Assert.AreEqual("Penguins", photoEntity.Title);
            Assert.AreEqual(photoUrl, photoEntity.Url);
        }

        [TestMethod]
        public void Test_PhotoService()
        {
            var tableRepository = new TableRepository();
            var blobRepository = new BlobRepository();
            var visionService = new VisionService();
            var photoService = new PhotoService(tableRepository, blobRepository, visionService);

            //Photo
            const string fileName = @"Desert.jpg";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Images\\{fileName}");

            if (!File.Exists(filePath))
                throw new ArgumentNullException(nameof(filePath));

            //Photo Entity
            var partitionKey = Guid.NewGuid();
            var entity = new PhotoEntity(partitionKey.ToString(), Rowkey)
            {
                ID = partitionKey,
                Title = "Desert",
                Owner = Owner,
            };

            using (Stream file = File.OpenRead(filePath))
            {
                photoService.Create(entity, file);
            }

            var photos = photoService.GetAll();
            Assert.IsNotNull(photos);
            Assert.IsTrue(photos.Count > 0);
            var singleOrDefault = photos.SingleOrDefault(p => p.ID.Equals(entity.ID));
            Assert.IsNotNull(singleOrDefault);
            Assert.AreEqual("Desert", singleOrDefault.Title);
        }

        [TestMethod]
        public void Get_All_Photos_Count()
        {
            var tableRepository = new TableRepository();
            var blobRepository = new BlobRepository();
            var visionService = new VisionService();
            var photoService = new PhotoService(tableRepository, blobRepository, visionService);

            var photos = photoService.GetAll();

            Assert.IsTrue(photos.Count > 0);
        }

        [TestMethod]
        public void Test_VisionService()
        {
            var visionService = new VisionService();
            const string photoUrl = @"https://medya.blob.core.windows.net:443/images/photo-c31a565a-b380-496f-8c17-8548c86509bc.jpg";

            var taskVision = visionService.DescripteUrlAsync(photoUrl);
            taskVision.Wait();
            var result = taskVision.Result;
            Assert.IsNotNull(result.Description);
            Assert.IsTrue(result.Description.Captions.Any());
            Assert.IsTrue(result.Description.Tags.Any());
        }

    }
}
