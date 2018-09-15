# Octopus Labs Technical Test
Purpose : getting **frequency** of words in given website.

## Implementation Details
Application fetches the words from given url. Url validation done in front-end side. After that get count of every word and order them from most repeated to least and take first 100, and return them to client.
When client gets the response draw the cloud using a [3rd party library](https://github.com/jasondavies/d3-cloud). I can do that cloud with simple font-size trick (actually I did) but it seems too simple. 
And client send another request for same url, which is for saving the results.

In first design I've tried to save all words when fetching time but that costs too much time. Decryption and checking the availability (for deciding update/insert) caused bad experience in client's perspective. 

I've spent almost 12 hours to finish this project. 

I've faced some difficulties; 
- Azure and Google Cloud SQL connection 
- RSACryptoServiceProvider version problem, I had to find an Extension for .Net Core 2.
- I've kept public and private keys in db but private key should keep another location.

I didn't separate client and API in application, but I've implemented Security, Data, Content parts separately. I've used Dependency Injection.

I've used [Pomelo EntityFramework MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) Library for database connection.

### Source Codes
https://github.com/MesutKeklik/Octopus

### Test links
[Word Cloud Page](http://mesutkeklikoctopus.azurewebsites.net)
[Admin Page](http://mesutkeklikoctopus.azurewebsites.net/Home/Admin)

