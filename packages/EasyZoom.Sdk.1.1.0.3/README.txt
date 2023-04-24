# EasyZoom .Net SDK

## Quick Start
```cs
//1 - Add the EasyZoom .Net SDK package to your project

//2 - Create EasyZoomApi with your AppAccessCode
IEasyZoomApi api = new EasyZoomApi("your AppAccessCode goes here");

//3 - Login
var authInfo = await api.Login("username", "password");

//4 - Upload and publish an image

var filePath = @"c:\bigImage.jpeg";
var fileName = "bigImage.jpeg";
var publishParams = new PublishParams("Big image", "This is my first microscopy slide");
            
using (var openedStream = File.OpenRead(filePath))
{
    var options = new UploadingImage(openedStream, fileName, publishParams);
    var publishedImage = await api.UploadAndPublish(authInfo, options, CancellationToken.None);
    var url = api.ConstructUrl(publishedImage.Image);
}

//5 - Use other methods
var userHeader = await api.GetMyUserHeader(authInfo);
var myImages = await api.GetMyImages(authInfo, 0, 100);
var myAlbums = await api.GetMyAlbums(authInfo);
var publicImages = await api.GetPublicUserImages(userHeader.UserId, 0, 100);
var categories = await api.GetCategories(authInfo);
```