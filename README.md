# Octopus Labs Technical Test
Purpose : getting **frequency** of words in given website.

## Implementation Details
Application fetches the words from given url. Url validation done in front-end side. After that get count of every word and order them from most repeated to least and take first 100, and return them to client.
When client gets the response draw the cloud using a [3rd party library](https://github.com/jasondavies/d3-cloud). I can do that cloud with simple font-size trick (actually I did) but it seems too simple. 
And client send another request for same url, which is for saving the results.

In first design I've tried to save all words when fetching time but that costs too much time. Decryption and checking the availability (for deciding update/insert) caused bad experience in client's perspective. 

I've spent almost 14 hours to finish this project. 

I've faced some difficulties; 
- Azure and Google Cloud SQL connection 
- RSACryptoServiceProvider version problem, I had to find an Extension for .Net Core 2.

I've kept both public and private keys in db, but private key should be kept in another location. Public key location can be anywhere, but private key's location should be a cold wallet or most importantly it should be offline somewhere.

I didn't separate client and API in application, but I've implemented Security, Data, Content parts separately. I've used Dependency Injection.

I've used [Pomelo EntityFramework MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) Library for database connection.

Also I've used built-in Memory Cache for performance.

### Source Codes
https://github.com/MesutKeklik/Octopus

### Test links
[Word Cloud Page](http://mesutkeklikoctopus.azurewebsites.net)

[Admin Page](http://mesutkeklikoctopus.azurewebsites.net/Home/Admin)

### Notes
In this project there are some design flaws; 
- I should split API, from Web Site Application. HomeController has more meaning than expected, I should done it more simple and less business inside it. 
- DB ConnectionString should getting from config file, but it is hardcoded now. I get some problems to get it from config file, and I've decided to keep it hardcoded (because of deadline).
- WebPageContentService just promise the download site from given url, but it also should do counting word task. So, that brings another flaw WebPageContent object implements counting word task. If I had more time I'd fix it.

Another point; I think it is not a flaw but I should write something about it;
- I couldn't bind the Entity Framework properly and I had to use another library (that is still entity framework but specialized, I've mentioned above) community used and almost 820k downloaded in nuget. 

I believe that's all.

Thanks for this opportunity.
