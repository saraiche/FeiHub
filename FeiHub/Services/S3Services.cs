﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Configuration;
using System.Threading.Tasks;

public class S3Service
{
    private const string BucketName = "feihub-admin-photos-bucket"; 

    private readonly AmazonS3Client amazonS3Client;

    public S3Service()
    {
        var accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
        var secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];

        var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast2 
        };

        amazonS3Client = new AmazonS3Client(credentials, config);
    }

    public async Task<bool> UploadImage(string imagePath, string customName)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = customName,
                FilePath = imagePath
            };

            await amazonS3Client.PutObjectAsync(putRequest);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GetImageURL(string imageName)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = BucketName,
                Key = imageName,
                Expires = DateTime.Now.AddSeconds(604800)
            };

            var url = amazonS3Client.GetPreSignedURL(request);
            return url;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> OverwriteImage(string imagePath, string imageName)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = imageName,
                FilePath = imagePath
            };

            await amazonS3Client.PutObjectAsync(putRequest);

            return true; 
        }
        catch 
        {
            return false; 
        }
    }
}
